using ProcessYieldOptimizer.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

/* Helps Load and Save full sessions with multiple scenarios
 * Note: Saving file not as .XML to avoid user confusion between load scenario and load session. 
 * Borrows Methods from XMLLoadSaveScenario.cs to do the following functions:
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
 *  Every valid XML file should have at least one PYOScenario element which contains the following elements
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
 *         <PYOSession>
 *          <PYOScenario>
 *           <Version>Process Yield Optimizer Version 1.0</Version>
 *           <ScenarioName>Empty</ScenarioName>
 *           <MainTab>
 *             <TabName>Main</TabName>
 *             <DataSet>
 *               <Row1>
 *                 <A>0</A>
 *                 <B>0</B>
 *                 <C>0</C>
 *                 <D>0</D>
 *                 <E>0</E>
 *                 <F>0</F>
 *                 <G>0</G>
 *                 <H>0</H>
 *                 <I>0</I>
 *                 <J>0</J>
 *               </Row1>
 */

namespace ProcessYieldOptimizer.Utilities
{
    class XMlLoadSaveSessions
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
                        loadXMLFile(xDoc.Element("PYOSession"));
                    }
                    else
                    {
                        CreateFile(); //Create a file then load static settings
                        XDocument xDoc = XDocument.Load(_FilePath, LoadOptions.SetBaseUri | LoadOptions.SetLineInfo);
                        loadXMLFile(xDoc.Element("PYOSession"));
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

        private XElement generateXMLFile()
        {
            XElement element = new XElement("PYOSession");
            for (int c = 0; c < Scenarios.Count; c++)
            {
                XMLSaveLoadScenario helperUtil = new XMLSaveLoadScenario();

                //consturct scenario in helper util
                ScenarioDetail myScenario = Scenarios[c];
                helperUtil.ScenarioName = myScenario.Name;
                helperUtil.MainTab = Scenarios[c].MainTab;

                List<TabDetail> myPIs = new List<TabDetail>();
                myPIs.Add(myScenario.PerformanceIndicator1);
                myPIs.Add(myScenario.PerformanceIndicator2);
                myPIs.Add(myScenario.PerformanceIndicator3);
                myPIs.Add(myScenario.PerformanceIndicator4);
                myPIs.Add(myScenario.PerformanceIndicator5);
                myPIs.Add(myScenario.PerformanceIndicator6);
                myPIs.Add(myScenario.PerformanceIndicator7);
                myPIs.Add(myScenario.PerformanceIndicator8);
                myPIs.Add(myScenario.PerformanceIndicator9);
                myPIs.Add(myScenario.PerformanceIndicator10);

                helperUtil.PerformanceIndicators = myPIs;                
                

                XElement pyoScenarioElement = helperUtil.generateXMLFile(); //get PYO element                               
                element.Add(pyoScenarioElement); //add pyro element
            }           

            return element;
        }

        private void loadXMLFile(XElement element)
        {
            XElement pyoScenarioElement = element.Element("PYOScenario");
            LoadedScenarios = new List<PreliminaryLoadScenario>();

            while (element.HasElements)
            {
                XMLSaveLoadScenario helperUtil = new XMLSaveLoadScenario();
                helperUtil.loadXMLFile(pyoScenarioElement);

                PreliminaryLoadScenario preScenario = new PreliminaryLoadScenario();
                preScenario.Version = helperUtil.Version;
                preScenario.ScenarioName = helperUtil.ScenarioName;
                preScenario.MainTab = helperUtil.MainTab;
                preScenario.PerformanceIndicators = helperUtil.PerformanceIndicators;

                LoadedScenarios.Add(preScenario); //Add this preScenario -> becomes ScenarioDetail in model add method

                pyoScenarioElement.Remove(); //get next scenari
                pyoScenarioElement = element.Element("PYOScenario");
            }
        }

        public List<ScenarioDetail> Scenarios;
        public List<PreliminaryLoadScenario> LoadedScenarios;
        
    }

    public class PreliminaryLoadScenario
    {
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
