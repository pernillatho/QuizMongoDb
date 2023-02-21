using System.Windows.Documents;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Labb3_NET22.Managers;
using System.Windows.Input;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.RightsManagement;
using System.Threading.Tasks;
using Labb3_NET22.DataModels;
using System;
using System.Collections.ObjectModel;
using System.Threading;

namespace Labb3_NET22.ViewModels;

public class EditQuizViewModel : ObservableObject
{
    private readonly QuizManager _dataManager;
    private readonly NavigationManager _navigationManager;
    private readonly QuestionManager _questionManager;
    private readonly CategoryManager _categoryManager;


    public EditQuizViewModel(QuizManager dataManager, NavigationManager navigationManager, QuestionManager questionManager, CategoryManager categoryManager)
    {
        _dataManager = dataManager;
        _navigationManager = navigationManager;
        _questionManager = questionManager;
        _categoryManager = categoryManager;
        StartViewCommand = new RelayCommand(() =>
            _navigationManager.CurrentViewModel = new StartViewModel(_dataManager, _navigationManager));
        AddQuestionCommand = new RelayCommand(AddQuestionWithCorrectAnswer, IsEnabled);
        GetQuestionList();
        GetCategoryList();
        RemoveQuestionCommand = new RelayCommand(RemoveQuestion ,IsEnabled);
        EditQuestionCommand = new RelayCommand(EditQuestion ,IsEnabled);
        SaveCategoryInQuestion = new RelayCommand(AddCategoryToQuestion, IsEnabledAddAndRemove);
        RemoveCategoryQuestion = new RelayCommand(RemoveCategoryInQuestion, IsEnabledAddAndRemove);
    }

    public RelayCommand AddQuestionCommand { get; }
    public RelayCommand RemoveQuestionCommand { get; }
    public RelayCommand StartViewCommand { get; }
    public RelayCommand EditQuestionCommand { get; }
    public RelayCommand SaveCategoryInQuestion { get; }
    public RelayCommand RemoveCategoryQuestion { get; }

    private string _statement;
    public string Statement
    {
        get => _statement;
        set
        {
                SetProperty(ref _statement, value);
                IfNotifyCanExecuteChanged();
        }
    }

    private string _answerOne;
    public string AnswerOne
    {
        get => _answerOne;
        set
        {
            _answerOne = value;
            OnPropertyChanged();
            IfNotifyCanExecuteChanged();
        }
    }

    private string _answerTwo;

    public string AnswerTwo
    {
        get => _answerTwo;
        set
        {
            _answerTwo = value;
            OnPropertyChanged();
            IfNotifyCanExecuteChanged();
        }
    }

    private string _answerThree;

    public string AnswerThree
    {
        get => _answerThree;
        set
        {
            _answerThree = value;
            OnPropertyChanged();
            IfNotifyCanExecuteChanged();

        }
    }

    private bool _correctAnswerOne;

    public bool CorrectAnswerOne
    {
        get { return _correctAnswerOne; }
        set
        {
            SetProperty(ref _correctAnswerOne, value);
            if (CorrectAnswerOne.Equals(true))
            {
                CorrectAnswerTwo = false;
                CorrectAnswerThree = false;
            }
            

        }

    }

    private bool _correctAnswerTwo;

    public bool CorrectAnswerTwo
    {
        get { return _correctAnswerTwo; }
        set
        {
            SetProperty(ref _correctAnswerTwo, value);
            if (CorrectAnswerTwo.Equals(true))
            {
                CorrectAnswerOne = false;
                CorrectAnswerThree = false;
            }
            

        }
    }

    private bool _correctAnswerThree;

    public bool CorrectAnswerThree
    {
        get { return _correctAnswerThree; }
        set
        {
            SetProperty(ref _correctAnswerThree, value);
            if (CorrectAnswerThree.Equals(true))
            {
                CorrectAnswerTwo = false;
                CorrectAnswerOne = false;
            }
           

        }
    }

    public int QuestionCorrectAnswer { get; set; }

    private ObservableCollection<Category> _categoryInQuestion;
    public ObservableCollection<Category> CategoryInQuestion
    {
        get { return _categoryInQuestion; }
        set
        {
            _categoryInQuestion = value;
            OnPropertyChanged();
            IfNotifyCanExecuteChanged();
        }
    }

    private List<Question> _questionList;

    public List<Question> QuestionList
    {
        get { return _questionList; }
        set { SetProperty(ref _questionList, value); }
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
            SaveCategoryInQuestion.NotifyCanExecuteChanged();
            RemoveCategoryQuestion.NotifyCanExecuteChanged();
        }
    }

    private int _selectedQuestionsIndex;
    public int SelectedQuestionIndex
    {
        get => _selectedQuestionsIndex;
        set
        {
            SetProperty(ref _selectedQuestionsIndex, value);
            FillQuestionBoxes();
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
            SaveCategoryInQuestion.NotifyCanExecuteChanged();
            RemoveCategoryQuestion.NotifyCanExecuteChanged();
        }
    }

    public void AddQuestionWithCorrectAnswer()
    {
        CorrectAnswer();
        _questionManager.SaveQuestion(Statement, QuestionCorrectAnswer, CategoryInQuestion, AnswerOne, AnswerTwo, AnswerThree);
        ClearQuestionBoxes();
        GetQuestionList();
    }

    public void EditQuestion()
    {
        _questionManager.EditQuestion(SelectedQuestion.Id, Statement, QuestionCorrectAnswer, AnswerOne, AnswerTwo, AnswerThree);
        ClearQuestionBoxes();
        GetQuestionList();
    }

    public void RemoveQuestion()
    {
        _questionManager.RemoveQuestion(SelectedQuestion);
        ClearQuestionBoxes();
        GetQuestionList();
    }

    public void IfNotifyCanExecuteChanged()
    {
        AddQuestionCommand.NotifyCanExecuteChanged();

        if (SelectedQuestionIndex != 0)
        {
            EditQuestionCommand.NotifyCanExecuteChanged();
            RemoveQuestionCommand.NotifyCanExecuteChanged();
        }
    }

    public bool IsEnabled()
    {
        if (string.IsNullOrEmpty(Statement) ||
            string.IsNullOrEmpty(AnswerOne) ||
            string.IsNullOrEmpty(AnswerTwo) ||
            string.IsNullOrEmpty(AnswerThree))
        {
            return false;
        }
        return true;
    }

    public bool IsEnabledAddAndRemove()
    {
        if (SelectedCategory is null || SelectedQuestion is null)
            return false;

        return true;
    }

    public void CorrectAnswer()
    {
        if (CorrectAnswerOne)
        {
            QuestionCorrectAnswer = 0;
        }

        else if (CorrectAnswerTwo)
        {
            QuestionCorrectAnswer = 1;
        }

        else if (CorrectAnswerThree)
        {
            QuestionCorrectAnswer = 2;
        }
    }

    public void FillQuestionBoxes()
    {
        var index = (SelectedQuestionIndex);

        if (index > -1)
        {
            Statement = QuestionList.ElementAt(index).Statement;
            
            AnswerOne = QuestionList.ElementAt(index).Answers[0];
            AnswerTwo = QuestionList.ElementAt(index).Answers[1];
            AnswerThree = QuestionList.ElementAt(index).Answers[2];
            CategoryInQuestion = new ObservableCollection<Category>(QuestionList.ElementAt(index).Categories);
        }
    }

    public void ClearQuestionBoxes()
    {
        Statement = string.Empty;
        AnswerOne = string.Empty;
        AnswerTwo = string.Empty;
        AnswerThree = string.Empty;
        CorrectAnswerOne = false;
        CorrectAnswerTwo = false;
        CorrectAnswerThree = false;
        SelectedCategory = null;
    }

    public void AddCategoryToQuestion()
    {
        if(SelectedQuestion.Categories.Contains(SelectedCategory))
            return;

        _questionManager.UpdateCurrentQuestionWithCategory(SelectedCategory,SelectedQuestion);
        CategoryInQuestion.Add(SelectedCategory);
    }

    public void RemoveCategoryInQuestion()
    {
        _questionManager.RemoveCategoryInQuestion(SelectedCategory, SelectedQuestion);

        CategoryInQuestion.Remove(SelectedCategory);
    }
}


