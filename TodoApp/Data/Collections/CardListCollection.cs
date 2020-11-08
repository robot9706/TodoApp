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
    [CollectionName("CardLists")]
    public class CardListCollection : BaseCollection<CardList>
    {
        public static List<CardList> FindAllInTable(ObjectId table)
        {
            return collection.Find(Builders<CardList>.Filter.Eq(x => x.Table, table)).ToList();
        }

        public static bool AddCardToTable(ObjectId table, ObjectId list, Card card)
        {
            UpdateResult result = collection.UpdateOne(Builders<CardList>.Filter.And(Builders<CardList>.Filter.Eq(x => x.ID, list), Builders<CardList>.Filter.Eq(x => x.Table, table)),
                Builders<CardList>.Update.Push(x => x.Content, card));

            return (result.IsAcknowledged && result.MatchedCount == 1);
        }

        public static CardList FindListByTableAndId(ObjectId table, ObjectId list)
        {
            return collection.Find(Builders<CardList>.Filter.And(Builders<CardList>.Filter.Eq(x => x.ID, list), Builders<CardList>.Filter.Eq(x => x.Table, table))).FirstOrDefault();
        }

        public static bool DeleteByTable(ObjectId table)
        {
            return collection.DeleteMany(Builders<CardList>.Filter.Eq(x => x.Table, table)).DeletedCount > 0;
        }

        public static bool DeleteByTableAndId(ObjectId table, ObjectId list)
        {
            return collection.DeleteMany(Builders<CardList>.Filter.And(Builders<CardList>.Filter.Eq(x => x.ID, list), Builders<CardList>.Filter.Eq(x => x.Table, table))).DeletedCount > 0;
        }

        public static bool UpdateContent(CardList list)
        {
            UpdateResult result = collection.UpdateOne(Builders<CardList>.Filter.And(Builders<CardList>.Filter.Eq(x => x.ID, list.ID), Builders<CardList>.Filter.Eq(x => x.Table, list.Table)),
                Builders<CardList>.Update.Set(x => x.Content, list.Content));

            return (result.IsAcknowledged && result.MatchedCount == 1);
        }
    }
}
