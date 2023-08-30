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
            if ((HttpContext.Session.GetString("Id")) == null)
            {
                return RedirectToAction("SignIn", "Register");
            }
            Loadst();
            var model = dd.ShowAllDemandsForDealer(Name, status);
            return View(model);
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
                    TempData["DeleteDemand"] = "Request Deleted Successfully.";
                    return RedirectToAction(nameof(ShowAllDemandsForDealer));
                }
                else
                {
                    TempData["DeleteDemandError"] = "Errror Occured When Delete Request!";
                    return RedirectToAction(nameof(ShowAllDemandsForDealer));
                }
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
            try
            {
                int result = dd.ConfirmByDealer(Id);
                if (result == 1)
                {
                    TempData["ConfirmByDealer"] = "Payment Confirm Successfully.";
                    return RedirectToAction(nameof(ShowAllDemandsForDealer));
                }
                else
                {
                    TempData["ConfirmByDealerError"] = "Errror Occured When Confirm Payment!";
                    return RedirectToAction(nameof(ShowAllDemandsForDealer));
                }
            }
            catch
            {
                return View();
            }
        }
        public ActionResult PayAgain(int Id)
        {
            try
            {
                int result = dd.PayAgain(Id);
                if (result == 1)
                {
                    TempData["PayAgain"] = "Payment Not Received.";
                    return RedirectToAction(nameof(ShowAllDemandsForDealer));
                }
                else
                {
                    TempData["PayAgainError"] = "Errror Occured When Confirm Payment!";
                    return RedirectToAction(nameof(ShowAllDemandsForDealer));
                }
            }
            catch
            {
                return View();
            }
        }
        public ActionResult ShippingByDealer(int Id)
        {
            try
            {
                int result = dd.ShippingByDealer(Id);
                if (result == 1)
                {
                    TempData["ShippingByDealer"] = "Order Shipped Successfully.";
                    return RedirectToAction(nameof(ShowAllDemandsForDealer));
                }
                else
                {
                    TempData["ShippingByDealerError"] = "Errror Occured When Shipped Status!";
                    return RedirectToAction(nameof(ShowAllDemandsForDealer));
                }
            }
            catch
            {
                return View();
            }
        }
        public ActionResult DeliveredByDealer(int Id)
        {
            try
            {
                int result = dd.DeliveredByDealer(Id);
                if (result == 1)
                {
                    TempData["DeliveredByDealer"] = "Order Deliverd Successfully.";
                    return RedirectToAction(nameof(ShowAllDemandsForDealer));
                }
                else
                {
                    TempData["DeliveredByDealerError"] = "Errror Occured When Deliverd Status!";
                    return RedirectToAction(nameof(ShowAllDemandsForDealer));
                }
            }
            catch
            {
                return View();
            }
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
                    TempData["AcceptRequest"] = "Request Accept Successfully.";
                    return RedirectToAction(nameof(ShowAllDemandsForDealer));
                }
                else
                {
                    TempData["AcceptRequestError"] = "Errror Occured When Accept Request!";
                    return RedirectToAction(nameof(ShowAllDemandsForDealer));
                }
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
