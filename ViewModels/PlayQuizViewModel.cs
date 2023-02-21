using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Threading.Tasks.Sources;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel.__Internals;
using CommunityToolkit.Mvvm.Input;
using Labb3_NET22.DataModels;
using Labb3_NET22.Managers;
using static System.Formats.Asn1.AsnWriter;

namespace Labb3_NET22.ViewModels;

public class PlayQuizViewModel : ObservableObject
{
    private readonly QuizManager _dataManager;
    private readonly NavigationManager _navigationManager;
    private readonly QuestionManager _questionManager;
    private readonly CategoryManager _categoryManager;

    public PlayQuizViewModel(QuizManager dataManager, NavigationManager navigationManager, QuestionManager questionManager, CategoryManager categoryManager)
    {
        _dataManager = dataManager;
        _navigationManager = navigationManager;
        _questionManager = questionManager;
        _categoryManager = categoryManager;
        StartViewCommand = new RelayCommand(StartView);
        GetQuizList();
        NextQuestionCommand = new RelayCommand(NextQuestion);
    }

    public RelayCommand StartViewCommand { get; }
    public RelayCommand NextQuestionCommand { get; }

    private string _statement;
    public string Statement
    {
        get => _statement;
        set => SetProperty(ref _statement, value);
    }

    private string _answerOne;
    public string AnswerOne
    {
        get => _answerOne;
        set
        {
            _answerOne = value;
            OnPropertyChanged();
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
        }
    }

    private bool _checkAnswerOne;

    public bool CheckAnswerOne
    {
        get { return _checkAnswerOne; }
        set
        {
            SetProperty(ref _checkAnswerOne, value);
            if (CheckAnswerOne.Equals(true))
            {
                CheckAnswerTwo = false;
                CheckAnswerThree = false;
            }
        }
    }

    private bool _checkAnswerTwo;

    public bool CheckAnswerTwo
    {
        get { return _checkAnswerTwo; }
        set
        {
            SetProperty(ref _checkAnswerTwo, value);
            if (CheckAnswerTwo.Equals(true))
            {
                CheckAnswerOne = false;
                CheckAnswerThree = false;
            }
        }
    }

    private bool _checkAnswerThree;

    public bool CheckAnswerThree
    {
        get { return _checkAnswerThree; }
        set
        {
            SetProperty(ref _checkAnswerThree, value);
            if (CheckAnswerThree.Equals(true))
            {
                CheckAnswerTwo = false;
                CheckAnswerOne = false;
            }
            

        }
    }

    public void StartView()
    {
        _navigationManager.CurrentViewModel = new StartViewModel(_dataManager, _navigationManager);
    }

    public List<Quiz> GetQuizList()
    {
        PlayQuiz = _dataManager.ListOfQuizzes();
        return PlayQuiz;
    }

    private List<Quiz> _playQuiz;

    public List<Quiz> PlayQuiz
    {
        get { return _playQuiz; }
        set
        {
            SetProperty(ref _playQuiz, value);

        }
    }

    private Quiz _selectedQuizTitle;
    public Quiz SelectedQuizTitle
    {
        get { return _selectedQuizTitle; }
        set
        {
            SetProperty(ref _selectedQuizTitle, value);
            SetQuestionList();
            Reset();
        }
    }

    private ObservableCollection<Question> _selectedQuizQuestionList;
    public ObservableCollection<Question> SelectedQuizQuestionList
    {
        get => _selectedQuizQuestionList;
        set => SetProperty(ref _selectedQuizQuestionList, value);
    }

    public void SetQuestionList()
    {
        _dataManager.LoadCurrentQuiz(SelectedQuizTitle);
        SelectedQuizQuestionList = _dataManager.QuestionInCurrentQuizCollection;
        IsEnabled = true;
        NextQuestion();
    }

    public Question CurrentQuestion { get; set; }

    public void NextQuestion()
    {
        CheckAnswer();

        CurrentQuestion = _dataManager.GetRandomQuestion();

        CountQuestions++;

        if (AmountOfQuestions != CountQuestions)
        {
            IsEnabled = true;
            Statement = CurrentQuestion.Statement;

            AnswerOne = CurrentQuestion.Answers[0];
            AnswerTwo = CurrentQuestion.Answers[1];
            AnswerThree = CurrentQuestion.Answers[2];

            InputCorrectAnswer = CurrentQuestion.CorrectAnswer;
        }
        else
        {
            IsEnabled = false;
        }
    }

    private int _score;

    public int Score
    {
        get { return _score; }
        set
        {
            _score = value;
            OnPropertyChanged();
        }
    }

    private int _amountOfQuestions;

    public int AmountOfQuestions
    {
        get { return _amountOfQuestions; }
        set
        {
            _amountOfQuestions = value;
            OnPropertyChanged();
        }
    }

    private bool _isEnabled;
    public bool IsEnabled
    {
        get { return _isEnabled; }
        set { SetProperty(ref _isEnabled, value); }
    }

    public int CountQuestions { get; set; } = -1; 

    public int InputCorrectAnswer { get; set; }

    public void CheckAnswer()
    {
        if (CheckAnswerOne)
        {
            if (InputCorrectAnswer == 0)
            {
                Score++;
            }
        }

        if (CheckAnswerTwo)
        {
            if (InputCorrectAnswer == 1)
            {
                Score++;
            }
        }

        else if (CheckAnswerThree)
        {
            if (InputCorrectAnswer == 2)
            {
                Score++;
            }
        }

        AmountOfQuestions = _dataManager.CurrentQuiz.Questions.Count();
    }

    public void Reset()
    {
        AmountOfQuestions = 0;
        Score = 0;
        CountQuestions = 0;
        //Statement = string.Empty;
        //AnswerOne = string.Empty;
        //AnswerTwo = string.Empty;
        //AnswerThree = string.Empty;
        //CheckAnswerOne = false;
        //CheckAnswerTwo = false;
        //CheckAnswerThree = false;
    }
}