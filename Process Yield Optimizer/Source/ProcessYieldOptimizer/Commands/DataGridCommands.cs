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
    class CopyCommand : ICommand
    {
        public CopyCommand(ProcessYieldViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        private ProcessYieldViewModel _viewModel;

        #region ICommand Members

        public bool CanExecute(object parameter)
        {
            try
            {
                //if selected index is valid
                if (_viewModel.MyScenarios.SelectedScenario.SelectedDetailTab.SelectedRowIndex >= 0)
                    return true;

                return false; //if index is not valid
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
            try
            {
                int row = _viewModel.MyScenarios.SelectedScenario.SelectedDetailTab.SelectedRowIndex;
                //int maxRow = _viewModel.MyScenarios.SelectedScenario.SelectedDetailTab.DataSet.Count;
                int maxRow = row + 1; //Copy only to one row...to confusing to phrase such mechanism to user for now

                if (row < 0)
                    row = 0; //in case it occurs make row default 0

                string line = "";

                while (row < maxRow)
                {
                    if (!String.IsNullOrEmpty(line))
                        line = line + "\n"; //make a line break if appending to line variable

                    int col = 0;
                    string valueToInsert = "";
                    while (col < StaticConfiguration.maxColumns)
                    {
                        valueToInsert = getSelectedColumnValue(row, col);

                        if (valueToInsert.Equals("Invalid"))
                            throw new Exception("Selected column value was not valid from DataGrid");

                        line = line + valueToInsert; //append column values for row
                        col++;
                    }

                    row++;
                }

                System.Windows.Clipboard.SetText(line);
            }
            catch (Exception ex)
            {
                ErrorWindow window = new ErrorWindow("Copy Failed", "Format was invalid", "Error Message: \n" + ex.Message);

                //Center dialog to user's window screen
                var desktopWorkingArea = System.Windows.SystemParameters.WorkArea;
                window.Left = desktopWorkingArea.Left + desktopWorkingArea.Right / 2 - window.Width / 2;
                window.Top = desktopWorkingArea.Top + desktopWorkingArea.Bottom / 2 - window.Height / 2;
                //also prevents re-opening in a different location which occurs if nothing is specified when using WPF

                window.ShowDialog();
            } 
        }

        private string getSelectedColumnValue(int row, int col)
        {
            switch (col)
            {
                case 0:
                    return _viewModel.MyScenarios.SelectedScenario.SelectedDetailTab.DataSet[row].Column1.ToString() + "\t";
                case 1:
                    return _viewModel.MyScenarios.SelectedScenario.SelectedDetailTab.DataSet[row].Column2.ToString() + "\t";
                case 2:
                    return _viewModel.MyScenarios.SelectedScenario.SelectedDetailTab.DataSet[row].Column3.ToString() + "\t";
                case 3:
                    return _viewModel.MyScenarios.SelectedScenario.SelectedDetailTab.DataSet[row].Column4.ToString() + "\t";
                case 4:
                    return _viewModel.MyScenarios.SelectedScenario.SelectedDetailTab.DataSet[row].Column5.ToString() + "\t";
                case 5:
                    return _viewModel.MyScenarios.SelectedScenario.SelectedDetailTab.DataSet[row].Column6.ToString() + "\t";
                case 6:
                    return _viewModel.MyScenarios.SelectedScenario.SelectedDetailTab.DataSet[row].Column7.ToString() + "\t";
                case 7:
                    return _viewModel.MyScenarios.SelectedScenario.SelectedDetailTab.DataSet[row].Column8.ToString() + "\t";
                case 8:
                    return _viewModel.MyScenarios.SelectedScenario.SelectedDetailTab.DataSet[row].Column9.ToString() + "\t";
                case 9:
                    return _viewModel.MyScenarios.SelectedScenario.SelectedDetailTab.DataSet[row].Column10.ToString() + "\r";
            }

            return "Invalid";
        }

        #endregion ICommand Members
    }

    class CopyAllCommand : ICommand
    {
        public CopyAllCommand(ProcessYieldViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        private ProcessYieldViewModel _viewModel;

        #region ICommand Members

        public bool CanExecute(object parameter)
        {
            //only execute if there is at least one row part of datagrid
            try
            {
                //if selected index is valid
                if (_viewModel.MyScenarios.SelectedScenario.SelectedDetailTab.DataSet.Count > 0)
                    return true;

                return false; //if index is not valid
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
            try
            {
                int row = 0;
                int maxRow = _viewModel.MyScenarios.SelectedScenario.SelectedDetailTab.DataSet.Count;

                string line = "";

                while (row < maxRow)
                {
                    if (!String.IsNullOrEmpty(line))
                        line = line + "\n"; //make a line break if appending to line variable

                    int col = 0;
                    string valueToInsert = "";
                    while (col < StaticConfiguration.maxColumns)
                    {
                        valueToInsert = getSelectedColumnValue(row, col);

                        if (valueToInsert.Equals("Invalid"))
                            throw new Exception("Selected column value was not valid from DataGrid");

                        line = line + valueToInsert; //append column values for row
                        col++;
                    }

                    row++;
                }

                System.Windows.Clipboard.Clear(); //clear whats in clipboard
                System.Windows.Clipboard.SetText(line); //set this line
            }
            catch (Exception ex)
            {
                ErrorWindow window = new ErrorWindow("Copy All Failed", "Format was invalid", "Error Message: \n" + ex.Message);

                //Center dialog to user's window screen
                var desktopWorkingArea = System.Windows.SystemParameters.WorkArea;
                window.Left = desktopWorkingArea.Left + desktopWorkingArea.Right / 2 - window.Width / 2;
                window.Top = desktopWorkingArea.Top + desktopWorkingArea.Bottom / 2 - window.Height / 2;
                //also prevents re-opening in a different location which occurs if nothing is specified when using WPF

                window.ShowDialog();
            }
        }

        private string getSelectedColumnValue(int row, int col)
        {
            switch (col)
            {
                case 0:
                    return _viewModel.MyScenarios.SelectedScenario.SelectedDetailTab.DataSet[row].Column1.ToString() + "\t";
                case 1:
                    return _viewModel.MyScenarios.SelectedScenario.SelectedDetailTab.DataSet[row].Column2.ToString() + "\t";
                case 2:
                    return _viewModel.MyScenarios.SelectedScenario.SelectedDetailTab.DataSet[row].Column3.ToString() + "\t";
                case 3:
                    return _viewModel.MyScenarios.SelectedScenario.SelectedDetailTab.DataSet[row].Column4.ToString() + "\t";
                case 4:
                    return _viewModel.MyScenarios.SelectedScenario.SelectedDetailTab.DataSet[row].Column5.ToString() + "\t";
                case 5:
                    return _viewModel.MyScenarios.SelectedScenario.SelectedDetailTab.DataSet[row].Column6.ToString() + "\t";
                case 6:
                    return _viewModel.MyScenarios.SelectedScenario.SelectedDetailTab.DataSet[row].Column7.ToString() + "\t";
                case 7:
                    return _viewModel.MyScenarios.SelectedScenario.SelectedDetailTab.DataSet[row].Column8.ToString() + "\t";
                case 8:
                    return _viewModel.MyScenarios.SelectedScenario.SelectedDetailTab.DataSet[row].Column9.ToString() + "\t";
                case 9:
                    return _viewModel.MyScenarios.SelectedScenario.SelectedDetailTab.DataSet[row].Column10.ToString() + "\r";
            }

            return "Invalid";
        }

        #endregion ICommand Members
    }

    class PasteAllCommand : ICommand
    {
        public PasteAllCommand(ProcessYieldViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        private ProcessYieldViewModel _viewModel;

        #region ICommand Members

        public bool CanExecute(object parameter) 
        {
            //only execute if there is at least one row part of datagrid
            try
            {
                //if selected index is valid
                if (_viewModel.MyScenarios.SelectedScenario.SelectedDetailTab.DataSet.Count > 0)
                    return true;

                return false; //if index is not valid
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
            try
            {
                int row = 0;
                int maxRow = _viewModel.MyScenarios.SelectedScenario.SelectedDetailTab.DataSet.Count;

                string copyInfo = System.Windows.Clipboard.GetText();

                while (!String.IsNullOrEmpty(copyInfo) && row < maxRow)
                {
                    string line = "";
                    if (copyInfo.Contains("\n"))
                        line = copyInfo.Substring(0, copyInfo.IndexOf("\n") + 1);
                    else
                        line = copyInfo;
                        

                    int col = 0;
                    while (!String.IsNullOrEmpty(line) && col < StaticConfiguration.maxColumns)
                    {
                        if (line.Contains("\t"))
                        {
                            double valueToInsert = 0;

                            bool replaceValue = true;

                            string value = line.Substring(0, line.IndexOf("\t") + 1);
                            
                            //Use try catch
                            try
                            {
                                valueToInsert = Double.Parse(value); 
                            }
                            catch
                            {
                                replaceValue = false; //if parse failed
                            }


                            if (replaceValue) //if valid double value insert into model
                                setSelectedColumnValue(row, col, valueToInsert);

                            line = line.Remove(0, line.IndexOf("\t") + 1);
                        }
                        else if (line.Contains("\r"))
                        {
                            double valueToInsert = 0;

                            bool replaceValue = true;

                            string value = line.Substring(0, line.IndexOf("\r") + 1);

                            //Use try catch
                            try
                            {
                                valueToInsert = Double.Parse(value);
                            }
                            catch
                            {
                                replaceValue = false; //if parse failed
                            }

                            if (replaceValue) //if valid double value insert into model
                                setSelectedColumnValue(row, col, valueToInsert);

                            line = line.Remove(0, line.IndexOf("\r") + 1);
                        }

                        
                        col++;
                    }

                    if (copyInfo.Contains("\n"))
                        copyInfo = copyInfo.Remove(0, copyInfo.IndexOf("\n") + 1); //Remove the line from text copied from clipboard
                    else
                        copyInfo = ""; //Remove everything if not '\n'
                    row++;
                }
            }
            catch (Exception ex)
            {
                ErrorWindow window = new ErrorWindow("Paste Failed", "Format was invalid", "Tested only for Excel and CSV formats.\nError Message: \n" + ex.Message);

                //Center dialog to user's window screen
                var desktopWorkingArea = System.Windows.SystemParameters.WorkArea;
                window.Left = desktopWorkingArea.Left + desktopWorkingArea.Right / 2 - window.Width / 2;
                window.Top = desktopWorkingArea.Top + desktopWorkingArea.Bottom / 2 - window.Height / 2;
                //also prevents re-opening in a different location which occurs if nothing is specified when using WPF

                window.ShowDialog();
            }
        }

        #endregion ICommand Members

        private void setSelectedColumnValue(int row, int col, double valueToInsert)
        {
            switch (col)
            {
                case 0:
                    _viewModel.MyScenarios.SelectedScenario.SelectedDetailTab.DataSet[row].Column1 = valueToInsert;
                    break;
                case 1:
                    _viewModel.MyScenarios.SelectedScenario.SelectedDetailTab.DataSet[row].Column2 = valueToInsert;
                    break;
                case 2:
                    _viewModel.MyScenarios.SelectedScenario.SelectedDetailTab.DataSet[row].Column3 = valueToInsert;
                    break;
                case 3:
                    _viewModel.MyScenarios.SelectedScenario.SelectedDetailTab.DataSet[row].Column4 = valueToInsert;
                    break;
                case 4:
                    _viewModel.MyScenarios.SelectedScenario.SelectedDetailTab.DataSet[row].Column5 = valueToInsert;
                    break;
                case 5:
                    _viewModel.MyScenarios.SelectedScenario.SelectedDetailTab.DataSet[row].Column6 = valueToInsert;
                    break;
                case 6:
                    _viewModel.MyScenarios.SelectedScenario.SelectedDetailTab.DataSet[row].Column7 = valueToInsert;
                    break;
                case 7:
                    _viewModel.MyScenarios.SelectedScenario.SelectedDetailTab.DataSet[row].Column8 = valueToInsert;
                    break;
                case 8:
                    _viewModel.MyScenarios.SelectedScenario.SelectedDetailTab.DataSet[row].Column9 = valueToInsert;
                    break;
                case 9:
                    _viewModel.MyScenarios.SelectedScenario.SelectedDetailTab.DataSet[row].Column10 = valueToInsert;
                    break;
            }
        }
    }

    class PasteCommand : ICommand
    {
        public PasteCommand(ProcessYieldViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        private ProcessYieldViewModel _viewModel;

        #region ICommand Members

        public bool CanExecute(object parameter)
        {
            try
            {
                //if selected index is valid
                if (_viewModel.MyScenarios.SelectedScenario.SelectedDetailTab.SelectedRowIndex >= 0)
                    return true;

                return false; //if index is not valid
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
            try
            {
                int row = _viewModel.MyScenarios.SelectedScenario.SelectedDetailTab.SelectedRowIndex;
               // int maxRow = _viewModel.MyScenarios.SelectedScenario.SelectedDetailTab.DataSet.Count;
                int maxRow = row + 1; //Copy only paste only to one row...to confusing to phrase such mechanism to user for now

                if (row < 0)
                    row = 0; //in case it occurs make row default 0

                string copyInfo = System.Windows.Clipboard.GetText();

                while (!String.IsNullOrEmpty(copyInfo) && row < maxRow)
                {
                    string line = "";
                    if (copyInfo.Contains("\n"))
                        line = copyInfo.Substring(0, copyInfo.IndexOf("\n") + 1);
                    else
                        line = copyInfo;


                    int col = 0;
                    while (!String.IsNullOrEmpty(line) && col < StaticConfiguration.maxColumns)
                    {
                        if (line.Contains("\t"))
                        {
                            double valueToInsert = 0;

                            bool replaceValue = true;

                            string value = line.Substring(0, line.IndexOf("\t") + 1);

                            //Use try catch
                            try
                            {
                                valueToInsert = Double.Parse(value);
                            }
                            catch
                            {
                                replaceValue = false; //if parse failed
                            }


                            if (replaceValue) //if valid double value insert into model
                                setSelectedColumnValue(row, col, valueToInsert);

                            line = line.Remove(0, line.IndexOf("\t") + 1);
                        }
                        else if (line.Contains("\r"))
                        {
                            double valueToInsert = 0;

                            bool replaceValue = true;

                            string value = line.Substring(0, line.IndexOf("\r") + 1);

                            //Use try catch
                            try
                            {
                                valueToInsert = Double.Parse(value);
                            }
                            catch
                            {
                                replaceValue = false; //if parse failed
                            }

                            if (replaceValue) //if valid double value insert into model
                                setSelectedColumnValue(row, col, valueToInsert);

                            line = line.Remove(0, line.IndexOf("\r") + 1);
                        }


                        col++;
                    }

                    if (copyInfo.Contains("\n"))
                        copyInfo = copyInfo.Remove(0, copyInfo.IndexOf("\n") + 1); //Remove the line from text copied from clipboard
                    else
                        copyInfo = ""; //Remove everything if not '\n'
                    row++;
                }
            }
            catch (Exception ex)
            {
                ErrorWindow window = new ErrorWindow("Paste Failed", "Format was invalid", "Tested only for Excel and CSV formats.\nError Message: \n" + ex.Message);

                //Center dialog to user's window screen
                var desktopWorkingArea = System.Windows.SystemParameters.WorkArea;
                window.Left = desktopWorkingArea.Left + desktopWorkingArea.Right / 2 - window.Width / 2;
                window.Top = desktopWorkingArea.Top + desktopWorkingArea.Bottom / 2 - window.Height / 2;
                //also prevents re-opening in a different location which occurs if nothing is specified when using WPF

                window.ShowDialog();
            }
        }

        #endregion ICommand Members

        private void setSelectedColumnValue(int row, int col, double valueToInsert)
        {
            switch (col)
            {
                case 0:
                    _viewModel.MyScenarios.SelectedScenario.SelectedDetailTab.DataSet[row].Column1 = valueToInsert;
                    break;
                case 1:
                    _viewModel.MyScenarios.SelectedScenario.SelectedDetailTab.DataSet[row].Column2 = valueToInsert;
                    break;
                case 2:
                    _viewModel.MyScenarios.SelectedScenario.SelectedDetailTab.DataSet[row].Column3 = valueToInsert;
                    break;
                case 3:
                    _viewModel.MyScenarios.SelectedScenario.SelectedDetailTab.DataSet[row].Column4 = valueToInsert;
                    break;
                case 4:
                    _viewModel.MyScenarios.SelectedScenario.SelectedDetailTab.DataSet[row].Column5 = valueToInsert;
                    break;
                case 5:
                    _viewModel.MyScenarios.SelectedScenario.SelectedDetailTab.DataSet[row].Column6 = valueToInsert;
                    break;
                case 6:
                    _viewModel.MyScenarios.SelectedScenario.SelectedDetailTab.DataSet[row].Column7 = valueToInsert;
                    break;
                case 7:
                    _viewModel.MyScenarios.SelectedScenario.SelectedDetailTab.DataSet[row].Column8 = valueToInsert;
                    break;
                case 8:
                    _viewModel.MyScenarios.SelectedScenario.SelectedDetailTab.DataSet[row].Column9 = valueToInsert;
                    break;
                case 9:
                    _viewModel.MyScenarios.SelectedScenario.SelectedDetailTab.DataSet[row].Column10 = valueToInsert;
                    break;
            }
        }
    }

    class AddDataDetailCommand : ICommand
    {
        public AddDataDetailCommand(ProcessYieldViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        private ProcessYieldViewModel _viewModel;

        #region ICommand Members

        public bool CanExecute(object parameter)
        {
            return true; //Always execute
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
                TabDetail mySelectedTab = _viewModel.MyScenarios.SelectedScenario.SelectedDetailTab;

                if (mySelectedTab != null) //should not be Null
                    mySelectedTab.DataSet.Add(new DataRowDetail(mySelectedTab));
            }
            catch (Exception ex)
            {
                ErrorWindow window = new ErrorWindow("Add Data Detail", "Format was invalid", "Error Message: \n" + ex.Message);

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

    class RemoveDataDetailCommand : ICommand
    {
        public RemoveDataDetailCommand(ProcessYieldViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        private ProcessYieldViewModel _viewModel;

        #region ICommand Members

        public bool CanExecute(object parameter)
        {
            try
            {
                //if selected index is valid

                if (_viewModel.MyScenarios.SelectedScenario.SelectedDetailTab.SelectedRowIndex >= 0)
                    return true;

                return false; //if index is not valid
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
            try
            {
                TabDetail mySelectedTab = _viewModel.MyScenarios.SelectedScenario.SelectedDetailTab;

                if (mySelectedTab != null) //should not be Null
                {
                    int selectedIndex = mySelectedTab.SelectedRowIndex;
                    mySelectedTab.DataSet.Remove(mySelectedTab.DataSet[selectedIndex]);

                    for (int c = selectedIndex; c < mySelectedTab.DataSet.Count; c++)
                    {
                        mySelectedTab.DataSet[c].Index = mySelectedTab.DataSet[c].Index - 1;
                    }

                }
            }
            catch (Exception ex)
            {
                ErrorWindow window = new ErrorWindow("Add Data Detail", "Format was invalid", "Error Message: \n" + ex.Message);

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

    class VisualizeCommand : ICommand
    {
        public VisualizeCommand(ProcessYieldViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        private ProcessYieldViewModel _viewModel;

        #region ICommand Members

        public bool CanExecute(object parameter)
        {
            try
            {
                //if valid scenario and yield tab is visible
                if (_viewModel.MyScenarios.SelectedScenario != null && _viewModel.MyScenarios.SelectedScenario.YieldTab.TabVisibility == true)
                {
                    //if not in percentage mode and user already didnt visualize color, disable [TODO] -- Using exisiting variables did the disable so no new code was added, easier to revise
                    if (!_viewModel.MyScenarios.SelectedScenario.YieldTab.HeaderText.Equals("Efficiency/Simple Yield [%]")
                        && _viewModel.MyScenarios.SelectedScenario.YieldTab.Visualize == false) 
                        return false;

                    return true;
                }

                return false; //not a valid option
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

        /* Colors:
         * Transparent - no efficiency representation or 0
         * Red - far away from being efficient (0% < val <= 25%)
         * Blue - in middle (25% < val < 75%)
         * Green - approaching efficiency or at ideal efficiency (75% <= val <= 100%)
         */
        public void Execute(object parameter)
        {
            try
            {
                ScenarioDetail mySelection = _viewModel.MyScenarios.SelectedScenario;
                double maxValue = _viewModel.MyScenarios.SelectedScenario.YieldTab.MinValue;
                double percentage = 0;

                if (_viewModel.MyScenarios.SelectedScenario.YieldTab.Visualize) //If visualize is turned on
                {
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
                }
                else //Turn on visualize and rework the colors
                {
                    _viewModel.MyScenarios.SelectedScenario.MainTab.Visualize = true;
                    _viewModel.MyScenarios.SelectedScenario.PerformanceIndicator1.Visualize = true;
                    _viewModel.MyScenarios.SelectedScenario.PerformanceIndicator2.Visualize = true;
                    _viewModel.MyScenarios.SelectedScenario.PerformanceIndicator3.Visualize = true;
                    _viewModel.MyScenarios.SelectedScenario.PerformanceIndicator4.Visualize = true;
                    _viewModel.MyScenarios.SelectedScenario.PerformanceIndicator5.Visualize = true;
                    _viewModel.MyScenarios.SelectedScenario.PerformanceIndicator6.Visualize = true;
                    _viewModel.MyScenarios.SelectedScenario.PerformanceIndicator7.Visualize = true;
                    _viewModel.MyScenarios.SelectedScenario.PerformanceIndicator8.Visualize = true;
                    _viewModel.MyScenarios.SelectedScenario.PerformanceIndicator9.Visualize = true;
                    _viewModel.MyScenarios.SelectedScenario.PerformanceIndicator10.Visualize = true;
                    _viewModel.MyScenarios.SelectedScenario.YieldTab.Visualize = true;

                    for (int row = 0; row < mySelection.YieldTab.DataSet.Count; row++)
                    {
                        if (!mySelection.YieldTab.DataSet[row].Visualize) //If Visualize == false do calc and then set it to true
                        {
                            if (mySelection.YieldTab.HeaderText.Equals("Efficiency/Simple Yield [%]")) //if in percentage mode
                            {
                                percentage = mySelection.YieldTab.DataSet[row].Column1;
                            }
                            else
                            {
                                if (maxValue > 0)
                                    percentage = mySelection.YieldTab.DataSet[row].Column1 / maxValue * 100;
                                else
                                    percentage = 0;
                            }

                            //Set all tabs' column colors
                            if (percentage == 0)
                            {
                                mySelection.MainTab.DataSet[row].Column1Color = ("Transparent");
                                mySelection.PerformanceIndicator1.DataSet[row].Column1Color = ("Transparent");

                                if (mySelection.PerformanceIndicator2.TabVisibility)
                                    mySelection.PerformanceIndicator2.DataSet[row].Column1Color = ("Transparent");
                                if (mySelection.PerformanceIndicator3.TabVisibility)
                                    mySelection.PerformanceIndicator3.DataSet[row].Column1Color = ("Transparent");
                                if (mySelection.PerformanceIndicator4.TabVisibility)
                                    mySelection.PerformanceIndicator4.DataSet[row].Column1Color = ("Transparent");
                                if (mySelection.PerformanceIndicator5.TabVisibility)
                                    mySelection.PerformanceIndicator5.DataSet[row].Column1Color = ("Transparent");
                                if (mySelection.PerformanceIndicator6.TabVisibility)
                                    mySelection.PerformanceIndicator6.DataSet[row].Column1Color = ("Transparent");
                                if (mySelection.PerformanceIndicator7.TabVisibility)
                                    mySelection.PerformanceIndicator7.DataSet[row].Column1Color = ("Transparent");
                                if (mySelection.PerformanceIndicator8.TabVisibility)
                                    mySelection.PerformanceIndicator8.DataSet[row].Column1Color = ("Transparent");
                                if (mySelection.PerformanceIndicator9.TabVisibility)
                                    mySelection.PerformanceIndicator9.DataSet[row].Column1Color = ("Transparent");
                                if (mySelection.PerformanceIndicator10.TabVisibility)
                                    mySelection.PerformanceIndicator10.DataSet[row].Column1Color = ("Transparent");

                                mySelection.YieldTab.DataSet[row].Column1Color = ("Transparent");
                            }
                            else if (percentage > 0 && percentage <= 25)
                            {
                                //set other all column 1 in tabs color to 'Red'
                                mySelection.MainTab.DataSet[row].Column1Color = ("Red");
                                mySelection.PerformanceIndicator1.DataSet[row].Column1Color = ("Red");

                                if (mySelection.PerformanceIndicator2.TabVisibility)
                                    mySelection.PerformanceIndicator2.DataSet[row].Column1Color = ("Red");
                                if (mySelection.PerformanceIndicator3.TabVisibility)
                                    mySelection.PerformanceIndicator3.DataSet[row].Column1Color = ("Red");
                                if (mySelection.PerformanceIndicator4.TabVisibility)
                                    mySelection.PerformanceIndicator4.DataSet[row].Column1Color = ("Red");
                                if (mySelection.PerformanceIndicator5.TabVisibility)
                                    mySelection.PerformanceIndicator5.DataSet[row].Column1Color = ("Red");
                                if (mySelection.PerformanceIndicator6.TabVisibility)
                                    mySelection.PerformanceIndicator6.DataSet[row].Column1Color = ("Red");
                                if (mySelection.PerformanceIndicator7.TabVisibility)
                                    mySelection.PerformanceIndicator7.DataSet[row].Column1Color = ("Red");
                                if (mySelection.PerformanceIndicator8.TabVisibility)
                                    mySelection.PerformanceIndicator8.DataSet[row].Column1Color = ("Red");
                                if (mySelection.PerformanceIndicator9.TabVisibility)
                                    mySelection.PerformanceIndicator9.DataSet[row].Column1Color = ("Red");
                                if (mySelection.PerformanceIndicator10.TabVisibility)
                                    mySelection.PerformanceIndicator10.DataSet[row].Column1Color = ("Red");

                                mySelection.YieldTab.DataSet[row].Column1Color = ("Red");
                            }
                            else if (percentage < 75)
                            {
                                //set other all column 1 in tabs color to 'Blue'
                                mySelection.MainTab.DataSet[row].Column1Color = ("Blue");
                                mySelection.PerformanceIndicator1.DataSet[row].Column1Color = ("Blue");

                                if (mySelection.PerformanceIndicator2.TabVisibility)
                                    mySelection.PerformanceIndicator2.DataSet[row].Column1Color = ("Blue");
                                if (mySelection.PerformanceIndicator3.TabVisibility)
                                    mySelection.PerformanceIndicator3.DataSet[row].Column1Color = ("Blue");
                                if (mySelection.PerformanceIndicator4.TabVisibility)
                                    mySelection.PerformanceIndicator4.DataSet[row].Column1Color = ("Blue");
                                if (mySelection.PerformanceIndicator5.TabVisibility)
                                    mySelection.PerformanceIndicator5.DataSet[row].Column1Color = ("Blue");
                                if (mySelection.PerformanceIndicator6.TabVisibility)
                                    mySelection.PerformanceIndicator6.DataSet[row].Column1Color = ("Blue");
                                if (mySelection.PerformanceIndicator7.TabVisibility)
                                    mySelection.PerformanceIndicator7.DataSet[row].Column1Color = ("Blue");
                                if (mySelection.PerformanceIndicator8.TabVisibility)
                                    mySelection.PerformanceIndicator8.DataSet[row].Column1Color = ("Blue");
                                if (mySelection.PerformanceIndicator9.TabVisibility)
                                    mySelection.PerformanceIndicator9.DataSet[row].Column1Color = ("Blue");
                                if (mySelection.PerformanceIndicator10.TabVisibility)
                                    mySelection.PerformanceIndicator10.DataSet[row].Column1Color = ("Blue");

                                mySelection.YieldTab.DataSet[row].Column1Color = ("Blue");
                            }
                            else
                            {
                                //set other all column 1 in tabs color to 'Green'
                                mySelection.MainTab.DataSet[row].Column1Color = ("Green");
                                mySelection.PerformanceIndicator1.DataSet[row].Column1Color = ("Green");

                                if (mySelection.PerformanceIndicator2.TabVisibility)
                                    mySelection.PerformanceIndicator2.DataSet[row].Column1Color = ("Green");
                                if (mySelection.PerformanceIndicator3.TabVisibility)
                                    mySelection.PerformanceIndicator3.DataSet[row].Column1Color = ("Green");
                                if (mySelection.PerformanceIndicator4.TabVisibility)
                                    mySelection.PerformanceIndicator4.DataSet[row].Column1Color = ("Green");
                                if (mySelection.PerformanceIndicator5.TabVisibility)
                                    mySelection.PerformanceIndicator5.DataSet[row].Column1Color = ("Green");
                                if (mySelection.PerformanceIndicator6.TabVisibility)
                                    mySelection.PerformanceIndicator6.DataSet[row].Column1Color = ("Green");
                                if (mySelection.PerformanceIndicator7.TabVisibility)
                                    mySelection.PerformanceIndicator7.DataSet[row].Column1Color = ("Green");
                                if (mySelection.PerformanceIndicator8.TabVisibility)
                                    mySelection.PerformanceIndicator8.DataSet[row].Column1Color = ("Green");
                                if (mySelection.PerformanceIndicator9.TabVisibility)
                                    mySelection.PerformanceIndicator9.DataSet[row].Column1Color = ("Green");
                                if (mySelection.PerformanceIndicator10.TabVisibility)
                                    mySelection.PerformanceIndicator10.DataSet[row].Column1Color = ("Green");

                                mySelection.YieldTab.DataSet[row].Column1Color = ("Green");
                            }

                            if (mySelection.YieldTab.HeaderText.Equals("Efficiency/Simple Yield [%]")) //if in percentage mode
                            {
                                percentage = mySelection.YieldTab.DataSet[row].Column2;
                            }
                            else
                            {
                                percentage = mySelection.YieldTab.DataSet[row].Column2 / maxValue * 100;
                            }

                            //Set all tabs' column colors
                            if (percentage == 0)
                            {
                                mySelection.MainTab.DataSet[row].Column2Color = ("Transparent");
                                mySelection.PerformanceIndicator1.DataSet[row].Column2Color = ("Transparent");

                                if (mySelection.PerformanceIndicator2.TabVisibility)
                                    mySelection.PerformanceIndicator2.DataSet[row].Column2Color = ("Transparent");
                                if (mySelection.PerformanceIndicator3.TabVisibility)
                                    mySelection.PerformanceIndicator3.DataSet[row].Column2Color = ("Transparent");
                                if (mySelection.PerformanceIndicator4.TabVisibility)
                                    mySelection.PerformanceIndicator4.DataSet[row].Column2Color = ("Transparent");
                                if (mySelection.PerformanceIndicator5.TabVisibility)
                                    mySelection.PerformanceIndicator5.DataSet[row].Column2Color = ("Transparent");
                                if (mySelection.PerformanceIndicator6.TabVisibility)
                                    mySelection.PerformanceIndicator6.DataSet[row].Column2Color = ("Transparent");
                                if (mySelection.PerformanceIndicator7.TabVisibility)
                                    mySelection.PerformanceIndicator7.DataSet[row].Column2Color = ("Transparent");
                                if (mySelection.PerformanceIndicator8.TabVisibility)
                                    mySelection.PerformanceIndicator8.DataSet[row].Column2Color = ("Transparent");
                                if (mySelection.PerformanceIndicator9.TabVisibility)
                                    mySelection.PerformanceIndicator9.DataSet[row].Column2Color = ("Transparent");
                                if (mySelection.PerformanceIndicator10.TabVisibility)
                                    mySelection.PerformanceIndicator10.DataSet[row].Column2Color = ("Transparent");

                                mySelection.YieldTab.DataSet[row].Column2Color = ("Transparent");
                            }
                            //Set all tabs' column colors
                            else if (percentage > 0 && percentage <= 25)
                            {
                                //set other all column 2 in tabs color to 'Red'
                                mySelection.MainTab.DataSet[row].Column2Color = ("Red");
                                mySelection.PerformanceIndicator1.DataSet[row].Column2Color = ("Red");

                                if (mySelection.PerformanceIndicator2.TabVisibility)
                                    mySelection.PerformanceIndicator2.DataSet[row].Column2Color = ("Red");
                                if (mySelection.PerformanceIndicator3.TabVisibility)
                                    mySelection.PerformanceIndicator3.DataSet[row].Column2Color = ("Red");
                                if (mySelection.PerformanceIndicator4.TabVisibility)
                                    mySelection.PerformanceIndicator4.DataSet[row].Column2Color = ("Red");
                                if (mySelection.PerformanceIndicator5.TabVisibility)
                                    mySelection.PerformanceIndicator5.DataSet[row].Column2Color = ("Red");
                                if (mySelection.PerformanceIndicator6.TabVisibility)
                                    mySelection.PerformanceIndicator6.DataSet[row].Column2Color = ("Red");
                                if (mySelection.PerformanceIndicator7.TabVisibility)
                                    mySelection.PerformanceIndicator7.DataSet[row].Column2Color = ("Red");
                                if (mySelection.PerformanceIndicator8.TabVisibility)
                                    mySelection.PerformanceIndicator8.DataSet[row].Column2Color = ("Red");
                                if (mySelection.PerformanceIndicator9.TabVisibility)
                                    mySelection.PerformanceIndicator9.DataSet[row].Column2Color = ("Red");
                                if (mySelection.PerformanceIndicator10.TabVisibility)
                                    mySelection.PerformanceIndicator10.DataSet[row].Column2Color = ("Red");

                                mySelection.YieldTab.DataSet[row].Column2Color = ("Red");
                            }
                            else if (percentage < 75)
                            {
                                //set other all column 2 in tabs color to 'Blue'
                                mySelection.MainTab.DataSet[row].Column2Color = ("Blue");
                                mySelection.PerformanceIndicator1.DataSet[row].Column2Color = ("Blue");

                                if (mySelection.PerformanceIndicator2.TabVisibility)
                                    mySelection.PerformanceIndicator2.DataSet[row].Column2Color = ("Blue");
                                if (mySelection.PerformanceIndicator3.TabVisibility)
                                    mySelection.PerformanceIndicator3.DataSet[row].Column2Color = ("Blue");
                                if (mySelection.PerformanceIndicator4.TabVisibility)
                                    mySelection.PerformanceIndicator4.DataSet[row].Column2Color = ("Blue");
                                if (mySelection.PerformanceIndicator5.TabVisibility)
                                    mySelection.PerformanceIndicator5.DataSet[row].Column2Color = ("Blue");
                                if (mySelection.PerformanceIndicator6.TabVisibility)
                                    mySelection.PerformanceIndicator6.DataSet[row].Column2Color = ("Blue");
                                if (mySelection.PerformanceIndicator7.TabVisibility)
                                    mySelection.PerformanceIndicator7.DataSet[row].Column2Color = ("Blue");
                                if (mySelection.PerformanceIndicator8.TabVisibility)
                                    mySelection.PerformanceIndicator8.DataSet[row].Column2Color = ("Blue");
                                if (mySelection.PerformanceIndicator9.TabVisibility)
                                    mySelection.PerformanceIndicator9.DataSet[row].Column2Color = ("Blue");
                                if (mySelection.PerformanceIndicator10.TabVisibility)
                                    mySelection.PerformanceIndicator10.DataSet[row].Column2Color = ("Blue");

                                mySelection.YieldTab.DataSet[row].Column2Color = ("Blue");
                            }
                            else
                            {
                                //set other all column 21 in tabs color to 'Green'
                                mySelection.MainTab.DataSet[row].Column2Color = ("Green");
                                mySelection.PerformanceIndicator1.DataSet[row].Column2Color = ("Green");

                                if (mySelection.PerformanceIndicator2.TabVisibility)
                                    mySelection.PerformanceIndicator2.DataSet[row].Column2Color = ("Green");
                                if (mySelection.PerformanceIndicator3.TabVisibility)
                                    mySelection.PerformanceIndicator3.DataSet[row].Column2Color = ("Green");
                                if (mySelection.PerformanceIndicator4.TabVisibility)
                                    mySelection.PerformanceIndicator4.DataSet[row].Column2Color = ("Green");
                                if (mySelection.PerformanceIndicator5.TabVisibility)
                                    mySelection.PerformanceIndicator5.DataSet[row].Column2Color = ("Green");
                                if (mySelection.PerformanceIndicator6.TabVisibility)
                                    mySelection.PerformanceIndicator6.DataSet[row].Column2Color = ("Green");
                                if (mySelection.PerformanceIndicator7.TabVisibility)
                                    mySelection.PerformanceIndicator7.DataSet[row].Column2Color = ("Green");
                                if (mySelection.PerformanceIndicator8.TabVisibility)
                                    mySelection.PerformanceIndicator8.DataSet[row].Column2Color = ("Green");
                                if (mySelection.PerformanceIndicator9.TabVisibility)
                                    mySelection.PerformanceIndicator9.DataSet[row].Column2Color = ("Green");
                                if (mySelection.PerformanceIndicator10.TabVisibility)
                                    mySelection.PerformanceIndicator10.DataSet[row].Column2Color = ("Green");

                                mySelection.YieldTab.DataSet[row].Column2Color = ("Green");
                            }

                            if (mySelection.YieldTab.HeaderText.Equals("Efficiency/Simple Yield [%]")) //if in percentage mode
                            {
                                percentage = mySelection.YieldTab.DataSet[row].Column3;
                            }
                            else
                            {
                                percentage = mySelection.YieldTab.DataSet[row].Column3 / maxValue * 100;
                            }

                            //Set all tabs' column colors
                            if (percentage == 0)
                            {
                                mySelection.MainTab.DataSet[row].Column3Color = ("Transparent");
                                mySelection.PerformanceIndicator1.DataSet[row].Column3Color = ("Transparent");

                                if (mySelection.PerformanceIndicator2.TabVisibility)
                                    mySelection.PerformanceIndicator2.DataSet[row].Column3Color = ("Transparent");
                                if (mySelection.PerformanceIndicator3.TabVisibility)
                                    mySelection.PerformanceIndicator3.DataSet[row].Column3Color = ("Transparent");
                                if (mySelection.PerformanceIndicator4.TabVisibility)
                                    mySelection.PerformanceIndicator4.DataSet[row].Column3Color = ("Transparent");
                                if (mySelection.PerformanceIndicator5.TabVisibility)
                                    mySelection.PerformanceIndicator5.DataSet[row].Column3Color = ("Transparent");
                                if (mySelection.PerformanceIndicator6.TabVisibility)
                                    mySelection.PerformanceIndicator6.DataSet[row].Column3Color = ("Transparent");
                                if (mySelection.PerformanceIndicator7.TabVisibility)
                                    mySelection.PerformanceIndicator7.DataSet[row].Column3Color = ("Transparent");
                                if (mySelection.PerformanceIndicator8.TabVisibility)
                                    mySelection.PerformanceIndicator8.DataSet[row].Column3Color = ("Transparent");
                                if (mySelection.PerformanceIndicator9.TabVisibility)
                                    mySelection.PerformanceIndicator9.DataSet[row].Column3Color = ("Transparent");
                                if (mySelection.PerformanceIndicator10.TabVisibility)
                                    mySelection.PerformanceIndicator10.DataSet[row].Column3Color = ("Transparent");

                                mySelection.YieldTab.DataSet[row].Column3Color = ("Transparent");
                            }
                            //Set all tabs' column colors
                            else if (percentage > 0 && percentage <= 25)
                            {
                                //set other all column 3 in tabs color to 'Red'
                                mySelection.MainTab.DataSet[row].Column3Color = ("Red");
                                mySelection.PerformanceIndicator1.DataSet[row].Column3Color = ("Red");

                                if (mySelection.PerformanceIndicator2.TabVisibility)
                                    mySelection.PerformanceIndicator2.DataSet[row].Column3Color = ("Red");
                                if (mySelection.PerformanceIndicator3.TabVisibility)
                                    mySelection.PerformanceIndicator3.DataSet[row].Column3Color = ("Red");
                                if (mySelection.PerformanceIndicator4.TabVisibility)
                                    mySelection.PerformanceIndicator4.DataSet[row].Column3Color = ("Red");
                                if (mySelection.PerformanceIndicator5.TabVisibility)
                                    mySelection.PerformanceIndicator5.DataSet[row].Column3Color = ("Red");
                                if (mySelection.PerformanceIndicator6.TabVisibility)
                                    mySelection.PerformanceIndicator6.DataSet[row].Column3Color = ("Red");
                                if (mySelection.PerformanceIndicator7.TabVisibility)
                                    mySelection.PerformanceIndicator7.DataSet[row].Column3Color = ("Red");
                                if (mySelection.PerformanceIndicator8.TabVisibility)
                                    mySelection.PerformanceIndicator8.DataSet[row].Column3Color = ("Red");
                                if (mySelection.PerformanceIndicator9.TabVisibility)
                                    mySelection.PerformanceIndicator9.DataSet[row].Column3Color = ("Red");
                                if (mySelection.PerformanceIndicator10.TabVisibility)
                                    mySelection.PerformanceIndicator10.DataSet[row].Column3Color = ("Red");

                                mySelection.YieldTab.DataSet[row].Column3Color = ("Red");
                            }
                            else if (percentage < 75)
                            {
                                //set other all column 3 in tabs color to 'Blue'
                                mySelection.MainTab.DataSet[row].Column3Color = ("Blue");
                                mySelection.PerformanceIndicator1.DataSet[row].Column3Color = ("Blue");

                                if (mySelection.PerformanceIndicator2.TabVisibility)
                                    mySelection.PerformanceIndicator2.DataSet[row].Column3Color = ("Blue");
                                if (mySelection.PerformanceIndicator3.TabVisibility)
                                    mySelection.PerformanceIndicator3.DataSet[row].Column3Color = ("Blue");
                                if (mySelection.PerformanceIndicator4.TabVisibility)
                                    mySelection.PerformanceIndicator4.DataSet[row].Column3Color = ("Blue");
                                if (mySelection.PerformanceIndicator5.TabVisibility)
                                    mySelection.PerformanceIndicator5.DataSet[row].Column3Color = ("Blue");
                                if (mySelection.PerformanceIndicator6.TabVisibility)
                                    mySelection.PerformanceIndicator6.DataSet[row].Column3Color = ("Blue");
                                if (mySelection.PerformanceIndicator7.TabVisibility)
                                    mySelection.PerformanceIndicator7.DataSet[row].Column3Color = ("Blue");
                                if (mySelection.PerformanceIndicator8.TabVisibility)
                                    mySelection.PerformanceIndicator8.DataSet[row].Column3Color = ("Blue");
                                if (mySelection.PerformanceIndicator9.TabVisibility)
                                    mySelection.PerformanceIndicator9.DataSet[row].Column3Color = ("Blue");
                                if (mySelection.PerformanceIndicator10.TabVisibility)
                                    mySelection.PerformanceIndicator10.DataSet[row].Column3Color = ("Blue");

                                mySelection.YieldTab.DataSet[row].Column3Color = ("Blue");
                            }
                            else
                            {
                                //set other all column 3 in tabs color to 'Green'
                                mySelection.MainTab.DataSet[row].Column3Color = ("Green");
                                mySelection.PerformanceIndicator1.DataSet[row].Column3Color = ("Green");

                                if (mySelection.PerformanceIndicator2.TabVisibility)
                                    mySelection.PerformanceIndicator2.DataSet[row].Column3Color = ("Green");
                                if (mySelection.PerformanceIndicator3.TabVisibility)
                                    mySelection.PerformanceIndicator3.DataSet[row].Column3Color = ("Green");
                                if (mySelection.PerformanceIndicator4.TabVisibility)
                                    mySelection.PerformanceIndicator4.DataSet[row].Column3Color = ("Green");
                                if (mySelection.PerformanceIndicator5.TabVisibility)
                                    mySelection.PerformanceIndicator5.DataSet[row].Column3Color = ("Green");
                                if (mySelection.PerformanceIndicator6.TabVisibility)
                                    mySelection.PerformanceIndicator6.DataSet[row].Column3Color = ("Green");
                                if (mySelection.PerformanceIndicator7.TabVisibility)
                                    mySelection.PerformanceIndicator7.DataSet[row].Column3Color = ("Green");
                                if (mySelection.PerformanceIndicator8.TabVisibility)
                                    mySelection.PerformanceIndicator8.DataSet[row].Column3Color = ("Green");
                                if (mySelection.PerformanceIndicator9.TabVisibility)
                                    mySelection.PerformanceIndicator9.DataSet[row].Column3Color = ("Green");
                                if (mySelection.PerformanceIndicator10.TabVisibility)
                                    mySelection.PerformanceIndicator10.DataSet[row].Column3Color = ("Green");

                                mySelection.YieldTab.DataSet[row].Column3Color = ("Green");
                            }

                            if (mySelection.YieldTab.HeaderText.Equals("Efficiency/Simple Yield [%]")) //if in percentage mode
                            {
                                percentage = mySelection.YieldTab.DataSet[row].Column4;
                            }
                            else
                            {
                                percentage = mySelection.YieldTab.DataSet[row].Column4 / maxValue * 100;
                            }

                            //Set all tabs' column colors
                            if (percentage == 0)
                            {
                                mySelection.MainTab.DataSet[row].Column4Color = ("Transparent");
                                mySelection.PerformanceIndicator1.DataSet[row].Column4Color = ("Transparent");

                                if (mySelection.PerformanceIndicator2.TabVisibility)
                                    mySelection.PerformanceIndicator2.DataSet[row].Column4Color = ("Transparent");
                                if (mySelection.PerformanceIndicator3.TabVisibility)
                                    mySelection.PerformanceIndicator3.DataSet[row].Column4Color = ("Transparent");
                                if (mySelection.PerformanceIndicator4.TabVisibility)
                                    mySelection.PerformanceIndicator4.DataSet[row].Column4Color = ("Transparent");
                                if (mySelection.PerformanceIndicator5.TabVisibility)
                                    mySelection.PerformanceIndicator5.DataSet[row].Column4Color = ("Transparent");
                                if (mySelection.PerformanceIndicator6.TabVisibility)
                                    mySelection.PerformanceIndicator6.DataSet[row].Column4Color = ("Transparent");
                                if (mySelection.PerformanceIndicator7.TabVisibility)
                                    mySelection.PerformanceIndicator7.DataSet[row].Column4Color = ("Transparent");
                                if (mySelection.PerformanceIndicator8.TabVisibility)
                                    mySelection.PerformanceIndicator8.DataSet[row].Column4Color = ("Transparent");
                                if (mySelection.PerformanceIndicator9.TabVisibility)
                                    mySelection.PerformanceIndicator9.DataSet[row].Column4Color = ("Transparent");
                                if (mySelection.PerformanceIndicator10.TabVisibility)
                                    mySelection.PerformanceIndicator10.DataSet[row].Column4Color = ("Transparent");

                                mySelection.YieldTab.DataSet[row].Column4Color = ("Transparent");
                            }
                            //Set all tabs' column colors
                            else if (percentage > 0 && percentage <= 25)
                            {
                                //set other all column 4 in tabs color to 'Red'
                                mySelection.MainTab.DataSet[row].Column4Color = ("Red");
                                mySelection.PerformanceIndicator1.DataSet[row].Column4Color = ("Red");

                                if (mySelection.PerformanceIndicator2.TabVisibility)
                                    mySelection.PerformanceIndicator2.DataSet[row].Column4Color = ("Red");
                                if (mySelection.PerformanceIndicator3.TabVisibility)
                                    mySelection.PerformanceIndicator3.DataSet[row].Column4Color = ("Red");
                                if (mySelection.PerformanceIndicator4.TabVisibility)
                                    mySelection.PerformanceIndicator4.DataSet[row].Column4Color = ("Red");
                                if (mySelection.PerformanceIndicator5.TabVisibility)
                                    mySelection.PerformanceIndicator5.DataSet[row].Column4Color = ("Red");
                                if (mySelection.PerformanceIndicator6.TabVisibility)
                                    mySelection.PerformanceIndicator6.DataSet[row].Column4Color = ("Red");
                                if (mySelection.PerformanceIndicator7.TabVisibility)
                                    mySelection.PerformanceIndicator7.DataSet[row].Column4Color = ("Red");
                                if (mySelection.PerformanceIndicator8.TabVisibility)
                                    mySelection.PerformanceIndicator8.DataSet[row].Column4Color = ("Red");
                                if (mySelection.PerformanceIndicator9.TabVisibility)
                                    mySelection.PerformanceIndicator9.DataSet[row].Column4Color = ("Red");
                                if (mySelection.PerformanceIndicator10.TabVisibility)
                                    mySelection.PerformanceIndicator10.DataSet[row].Column4Color = ("Red");

                                mySelection.YieldTab.DataSet[row].Column4Color = ("Red");
                            }
                            else if (percentage < 75)
                            {
                                //set other all column 4 in tabs color to 'Blue'
                                mySelection.MainTab.DataSet[row].Column4Color = ("Blue");
                                mySelection.PerformanceIndicator1.DataSet[row].Column4Color = ("Blue");

                                if (mySelection.PerformanceIndicator2.TabVisibility)
                                    mySelection.PerformanceIndicator2.DataSet[row].Column4Color = ("Blue");
                                if (mySelection.PerformanceIndicator3.TabVisibility)
                                    mySelection.PerformanceIndicator3.DataSet[row].Column4Color = ("Blue");
                                if (mySelection.PerformanceIndicator4.TabVisibility)
                                    mySelection.PerformanceIndicator4.DataSet[row].Column4Color = ("Blue");
                                if (mySelection.PerformanceIndicator5.TabVisibility)
                                    mySelection.PerformanceIndicator5.DataSet[row].Column4Color = ("Blue");
                                if (mySelection.PerformanceIndicator6.TabVisibility)
                                    mySelection.PerformanceIndicator6.DataSet[row].Column4Color = ("Blue");
                                if (mySelection.PerformanceIndicator7.TabVisibility)
                                    mySelection.PerformanceIndicator7.DataSet[row].Column4Color = ("Blue");
                                if (mySelection.PerformanceIndicator8.TabVisibility)
                                    mySelection.PerformanceIndicator8.DataSet[row].Column4Color = ("Blue");
                                if (mySelection.PerformanceIndicator9.TabVisibility)
                                    mySelection.PerformanceIndicator9.DataSet[row].Column4Color = ("Blue");
                                if (mySelection.PerformanceIndicator10.TabVisibility)
                                    mySelection.PerformanceIndicator10.DataSet[row].Column4Color = ("Blue");

                                mySelection.YieldTab.DataSet[row].Column4Color = ("Blue");
                            }
                            else
                            {
                                //set other all column 4 in tabs color to 'Green'
                                mySelection.MainTab.DataSet[row].Column4Color = ("Green");
                                mySelection.PerformanceIndicator1.DataSet[row].Column4Color = ("Green");

                                if (mySelection.PerformanceIndicator2.TabVisibility)
                                    mySelection.PerformanceIndicator2.DataSet[row].Column4Color = ("Green");
                                if (mySelection.PerformanceIndicator3.TabVisibility)
                                    mySelection.PerformanceIndicator3.DataSet[row].Column4Color = ("Green");
                                if (mySelection.PerformanceIndicator4.TabVisibility)
                                    mySelection.PerformanceIndicator4.DataSet[row].Column4Color = ("Green");
                                if (mySelection.PerformanceIndicator5.TabVisibility)
                                    mySelection.PerformanceIndicator5.DataSet[row].Column4Color = ("Green");
                                if (mySelection.PerformanceIndicator6.TabVisibility)
                                    mySelection.PerformanceIndicator6.DataSet[row].Column4Color = ("Green");
                                if (mySelection.PerformanceIndicator7.TabVisibility)
                                    mySelection.PerformanceIndicator7.DataSet[row].Column4Color = ("Green");
                                if (mySelection.PerformanceIndicator8.TabVisibility)
                                    mySelection.PerformanceIndicator8.DataSet[row].Column4Color = ("Green");
                                if (mySelection.PerformanceIndicator9.TabVisibility)
                                    mySelection.PerformanceIndicator9.DataSet[row].Column4Color = ("Green");
                                if (mySelection.PerformanceIndicator10.TabVisibility)
                                    mySelection.PerformanceIndicator10.DataSet[row].Column4Color = ("Green");

                                mySelection.YieldTab.DataSet[row].Column4Color = ("Green");
                            }

                            if (mySelection.YieldTab.HeaderText.Equals("Efficiency/Simple Yield [%]")) //if in percentage mode
                            {
                                percentage = mySelection.YieldTab.DataSet[row].Column5;
                            }
                            else
                            {
                                percentage = mySelection.YieldTab.DataSet[row].Column5 / maxValue * 100;
                            }

                            //Set all tabs' column colors
                            if (percentage == 0)
                            {
                                mySelection.MainTab.DataSet[row].Column5Color = ("Transparent");
                                mySelection.PerformanceIndicator1.DataSet[row].Column5Color = ("Transparent");

                                if (mySelection.PerformanceIndicator2.TabVisibility)
                                    mySelection.PerformanceIndicator2.DataSet[row].Column5Color = ("Transparent");
                                if (mySelection.PerformanceIndicator3.TabVisibility)
                                    mySelection.PerformanceIndicator3.DataSet[row].Column5Color = ("Transparent");
                                if (mySelection.PerformanceIndicator4.TabVisibility)
                                    mySelection.PerformanceIndicator4.DataSet[row].Column5Color = ("Transparent");
                                if (mySelection.PerformanceIndicator5.TabVisibility)
                                    mySelection.PerformanceIndicator5.DataSet[row].Column5Color = ("Transparent");
                                if (mySelection.PerformanceIndicator6.TabVisibility)
                                    mySelection.PerformanceIndicator6.DataSet[row].Column5Color = ("Transparent");
                                if (mySelection.PerformanceIndicator7.TabVisibility)
                                    mySelection.PerformanceIndicator7.DataSet[row].Column5Color = ("Transparent");
                                if (mySelection.PerformanceIndicator8.TabVisibility)
                                    mySelection.PerformanceIndicator8.DataSet[row].Column5Color = ("Transparent");
                                if (mySelection.PerformanceIndicator9.TabVisibility)
                                    mySelection.PerformanceIndicator9.DataSet[row].Column5Color = ("Transparent");
                                if (mySelection.PerformanceIndicator10.TabVisibility)
                                    mySelection.PerformanceIndicator10.DataSet[row].Column5Color = ("Transparent");

                                mySelection.YieldTab.DataSet[row].Column5Color = ("Transparent");
                            }
                            //Set all tabs' column colors
                            else if (percentage > 0 && percentage <= 25)
                            {
                                mySelection.MainTab.DataSet[row].Column5Color = ("Red");
                                mySelection.PerformanceIndicator1.DataSet[row].Column5Color = ("Red");

                                if (mySelection.PerformanceIndicator2.TabVisibility)
                                    mySelection.PerformanceIndicator2.DataSet[row].Column5Color = ("Red");
                                if (mySelection.PerformanceIndicator3.TabVisibility)
                                    mySelection.PerformanceIndicator3.DataSet[row].Column5Color = ("Red");
                                if (mySelection.PerformanceIndicator4.TabVisibility)
                                    mySelection.PerformanceIndicator4.DataSet[row].Column5Color = ("Red");
                                if (mySelection.PerformanceIndicator5.TabVisibility)
                                    mySelection.PerformanceIndicator5.DataSet[row].Column5Color = ("Red");
                                if (mySelection.PerformanceIndicator6.TabVisibility)
                                    mySelection.PerformanceIndicator6.DataSet[row].Column5Color = ("Red");
                                if (mySelection.PerformanceIndicator7.TabVisibility)
                                    mySelection.PerformanceIndicator7.DataSet[row].Column5Color = ("Red");
                                if (mySelection.PerformanceIndicator8.TabVisibility)
                                    mySelection.PerformanceIndicator8.DataSet[row].Column5Color = ("Red");
                                if (mySelection.PerformanceIndicator9.TabVisibility)
                                    mySelection.PerformanceIndicator9.DataSet[row].Column5Color = ("Red");
                                if (mySelection.PerformanceIndicator10.TabVisibility)
                                    mySelection.PerformanceIndicator10.DataSet[row].Column5Color = ("Red");

                                mySelection.YieldTab.DataSet[row].Column5Color = ("Red");
                            }
                            else if (percentage < 75)
                            {
                                mySelection.MainTab.DataSet[row].Column5Color = ("Blue");
                                mySelection.PerformanceIndicator1.DataSet[row].Column5Color = ("Blue");

                                if (mySelection.PerformanceIndicator2.TabVisibility)
                                    mySelection.PerformanceIndicator2.DataSet[row].Column5Color = ("Blue");
                                if (mySelection.PerformanceIndicator3.TabVisibility)
                                    mySelection.PerformanceIndicator3.DataSet[row].Column5Color = ("Blue");
                                if (mySelection.PerformanceIndicator4.TabVisibility)
                                    mySelection.PerformanceIndicator4.DataSet[row].Column5Color = ("Blue");
                                if (mySelection.PerformanceIndicator5.TabVisibility)
                                    mySelection.PerformanceIndicator5.DataSet[row].Column5Color = ("Blue");
                                if (mySelection.PerformanceIndicator6.TabVisibility)
                                    mySelection.PerformanceIndicator6.DataSet[row].Column5Color = ("Blue");
                                if (mySelection.PerformanceIndicator7.TabVisibility)
                                    mySelection.PerformanceIndicator7.DataSet[row].Column5Color = ("Blue");
                                if (mySelection.PerformanceIndicator8.TabVisibility)
                                    mySelection.PerformanceIndicator8.DataSet[row].Column5Color = ("Blue");
                                if (mySelection.PerformanceIndicator9.TabVisibility)
                                    mySelection.PerformanceIndicator9.DataSet[row].Column5Color = ("Blue");
                                if (mySelection.PerformanceIndicator10.TabVisibility)
                                    mySelection.PerformanceIndicator10.DataSet[row].Column5Color = ("Blue");

                                mySelection.YieldTab.DataSet[row].Column5Color = ("Blue");
                            }
                            else
                            {
                                mySelection.MainTab.DataSet[row].Column5Color = ("Green");
                                mySelection.PerformanceIndicator1.DataSet[row].Column5Color = ("Green");

                                if (mySelection.PerformanceIndicator2.TabVisibility)
                                    mySelection.PerformanceIndicator2.DataSet[row].Column5Color = ("Green");
                                if (mySelection.PerformanceIndicator3.TabVisibility)
                                    mySelection.PerformanceIndicator3.DataSet[row].Column5Color = ("Green");
                                if (mySelection.PerformanceIndicator4.TabVisibility)
                                    mySelection.PerformanceIndicator4.DataSet[row].Column5Color = ("Green");
                                if (mySelection.PerformanceIndicator5.TabVisibility)
                                    mySelection.PerformanceIndicator5.DataSet[row].Column5Color = ("Green");
                                if (mySelection.PerformanceIndicator6.TabVisibility)
                                    mySelection.PerformanceIndicator6.DataSet[row].Column5Color = ("Green");
                                if (mySelection.PerformanceIndicator7.TabVisibility)
                                    mySelection.PerformanceIndicator7.DataSet[row].Column5Color = ("Green");
                                if (mySelection.PerformanceIndicator8.TabVisibility)
                                    mySelection.PerformanceIndicator8.DataSet[row].Column5Color = ("Green");
                                if (mySelection.PerformanceIndicator9.TabVisibility)
                                    mySelection.PerformanceIndicator9.DataSet[row].Column5Color = ("Green");
                                if (mySelection.PerformanceIndicator10.TabVisibility)
                                    mySelection.PerformanceIndicator10.DataSet[row].Column5Color = ("Green");

                                mySelection.YieldTab.DataSet[row].Column5Color = ("Green");
                            }

                            if (mySelection.YieldTab.HeaderText.Equals("Efficiency/Simple Yield [%]")) //if in percentage mode
                            {
                                percentage = mySelection.YieldTab.DataSet[row].Column6;
                            }
                            else
                            {
                                percentage = mySelection.YieldTab.DataSet[row].Column6 / maxValue * 100;
                            }

                            //Set all tabs' column colors
                            if (percentage == 0)
                            {
                                mySelection.MainTab.DataSet[row].Column6Color = ("Transparent");
                                mySelection.PerformanceIndicator1.DataSet[row].Column6Color = ("Transparent");

                                if (mySelection.PerformanceIndicator2.TabVisibility)
                                    mySelection.PerformanceIndicator2.DataSet[row].Column6Color = ("Transparent");
                                if (mySelection.PerformanceIndicator3.TabVisibility)
                                    mySelection.PerformanceIndicator3.DataSet[row].Column6Color = ("Transparent");
                                if (mySelection.PerformanceIndicator4.TabVisibility)
                                    mySelection.PerformanceIndicator4.DataSet[row].Column6Color = ("Transparent");
                                if (mySelection.PerformanceIndicator5.TabVisibility)
                                    mySelection.PerformanceIndicator5.DataSet[row].Column6Color = ("Transparent");
                                if (mySelection.PerformanceIndicator6.TabVisibility)
                                    mySelection.PerformanceIndicator6.DataSet[row].Column6Color = ("Transparent");
                                if (mySelection.PerformanceIndicator7.TabVisibility)
                                    mySelection.PerformanceIndicator7.DataSet[row].Column6Color = ("Transparent");
                                if (mySelection.PerformanceIndicator8.TabVisibility)
                                    mySelection.PerformanceIndicator8.DataSet[row].Column6Color = ("Transparent");
                                if (mySelection.PerformanceIndicator9.TabVisibility)
                                    mySelection.PerformanceIndicator9.DataSet[row].Column6Color = ("Transparent");
                                if (mySelection.PerformanceIndicator10.TabVisibility)
                                    mySelection.PerformanceIndicator10.DataSet[row].Column6Color = ("Transparent");

                                mySelection.YieldTab.DataSet[row].Column6Color = ("Transparent");
                            }
                            //Set all tabs' column colors
                            else if (percentage > 0 && percentage <= 25)
                            {
                                mySelection.MainTab.DataSet[row].Column6Color = ("Red");
                                mySelection.PerformanceIndicator1.DataSet[row].Column6Color = ("Red");

                                if (mySelection.PerformanceIndicator2.TabVisibility)
                                    mySelection.PerformanceIndicator2.DataSet[row].Column6Color = ("Red");
                                if (mySelection.PerformanceIndicator3.TabVisibility)
                                    mySelection.PerformanceIndicator3.DataSet[row].Column6Color = ("Red");
                                if (mySelection.PerformanceIndicator4.TabVisibility)
                                    mySelection.PerformanceIndicator4.DataSet[row].Column6Color = ("Red");
                                if (mySelection.PerformanceIndicator5.TabVisibility)
                                    mySelection.PerformanceIndicator5.DataSet[row].Column6Color = ("Red");
                                if (mySelection.PerformanceIndicator6.TabVisibility)
                                    mySelection.PerformanceIndicator6.DataSet[row].Column6Color = ("Red");
                                if (mySelection.PerformanceIndicator7.TabVisibility)
                                    mySelection.PerformanceIndicator7.DataSet[row].Column6Color = ("Red");
                                if (mySelection.PerformanceIndicator8.TabVisibility)
                                    mySelection.PerformanceIndicator8.DataSet[row].Column6Color = ("Red");
                                if (mySelection.PerformanceIndicator9.TabVisibility)
                                    mySelection.PerformanceIndicator9.DataSet[row].Column6Color = ("Red");
                                if (mySelection.PerformanceIndicator10.TabVisibility)
                                    mySelection.PerformanceIndicator10.DataSet[row].Column6Color = ("Red");

                                mySelection.YieldTab.DataSet[row].Column6Color = ("Red");
                            }
                            else if (percentage < 75)
                            {
                                mySelection.MainTab.DataSet[row].Column6Color = ("Blue");
                                mySelection.PerformanceIndicator1.DataSet[row].Column6Color = ("Blue");

                                if (mySelection.PerformanceIndicator2.TabVisibility)
                                    mySelection.PerformanceIndicator2.DataSet[row].Column6Color = ("Blue");
                                if (mySelection.PerformanceIndicator3.TabVisibility)
                                    mySelection.PerformanceIndicator3.DataSet[row].Column6Color = ("Blue");
                                if (mySelection.PerformanceIndicator4.TabVisibility)
                                    mySelection.PerformanceIndicator4.DataSet[row].Column6Color = ("Blue");
                                if (mySelection.PerformanceIndicator5.TabVisibility)
                                    mySelection.PerformanceIndicator5.DataSet[row].Column6Color = ("Blue");
                                if (mySelection.PerformanceIndicator6.TabVisibility)
                                    mySelection.PerformanceIndicator6.DataSet[row].Column6Color = ("Blue");
                                if (mySelection.PerformanceIndicator7.TabVisibility)
                                    mySelection.PerformanceIndicator7.DataSet[row].Column6Color = ("Blue");
                                if (mySelection.PerformanceIndicator8.TabVisibility)
                                    mySelection.PerformanceIndicator8.DataSet[row].Column6Color = ("Blue");
                                if (mySelection.PerformanceIndicator9.TabVisibility)
                                    mySelection.PerformanceIndicator9.DataSet[row].Column6Color = ("Blue");
                                if (mySelection.PerformanceIndicator10.TabVisibility)
                                    mySelection.PerformanceIndicator10.DataSet[row].Column6Color = ("Blue");

                                mySelection.YieldTab.DataSet[row].Column6Color = ("Blue");
                            }
                            else
                            {
                                mySelection.MainTab.DataSet[row].Column6Color = ("Green");
                                mySelection.PerformanceIndicator1.DataSet[row].Column6Color = ("Green");

                                if (mySelection.PerformanceIndicator2.TabVisibility)
                                    mySelection.PerformanceIndicator2.DataSet[row].Column6Color = ("Green");
                                if (mySelection.PerformanceIndicator3.TabVisibility)
                                    mySelection.PerformanceIndicator3.DataSet[row].Column6Color = ("Green");
                                if (mySelection.PerformanceIndicator4.TabVisibility)
                                    mySelection.PerformanceIndicator4.DataSet[row].Column6Color = ("Green");
                                if (mySelection.PerformanceIndicator5.TabVisibility)
                                    mySelection.PerformanceIndicator5.DataSet[row].Column6Color = ("Green");
                                if (mySelection.PerformanceIndicator6.TabVisibility)
                                    mySelection.PerformanceIndicator6.DataSet[row].Column6Color = ("Green");
                                if (mySelection.PerformanceIndicator7.TabVisibility)
                                    mySelection.PerformanceIndicator7.DataSet[row].Column6Color = ("Green");
                                if (mySelection.PerformanceIndicator8.TabVisibility)
                                    mySelection.PerformanceIndicator8.DataSet[row].Column6Color = ("Green");
                                if (mySelection.PerformanceIndicator9.TabVisibility)
                                    mySelection.PerformanceIndicator9.DataSet[row].Column6Color = ("Green");
                                if (mySelection.PerformanceIndicator10.TabVisibility)
                                    mySelection.PerformanceIndicator10.DataSet[row].Column6Color = ("Green");

                                mySelection.YieldTab.DataSet[row].Column6Color = ("Green");
                            }

                            if (mySelection.YieldTab.HeaderText.Equals("Efficiency/Simple Yield [%]")) //if in percentage mode
                            {
                                percentage = mySelection.YieldTab.DataSet[row].Column7;
                            }
                            else
                            {
                                percentage = mySelection.YieldTab.DataSet[row].Column7 / maxValue * 100;
                            }

                            //Set all tabs' column colors
                            if (percentage == 0)
                            {
                                mySelection.MainTab.DataSet[row].Column7Color = ("Transparent");
                                mySelection.PerformanceIndicator1.DataSet[row].Column7Color = ("Transparent");

                                if (mySelection.PerformanceIndicator2.TabVisibility)
                                    mySelection.PerformanceIndicator2.DataSet[row].Column7Color = ("Transparent");
                                if (mySelection.PerformanceIndicator3.TabVisibility)
                                    mySelection.PerformanceIndicator3.DataSet[row].Column7Color = ("Transparent");
                                if (mySelection.PerformanceIndicator4.TabVisibility)
                                    mySelection.PerformanceIndicator4.DataSet[row].Column7Color = ("Transparent");
                                if (mySelection.PerformanceIndicator5.TabVisibility)
                                    mySelection.PerformanceIndicator5.DataSet[row].Column7Color = ("Transparent");
                                if (mySelection.PerformanceIndicator6.TabVisibility)
                                    mySelection.PerformanceIndicator6.DataSet[row].Column7Color = ("Transparent");
                                if (mySelection.PerformanceIndicator7.TabVisibility)
                                    mySelection.PerformanceIndicator7.DataSet[row].Column7Color = ("Transparent");
                                if (mySelection.PerformanceIndicator8.TabVisibility)
                                    mySelection.PerformanceIndicator8.DataSet[row].Column7Color = ("Transparent");
                                if (mySelection.PerformanceIndicator9.TabVisibility)
                                    mySelection.PerformanceIndicator9.DataSet[row].Column7Color = ("Transparent");
                                if (mySelection.PerformanceIndicator10.TabVisibility)
                                    mySelection.PerformanceIndicator10.DataSet[row].Column7Color = ("Transparent");

                                mySelection.YieldTab.DataSet[row].Column7Color = ("Transparent");
                            }
                            //Set all tabs' column colors
                            else if (percentage > 0 && percentage <= 25)
                            {
                                mySelection.MainTab.DataSet[row].Column7Color = ("Red");
                                mySelection.PerformanceIndicator1.DataSet[row].Column7Color = ("Red");

                                if (mySelection.PerformanceIndicator2.TabVisibility)
                                    mySelection.PerformanceIndicator2.DataSet[row].Column7Color = ("Red");
                                if (mySelection.PerformanceIndicator3.TabVisibility)
                                    mySelection.PerformanceIndicator3.DataSet[row].Column7Color = ("Red");
                                if (mySelection.PerformanceIndicator4.TabVisibility)
                                    mySelection.PerformanceIndicator4.DataSet[row].Column7Color = ("Red");
                                if (mySelection.PerformanceIndicator5.TabVisibility)
                                    mySelection.PerformanceIndicator5.DataSet[row].Column7Color = ("Red");
                                if (mySelection.PerformanceIndicator6.TabVisibility)
                                    mySelection.PerformanceIndicator6.DataSet[row].Column7Color = ("Red");
                                if (mySelection.PerformanceIndicator7.TabVisibility)
                                    mySelection.PerformanceIndicator7.DataSet[row].Column7Color = ("Red");
                                if (mySelection.PerformanceIndicator8.TabVisibility)
                                    mySelection.PerformanceIndicator8.DataSet[row].Column7Color = ("Red");
                                if (mySelection.PerformanceIndicator9.TabVisibility)
                                    mySelection.PerformanceIndicator9.DataSet[row].Column7Color = ("Red");
                                if (mySelection.PerformanceIndicator10.TabVisibility)
                                    mySelection.PerformanceIndicator10.DataSet[row].Column7Color = ("Red");

                                mySelection.YieldTab.DataSet[row].Column7Color = ("Red");
                            }
                            else if (percentage < 75)
                            {
                                mySelection.MainTab.DataSet[row].Column7Color = ("Blue");
                                mySelection.PerformanceIndicator1.DataSet[row].Column7Color = ("Blue");

                                if (mySelection.PerformanceIndicator2.TabVisibility)
                                    mySelection.PerformanceIndicator2.DataSet[row].Column7Color = ("Blue");
                                if (mySelection.PerformanceIndicator3.TabVisibility)
                                    mySelection.PerformanceIndicator3.DataSet[row].Column7Color = ("Blue");
                                if (mySelection.PerformanceIndicator4.TabVisibility)
                                    mySelection.PerformanceIndicator4.DataSet[row].Column7Color = ("Blue");
                                if (mySelection.PerformanceIndicator5.TabVisibility)
                                    mySelection.PerformanceIndicator5.DataSet[row].Column7Color = ("Blue");
                                if (mySelection.PerformanceIndicator6.TabVisibility)
                                    mySelection.PerformanceIndicator6.DataSet[row].Column7Color = ("Blue");
                                if (mySelection.PerformanceIndicator7.TabVisibility)
                                    mySelection.PerformanceIndicator7.DataSet[row].Column7Color = ("Blue");
                                if (mySelection.PerformanceIndicator8.TabVisibility)
                                    mySelection.PerformanceIndicator8.DataSet[row].Column7Color = ("Blue");
                                if (mySelection.PerformanceIndicator9.TabVisibility)
                                    mySelection.PerformanceIndicator9.DataSet[row].Column7Color = ("Blue");
                                if (mySelection.PerformanceIndicator10.TabVisibility)
                                    mySelection.PerformanceIndicator10.DataSet[row].Column7Color = ("Blue");

                                mySelection.YieldTab.DataSet[row].Column7Color = ("Blue");
                            }
                            else
                            {
                                mySelection.MainTab.DataSet[row].Column7Color = ("Green");
                                mySelection.PerformanceIndicator1.DataSet[row].Column7Color = ("Green");

                                if (mySelection.PerformanceIndicator2.TabVisibility)
                                    mySelection.PerformanceIndicator2.DataSet[row].Column7Color = ("Green");
                                if (mySelection.PerformanceIndicator3.TabVisibility)
                                    mySelection.PerformanceIndicator3.DataSet[row].Column7Color = ("Green");
                                if (mySelection.PerformanceIndicator4.TabVisibility)
                                    mySelection.PerformanceIndicator4.DataSet[row].Column7Color = ("Green");
                                if (mySelection.PerformanceIndicator5.TabVisibility)
                                    mySelection.PerformanceIndicator5.DataSet[row].Column7Color = ("Green");
                                if (mySelection.PerformanceIndicator6.TabVisibility)
                                    mySelection.PerformanceIndicator6.DataSet[row].Column7Color = ("Green");
                                if (mySelection.PerformanceIndicator7.TabVisibility)
                                    mySelection.PerformanceIndicator7.DataSet[row].Column7Color = ("Green");
                                if (mySelection.PerformanceIndicator8.TabVisibility)
                                    mySelection.PerformanceIndicator8.DataSet[row].Column7Color = ("Green");
                                if (mySelection.PerformanceIndicator9.TabVisibility)
                                    mySelection.PerformanceIndicator9.DataSet[row].Column7Color = ("Green");
                                if (mySelection.PerformanceIndicator10.TabVisibility)
                                    mySelection.PerformanceIndicator10.DataSet[row].Column7Color = ("Green");

                                mySelection.YieldTab.DataSet[row].Column7Color = ("Green");
                            }

                            if (mySelection.YieldTab.HeaderText.Equals("Efficiency/Simple Yield [%]")) //if in percentage mode
                            {
                                percentage = mySelection.YieldTab.DataSet[row].Column8;
                            }
                            else
                            {
                                percentage = mySelection.YieldTab.DataSet[row].Column8 / maxValue * 100;
                            }


                            //Set all tabs' column colors
                            if (percentage == 0)
                            {
                                mySelection.MainTab.DataSet[row].Column8Color = ("Transparent");
                                mySelection.PerformanceIndicator1.DataSet[row].Column8Color = ("Transparent");

                                if (mySelection.PerformanceIndicator2.TabVisibility)
                                    mySelection.PerformanceIndicator2.DataSet[row].Column8Color = ("Transparent");
                                if (mySelection.PerformanceIndicator3.TabVisibility)
                                    mySelection.PerformanceIndicator3.DataSet[row].Column8Color = ("Transparent");
                                if (mySelection.PerformanceIndicator4.TabVisibility)
                                    mySelection.PerformanceIndicator4.DataSet[row].Column8Color = ("Transparent");
                                if (mySelection.PerformanceIndicator5.TabVisibility)
                                    mySelection.PerformanceIndicator5.DataSet[row].Column8Color = ("Transparent");
                                if (mySelection.PerformanceIndicator6.TabVisibility)
                                    mySelection.PerformanceIndicator6.DataSet[row].Column8Color = ("Transparent");
                                if (mySelection.PerformanceIndicator7.TabVisibility)
                                    mySelection.PerformanceIndicator7.DataSet[row].Column8Color = ("Transparent");
                                if (mySelection.PerformanceIndicator8.TabVisibility)
                                    mySelection.PerformanceIndicator8.DataSet[row].Column8Color = ("Transparent");
                                if (mySelection.PerformanceIndicator9.TabVisibility)
                                    mySelection.PerformanceIndicator9.DataSet[row].Column8Color = ("Transparent");
                                if (mySelection.PerformanceIndicator10.TabVisibility)
                                    mySelection.PerformanceIndicator10.DataSet[row].Column8Color = ("Transparent");

                                mySelection.YieldTab.DataSet[row].Column8Color = ("Transparent");
                            }
                            //Set all tabs' column colors
                            else if (percentage > 0 && percentage <= 25)
                            {
                                mySelection.MainTab.DataSet[row].Column8Color = ("Red");
                                mySelection.PerformanceIndicator1.DataSet[row].Column8Color = ("Red");

                                if (mySelection.PerformanceIndicator2.TabVisibility)
                                    mySelection.PerformanceIndicator2.DataSet[row].Column8Color = ("Red");
                                if (mySelection.PerformanceIndicator3.TabVisibility)
                                    mySelection.PerformanceIndicator3.DataSet[row].Column8Color = ("Red");
                                if (mySelection.PerformanceIndicator4.TabVisibility)
                                    mySelection.PerformanceIndicator4.DataSet[row].Column8Color = ("Red");
                                if (mySelection.PerformanceIndicator5.TabVisibility)
                                    mySelection.PerformanceIndicator5.DataSet[row].Column8Color = ("Red");
                                if (mySelection.PerformanceIndicator6.TabVisibility)
                                    mySelection.PerformanceIndicator6.DataSet[row].Column8Color = ("Red");
                                if (mySelection.PerformanceIndicator7.TabVisibility)
                                    mySelection.PerformanceIndicator7.DataSet[row].Column8Color = ("Red");
                                if (mySelection.PerformanceIndicator8.TabVisibility)
                                    mySelection.PerformanceIndicator8.DataSet[row].Column8Color = ("Red");
                                if (mySelection.PerformanceIndicator9.TabVisibility)
                                    mySelection.PerformanceIndicator9.DataSet[row].Column8Color = ("Red");
                                if (mySelection.PerformanceIndicator10.TabVisibility)
                                    mySelection.PerformanceIndicator10.DataSet[row].Column8Color = ("Red");

                                mySelection.YieldTab.DataSet[row].Column8Color = ("Red");
                            }
                            else if (percentage < 75)
                            {
                                mySelection.MainTab.DataSet[row].Column8Color = ("Blue");
                                mySelection.PerformanceIndicator1.DataSet[row].Column8Color = ("Blue");

                                if (mySelection.PerformanceIndicator2.TabVisibility)
                                    mySelection.PerformanceIndicator2.DataSet[row].Column8Color = ("Blue");
                                if (mySelection.PerformanceIndicator3.TabVisibility)
                                    mySelection.PerformanceIndicator3.DataSet[row].Column8Color = ("Blue");
                                if (mySelection.PerformanceIndicator4.TabVisibility)
                                    mySelection.PerformanceIndicator4.DataSet[row].Column8Color = ("Blue");
                                if (mySelection.PerformanceIndicator5.TabVisibility)
                                    mySelection.PerformanceIndicator5.DataSet[row].Column8Color = ("Blue");
                                if (mySelection.PerformanceIndicator6.TabVisibility)
                                    mySelection.PerformanceIndicator6.DataSet[row].Column8Color = ("Blue");
                                if (mySelection.PerformanceIndicator7.TabVisibility)
                                    mySelection.PerformanceIndicator7.DataSet[row].Column8Color = ("Blue");
                                if (mySelection.PerformanceIndicator8.TabVisibility)
                                    mySelection.PerformanceIndicator8.DataSet[row].Column8Color = ("Blue");
                                if (mySelection.PerformanceIndicator9.TabVisibility)
                                    mySelection.PerformanceIndicator9.DataSet[row].Column8Color = ("Blue");
                                if (mySelection.PerformanceIndicator10.TabVisibility)
                                    mySelection.PerformanceIndicator10.DataSet[row].Column8Color = ("Blue");

                                mySelection.YieldTab.DataSet[row].Column8Color = ("Blue");
                            }
                            else
                            {
                                mySelection.MainTab.DataSet[row].Column8Color = ("Green");
                                mySelection.PerformanceIndicator1.DataSet[row].Column8Color = ("Green");

                                if (mySelection.PerformanceIndicator2.TabVisibility)
                                    mySelection.PerformanceIndicator2.DataSet[row].Column8Color = ("Green");
                                if (mySelection.PerformanceIndicator3.TabVisibility)
                                    mySelection.PerformanceIndicator3.DataSet[row].Column8Color = ("Green");
                                if (mySelection.PerformanceIndicator4.TabVisibility)
                                    mySelection.PerformanceIndicator4.DataSet[row].Column8Color = ("Green");
                                if (mySelection.PerformanceIndicator5.TabVisibility)
                                    mySelection.PerformanceIndicator5.DataSet[row].Column8Color = ("Green");
                                if (mySelection.PerformanceIndicator6.TabVisibility)
                                    mySelection.PerformanceIndicator6.DataSet[row].Column8Color = ("Green");
                                if (mySelection.PerformanceIndicator7.TabVisibility)
                                    mySelection.PerformanceIndicator7.DataSet[row].Column8Color = ("Green");
                                if (mySelection.PerformanceIndicator8.TabVisibility)
                                    mySelection.PerformanceIndicator8.DataSet[row].Column8Color = ("Green");
                                if (mySelection.PerformanceIndicator9.TabVisibility)
                                    mySelection.PerformanceIndicator9.DataSet[row].Column8Color = ("Green");
                                if (mySelection.PerformanceIndicator10.TabVisibility)
                                    mySelection.PerformanceIndicator10.DataSet[row].Column8Color = ("Green");

                                mySelection.YieldTab.DataSet[row].Column8Color = ("Green");
                            }

                            if (mySelection.YieldTab.HeaderText.Equals("Efficiency/Simple Yield [%]")) //if in percentage mode
                            {
                                percentage = mySelection.YieldTab.DataSet[row].Column9;
                            }
                            else
                            {
                                percentage = mySelection.YieldTab.DataSet[row].Column9 / maxValue * 100;
                            }

                            //Set all tabs' column colors
                            if (percentage == 0)
                            {
                                mySelection.MainTab.DataSet[row].Column9Color = ("Transparent");
                                mySelection.PerformanceIndicator1.DataSet[row].Column9Color = ("Transparent");

                                if (mySelection.PerformanceIndicator2.TabVisibility)
                                    mySelection.PerformanceIndicator2.DataSet[row].Column9Color = ("Transparent");
                                if (mySelection.PerformanceIndicator3.TabVisibility)
                                    mySelection.PerformanceIndicator3.DataSet[row].Column9Color = ("Transparent");
                                if (mySelection.PerformanceIndicator4.TabVisibility)
                                    mySelection.PerformanceIndicator4.DataSet[row].Column9Color = ("Transparent");
                                if (mySelection.PerformanceIndicator5.TabVisibility)
                                    mySelection.PerformanceIndicator5.DataSet[row].Column9Color = ("Transparent");
                                if (mySelection.PerformanceIndicator6.TabVisibility)
                                    mySelection.PerformanceIndicator6.DataSet[row].Column9Color = ("Transparent");
                                if (mySelection.PerformanceIndicator7.TabVisibility)
                                    mySelection.PerformanceIndicator7.DataSet[row].Column9Color = ("Transparent");
                                if (mySelection.PerformanceIndicator8.TabVisibility)
                                    mySelection.PerformanceIndicator8.DataSet[row].Column9Color = ("Transparent");
                                if (mySelection.PerformanceIndicator9.TabVisibility)
                                    mySelection.PerformanceIndicator9.DataSet[row].Column9Color = ("Transparent");
                                if (mySelection.PerformanceIndicator10.TabVisibility)
                                    mySelection.PerformanceIndicator10.DataSet[row].Column9Color = ("Transparent");

                                mySelection.YieldTab.DataSet[row].Column9Color = ("Transparent");
                            }
                            //Set all tabs' column colors
                            else if (percentage > 0 && percentage <= 25)
                            {
                                mySelection.MainTab.DataSet[row].Column9Color = ("Red");
                                mySelection.PerformanceIndicator1.DataSet[row].Column9Color = ("Red");

                                if (mySelection.PerformanceIndicator2.TabVisibility)
                                    mySelection.PerformanceIndicator2.DataSet[row].Column9Color = ("Red");
                                if (mySelection.PerformanceIndicator3.TabVisibility)
                                    mySelection.PerformanceIndicator3.DataSet[row].Column9Color = ("Red");
                                if (mySelection.PerformanceIndicator4.TabVisibility)
                                    mySelection.PerformanceIndicator4.DataSet[row].Column9Color = ("Red");
                                if (mySelection.PerformanceIndicator5.TabVisibility)
                                    mySelection.PerformanceIndicator5.DataSet[row].Column9Color = ("Red");
                                if (mySelection.PerformanceIndicator6.TabVisibility)
                                    mySelection.PerformanceIndicator6.DataSet[row].Column9Color = ("Red");
                                if (mySelection.PerformanceIndicator7.TabVisibility)
                                    mySelection.PerformanceIndicator7.DataSet[row].Column9Color = ("Red");
                                if (mySelection.PerformanceIndicator8.TabVisibility)
                                    mySelection.PerformanceIndicator8.DataSet[row].Column9Color = ("Red");
                                if (mySelection.PerformanceIndicator9.TabVisibility)
                                    mySelection.PerformanceIndicator9.DataSet[row].Column9Color = ("Red");
                                if (mySelection.PerformanceIndicator10.TabVisibility)
                                    mySelection.PerformanceIndicator10.DataSet[row].Column9Color = ("Red");

                                mySelection.YieldTab.DataSet[row].Column9Color = ("Red");
                            }
                            else if (percentage < 75)
                            {
                                mySelection.MainTab.DataSet[row].Column9Color = ("Blue");
                                mySelection.PerformanceIndicator1.DataSet[row].Column9Color = ("Blue");

                                if (mySelection.PerformanceIndicator2.TabVisibility)
                                    mySelection.PerformanceIndicator2.DataSet[row].Column9Color = ("Blue");
                                if (mySelection.PerformanceIndicator3.TabVisibility)
                                    mySelection.PerformanceIndicator3.DataSet[row].Column9Color = ("Blue");
                                if (mySelection.PerformanceIndicator4.TabVisibility)
                                    mySelection.PerformanceIndicator4.DataSet[row].Column9Color = ("Blue");
                                if (mySelection.PerformanceIndicator5.TabVisibility)
                                    mySelection.PerformanceIndicator5.DataSet[row].Column9Color = ("Blue");
                                if (mySelection.PerformanceIndicator6.TabVisibility)
                                    mySelection.PerformanceIndicator6.DataSet[row].Column9Color = ("Blue");
                                if (mySelection.PerformanceIndicator7.TabVisibility)
                                    mySelection.PerformanceIndicator7.DataSet[row].Column9Color = ("Blue");
                                if (mySelection.PerformanceIndicator8.TabVisibility)
                                    mySelection.PerformanceIndicator8.DataSet[row].Column9Color = ("Blue");
                                if (mySelection.PerformanceIndicator9.TabVisibility)
                                    mySelection.PerformanceIndicator9.DataSet[row].Column9Color = ("Blue");
                                if (mySelection.PerformanceIndicator10.TabVisibility)
                                    mySelection.PerformanceIndicator10.DataSet[row].Column9Color = ("Blue");

                                mySelection.YieldTab.DataSet[row].Column9Color = ("Blue");
                            }
                            else
                            {
                                mySelection.MainTab.DataSet[row].Column9Color = ("Green");
                                mySelection.PerformanceIndicator1.DataSet[row].Column9Color = ("Green");

                                if (mySelection.PerformanceIndicator2.TabVisibility)
                                    mySelection.PerformanceIndicator2.DataSet[row].Column9Color = ("Green");
                                if (mySelection.PerformanceIndicator3.TabVisibility)
                                    mySelection.PerformanceIndicator3.DataSet[row].Column9Color = ("Green");
                                if (mySelection.PerformanceIndicator4.TabVisibility)
                                    mySelection.PerformanceIndicator4.DataSet[row].Column9Color = ("Green");
                                if (mySelection.PerformanceIndicator5.TabVisibility)
                                    mySelection.PerformanceIndicator5.DataSet[row].Column9Color = ("Green");
                                if (mySelection.PerformanceIndicator6.TabVisibility)
                                    mySelection.PerformanceIndicator6.DataSet[row].Column9Color = ("Green");
                                if (mySelection.PerformanceIndicator7.TabVisibility)
                                    mySelection.PerformanceIndicator7.DataSet[row].Column9Color = ("Green");
                                if (mySelection.PerformanceIndicator8.TabVisibility)
                                    mySelection.PerformanceIndicator8.DataSet[row].Column9Color = ("Green");
                                if (mySelection.PerformanceIndicator9.TabVisibility)
                                    mySelection.PerformanceIndicator9.DataSet[row].Column9Color = ("Green");
                                if (mySelection.PerformanceIndicator10.TabVisibility)
                                    mySelection.PerformanceIndicator10.DataSet[row].Column9Color = ("Green");

                                mySelection.YieldTab.DataSet[row].Column9Color = ("Green");
                            }

                            if (mySelection.YieldTab.HeaderText.Equals("Efficiency/Simple Yield [%]")) //if in percentage mode
                            {
                                percentage = mySelection.YieldTab.DataSet[row].Column10;
                            }
                            else
                            {
                                percentage = mySelection.YieldTab.DataSet[row].Column10 / maxValue * 100;
                            }

                            //Set all tabs' column colors
                            if (percentage == 0)
                            {
                                mySelection.MainTab.DataSet[row].Column10Color = ("Transparent");
                                mySelection.PerformanceIndicator1.DataSet[row].Column10Color = ("Transparent");

                                if (mySelection.PerformanceIndicator2.TabVisibility)
                                    mySelection.PerformanceIndicator2.DataSet[row].Column10Color = ("Transparent");
                                if (mySelection.PerformanceIndicator3.TabVisibility)
                                    mySelection.PerformanceIndicator3.DataSet[row].Column10Color = ("Transparent");
                                if (mySelection.PerformanceIndicator4.TabVisibility)
                                    mySelection.PerformanceIndicator4.DataSet[row].Column10Color = ("Transparent");
                                if (mySelection.PerformanceIndicator5.TabVisibility)
                                    mySelection.PerformanceIndicator5.DataSet[row].Column10Color = ("Transparent");
                                if (mySelection.PerformanceIndicator6.TabVisibility)
                                    mySelection.PerformanceIndicator6.DataSet[row].Column10Color = ("Transparent");
                                if (mySelection.PerformanceIndicator7.TabVisibility)
                                    mySelection.PerformanceIndicator7.DataSet[row].Column10Color = ("Transparent");
                                if (mySelection.PerformanceIndicator8.TabVisibility)
                                    mySelection.PerformanceIndicator8.DataSet[row].Column10Color = ("Transparent");
                                if (mySelection.PerformanceIndicator9.TabVisibility)
                                    mySelection.PerformanceIndicator9.DataSet[row].Column10Color = ("Transparent");
                                if (mySelection.PerformanceIndicator10.TabVisibility)
                                    mySelection.PerformanceIndicator10.DataSet[row].Column10Color = ("Transparent");

                                mySelection.YieldTab.DataSet[row].Column10Color = ("Transparent");
                            }
                            //Set all tabs' column colors
                            else if (percentage > 0 && percentage <= 25)
                            {
                                mySelection.MainTab.DataSet[row].Column10Color = ("Red");
                                mySelection.PerformanceIndicator1.DataSet[row].Column10Color = ("Red");

                                if (mySelection.PerformanceIndicator2.TabVisibility)
                                    mySelection.PerformanceIndicator2.DataSet[row].Column10Color = ("Red");
                                if (mySelection.PerformanceIndicator3.TabVisibility)
                                    mySelection.PerformanceIndicator3.DataSet[row].Column10Color = ("Red");
                                if (mySelection.PerformanceIndicator4.TabVisibility)
                                    mySelection.PerformanceIndicator4.DataSet[row].Column10Color = ("Red");
                                if (mySelection.PerformanceIndicator5.TabVisibility)
                                    mySelection.PerformanceIndicator5.DataSet[row].Column10Color = ("Red");
                                if (mySelection.PerformanceIndicator6.TabVisibility)
                                    mySelection.PerformanceIndicator6.DataSet[row].Column10Color = ("Red");
                                if (mySelection.PerformanceIndicator7.TabVisibility)
                                    mySelection.PerformanceIndicator7.DataSet[row].Column10Color = ("Red");
                                if (mySelection.PerformanceIndicator8.TabVisibility)
                                    mySelection.PerformanceIndicator8.DataSet[row].Column10Color = ("Red");
                                if (mySelection.PerformanceIndicator9.TabVisibility)
                                    mySelection.PerformanceIndicator9.DataSet[row].Column10Color = ("Red");
                                if (mySelection.PerformanceIndicator10.TabVisibility)
                                    mySelection.PerformanceIndicator10.DataSet[row].Column10Color = ("Red");

                                mySelection.YieldTab.DataSet[row].Column10Color = ("Red");
                            }
                            else if (percentage < 75)
                            {
                                mySelection.MainTab.DataSet[row].Column10Color = ("Blue");
                                mySelection.PerformanceIndicator1.DataSet[row].Column10Color = ("Blue");

                                if (mySelection.PerformanceIndicator2.TabVisibility)
                                    mySelection.PerformanceIndicator2.DataSet[row].Column10Color = ("Blue");
                                if (mySelection.PerformanceIndicator3.TabVisibility)
                                    mySelection.PerformanceIndicator3.DataSet[row].Column10Color = ("Blue");
                                if (mySelection.PerformanceIndicator4.TabVisibility)
                                    mySelection.PerformanceIndicator4.DataSet[row].Column10Color = ("Blue");
                                if (mySelection.PerformanceIndicator5.TabVisibility)
                                    mySelection.PerformanceIndicator5.DataSet[row].Column10Color = ("Blue");
                                if (mySelection.PerformanceIndicator6.TabVisibility)
                                    mySelection.PerformanceIndicator6.DataSet[row].Column10Color = ("Blue");
                                if (mySelection.PerformanceIndicator7.TabVisibility)
                                    mySelection.PerformanceIndicator7.DataSet[row].Column10Color = ("Blue");
                                if (mySelection.PerformanceIndicator8.TabVisibility)
                                    mySelection.PerformanceIndicator8.DataSet[row].Column10Color = ("Blue");
                                if (mySelection.PerformanceIndicator9.TabVisibility)
                                    mySelection.PerformanceIndicator9.DataSet[row].Column10Color = ("Blue");
                                if (mySelection.PerformanceIndicator10.TabVisibility)
                                    mySelection.PerformanceIndicator10.DataSet[row].Column10Color = ("Blue");

                                mySelection.YieldTab.DataSet[row].Column10Color = ("Blue");
                            }
                            else
                            {
                                mySelection.MainTab.DataSet[row].Column10Color = ("Green");
                                mySelection.PerformanceIndicator1.DataSet[row].Column10Color = ("Green");

                                if (mySelection.PerformanceIndicator2.TabVisibility)
                                    mySelection.PerformanceIndicator2.DataSet[row].Column10Color = ("Green");
                                if (mySelection.PerformanceIndicator3.TabVisibility)
                                    mySelection.PerformanceIndicator3.DataSet[row].Column10Color = ("Green");
                                if (mySelection.PerformanceIndicator4.TabVisibility)
                                    mySelection.PerformanceIndicator4.DataSet[row].Column10Color = ("Green");
                                if (mySelection.PerformanceIndicator5.TabVisibility)
                                    mySelection.PerformanceIndicator5.DataSet[row].Column10Color = ("Green");
                                if (mySelection.PerformanceIndicator6.TabVisibility)
                                    mySelection.PerformanceIndicator6.DataSet[row].Column10Color = ("Green");
                                if (mySelection.PerformanceIndicator7.TabVisibility)
                                    mySelection.PerformanceIndicator7.DataSet[row].Column10Color = ("Green");
                                if (mySelection.PerformanceIndicator8.TabVisibility)
                                    mySelection.PerformanceIndicator8.DataSet[row].Column10Color = ("Green");
                                if (mySelection.PerformanceIndicator9.TabVisibility)
                                    mySelection.PerformanceIndicator9.DataSet[row].Column10Color = ("Green");
                                if (mySelection.PerformanceIndicator10.TabVisibility)
                                    mySelection.PerformanceIndicator10.DataSet[row].Column10Color = ("Green");

                                mySelection.YieldTab.DataSet[row].Column10Color = ("Green");
                            }
                        }
                        else
                            mySelection.YieldTab.DataSet[row].Visualize = false; //else just turn it to false
                    }
                }

            }
            catch (Exception ex)
            {
                ErrorWindow window = new ErrorWindow("Visualize Detail", "Format was invalid", "Error Message: \n" + ex.Message);

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
