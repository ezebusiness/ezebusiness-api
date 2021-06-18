using dbsql_setup.Models;
using System;

namespace cafe_api_auth.Models
{
    public class Customer : SqlBaseEntity
    {
        public Guid CustomerGuid { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PasswordSalt { get; set; }
    }
}
