using System.Windows.Controls;
using DuplicateFileFinder.ViewModels;

namespace DuplicateFileFinder.Views
{
    /// <summary>
    /// Interaction logic for SelectDirs.xaml
    /// </summary>
    public partial class SelectDirs : UserControl
    {
        public SelectDirs()
        {
            this.DataContext = new SelectDirsViewModel();
            InitializeComponent();
        }
    }
}
