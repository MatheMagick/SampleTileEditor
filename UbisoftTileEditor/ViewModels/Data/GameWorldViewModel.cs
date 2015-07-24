using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using GameLogic.Data;

namespace UbisoftTileEditor.ViewModels.Data
{
    public sealed class GameWorldViewModel : NotifyPropertyChangedBase
    {
        public GameWorldViewModel(GameWorld originalWorld)
        {
            _worldSize = new WorldSizeViewModel(originalWorld.WorldSize);
            this.DefaultCellTemplateIndex = originalWorld.DefaultCell == null ? (byte)0 : originalWorld.DefaultCell.TemplateIndex;
            this.Templates = originalWorld.Templates;

            var cells = new ObservableCollection<CellViewModel>();

            foreach (var cell in originalWorld.Cells)
            {
                cells.Add(new CellViewModel(cell));
            }

            this.Cells = cells;

            var gameObjects = new ObservableCollection<GameObjectViewModel>();

            foreach (var gameObject in originalWorld.GameObjects)
            {
                gameObjects.Add(new GameObjectViewModel(gameObject));
            }

            this.GameObjects = gameObjects;
        }

        public WorldSizeViewModel WorldSize
        {
            get { return _worldSize; }
            set
            {
                if (Equals(value, _worldSize)) return;
                _worldSize = value;
                OnPropertyChanged();
            }
        }

        public Template[] Templates
        {
            get { return _templates; }
            set { _templates = value; }
        }

        public byte DefaultCellTemplateIndex
        {
            get { return _defaultCell; }
            set
            {
                if (value == _defaultCell) return;
                _defaultCell = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<CellViewModel> Cells
        {
            get { return _cells; }
            set
            {
                if (value != null)
                {
                    value.CollectionChanged -= Cells_CollectionChanged;
                }

                _cells = value;

                if (_cells != null)
                {
                    _cells.CollectionChanged += Cells_CollectionChanged;
                }
            }
        }

        private void Cells_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.OnPropertyChanged("");
        }

        public ObservableCollection<GameObjectViewModel> GameObjects
        {
            get { return _gameObjects; }
            private set
            {
                if (Equals(value, _gameObjects)) return;
                _gameObjects = value;
                OnPropertyChanged();
            }
        }

        public CellViewModel this[int x, int y]
        {
            get
            {
                var cell = this.Cells.FirstOrDefault(c => c.TileX == x && c.TileY == y);

                if (cell == null)
                {
                    cell = new CellViewModel(new Cell(){ TileX = x, TileY = y, TemplateIndex = this.DefaultCellTemplateIndex });
                }

                return cell;
            }
        }

        private ObservableCollection<CellViewModel> _cells;
        private byte _defaultCell;
        private Template[] _templates;
        private WorldSizeViewModel _worldSize;
        private ObservableCollection<GameObjectViewModel> _gameObjects;

        public GameWorld ToModel()
        {
            throw new System.NotImplementedException();
        }
    }
}