﻿using DMartMallSoftware.DAL;
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
                    return RedirectToAction("ShowAllDiscountTypes", "Discount");
                }
                else
                    return View();
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
                    return RedirectToAction(nameof(ShowAllDiscountTypes));
                }
                else
                    return View();
            }
            catch
            {
                return View();
            }
        }

      

        public ActionResult DeleteConfirm(int Id)
        {
                var model = dd.DeleteDiscountType(Id);
                return RedirectToAction("ShowAllDiscountTypes", "Discount");
        }
    }
}
