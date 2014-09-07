using ProcessYieldOptimizer.Dialogs;
using ProcessYieldOptimizer.Models;
using ProcessYieldOptimizer.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ProcessYieldOptimizer.Commands
{
    class DialogAddPICommand : ICommand
    {
        public DialogAddPICommand(ProcessYieldViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        private ProcessYieldViewModel _viewModel;

        #region ICommand Members

        public bool CanExecute(object parameter)
        {
            if (_viewModel.ViewModelFormatter.PerformanceIndicators.Count < StaticConfiguration.maxPerformanceIndicators)
            {
                return true;
            }//valid option for user unless amount of PIs reached

            return false;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object parameter)
        {
            TempPerformanceIndicator newPI = new TempPerformanceIndicator("Empty"); //Create a new Performance Indicator
            newPI.Index = _viewModel.ViewModelFormatter.PerformanceIndicators.Count + 1; //Adjust index value

            _viewModel.ViewModelFormatter.PerformanceIndicators.Add(newPI); //Add a new Performance Indicator
        }

        #endregion ICommand Members
    }

    class DialogRemovePICommand : ICommand
    {
        public DialogRemovePICommand(ProcessYieldViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        private ProcessYieldViewModel _viewModel;

        #region ICommand Members

        public bool CanExecute(object parameter)
        {
            if (_viewModel.ViewModelFormatter.SelectedPerformanceIndicator != null && _viewModel.ViewModelFormatter.SelectedPerformanceIndicator.Index != 1)
            {
                //if not null and not the first index (since we need minimum 1 Performance Indicator)
                return true; //remove command is valid option for user
            }

            return false; //remove command is not a valid option for user
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object parameter)
        {
            _viewModel.ViewModelFormatter.removeSelected();
        }

        #endregion ICommand Members
    }

    class DialogAddScenariocommand : ICommand
    {
        public DialogAddScenariocommand(ProcessYieldViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        private ProcessYieldViewModel _viewModel;

        #region ICommand Members

        public bool CanExecute(object parameter)
        {
            try
            {
                if (String.IsNullOrEmpty(_viewModel.ViewModelFormatter.ScenarioName)) //if scenario name is empty
                    return false; //Cant add invalid format to model
                
                if (String.IsNullOrEmpty(_viewModel.ViewModelFormatter.MainTabName)) //if main tab name is empty
                    return false; //Cant add invalid format to model
                
                if (_viewModel.ViewModelFormatter.PerformanceIndicators != null && String.IsNullOrEmpty(_viewModel.ViewModelFormatter.PerformanceIndicators[0].Name)) //if first PI name is empty
                    return false; //Cant add invalid format to model

                return true; //Passed all conditions so format is valid for model                
            }
            catch
            {
                return false;
            }
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object parameter)
        {
            //Did not make this validation part of Can Execute because want user to know this is the reason it wont add ..we can change later or use interface for dataerror info
            if (_viewModel.validateScenario(_viewModel.ViewModelFormatter.ScenarioName)) //if validation of scenario name is successfull
            {
                //Give Main Tab same amount of rows as the first PI which is the required PI by user
                TempPerformanceIndicator mainTab = new TempPerformanceIndicator(_viewModel.ViewModelFormatter.MainTabName, _viewModel.ViewModelFormatter.PerformanceIndicators[0].Rows);
                _viewModel.MyScenarios.addScenario(_viewModel.ViewModelFormatter.ScenarioName, mainTab, _viewModel.ViewModelFormatter.PerformanceIndicators);
                //Come up with a better scheme later if need be here
            }
            else
            {
                ErrorWindow window = new ErrorWindow("Invalid Name", "A scenario with that name already exists in this application.");

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

    class DialogModifyScenarioCommand : ICommand
    {
        public DialogModifyScenarioCommand(ProcessYieldViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        private ProcessYieldViewModel _viewModel;

        #region ICommand Members

        public bool CanExecute(object parameter)
        {
            try
            {
                if (String.IsNullOrEmpty(_viewModel.ViewModelFormatter.ScenarioName)) //if scenario name is empty
                    return false; //Cant add invalid format to model

                if (String.IsNullOrEmpty(_viewModel.ViewModelFormatter.MainTabName)) //if main tab name is empty
                    return false; //Cant add invalid format to model

                if (_viewModel.ViewModelFormatter.PerformanceIndicators != null && String.IsNullOrEmpty(_viewModel.ViewModelFormatter.PerformanceIndicators[0].Name)) //if first PI name is empty
                    return false; //Cant add invalid format to model

                return true; //Passed all conditions so format is valid for model                
            }
            catch
            {
                return false;
            }
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object parameter)
        {
            int tolerance = 0;
            //check if scenario name is different than parent
            if (_viewModel.ViewModelFormatter.ScenarioName.Equals(_viewModel.ViewModelFormatter.ParentScenario.Name))
                tolerance = 1; //if same then in validation we should except only one match to exisiting lists of scenarios

            //Did not make this validation part of Can Execute because want user to know this is the reason it wont add ..we can change later or use interface for dataerror info
            if (_viewModel.validateScenario(_viewModel.ViewModelFormatter.ScenarioName, tolerance)) //if validation of scenario name is successfull with only 1 exact match at most
            {
                //Give Main Tab same amount of rows as the first PI which is the required PI by user
                TempPerformanceIndicator mainTab = new TempPerformanceIndicator(_viewModel.ViewModelFormatter.MainTabName, _viewModel.ViewModelFormatter.PerformanceIndicators[0].Rows);
                _viewModel.MyScenarios.modifyScenario(_viewModel.ViewModelFormatter.ParentScenario.Name, _viewModel.ViewModelFormatter.ScenarioName, mainTab, _viewModel.ViewModelFormatter.PerformanceIndicators);
                //Come up with a better scheme later if need be here
            }
            else
            {
                ErrorWindow window = new ErrorWindow("Invalid Name", "A scenario with that name already exists in this application.");

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

    class CalculateEfficiencyCommand : ICommand
    {
        public CalculateEfficiencyCommand(ProcessYieldViewModel viewModel)
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
            try
            {
                string validate = validateScenario();

                if (validate.Equals("Valid"))
                {
                    _viewModel.MyScenarios.SelectedScenario.YieldTab.HeaderText = "Efficiency/Simple Yield [%]";
                    _viewModel.MyScenarios.SelectedScenario.YieldTab.UnitsText = _viewModel.CalculateModelFormatter.YieldTextUnits;

                    double minValue = Double.MaxValue; //Min Value = 100% Efficiency under the optimal ideal conditions
                    ScenarioDetail mySelection = _viewModel.MyScenarios.SelectedScenario; //get reference ... incase slow that selection changed by user and for ease of reading

                    mySelection.YieldTab.DataSet = new ObservableCollection<DataRowDetail>(); //remove any exisitng values
                    mySelection.YieldTab.addRows(mySelection.MainTab.DataSet.Count); //Add rows to yield tab

                    //Fill table with just the sum of all PIs
                    for (int rows = 0; rows < mySelection.MainTab.DataSet.Count; rows++)
                    {
                        double val = 0;

                        val = getTotalColumn1(mySelection, rows) / mySelection.MainTab.DataSet[rows].Column1;

                        if (val < minValue) //If value of this column was greater than max value
                            minValue = val; //set this value as 100% Efficiency

                        mySelection.YieldTab.DataSet[rows].Column1 = val; //put value into the right column in YieldTab
                        //Do same for rest of the columns below 

                        val = getTotalColumn2(mySelection, rows) / mySelection.MainTab.DataSet[rows].Column2;

                        if (val < minValue)
                            minValue = val;

                        mySelection.YieldTab.DataSet[rows].Column2 = val;

                        val = getTotalColumn3(mySelection, rows) / mySelection.MainTab.DataSet[rows].Column3;

                        if (val < minValue)
                            minValue = val;

                        mySelection.YieldTab.DataSet[rows].Column3 = val;

                        val = getTotalColumn4(mySelection, rows) / mySelection.MainTab.DataSet[rows].Column4;

                        if (val < minValue)
                            minValue = val;

                        mySelection.YieldTab.DataSet[rows].Column4 = val;

                        val = getTotalColumn5(mySelection, rows) / mySelection.MainTab.DataSet[rows].Column5;

                        if (val < minValue)
                            minValue = val;

                        mySelection.YieldTab.DataSet[rows].Column5 = val;

                        val = getTotalColumn6(mySelection, rows) / mySelection.MainTab.DataSet[rows].Column6;

                        if (val < minValue)
                            minValue = val;

                        mySelection.YieldTab.DataSet[rows].Column6 = val;

                        val = getTotalColumn7(mySelection, rows) / mySelection.MainTab.DataSet[rows].Column7;

                        if (val < minValue)
                            minValue = val;

                        mySelection.YieldTab.DataSet[rows].Column7 = val;

                        val = getTotalColumn8(mySelection, rows) / mySelection.MainTab.DataSet[rows].Column8;

                        if (val < minValue)
                            minValue = val;

                        mySelection.YieldTab.DataSet[rows].Column8 = val;

                        val = getTotalColumn9(mySelection, rows) / mySelection.MainTab.DataSet[rows].Column9;

                        if (val < minValue)
                            minValue = val;

                        mySelection.YieldTab.DataSet[rows].Column9 = val;

                        val = getTotalColumn10(mySelection, rows) / mySelection.MainTab.DataSet[rows].Column10;

                        if (val < minValue)
                            minValue = val;

                        mySelection.YieldTab.DataSet[rows].Column10 = val;
                    }

                    //Now Reformat Yield Tab to reflect values close to maxValue approaching 100% efficiency
                    if (minValue > 0) //if greater than 0 - minvalue doesnt make sense 
                    {
                        for (int row = 0; row < mySelection.YieldTab.DataSet.Count; row++)
                        {
                            mySelection.YieldTab.DataSet[row].Column1 = (1 - (mySelection.YieldTab.DataSet[row].Column1 - minValue) / minValue) * 100;
                            mySelection.YieldTab.DataSet[row].Column2 = (1 - (mySelection.YieldTab.DataSet[row].Column2 - minValue) / minValue) * 100;
                            mySelection.YieldTab.DataSet[row].Column3 = (1 - (mySelection.YieldTab.DataSet[row].Column3- minValue) / minValue) * 100;
                            mySelection.YieldTab.DataSet[row].Column4 = (1 - (mySelection.YieldTab.DataSet[row].Column4 - minValue) / minValue) * 100;
                            mySelection.YieldTab.DataSet[row].Column5 = (1 - (mySelection.YieldTab.DataSet[row].Column5 - minValue) / minValue) * 100;
                            mySelection.YieldTab.DataSet[row].Column6 = (1 - (mySelection.YieldTab.DataSet[row].Column6 - minValue) / minValue) * 100;
                            mySelection.YieldTab.DataSet[row].Column7 = (1 - (mySelection.YieldTab.DataSet[row].Column7 - minValue) / minValue) * 100;
                            mySelection.YieldTab.DataSet[row].Column8 = (1 - (mySelection.YieldTab.DataSet[row].Column8 - minValue) / minValue) * 100;
                            mySelection.YieldTab.DataSet[row].Column9 = (1 - (mySelection.YieldTab.DataSet[row].Column9 - minValue) / minValue) * 100;
                            mySelection.YieldTab.DataSet[row].Column10 = (1 - (mySelection.YieldTab.DataSet[row].Column10 - minValue) / minValue) * 100;
                            mySelection.YieldTab.MinValue = minValue; //set max value so we can toggle to get actual amount later
                        }
                    }
                    else //if 0 or negative
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

                            mySelection.YieldTab.MinValue = minValue; //clear any exisitng max value
                        }
                    }

                    //turn of colors / visualization
                    _viewModel.MyScenarios.SelectedScenario.MainTab.Visualize = false;
                    _viewModel.MyScenarios.SelectedScenario.PerformanceIndicator1.Visualize = false;
                    _viewModel.MyScenarios.SelectedScenario.PerformanceIndicator2.Visualize = false;
                    _viewModel.MyScenarios.SelectedScenario.PerformanceIndicator3.Visualize = false;
                    _viewModel.MyScenarios.SelectedScenario.PerformanceIndicator4.Visualize = false;
                    _viewModel.MyScenarios.SelectedScenario.PerformanceIndicator5.Visualize = false;
                    _viewModel.MyScenarios.SelectedScenario.PerformanceIndicator6.Visualize = false;
                    _viewModel.MyScenarios.SelectedScenario.PerformanceIndicator7.Visualize = false;
                    _viewModel.MyScenarios.SelectedScenario.PerformanceIndicator8.Visualize = false;
                    _viewModel.MyScenarios.SelectedScenario.PerformanceIndicator9.Visualize = false;
                    _viewModel.MyScenarios.SelectedScenario.PerformanceIndicator10.Visualize = false;
                    _viewModel.MyScenarios.SelectedScenario.YieldTab.Visualize = false;
                    //wait for user to re-issue command to recalculate colors for new efficiency values

                    _viewModel.MyScenarios.SelectedScenario.YieldTab.TabVisibility = true; //Set Tab as visible
                }
                else //If validation failed
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

        #region Private Helper methods
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

        /* Below are methods to get total value for each column in a specific row
         * Call below methods only after validation done by 'validateScenario' method above
         */
        private double getTotalColumn1(ScenarioDetail mySelection, int row)
        {
            double total = 0;
            for (int tabs = 0; tabs < StaticConfiguration.maxPerformanceIndicators; tabs++)
            {
                switch (tabs)
                {
                    case 0:
                        if (_viewModel.MyScenarios.SelectedScenario.PerformanceIndicator1.TabVisibility == true)
                            total += mySelection.PerformanceIndicator1.DataSet[row].Column1;
                        break; //dont bother tab is not visible
                    case 1:
                        if (_viewModel.MyScenarios.SelectedScenario.PerformanceIndicator2.TabVisibility == true)
                            total += mySelection.PerformanceIndicator2.DataSet[row].Column1;
                        break; //dont bother tab is not visible
                    case 2:
                        if (_viewModel.MyScenarios.SelectedScenario.PerformanceIndicator3.TabVisibility == true)
                            total += mySelection.PerformanceIndicator3.DataSet[row].Column1;
                        break; //dont bother tab is not visible
                    case 3:
                        if (_viewModel.MyScenarios.SelectedScenario.PerformanceIndicator4.TabVisibility == true)
                            total += mySelection.PerformanceIndicator4.DataSet[row].Column1;
                        break; //dont bother tab is not visible
                    case 4:
                        if (_viewModel.MyScenarios.SelectedScenario.PerformanceIndicator5.TabVisibility == true)
                            total += mySelection.PerformanceIndicator5.DataSet[row].Column1;
                        break; //dont bother tab is not visible
                    case 5:
                        if (_viewModel.MyScenarios.SelectedScenario.PerformanceIndicator6.TabVisibility == true)
                            total += mySelection.PerformanceIndicator6.DataSet[row].Column1;
                        break; //dont bother tab is not visible
                    case 6:
                        if (_viewModel.MyScenarios.SelectedScenario.PerformanceIndicator7.TabVisibility == true)
                            total += mySelection.PerformanceIndicator7.DataSet[row].Column1;
                        break; //dont bother tab is not visible
                    case 7:
                        if (_viewModel.MyScenarios.SelectedScenario.PerformanceIndicator8.TabVisibility == true)
                            total += mySelection.PerformanceIndicator8.DataSet[row].Column1;
                        break; //dont bother tab is not visible
                    case 8:
                        if (_viewModel.MyScenarios.SelectedScenario.PerformanceIndicator9.TabVisibility == true)
                            total += mySelection.PerformanceIndicator9.DataSet[row].Column1;
                        break; //dont bother tab is not visible
                    case 9:
                        if (_viewModel.MyScenarios.SelectedScenario.PerformanceIndicator10.TabVisibility == true)
                            total += mySelection.PerformanceIndicator10.DataSet[row].Column1;
                        break; //dont bother tab is not visible
                }
            }

            return total;
        }

        private double getTotalColumn2(ScenarioDetail mySelection, int row)
        {
            double total = 0;
            for (int tabs = 0; tabs < StaticConfiguration.maxPerformanceIndicators; tabs++)
            {
                switch (tabs)
                {
                    case 0:
                        if (_viewModel.MyScenarios.SelectedScenario.PerformanceIndicator1.TabVisibility == true)
                            total += mySelection.PerformanceIndicator1.DataSet[row].Column2;
                        break; //dont bother tab is not visible
                    case 1:
                        if (_viewModel.MyScenarios.SelectedScenario.PerformanceIndicator2.TabVisibility == true)
                            total += mySelection.PerformanceIndicator2.DataSet[row].Column2;
                        break; //dont bother tab is not visible
                    case 2:
                        if (_viewModel.MyScenarios.SelectedScenario.PerformanceIndicator3.TabVisibility == true)
                            total += mySelection.PerformanceIndicator3.DataSet[row].Column2;
                        break; //dont bother tab is not visible
                    case 3:
                        if (_viewModel.MyScenarios.SelectedScenario.PerformanceIndicator4.TabVisibility == true)
                            total += mySelection.PerformanceIndicator4.DataSet[row].Column2;
                        break; //dont bother tab is not visible
                    case 4:
                        if (_viewModel.MyScenarios.SelectedScenario.PerformanceIndicator5.TabVisibility == true)
                            total += mySelection.PerformanceIndicator5.DataSet[row].Column2;
                        break; //dont bother tab is not visible
                    case 5:
                        if (_viewModel.MyScenarios.SelectedScenario.PerformanceIndicator6.TabVisibility == true)
                            total += mySelection.PerformanceIndicator6.DataSet[row].Column2;
                        break; //dont bother tab is not visible
                    case 6:
                        if (_viewModel.MyScenarios.SelectedScenario.PerformanceIndicator7.TabVisibility == true)
                            total += mySelection.PerformanceIndicator7.DataSet[row].Column2;
                        break; //dont bother tab is not visible
                    case 7:
                        if (_viewModel.MyScenarios.SelectedScenario.PerformanceIndicator8.TabVisibility == true)
                            total += mySelection.PerformanceIndicator8.DataSet[row].Column2;
                        break; //dont bother tab is not visible
                    case 8:
                        if (_viewModel.MyScenarios.SelectedScenario.PerformanceIndicator9.TabVisibility == true)
                            total += mySelection.PerformanceIndicator9.DataSet[row].Column2;
                        break; //dont bother tab is not visible
                    case 9:
                        if (_viewModel.MyScenarios.SelectedScenario.PerformanceIndicator10.TabVisibility == true)
                            total += mySelection.PerformanceIndicator10.DataSet[row].Column2;
                        break; //dont bother tab is not visible
                }
            }

            return total;
        }

        private double getTotalColumn3(ScenarioDetail mySelection, int row)
        {
            double total = 0;
            for (int tabs = 0; tabs < StaticConfiguration.maxPerformanceIndicators; tabs++)
            {
                switch (tabs)
                {
                    case 0:
                        if (_viewModel.MyScenarios.SelectedScenario.PerformanceIndicator1.TabVisibility == true)
                            total += mySelection.PerformanceIndicator1.DataSet[row].Column3;
                        break; //dont bother tab is not visible
                    case 1:
                        if (_viewModel.MyScenarios.SelectedScenario.PerformanceIndicator2.TabVisibility == true)
                            total += mySelection.PerformanceIndicator2.DataSet[row].Column3;
                        break; //dont bother tab is not visible
                    case 2:
                        if (_viewModel.MyScenarios.SelectedScenario.PerformanceIndicator3.TabVisibility == true)
                            total += mySelection.PerformanceIndicator3.DataSet[row].Column3;
                        break; //dont bother tab is not visible
                    case 3:
                        if (_viewModel.MyScenarios.SelectedScenario.PerformanceIndicator4.TabVisibility == true)
                            total += mySelection.PerformanceIndicator4.DataSet[row].Column3;
                        break; //dont bother tab is not visible
                    case 4:
                        if (_viewModel.MyScenarios.SelectedScenario.PerformanceIndicator5.TabVisibility == true)
                            total += mySelection.PerformanceIndicator5.DataSet[row].Column3;
                        break; //dont bother tab is not visible
                    case 5:
                        if (_viewModel.MyScenarios.SelectedScenario.PerformanceIndicator6.TabVisibility == true)
                            total += mySelection.PerformanceIndicator6.DataSet[row].Column3;
                        break; //dont bother tab is not visible
                    case 6:
                        if (_viewModel.MyScenarios.SelectedScenario.PerformanceIndicator7.TabVisibility == true)
                            total += mySelection.PerformanceIndicator7.DataSet[row].Column3;
                        break; //dont bother tab is not visible
                    case 7:
                        if (_viewModel.MyScenarios.SelectedScenario.PerformanceIndicator8.TabVisibility == true)
                            total += mySelection.PerformanceIndicator8.DataSet[row].Column3;
                        break; //dont bother tab is not visible
                    case 8:
                        if (_viewModel.MyScenarios.SelectedScenario.PerformanceIndicator9.TabVisibility == true)
                            total += mySelection.PerformanceIndicator9.DataSet[row].Column3;
                        break; //dont bother tab is not visible
                    case 9:
                        if (_viewModel.MyScenarios.SelectedScenario.PerformanceIndicator10.TabVisibility == true)
                            total += mySelection.PerformanceIndicator10.DataSet[row].Column3;
                        break; //dont bother tab is not visible
                }
            }

            return total;
        }

        private double getTotalColumn4(ScenarioDetail mySelection, int row)
        {
            double total = 0;
            for (int tabs = 0; tabs < StaticConfiguration.maxPerformanceIndicators; tabs++)
            {
                switch (tabs)
                {
                    case 0:
                        if (_viewModel.MyScenarios.SelectedScenario.PerformanceIndicator1.TabVisibility == true)
                            total += mySelection.PerformanceIndicator1.DataSet[row].Column4;
                        break; //dont bother tab is not visible
                    case 1:
                        if (_viewModel.MyScenarios.SelectedScenario.PerformanceIndicator2.TabVisibility == true)
                            total += mySelection.PerformanceIndicator2.DataSet[row].Column4;
                        break; //dont bother tab is not visible
                    case 2:
                        if (_viewModel.MyScenarios.SelectedScenario.PerformanceIndicator3.TabVisibility == true)
                            total += mySelection.PerformanceIndicator3.DataSet[row].Column4;
                        break; //dont bother tab is not visible
                    case 3:
                        if (_viewModel.MyScenarios.SelectedScenario.PerformanceIndicator4.TabVisibility == true)
                            total += mySelection.PerformanceIndicator4.DataSet[row].Column4;
                        break; //dont bother tab is not visible
                    case 4:
                        if (_viewModel.MyScenarios.SelectedScenario.PerformanceIndicator5.TabVisibility == true)
                            total += mySelection.PerformanceIndicator5.DataSet[row].Column4;
                        break; //dont bother tab is not visible
                    case 5:
                        if (_viewModel.MyScenarios.SelectedScenario.PerformanceIndicator6.TabVisibility == true)
                            total += mySelection.PerformanceIndicator6.DataSet[row].Column4;
                        break; //dont bother tab is not visible
                    case 6:
                        if (_viewModel.MyScenarios.SelectedScenario.PerformanceIndicator7.TabVisibility == true)
                            total += mySelection.PerformanceIndicator7.DataSet[row].Column4;
                        break; //dont bother tab is not visible
                    case 7:
                        if (_viewModel.MyScenarios.SelectedScenario.PerformanceIndicator8.TabVisibility == true)
                            total += mySelection.PerformanceIndicator8.DataSet[row].Column4;
                        break; //dont bother tab is not visible
                    case 8:
                        if (_viewModel.MyScenarios.SelectedScenario.PerformanceIndicator9.TabVisibility == true)
                            total += mySelection.PerformanceIndicator9.DataSet[row].Column4;
                        break; //dont bother tab is not visible
                    case 9:
                        if (_viewModel.MyScenarios.SelectedScenario.PerformanceIndicator10.TabVisibility == true)
                            total += mySelection.PerformanceIndicator10.DataSet[row].Column4;
                        break; //dont bother tab is not visible
                }
            }

            return total;
        }

        private double getTotalColumn5(ScenarioDetail mySelection, int row)
        {
            double total = 0;
            for (int tabs = 0; tabs < StaticConfiguration.maxPerformanceIndicators; tabs++)
            {
                switch (tabs)
                {
                    case 0:
                        if (_viewModel.MyScenarios.SelectedScenario.PerformanceIndicator1.TabVisibility == true)
                            total += mySelection.PerformanceIndicator1.DataSet[row].Column5;
                        break; //dont bother tab is not visible
                    case 1:
                        if (_viewModel.MyScenarios.SelectedScenario.PerformanceIndicator2.TabVisibility == true)
                            total += mySelection.PerformanceIndicator2.DataSet[row].Column5;
                        break; //dont bother tab is not visible
                    case 2:
                        if (_viewModel.MyScenarios.SelectedScenario.PerformanceIndicator3.TabVisibility == true)
                            total += mySelection.PerformanceIndicator3.DataSet[row].Column5;
                        break; //dont bother tab is not visible
                    case 3:
                        if (_viewModel.MyScenarios.SelectedScenario.PerformanceIndicator4.TabVisibility == true)
                            total += mySelection.PerformanceIndicator4.DataSet[row].Column5;
                        break; //dont bother tab is not visible
                    case 4:
                        if (_viewModel.MyScenarios.SelectedScenario.PerformanceIndicator5.TabVisibility == true)
                            total += mySelection.PerformanceIndicator5.DataSet[row].Column5;
                        break; //dont bother tab is not visible
                    case 5:
                        if (_viewModel.MyScenarios.SelectedScenario.PerformanceIndicator6.TabVisibility == true)
                            total += mySelection.PerformanceIndicator6.DataSet[row].Column5;
                        break; //dont bother tab is not visible
                    case 6:
                        if (_viewModel.MyScenarios.SelectedScenario.PerformanceIndicator7.TabVisibility == true)
                            total += mySelection.PerformanceIndicator7.DataSet[row].Column5;
                        break; //dont bother tab is not visible
                    case 7:
                        if (_viewModel.MyScenarios.SelectedScenario.PerformanceIndicator8.TabVisibility == true)
                            total += mySelection.PerformanceIndicator8.DataSet[row].Column5;
                        break; //dont bother tab is not visible
                    case 8:
                        if (_viewModel.MyScenarios.SelectedScenario.PerformanceIndicator9.TabVisibility == true)
                            total += mySelection.PerformanceIndicator9.DataSet[row].Column5;
                        break; //dont bother tab is not visible
                    case 9:
                        if (_viewModel.MyScenarios.SelectedScenario.PerformanceIndicator10.TabVisibility == true)
                            total += mySelection.PerformanceIndicator10.DataSet[row].Column5;
                        break; //dont bother tab is not visible
                }
            }

            return total;
        }

        private double getTotalColumn6(ScenarioDetail mySelection, int row)
        {
            double total = 0;
            for (int tabs = 0; tabs < StaticConfiguration.maxPerformanceIndicators; tabs++)
            {
                switch (tabs)
                {
                    case 0:
                        if (_viewModel.MyScenarios.SelectedScenario.PerformanceIndicator1.TabVisibility == true)
                            total += mySelection.PerformanceIndicator1.DataSet[row].Column6;
                        break; //dont bother tab is not visible
                    case 1:
                        if (_viewModel.MyScenarios.SelectedScenario.PerformanceIndicator2.TabVisibility == true)
                            total += mySelection.PerformanceIndicator2.DataSet[row].Column6;
                        break; //dont bother tab is not visible
                    case 2:
                        if (_viewModel.MyScenarios.SelectedScenario.PerformanceIndicator3.TabVisibility == true)
                            total += mySelection.PerformanceIndicator3.DataSet[row].Column6;
                        break; //dont bother tab is not visible
                    case 3:
                        if (_viewModel.MyScenarios.SelectedScenario.PerformanceIndicator4.TabVisibility == true)
                            total += mySelection.PerformanceIndicator4.DataSet[row].Column6;
                        break; //dont bother tab is not visible
                    case 4:
                        if (_viewModel.MyScenarios.SelectedScenario.PerformanceIndicator5.TabVisibility == true)
                            total += mySelection.PerformanceIndicator5.DataSet[row].Column6;
                        break; //dont bother tab is not visible
                    case 5:
                        if (_viewModel.MyScenarios.SelectedScenario.PerformanceIndicator6.TabVisibility == true)
                            total += mySelection.PerformanceIndicator6.DataSet[row].Column6;
                        break; //dont bother tab is not visible
                    case 6:
                        if (_viewModel.MyScenarios.SelectedScenario.PerformanceIndicator7.TabVisibility == true)
                            total += mySelection.PerformanceIndicator7.DataSet[row].Column6;
                        break; //dont bother tab is not visible
                    case 7:
                        if (_viewModel.MyScenarios.SelectedScenario.PerformanceIndicator8.TabVisibility == true)
                            total += mySelection.PerformanceIndicator8.DataSet[row].Column6;
                        break; //dont bother tab is not visible
                    case 8:
                        if (_viewModel.MyScenarios.SelectedScenario.PerformanceIndicator9.TabVisibility == true)
                            total += mySelection.PerformanceIndicator9.DataSet[row].Column6;
                        break; //dont bother tab is not visible
                    case 9:
                        if (_viewModel.MyScenarios.SelectedScenario.PerformanceIndicator10.TabVisibility == true)
                            total += mySelection.PerformanceIndicator10.DataSet[row].Column6;
                        break; //dont bother tab is not visible
                }
            }

            return total;
        }

        private double getTotalColumn7(ScenarioDetail mySelection, int row)
        {
            double total = 0;
            for (int tabs = 0; tabs < StaticConfiguration.maxPerformanceIndicators; tabs++)
            {
                switch (tabs)
                {
                    case 0:
                        if (_viewModel.MyScenarios.SelectedScenario.PerformanceIndicator1.TabVisibility == true)
                            total += mySelection.PerformanceIndicator1.DataSet[row].Column7;
                        break; //dont bother tab is not visible
                    case 1:
                        if (_viewModel.MyScenarios.SelectedScenario.PerformanceIndicator2.TabVisibility == true)
                            total += mySelection.PerformanceIndicator2.DataSet[row].Column7;
                        break; //dont bother tab is not visible
                    case 2:
                        if (_viewModel.MyScenarios.SelectedScenario.PerformanceIndicator3.TabVisibility == true)
                            total += mySelection.PerformanceIndicator3.DataSet[row].Column7;
                        break; //dont bother tab is not visible
                    case 3:
                        if (_viewModel.MyScenarios.SelectedScenario.PerformanceIndicator4.TabVisibility == true)
                            total += mySelection.PerformanceIndicator4.DataSet[row].Column7;
                        break; //dont bother tab is not visible
                    case 4:
                        if (_viewModel.MyScenarios.SelectedScenario.PerformanceIndicator5.TabVisibility == true)
                            total += mySelection.PerformanceIndicator5.DataSet[row].Column7;
                        break; //dont bother tab is not visible
                    case 5:
                        if (_viewModel.MyScenarios.SelectedScenario.PerformanceIndicator6.TabVisibility == true)
                            total += mySelection.PerformanceIndicator6.DataSet[row].Column7;
                        break; //dont bother tab is not visible
                    case 6:
                        if (_viewModel.MyScenarios.SelectedScenario.PerformanceIndicator7.TabVisibility == true)
                            total += mySelection.PerformanceIndicator7.DataSet[row].Column7;
                        break; //dont bother tab is not visible
                    case 7:
                        if (_viewModel.MyScenarios.SelectedScenario.PerformanceIndicator8.TabVisibility == true)
                            total += mySelection.PerformanceIndicator8.DataSet[row].Column7;
                        break; //dont bother tab is not visible
                    case 8:
                        if (_viewModel.MyScenarios.SelectedScenario.PerformanceIndicator9.TabVisibility == true)
                            total += mySelection.PerformanceIndicator9.DataSet[row].Column7;
                        break; //dont bother tab is not visible
                    case 9:
                        if (_viewModel.MyScenarios.SelectedScenario.PerformanceIndicator10.TabVisibility == true)
                            total += mySelection.PerformanceIndicator10.DataSet[row].Column7;
                        break; //dont bother tab is not visible
                }
            }

            return total;
        }

        private double getTotalColumn8(ScenarioDetail mySelection, int row)
        {
            double total = 0;
            for (int tabs = 0; tabs < StaticConfiguration.maxPerformanceIndicators; tabs++)
            {
                switch (tabs)
                {
                    case 0:
                        if (_viewModel.MyScenarios.SelectedScenario.PerformanceIndicator1.TabVisibility == true)
                            total += mySelection.PerformanceIndicator1.DataSet[row].Column8;
                        break; //dont bother tab is not visible
                    case 1:
                        if (_viewModel.MyScenarios.SelectedScenario.PerformanceIndicator2.TabVisibility == true)
                            total += mySelection.PerformanceIndicator2.DataSet[row].Column8;
                        break; //dont bother tab is not visible
                    case 2:
                        if (_viewModel.MyScenarios.SelectedScenario.PerformanceIndicator3.TabVisibility == true)
                            total += mySelection.PerformanceIndicator3.DataSet[row].Column8;
                        break; //dont bother tab is not visible
                    case 3:
                        if (_viewModel.MyScenarios.SelectedScenario.PerformanceIndicator4.TabVisibility == true)
                            total += mySelection.PerformanceIndicator4.DataSet[row].Column8;
                        break; //dont bother tab is not visible
                    case 4:
                        if (_viewModel.MyScenarios.SelectedScenario.PerformanceIndicator5.TabVisibility == true)
                            total += mySelection.PerformanceIndicator5.DataSet[row].Column8;
                        break; //dont bother tab is not visible
                    case 5:
                        if (_viewModel.MyScenarios.SelectedScenario.PerformanceIndicator6.TabVisibility == true)
                            total += mySelection.PerformanceIndicator6.DataSet[row].Column8;
                        break; //dont bother tab is not visible
                    case 6:
                        if (_viewModel.MyScenarios.SelectedScenario.PerformanceIndicator7.TabVisibility == true)
                            total += mySelection.PerformanceIndicator7.DataSet[row].Column8;
                        break; //dont bother tab is not visible
                    case 7:
                        if (_viewModel.MyScenarios.SelectedScenario.PerformanceIndicator8.TabVisibility == true)
                            total += mySelection.PerformanceIndicator8.DataSet[row].Column8;
                        break; //dont bother tab is not visible
                    case 8:
                        if (_viewModel.MyScenarios.SelectedScenario.PerformanceIndicator9.TabVisibility == true)
                            total += mySelection.PerformanceIndicator9.DataSet[row].Column8;
                        break; //dont bother tab is not visible
                    case 9:
                        if (_viewModel.MyScenarios.SelectedScenario.PerformanceIndicator10.TabVisibility == true)
                            total += mySelection.PerformanceIndicator10.DataSet[row].Column8;
                        break; //dont bother tab is not visible
                }
            }

            return total;
        }

        private double getTotalColumn9(ScenarioDetail mySelection, int row)
        {
            double total = 0;
            for (int tabs = 0; tabs < StaticConfiguration.maxPerformanceIndicators; tabs++)
            {
                switch (tabs)
                {
                    case 0:
                        if (_viewModel.MyScenarios.SelectedScenario.PerformanceIndicator1.TabVisibility == true)
                            total += mySelection.PerformanceIndicator1.DataSet[row].Column9;
                        break; //dont bother tab is not visible
                    case 1:
                        if (_viewModel.MyScenarios.SelectedScenario.PerformanceIndicator2.TabVisibility == true)
                            total += mySelection.PerformanceIndicator2.DataSet[row].Column9;
                        break; //dont bother tab is not visible
                    case 2:
                        if (_viewModel.MyScenarios.SelectedScenario.PerformanceIndicator3.TabVisibility == true)
                            total += mySelection.PerformanceIndicator3.DataSet[row].Column9;
                        break; //dont bother tab is not visible
                    case 3:
                        if (_viewModel.MyScenarios.SelectedScenario.PerformanceIndicator4.TabVisibility == true)
                            total += mySelection.PerformanceIndicator4.DataSet[row].Column9;
                        break; //dont bother tab is not visible
                    case 4:
                        if (_viewModel.MyScenarios.SelectedScenario.PerformanceIndicator5.TabVisibility == true)
                            total += mySelection.PerformanceIndicator5.DataSet[row].Column9;
                        break; //dont bother tab is not visible
                    case 5:
                        if (_viewModel.MyScenarios.SelectedScenario.PerformanceIndicator6.TabVisibility == true)
                            total += mySelection.PerformanceIndicator6.DataSet[row].Column9;
                        break; //dont bother tab is not visible
                    case 6:
                        if (_viewModel.MyScenarios.SelectedScenario.PerformanceIndicator7.TabVisibility == true)
                            total += mySelection.PerformanceIndicator7.DataSet[row].Column9;
                        break; //dont bother tab is not visible
                    case 7:
                        if (_viewModel.MyScenarios.SelectedScenario.PerformanceIndicator8.TabVisibility == true)
                            total += mySelection.PerformanceIndicator8.DataSet[row].Column9;
                        break; //dont bother tab is not visible
                    case 8:
                        if (_viewModel.MyScenarios.SelectedScenario.PerformanceIndicator9.TabVisibility == true)
                            total += mySelection.PerformanceIndicator9.DataSet[row].Column9;
                        break; //dont bother tab is not visible
                    case 9:
                        if (_viewModel.MyScenarios.SelectedScenario.PerformanceIndicator10.TabVisibility == true)
                            total += mySelection.PerformanceIndicator10.DataSet[row].Column9;
                        break; //dont bother tab is not visible
                }
            }

            return total;
        }

        private double getTotalColumn10(ScenarioDetail mySelection, int row)
        {
            double total = 0;
            for (int tabs = 0; tabs < StaticConfiguration.maxPerformanceIndicators; tabs++)
            {
                switch (tabs)
                {
                    case 0:
                        if (_viewModel.MyScenarios.SelectedScenario.PerformanceIndicator1.TabVisibility == true)
                            total += mySelection.PerformanceIndicator1.DataSet[row].Column10;
                        break; //dont bother tab is not visible
                    case 1:
                        if (_viewModel.MyScenarios.SelectedScenario.PerformanceIndicator2.TabVisibility == true)
                            total += mySelection.PerformanceIndicator2.DataSet[row].Column10;
                        break; //dont bother tab is not visible
                    case 2:
                        if (_viewModel.MyScenarios.SelectedScenario.PerformanceIndicator3.TabVisibility == true)
                            total += mySelection.PerformanceIndicator3.DataSet[row].Column10;
                        break; //dont bother tab is not visible
                    case 3:
                        if (_viewModel.MyScenarios.SelectedScenario.PerformanceIndicator4.TabVisibility == true)
                            total += mySelection.PerformanceIndicator4.DataSet[row].Column10;
                        break; //dont bother tab is not visible
                    case 4:
                        if (_viewModel.MyScenarios.SelectedScenario.PerformanceIndicator5.TabVisibility == true)
                            total += mySelection.PerformanceIndicator5.DataSet[row].Column10;
                        break; //dont bother tab is not visible
                    case 5:
                        if (_viewModel.MyScenarios.SelectedScenario.PerformanceIndicator6.TabVisibility == true)
                            total += mySelection.PerformanceIndicator6.DataSet[row].Column10;
                        break; //dont bother tab is not visible
                    case 6:
                        if (_viewModel.MyScenarios.SelectedScenario.PerformanceIndicator7.TabVisibility == true)
                            total += mySelection.PerformanceIndicator7.DataSet[row].Column10;
                        break; //dont bother tab is not visible
                    case 7:
                        if (_viewModel.MyScenarios.SelectedScenario.PerformanceIndicator8.TabVisibility == true)
                            total += mySelection.PerformanceIndicator8.DataSet[row].Column10;
                        break; //dont bother tab is not visible
                    case 8:
                        if (_viewModel.MyScenarios.SelectedScenario.PerformanceIndicator9.TabVisibility == true)
                            total += mySelection.PerformanceIndicator9.DataSet[row].Column10;
                        break; //dont bother tab is not visible
                    case 9:
                        if (_viewModel.MyScenarios.SelectedScenario.PerformanceIndicator10.TabVisibility == true)
                            total += mySelection.PerformanceIndicator10.DataSet[row].Column10;
                        break; //dont bother tab is not visible
                }
            }

            return total;
        }

        #endregion Private Helper methods
    }
}
