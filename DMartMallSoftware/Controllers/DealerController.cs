using DMartMallSoftware.DAL;
using DMartMallSoftware.Models;
using Microsoft.AspNetCore.Mvc;

namespace DMartMallSoftware.Controllers
{
    public class DealerController : Controller
    {
        public readonly IConfiguration configuration;
        public readonly DealerDAL dd;

        public DealerController(IConfiguration configuration)
        {
            this.configuration = configuration;
            dd = new DealerDAL(configuration);
        }

        public ActionResult ShowAllDemandsForDealer(string Name, int status)
        {
            Loadst();
            var model = dd.ShowAllDemandsForDealer( Name,  status);
            return View(model);
        }

        public ActionResult DeleteDemand(int Id)
        {
            var model = dd.GetDemandById(Id);
            return View(model);
        }

        // POST: StockController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("DeleteDemand")]
        public ActionResult DeleteConfirm(int Id)
        {
            try
            {
                int result = dd.DeleteDemand(Id);
                if (result == 1)
                {
                    return RedirectToAction(nameof(ShowAllDemandsForDealer));
                }
                else
                    return View();
            }
            catch
            {
                return View();
            }
        }

        public ActionResult AcceptRequest(int Id)
        {
            var model = dd.GetDemandById(Id);
            return View(model);
        }

        public ActionResult ConfirmByDealer(int Id)
        {
            var model = dd.ConfirmByDealer(Id);
            return RedirectToAction("ShowAllDemandsForDealer", "Dealer");
        }
        public ActionResult PayAgain(int Id)
        {
            var model = dd.PayAgain(Id);
            return RedirectToAction("ShowAllDemandsForDealer", "Dealer");
        }
        public ActionResult ShippingByDealer(int Id)
        {
            var model = dd.ShippingByDealer(Id);
            return RedirectToAction("ShowAllDemandsForDealer", "Dealer");
        }
        public ActionResult DeliveredByDealer(int Id)
        {
            var model = dd.DeliveredByDealer(Id);
            return RedirectToAction("ShowAllDemandsForDealer", "Dealer");
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AcceptRequest(DemandModel demand)
        {
            try
            {
                int result = dd.AcceptRequest(demand);
                if (result == 1)
                {
                    return RedirectToAction(nameof(ShowAllDemandsForDealer));
                }
                else
                    return View();
            }
            catch
            {
                return View();
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
