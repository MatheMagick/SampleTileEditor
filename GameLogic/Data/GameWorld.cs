namespace GameLogic.Data
{
    class GameWorld
    {
        public WorldSize WorldSize { get; set; }
        public Template[] Templates { get; set; }
        public DefaultCell DefaultCell{ get; set; }
    }
}
