using dbmongo_setup.Interfaces;
using dbmongo_setup.Services;
using cafe_api_orders.Interfaces;
using cafe_common.Models;

namespace cafe_api_orders.Services
{
    public class BookService : MongoBaseService<Book>, IBookService
    {

        public BookService(IMongoGenericRepository<Book> bookRepository) : base(bookRepository) {

        }
    }
}