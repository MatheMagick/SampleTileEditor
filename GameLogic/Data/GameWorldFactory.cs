using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace GameLogic.Data
{
    public static class GameWorldFactory
    {
        public static GameWorld CreateUbisoftGameWorld()
        {
            GameWorld result = new GameWorld();

            InitializeTemplates(result);

            result.DefaultCell.TemplateIndex = 2;

            return result;
        }

        private static void InitializeTemplates(GameWorld result)
        {
            string ubisoftTemplates = Resources.UbisoftTemplatesJSON;
            result.Templates = JsonConvert.DeserializeObject<Template[]>(ubisoftTemplates);
        }
    }
}
