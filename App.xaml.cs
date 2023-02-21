using Labb3_NET22.Managers;
using Labb3_NET22.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Labb3_NET22
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly NavigationManager _navigationManager;
        private readonly QuizManager _dataManager;
        private readonly QuestionManager _questionManager;
        private readonly CategoryManager _categoryManager;

        public App()
        {
            _navigationManager = new NavigationManager();
            _dataManager = new QuizManager();
        }
        protected override void OnStartup(StartupEventArgs e)
        {
            _navigationManager.CurrentViewModel = new StartViewModel(_dataManager, _navigationManager);

            var mainWindow = new MainWindow { DataContext = new MainViewModel(_navigationManager, _dataManager, _questionManager, _categoryManager) };
            mainWindow.Show();
            base.OnStartup(e);
        }
    }
}
