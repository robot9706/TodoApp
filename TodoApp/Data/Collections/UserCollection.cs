using MongoDB.Bson;
using MongoDB.Driver;
using System;
using TodoApp.Data.Model;
using TodoApp.Data.Mongo;

namespace TodoApp.Data.Collections
{
    [CollectionName("Users")]
    public class UserCollection : BaseCollection<User>
    {
        public static User FindByUsername(string username)
        {
            return collection.Find(Builders<User>.Filter.Eq(x => x.Username, username)).FirstOrDefault();
        }

        public static bool SetUsername(ObjectId id, string username)
        {
            return collection.UpdateOne(Builders<User>.Filter.Eq(x => x.ID, id), Builders<User>.Update.Set(x => x.Username, username)).ModifiedCount > 0;
        }

        public static bool SetPasswordHash(ObjectId id, string passwordHash)
        {
            return collection.UpdateOne(Builders<User>.Filter.Eq(x => x.ID, id), Builders<User>.Update.Set(x => x.PasswordHash, passwordHash)).ModifiedCount > 0;
        }

        public static bool AddRoleToUser(ObjectId id, string role)
        {
            return collection.UpdateOne(Builders<User>.Filter.Eq(x => x.ID, id), Builders<User>.Update.Push(x => x.Roles, role)).ModifiedCount > 0;
        }

        public static bool RemoveRoleFromUser(ObjectId id, string role)
        {
            return collection.UpdateOne(Builders<User>.Filter.Eq(x => x.ID, id), Builders<User>.Update.Pull(x => x.Roles, role)).ModifiedCount > 0;
        }
    }
}
