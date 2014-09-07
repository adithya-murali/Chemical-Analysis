using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.ViewModel;
using Microsoft.Practices.Prism.Mvvm;
using System.Collections.ObjectModel;

namespace ProcessYieldOptimizer.Models
{
    /// <summary>
    /// ProcessYieldModel contains the objects for the program to function.
    ///  
    /// The program needs 'Scenario's object which contains
    ///     - SelectedScenario - The selected scenario by user
    ///     - UserCalculationList- List of 'ScenarioDetails' objects
    /// Each 'ScenarioDetail object contains:
    ///     - Name - Name of the Scenario
    ///     - MainTab - a 'TabDetail' object
    ///     - 10 Performance Indicators - 'TabDetail' objects
    ///     - YieldTab - a 'TabDetail' object
    ///     - SelectedDetailTab - Selected 'TabDetail' object based selected Tab by user
    ///     - SelectedTabIndex - the Selected index of 'TabDetail' in view by user adjusts SelectedDetailTab when changed
    /// Each 'TabDetail' object contains:
    ///     - Parent - public reference to the Scenario (more stuff with this later)
    ///     - HeaderText - string with name given by user
    ///     - TabVisibility - True if tab is visible / being used, false if hidden or not used
    ///     - DataSet - Collection of 'DataRowDetail' objects
    ///     - SelectedRowIndex - Selected Row in the visualization of the 'DataRowDetails', should be in the item number in collection list
    ///     - MinValue - public double only used by YieldTab but have some ideas for its use for other tabs later
    ///     - UnitsText - public string only used by YieldTab 
    /// Each 'DataRowDetail' object contains:
    ///     - Parent - public reference to the TabDetail (used in IndexConverter part of SingleValueConverter.cs)
    ///     - Index - column index to display by DataGrid (changes when rows are deleted in middle of datagrid - see DataGridCommands.cs for details)
    ///     - Column1 - Column10 - Double values for column 'A' thru 'J' in visualization of 'TabDetail' [Accepts only values > 0, since calcuation and logic doesnt make sense for negative values]
    ///     - Colum1Color - Column10Color - String containing 'Red', 'Blue', 'Green' or 'Transparent' to indicate color choice when 'Visualize' command is given
    ///     - Visualize - Bool true or false. If false multibinding converter will render colors as 'Transparent' for the respective cells in datagrid
    /// TempPerformanceIndicator object is a special object that is used in helping convert
    ///     user data in GUI to Model type structured data.
    ///     
    /// There are many ways to achieve the end goal of this project being done on free time. 
    /// Some more effective and some less, this was chosen since it seemed simple. 
    /// Other than PRISM from Microsoft tried to stay away from third party controls for this project.
    /// Any feedback is appreciated as this is a learning process.
    /// 
    /// Technical Note: In PRISM 5.0 NotificationObject is 'deprecated' and replaced with BindableBase
    /// </summary>
    
    public class Scenarios : BindableBase
    {
        private ObservableCollection<ScenarioDetail> _UserCalculationList;

        public ObservableCollection<ScenarioDetail> UserCalculationList
        {
            get
            {
                return _UserCalculationList;
            }
            set
            {
                _UserCalculationList = value;
                this.OnPropertyChanged(() => this.UserCalculationList);
            }
        }

        private ScenarioDetail _SelectedScenario;
        public ScenarioDetail SelectedScenario
        {
            get
            {
                return _SelectedScenario;
            }
            set
            {
                _SelectedScenario = value;
                this.OnPropertyChanged(() => this.SelectedScenario);
            }
        }

        public Scenarios()
        {
            UserCalculationList = new ObservableCollection<ScenarioDetail>();
        }

        //using add command from dialog
        public void addScenario(string scenario, TempPerformanceIndicator maintab, ObservableCollection<TempPerformanceIndicator> perfindicatorCollection)
        {
            ScenarioDetail newScenario = new ScenarioDetail(scenario);
            newScenario.MainTab = new TabDetail(scenario, maintab.Name);
            newScenario.MainTab.addRows(perfindicatorCollection[0].Rows);

            for (int count = 0; count < perfindicatorCollection.Count; count++)
            {
                TempPerformanceIndicator myPerfIndicator = perfindicatorCollection[count];

                switch(count)
                {
                    case 0:
                        newScenario.PerformanceIndicator1 = new TabDetail(scenario, myPerfIndicator.Name);
                        newScenario.PerformanceIndicator1.addRows(myPerfIndicator.Rows);
                        break;
                    case 1:
                        newScenario.PerformanceIndicator2 = new TabDetail(scenario, myPerfIndicator.Name);
                        newScenario.PerformanceIndicator2.addRows(myPerfIndicator.Rows);
                        break;
                    case 2:
                        newScenario.PerformanceIndicator3 = new TabDetail(scenario, myPerfIndicator.Name);
                        newScenario.PerformanceIndicator3.addRows(myPerfIndicator.Rows);
                        break;
                    case 3:
                        newScenario.PerformanceIndicator4 = new TabDetail(scenario, myPerfIndicator.Name);
                        newScenario.PerformanceIndicator4.addRows(myPerfIndicator.Rows);
                        break;
                    case 4:
                        newScenario.PerformanceIndicator5 = new TabDetail(scenario, myPerfIndicator.Name);
                        newScenario.PerformanceIndicator5.addRows(myPerfIndicator.Rows);
                        break;
                    case 5:
                        newScenario.PerformanceIndicator6 = new TabDetail(scenario, myPerfIndicator.Name);
                        newScenario.PerformanceIndicator6.addRows(myPerfIndicator.Rows);
                        break;
                    case 6:
                        newScenario.PerformanceIndicator7 = new TabDetail(scenario, myPerfIndicator.Name);
                        newScenario.PerformanceIndicator7.addRows(myPerfIndicator.Rows);
                        break;
                    case 7:
                        newScenario.PerformanceIndicator8 = new TabDetail(scenario, myPerfIndicator.Name);
                        newScenario.PerformanceIndicator8.addRows(myPerfIndicator.Rows);
                        break;
                    case 8:
                        newScenario.PerformanceIndicator9 = new TabDetail(scenario, myPerfIndicator.Name);
                        newScenario.PerformanceIndicator9.addRows(myPerfIndicator.Rows);
                        break;
                    case 9:
                        newScenario.PerformanceIndicator10 = new TabDetail(scenario, myPerfIndicator.Name);
                        newScenario.PerformanceIndicator10.addRows(myPerfIndicator.Rows);
                        break;
                }
            }
            newScenario.SelectedDetailTab = newScenario.MainTab; //default select first tab
            UserCalculationList.Add(newScenario);
        }

        //using load command 
        public void addScenario(string scenario, TabDetail maintab, List<TabDetail> perfindicatorCollection)
        {
            ScenarioDetail newScenario = new ScenarioDetail(scenario);
            newScenario.MainTab = maintab;

            for (int count = 0; count < perfindicatorCollection.Count; count++)
            {
                TabDetail myPerfIndicator = perfindicatorCollection[count];

                switch (count)
                {
                    case 0:
                        newScenario.PerformanceIndicator1 = myPerfIndicator;
                        break;
                    case 1:
                        newScenario.PerformanceIndicator2 = myPerfIndicator;
                        break;
                    case 2:
                        newScenario.PerformanceIndicator3 = myPerfIndicator;
                        break;
                    case 3:
                        newScenario.PerformanceIndicator4 = myPerfIndicator;
                        break;
                    case 4:
                        newScenario.PerformanceIndicator5 = myPerfIndicator;
                        break;
                    case 5:
                        newScenario.PerformanceIndicator6 = myPerfIndicator;
                        break;
                    case 6:
                        newScenario.PerformanceIndicator7 = myPerfIndicator;
                        break;
                    case 7:
                        newScenario.PerformanceIndicator8 = myPerfIndicator;
                        break;
                    case 8:
                        newScenario.PerformanceIndicator9 = myPerfIndicator;
                        break;
                    case 9:
                        newScenario.PerformanceIndicator10 = myPerfIndicator;
                        break;
                }
            }
            newScenario.SelectedDetailTab = newScenario.MainTab; //default select first tab
            UserCalculationList.Add(newScenario);
        }

        //Assuming everything is passing validation and we have unique scenario names (case sensitive)
        public void modifyScenario(string origScenarioName, string newScenarioName, TempPerformanceIndicator maintab, ObservableCollection<TempPerformanceIndicator> perfindicatorCollection)
        {
            ScenarioDetail myScenario = null;

            for (int count = 0; count < UserCalculationList.Count; count++)
            {
                if (UserCalculationList[count].Name.Equals(origScenarioName))
                {
                    myScenario = UserCalculationList[count];
                    break;
                }
            }

            if (myScenario != null)
            {
                myScenario.Name = newScenarioName;
                myScenario.MainTab.HeaderText = maintab.Name; //just change header text

                //turn off visibility of all tabs but PI 1 tab
                myScenario.PerformanceIndicator2.TabVisibility = false;
                myScenario.PerformanceIndicator3.TabVisibility = false;
                myScenario.PerformanceIndicator4.TabVisibility = false;
                myScenario.PerformanceIndicator5.TabVisibility = false;
                myScenario.PerformanceIndicator6.TabVisibility = false;
                myScenario.PerformanceIndicator7.TabVisibility = false;
                myScenario.PerformanceIndicator8.TabVisibility = false;
                myScenario.PerformanceIndicator9.TabVisibility = false;
                myScenario.PerformanceIndicator10.TabVisibility = false;

                for (int count = 0; count < perfindicatorCollection.Count; count++)
                {
                    TempPerformanceIndicator myPerfIndicator = perfindicatorCollection[count];             

                    switch (count)
                    {
                        case 0:
                            myScenario.PerformanceIndicator1.HeaderText = myPerfIndicator.Name;
                            myScenario.PerformanceIndicator1.TabVisibility = true; //set visibility true
                            if (myScenario.PerformanceIndicator1.DataSet.Count < myPerfIndicator.Rows) //add rows if modified to have more rows
                            {
                                myScenario.PerformanceIndicator1.addRows(myPerfIndicator.Rows - myScenario.PerformanceIndicator1.DataSet.Count);
                            }
                            else if (myScenario.PerformanceIndicator1.DataSet.Count > myPerfIndicator.Rows)
                            {
                                myScenario.PerformanceIndicator1.deleteRows(myPerfIndicator.Rows);
                            }
                            break;
                        case 1:
                            myScenario.PerformanceIndicator2.HeaderText = myPerfIndicator.Name;
                            myScenario.PerformanceIndicator2.TabVisibility = true; //set visibility true
                            if (myScenario.PerformanceIndicator2.DataSet.Count < myPerfIndicator.Rows) //add rows if modified to have more rows
                            {
                                myScenario.PerformanceIndicator2.addRows(myPerfIndicator.Rows - myScenario.PerformanceIndicator2.DataSet.Count);
                            }
                            else if (myScenario.PerformanceIndicator2.DataSet.Count > myPerfIndicator.Rows)
                            {
                                myScenario.PerformanceIndicator2.deleteRows(myPerfIndicator.Rows);
                            }
                            break;
                        case 2:
                            myScenario.PerformanceIndicator3.HeaderText = myPerfIndicator.Name;
                            myScenario.PerformanceIndicator3.TabVisibility = true; //set visibility true
                            if (myScenario.PerformanceIndicator3.DataSet.Count < myPerfIndicator.Rows) //add rows if modified to have more rows
                            {
                                myScenario.PerformanceIndicator3.addRows(myPerfIndicator.Rows - myScenario.PerformanceIndicator3.DataSet.Count);
                            }
                            else if (myScenario.PerformanceIndicator3.DataSet.Count > myPerfIndicator.Rows)
                            {
                                myScenario.PerformanceIndicator3.deleteRows(myPerfIndicator.Rows);
                            }
                            break;
                        case 3:
                            myScenario.PerformanceIndicator4.HeaderText = myPerfIndicator.Name;
                            myScenario.PerformanceIndicator4.TabVisibility = true; //set visibility true
                            if (myScenario.PerformanceIndicator4.DataSet.Count < myPerfIndicator.Rows) //add rows if modified to have more rows
                            {
                                myScenario.PerformanceIndicator4.addRows(myPerfIndicator.Rows - myScenario.PerformanceIndicator4.DataSet.Count);
                            }
                            else if (myScenario.PerformanceIndicator4.DataSet.Count > myPerfIndicator.Rows)
                            {
                                myScenario.PerformanceIndicator4.deleteRows(myPerfIndicator.Rows);
                            }
                            break;
                        case 4:
                            myScenario.PerformanceIndicator5.HeaderText = myPerfIndicator.Name;
                            myScenario.PerformanceIndicator5.TabVisibility = true; //set visibility true
                            if (myScenario.PerformanceIndicator5.DataSet.Count < myPerfIndicator.Rows) //add rows if modified to have more rows
                            {
                                myScenario.PerformanceIndicator5.addRows(myPerfIndicator.Rows - myScenario.PerformanceIndicator5.DataSet.Count);
                            }
                            else if (myScenario.PerformanceIndicator5.DataSet.Count > myPerfIndicator.Rows)
                            {
                                myScenario.PerformanceIndicator5.deleteRows(myPerfIndicator.Rows);
                            }
                            break;
                        case 5:
                            myScenario.PerformanceIndicator6.HeaderText = myPerfIndicator.Name;
                            myScenario.PerformanceIndicator6.TabVisibility = true; //set visibility true
                            if (myScenario.PerformanceIndicator6.DataSet.Count < myPerfIndicator.Rows) //add rows if modified to have more rows
                            {
                                myScenario.PerformanceIndicator6.addRows(myPerfIndicator.Rows - myScenario.PerformanceIndicator6.DataSet.Count);
                            }
                            else if (myScenario.PerformanceIndicator6.DataSet.Count > myPerfIndicator.Rows)
                            {
                                myScenario.PerformanceIndicator6.deleteRows(myPerfIndicator.Rows);
                            }
                            break;
                        case 6:
                            myScenario.PerformanceIndicator7.HeaderText = myPerfIndicator.Name;
                            myScenario.PerformanceIndicator7.TabVisibility = true; //set visibility true
                            if (myScenario.PerformanceIndicator7.DataSet.Count < myPerfIndicator.Rows) //add rows if modified to have more rows
                            {
                                myScenario.PerformanceIndicator7.addRows(myPerfIndicator.Rows - myScenario.PerformanceIndicator7.DataSet.Count);
                            }
                            else if (myScenario.PerformanceIndicator7.DataSet.Count > myPerfIndicator.Rows)
                            {
                                myScenario.PerformanceIndicator7.deleteRows(myPerfIndicator.Rows);
                            }
                            break;
                        case 7:
                            myScenario.PerformanceIndicator8.HeaderText = myPerfIndicator.Name;
                            myScenario.PerformanceIndicator8.TabVisibility = true; //set visibility true
                            if (myScenario.PerformanceIndicator8.DataSet.Count < myPerfIndicator.Rows) //add rows if modified to have more rows
                            {
                                myScenario.PerformanceIndicator8.addRows(myPerfIndicator.Rows - myScenario.PerformanceIndicator8.DataSet.Count);
                            }
                            else if (myScenario.PerformanceIndicator8.DataSet.Count > myPerfIndicator.Rows)
                            {
                                myScenario.PerformanceIndicator8.deleteRows(myPerfIndicator.Rows);
                            }
                            break;
                        case 8:
                            myScenario.PerformanceIndicator9.HeaderText = myPerfIndicator.Name;
                            myScenario.PerformanceIndicator9.TabVisibility = true; //set visibility true
                            if (myScenario.PerformanceIndicator9.DataSet.Count < myPerfIndicator.Rows) //add rows if modified to have more rows
                            {
                                myScenario.PerformanceIndicator9.addRows(myPerfIndicator.Rows - myScenario.PerformanceIndicator9.DataSet.Count);
                            }
                            else if (myScenario.PerformanceIndicator9.DataSet.Count > myPerfIndicator.Rows)
                            {
                                myScenario.PerformanceIndicator9.deleteRows(myPerfIndicator.Rows);
                            }
                            break;
                        case 9:
                            myScenario.PerformanceIndicator10.HeaderText = myPerfIndicator.Name;
                            myScenario.PerformanceIndicator10.TabVisibility = true; //set visibility true
                            if (myScenario.PerformanceIndicator10.DataSet.Count < myPerfIndicator.Rows) //add rows if modified to have more rows
                            {
                                myScenario.PerformanceIndicator10.addRows(myPerfIndicator.Rows - myScenario.PerformanceIndicator10.DataSet.Count);
                            }
                            else if (myScenario.PerformanceIndicator10.DataSet.Count > myPerfIndicator.Rows)
                            {
                                myScenario.PerformanceIndicator10.deleteRows(myPerfIndicator.Rows);
                            }
                            break;
                    }
                }
            }
            else
                throw new Exception("Unable to find scenario to modify");
        }

        public void deleteScenario()
        {
            UserCalculationList.Remove(SelectedScenario);
        }
    }

    public class ScenarioDetail : BindableBase
    {
        private String _Name;
        public String Name
        {
            get
            {
                return _Name;
            }
            set
            {
                _Name = value;
                this.OnPropertyChanged(() => this.Name);
            }
        }

        private TabDetail _MainTab;
        public TabDetail MainTab
        {
            get
            {
                return _MainTab;                
            }
            set
            {
                _MainTab = value;
                this.OnPropertyChanged(() => this.MainTab);
            }
        }

        private TabDetail _PerformanceIndicator1;
        public TabDetail PerformanceIndicator1
        {
            get
            {
                return _PerformanceIndicator1;
            }
            set
            {
                _PerformanceIndicator1 = value;
                this.OnPropertyChanged(() => this.PerformanceIndicator1);
            }
        }

        private TabDetail _PerformanceIndicator2;
        public TabDetail PerformanceIndicator2
        {
            get
            {
                return _PerformanceIndicator2;
            }
            set
            {
                _PerformanceIndicator2 = value;
                this.OnPropertyChanged(() => this.PerformanceIndicator2);
            }
        }

        private TabDetail _PerformanceIndicator3;
        public TabDetail PerformanceIndicator3
        {
            get
            {
                return _PerformanceIndicator3;
            }
            set
            {
                _PerformanceIndicator3 = value;
                this.OnPropertyChanged(() => this.PerformanceIndicator3);
            }
        }

        private TabDetail _PerformanceIndicator4;
        public TabDetail PerformanceIndicator4
        {
            get
            {
                return _PerformanceIndicator4;
            }
            set
            {
                _PerformanceIndicator4 = value;
                this.OnPropertyChanged(() => this.PerformanceIndicator4);
            }
        }

        private TabDetail _PerformanceIndicator5;
        public TabDetail PerformanceIndicator5
        {
            get
            {
                return _PerformanceIndicator5;
            }
            set
            {
                _PerformanceIndicator5 = value;
                this.OnPropertyChanged(() => this.PerformanceIndicator5);
            }
        }

        private TabDetail _PerformanceIndicator6;
        public TabDetail PerformanceIndicator6
        {
            get
            {
                return _PerformanceIndicator6;
            }
            set
            {
                _PerformanceIndicator6 = value;
                this.OnPropertyChanged(() => this.PerformanceIndicator6);
            }
        }

        private TabDetail _PerformanceIndicator7;
        public TabDetail PerformanceIndicator7
        {
            get
            {
                return _PerformanceIndicator7;
            }
            set
            {
                _PerformanceIndicator7 = value;
                this.OnPropertyChanged(() => this.PerformanceIndicator7);
            }
        }


        private TabDetail _PerformanceIndicator8;
        public TabDetail PerformanceIndicator8
        {
            get
            {
                return _PerformanceIndicator8;
            }
            set
            {
                _PerformanceIndicator8 = value;
                this.OnPropertyChanged(() => this.PerformanceIndicator8);
            }
        }

        private TabDetail _PerformanceIndicator9;
        public TabDetail PerformanceIndicator9
        {
            get
            {
                return _PerformanceIndicator9;
            }
            set
            {
                _PerformanceIndicator9 = value;
                this.OnPropertyChanged(() => this.PerformanceIndicator9);
            }
        }

        private TabDetail _PerformanceIndicator10;
        public TabDetail PerformanceIndicator10
        {
            get
            {
                return _PerformanceIndicator10;
            }
            set
            {
                _PerformanceIndicator10 = value;
                this.OnPropertyChanged(() => this.PerformanceIndicator10);
            }
        }

        private TabDetail _YieldTab;
        public TabDetail YieldTab
        {
            get
            {
                return _YieldTab;
            }
            set
            {
                _YieldTab = value;
                this.OnPropertyChanged(() => this.YieldTab);
            }
        }

        private TabDetail _SelectedDetailTab;
        public TabDetail SelectedDetailTab
        {
            get
            {
                return _SelectedDetailTab;
            }
            set
            {
                _SelectedDetailTab = value;
                this.OnPropertyChanged(() => this.SelectedDetailTab);
            }
        }   

        private int _SelectedTabIndex;
        public int SelectedTabIndex
        {
            get
            {
                return _SelectedTabIndex;
            }
            set
            {
                _SelectedTabIndex = value;

                //Adjust Tabs - added later in the design of code...can be replaced with converter later.
                switch(_SelectedTabIndex)
                {
                    case 0:
                         SelectedDetailTab = MainTab;
                        break;
                    case 1:
                        SelectedDetailTab = PerformanceIndicator1;
                        break;
                    case 2:
                        SelectedDetailTab = PerformanceIndicator2;
                        break;
                    case 3:
                        SelectedDetailTab = PerformanceIndicator3;
                        break;
                    case 4:
                        SelectedDetailTab = PerformanceIndicator4;
                        break;
                    case 5:
                        SelectedDetailTab = PerformanceIndicator5;
                        break;
                    case 6:
                        SelectedDetailTab = PerformanceIndicator6;
                        break;
                    case 7:
                        SelectedDetailTab = PerformanceIndicator7;
                        break;
                    case 8:
                        SelectedDetailTab = PerformanceIndicator8;
                        break;
                    case 9:
                        SelectedDetailTab = PerformanceIndicator9;
                        break;
                    case 10:
                        SelectedDetailTab = PerformanceIndicator10;
                        break;
                    case 11:
                        SelectedDetailTab = YieldTab;
                        break;
                }

                this.OnPropertyChanged(() => this.SelectedTabIndex);
            }
        }

        public ScenarioDetail(string scenario)
        {
            Name = scenario; 
            MainTab = new TabDetail(Name, "Main");
            PerformanceIndicator1 = new TabDetail(Name, "PI1");

            //Default only one PI is required
            PerformanceIndicator2 = new TabDetail(Name, "", false);
            PerformanceIndicator3 = new TabDetail(Name, "", false);
            PerformanceIndicator4 = new TabDetail(Name, "", false);
            PerformanceIndicator5 = new TabDetail(Name, "", false);
            PerformanceIndicator6 = new TabDetail(Name, "", false);
            PerformanceIndicator7 = new TabDetail(Name, "", false);
            PerformanceIndicator8 = new TabDetail(Name, "", false);
            PerformanceIndicator9 = new TabDetail(Name, "", false);
            PerformanceIndicator10 = new TabDetail(Name, "", false);

            YieldTab = new TabDetail(Name, "", false); //By default set Yield Tab to invisible
        }
    }

    public class TabDetail : BindableBase
    {
        public string Parent;
        public double MinValue; //Property now used only by YieldTab
        public string UnitsText; //Property now used ony by YieldTab (future abstraction can be consideration)

        private string _HeaderText;
        public string HeaderText
        {
            get
            {
                return _HeaderText;
            }
            set
            {
                _HeaderText = value;
                this.OnPropertyChanged(() => this.HeaderText);
            }
        }

        private ObservableCollection<DataRowDetail> _DataSet;
        public ObservableCollection<DataRowDetail> DataSet
        {
            get
            {
                return _DataSet;
            }
            set
            {
                _DataSet = value;
                this.OnPropertyChanged(() => this.DataSet);
            }
        }


        private int _SelectedRowIndex;
        public int SelectedRowIndex
        {
            get
            {
                return _SelectedRowIndex;
            }
            set
            {
                _SelectedRowIndex = value;
                this.OnPropertyChanged(() => this.SelectedRowIndex);
            }
        }

        private bool _TabVisibility;
        public bool TabVisibility
        {
            get
            {
                return _TabVisibility;
            }
            set
            {
                _TabVisibility = value;
                this.OnPropertyChanged(() => this.TabVisibility);
            }
        }


        private bool _Visualize;
        public bool Visualize
        {
            get
            {
                return _Visualize;
            }
            set
            {
                _Visualize = value;
                this.OnPropertyChanged(() => this.Visualize);
            }
        }  

        public TabDetail(string parent, string headerText, bool visibile = true)
        {
            this.Parent = parent; //Gives some structure to tab details for possible future organization features etc.
            //Note currently scenarios with same name can be created

            HeaderText = headerText;
            
            DataSet = new ObservableCollection<DataRowDetail>();
            TabVisibility = visibile; //default visible

            SelectedRowIndex = -1; //default nothing selected            
        }

        //Used by add and modify scenario method to add rows to blank rows
        public void addRows(int rows)
        {
            for (int c = 0; c < rows; c++)
            {
                DataSet.Add(new DataRowDetail(this));
            }
        }

        //Used by modify scenario method to delete extra rows
        public void deleteRows(int maxRow)
        {
            ObservableCollection<DataRowDetail> tempCollection = new ObservableCollection<DataRowDetail>();
            for (int c = 0; c < maxRow; c++)
            {
                tempCollection.Add(DataSet[c]);
            }

            DataSet = tempCollection; 
        }
    }

    public class DataRowDetail : BindableBase
    {
        private TabDetail _Parent;
        public TabDetail Parent
        {
            get
            {
                return _Parent;
            }
            set
            {
                _Parent = value;
                this.OnPropertyChanged(() => this.Parent);
            }
        }

        private int _Index;
        public int Index
        {
            get
            {
                return _Index;
            }
            set
            {
                _Index = value;
                this.OnPropertyChanged(() => this.Index);
            }
        }

        private double _Column1;
        public double Column1
        {
            get
            {
                return _Column1;
            }
            set
            {
                if (value > 0)
                    _Column1 = value;
                else
                    _Column1 = 0;

                this.OnPropertyChanged(() => this.Column1);
            }
        }

        private String _Column1Color;
        public String Column1Color
        {
            get
            {
                return _Column1Color;
            }
            set
            {
                _Column1Color = value;
                this.OnPropertyChanged(() => this.Column1Color);
            }
        }

        private double _Column2;
        public double Column2
        {
            get
            {
                return _Column2;
            }
            set
            {
                if (value > 0)
                    _Column2 = value;
                else
                    _Column2 = 0;

                this.OnPropertyChanged(() => this.Column2);
            }
        }

        private String _Column2Color;
        public String Column2Color
        {
            get
            {
                return _Column2Color;
            }
            set
            {
                _Column2Color = value;
                this.OnPropertyChanged(() => this.Column2Color);
            }
        }

        private double _Column3;
        public double Column3
        {
            get
            {
                return _Column3;
            }
            set
            {
                if (value > 0)
                    _Column3 = value;
                else
                    _Column3 = 0;
                this.OnPropertyChanged(() => this.Column3);
            }
        }

        private String _Column3Color;
        public String Column3Color
        {
            get
            {
                return _Column3Color;
            }
            set
            {
                _Column3Color = value;
                this.OnPropertyChanged(() => this.Column3Color);
            }
        }

        private double _Column4;
        public double Column4
        {
            get
            {
                return _Column4;
            }
            set
            {
                if (value > 0)
                    _Column4 = value;
                else
                    _Column4 = 0;
                this.OnPropertyChanged(() => this.Column4);
            }
        }

        private String _Column4Color;
        public String Column4Color
        {
            get
            {
                return _Column4Color;
            }
            set
            {
                _Column4Color = value;
                this.OnPropertyChanged(() => this.Column4Color);
            }
        }

        private double _Column5;
        public double Column5
        {
            get
            {
                return _Column5;
            }
            set
            {
                if (value > 0)
                    _Column5 = value;
                else
                    _Column5 = 0;
                this.OnPropertyChanged(() => this.Column5);
            }
        }

        private String _Column5Color;
        public String Column5Color
        {
            get
            {
                return _Column5Color;
            }
            set
            {
                _Column5Color = value;
                this.OnPropertyChanged(() => this.Column5Color);
            }
        }

        private double _Column6;
        public double Column6
        {
            get
            {
                return _Column6;
            }
            set
            {
                if (value > 0)
                    _Column6 = value;
                else
                    _Column6 = 0;
                this.OnPropertyChanged(() => this.Column6);
            }
        }

        private String _Column6Color;
        public String Column6Color
        {
            get
            {
                return _Column6Color;
            }
            set
            {
                _Column6Color = value;
                this.OnPropertyChanged(() => this.Column6Color);
            }
        }

        private double _Column7;
        public double Column7
        {
            get
            {
                return _Column7;
            }
            set
            {
                if (value > 0)
                    _Column7 = value;
                else
                    _Column7 = 0;
                this.OnPropertyChanged(() => this.Column7);
            }
        }

        private String _Column7Color;
        public String Column7Color
        {
            get
            {
                return _Column7Color;
            }
            set
            {
                _Column7Color = value;
                this.OnPropertyChanged(() => this.Column7Color);
            }
        }

        private double _Column8;
        public double Column8
        {
            get
            {
                return _Column8;
            }
            set
            {
                if (value > 0)
                    _Column8 = value;
                else
                    _Column8 = 0;
                this.OnPropertyChanged(() => this.Column8);
            }
        }

        private String _Column8Color;
        public String Column8Color
        {
            get
            {
                return _Column8Color;
            }
            set
            {
                _Column8Color = value;
                this.OnPropertyChanged(() => this.Column8Color);
            }
        }

        private double _Column9;
        public double Column9
        {
            get
            {
                return _Column9;
            }
            set
            {
                if (value > 0)
                    _Column9 = value;
                else
                    _Column9 = 0;
                this.OnPropertyChanged(() => this.Column9);
            }
        }

        private String _Column9Color;
        public String Column9Color
        {
            get
            {
                return _Column9Color;
            }
            set
            {
                _Column9Color = value;
                this.OnPropertyChanged(() => this.Column9Color);
            }
        }

        private double _Column10;
        public double Column10
        {
            get
            {
                return _Column10;
            }
            set
            {
                if (value > 0)
                    _Column10 = value;
                else
                    _Column10 = 0;
                this.OnPropertyChanged(() => this.Column10);
            }
        }

        private String _Column10Color;
        public String Column10Color
        {
            get
            {
                return _Column10Color;
            }
            set
            {
                _Column10Color = value;
                this.OnPropertyChanged(() => this.Column10Color);
            }
        }

        //Used so progam doesnt keep setting all data grids at same time
        private bool _Visualize;
        public bool Visualize
        {
            get
            {
                return _Visualize;
            }
            set
            {
                _Visualize = value;
                this.OnPropertyChanged(() => this.Visualize);
            }
        }

        public DataRowDetail(TabDetail parent)
        {
            Parent = parent; //Similar to TabDetail gives some structure to DataDetail for possible future organization features etc.
            //Currently used in IndexConverter - SingleValueConverter.cs
            Index = Parent.DataSet.Count + 1;
        }        
    }
    
    /*
     * TempPerformanceIndicator object is a special object that is used in helping convert
     * user data in GUI to Model type structured data.
     */
    public class TempPerformanceIndicator : BindableBase
    {
        private int _Index;
        public int Index
        {
            get
            {
                return _Index;
            }
            set
            {
                _Index = value;
                this.OnPropertyChanged(() => this.Index);
            }
        }

        private int _Rows;
        public int Rows //Used by ScenarioFormatHelper
        {
            get
            {
                return _Rows;
            }
            set
            {
                _Rows = value;
                this.OnPropertyChanged(() => this.Rows);
            }
        }

        private double _Weight; //only between 0 and 1
        public double Weight //used by ScenarioCalculateHelper
        {
            get
            {
                return _Weight;
            }
            set
            {
                if (value > 1)
                    _Weight = 1;
                else if (value < 0)
                    _Weight = 0;
                else
                    _Weight = value;

                this.OnPropertyChanged(() => this.Weight);
            }
        }

        private String _Name;
        public String Name
        {
            get
            {
                return _Name;
            }
            set
            {
                _Name = value;
                this.OnPropertyChanged(() => this.Name);
            }
        }

        public TempPerformanceIndicator(string name, int rows = 0, double weight = 1)
        {
            Name = name;
            Rows = rows; //default empty sheet
            Weight = weight; //default all weigh same
        }
    }
}
