using DMartMallSoftware.DAL;
using DMartMallSoftware.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace DMartMallSoftware.Controllers
{
    public class StockController : Controller
    {
        public readonly IConfiguration configuration;
        public readonly StockDAL ps;

        public StockController(IConfiguration configuration)
        {
            this.configuration = configuration;
            ps = new StockDAL(configuration);
        }
        public ActionResult ShowAllStock(string Name)
        {
            var model = ps.GetStock(Name);
            return View(model);
        }

        public ActionResult EditStock(int Id)
        {
            LoadDDl();
            LoadUDl();
            var model = ps.GetStockById(Id);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditStock(StockModel stock)
        {
            try
            {
                int result = ps.UpdateStock(stock);
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

        public ActionResult DeleteStock(int Id)
        {
            var model = ps.GetStockById(Id);
            return View(model);
        }

        // POST: StockController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("DeleteStock")]
        public ActionResult DeleteConfirm(int Id)
        {
            try
            {
                int result = ps.DeleteStock(Id);
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


        private void LoadDDl()
        {
            try
            {
                List<DiscountModel> discount = new List<DiscountModel>();
                discount = ps.LoadDiscount().ToList();
                //discount.Insert(0, new DiscountModel { Id = 0, DiscountPerc = "Please Select" });
                ViewBag.DiscountTypes = discount;
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
                unit = ps.LoadUnit().ToList();
                unit.Insert(0, new UnitModel { Id = 0, Unit = "Please Select" });
                ViewBag.UnitTypes = unit;
            }
            catch (Exception ex)
            {

            }
        }
    }
}
