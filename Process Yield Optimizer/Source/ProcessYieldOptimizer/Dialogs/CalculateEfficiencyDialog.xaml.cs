using ProcessYieldOptimizer.ViewModels;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for CalculateEfficiencyDialog.xaml
    /// </summary>
    public partial class CalculateEfficiencyDialog : Window
    {
        public CalculateEfficiencyDialog()
        {
            InitializeComponent();
        }

        //Two Way binding was too slow in certain cases and additional data validation for simple concepts seems easier or quicker using event handler
        private void YieldInputsBoxInput_Changed(object sender, TextChangedEventArgs e)
        {
            ProcessYieldViewModel myVM = (ProcessYieldViewModel)this.DataContext;

            myVM.CalculateModelFormatter.YieldTextUnits = ((TextBox)sender).Text;
        }        

        private void Calculate_Click(object sender, RoutedEventArgs e)
        {
            this.Close(); //Allow ICommand to do the rest
        }
    }
}
