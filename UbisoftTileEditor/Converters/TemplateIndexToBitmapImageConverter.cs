using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using GameLogic.Data;

namespace UbisoftTileEditor.Converters
{
    internal sealed class TemplateIndexToBitmapImageConverter : IValueConverter
    {
        private readonly Template[] _templates;

        public TemplateIndexToBitmapImageConverter(Template[] templates)
        {
            _templates = templates;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // TODO Remove
            //CellViewModel cell = value as CellViewModel;
            //byte templateIndex = (cell == null) ? _gameWorld.DefaultCellTemplateIndex : cell.TemplateIndex;
            byte templateIndex = (byte)value;
            var texturePath = _templates[templateIndex].Components[0].Sprites[0].TexturePath;

            return new BitmapImage(new Uri(texturePath, UriKind.Relative));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}