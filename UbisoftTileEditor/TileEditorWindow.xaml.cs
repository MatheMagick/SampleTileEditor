using System.IO;
using System.Windows;
using GameLogic.Data;
using Newtonsoft.Json;

namespace UbisoftTileEditor
{
    /// <summary>
    /// Interaction logic for TileEditorWindow.xaml
    /// </summary>
    public partial class TileEditorWindow : Window
    {
        public TileEditorWindow()
        {
            InitializeComponent();

            var gameWorld = JsonConvert.DeserializeObject<GameWorld>(File.ReadAllText("level.json"));
            this.WorldView.GameWorld = gameWorld;
        }

        private void MenuItem_OnSaveClick(object sender, RoutedEventArgs e)
        {
        }
    }
}
