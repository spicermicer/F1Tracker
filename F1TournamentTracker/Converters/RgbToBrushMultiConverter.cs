using Avalonia;
using Avalonia.Data;
using Avalonia.Data.Converters;
using Avalonia.Media;
using Avalonia.Media.Immutable;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace F1TournamentTracker.Converters
{
    internal class RgbToBrushMultiConverter : IMultiValueConverter
    {
        public object? Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
        {
            // Ensure all bindings are provided and attached to correct target type
            if (values?.Count != 3 || !targetType.IsAssignableFrom(typeof(ImmutableSolidColorBrush)))
                throw new NotSupportedException();

            // Ensure all bindings are correct type
            if (!values.All(x => x is byte or UnsetValueType or null))
                throw new NotSupportedException();

            // Pull values, DoNothing if any are unset.
            // Convert is called several times during initialization of bindings,
            // so some properties will be initially unset.
            if (values[0] is not byte r ||
                values[1] is not byte g ||
                values[2] is not byte b)
                return BindingOperations.DoNothing;

            byte a = 255;
            var color = new Color(a, r, g, b);
            return new ImmutableSolidColorBrush(color);
        }
    }
}
