using System;
using System.Globalization;
using System.Windows.Data;
using GameLogic.Data;

namespace UbisoftTileEditor
{
    internal sealed class CellToBitmapImageConverter : IValueConverter
    {
        private readonly GameWorld _gameWorld;

        public CellToBitmapImageConverter(GameWorld gameWorld)
        {
            _gameWorld = gameWorld;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Cell cell = value as Cell;
            byte templateIndex = (cell == null) ? _gameWorld.DefaultCell.TemplateIndex : cell.TemplateIndex;

            return _gameWorld.Templates[templateIndex].BitmapImage;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}