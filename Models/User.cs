using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Ecommerce.Models
{
    public class User
    {
        [Key]
        public int UserId{get;set;}
        // Name
        [Required(ErrorMessage="Name is required")]
        [MinLength(2, ErrorMessage="Name must be longer than two characters")]
        public string Name{get;set;}

        public DateTime CreatedAt{get;set;} = DateTime.Now;
        public DateTime UpdatedAt{get;set;} = DateTime.Now;

        // Foreign Key to Orders (Many to Many)

        public List<Order> OrdersMade {get;set;}

    }
}