using GameLogic.Data;

namespace UbisoftTileEditor.ViewModels.Data
{
    public sealed class GameObjectViewModel : NotifyPropertyChangedBase
    {
        private int _x;
        private int _y;
        private byte _templateIndex;

        public GameObjectViewModel(GameObject gameObject)
        {
            this.X = gameObject.X;
            this.Y = gameObject.Y;
            this.TemplateIndex = gameObject.TemplateIndex;
        }

        public int X
        {
            get { return _x; }
            set
            {
                if (value == _x) return;
                _x = value;
                OnPropertyChanged();
            }
        }

        public int Y
        {
            get { return _y; }
            set
            {
                if (value == _y) return;
                _y = value;
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

        public GameObject ToModel()
        {
            return new GameObject() {TemplateIndex = this.TemplateIndex, X = this.X, Y = this.Y};
        }
    }
}