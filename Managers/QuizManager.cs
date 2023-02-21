using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text.Json;
using System.Threading.Tasks;
using System.Threading.Tasks.Sources;
using CommunityToolkit.Mvvm.ComponentModel;
using Labb3_NET22.DataModels;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using static System.Formats.Asn1.AsnWriter;

namespace Labb3_NET22.Managers;

public class QuizManager : ObservableObject
{
    private Quiz _quiz;

    public Quiz CurrentQuiz
    {
        get => _quiz;
        set
        {
            _quiz = value;
            CurrentQuizChanged?.Invoke();
        }
    }

    private readonly IMongoCollection<Quiz> collectionQuiz;

    public QuizManager()
    {
        var settings = MongoClientSettings.FromConnectionString("mongodb://127.0.0.1:27017");
        var client = new MongoClient(settings);
        var database = client.GetDatabase("quizDB");

        collectionQuiz =
            database.GetCollection<Quiz>("Quiz", new MongoCollectionSettings() { AssignIdOnInsert = true });
    }

    public event Action? CurrentQuizChanged;

    public void SaveQuiz(Quiz quiz)
    {
        collectionQuiz.InsertOne(quiz);
    }

    public void UpdateCurrentQuizWithQuestion(Quiz quiz, Question question)
    {
        var filter = Builders<Quiz>.Filter.Eq("Id", quiz.Id);
        var update = Builders<Quiz>.Update.AddToSet("Questions", question);

        collectionQuiz.UpdateOne(filter, update);
    }

    public void RemoveQuestionOnCurrentQuiz(Quiz quiz, Question question)
    {
        var filter = Builders<Quiz>.Filter.Eq("Id", quiz.Id);
        var update = Builders<Quiz>.Update.Pull("Questions", question);

        collectionQuiz.UpdateOne(filter, update);
    }

    private ObservableCollection<Question> _questionInCurrentQuizCollection;

    public ObservableCollection<Question> QuestionInCurrentQuizCollection
    {
        get { return _questionInCurrentQuizCollection; }
        set { SetProperty(ref _questionInCurrentQuizCollection, value); }
    }

    public void LoadCurrentQuiz(Quiz quiz)
    {
        _quiz = quiz;

        QuestionInCurrentQuizCollection = new ObservableCollection<Question>();

        foreach (var result in quiz.Questions)
        {
            _questionInCurrentQuizCollection.Add(result);
        }
    }

    public List<Quiz> ListOfQuizzes()
    {
        var quiz = collectionQuiz.Find(_ => true)
            .ToList();

        return new List<Quiz>(quiz);
    }

    public Question GetRandomQuestion()
    {
        var random = new Random();
        var question = _quiz.Questions.ElementAt(random.Next(0, _quiz.Questions.Count()));

        return question;
    }
}