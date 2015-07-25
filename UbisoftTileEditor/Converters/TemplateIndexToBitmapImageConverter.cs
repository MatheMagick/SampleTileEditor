using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using GameLogic.Data;

namespace UbisoftTileEditor.Converters
{
    internal sealed class TemplateIndexToBitmapImageConverter : IValueConverter
    {
        private readonly Template[] _templates;
        private Dictionary<byte, BitmapImage> _imagesCache;

        public TemplateIndexToBitmapImageConverter(Template[] templates)
        {
            _templates = templates;

            if (_imagesCache == null)
            {
                _imagesCache = templates.Select((x, i) =>
                    new
                    {
                        Image = new BitmapImage(new Uri(x.Components[0].Sprites[0].TexturePath, UriKind.Relative)),
                        Index = i
                    }).ToDictionary(x => (byte)x.Index, y => y.Image);
            }
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // TODO Remove
            //CellViewModel cell = value as CellViewModel;
            //byte templateIndex = (cell == null) ? _gameWorld.DefaultCellTemplateIndex : cell.TemplateIndex;
            byte templateIndex = (byte)value;

            return _imagesCache[templateIndex];
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}