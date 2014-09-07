using Microsoft.Practices.Prism.Mvvm;
using ProcessYieldOptimizer.Models;
using ProcessYieldOptimizer.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for AddScenarioDialog.xaml
    /// </summary>
    public partial class AddScenarioDialog : Window
    {
        public AddScenarioDialog()
        {
            InitializeComponent();
        }

        private void AddClickEvent_Click(object sender, RoutedEventArgs e)
        {
            ProcessYieldViewModel myVM = (ProcessYieldViewModel)this.DataContext;

            if (myVM.validateScenario(myVM.ViewModelFormatter.ScenarioName)) //if passes validation, else ICommand will generate error
                this.Close(); //Close dialog let ICommand do rest
        }

        //Two Way binding was too slow in certain cases and additional data validation for simple concepts seems easier or quicker using event handler
        private void MainTextBoxInput_Changed(object sender, TextChangedEventArgs e)
        {
            ProcessYieldViewModel myVM = (ProcessYieldViewModel)this.DataContext;

            myVM.ViewModelFormatter.MainTabName = ((TextBox)sender).Text;
        }

        //Two Way binding was too slow in certain cases and additional data validation for simple concepts seems easier or quicker using event handler
        private void ScenarioBoxInput_Changed(object sender, TextChangedEventArgs e)
        {
            ProcessYieldViewModel myVM = (ProcessYieldViewModel)this.DataContext;

            myVM.ViewModelFormatter.ScenarioName = ((TextBox)sender).Text;
        }
    }
}
