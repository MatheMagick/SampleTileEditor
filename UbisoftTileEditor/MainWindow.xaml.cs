using System.IO;
using System.Windows;
using GameLogic.Data;
using Newtonsoft.Json;

namespace UbisoftTileEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            GameWorld world = GameWorldFactory.CreateUbisoftGameWorld();
            string json = JsonConvert.SerializeObject(world, Formatting.Indented);
            GameWorld ubisoftWorld = JsonConvert.DeserializeObject<GameWorld>(File.ReadAllText("level.json"));
            var serializeObject = JsonConvert.SerializeObject(ubisoftWorld, Formatting.Indented);
            File.WriteAllText("parsed.json", serializeObject);
        }
    }
}
