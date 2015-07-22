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
            GameWorld world = GameWorldFactory.CreateUbisoftGameWorld();
            string json = JsonConvert.SerializeObject(world, Formatting.Indented);
            GameWorld ubisoftWorld = JsonConvert.DeserializeObject<GameWorld>(File.ReadAllText("level.json"));
            var serializeObject = JsonConvert.SerializeObject(ubisoftWorld, Formatting.Indented);
            File.WriteAllText("parsed.json", serializeObject);
        }

        private void MenuItem_OnSaveClick(object sender, RoutedEventArgs e)
        {
            Task.
        }
    }
}
