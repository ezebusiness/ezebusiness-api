using dbsql_setup.Interfaces;
using dbsql_setup.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace cafe_api_auth.Models
{
    public class Customer : SqlBaseEntity, IModelBuilder
    {
        public Guid CustomerGuid { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PasswordSalt { get; set; }

        public ICollection<CustomerRoleMapping> CustomerRoleMappings { get; set; }

        public void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.ToTable(typeof(Customer).Name);
            });
        }
    }

    public class CustomerRole : SqlBaseEntity, IModelBuilder
    {
        public string Name { get; set; }
        public bool Active { get; set; }
        public bool IsSystemRole { get; set; }
        public string SystemName { get; set; }

        public ICollection<CustomerRoleMapping> CustomerRoleMappings { get; set; }

        public void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CustomerRole>(entity =>
            {
                entity.ToTable(typeof(CustomerRole).Name);
            });
        }
    }

    public class CustomerRoleMapping : IModelBuilder
    {
        public int Customer_Id { get; set; }
        public Customer Customer { get; set; }
        public int CustomerRole_Id { get; set; }
        public CustomerRole CustomerRole { get; set; }

        public void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CustomerRoleMapping>(entity =>
            {
                entity.ToTable("Customer_CustomerRole_Mapping");

                entity.HasKey(sc => new { sc.Customer_Id, sc.CustomerRole_Id });

                entity.HasOne(bc => bc.Customer)
                    .WithMany(b => b.CustomerRoleMappings)
                    .HasForeignKey(bc => bc.Customer_Id);

                entity.HasOne(bc => bc.CustomerRole)
                    .WithMany(c => c.CustomerRoleMappings)
                    .HasForeignKey(bc => bc.CustomerRole_Id);
            });
        }
    }
}
