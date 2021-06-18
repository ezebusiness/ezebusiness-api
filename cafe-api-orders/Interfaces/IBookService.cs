using dbmongo_setup.Interfaces;
using common_setup;
using cafe_common.Models;

namespace cafe_api_orders.Interfaces
{
    public interface IBookService : IMongoBaseService<Book>, IInjectable
    {

    }
}