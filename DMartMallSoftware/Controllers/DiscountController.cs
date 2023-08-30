using DMartMallSoftware.DAL;
using DMartMallSoftware.Models;
using Microsoft.AspNetCore.Mvc;

namespace DMartMallSoftware.Controllers
{
    public class DiscountController : Controller
    {
        public readonly IConfiguration configuration;
        public readonly DiscountDAL dd;

        public DiscountController(IConfiguration configuration)
        {
            this.configuration = configuration;
            dd = new DiscountDAL(configuration);
        }
        public ActionResult ShowAllDiscountTypes()
        {
            if ((HttpContext.Session.GetString("Id")) == null)
            {
                return RedirectToAction("SignIn", "Register");
            }
            var model = dd.GetDiscountType();
            return View(model);
        }

        public ActionResult AddDiscountType()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddDiscountType(DiscountModel discount)
        {

            try
            {
                int result = dd.AddDiscountType(discount);
                if (result == 1)
                {
                    TempData["AddDiscount"] = "Discount Type Added Successfully.";
                    return RedirectToAction("ShowAllDiscountTypes", "Discount");
                }
                else
                {
                    TempData["AddDiscountFail"] = "Discount Type Already Exists!";
                    return RedirectToAction("ShowAllDiscountTypes", "Discount");
                }
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Details(int Id)
        {
            var model = dd.GetDiscountTypeById(Id);
            return View(model);
        }

        public ActionResult EditDiscountType(int Id)
        {
            var model = dd.GetDiscountTypeById(Id);
            return View(model);
        }

        // POST: ProductController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditDiscountType(DiscountModel discount)
        {
            try
            {
                int result = dd.UpdateDiscountType(discount);
                if (result == 1)
                {
                    TempData["EditDiscount"] = "Discount Type Edit Successfully.";
                    return RedirectToAction(nameof(ShowAllDiscountTypes));
                }
                else
                {
                    TempData["AddDiscountFail"] = "Discount Type Already Exists!";
                    return RedirectToAction(nameof(ShowAllDiscountTypes));
                }
            }
            catch
            {
                return View();
            }
        }



        public ActionResult DeleteConfirm(int Id)
        {
            var model = dd.DeleteDiscountType(Id);
            TempData["DeleteDiscount"] = "Discount Type Delete Successfully.";
            return RedirectToAction("ShowAllDiscountTypes", "Discount");
        }
    }
}
