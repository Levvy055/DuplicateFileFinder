﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using DuplicateFileFinder.ViewModels;

namespace DuplicateFileFinder.Views
{
    /// <summary>
    /// Interaction logic for ShowResults.xaml
    /// </summary>
    public partial class ShowResults : UserControl
    {
        private readonly ShowResultsViewModel _viewModel;

        public ShowResults()
        {
            _viewModel = new ShowResultsViewModel();
            this.DataContext = _viewModel;
            InitializeComponent();
        }

        public void ApplyResults(Results results)
        {
            _viewModel.ApplyResults(results);

        }
    }
}
