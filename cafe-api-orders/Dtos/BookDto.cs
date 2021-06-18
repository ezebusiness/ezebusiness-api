using dbmongo_setup.Models;

namespace cafe_api_orders.Dtos
{
    public class BookDto : MongoBaseEntity
    {
        public string BookName { get; set; }

        public decimal Price { get; set; }
    }
}