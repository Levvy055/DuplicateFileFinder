using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
using DuplicateFileFinder.Annotations;

namespace DuplicateFileFinder.Views
{
    /// <summary>
    /// Interaction logic for FileResult.xaml
    /// </summary>
    public partial class FileResult : UserControl
    {
        public FileResult()
        {
            this.DataContext = this;
            InitializeComponent();
        }

        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(
            nameof(Title), typeof(string), typeof(FileResult),
            new PropertyMetadata("unknown", (dp, e) =>
                (dp as FileResult)?.OnTitleChanged(e)));

        private void OnTitleChanged(DependencyPropertyChangedEventArgs e)
        {
            Exp.Header = e.NewValue.ToString();
        }
    }
}
