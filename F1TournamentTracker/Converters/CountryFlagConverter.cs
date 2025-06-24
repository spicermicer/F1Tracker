using Avalonia.Data.Converters;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using System;
using System.Globalization;

namespace F1TournamentTracker.Converters
{
    class CountryFlagConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is not CultureInfo cultureInfo)
                return null;            

            var path = $"avares://F1TournamentTracker/Assets/Flags/{cultureInfo.TwoLetterISOLanguageName}.png";


            var bitmap = new Bitmap(AssetLoader.Open(new Uri(path)));
            return bitmap;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
