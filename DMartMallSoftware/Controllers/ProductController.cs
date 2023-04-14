using DMartMallSoftware.DAL;
using DMartMallSoftware.Models;
using Microsoft.AspNetCore.Mvc;

namespace DMartMallSoftware.Controllers
{
    public class ProductController : Controller
    {
        public readonly IConfiguration configuration;
        public readonly ProductDAL pd;

        public ProductController(IConfiguration configuration)
        {
            this.configuration = configuration;
            pd = new ProductDAL(configuration);
        }
        public ActionResult ShowAllStock()
        {
            var model = pd.GetStock();
            return View(model);
        }

        public ActionResult AddProduct()
        {
            LoadDDl();
            return View();
        }

        [HttpPost]
        public ActionResult AddProduct(ProductModel product)
        {

            try
            {
                int result = pd.AddProduct(product);
                if (result == 1)
                {
                    return RedirectToAction("ShowAllStock", "Product");
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
                List<DiscountModel> discount = new List<DiscountModel>();
                discount = pd.LoadDiscount().ToList();
               // discount.Insert(0, new DiscountModel { Id = 0, UserType = "Please Select" });
                ViewBag.DiscountTypes = discount;
            }
            catch (Exception ex)
            {

            }
        }

        public ActionResult Details(int Id)
        {
            var model = pd.GetProductById(Id);
            return View(model);
        }

        public ActionResult EditProduct(int Id)
        {
            LoadDDl();
            var model = pd.GetProductById(Id);
            return View(model);
        }

        // POST: ProductController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditProduct(ProductModel product)
        {
            try
            {
                int result = pd.UpdateProduct(product);
                if (result == 1)
                {
                    return RedirectToAction(nameof(ShowAllStock));
                }
                else
                    return View();
            }
            catch
            {
                return View();
            }
        }

        public ActionResult DeleteProduct(int Id)
        {
            var model = pd.GetProductById(Id);
            return View(model);
        }

        // POST: ProductController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("DeleteProduct")]
        public ActionResult DeleteConfirm(int Id)
        {
            try
            {
                int result = pd.DeleteProduct(Id);
                if (result == 1)
                {
                    return RedirectToAction(nameof(ShowAllStock));
                }
                else
                    return View();
            }
            catch
            {
                return View();
            }
        }
    }
}
