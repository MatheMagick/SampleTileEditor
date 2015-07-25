namespace GameLogic.Data
{
    public sealed class GameWorld
    {
        public WorldSize WorldSize { get; set; }
        public Template[] Templates { get; set; }
        public DefaultCell DefaultCell { get; set; }
        public Cell[] Cells { get; set; }
        public GameObject[] GameObjects { get; set; }
    }
}