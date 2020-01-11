using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CustomerApi.Abstractions.Models
{
    public class CustomerDto
    {
        [Key]
        public Guid CustomerId { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public DateTime LastUpdatedDate { get; set; }

        public bool IsDeleted { get; set; } = false;
    }
}
