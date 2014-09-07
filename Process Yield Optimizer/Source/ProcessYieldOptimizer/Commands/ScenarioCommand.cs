using ProcessYieldOptimizer.Dialogs;
using ProcessYieldOptimizer.Models;
using ProcessYieldOptimizer.Utilities;
using ProcessYieldOptimizer.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ProcessYieldOptimizer.Commands
{
    class AddScenarioCommand : ICommand
    {
        public AddScenarioCommand(ProcessYieldViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        private ProcessYieldViewModel _viewModel;

        #region ICommand Members

        public bool CanExecute(object parameter)
        {
            return true; //always execute
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object parameter)
        {
            _viewModel.ViewModelFormatter.clear(); //Forget previous user input

            AddScenarioDialog window = new AddScenarioDialog();

            //Center dialog to user's window screen
            var desktopWorkingArea = System.Windows.SystemParameters.WorkArea;
            window.Left = desktopWorkingArea.Left + desktopWorkingArea.Right / 2 - window.Width / 2;
            window.Top = desktopWorkingArea.Top + desktopWorkingArea.Bottom / 2 - window.Height / 2;
            //also prevents re-opening in a different location which occurs if nothing is specified when using WPF

            window.DataContext = _viewModel;

            window.ShowDialog();
        }

        #endregion ICommand Members
    }

    class ModifyScenarioCommand : ICommand
    {
        public ModifyScenarioCommand(ProcessYieldViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        private ProcessYieldViewModel _viewModel;

        #region ICommand Members

        public bool CanExecute(object parameter)
        {
            if (_viewModel.MyScenarios.SelectedScenario != null) //if a scenario is selected by user
                return true;
            else
                return false;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object parameter)
        {
            _viewModel.ViewModelFormatter.clear(); //Forget previous user input
            _viewModel.ViewModelFormatter.setUp(_viewModel.MyScenarios.SelectedScenario); //Set it up with current scenario details

            ModifyScenarioDialog window = new ModifyScenarioDialog();

            //Center dialog to user's window screen
            var desktopWorkingArea = System.Windows.SystemParameters.WorkArea;
            window.Left = desktopWorkingArea.Left + desktopWorkingArea.Right / 2 - window.Width / 2;
            window.Top = desktopWorkingArea.Top + desktopWorkingArea.Bottom / 2 - window.Height / 2;
            //also prevents re-opening in a different location which occurs if nothing is specified when using WPF

            window.DataContext = _viewModel;

            window.ShowDialog();
        }

        #endregion ICommand Members
    }

    class RemoveScenarioCommand : ICommand
    {
        public RemoveScenarioCommand(ProcessYieldViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        private ProcessYieldViewModel _viewModel;

        #region ICommand Members

        public bool CanExecute(object parameter)
        {
            if (_viewModel.MyScenarios.SelectedScenario != null) //if a scenario is selected by user
                return true;
            else
                return false;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object parameter)
        {
            MessageBoxResult result = MessageBox.Show("Delete selected scenario: " + _viewModel.MyScenarios.SelectedScenario.Name + "?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                _viewModel.MyScenarios.deleteScenario();
            }
        }

        #endregion ICommand Members
    }

    class CalculateDialogCommand : ICommand
    {
        public CalculateDialogCommand(ProcessYieldViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        private ProcessYieldViewModel _viewModel;

        #region ICommand Members

        public bool CanExecute(object parameter)
        {
            if (_viewModel.MyScenarios.SelectedScenario != null) //if a scenario is selected by user
                return true; //Second type of validation occurs when command is exectued
            else
                return false;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object parameter)
        {
            string validate = validateScenario();

            if (validate.Equals("Valid"))
            {
                CalculateEfficiencyDialog window = new CalculateEfficiencyDialog();

                //Center dialog to user's window screen
                var desktopWorkingArea = System.Windows.SystemParameters.WorkArea;
                window.Left = desktopWorkingArea.Left + desktopWorkingArea.Right / 2 - window.Width / 2;
                window.Top = desktopWorkingArea.Top + desktopWorkingArea.Bottom / 2 - window.Height / 2;
                //also prevents re-opening in a different location which occurs if nothing is specified when using WPF

                _viewModel.CalculateModelFormatter = new ScenarioCalculateHelper(_viewModel.MyScenarios.SelectedScenario); //Set format helper to new instance with parent scenario detail as selected            
                _viewModel.CalculateModelFormatter.setUp(); //Tell formatter to set up itself based on given scenario detail

                window.DataContext = _viewModel; //set window datacontext

                window.ShowDialog();
            }
            else //Tell User why validation failed
            {
                ErrorWindow window = new ErrorWindow("Efficiency Validation", validate);

                //Center dialog to user's window screen
                var desktopWorkingArea = System.Windows.SystemParameters.WorkArea;
                window.Left = desktopWorkingArea.Left + desktopWorkingArea.Right / 2 - window.Width / 2;
                window.Top = desktopWorkingArea.Top + desktopWorkingArea.Bottom / 2 - window.Height / 2;
                //also prevents re-opening in a different location which occurs if nothing is specified when using WPF

                window.ShowDialog();
            }
        }

        #endregion ICommand Members

        /* Valid Scenario does another different validation check 
         * 
         * Makes sures all Performance Indicators that are visible have more or equal number of rows similar to MainTab
         * 
         * If not returns string with message where the first validation error occurs.
         * The message will shown to user so he/she can make the necessary changes.
         */
        private string validateScenario()
        {
            int mainTabRows = _viewModel.MyScenarios.SelectedScenario.MainTab.DataSet.Count;

            if (mainTabRows > 0) //if at least one row of values
            {
                for (int tabs = 0; tabs < StaticConfiguration.maxPerformanceIndicators; tabs++)
                {
                    switch (tabs)
                    {
                        case 0:
                            if (_viewModel.MyScenarios.SelectedScenario.PerformanceIndicator1.TabVisibility == true
                                && _viewModel.MyScenarios.SelectedScenario.PerformanceIndicator1.DataSet.Count < mainTabRows)
                                return "Invalid;" + _viewModel.MyScenarios.SelectedScenario.PerformanceIndicator1.HeaderText + " has less amount values than "
                                    + _viewModel.MyScenarios.SelectedScenario.MainTab.HeaderText;
                            break; //else its valid
                        case 1:
                            if (_viewModel.MyScenarios.SelectedScenario.PerformanceIndicator2.TabVisibility == true
                                && _viewModel.MyScenarios.SelectedScenario.PerformanceIndicator2.DataSet.Count < mainTabRows)
                                return "Invalid;" + _viewModel.MyScenarios.SelectedScenario.PerformanceIndicator2.HeaderText + " has less amount values than "
                                    + _viewModel.MyScenarios.SelectedScenario.MainTab.HeaderText;
                            break; //else its valid
                        case 2:
                            if (_viewModel.MyScenarios.SelectedScenario.PerformanceIndicator3.TabVisibility == true
                                && _viewModel.MyScenarios.SelectedScenario.PerformanceIndicator3.DataSet.Count < mainTabRows)
                                return "Invalid;" + _viewModel.MyScenarios.SelectedScenario.PerformanceIndicator3.HeaderText + " has less amount values than "
                                    + _viewModel.MyScenarios.SelectedScenario.MainTab.HeaderText;
                            break; //else its valid
                        case 3:
                            if (_viewModel.MyScenarios.SelectedScenario.PerformanceIndicator4.TabVisibility == true
                                && _viewModel.MyScenarios.SelectedScenario.PerformanceIndicator4.DataSet.Count < mainTabRows)
                                return "Invalid;" + _viewModel.MyScenarios.SelectedScenario.PerformanceIndicator4.HeaderText + " has less amount values than "
                                    + _viewModel.MyScenarios.SelectedScenario.MainTab.HeaderText;
                            break; //else its valid
                        case 4:
                            if (_viewModel.MyScenarios.SelectedScenario.PerformanceIndicator5.TabVisibility == true
                                && _viewModel.MyScenarios.SelectedScenario.PerformanceIndicator5.DataSet.Count < mainTabRows)
                                return "Invalid;" + _viewModel.MyScenarios.SelectedScenario.PerformanceIndicator5.HeaderText + " has less amount values than "
                                    + _viewModel.MyScenarios.SelectedScenario.MainTab.HeaderText;
                            break; //else its valid
                        case 5:
                            if (_viewModel.MyScenarios.SelectedScenario.PerformanceIndicator6.TabVisibility == true
                                && _viewModel.MyScenarios.SelectedScenario.PerformanceIndicator6.DataSet.Count < mainTabRows)
                                return "Invalid;" + _viewModel.MyScenarios.SelectedScenario.PerformanceIndicator6.HeaderText + " has less amount values than "
                                    + _viewModel.MyScenarios.SelectedScenario.MainTab.HeaderText;
                            break; //else its valid
                        case 6:
                            if (_viewModel.MyScenarios.SelectedScenario.PerformanceIndicator7.TabVisibility == true
                                && _viewModel.MyScenarios.SelectedScenario.PerformanceIndicator7.DataSet.Count < mainTabRows)
                                return "Invalid;" + _viewModel.MyScenarios.SelectedScenario.PerformanceIndicator7.HeaderText + " has less amount values than "
                                    + _viewModel.MyScenarios.SelectedScenario.MainTab.HeaderText;
                            break; //else its valid
                        case 7:
                            if (_viewModel.MyScenarios.SelectedScenario.PerformanceIndicator8.TabVisibility == true
                                && _viewModel.MyScenarios.SelectedScenario.PerformanceIndicator8.DataSet.Count < mainTabRows)
                                return "Invalid;" + _viewModel.MyScenarios.SelectedScenario.PerformanceIndicator8.HeaderText + " has less amount values than "
                                    + _viewModel.MyScenarios.SelectedScenario.MainTab.HeaderText;
                            break; //else its valid
                        case 8:
                            if (_viewModel.MyScenarios.SelectedScenario.PerformanceIndicator9.TabVisibility == true
                                && _viewModel.MyScenarios.SelectedScenario.PerformanceIndicator9.DataSet.Count < mainTabRows)
                                return "Invalid;" + _viewModel.MyScenarios.SelectedScenario.PerformanceIndicator9.HeaderText + " has less amount values than "
                                    + _viewModel.MyScenarios.SelectedScenario.MainTab.HeaderText;
                            break; //else its valid
                        case 9:
                            if (_viewModel.MyScenarios.SelectedScenario.PerformanceIndicator10.TabVisibility == true
                                && _viewModel.MyScenarios.SelectedScenario.PerformanceIndicator10.DataSet.Count < mainTabRows)
                                return "Invalid;" + _viewModel.MyScenarios.SelectedScenario.PerformanceIndicator10.HeaderText + " has less amount values than "
                                    + _viewModel.MyScenarios.SelectedScenario.MainTab.HeaderText;
                            break; //else its valid
                    }
                }

                return "Valid";
            }
            else
                return "Invalid; Main/Base tab has now rows of values.";
        }
    }

    class ToggleEfficiencyCommand : ICommand
    {
        public ToggleEfficiencyCommand(ProcessYieldViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        private ProcessYieldViewModel _viewModel;

        #region ICommand Members

        public bool CanExecute(object parameter)
        {
            if (_viewModel.MyScenarios.SelectedScenario != null && _viewModel.MyScenarios.SelectedScenario.YieldTab.TabVisibility == true) //if not null and yield tab is visible
                return true; 
            else
                return false;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object parameter)
        {
            try
            {
                ScenarioDetail mySelection = _viewModel.MyScenarios.SelectedScenario;

                if (_viewModel.MyScenarios.SelectedScenario.YieldTab.HeaderText.Equals("Efficiency/Simple Yield [%]"))
                {                    
                    double minValue = mySelection.YieldTab.MinValue; //get stored min value and convert to value

                    //Convert percentages to total value
                    if (minValue > 0) //needs to be > 0
                    {
                        for (int row = 0; row < mySelection.YieldTab.DataSet.Count; row++)
                        {
                            mySelection.YieldTab.DataSet[row].Column1 = (mySelection.YieldTab.DataSet[row].Column1 / 100 - 1) * -1 * minValue + minValue;
                            mySelection.YieldTab.DataSet[row].Column2 = (mySelection.YieldTab.DataSet[row].Column2 / 100 - 1) * -1 * minValue + minValue;
                            mySelection.YieldTab.DataSet[row].Column3 = (mySelection.YieldTab.DataSet[row].Column3 / 100 - 1) * -1 * minValue + minValue;
                            mySelection.YieldTab.DataSet[row].Column4 = (mySelection.YieldTab.DataSet[row].Column4 / 100 - 1) * -1 * minValue + minValue;
                            mySelection.YieldTab.DataSet[row].Column5 = (mySelection.YieldTab.DataSet[row].Column5 / 100 - 1) * -1 * minValue + minValue;
                            mySelection.YieldTab.DataSet[row].Column6 = (mySelection.YieldTab.DataSet[row].Column6 / 100 - 1) * -1 * minValue + minValue;
                            mySelection.YieldTab.DataSet[row].Column7 = (mySelection.YieldTab.DataSet[row].Column7 / 100 - 1) * -1 * minValue + minValue;
                            mySelection.YieldTab.DataSet[row].Column8 = (mySelection.YieldTab.DataSet[row].Column8 / 100 - 1) * -1 * minValue + minValue;
                            mySelection.YieldTab.DataSet[row].Column9 = (mySelection.YieldTab.DataSet[row].Column9 / 100 - 1) * -1 * minValue + minValue;
                            mySelection.YieldTab.DataSet[row].Column10 = (mySelection.YieldTab.DataSet[row].Column10 / 100 - 1) * -1 * minValue + minValue;
                        }
                    }
                    else //if 0 or negative..should already be 0 but set it to 0
                    {
                        for (int row = 0; row < mySelection.YieldTab.DataSet.Count; row++)
                        {
                            mySelection.YieldTab.DataSet[row].Column1 = 0;
                            mySelection.YieldTab.DataSet[row].Column2 = 0;
                            mySelection.YieldTab.DataSet[row].Column3 = 0;
                            mySelection.YieldTab.DataSet[row].Column4 = 0;
                            mySelection.YieldTab.DataSet[row].Column5 = 0;
                            mySelection.YieldTab.DataSet[row].Column6 = 0;
                            mySelection.YieldTab.DataSet[row].Column7 = 0;
                            mySelection.YieldTab.DataSet[row].Column8 = 0;
                            mySelection.YieldTab.DataSet[row].Column9 = 0;
                            mySelection.YieldTab.DataSet[row].Column10 = 0;
                        }
                    }

                    _viewModel.MyScenarios.SelectedScenario.YieldTab.HeaderText = "Efficiency/Simple Yield " + _viewModel.MyScenarios.SelectedScenario.YieldTab.UnitsText;
                }
                else
                {                    
                    double minValue = mySelection.YieldTab.MinValue; //get stored min value and convert to value

                    //Convert percentages to total value
                    if (minValue > 0) //needs to be > 0
                    {
                        for (int row = 0; row < mySelection.YieldTab.DataSet.Count; row++)
                        {
                            mySelection.YieldTab.DataSet[row].Column1 = (1 - (mySelection.YieldTab.DataSet[row].Column1 - minValue) / minValue) * 100;
                            mySelection.YieldTab.DataSet[row].Column2 = (1 - (mySelection.YieldTab.DataSet[row].Column2 - minValue) / minValue) * 100;
                            mySelection.YieldTab.DataSet[row].Column3 = (1 - (mySelection.YieldTab.DataSet[row].Column3 - minValue) / minValue) * 100;
                            mySelection.YieldTab.DataSet[row].Column4 = (1 - (mySelection.YieldTab.DataSet[row].Column4 - minValue) / minValue) * 100;
                            mySelection.YieldTab.DataSet[row].Column5 = (1 - (mySelection.YieldTab.DataSet[row].Column5 - minValue) / minValue) * 100;
                            mySelection.YieldTab.DataSet[row].Column6 = (1 - (mySelection.YieldTab.DataSet[row].Column6 - minValue) / minValue) * 100;
                            mySelection.YieldTab.DataSet[row].Column7 = (1 - (mySelection.YieldTab.DataSet[row].Column7 - minValue) / minValue) * 100;
                            mySelection.YieldTab.DataSet[row].Column8 = (1 - (mySelection.YieldTab.DataSet[row].Column8 - minValue) / minValue) * 100;
                            mySelection.YieldTab.DataSet[row].Column9 = (1 - (mySelection.YieldTab.DataSet[row].Column9 - minValue) / minValue) * 100;
                            mySelection.YieldTab.DataSet[row].Column10 = (1 - (mySelection.YieldTab.DataSet[row].Column10 - minValue) / minValue) * 100;
                        }
                    }
                    else //if 0 or negative..should already be 0 but set it to 0
                    {
                        for (int row = 0; row < mySelection.YieldTab.DataSet.Count; row++)
                        {
                            mySelection.YieldTab.DataSet[row].Column1 = 0;
                            mySelection.YieldTab.DataSet[row].Column2 = 0;
                            mySelection.YieldTab.DataSet[row].Column3 = 0;
                            mySelection.YieldTab.DataSet[row].Column4 = 0;
                            mySelection.YieldTab.DataSet[row].Column5 = 0;
                            mySelection.YieldTab.DataSet[row].Column6 = 0;
                            mySelection.YieldTab.DataSet[row].Column7 = 0;
                            mySelection.YieldTab.DataSet[row].Column8 = 0;
                            mySelection.YieldTab.DataSet[row].Column9 = 0;
                            mySelection.YieldTab.DataSet[row].Column10 = 0;
                        }
                    }

                    _viewModel.MyScenarios.SelectedScenario.YieldTab.HeaderText = "Efficiency/Simple Yield [%]";
                }
            }
            catch (Exception ex)
            {
                ErrorWindow window = new ErrorWindow("Efficiency Calculation", "Error occured after validation.\nSee details.", "Error Message: \n" + ex.Message);

                //Center dialog to user's window screen
                var desktopWorkingArea = System.Windows.SystemParameters.WorkArea;
                window.Left = desktopWorkingArea.Left + desktopWorkingArea.Right / 2 - window.Width / 2;
                window.Top = desktopWorkingArea.Top + desktopWorkingArea.Bottom / 2 - window.Height / 2;
                //also prevents re-opening in a different location which occurs if nothing is specified when using WPF

                window.ShowDialog();
            }
        }

        #endregion ICommand Members

    }

    class SaveScenarioCommand : ICommand
    {
        public SaveScenarioCommand(ProcessYieldViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        private ProcessYieldViewModel _viewModel;

        #region ICommand Members

        public bool CanExecute(object parameter)
        {
            if (_viewModel.MyScenarios.SelectedScenario != null) //if nothing is sleectiod
                return true; 
            else
                return false;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object parameter)
        {
            try
            {
                ScenarioDetail mySelection = _viewModel.MyScenarios.SelectedScenario;

                Microsoft.Win32.SaveFileDialog Dialog = new Microsoft.Win32.SaveFileDialog();
                Dialog.DefaultExt = ".xml";
                Dialog.Filter = "XML Files (*.xml)|*.xml";

                Nullable<bool> userResult = Dialog.ShowDialog();

                if (userResult == true)
                {
                    XMLSaveLoadScenario saveScenario = new XMLSaveLoadScenario();
                    saveScenario.setFilePath(Dialog.FileName);
                    saveScenario.ScenarioName = mySelection.Name;
                    saveScenario.MainTab = mySelection.MainTab;

                    List<TabDetail> myPIs = new List<TabDetail>();
                    myPIs.Add(mySelection.PerformanceIndicator1);
                    myPIs.Add(mySelection.PerformanceIndicator2);
                    myPIs.Add(mySelection.PerformanceIndicator3);
                    myPIs.Add(mySelection.PerformanceIndicator4);
                    myPIs.Add(mySelection.PerformanceIndicator5);
                    myPIs.Add(mySelection.PerformanceIndicator6);
                    myPIs.Add(mySelection.PerformanceIndicator7);
                    myPIs.Add(mySelection.PerformanceIndicator8);
                    myPIs.Add(mySelection.PerformanceIndicator9);
                    myPIs.Add(mySelection.PerformanceIndicator10);

                    saveScenario.PerformanceIndicators = myPIs;

                    string exception = saveScenario.CreateFile();

                    if (!String.IsNullOrEmpty(exception))
                    {
                        throw new Exception(exception);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorWindow window = new ErrorWindow("Save Failed", "Error occured after validation.\nSee details.", "Error Message: \n" + ex.Message);

                //Center dialog to user's window screen
                var desktopWorkingArea = System.Windows.SystemParameters.WorkArea;
                window.Left = desktopWorkingArea.Left + desktopWorkingArea.Right / 2 - window.Width / 2;
                window.Top = desktopWorkingArea.Top + desktopWorkingArea.Bottom / 2 - window.Height / 2;
                //also prevents re-opening in a different location which occurs if nothing is specified when using WPF

                window.ShowDialog();
            }
        }

        #endregion ICommand Members
    }


    class LoadScenarioCommand : ICommand
    {
        public LoadScenarioCommand(ProcessYieldViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        private ProcessYieldViewModel _viewModel;

        #region ICommand Members

        public bool CanExecute(object parameter)
        {
            return true; //always valid option
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object parameter)
        {
            try
            {
                XMLSaveLoadScenario loadScenario = new XMLSaveLoadScenario();

                Microsoft.Win32.OpenFileDialog Dialog = new Microsoft.Win32.OpenFileDialog();
                Dialog.DefaultExt = ".xml";
                Dialog.Filter = "XML Files (*.xml)|*.xml";

                Nullable<bool> userResult = Dialog.ShowDialog();

                if (userResult == true)
                {
                    loadScenario.setFilePath(Dialog.FileName);
                    string exception = loadScenario.ReadFile();

                    if (!String.IsNullOrEmpty(exception))
                    {
                        throw new Exception(exception);
                    }
                    else
                    {
                        if (!_viewModel.validateScenario(loadScenario.ScenarioName)) //validate scenario name that it is unique
                        {
                            //Do what windows does and say 'Copy of '
                            loadScenario.ScenarioName = "Copy of " + loadScenario.ScenarioName;
                        }

                        _viewModel.MyScenarios.addScenario(loadScenario.ScenarioName, loadScenario.MainTab, loadScenario.PerformanceIndicators);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorWindow window = new ErrorWindow("Load Failed", "See details", "Error Message: \n" + ex.Message);

                //Center dialog to user's window screen
                var desktopWorkingArea = System.Windows.SystemParameters.WorkArea;
                window.Left = desktopWorkingArea.Left + desktopWorkingArea.Right / 2 - window.Width / 2;
                window.Top = desktopWorkingArea.Top + desktopWorkingArea.Bottom / 2 - window.Height / 2;
                //also prevents re-opening in a different location which occurs if nothing is specified when using WPF

                window.ShowDialog();
            }
        }

        #endregion ICommand Members
    }
}
