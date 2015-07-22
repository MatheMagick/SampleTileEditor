using System.Collections.Generic;

namespace GameLogic.Data
{
    public sealed class GameWorld
    {
        public WorldSize WorldSize
        {
            get { return _worldSize; }
            set { _worldSize = value; }
        }

        public Template[] Templates
        {
            get { return _templates; }
            set { _templates = value; }
        }

        public DefaultCell DefaultCell
        {
            get { return _defaultCell; }
            set { _defaultCell = value; }
        }

        public List<Cell> Cells
        {
            get { return _cells; }
            set { _cells = value; }
        }

        public List<GameObject> GameObjects
        {
            get { return _gameObjects; }
            private set { _gameObjects = value; }
        }

        private List<Cell> _cells = new List<Cell>();
        private DefaultCell _defaultCell = new DefaultCell();
        private Template[] _templates = { };
        private WorldSize _worldSize = new WorldSize();
        private List<GameObject> _gameObjects = new List<GameObject>();
    }

    public sealed class GameObject
    {
        public int X { get; set; }
        public int Y { get; set; }
        public byte TemplateIndex { get; set; }
    }
}
