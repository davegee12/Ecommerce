using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Ecommerce.Models
{
    public class Order
    {
        [Key]
        public int OrderId{get;set;}

        public int UserId{get;set;}

        public int ProductId{get;set;}

        public int Quantity{get;set;}

        public DateTime CreatedAt{get;set;} = DateTime.Now;
        public DateTime UpdateAt{get;set;} = DateTime.Now;

        // Navigation
        public User Customer{get;set;}
        public Product ItemPurchased{get;set;}
    }
}