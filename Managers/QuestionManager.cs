using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using Labb3_NET22.DataModels;
using MongoDB.Driver.Core.Configuration;
using MongoDB.Driver;

namespace Labb3_NET22.Managers
{
    public class QuestionManager : ObservableObject
    {
        private readonly IMongoCollection<Question> collectionQuestion;

        public QuestionManager()
        {
            var settings = MongoClientSettings.FromConnectionString("mongodb://127.0.0.1:27017");
            var client = new MongoClient(settings);
            var database = client.GetDatabase("quizDB");

            collectionQuestion = database.GetCollection<Question>("Question", new MongoCollectionSettings() { AssignIdOnInsert = true });
        }

        public void SaveQuestion(string statement, int correctAnswer, ObservableCollection<Category> category, params string[] answers)
        {
            var newQuestion = new Question(statement, answers, correctAnswer, category);
            collectionQuestion.InsertOne(newQuestion);
        }

        public void RemoveQuestion(Question question)
        {
            var filter = Builders<Question>.Filter.Eq("Id", question.Id);
            collectionQuestion.DeleteOne(filter);
        }

        public void EditQuestion(string id, string statement, int correctAnswer, params string[] answers)
        {
            var filter = Builders<Question>.Filter.Eq("Id", id);
            var update = Builders<Question>.Update.Set("Statement", statement).
                Set("CorrectAnswer", correctAnswer).Set("Answers", answers);

            collectionQuestion
                .FindOneAndUpdate(
                    filter,
                    update,
                    new FindOneAndUpdateOptions<Question, Question>
                    {
                        IsUpsert = true
                    }
                );
        }

        public void UpdateCurrentQuestionWithCategory(Category category, Question question)
        {
            var filter = Builders<Question>.Filter.Eq("Id", question.Id);
            var update = Builders<Question>.Update.AddToSet("Categories", category);

            collectionQuestion.UpdateOne(filter, update);
        }

        public void RemoveCategoryInQuestion(Category category, Question question)
        {
            var filter = Builders<Question>.Filter.Eq("Id", question.Id);
            var update = Builders<Question>.Update.Pull("Categories", category);

            collectionQuestion.UpdateOne(filter, update);

        }

        public List<Question> ListOfQuestions()
        {
            var quiz = collectionQuestion.Find(_ => true)
                .ToList();

            return new List<Question>(quiz);
        }

        public List<Question> QuestionsInSelectedCategory(Category category)
        {
            var filter = Builders<Question>.Filter.ElemMatch(x => x.Categories, x => x.Name.Contains(category.Name));
            var result = collectionQuestion.Find(filter).ToList();

            return new List<Question>(result);
        }

        public List<Question> QuestionsByChar(string character)
        {
            var questions = collectionQuestion.Find(x => x.Statement.Contains(character)).ToList();

            return questions;
        }
    }
}
