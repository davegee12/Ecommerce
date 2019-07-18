using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Ecommerce.Models
{
    public class Product
    {
        [Key]
        public int ProductId{get;set;}
        // Product Name
        [Required(ErrorMessage="Product name is required")]
        [MinLength(2, ErrorMessage="Product name must be longer than two characters")]
        public string Name{get;set;}

        // Image
        [Required(ErrorMessage="Image is required")]
        public string Image{get;set;}

        // Description
        [Required(ErrorMessage="Description is required")]
        [MinLength(10, ErrorMessage="Product name must be longer than ten characters")]
        public string Description {get;set;}

        // Initial Quantity
        [Required(ErrorMessage="Quantity is required")]
        public int Quantity {get;set;}

        // Many to Many Navigations
        public List<Order> UsersWhoOrderedThisProduct {get;set;}

        public DateTime CreatedAt {get;set;} = DateTime.Now;
        public DateTime UpdatedAt {get;set;} = DateTime.Now;

    }
}