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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ProcessYieldOptimizer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// 
    /// Not due to debug statements the interaction can be a little slower in debug environment using Visual Studio
    /// But program as an executable is fast
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new ProcessYieldViewModel();            
        }

        private void ExitProg_Click(object sender, RoutedEventArgs e)
        {            
            this.Close();
        }
    }
}
