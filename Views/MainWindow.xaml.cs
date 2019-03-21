using System.Windows;
using System.Windows.Controls;
using DuplicateFileFinder.ViewModels;

namespace DuplicateFileFinder.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            SelectDirsWindow = new SelectDirs();
            ShowResultsWindow = new ShowResults();

            GoTo(SelectDirsWindow);
        }

        public void GoTo(UserControl control)
        {
            if (control != null && (control == SelectDirsWindow || control == ShowResultsWindow))
            {
                this.Content = control;
            }
        }

        public SelectDirs SelectDirsWindow { get; }
        public ShowResults ShowResultsWindow { get; }
    }
}
