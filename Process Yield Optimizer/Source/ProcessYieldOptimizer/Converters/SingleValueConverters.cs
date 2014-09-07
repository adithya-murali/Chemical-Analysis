using ProcessYieldOptimizer.Dialogs;
using ProcessYieldOptimizer.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace ProcessYieldOptimizer.Converters
{
    //For Scenario ListView in ScenarioControl.xaml, also used by ListView in AddScenarioDialog.xaml
    public class ScenarioListViewWidthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                double returnWidth = 0;
                double width = 0;

                Double.TryParse(value.ToString(), out width);

                returnWidth = width - 15; //at 1080P the column should fill most space on the screen without generating a scrollbar

                if (returnWidth >= 0)
                    return returnWidth; //if less than 0
                else
                    return width; //return original value
            }
            catch(Exception ex)
            {
                ErrorWindow window = new ErrorWindow("ColumnWidth Conversion fail [1]", "Format invalid", "Error Message: \n" + ex.Message);

                //Center dialog to user's window screen
                var desktopWorkingArea = System.Windows.SystemParameters.WorkArea;
                window.Left = desktopWorkingArea.Left + desktopWorkingArea.Right / 2 - window.Width / 2;
                window.Top = desktopWorkingArea.Top + desktopWorkingArea.Bottom / 2 - window.Height / 2;
                //also prevents re-opening in a different location which occurs if nothing is specified when using WPF

                window.ShowDialog();

            } //Should not come here...unless programmer wrote the wrong invalid option format

            return 0; 
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    //Boolean to Visibility Converter (True = Visible, False = Collapsed) used in ScenarioDetailsControl.xaml
    public class BooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (value != null && (Boolean)value == true) //if not null and true
                    return Visibility.Visible;
                else
                    return Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                ErrorWindow window = new ErrorWindow("Boolean to Visibility Conversion", "Format invalid", "Error Message: \n" + ex.Message);

                //Center dialog to user's window screen
                var desktopWorkingArea = System.Windows.SystemParameters.WorkArea;
                window.Left = desktopWorkingArea.Left + desktopWorkingArea.Right / 2 - window.Width / 2;
                window.Top = desktopWorkingArea.Top + desktopWorkingArea.Bottom / 2 - window.Height / 2;
                //also prevents re-opening in a different location which occurs if nothing is specified when using WPF

                window.ShowDialog();

            } //Should not come here...unless programmer wrote the wrong invalid option format

            return value; 
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    //Item to Visibility Converter (NotNull = Visible, Null = Collapsed) used in ScenarioDetailsControl.xaml
    public class ScenarioDetailsVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (value != null) //if not null and true
                    return Visibility.Visible;
                else
                    return Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                ErrorWindow window = new ErrorWindow("Item Visibility Conversion", "Format invalid", "Error Message: \n" + ex.Message);

                //Center dialog to user's window screen
                var desktopWorkingArea = System.Windows.SystemParameters.WorkArea;
                window.Left = desktopWorkingArea.Left + desktopWorkingArea.Right / 2 - window.Width / 2;
                window.Top = desktopWorkingArea.Top + desktopWorkingArea.Bottom / 2 - window.Height / 2;
                //also prevents re-opening in a different location which occurs if nothing is specified when using WPF

                window.ShowDialog();

            } //Should not come here...unless programmer wrote the wrong invalid option format

            return value; 
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    //Item to Enabled Converter (NotNull = Visible, Null = Hidden) used in ScenarioDetailsControl.xaml
    public class ScenarioDetailsEnabledConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (value != null) //if not null
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                ErrorWindow window = new ErrorWindow("Item to Enabled Conversion", "Format invalid", "Error Message: \n" + ex.Message);

                //Center dialog to user's window screen
                var desktopWorkingArea = System.Windows.SystemParameters.WorkArea;
                window.Left = desktopWorkingArea.Left + desktopWorkingArea.Right / 2 - window.Width / 2;
                window.Top = desktopWorkingArea.Top + desktopWorkingArea.Bottom / 2 - window.Height / 2;
                //also prevents re-opening in a different location which occurs if nothing is specified when using WPF

                window.ShowDialog();

            } //Should not come here...unless programmer wrote the wrong invalid option format

            return value; 
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    //Convert Text for Calculate Efficient Button in ScenarioControl.xaml
    public class CalculateButtonTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (value != null)
                {
                    int dataSetCount = (int)value; 

                    if (dataSetCount > 0) //if dataset has values
                    {
                        return "Recalculate Efficiency";
                    }
                }

                return "Calculate Efficiency";
            }
            catch (Exception ex)
            {
                ErrorWindow window = new ErrorWindow("Calculate Button Text", "Format invalid", "Error Message: \n" + ex.Message);

                //Center dialog to user's window screen
                var desktopWorkingArea = System.Windows.SystemParameters.WorkArea;
                window.Left = desktopWorkingArea.Left + desktopWorkingArea.Right / 2 - window.Width / 2;
                window.Top = desktopWorkingArea.Top + desktopWorkingArea.Bottom / 2 - window.Height / 2;
                //also prevents re-opening in a different location which occurs if nothing is specified when using WPF

                window.ShowDialog();
            } //Should not come here...unless programmer wrote the wrong invalid option format

            return value; 
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    //Convert Text for Show Actual/Percentage Button in ScenarioControl.xaml
    public class ShowActualToggleTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (value != null)
                {
                    string header = (string)value;

                    if (!header.Equals("Efficiency/Simple Yield [%]")) //if not in percentage
                        return "Show in %";
                }

                return "Show Actual";
            }
            catch (Exception ex)
            {
                ErrorWindow window = new ErrorWindow("Show Actual/Percentage Text", "Format invalid", "Error Message: \n" + ex.Message);

                //Center dialog to user's window screen
                var desktopWorkingArea = System.Windows.SystemParameters.WorkArea;
                window.Left = desktopWorkingArea.Left + desktopWorkingArea.Right / 2 - window.Width / 2;
                window.Top = desktopWorkingArea.Top + desktopWorkingArea.Bottom / 2 - window.Height / 2;
                //also prevents re-opening in a different location which occurs if nothing is specified when using WPF

                window.ShowDialog();

            } //Should not come here...unless programmer wrote the wrong invalid option format

            return value; 
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    //Convert Text for Visualize Button in ScenarioControl.xaml [Yield Tab only]
    public class VisualizeButtonTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) //value is a boolean
        {
            try
            {
                if (value != null)
                {
                    bool visualize = (bool)value;

                    if (visualize) //if already visualizing colors
                        return "Hide Colors";
                    else
                        return "Visualize";
                }

                return "Visualize";
            }
            catch (Exception ex)
            {
                ErrorWindow window = new ErrorWindow(" Visualize Button Text", "Format invalid", "Error Message: \n" + ex.Message);

                //Center dialog to user's window screen
                var desktopWorkingArea = System.Windows.SystemParameters.WorkArea;
                window.Left = desktopWorkingArea.Left + desktopWorkingArea.Right / 2 - window.Width / 2;
                window.Top = desktopWorkingArea.Top + desktopWorkingArea.Bottom / 2 - window.Height / 2;
                //also prevents re-opening in a different location which occurs if nothing is specified when using WPF

                window.ShowDialog();
              
            } //Should not come here...unless programmer wrote the wrong invalid option format

            return "Visualize";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    //Column Value format to two decimal places for DataGrids used in ScenarioDetailsControl.xaml [Only Yield Tab]
    public class ColumnValueFormatConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (value != null)
                {
                    double initValue = (double)value;
                    string formattedValue = String.Format("{0:0.##}", initValue); //Max two decimal places format
                    double returnValue = Double.Parse(formattedValue); //convert back to double

                    return returnValue; //return formatted double
                }

                return 0; //return 0
            }
            catch (Exception ex)
            {
                ErrorWindow window = new ErrorWindow("Column value format", "Format invalid", "Error Message: \n" + ex.Message);

                //Center dialog to user's window screen
                var desktopWorkingArea = System.Windows.SystemParameters.WorkArea;
                window.Left = desktopWorkingArea.Left + desktopWorkingArea.Right / 2 - window.Width / 2;
                window.Top = desktopWorkingArea.Top + desktopWorkingArea.Bottom / 2 - window.Height / 2;
                //also prevents re-opening in a different location which occurs if nothing is specified when using WPF

                window.ShowDialog();

            } 

            return 0; 
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }     
}
