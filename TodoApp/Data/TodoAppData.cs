using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using TodoApp.Data.Collections;
using TodoApp.Data.Mongo;

namespace TodoApp.Data
{
    public class TodoAppData
    {
        public static void Init(IMongoDatabase database)
        {
            InitCollection<UserCollection>(database);
        }

        private static void InitCollection<T>(IMongoDatabase database)
        {
            string collectionName = typeof(T).GetCustomAttribute<CollectionNameAttribute>().Name;

            typeof(T).GetMethod("Init", BindingFlags.FlattenHierarchy | BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, new object[] { database, collectionName });
        }
    }
}
