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
    /// Interaction logic for ModifyScenarioDialog.xaml
    /// </summary>
    public partial class ModifyScenarioDialog : Window
    {
        public ModifyScenarioDialog()
        {
            InitializeComponent();
        }

        private void ModifyClickEvent_Click(object sender, RoutedEventArgs e)
        {
            ProcessYieldViewModel myVM = (ProcessYieldViewModel)this.DataContext;
            int tolerance = 0;
            //check if scenario name is different than parent
            if (myVM.ViewModelFormatter.ScenarioName.Equals(myVM.ViewModelFormatter.ParentScenario.Name))
                tolerance = 1; //if same then in validation we should except only one match to exisiting lists of scenarios

            if (myVM.validateScenario(myVM.ViewModelFormatter.ScenarioName, tolerance)) //if passes validation, else ICommand will generate error
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
