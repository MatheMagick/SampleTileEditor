using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Input;

namespace UbisoftTileEditor.Converters
{
    public sealed class BoolToCursorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool objectValue = (bool) value;
            return objectValue ? Cursors.Arrow : Cursors.Cross;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
