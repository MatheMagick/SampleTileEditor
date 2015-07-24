using GameLogic.Data;

namespace UbisoftTileEditor.ViewModels.Data
{
    public sealed class WorldSizeViewModel : NotifyPropertyChangedBase
    {
        private byte _tileWidth;
        private byte _tileHeight;
        private byte _widthInTiles;
        private byte _heightInTiles;

        public WorldSizeViewModel(WorldSize worldSize)
        {
            this.TileHeight = worldSize.TileHeight;
            this.TileWidth = worldSize.TileWidth;
            this.HeightInTiles = worldSize.HeightInTiles;
            this.WidthInTiles = worldSize.WidthInTiles;
        }

        public byte TileWidth
        {
            get { return _tileWidth; }
            set
            {
                _tileWidth = value;
                this.OnPropertyChanged();
            }
        }

        public byte TileHeight
        {
            get { return _tileHeight; }
            set
            {
                _tileHeight = value;
                this.OnPropertyChanged();
            }
        }

        public byte WidthInTiles
        {
            get { return _widthInTiles; }
            set
            {
                _widthInTiles = value;
                this.OnPropertyChanged();
            }
        }

        public byte HeightInTiles
        {
            get { return _heightInTiles; }
            set
            {
                _heightInTiles = value;
                this.OnPropertyChanged();
            }
        }
    }
}