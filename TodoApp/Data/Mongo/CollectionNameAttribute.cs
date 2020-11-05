using System;
using System.Collections.Generic;
using System.Text;

namespace TodoApp.Data.Mongo
{
    public class CollectionNameAttribute : Attribute
    {
        public string Name { get; private set; }

        public CollectionNameAttribute(string name)
        {
            Name = name;
        }
    }
}
