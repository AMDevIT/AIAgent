using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace AMDevIT.AI.App.WPF.UI.Converters;

public class BooleanToBrushConverter
    : IValueConverter
{
    #region Properties

    public Brush? TrueBrush
    {
        get;
        set;
    }

    public Brush? FalseBrush
    {
        get;
        set;
    }

    #endregion

    #region Methods

    public object? Convert(object? value, Type? targetType, object? parameter, CultureInfo? language)
    {
        if (value is bool booleanValue)
            return booleanValue ? TrueBrush : FalseBrush;
        return FalseBrush;
    }

    public object ConvertBack(object? value, Type? targetType, object? parameter, CultureInfo? language)
    {
        throw new NotSupportedException();
    }

    #endregion
}
