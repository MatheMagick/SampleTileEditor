using Newtonsoft.Json;

namespace GameLogic.Data
{
    public sealed class Cell
    {
        public int TileX { get; set; }
        public int TileY { get; set; }
        public byte TemplateIndex { get; set; }
    }
}