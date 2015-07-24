using System.IO;
using Newtonsoft.Json;

namespace GameLogic.Data
{
    public static class GameWorldFactory
    {
        private static readonly object Lock = new object();

        public static GameWorld CreateDefaultUbisoftGameWorld()
        {
            GameWorld result = new GameWorld();

            InitializeTemplates(result);

            result.DefaultCell.TemplateIndex = 2;

            return result;
        }

        public static GameWorld OpenWorld(string filePath)
        {
            return JsonConvert.DeserializeObject<GameWorld>(File.ReadAllText(filePath));
        }

        public static void SaveWorld(GameWorld world, string filePath)
        {
            lock (Lock)
            {
                File.WriteAllText(filePath, JsonConvert.SerializeObject(world));
            }
        }

        private static void InitializeTemplates(GameWorld result)
        {
            string ubisoftTemplates = Resources.UbisoftTemplatesJSON;
            result.Templates = JsonConvert.DeserializeObject<Template[]>(ubisoftTemplates);
        }
    }
}
