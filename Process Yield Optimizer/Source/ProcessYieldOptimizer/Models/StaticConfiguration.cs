using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessYieldOptimizer.Models
{
    /// <summary>
    /// Provide convienence in scaling all operations as features / requests are added
    /// Variables here represent hardcoded limitations that can be increased in future
    /// </summary>
    public class StaticConfiguration
    {
        public static int maxColumns = 10; //Max Columns for DataGrids
        public static int maxPerformanceIndicators = 10; //Max Performance Indicators for program

        //Settings Version for saved files used by XML utility classes
        public static string settingsVersion = "Process Yield Optimizer Version 1.0"; 
    }
}
