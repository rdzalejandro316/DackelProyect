using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace ClassLibrary1
{
    public class ForegroundColorConverter : IValueConverter

    {

        public object Convert(object value, Type targetType, object parameter, string language)

        {

            var data = value as int?;

            if (data != null && data > 0)

                return new SolidColorBrush(Colors.Green);

            else

                return new SolidColorBrush(Colors.Red);

        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)

        {

            throw new NotImplementedException();

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }


    }



}
