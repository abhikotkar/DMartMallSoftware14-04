using DMartMallSoftware.DAL;
using DMartMallSoftware.Models;
using Microsoft.AspNetCore.Mvc;

namespace DMartMallSoftware.Controllers
{
    public class DemandController : Controller
    {
        public readonly IConfiguration configuration;
        public readonly DemandDAL dd;

        public DemandController(IConfiguration configuration)
        {
            this.configuration = configuration;
            dd = new DemandDAL(configuration);
        }

        public ActionResult AddDemand()
        {
            LoadUDl();
            LoadDlDl();
            return View();
        }

        [HttpPost]
        public ActionResult AddDemand(DemandModel demand)
        {

            try
            {
                int result = dd.AddDemand(demand);
                if (result == 1)
                {
                    TempData["UpdateDemand"] = "Request Updated Successfully.";
                    return RedirectToAction("ShowAllDemandsForStaff", "Demand");
                }
                else
                {
                    TempData["AddDemand"] = "Request Send Successfully.";
                    return RedirectToAction("ShowAllDemandsForStaff", "Demand");
                }

            }
            catch
            {
                return View();
            }
        }

        public ActionResult ShowAllDemandsForStaff(string Name, int dealer, int status)
        {
            LoadDlDl();
            Loadst();
            var model = dd.ShowAllDemandsForStaff(Name, dealer, status);
            return View(model);
        }

        public ActionResult RejectOffer(int Id)
        {
            var model = dd.RejectOffer(Id);
            return RedirectToAction("ShowAllDemandsForStaff", "Demand");
        }

        public ActionResult ConfirmByAdmin(int Id)
        {
            var model = dd.ConfirmByAdmin(Id);
            return RedirectToAction("ShowAllDemandsForStaff", "Demand");
        }
        public ActionResult NotDelinered(int Id)
        {
            var model = dd.NotDelinered(Id);
            return RedirectToAction("ShowAllDemandsForStaff", "Demand");
        }
        public ActionResult DeliveredByAdmin(int Id)
        {
            var model = dd.DeliveredByAdmin(Id);
            return RedirectToAction("ShowAllDemandsForStaff", "Demand");
        }
        public ActionResult AddToStock(int Id)
        {

            LoadDISC();
            var model = dd.GetDemandById(Id);
            StockModel stock = new StockModel();
            stock.Name = model.Name;
            stock.Quantity = model.Quantity;
            stock.UnitId = model.UnitId;
            stock.Unit = model.Unit;
            stock.Price = model.Price;
            stock.Id = model.Id;
            LoadUDll(stock.UnitId, stock.Unit);
            return View(stock);
        }

        [HttpPost]
        public ActionResult AddToStock(StockModel stock)
        {

            try
            {
                int result = dd.AddToStock(stock);
                if (result == 1)
                {
                    return RedirectToAction("ShowAllStock", "Stock");
                }
                else
                    return View();
            }
            catch
            {
                return View();
            }
        }

        /*public ActionResult DeleteDemand(int Id)
        {

            var model = dd.GetDemandById(Id);
            return View(model);
        }

        // POST: StockController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]*/
        [ActionName("DeleteDemand")]
        public ActionResult DeleteConfirm(int Id)
        {
            try
            {
                int result = dd.DeleteDemand(Id);
                if (result == 1)
                {
                    TempData["DeleteDemand"] = "Demand Deleted Successfully.";
                    return RedirectToAction(nameof(ShowAllDemandsForStaff));
                }
                else
                {
                    TempData["DeleteDemandError"] = "Errror Occured When Delete Demand!";
                    return RedirectToAction(nameof(ShowAllDemandsForStaff));
                }
            }
            catch
            {
                return View();
            }
        }

        private void LoadUDl()
        {
            try
            {
                List<UnitModel> unit = new List<UnitModel>();
                unit = dd.LoadUnit().ToList();
                unit.Insert(0, new UnitModel { Id = 0, Unit = "Please Select" });
                ViewBag.UnitTypes = unit;
            }
            catch (Exception ex)
            {

            }
        }

        private void LoadUDll(int un, string unt)
        {
            try
            {
                List<UnitModel> unit = new List<UnitModel>();
                unit = dd.LoadUnit1(un).ToList();
                unit.Insert(0, new UnitModel { Id = un, Unit = unt });
                ViewBag.UnitTypes1 = unit;
            }
            catch (Exception ex)
            {

            }
        }

        private void LoadDlDl()
        {
            try
            {
                List<StaffModel> staff = new List<StaffModel>();
                staff = dd.LoadDealer().ToList();
                staff.Insert(0, new StaffModel { Id = 0, Name = "Please Select" });
                ViewBag.Dealers = staff;
            }
            catch (Exception ex)
            {

            }
        }

        private void LoadDISC()
        {
            try
            {
                List<DiscountModel> discount = new List<DiscountModel>();
                discount = dd.LoadDisc().ToList();
                discount.Insert(0, new DiscountModel { Id = 0, DiscountPerc = 0 });
                ViewBag.DiscountTypes = discount;
            }
            catch (Exception ex)
            {

            }
        }

        private void Loadst()
        {
            try
            {
                List<StatusModel> st = new List<StatusModel>();
                st = dd.Loadstatus().ToList();
                st.Insert(0, new StatusModel { Id = 0, Status = "Please Select" });
                ViewBag.Statuses = st;
            }
            catch (Exception ex)
            {

            }
        }
    }
}
