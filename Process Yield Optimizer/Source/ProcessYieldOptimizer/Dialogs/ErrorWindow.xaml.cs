using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ProcessYieldOptimizer.Dialogs
{
    /// <summary>
    /// Interaction logic for ErrorWindow.xaml
    /// 
    /// </summary>
    public partial class ErrorWindow : Window, INotifyPropertyChanged
    {
        public ErrorWindow(string windowTitle, string errorDescription, string errorDetail = "")
        {
            InitializeComponent();

            this.Title = windowTitle;

            myGrid.DataContext = this;

            Error = errorDescription;
            ErrorDetails = errorDetail;

            this.MyDetailsBlock.Visibility = Visibility.Hidden;

            if (!String.IsNullOrEmpty(ErrorDetails)) //if error details is not empty
            {
                this.DetailsButton.Visibility = Visibility.Visible;
            }
            else
            {
                this.DetailsButton.Visibility = Visibility.Hidden;
            }
        }

        private string _Error;
        public string Error
        {
            get
            {
                return _Error;
            }
            set
            {
                _Error = value;
                OnPropertyChanged("Error");
            }
        }

        private string _ErrorDetails;
        public string ErrorDetails
        {
            get
            {
                return _ErrorDetails;
            }
            set
            {
                _ErrorDetails = value;
                OnPropertyChanged("ErrorDetails");
            }
        }

        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string change)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(change));
        }
        #endregion

        private bool _Toggle = false; //false = Show Details, true = Hide Details

        //Replace with an expander in future
        private void DetailsClick(object sender, RoutedEventArgs e)
        {

            if (_Toggle == false)
            {
                this.Height = 200;
                this.MyDetailsBlock.Visibility = Visibility.Visible;
                this.DetailsButton.Content = "Hide Details";
            }
            else
            {
                this.Height = 120;
                this.MyDetailsBlock.Visibility = Visibility.Hidden;
                this.DetailsButton.Content = "Show Details";
            }

            _Toggle = !_Toggle;
        }

    }
}
