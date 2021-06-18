using dbmongo_setup.Models;
using System.Collections.Generic;

namespace cafe_common.Models
{
    public class Book : MongoBaseEntity
    {
        public string BookName { get; set; }

        public decimal Price { get; set; }

        public string Category { get; set; }

        public string Author { get; set; }

        public ICollection<Authors> Authors { get; set; }
    }

    public class Authors
    {
        public string Ref { get; set; }

        public string Name { get; set; }
    }
}