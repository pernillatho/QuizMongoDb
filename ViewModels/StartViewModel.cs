using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Labb3_NET22.DataModels;
using Labb3_NET22.Managers;

namespace Labb3_NET22.ViewModels;

public class StartViewModel : ObservableObject
{
    private readonly NavigationManager _navigationManager;
    private readonly QuizManager _dataManager;
    private readonly QuestionManager _questionManager;
    private readonly CategoryManager _categoryManager;

    public IRelayCommand NavigateCreateQuiz { get; }
    public IRelayCommand NavigateStartView { get; }
    public IRelayCommand NavigateEditView { get; }
    public IRelayCommand NavigatePlayQuiz { get; }
    public IRelayCommand NavigateCategoryView { get; }

    public StartViewModel(QuizManager dataManager, NavigationManager navigationManager)
    {
        _dataManager = dataManager;
        _navigationManager = navigationManager;
        _questionManager = new QuestionManager();
        _categoryManager = new CategoryManager();

        NavigateCreateQuiz = new RelayCommand(() => _navigationManager.CurrentViewModel = new CreateQuizViewModel(_dataManager, _navigationManager, _questionManager, _categoryManager));
        NavigateStartView = new RelayCommand(() => _navigationManager.CurrentViewModel = new StartViewModel(_dataManager, _navigationManager));
        NavigateEditView = new RelayCommand(() => _navigationManager.CurrentViewModel = new EditQuizViewModel(_dataManager, _navigationManager, _questionManager, _categoryManager));
        NavigatePlayQuiz = new RelayCommand(() =>
            _navigationManager.CurrentViewModel = new PlayQuizViewModel(_dataManager, _navigationManager, _questionManager, _categoryManager));
        NavigateCategoryView = new RelayCommand(() => _navigationManager.CurrentViewModel = new CategoryViewModel(_dataManager, _navigationManager, _categoryManager, _questionManager));
    }
}
