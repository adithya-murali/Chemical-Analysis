using ProcessYieldOptimizer.Dialogs;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace ProcessYieldOptimizer.Converters
{
    //Convert Text to Solidbrush color for DataGrid Cells in ScenarioControl.xaml
    public class ForeGroundTextColorConverter : IMultiValueConverter
    {
        public object Convert(object[] value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (value[0] != null)
                {
                    bool visualize = (bool)value[0];

                    if (visualize)
                    {
                        if (value[1] != null)
                        {
                            string color = (string)value[1];

                            if (color.Equals("Red"))
                                return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F27474"));
                            else if (color.Equals("Blue"))
                                return new SolidColorBrush(Colors.LightBlue);
                            else if (color.Equals("Green"))
                                return new SolidColorBrush(Colors.LightGreen);
                        }
                    }
                }

                return new SolidColorBrush(Colors.Transparent); //return transparent
            }
            catch
            {
                return new SolidColorBrush(Colors.Transparent); //happens when user is editing values
            }

        }
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
