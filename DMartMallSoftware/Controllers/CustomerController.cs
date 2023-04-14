﻿using DMartMallSoftware.DAL;
using DMartMallSoftware.Models;
using Microsoft.AspNetCore.Mvc;

namespace DMartMallSoftware.Controllers
{
  
    public class CustomerController : Controller
    {
        int custId;
        public readonly IConfiguration configuration;
        public readonly CusromerDAL cd;

        public CustomerController(IConfiguration configuration)
        {
            this.configuration = configuration;
            cd = new CusromerDAL(configuration);
        }

        

       
        public ActionResult ShowAllCustomers(string? searchText)
        {
          
            var model = cd.GetAllCustomers(searchText);
            return View(model);
        }


        public ActionResult Details(int Id)
        {
            var model = cd.GetCustomerById(Id);
            return View(model);
        }

        public ActionResult AddCustomer()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddCustomer(CustomerModel customer)
        {

            try
            {
             
                int result = cd.AddCustomer(customer);
                if (result == 1)
                {
                    return RedirectToAction("ShowAllCustomers", "Customer");
                }
                else if (result == -1)
                {
                    return RedirectToAction("ShowAllCustomers", "Customer");
                }
                else
                    return View();
            }
            catch
            {
                return View();
            }
        }
        public ActionResult AddProduct()
        {
            LoadDDl();
            return View();
        }

        [HttpPost]
        public ActionResult AddProduct(CartModel cart,int id)
        {

            try
            {
                int Id = cd.AddProduct(cart,id);
                if (Id >= 1)
                {
                    return RedirectToAction("Details", "Customer", new { Id=Id});
                }
                else
                    return View();
            }
            catch
            {
                return View();
            }
        }


        private void LoadDDl()
        {
            try
            {
                List<ProductModel> product = new List<ProductModel>();
                product = cd.LoadProducts().ToList();
                product.Insert(0, new ProductModel { Id = 0, Name = "Please Select Product",Price=0 });
                ViewBag.DiscountTypes = product;
            }
            catch (Exception ex)
            {

            }
        }
    }
}
