using System.Windows.Controls;
using System.Windows.Input;

namespace UbisoftTileEditor.Helpers
{
    public sealed class NumericTextBox : TextBox
    {
        public NumericTextBox()
        {
            this.PreviewTextInput += NumericTextBox_PreviewTextInput;
        }

        void NumericTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            byte value;
            e.Handled = !byte.TryParse(e.Text, out value);
        }
    }
}