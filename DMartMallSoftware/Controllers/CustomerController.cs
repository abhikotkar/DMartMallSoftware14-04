using DMartMallSoftware.DAL;
using DMartMallSoftware.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;

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




        /* public ActionResult ShowAllCustomers(string? searchText)
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
         public ActionResult AddProduct(int Id)
         {
             ViewBag.custId = Id;
             LoadUDl();
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
                     return RedirectToAction("AddProduct", "Customer");
             }
             catch
             {
                 return View();
             }
         }

         public ActionResult EditCustomerDetails(int Id)
         {
             var model = cd.GetCustomerById(Id);
             return View(model);
         }

         [HttpPost]
         [ValidateAntiForgeryToken]
         public ActionResult EditCustomerDetails(CustomerModel customer)
         {
             try
             {
                 int result = cd.UpdateCustomerDetails(customer);
                 if (result == 1)
                 {
                     return RedirectToAction(nameof(ShowAllCustomers));
                 }
                 else
                     return View();
             }
             catch
             {
                 return View();
             }
         }



         public ActionResult EditCartItem(int Id,int CId)
         {
             ViewBag.CustIds = CId;
             LoadUDl();
             var model = cd.GetCartItemById(Id);
             return View(model);
         }

         [HttpPost]
         [ValidateAntiForgeryToken]
         public ActionResult EditCartItem(CartModel cart)
         {
             try
             {
                 int Id = cd.EditCartItem(cart);
                 if (Id >= 1)
                 {
                     return RedirectToAction("Details", "Customer", new { Id = Id });
                 }
                 else
                     return RedirectToAction("Details", "Customer", new { Id = Id });
             }
             catch
             {
                 return View();
             }
         }

         *//*[HttpPost]
         public ActionResult EditCartItem(CartModel cart)
         {

             try
             {
                 int Id = cd.EditCartItem(cart);
                 if (Id >= 1)
                 {
                     return RedirectToAction("Details", "Customer", new { Id = Id });
                 }
                 else
                     return RedirectToAction("EditCartItem", "Customer");
             }
             catch
             {
                 return View();
             }
         }*//*

         public ActionResult CancelCustomerBill(int Id)
         {
             var model = cd.CancelCustomerBill(Id);
             return RedirectToAction("ShowAllCustomers", "Customer");
         } 

         public ActionResult RemoveCartItem(CartModel cart, int CId)
         {
             var model = cd.EditCartItem(cart);
             return RedirectToAction("Details", "Customer", new { Id = CId });
         }
         private void LoadDDl()
         {
             try
             {
                 List<ProductModel> product = new List<ProductModel>();
                 product = cd.LoadProducts().ToList();
                 product.Insert(0, new ProductModel {Name = "Please Select Product"});
                 ViewBag.DiscountTypes = product;
             }
             catch (Exception ex)
             {

             }
         }

         private void LoadUDl()
         {
             try
             {
                 List<UnitModel> unit = new List<UnitModel>();
                 unit = cd.LoadUnit().ToList();
                 unit.Insert(0, new UnitModel { Id = 0, Unit = "Please Select" });
                 ViewBag.UnitTypes1 = unit;
             }
             catch (Exception ex)
             {

             }
         }*/


        public ActionResult AllCustomers(string? searchText)
        {
            if ((HttpContext.Session.GetString("Id")) == null)
            {
                return RedirectToAction("SignIn", "Register");
            }
            var model = cd.GetAllCustomers(searchText);
            return View(model);
        }

        public ActionResult EditCustomer(int Id)
        {
            var model = cd.GetCustomerById(Id, 0);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditCustomer(CustomerModelV1 customer)
        {
            try
            {
                int result = cd.UpdateCustomerDetails(customer);
                if (result == 1)
                {
                    TempData["EditCustomer"] = "Customer Details Update Successfully.";
                    return RedirectToAction(nameof(AllCustomers));
                }
                else
                {
                    TempData["EditCustomerFail"] = "Mobile No. Already Exists!";
                    return RedirectToAction("EditCustomer", "Customer", new { Id = customer.Id });
                }
            }
            catch
            {
                return View();
            }
        }

        public ActionResult CancelCustomer(int Id)
        {
            var model = cd.CancelCustomer(Id);
            if (model == 1)
            {
                TempData["CancelCust"] = "Customer Deleted Successfully.";
                return RedirectToAction("AllCustomers", "Customer");
            }
            else
            {
                TempData["CancelCustFail"] = "Some Order Present Against Customer!";
                return RedirectToAction("AllCustomers", "Customer");
            }
            return RedirectToAction("AllCustomers", "Customer");
        }

        public ActionResult AddCustomerDetail()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddCustomerDetail(CustomerModelV1 customer)
        {
            try
            {

                int result = cd.AddCustomer(customer);
                if (result == 1)
                {
                    TempData["AddCust"] = "Customer Added Successfully.";
                    return RedirectToAction("AllCustomers", "Customer");
                }
                else if (result == -1)
                {
                    TempData["AddCustFail"] = "Mobile No. Already Exists!";
                    return RedirectToAction("AllCustomers", "Customer");
                }
                else
                    return View();
            }
            catch
            {
                return View();
            }
        }

        public ActionResult CustomerDetails(int CustId, int searchText)
        {
            var model = cd.GetCustomerById(CustId, searchText);
            if (model.Id == 0)
            {
                return RedirectToAction("AllCustomers", "Customer");
            }
            return View(model);
        }

        public ActionResult CancelOrder(int Id, int CId)
        {
            var model = cd.CancelOrder(Id);
            return RedirectToAction("CustomerDetails", "Customer", new { CustId = CId, searchText = 0 });
        }

        public ActionResult AddOrder(int CustId)
        {
            try
            {
                int result = cd.AddOrder(CustId);
                if (result >= 1)
                {
                    TempData["AddOrder"] = "Order Added Successfully.";
                    return RedirectToAction("CustomerDetails", "Customer", new { CustId = CustId });
                }
                else
                {
                    TempData["AddOrderFail"] = "Existing Order Payment Has Pending!";
                    return RedirectToAction("CustomerDetails", "Customer", new { CustId = CustId });
                }
            }
            catch
            {
                return View();
            }
        }


        public ActionResult OrderDetails(int Id)
        {
            var model = cd.GetOrderById(Id);
            return View(model);
        }

        public ActionResult AddItem(int Id)
        {
            ViewBag.orderId = Id;
            LoadUDl();
            LoadDDl();
            return View();
        }

        [HttpPost]
        public ActionResult AddItem(OrderItemModel order, int id)
        {

            try
            {
                int Id = cd.AddProduct(order, id);

                return RedirectToAction("OrderDetails", "Customer", new { Id = id });

            }
            catch
            {
                return View();
            }
        }


        public ActionResult EditOrderItem(int Id, int CId)
        {
            ViewBag.CustIds = CId;
            LoadUDl();
            var model = cd.GetOrderItemById(Id);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditOrderItem(OrderItemModel order)
        {
            try
            {
                int Id = cd.EditOrderItem(order);
                return RedirectToAction("OrderDetails", "Customer", new { Id = Id });

            }
            catch
            {
                return View();
            }
        }

        public ActionResult RemoveCartItem(OrderItemModel order, int CId)
        {
            var model = cd.EditOrderItem(order);
            return RedirectToAction("OrderDetails", "Customer", new { Id = CId });
        }

        public ActionResult PayAmt(int Id)
        {
            var model = cd.PayAmt(Id);
            return RedirectToAction("OrderDetails", "Customer", new { Id = Id });
        }

        private void LoadDDl()
        {
            try
            {
                List<ProductModel> product = new List<ProductModel>();
                product = cd.LoadProducts().ToList();
                product.Insert(0, new ProductModel { Name = "Please Select Product" });
                ViewBag.DiscountTypes = product;
            }
            catch (Exception ex)
            {

            }
        }

        private void LoadUDl()
        {
            try
            {
                List<UnitModel> unit = new List<UnitModel>();
                unit = cd.LoadUnit1().ToList();
                unit.Insert(0, new UnitModel { Id = 0, Unit = "Please Select" });
                ViewBag.UnitTypes1 = unit;
            }
            catch (Exception ex)
            {

            }
        }
    }
}
