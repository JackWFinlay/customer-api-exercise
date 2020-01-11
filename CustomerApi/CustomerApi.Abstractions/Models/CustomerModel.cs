using System;
using System.Collections.Generic;
using System.Text;

namespace CustomerApi.Abstractions.Models
{
    public class CustomerModel
    {
        public Guid CustomerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}
