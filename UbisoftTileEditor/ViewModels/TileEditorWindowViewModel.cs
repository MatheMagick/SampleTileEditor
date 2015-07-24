using System.IO;
using GameLogic.Data;
using Newtonsoft.Json;

namespace UbisoftTileEditor.ViewModels
{
    public sealed class TileEditorWindowViewModel
    {
        public TileEditorWindowViewModel()
        {
            var gameWorld = JsonConvert.DeserializeObject<GameWorld>(File.ReadAllText("level.json"));
            this.GameWorld = gameWorld;
        }

        public GameWorld GameWorld { get; set; }
    }
}
