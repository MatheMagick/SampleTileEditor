namespace GameLogic.Data
{
    public sealed class Component
    {
        public string ComponentType { get; set; }
        public double AnchorX { get; set; }
        public double AnchorY { get; set; }
        public byte Alpha { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public Sprite[] Sprites { get; set; }
    }
}
