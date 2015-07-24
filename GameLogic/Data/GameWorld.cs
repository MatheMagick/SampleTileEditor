using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Media.Imaging;

namespace GameLogic.Data
{
    public sealed class GameWorld : INotifyPropertyChanged
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

        public ObservableCollection<Cell> Cells
        {
            get { return _cells; }
            set
            {
                if (value != null)
                {
                    value.CollectionChanged -= _cells_CollectionChanged;
                }

                _cells = value;

                if (_cells != null)
                {
                    _cells.CollectionChanged += _cells_CollectionChanged;
                }
            }
        }

        void _cells_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            this.OnPropertyChanged("[,]");
        }

        public List<GameObject> GameObjects
        {
            get { return _gameObjects; }
            private set { _gameObjects = value; }
        }

        public Cell this[int x, int y]
        {
            get
            {
                var cell = this.Cells.FirstOrDefault(c => c.TileX == x && c.TileY == y);

                //if (cell == null)
                //{
                //    cell = new Cell(){TileX = x, TileY = y, TemplateIndex = this.DefaultCell.TemplateIndex};
                //}

                return cell;
            }
        }

        private ObservableCollection<Cell> _cells = new ObservableCollection<Cell>();
        private DefaultCell _defaultCell = new DefaultCell();
        private Template[] _templates = { };
        private WorldSize _worldSize = new WorldSize();
        private List<GameObject> _gameObjects = new List<GameObject>();
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}