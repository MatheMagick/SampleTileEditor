using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using GameLogic.Data;

namespace UbisoftTileEditor.ViewModels.Data
{
    public sealed class GameWorldViewModel : NotifyPropertyChangedBase
    {
        private ObservableCollection<CellViewModel> _cells;
        private byte _defaultCell;
        private WorldSizeViewModel _worldSize;
        private ObservableCollection<GameObjectViewModel> _gameObjects;
        private byte _selectedTemplateIndex;
        private bool _isInAddMode = true;

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

        public Template[] Templates { get; private set; }

        public IEnumerable<Template> TileTemplates
        {
            get { return Templates.Take(4); }
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

        public byte SelectedTemplateIndex
        {
            get { return _selectedTemplateIndex; }
            set
            {
                if (value == _selectedTemplateIndex) return;
                _selectedTemplateIndex = value;
                OnPropertyChanged();
            }
        }

        public bool IsInAddMode
        {
            get { return _isInAddMode; }
            set
            {
                if (value.Equals(_isInAddMode)) return;
                _isInAddMode = value;
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

        public GameWorld ToModel()
        {
            GameWorld result = new GameWorld();

            result.Templates = this.Templates;
            result.DefaultCell = new DefaultCell(){TemplateIndex = this.DefaultCellTemplateIndex};
            result.GameObjects = this.GameObjects.Select(x => x.ToModel()).ToArray();
            result.Cells = this.Cells.Where(x => x.TemplateIndex != this.DefaultCellTemplateIndex).Select(x => x.ToModel()).ToArray();
            result.WorldSize = this.WorldSize.ToModel();

            return result;
        }
    }
}