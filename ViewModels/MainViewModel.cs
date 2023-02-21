using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Labb3_NET22.DataModels;
using Labb3_NET22.Managers;

namespace Labb3_NET22.ViewModels;

public class MainViewModel : ObservableObject
{
    private readonly NavigationManager _navigationManager;
    private readonly QuizManager _dataManager;
    private readonly QuestionManager _questionManager;
    private readonly CategoryManager _categoryManager;

    public ObservableObject CurrentViewModel => _navigationManager.CurrentViewModel;

    public MainViewModel(NavigationManager navigationManager, QuizManager dataManager, QuestionManager questionManager, CategoryManager categoryManager)
    {
        _navigationManager = navigationManager;
        _dataManager = dataManager;
        _questionManager = questionManager;
        _categoryManager = categoryManager;

        _navigationManager.CurrentViewModelChanged += CurrentViewModelChanged;

    }

    private void CurrentViewModelChanged()
    {
        OnPropertyChanged(nameof(CurrentViewModel));
    }
}