using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using NasobeniEdhouse.Models.EdhouseTask;

namespace NasobeniEdhouse.Views.Converters
{
    class CollectionValuesConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string toReturn ="";
            
            foreach (int iterator in (List<int>)value)
            {
                toReturn += (toReturn.Length == 0) ? iterator.ToString() : string.Format($",{iterator}");
            }

            return toReturn;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
