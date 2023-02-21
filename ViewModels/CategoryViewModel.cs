using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Labb3_NET22.DataModels;
using Labb3_NET22.Managers;

namespace Labb3_NET22.ViewModels
{
    public class CategoryViewModel : ObservableObject
    {
        private readonly QuizManager _dataManager;
        private readonly NavigationManager _navigationManager;
        private readonly CategoryManager _categoryManager;
        private readonly QuestionManager _questionManager;

        public CategoryViewModel(QuizManager dataManager, NavigationManager navigationManager, CategoryManager categoryManager, QuestionManager questionManager)
        {
            _dataManager = dataManager;
            _navigationManager = navigationManager;
            _categoryManager = categoryManager;
            _questionManager = questionManager;
            SaveCreateQuizCommand = new RelayCommand(AddCategory);
            StartViewCommand = new RelayCommand(() =>
                _navigationManager.CurrentViewModel = new StartViewModel(_dataManager, _navigationManager));
            GetCategoryList();
 
        }

        public RelayCommand SaveCreateQuizCommand { get; }
        public RelayCommand StartViewCommand { get; }

        private string _newCategory;
        public string NewCategory
        {
            get { return _newCategory; }
            set
            {
                _newCategory = value;
                OnPropertyChanged(NewCategory);
                SaveCreateQuizCommand.NotifyCanExecuteChanged();
            }
        }

        public void AddCategory()
        {
            var newQuiz = new Category(NewCategory);

            _categoryManager.SaveCategory(newQuiz);

            GetCategoryList();
        }

        public List<Category> _categoryList;
        public List<Category> CategoryList
        {
            get { return _categoryList; }
            set { SetProperty(ref _categoryList, value); }
        }

        public List<Category> GetCategoryList()
        {
            CategoryList = _categoryManager.ListOfCategories();
            return CategoryList;
        }
    }
}
