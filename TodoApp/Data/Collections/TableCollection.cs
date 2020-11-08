using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoApp.Data.Model;
using TodoApp.Data.Mongo;

namespace TodoApp.Data.Collections
{
    [CollectionName("Tables")]
    public class TableCollection : BaseCollection<Table>
    {
        public static List<Table> FindByUser(ObjectId owner)
        {
            return collection.Find(Builders<Table>.Filter.Eq(x => x.Owner, owner)).ToList();
        }

        public static Table FindByUserAndId(ObjectId owner, ObjectId tableId)
        {
            return collection.Find(Builders<Table>.Filter.And(Builders<Table>.Filter.Eq(x => x.ID, tableId), Builders<Table>.Filter.Eq(x => x.Owner, owner))).FirstOrDefault();
        }
    }
}
