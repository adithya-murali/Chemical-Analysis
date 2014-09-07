using ProcessYieldOptimizer.Dialogs;
using ProcessYieldOptimizer.Models;
using ProcessYieldOptimizer.Utilities;
using ProcessYieldOptimizer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ProcessYieldOptimizer.Commands
{
    class SaveSessionCommand : ICommand
    {
        public SaveSessionCommand(ProcessYieldViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        private ProcessYieldViewModel _viewModel;

        #region ICommand Members

        public bool CanExecute(object parameter)
        {
            if (_viewModel.MyScenarios.UserCalculationList.Count > 0) //if at least one scenario
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
                Microsoft.Win32.SaveFileDialog Dialog = new Microsoft.Win32.SaveFileDialog();
                Dialog.DefaultExt = ".pyo";
                Dialog.Filter = "PYO Session (*.pyo)|*.pyo";

                Nullable<bool> userResult = Dialog.ShowDialog();

                if (userResult == true)
                {

                    List<ScenarioDetail> myScenarios = new List<ScenarioDetail>();
                    for (int c = 0; c < _viewModel.MyScenarios.UserCalculationList.Count; c++)
                    {
                        myScenarios.Add(_viewModel.MyScenarios.UserCalculationList[c]);
                    }

                    XMlLoadSaveSessions saveSession = new XMlLoadSaveSessions();
                    saveSession.setFilePath(Dialog.FileName);
                    saveSession.Scenarios = myScenarios;
                    saveSession.CreateFile();
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

    class LoadSessionCommand : ICommand
    {
        public LoadSessionCommand(ProcessYieldViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        private ProcessYieldViewModel _viewModel;

        #region ICommand Members

        public bool CanExecute(object parameter)
        {
            return true; //always true
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
                Microsoft.Win32.OpenFileDialog Dialog = new Microsoft.Win32.OpenFileDialog();
                Dialog.DefaultExt = ".pyo";
                Dialog.Filter = "PYO Session (*.pyo)|*.pyo";

                Nullable<bool> userResult = Dialog.ShowDialog();

                if (userResult == true)
                {
                    XMlLoadSaveSessions loadSession = new XMlLoadSaveSessions();
                    loadSession.setFilePath(Dialog.FileName);
                    loadSession.ReadFile();
                    List<PreliminaryLoadScenario> preScenarios = loadSession.LoadedScenarios;

                    for (int c = 0; c < preScenarios.Count; c++)
                    {
                        if (!_viewModel.validateScenario(preScenarios[c].ScenarioName)) //validate scenario name that it is unique
                        {
                            //Do what windows does and say 'Copy of '
                            preScenarios[c].ScenarioName = "Copy of " + preScenarios[c].ScenarioName;
                        }

                        _viewModel.MyScenarios.addScenario(preScenarios[c].ScenarioName, preScenarios[c].MainTab, preScenarios[c].PerformanceIndicators);
                    }
                    
                }
            }
            catch (Exception ex)
            {
                ErrorWindow window = new ErrorWindow("Load Failed", "Error occured after validation.\nSee details.", "Error Message: \n" + ex.Message);

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
