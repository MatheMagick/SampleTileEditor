using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using GameLogic.Data;
using UbisoftTileEditor.ViewModels.Data;

namespace UbisoftTileEditor
{
    internal sealed class CellToBitmapImageConverter : IValueConverter
    {
        private readonly GameWorldViewModel _gameWorld;

        public CellToBitmapImageConverter(GameWorldViewModel gameWorld)
        {
            _gameWorld = gameWorld;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //CellViewModel cell = value as CellViewModel;
            //byte templateIndex = (cell == null) ? _gameWorld.DefaultCellTemplateIndex : cell.TemplateIndex;
            byte templateIndex = (byte)value;

            return new BitmapImage(new Uri(_gameWorld.Templates[templateIndex].Components[0].Sprites[0].TexturePath, UriKind.Relative));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}