using System.IO;
using System.Threading.Tasks;
using GameLogic.Data;
using Newtonsoft.Json;
using UbisoftTileEditor.ViewModels.Data;

namespace UbisoftTileEditor.ViewModels
{
    public sealed class TileEditorWindowViewModel : NotifyPropertyChangedBase
    {
        private string _filePath;

        public TileEditorWindowViewModel()
        {
            var gameWorld = JsonConvert.DeserializeObject<GameWorld>(File.ReadAllText("level.json"));
            this.GameWorld = new GameWorldViewModel(gameWorld);
            this.SaveCommand = new RelayCommand(x => SaveCurrentGameWorld(), x => _filePath != null);
        }

        private void SaveCurrentGameWorld()
        {
            // Get snapshot of data
            GameWorld world = this.GameWorld.ToModel();

            Task.Run(() => GameWorldFactory.SaveWorld(world, _filePath));
        }

        public RelayCommand SaveCommand { get; private set; }
        public GameWorldViewModel GameWorld { get; set; }
    }
}
