using GameLogic.Data;

namespace UbisoftTileEditor.ViewModels.Data
{
    public sealed class CellViewModel : NotifyPropertyChangedBase
    {
        private int _tileX;
        private int _tileY;
        private byte _templateIndex;

        public CellViewModel(Cell cell)
        {
            this.TileX = cell.TileX;
            this.TileY = cell.TileY;
            this.TemplateIndex = cell.TemplateIndex;
        }

        public int TileX
        {
            get { return _tileX; }
            set
            {
                if (value == _tileX) return;
                _tileX = value;
                OnPropertyChanged();
            }
        }

        public int TileY
        {
            get { return _tileY; }
            set
            {
                if (value == _tileY) return;
                _tileY = value;
                OnPropertyChanged();
            }
        }

        public byte TemplateIndex
        {
            get { return _templateIndex; }
            set
            {
                if (value == _templateIndex) return;
                _templateIndex = value;
                OnPropertyChanged();
            }
        }
    }
}