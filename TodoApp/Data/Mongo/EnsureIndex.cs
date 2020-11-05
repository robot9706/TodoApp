using System;
using System.Collections.Generic;
using System.Text;

namespace TodoApp.Data.Mongo
{
    public enum IndexType
    { 
        Ascending,
        Descending
    }

    public class EnsureIndex : Attribute
    {
        public IndexType IndexType { get; private set; } = IndexType.Ascending;

        public bool? IsUnique { get; private set; } = null;

        public EnsureIndex(IndexType type, bool unique)
        {
            IndexType = type;
            IsUnique = unique;
        }
    }
}
