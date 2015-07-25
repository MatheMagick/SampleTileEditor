using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using GameLogic.Data;
using Microsoft.Win32;
using Newtonsoft.Json;
using UbisoftTileEditor.Helpers;
using UbisoftTileEditor.ViewModels.Data;

namespace UbisoftTileEditor.ViewModels
{
    public sealed class TileEditorWindowViewModel : NotifyPropertyChangedBase
    {
        private string _filePath;
        private Timer _timer;
        private GameWorldViewModel _gameWorld;
        private readonly object _lock = new object();
        private string _titleFileName = "No file loaded";

        public TileEditorWindowViewModel()
        {
            // TODO remove
            //var gameWorld = JsonConvert.DeserializeObject<GameWorld>(File.ReadAllText("level.json"));
            //this.GameWorld = new GameWorldViewModel(gameWorld);

            this.NewCommand = new RelayCommand(x => NewGameWorld());
            this.OpenCommand = new RelayCommand(x => OpenGameWorld());
            this.SaveCommand = new RelayCommand(x => SaveCurrentGameWorld(), x => this.GameWorld != null);
        }

        public RelayCommand OpenCommand { get; private set; }
        public RelayCommand NewCommand { get; private set; }
        public RelayCommand SaveCommand { get; private set; }

        public GameWorldViewModel GameWorld
        {
            get { return _gameWorld; }
            set
            {
                if (Equals(value, _gameWorld)) return;
                _gameWorld = value;
                OnPropertyChanged();
            }
        }

        public string TitleFileName
        {
            get { return _titleFileName; }
            set
            {
                if (value == _titleFileName) return;
                _titleFileName = value;
                OnPropertyChanged();
            }
        }

        private void NewGameWorld()
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.DefaultExt = ".json";
            dialog.Filter = "JSON files(*.json)|*.json";

            var result = dialog.ShowDialog();

            if (result.HasValue && result.Value)
            {
                GameWorld newModel = GameWorldFactory.CreateDefaultUbisoftGameWorld();

                ChangeGameWorld(dialog.FileName, newModel);

                this.SaveCurrentGameWorld();
            }
        }

        private void OpenGameWorld()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.DefaultExt = ".json";
            dialog.Filter = "JSON files(*.json)|*.json";

            var result = dialog.ShowDialog();

            if (result.HasValue && result.Value)
            {
                OpenGameWorldFile(dialog.FileName);
            }
        }

        private void OpenGameWorldFile(string fileName)
        {
            try
            {
                GameWorld newModel = GameWorldFactory.OpenWorld(fileName);

                ChangeGameWorld(fileName, newModel);
            }
            catch (JsonReaderException)
            {
                MessageBox.Show("The file is either corrupted or not a game file.", "Parse error", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void ChangeGameWorld(string fileName, GameWorld newModel)
        {
            lock (_lock)
            {
                this.GameWorld = new GameWorldViewModel(newModel);
                _filePath = fileName;
                this.TitleFileName = Path.GetFileName(_filePath);

                ResetSaveTimer();
            }
        }

        private void SaveCurrentGameWorld()
        {
            // Get snapshot of current data
            GameWorld world = this.GameWorld.ToModel();

            Task.Run(() => GameWorldFactory.SaveWorld(world, _filePath));

            ResetSaveTimer();
        }

        private void ResetSaveTimer()
        {
            if (_timer != null)
            {
                _timer.Dispose();
            }

            _timer = new Timer(x => this.SaveCurrentGameWorld(), null, 60000, 60000);
        }
    }
}