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

        public TileEditorWindowViewModel()
        {
            // TODO remove
            var gameWorld = JsonConvert.DeserializeObject<GameWorld>(File.ReadAllText("level.json"));
            this.GameWorld = new GameWorldViewModel(gameWorld);

            this.NewCommand = new RelayCommand(x => NewGameWorld());
            this.OpenCommand = new RelayCommand(x => OpenGameWorld());
            this.SaveCommand = new RelayCommand(x => SaveCurrentGameWorld(), x => _filePath != null);
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

        private void NewGameWorld()
        {
            GameWorld newModel = GameWorldFactory.CreateDefaultUbisoftGameWorld();
            this.GameWorld = new GameWorldViewModel(newModel);
        }

        private void OpenGameWorld()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.DefaultExt = ".json";
            dialog.Filter = "JSON files(*.json)|*.json";

            var result = dialog.ShowDialog();

            if (result.HasValue && result.Value)
            {
                try
                {
                    GameWorld newModel = GameWorldFactory.OpenWorld(dialog.FileName);
                    this.GameWorld = new GameWorldViewModel(newModel);
                    _filePath = dialog.FileName;

                    if (_timer != null)
                    {
                        _timer.Dispose();
                    }

                    _timer = new Timer(x => this.SaveCurrentGameWorld(), null, 60000, 60000);
                }
                catch (JsonReaderException)
                {
                    MessageBox.Show("The file is either corrupted or not a game file.", "Parse error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void SaveCurrentGameWorld()
        {
            // Get snapshot of current data
            GameWorld world = this.GameWorld.ToModel();

            Task.Run(() => GameWorldFactory.SaveWorld(world, _filePath));
        }
    }
}