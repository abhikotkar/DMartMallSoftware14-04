using DMartMallSoftware.DAL;
using DMartMallSoftware.Models;
using Microsoft.AspNetCore.Mvc;

namespace DMartMallSoftware.Controllers
{
    public class RegisterController : Controller
    {
        public readonly IConfiguration configuration;
        public readonly RegisterDAL rd;

        public RegisterController(IConfiguration configuration)
        {
           this.configuration  = configuration;
           rd = new RegisterDAL(configuration);
        }


        public ActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SignIn(StaffModel staff)

        {

            StaffModel user = rd.UserLogin(staff);

            if (user.Password == staff.Password && user.IsConfirmed==true)
            {
                HttpContext.Session.SetString("Name", user.Name.ToString());
                HttpContext.Session.SetString("UserTypeId", user.UserTypeId.ToString());
                HttpContext.Session.SetString("Email", user.Email.ToString());
                HttpContext.Session.SetString("Id", user.Id.ToString());

                return RedirectToAction("Index", "Home");
                /* }
                 else if (user.UserTypeId != 1)
                 {
                     return RedirectToAction("SignIn", "Register");
                 }
                 else
                     return View();
     */

            }
            else
                return View();




        }

        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("SignIn");
        }

        // POST: RegisterController/Create/(Add Staff)
        public ActionResult Create()
         {
            LoadDDl();
            LoadGDl();
            LoadQDl();
            return View();
        }

        [HttpPost]
        public ActionResult Create(StaffModel staff)
        {

            try
            {
                int result = rd.AddStaff(staff);
                if (result == 1)
                {
                    return RedirectToAction("SignIn", "Register");
                }
                else
                    return RedirectToAction("Create", "Register");
            }
            catch
            {
                return View();
            }
        }

        // POST: RegisterController/SignIn/(For login Authentications)
       
        //GET: RegisterController/SignIn/(For Logout session)

       

        //Show all Staff Details
        public ActionResult ShowAllStaff(string Name, int TextId,string IsConfirmed)
        {
            LoadDDl();
            var model = rd.GetStaff(Name, TextId, IsConfirmed);
            return View(model);
        }


        //show details by Id
        public ActionResult Details(int Id)
        {
            var model = rd.GetStaffById(Id);
            return View(model);
        }

        public ActionResult Edit(int Id)
        {
            LoadDDl1();
            var model = rd.GetStaffById(Id);            
            return View(model);
        }

        // POST: ProductController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(StaffModel staff)
        {
            try
            {
                int result = rd.UpdateStaff(staff);
                if (result == 1)
                {
                    return RedirectToAction(nameof(ShowAllStaff));
                }
                else
                    return View();
            }
            catch
            {
                return View();
            }
        }


        public ActionResult EditMyProfile(int Id)
        {
            LoadQDl();
            var model = rd.GetStaffById(Id);
            return View(model);
        }

        // POST: ProductController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditMyProfile(StaffModel staff)
        {
            try
            {
                int result = rd.UpdateMyProfile(staff);
                if (result == 1)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                    return View();
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Delete(int Id)
        {
            var model = rd.GetStaffById(Id);
            return View(model);
        }

        // POST: ProductController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public ActionResult DeleteConfirm(int Id)
        {
            try
            {
                int result = rd.DeleteMember(Id);
                if (result == 1)
                {
                    return RedirectToAction(nameof(ShowAllStaff));
                }
                else
                    return View();
            }
            catch
            {
                return View();
            }
        }

        //forgot password

        public ActionResult ForgotPassword()
        {
            LoadQDl();
            return View();
        }

        [HttpPost]
        public IActionResult ForgotPassword(StaffModel staff)

        {

            try
            {
                int result = rd.CheckAns(staff);
                if (result != 0)
                {
                    return RedirectToAction("ForgotPasswordNew", "Register", new {Id=result});
                }
                else
                    return RedirectToAction("ForgotPassword", "Register");
            }
            catch
            {
                return View();
            }

        }

        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ChangePassword(StaffModel staff)
        {

            try
            {
                int result = rd.ChangePass(staff);
                if (result != 0)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                    return RedirectToAction("ChangePassword", "Register");
            }
            catch
            {
                return View();
            }

        }

        public ActionResult ForgotPasswordNew()
        {
            return View();
        }
        [HttpPost]
        public IActionResult ForgotPasswordNew(StaffModel staff)

        {

            try
            {
                int result = rd.forgate(staff);
                if (result != 0)
                {
                    return RedirectToAction("SignIn", "Register");
                }
                else
                    return RedirectToAction("ForgotPasswordNew", "Register");
            }
            catch
            {
                return View();
            }

        }

        //for usertype,gender,quetion dropdown
        private void LoadDDl()
        {
            try
            {
                List<UserTypes> user1 = new List<UserTypes>();
                user1 = rd.LoadUser().ToList();
                user1.Insert(0, new UserTypes { Id = 0, UserType = "Please Select" });
                ViewBag.UTypes = user1;
            }
            catch (Exception ex)
            {

            }
        }
        private void LoadDDl1()
        {
            try
            {
                List<UserTypes> user1 = new List<UserTypes>();
                user1 = rd.LoadUser().ToList();
                user1.Insert(0, new UserTypes { Id = 0, UserType = "Please Select" });
                ViewBag.UTypes1 = user1;
            }
            catch (Exception ex)
            {

            }
        }
        private void LoadGDl()
        {
            try
            {
                List<GenderModel> user1 = new List<GenderModel>();
                user1 = rd.LoadGender().ToList();
                user1.Insert(0, new GenderModel { Id = 0, Gender = "Please Select" });
                ViewBag.Genders = user1;
            }
            catch (Exception ex)
            {

            }
        }

        private void LoadQDl()
        {
            try
            {
                List<QuetionModel> user1 = new List<QuetionModel>();
                user1 = rd.LoadQuestion().ToList();
                user1.Insert(0, new QuetionModel { Id = 0, Quetion = "Please Select" });
                ViewBag.Quetions = user1;
            }
            catch (Exception ex)
            {

            }
        }

    }
}
