using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace AMDevIT.AI.App.WPF.UI.Converters
{
    public class BooleanToAlignmentConverter : IValueConverter
    {
        #region Methods

        public object? Convert(object? value, Type? targetType, object? parameter, CultureInfo? language)
        {
            if (value is bool booleanValue)
            {
                // Ritorna un valore di HorizontalAlignment basato sul valore booleano
                return booleanValue ? HorizontalAlignment.Right : HorizontalAlignment.Left;
            }

            return HorizontalAlignment.Left; // Default se il valore non è un booleano
        }

        public object ConvertBack(object? value, Type? targetType, object? parameter, CultureInfo? language)
        {
            // Non implementiamo ConvertBack perché non è necessario in questo caso
            throw new NotSupportedException();
        }

        #endregion
    }
}
