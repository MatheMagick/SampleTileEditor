using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using GameLogic.Data;
using UbisoftTileEditor.Annotations;
using UbisoftTileEditor.Converters;
using UbisoftTileEditor.Helpers;
using UbisoftTileEditor.ViewModels.Data;

namespace UbisoftTileEditor
{
    /// <summary>
    /// Interaction logic for WorldViewControl.xaml
    /// </summary>
    public partial class WorldViewControl : UserControl, INotifyPropertyChanged
    {
        public static readonly DependencyProperty GameWorldProperty = DependencyProperty.Register("GameWorld", typeof(GameWorldViewModel), typeof(WorldViewControl), new PropertyMetadata(GameWorldChanged));

        private static void GameWorldChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ( (WorldViewControl)d ).RegenerateWorld();
        }

        private byte _selectedTemplateIndex;

        public WorldViewControl()
        {
            InitializeComponent();
            this.ChangeTemplateCommand = new RelayCommand(x => this.ExecuteChangeTemplate(x));
        }

        public byte SelectedTemplateIndex
        {
            get { return _selectedTemplateIndex; }
            set
            {
                _selectedTemplateIndex = value;
                this.OnPropertyChanged();
            }
        }

        public GameWorldViewModel GameWorld
        {
            get
            {
                return this.GetValue(GameWorldProperty) as GameWorldViewModel;
            }
            set
            {
                this.SetValue(GameWorldProperty, value);
            }
        }

        public ICommand ChangeTemplateCommand { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void RegenerateWorld()
        {
            this.PanelTiles.Children.Clear();
            var worldSize = this.GameWorld.WorldSize;

            for (byte y = 0; ( y * worldSize.TileHeight ) < this.PanelTiles.Height; y++)
            {
                for (byte x = 0; ( x * worldSize.TileWidth ) < this.PanelTiles.Width; x++)
                {
                    var image = new Image();
                    image.Width = worldSize.TileWidth;
                    image.Height = worldSize.TileWidth;

                    if (( y < worldSize.HeightInTiles ) && ( x < worldSize.WidthInTiles ))
                    {
                        Binding imageSourceBinding = new Binding(string.Format("GameWorld[{0},{1}].TemplateIndex", x.ToString(), y.ToString()));
                        imageSourceBinding.Converter = new TemplateIndexToBitmapImageConverter(GameWorld.Templates);
                        image.SetBinding(Image.SourceProperty, imageSourceBinding);
                    }

                    Button button = new Button();

                    // Remove border and highlight style
                    button.BorderThickness = new Thickness(0);
                    button.Style = (Style)this.FindResource(ToolBar.ButtonStyleKey);
                    button.Padding = new Thickness(0);

                    button.Content = image;
                    button.CommandParameter = new Vector(x, y);
                    button.Command = this.ChangeTemplateCommand;

                    this.PanelTiles.Children.Add(button);
                }
            }
        }

        private void ExecuteChangeTemplate(object o)
        {
            Vector position = (Vector)o;

            byte x = Convert.ToByte(position.X);
            byte y = Convert.ToByte(position.Y);

            var cell = this.GameWorld.Cells.FirstOrDefault(c => c.TileX == x && c.TileY == y);

            if (cell == null)
            {
                cell = new CellViewModel(new Cell() { TileX = x, TileY = y });
                this.GameWorld.Cells.Add(cell);
            }

            //cell.TemplateIndex = this.SelectedTemplateIndex;
            cell.TemplateIndex = (byte)this.ListBox.SelectedIndex;
        }
    }
}