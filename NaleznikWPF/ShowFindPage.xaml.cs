using Lib;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace NaleznikWPF {
    /// <summary>
    /// Interaction logic for ShowFindPage.xaml
    /// </summary>
    public partial class ShowFindPage : UserControl {
        NaleznikController controller;
        private IList<Finding> findings;
        public ShowFindPage() {
            InitializeComponent();
            controller = new NaleznikController();
            findings = new ObservableCollection<Finding>();
            controller.LoadFindings();

        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e) {
            findings = controller.GetFindings();
            DataContext = findings;
            
        }
    }
}
