using System;
using System.Windows.Media.Imaging;
using Newtonsoft.Json;

namespace GameLogic.Data
{
    public sealed class Template
    {
        public string TemplateID { get; set; }
        public double Angle { get; set; }
        public Component[] Components { get; set; }

        [JsonIgnore]
        public BitmapImage BitmapImage {
            get
            {
                return new BitmapImage(new Uri(this.Components[0].Sprites[0].TexturePath, UriKind.Relative));
            }
        }
    }
}
