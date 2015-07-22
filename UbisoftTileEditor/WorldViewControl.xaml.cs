using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using GameLogic.Data;

namespace UbisoftTileEditor
{
    /// <summary>
    /// Interaction logic for WorldViewControl.xaml
    /// </summary>
    public partial class WorldViewControl : UserControl
    {
        public WorldViewControl()
        {
            InitializeComponent();
        }

        public GameWorld GameWorld { get; set; }

        private void WorldViewControl_OnLoaded(object sender, RoutedEventArgs e)
        {
            this.PanelTiles.Children.Clear();
            var worldSize = this.GameWorld.WorldSize;

            for (byte y = 0; ( y * worldSize.HeightInTiles ) < this.ActualHeight && y < worldSize.HeightInTiles; y++)
            {
                for (byte x = 0; ( x * worldSize.WidthInTiles ) < this.ActualWidth && x < worldSize.WidthInTiles; x++)
                {
                    var image = new Image();

                    image.Width = worldSize.TileWidth;
                    image.Height = worldSize.TileWidth;
                    Binding imageSourceBinding = new Binding("");
                    imageSourceBinding.Source = GameWorld.Cells.FirstOrDefault(cell => cell.TileX == x && cell.TileY == y);
                    imageSourceBinding.Converter = new CellToBitmapImageConverter(GameWorld);
                    image.SetBinding(Image.SourceProperty, imageSourceBinding);

                    this.PanelTiles.Children.Add(image);
                }
            }
        }
    }
}