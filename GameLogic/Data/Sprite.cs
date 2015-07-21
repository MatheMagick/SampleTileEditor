
namespace GameLogic
{
    public sealed class Sprite
    {
        public int FramesPerSecond { get; set; }
        public int Looped { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int NumberOfFrames { get; set; }
        public string TexturePath { get; set; }
        public string Name { get; set; }
    }
}