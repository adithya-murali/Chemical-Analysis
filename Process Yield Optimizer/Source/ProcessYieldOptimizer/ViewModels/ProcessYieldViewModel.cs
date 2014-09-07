using Microsoft.Practices.Prism.Mvvm;
using ProcessYieldOptimizer.Commands;
using ProcessYieldOptimizer.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ProcessYieldOptimizer.ViewModels
{
    /// <summary>
    /// ProcessYieldViewModel has everything that views and controls need to bind for program to function properly.
    /// In this simple program with no database or server to serve as model the 'MyScenarios' acts like the model.
    /// 
    /// Creates ICommand bindings for most buttons, and menu and context menu items.
    ///     - Icommands region is broken down into 
    ///       - Scenario Commands
    ///       - DataGrid Commands
    ///       - MainMenu Commands
    ///       - ContextMenu Commands
    /// Data is already structured in that model for Views to bind to properly without too much formatting. 
    ///   Uses object ScenarioFormatHelper to help format UI information with model  
    ///   
    /// Debug Notes: Debug/warnings in console output can cause interaction to be little slower in debug environment using Visual Studio especially (version 2013 Update 2 in x86)
    ///              Program when directly executed and not attached to visual studio should not exhibit any such behavior.
    /// </summary>
    class ProcessYieldViewModel
    {
        public ProcessYieldViewModel()
        {
            //Set Model
            MyScenarios = new Scenarios();

            //Set Scenario Commands
            AddScenarioCommand = new AddScenarioCommand(this);
            ModifyScenarioCommand = new ModifyScenarioCommand(this);
            RemoveScenarioCommand = new RemoveScenarioCommand(this);
            CalculateDialogCommand = new CalculateDialogCommand(this);            
            ToggleEfficiencyCommand = new ToggleEfficiencyCommand(this);
            SaveScenarioCommand = new SaveScenarioCommand(this);
            LoadScenarioCommand = new LoadScenarioCommand(this);

            //Set DataGrid Commands 
            AddDataDetailCommand = new AddDataDetailCommand(this);
            RemoveDataDetailCommand = new RemoveDataDetailCommand(this);
            CopyCommand = new CopyCommand(this);
            PasteCommand = new PasteCommand(this);
            CopyAllCommand = new CopyAllCommand(this);
            PasteAllCommand = new PasteAllCommand(this);
            VisualizeCommand = new VisualizeCommand(this);

            //Set Dialog Commands
            DialogAddPICommand = new DialogAddPICommand(this);
            DialogModifyScenarioCommand = new DialogModifyScenarioCommand(this);
            DialogRemovePICommand = new DialogRemovePICommand(this);
            DialogAddScenariocommand = new DialogAddScenariocommand(this);
            CalculateEfficiencyCommand = new CalculateEfficiencyCommand(this);

            //Set Menu Commands
            SaveSessionCommand = new SaveSessionCommand(this);
            LoadSessionCommand = new LoadSessionCommand(this);

            //Set ViewModelFormatter
            ViewModelFormatter = new ScenarioFormatHelper();        
        }

        //Model
        public Scenarios MyScenarios
        {
            get;
            set;
        }

        //Converts user input dialog info to model and viceversa
        public ScenarioFormatHelper ViewModelFormatter
        {
            get;
            set;
        }

        //Converts user input dialog info to model and viceversa
        public ScenarioCalculateHelper CalculateModelFormatter
        {
            get;
            set;
        }

        /// <summary>
        /// Can only have scenario with unique names
        /// </summary>
        /// <param name="name"> String name of scenario to add </param> 
        /// <param name="name"> Exception; is used if it is expected at least one scenario to match given name
        ///                     Used by ModifyScenario command. Default is 0 matches toleration </param> 
        /// <returns>true if no scenario with that name already exists,
        ///          false if a scenario with the same name already exists</returns>
        public bool validateScenario(string name, int exception = 0)
        {
            int matches = 0;

            for(int count = 0; count < MyScenarios.UserCalculationList.Count; count++)
            {
                string scenarioName = MyScenarios.UserCalculationList[count].Name;

                if (scenarioName.Equals(name)) //if exisiting scenario name equals name provided for validation
                {
                    matches++; //add match

                    if (matches > exception)
                    {
                        return false; //If hit maximum amount of exceptions return false as validation failed
                    }
                }
            }

            return true; //it is a unique name so return true
        }

        #region ICommands

        #region ScenarioCommands

        //Scenario Based Commands
        public ICommand AddScenarioCommand
        {
            get;
            private set;
        }

        public ICommand ModifyScenarioCommand
        {
            get;
            private set;
        }

        public ICommand RemoveScenarioCommand
        {
            get;
            private set;
        }

        public ICommand ToggleEfficiencyCommand
        {
            get;
            private set;
        }

        public ICommand CalculateDialogCommand
        {            
            get;
            private set;
        }

        public ICommand SaveScenarioCommand
        {
            get;
            private set;
        }

        public ICommand LoadScenarioCommand
        {
            get;
            private set;
        }

        #endregion ScenarioCommands

        #region DataGridCommands

        public ICommand AddDataDetailCommand
        {
            get;
            private set;
        }

        public ICommand RemoveDataDetailCommand
        {
            get;
            private set;
        }

        public ICommand CopyCommand
        {
            get;
            private set;
        }

        public ICommand CopyAllCommand
        {
            get;
            private set;
        }

        public ICommand PasteCommand
        {
            get;
            private set;
        }

        public ICommand PasteAllCommand
        {
            get;
            private set;
        }

        public ICommand VisualizeCommand
        {
            get;
            private set;
        }

        #endregion DataGridCommands

        #region DialogCommands

        public ICommand DialogAddPICommand
        {
            get;
            set;
        }

        public ICommand DialogModifyScenarioCommand
        {
            get;
            private set;
        }

        public ICommand DialogRemovePICommand
        {
            get;
            set;
        }

        public ICommand DialogAddScenariocommand
        {
            get;
            set;
        }

        public ICommand CalculateEfficiencyCommand
        {
            get;
            private set;
        }

        #endregion Dialogcommands

        #region MenuCommands

        public ICommand SaveSessionCommand
        {
            get;
            private set;
        }

        public ICommand LoadSessionCommand
        {
            get;
            private set;
        }

        #endregion MenuCommands

        #endregion ICommands
    }

    /* Model formatting for views
     *  - This region helps convert data given from user in AddScenarioDialog.xaml to model
     *      and vice-versa.
     *  - This region also helps convert data given from user in ModifyScenarioDialog.xaml to model
     *      and vice-versa.
     *      
     * Technical Note: In PRISM 5.0 NotificationObject is 'outdated' and replaced with BindableBase
     */
    public class ScenarioFormatHelper : BindableBase
    {
        private ScenarioDetail _ParentScenario;
        public ScenarioDetail ParentScenario
        {
            get
            {
                return _ParentScenario;
            }
            set
            {
                _ParentScenario = value;
                this.OnPropertyChanged(() => this.ParentScenario);
            }
        }

        private String _ScenarioName;
        public String ScenarioName
        {
            get
            {
                return _ScenarioName;
            }
            set
            {
                _ScenarioName = value;
                this.OnPropertyChanged(() => this.ScenarioName);
            }
        }        

        private String _MainTabName;
        public String MainTabName
        {
            get
            {
                return _MainTabName;
            }
            set
            {
                _MainTabName = value;
                this.OnPropertyChanged(() => this.MainTabName);
            }
        }

        private ObservableCollection<TempPerformanceIndicator> _PerformanceIndicators;
        public ObservableCollection<TempPerformanceIndicator> PerformanceIndicators
        {
            get
            {
                return _PerformanceIndicators;
            }
            set
            {
                _PerformanceIndicators = value;
                this.OnPropertyChanged(() => this.PerformanceIndicators);
            }
        }

        private TempPerformanceIndicator _SelectedPerformanceIndicator;
        public TempPerformanceIndicator SelectedPerformanceIndicator
        {
            get
            {
                return _SelectedPerformanceIndicator;
            }
            set
            {
                _SelectedPerformanceIndicator = value;
                this.OnPropertyChanged(() => this.SelectedPerformanceIndicator);
            }
        }

        public bool ScenarioNameChanged = true;

        public void clear()
        {
            ScenarioName = "";
            MainTabName = "";

            TempPerformanceIndicator FirstPerformanceIndicator = new TempPerformanceIndicator("Empty", 0);
            PerformanceIndicators = new ObservableCollection<TempPerformanceIndicator>();
            PerformanceIndicators.Add(FirstPerformanceIndicator);
            FirstPerformanceIndicator.Index = 1;

            SelectedPerformanceIndicator = new TempPerformanceIndicator("", 0);
        }

        //Removes selected PI - called by ICommand in DialogCommands.cs
        //Adjusts the index of PIs so logic still works when saved to the Model
        public void removeSelected()
        {
            bool marked = false;
            TempPerformanceIndicator toRemove = null;
            for (int c = 0; c < PerformanceIndicators.Count; c++)
            {
                if (marked == false && PerformanceIndicators[c].Equals(SelectedPerformanceIndicator))
                {
                    PerformanceIndicators[c].Index = -1; //mark index
                    marked = true;
                    toRemove = PerformanceIndicators[c]; //in case selection changed also
                }

                if (marked == true)
                {
                    PerformanceIndicators[c].Index = PerformanceIndicators[c].Index - 1; //decrease index by 1
                }
            }

            if (toRemove != null)
            {
                PerformanceIndicators.Remove(toRemove);
            }
        }

        //Used by Modify command in ScenarioCommand.cs
        public void setUp(ScenarioDetail parentScenario)
        {
            ParentScenario = parentScenario;
            ScenarioName = parentScenario.Name;
            MainTabName = parentScenario.MainTab.HeaderText;

            PerformanceIndicators = new ObservableCollection<TempPerformanceIndicator>();
            PerformanceIndicators.Add(new TempPerformanceIndicator(parentScenario.PerformanceIndicator1.HeaderText, parentScenario.PerformanceIndicator1.DataSet.Count));

            if (parentScenario.PerformanceIndicator2.TabVisibility)
                PerformanceIndicators.Add(new TempPerformanceIndicator(parentScenario.PerformanceIndicator2.HeaderText, parentScenario.PerformanceIndicator2.DataSet.Count));
            if (parentScenario.PerformanceIndicator3.TabVisibility)
                PerformanceIndicators.Add(new TempPerformanceIndicator(parentScenario.PerformanceIndicator3.HeaderText, parentScenario.PerformanceIndicator3.DataSet.Count));
            if (parentScenario.PerformanceIndicator4.TabVisibility)
                PerformanceIndicators.Add(new TempPerformanceIndicator(parentScenario.PerformanceIndicator4.HeaderText, parentScenario.PerformanceIndicator4.DataSet.Count));
            if (parentScenario.PerformanceIndicator5.TabVisibility)
                PerformanceIndicators.Add(new TempPerformanceIndicator(parentScenario.PerformanceIndicator5.HeaderText, parentScenario.PerformanceIndicator5.DataSet.Count));
            if (parentScenario.PerformanceIndicator6.TabVisibility)
                PerformanceIndicators.Add(new TempPerformanceIndicator(parentScenario.PerformanceIndicator6.HeaderText, parentScenario.PerformanceIndicator6.DataSet.Count));
            if (parentScenario.PerformanceIndicator7.TabVisibility)
                PerformanceIndicators.Add(new TempPerformanceIndicator(parentScenario.PerformanceIndicator7.HeaderText, parentScenario.PerformanceIndicator7.DataSet.Count));
            if (parentScenario.PerformanceIndicator8.TabVisibility)
                PerformanceIndicators.Add(new TempPerformanceIndicator(parentScenario.PerformanceIndicator8.HeaderText, parentScenario.PerformanceIndicator8.DataSet.Count));
            if (parentScenario.PerformanceIndicator9.TabVisibility)
                PerformanceIndicators.Add(new TempPerformanceIndicator(parentScenario.PerformanceIndicator9.HeaderText, parentScenario.PerformanceIndicator9.DataSet.Count));
            if (parentScenario.PerformanceIndicator10.TabVisibility)
                PerformanceIndicators.Add(new TempPerformanceIndicator(parentScenario.PerformanceIndicator10.HeaderText, parentScenario.PerformanceIndicator10.DataSet.Count));

            //set every TempPI's index based on count + 1 so it starts with Index 1
            for (int count = 0; count < PerformanceIndicators.Count; count++)
            {
                PerformanceIndicators[count].Index = count + 1;
            }
        }
    }

    /* Model formatting for views
     *  - This region helps convert data given from user in CalculateEfficiencyDialog.xaml to model
     *      and vice-versa.
     *     o Can take advantage of the unique scenario name during runtime and use reference to parent
     *       in command
     */
    public class ScenarioCalculateHelper : BindableBase
    {
        private ScenarioDetail _ParentScenario;
        public ScenarioDetail ParentScenario
        {
            get
            {
                return _ParentScenario;
            }
            set
            {
                _ParentScenario = value;
                this.OnPropertyChanged(() => this.ParentScenario);
            }
        }

        private string _YieldTextUnits;
        public string YieldTextUnits
        {
            get
            {
                return _YieldTextUnits;
            }
            set
            {
                _YieldTextUnits = value;
                this.OnPropertyChanged(() => this.YieldTextUnits);
            }
        }

        private ObservableCollection<TempPerformanceIndicator> _MyPerformanceIndicators;
        public ObservableCollection<TempPerformanceIndicator> MyPerformanceIndicators
        {
            get
            {
                return _MyPerformanceIndicators;
            }
            set
            {
                _MyPerformanceIndicators = value;
                this.OnPropertyChanged(() => this.MyPerformanceIndicators);
            }
        }

        public ScenarioCalculateHelper(ScenarioDetail parent)
        {
            ParentScenario = parent;            
        }

        //format self to match parent scenario details model
        public void setUp()
        {
            MyPerformanceIndicators = new ObservableCollection<TempPerformanceIndicator>();
            MyPerformanceIndicators.Add(new TempPerformanceIndicator(ParentScenario.PerformanceIndicator1.HeaderText, 0, 1));

            if (ParentScenario.PerformanceIndicator2.TabVisibility)
                MyPerformanceIndicators.Add(new TempPerformanceIndicator(ParentScenario.PerformanceIndicator2.HeaderText, 0, 1));
            if (ParentScenario.PerformanceIndicator3.TabVisibility) 
                MyPerformanceIndicators.Add(new TempPerformanceIndicator(ParentScenario.PerformanceIndicator3.HeaderText, 0, 1));
            if (ParentScenario.PerformanceIndicator4.TabVisibility) 
                MyPerformanceIndicators.Add(new TempPerformanceIndicator(ParentScenario.PerformanceIndicator4.HeaderText, 0, 1));
            if (ParentScenario.PerformanceIndicator5.TabVisibility) 
                MyPerformanceIndicators.Add(new TempPerformanceIndicator(ParentScenario.PerformanceIndicator5.HeaderText, 0, 1));
            if (ParentScenario.PerformanceIndicator6.TabVisibility) 
                MyPerformanceIndicators.Add(new TempPerformanceIndicator(ParentScenario.PerformanceIndicator6.HeaderText, 0, 1));
            if (ParentScenario.PerformanceIndicator7.TabVisibility) 
                MyPerformanceIndicators.Add(new TempPerformanceIndicator(ParentScenario.PerformanceIndicator7.HeaderText, 0, 1));
            if (ParentScenario.PerformanceIndicator8.TabVisibility) 
                MyPerformanceIndicators.Add(new TempPerformanceIndicator(ParentScenario.PerformanceIndicator8.HeaderText, 0, 1));
            if (ParentScenario.PerformanceIndicator9.TabVisibility) 
                MyPerformanceIndicators.Add(new TempPerformanceIndicator(ParentScenario.PerformanceIndicator9.HeaderText, 0, 1));
            if (ParentScenario.PerformanceIndicator10.TabVisibility) 
                MyPerformanceIndicators.Add(new TempPerformanceIndicator(ParentScenario.PerformanceIndicator10.HeaderText, 0, 1));

            //set every TempPI's index based on count + 1 so it starts with Index 1
            for (int count = 0; count < MyPerformanceIndicators.Count; count++)
            {
                MyPerformanceIndicators[count].Index = count + 1; 
            }

                YieldTextUnits = "[Amount]";
        }
    }
}
