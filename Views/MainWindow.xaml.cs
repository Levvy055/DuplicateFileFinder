using System.Windows;
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
            this.Content = SelectDirsWindow;
        }

        public SelectDirs SelectDirsWindow { get; }
    }
}
