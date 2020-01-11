using CustomerApi.Abstractions.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CustomerApi.Data
{
    public class CustomerContext : DbContext
    {
        public DbSet<CustomerDto> Customers { get; set; }

        public CustomerContext()
        {
        }

        public CustomerContext(DbContextOptions<CustomerContext> options) : base(options)
        {
        }
    }
}
