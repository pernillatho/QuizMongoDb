using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.Json.Serialization;
using Labb3_NET22.Managers;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;

namespace Labb3_NET22.DataModels;

public record Question
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]

    public string Id { get; set; }
    public string Statement { get; set; }
    public string[] Answers { get; set; }
    public int CorrectAnswer { get; set; }
    public IEnumerable<Category> Categories { get; set; }

    public Question(string statement, string[] answers, int correctAnswer ,ObservableCollection<Category> categories)
    {
        Statement = statement;
        Answers = answers;
        CorrectAnswer = correctAnswer;
       Categories = categories;
    }
}