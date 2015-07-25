using System.IO;
using Newtonsoft.Json;

namespace GameLogic.Data
{
    public static class GameWorldFactory
    {
        private static readonly object Lock = new object();

        public static GameWorld CreateDefaultUbisoftGameWorld()
        {
            string ubisoftTemplates = Resources.UbisoftTemplatesJSON;
            GameWorld result =  JsonConvert.DeserializeObject<GameWorld>(ubisoftTemplates);

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
    }
}
