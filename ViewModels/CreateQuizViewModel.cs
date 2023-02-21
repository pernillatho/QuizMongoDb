using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Labb3_NET22.DataModels;
using Labb3_NET22.Managers;

namespace Labb3_NET22.ViewModels;

public class CreateQuizViewModel : ObservableObject
{
    private readonly QuizManager _dataManager;
    private readonly NavigationManager _navigationManager;
    private readonly QuestionManager _questionManager;
    private readonly CategoryManager _categoryManager;

    public CreateQuizViewModel(QuizManager dataManager, NavigationManager navigationManager, QuestionManager questionManager, CategoryManager categoryManager)
    {
        _dataManager = dataManager;
        _navigationManager = navigationManager;
        _questionManager = questionManager;
        _categoryManager = categoryManager;
        GetQuizList();
        GetQuestionList();
        GetCategoryList();
        SaveCreateQuizCommand = new RelayCommand(AddQuiz, IsEnabledCreateQuiz);
        StartViewCommand = new RelayCommand(() =>
            _navigationManager.CurrentViewModel = new StartViewModel(_dataManager, _navigationManager));
        AddQuestionToQuizCommand = new RelayCommand(AddQuestionToQuiz, IsEnabledAddAndRemoveQuestion);
        RemoveQuestionInQuizCommand = new RelayCommand(RemoveQuestionInQuiz, IsEnabledAddAndRemoveQuestion);
        SearchCommand = new RelayCommand(SearchCharsInQuestion);
        ResetCommand = new RelayCommand(ResetQuestionList);
    }

    public RelayCommand SaveCreateQuizCommand { get; }
    public RelayCommand StartViewCommand { get; }
    public RelayCommand AddQuestionToQuizCommand { get; }
    public RelayCommand RemoveQuestionInQuizCommand { get; }
    public RelayCommand SearchCommand { get; }
    public RelayCommand ResetCommand { get; }

    private string _newTitle;
    public string NewTitle
    {
        get { return _newTitle; }
        set
        {
            _newTitle = value;
            OnPropertyChanged(NewTitle);
            SaveCreateQuizCommand.NotifyCanExecuteChanged();
        }
    }

    private Quiz _selectedQuiz;
    public Quiz SelectedQuiz
    {
        get { return _selectedQuiz; }
        set
        {
            SetProperty(ref _selectedQuiz, value);

            AddQuestionToQuizCommand.NotifyCanExecuteChanged();
            RemoveQuestionInQuizCommand.NotifyCanExecuteChanged();
        }
    }

    private List<Quiz> _quizList;
    public List<Quiz> QuizList
    {
        get { return _quizList; }
        set
        {
            SetProperty(ref _quizList, value);
          
        }
    }

    public List<Quiz> GetQuizList()
    {
        QuizList = _dataManager.ListOfQuizzes();
        return QuizList;
    }

    private List<Question> _questionList;

    public List<Question> QuestionList
    {
        get { return _questionList; }
        set
        {
            SetProperty(ref _questionList, value);
        }
    }

    public List<Question> GetQuestionList()
    {
        QuestionList = _questionManager.ListOfQuestions();
        return QuestionList;
    }

    private Question _selectedQuestion;
    public Question SelectedQuestion
    {
        get { return _selectedQuestion; }
        set
        {
            SetProperty(ref _selectedQuestion, value);
            OnPropertyChanged(SelectedQuestion.Statement);

            AddQuestionToQuizCommand.NotifyCanExecuteChanged();
            RemoveQuestionInQuizCommand.NotifyCanExecuteChanged();
        }
    }

    private string _searchWord;

    public string SearchWord
    {
        get { return _searchWord; }
        set
        {
            _searchWord = value;
            SearchCommand.NotifyCanExecuteChanged();
        }
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

    private Category _selectedCategory;
    public Category SelectedCategory
    {
        get { return _selectedCategory; }

        set
        {
            SetProperty(ref _selectedCategory, value);
            QuestionList = _questionManager.QuestionsInSelectedCategory(SelectedCategory); 
            OnPropertyChanged();
        }
    }

    public void SearchCharsInQuestion()
    {
        if (SearchWord is null)
            return;

        QuestionList = _questionManager.QuestionsByChar(SearchWord);
    }

    public void AddQuiz()
    {
        var newQuiz = new Quiz(NewTitle, new ObservableCollection<Question>());
        _dataManager.CurrentQuiz = newQuiz;

        _dataManager.SaveQuiz(newQuiz);
        GetQuizList();

        NewTitle = string.Empty;
    }

    public void AddQuestionToQuiz()
    {
        _dataManager.UpdateCurrentQuizWithQuestion(SelectedQuiz, SelectedQuestion);

        UpdateSelectedQuizQuestionList();
    }

    public void RemoveQuestionInQuiz()
    {
        _dataManager.RemoveQuestionOnCurrentQuiz(SelectedQuiz, SelectedQuestion);

        UpdateSelectedQuizQuestionList();
    }

    public void UpdateSelectedQuizQuestionList()
    {
        var quiz = SelectedQuiz.Title;
        QuizList = _dataManager.ListOfQuizzes();

        SelectedQuiz = QuizList.First(q => q.Title.Equals(quiz));
    }

    public bool IsEnabledCreateQuiz()
    {
        if (string.IsNullOrEmpty(NewTitle))
            return false;

        return true;
    }

    public bool IsEnabledAddAndRemoveQuestion()
    {
        if (SelectedQuiz is null || SelectedQuestion is null)
            return false;

        return true;
    }

    public void ResetQuestionList()
    {
        QuestionList = _questionManager.ListOfQuestions();
    }
}

