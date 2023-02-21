using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Navigation;
using MongoDB.Driver.Core.Configuration;
using MongoDB.Driver;

namespace Labb3_NET22.DataModels;

public record Quiz
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]

    public string Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public IEnumerable<Question> Questions { get; set; }

    public Quiz(string title, ObservableCollection<Question> questions)
    {
        Title = title;
        Questions = new List<Question>();
    }

}