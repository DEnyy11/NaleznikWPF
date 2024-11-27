using System.IO.Packaging;
using System.Windows;
using System.Windows.Input;
using Lib;
using SQLitePCL;

namespace NaleznikWPF {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        NaleznikController controller;
        public MainWindow() {
            InitializeComponent();
            controller = new NaleznikController();
            controller.CheckDirectoryOrCreate();
            MainContent.Content = new HomePage();
            Batteries.Init();
            controller.LoadFindings();
        }



        private void btExit_Click(object sender, RoutedEventArgs e) {
            
            if (MessageBox.Show("Opravdu chcete ukončit aplikaci?","Konec",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes) {Application.Current.Shutdown();} 
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e) {
            if (e.LeftButton == MouseButtonState.Pressed) {this.DragMove();}
        }

        private void btHome_Click(object sender, RoutedEventArgs e) {
            MainContent.Content = new HomePage();
        }

        private void btNewFind_Click(object sender, RoutedEventArgs e) {
            MainContent.Content = new NewFind(controller);
            
        }

        private void btMap_Click(object sender, RoutedEventArgs e) {
            MainContent.Content = new MapPage();
        }

        private void btHelp_click(object sender, RoutedEventArgs e) {
            MainContent.Content = new HelpPage();
        }

  
        private void btShowFinds_click(object sender, RoutedEventArgs e) {
            MainContent.Content = new ShowFindPage();
        }
    }
}
