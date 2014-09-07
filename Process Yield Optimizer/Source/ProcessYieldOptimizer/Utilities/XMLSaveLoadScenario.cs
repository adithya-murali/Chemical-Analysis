using ProcessYieldOptimizer.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

/* Helps Load and Save individual Scenarios 
 * - Does not save calculation tab due to compatibility as chagnes are expected in calculation tab and visualization property
 * - Uses Add scenario method in model to added load scenario
 * - XML file generated is not encrypted and any advanced excel user can load the XML data onto spreadsheet
 *   o With other 3rd party tools or soon to be developed tools same can be done with .csv files and google sheets
 *   o This way we can support multiple parts
 *   o Encryption (such as using AES) is an option if need to expand
 *  
 * Methods
 * - setFilePath
 * - ReadFile()
 * - CreateFile()
 * - generateXMLFile()
 * - loadXMLFile(XElement element)
 *   o element is passed on by ReadFile() which makes sure 'PYOScenario' element exists in XML file 
 *   
 * Users should refrain from editing XML manually despite try catch statements
 * 
 * Basic Logic in english:
 *  Every valid XML file should have a PYOScenario element which contains the following elements
 *      - Version (of program that it belongs to)
 *      - ScenarioName
 *      - MainTab
 *         - Name
 *         - DataSet
 *      - Performance Indicator Tab1
 *         - Name
 *         - DataSet
 *         - Visibility
 *      - Repeat till Performance Indicator Tab10
 *      - See snippet of an example below
 *      <PYOScenario>
 *         <Version>Process Yield Optimizer Version 1.0</Version>
 *         <ScenarioName>Empty</ScenarioName>
 *         <MainTab>
 *           <TabName>Main</TabName>
 *           <DataSet>
 *             <Row1>
 *               <A>0</A>
 *               <B>0</B>
 *               <C>0</C>
 *               <D>0</D>
 *               <E>0</E>
 *               <F>0</F>
 *               <G>0</G>
 *               <H>0</H>
 *               <I>0</I>
 *               <J>0</J>
 *             </Row1>
 */
namespace ProcessYieldOptimizer.Utilities
{
    public class XMLSaveLoadScenario
    {
        private string _FilePath = "";

        public void setFilePath(string filePath)
        {
            _FilePath = filePath;
        }

        //Read XML and set settings 
        public string ReadFile()
        {
            try
            {
                if (_FilePath != "")
                {
                    if (File.Exists(_FilePath))
                    {
                        XDocument xDoc = XDocument.Load(_FilePath, LoadOptions.SetBaseUri | LoadOptions.SetLineInfo);
                        loadXMLFile(xDoc.Element("PYOScenario"));
                    }
                    else
                    {
                        CreateFile(); //Create a file then load static settings
                        XDocument xDoc = XDocument.Load(_FilePath, LoadOptions.SetBaseUri | LoadOptions.SetLineInfo);
                        loadXMLFile(xDoc.Element("PYOScenario"));
                    }
                }
                else
                    throw new Exception("Filename is empty!");

            }
            catch (Exception e)
            {
                return e.ToString();
            }

            return "";
        }

        //Create XML with settings
        public string CreateFile()
        {
            if (_FilePath != "")
            {
                try
                {

                    XDocument xDoc = new XDocument(new XDeclaration("1.0", "UTF-8", "yes"));

                    System.Diagnostics.Debug.WriteLine("Line : " + _FilePath);
                    xDoc.Add(new XElement(generateXMLFile()));
                    if (File.Exists(_FilePath))
                    {
                        FileAttributes attributes = File.GetAttributes(_FilePath);
                        if (attributes == FileAttributes.ReadOnly)
                        {
                            File.SetAttributes(_FilePath, FileAttributes.Normal);
                        }
                        File.Delete(_FilePath);
                    }
                    xDoc.Save(_FilePath);
                }
                catch (Exception ex)
                {
                    return ex.ToString(); //return exception
                }

                return "";
            }
            else
                throw new Exception("Filename is empty!");

        }

        public XElement generateXMLFile()
        {
            XElement element = new XElement("PYOScenario");
            Version = StaticConfiguration.settingsVersion; //set version based on static configuration

            element.Add(new XElement("Version", Version));
            element.Add(new XElement("ScenarioName", ScenarioName));

            //Construct the MainTab element
            XElement mainTabElement = new XElement("MainTab");
            mainTabElement.Add(new XElement("TabName", MainTab.HeaderText)); //Put Header name

            //Construct DataSet element for the MainTab
            XElement mainTabDataSet = new XElement("DataSet");

            for (int row = 0; row < MainTab.DataSet.Count; row++)
            {
                XElement DataRow = new XElement("Row" + (row + 1)); //construct datarow
                //add 10 column values to it
                DataRow.Add(new XElement("A", MainTab.DataSet[row].Column1));
                DataRow.Add(new XElement("B", MainTab.DataSet[row].Column2));
                DataRow.Add(new XElement("C", MainTab.DataSet[row].Column3));
                DataRow.Add(new XElement("D", MainTab.DataSet[row].Column4));
                DataRow.Add(new XElement("E", MainTab.DataSet[row].Column5));
                DataRow.Add(new XElement("F", MainTab.DataSet[row].Column6));
                DataRow.Add(new XElement("G", MainTab.DataSet[row].Column7));
                DataRow.Add(new XElement("H", MainTab.DataSet[row].Column8));
                DataRow.Add(new XElement("I", MainTab.DataSet[row].Column9));
                DataRow.Add(new XElement("J", MainTab.DataSet[row].Column10));
                //Add datarow to maintabdataset

                mainTabDataSet.Add(DataRow);
            }

            mainTabElement.Add(mainTabDataSet); //add maintab dataset to the element
            element.Add(mainTabElement); //add maintab element to the main element

            //Construct the Performance Tabs element

            for (int c = 0; c < PerformanceIndicators.Count; c++)
            {
                XElement piTabElement = new XElement("PerformanceIndicator" + (c+1));
                piTabElement.Add(new XElement("TabName", PerformanceIndicators[c].HeaderText)); //Put Header name

                //Construct DataSet element for the MainTab
                XElement piTabDataSet = new XElement("DataSet");

                for (int row = 0; row < PerformanceIndicators[c].DataSet.Count; row++)
                {
                    XElement DataRow = new XElement("Row" + (row + 1)); //construct datarow
                    //add 10 column values to it
                    DataRow.Add(new XElement("A", PerformanceIndicators[c].DataSet[row].Column1));
                    DataRow.Add(new XElement("B", PerformanceIndicators[c].DataSet[row].Column2));
                    DataRow.Add(new XElement("C", PerformanceIndicators[c].DataSet[row].Column3));
                    DataRow.Add(new XElement("D", PerformanceIndicators[c].DataSet[row].Column4));
                    DataRow.Add(new XElement("E", PerformanceIndicators[c].DataSet[row].Column5));
                    DataRow.Add(new XElement("F", PerformanceIndicators[c].DataSet[row].Column6));
                    DataRow.Add(new XElement("G", PerformanceIndicators[c].DataSet[row].Column7));
                    DataRow.Add(new XElement("H", PerformanceIndicators[c].DataSet[row].Column8));
                    DataRow.Add(new XElement("I", PerformanceIndicators[c].DataSet[row].Column9));
                    DataRow.Add(new XElement("J", PerformanceIndicators[c].DataSet[row].Column10));
                    //Add datarow to piTabDataSet

                    piTabDataSet.Add(DataRow);
                }

                piTabElement.Add(piTabDataSet); //add PI dataset to the element
                piTabElement.Add(new XElement("Visibility", PerformanceIndicators[c].TabVisibility)); //Put Visbility
                element.Add(piTabElement); //add PI element to the main element
            }

            return element;
        }

        public void loadXMLFile(XElement element)
        {
            Version = (element.Element("Version").Value);
            ScenarioName = (element.Element("ScenarioName").Value);

            //Read the MainTab element
            XElement mainTabElement = (element.Element("MainTab"));

            MainTab = new TabDetail(ScenarioName, ""); //create maintab
            //Assemble maintab
            MainTab.HeaderText = mainTabElement.Element("TabName").Value;

            //Assemble DataSet for the MainTab
            XElement mainTabDataSet = mainTabElement.Element("DataSet");
            int row = 0;
            while(mainTabDataSet.HasElements) //While there are still additional child datarow elements
            {
                XElement DataRow = mainTabDataSet.Element("Row" + (row + 1)); //read each datarow
                //assemble column values
                DataRowDetail newRow = new DataRowDetail(MainTab);
                newRow.Column1 = Double.Parse(DataRow.Element("A").Value);
                newRow.Column2 = Double.Parse(DataRow.Element("B").Value);
                newRow.Column3 = Double.Parse(DataRow.Element("C").Value);
                newRow.Column4 = Double.Parse(DataRow.Element("D").Value);
                newRow.Column5 = Double.Parse(DataRow.Element("E").Value);
                newRow.Column6 = Double.Parse(DataRow.Element("F").Value);
                newRow.Column7 = Double.Parse(DataRow.Element("G").Value);
                newRow.Column8 = Double.Parse(DataRow.Element("H").Value);
                newRow.Column9 = Double.Parse(DataRow.Element("I").Value);
                newRow.Column10 = Double.Parse(DataRow.Element("J").Value);

                MainTab.DataSet.Add(newRow); //add data detail row to dataset

                DataRow.Remove(); //remove the data row from the element

                row++;
            }

            PerformanceIndicators = new List<TabDetail>();
            TabDetail piTab;

            int count = 0;
            while (element.HasElements && count < StaticConfiguration.maxPerformanceIndicators)
            {
                //Read the PItab element
                XElement piTabElement = (element.Element("PerformanceIndicator" + (count + 1)));

                piTab = new TabDetail(ScenarioName, ""); //create maintab
                //Assemble maintab
                piTab.HeaderText = piTabElement.Element("TabName").Value;

                //Assemble DataSet for the MainTab
                XElement piTabDataSet = piTabElement.Element("DataSet");
                row = 0;
                while (piTabDataSet.HasElements) //While there are still additional child datarow elements
                {
                    XElement DataRow = piTabDataSet.Element("Row" + (row + 1)); //read each datarow
                    //assemble column values
                    DataRowDetail newRow = new DataRowDetail(piTab);
                    newRow.Column1 = Double.Parse(DataRow.Element("A").Value);
                    newRow.Column2 = Double.Parse(DataRow.Element("B").Value);
                    newRow.Column3 = Double.Parse(DataRow.Element("C").Value);
                    newRow.Column4 = Double.Parse(DataRow.Element("D").Value);
                    newRow.Column5 = Double.Parse(DataRow.Element("E").Value);
                    newRow.Column6 = Double.Parse(DataRow.Element("F").Value);
                    newRow.Column7 = Double.Parse(DataRow.Element("G").Value);
                    newRow.Column8 = Double.Parse(DataRow.Element("H").Value);
                    newRow.Column9 = Double.Parse(DataRow.Element("I").Value);
                    newRow.Column10 = Double.Parse(DataRow.Element("J").Value);

                    piTab.DataSet.Add(newRow); //add data detail row to dataset

                    DataRow.Remove(); //remove the data row from the element

                    row++;
                }
                piTab.TabVisibility = Boolean.Parse(piTabElement.Element("Visibility").Value);
                PerformanceIndicators.Add(piTab); 
                count++;                
            }
        }

        #region Properties

        private string _Version;
        public string Version
        {
            get
            {
                return _Version;
            }
            set
            {
                _Version = value;
            }
        }

        private string _ScenarioName;
        public string ScenarioName
        {
            get
            {
                return _ScenarioName;
            }
            set
            {
                _ScenarioName = value;
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
            }
        }


        private List<TabDetail> _PerformanceIndicators;
        public List<TabDetail> PerformanceIndicators
        {
            get
            {
                return _PerformanceIndicators;
            }
            set
            {
                _PerformanceIndicators = value;
            }
        }

        #endregion
    }
}
