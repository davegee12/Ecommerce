using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Ecommerce.Models;
//
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Ecommerce.Controllers
{
    public class HomeController : Controller
    {
        private MyContext dbContext;

        // here we can "inject" our context service into the constructor
        public HomeController(MyContext context)
        {
            dbContext = context;
        }

        // Display Index Page
        [HttpGet("")]
        public ViewResult Index()
        {
            // grab all Users include Orders
            List<User> AllUsers = dbContext.Users
            .Include(d => d.OrdersMade)
            .ThenInclude(v => v.ItemPurchased)
            .OrderByDescending(i => i.CreatedAt)
            .Take(3)
            .ToList();

            // grab all Products
            List<Product> AllProducts = dbContext.Products
            .Include(d => d.UsersWhoOrderedThisProduct)
            .ThenInclude(v => v.Customer)
            .OrderByDescending(i => i.CreatedAt)
            .Take(5)
            .ToList();

            // grab all Orders
            List<Order> AllOrders = dbContext.Orders
            .OrderByDescending(i => i.CreatedAt)
            .Take(3)
            .ToList();

            ViewBag.AllUsers = AllUsers;
            ViewBag.AllProducts = AllProducts;
            ViewBag.AllOrders = AllOrders;
            return View("Dashboard");
        }

        // Display Products Page
        [HttpGet("/products")]
        public ViewResult Product()
        {
            List<Product> AllProducts = dbContext.Products
            .Include(d => d.UsersWhoOrderedThisProduct)
            .ThenInclude(v => v.Customer)
            .OrderByDescending(i => i.CreatedAt)
            .ToList();

            ViewBag.AllProducts = AllProducts;
            return View("Product");
        }

        // Display Orders Page
        [HttpGet("/orders")]
        public ViewResult Order()
        {
            // grab all Users include Orders
            List<User> AllUsers = dbContext.Users
            .Include(d => d.OrdersMade)
            .ThenInclude(v => v.ItemPurchased)
            .OrderByDescending(i => i.CreatedAt)
            .ToList();

            // grab all Products
            List<Product> AllProducts = dbContext.Products
            .Include(d => d.UsersWhoOrderedThisProduct)
            .ThenInclude(v => v.Customer)
            .OrderByDescending(i => i.CreatedAt)
            .ToList();

            // grab all Orders
            List<Order> AllOrders = dbContext.Orders
            .OrderByDescending(i => i.CreatedAt)
            .ToList();

            ViewBag.AllUsers = AllUsers;
            ViewBag.AllProducts = AllProducts;
            ViewBag.AllOrders = AllOrders;
            return View("Order");
        }

        // Display Customers Page
        [HttpGet("/customers")]
        public ViewResult Customer()
        {
            List<User> AllUsers = dbContext.Users
            .Include(d => d.OrdersMade)
            .ThenInclude(v => v.ItemPurchased)
            .OrderByDescending(i => i.CreatedAt)
            .ToList();

            ViewBag.AllUsers = AllUsers;
            return View("Customer");
        }

        // Create User POST route
        [HttpPost("create/customer")]
        public IActionResult CreateUser(User newUser)
        {
            if(ModelState.IsValid)
            {
                if(dbContext.Users.Any(u => u.Name == newUser.Name))
                {
                    ModelState.AddModelError("Name", "Name already exists!");

                    List<User> AllUsers = dbContext.Users
                    .Include(d => d.OrdersMade)
                    .ThenInclude(v => v.ItemPurchased)
                    .OrderByDescending(i => i.CreatedAt)
                    .ToList();

                    ViewBag.AllUsers = AllUsers;
                    return View("Customer");
                }
                else
                {
                    dbContext.Add(newUser);
                    dbContext.SaveChanges();
                    return RedirectToAction("Customer");
                }
            }
            else
            {
                List<User> AllUsers = dbContext.Users
                .Include(d => d.OrdersMade)
                .ThenInclude(v => v.ItemPurchased)
                .OrderByDescending(i => i.CreatedAt)
                .ToList();

                ViewBag.AllUsers = AllUsers;
                return View("Customer");
            }
        }

        // Delete Customer
        [HttpGet("customer/{id}/delete")]
        public IActionResult DeleteCustomer(int id)
        {
            User RetrievedUser = dbContext.Users.FirstOrDefault(m => m.UserId == id);
            dbContext.Users.Remove(RetrievedUser);
            dbContext.SaveChanges();

            // grab all Users including orders
            List<User> AllUsers = dbContext.Users
            .Include(d => d.OrdersMade)
            .ThenInclude(v => v.ItemPurchased)
            .OrderByDescending(i => i.CreatedAt)
            .ToList();

            ViewBag.AllUsers = AllUsers;
            return RedirectToAction("Customer");
        }

        // Create Product POST route
        [HttpPost("create/product")]
        public IActionResult CreateProduct(Product newProduct)
        {
            if(ModelState.IsValid)
            {
                if(dbContext.Products.Any(u => u.Name == newProduct.Name))
                {
                    ModelState.AddModelError("Name", "That product is already available!");

                    List<Product> AllProducts = dbContext.Products
                    .Include(d => d.UsersWhoOrderedThisProduct)
                    .ThenInclude(v => v.Customer)
                    .OrderByDescending(i => i.CreatedAt)
                    .ToList();

                    ViewBag.AllProducts = AllProducts;
                    return View("Product");
                }
                else
                {
                    dbContext.Add(newProduct);
                    dbContext.SaveChanges();
                    return RedirectToAction("Product");
                }
            }
            else
            {
                List<Product> AllProducts = dbContext.Products
                .Include(d => d.UsersWhoOrderedThisProduct)
                .ThenInclude(v => v.Customer)
                .OrderByDescending(i => i.CreatedAt)
                .ToList();

                ViewBag.AllProducts = AllProducts;
                return View("Product");
            }
        }

        // Create Order POST route
        [HttpPost("create/order")]
        public IActionResult CreateOrder(Order newOrder)
        {
            if(ModelState.IsValid)
            {
                var ProductBought = dbContext.Products
                .FirstOrDefault(q => q.ProductId == newOrder.ProductId);
                if(newOrder.Quantity > ProductBought.Quantity)
                {
                    ModelState.AddModelError("Quantity", "We don't have enough in stock!");

                    // grab all Users include Orders
                    List<User> AllUsers = dbContext.Users
                    .Include(d => d.OrdersMade)
                    .ThenInclude(v => v.ItemPurchased)
                    .OrderByDescending(i => i.CreatedAt)
                    .ToList();

                    // grab all Products
                    List<Product> AllProducts = dbContext.Products
                    .Include(d => d.UsersWhoOrderedThisProduct)
                    .ThenInclude(v => v.Customer)
                    .OrderByDescending(i => i.CreatedAt)
                    .ToList();

                    // grab all Orders
                    List<Order> AllOrders = dbContext.Orders
                    .OrderByDescending(i => i.CreatedAt)
                    .ToList();

                    ViewBag.AllUsers = AllUsers;
                    ViewBag.AllProducts = AllProducts;
                    ViewBag.AllOrders = AllOrders;
                    return View("Order");
                }
                else
                {
                    ProductBought.Quantity -= newOrder.Quantity;
                    dbContext.Add(newOrder);
                    dbContext.SaveChanges();
                    return RedirectToAction("Order");
                }
            }
            else
            {
                // grab all Users include Orders
                List<User> AllUsers = dbContext.Users
                .Include(d => d.OrdersMade)
                .ThenInclude(v => v.ItemPurchased)
                .OrderByDescending(i => i.CreatedAt)
                .ToList();

                // grab all Products
                List<Product> AllProducts = dbContext.Products
                .Include(d => d.UsersWhoOrderedThisProduct)
                .ThenInclude(v => v.Customer)
                .OrderByDescending(i => i.CreatedAt)
                .ToList();

                // grab all Orders
                List<Order> AllOrders = dbContext.Orders
                .OrderByDescending(i => i.CreatedAt)
                .ToList();

                ViewBag.AllUsers = AllUsers;
                ViewBag.AllProducts = AllProducts;
                ViewBag.AllOrders = AllOrders;
                return View("Order");
            }
        }

    }
}
