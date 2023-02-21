using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using Labb3_NET22.DataModels;
using MongoDB.Driver;

namespace Labb3_NET22.Managers
{
    public class CategoryManager : ObservableObject
    {
        private readonly IMongoCollection<Category> collectionCategory;

        public CategoryManager()
        {
            var settings = MongoClientSettings.FromConnectionString("mongodb://127.0.0.1:27017");
            var client = new MongoClient(settings);
            var database = client.GetDatabase("quizDB");

            collectionCategory = database.GetCollection<Category>("Category", new MongoCollectionSettings() { AssignIdOnInsert = true });
        }

        public void SaveCategory(Category category)
        {
            collectionCategory.InsertOne(category);
        }

        public List<Category> ListOfCategories()
        {
            var categoryList = collectionCategory.Find(_ => true)
                .ToList();

            return new List<Category>(categoryList);
        }
    }
}
