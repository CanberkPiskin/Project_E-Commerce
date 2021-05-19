using Project.BLL.DesignPatterns.GenericRepository.ConcRep;
using Project.COMMON.Tools;
using Project.ENTITIES.Models;
using Project.WEBUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project.WEBUI.Controllers
{
    public class RegisterController : Controller
    {
        AppUserRepository apRep;
        UserProfileRepository apdRep;

        public RegisterController()
        {
            apRep = new AppUserRepository();
            apdRep = new UserProfileRepository();
        }
        // GET: Register
        public ActionResult RegisterNow()
        {
            return View();
        }

        [HttpPost]
        public ActionResult RegisterNow(AppUserVM apvm)
        {
            AppUser appUser = apvm.AppUser;
            UserProfile profile = apvm.Profile;

            appUser.Password = DantexCrypt.Crypt(appUser.Password); //sifreyi kriptoladık

            //AppUser.Password = DantexCrypt.DeCrypt(apvm.AppUser.Password);

            if (apRep.Any(x=>x.UserName==appUser.UserName))
            {
                ViewBag.ZatenVar = "This username is already taken";
                return View();
            }
            else if (apRep.Any(x => x.Email == appUser.Email))
            {
                ViewBag.ZatenVar = "This email is already registered";
                return View();
            }

            //Kullanıcı basarılı bir şekilde register işlemini tamamladıysa ona mail gönder

            string gonderilecekMail = "Congratulations...Your account has been created. You can click the https://localhost:44362/Register/Activation/" + appUser.ActivationCode+ " link to activate your account..";

            MailSender.Send(appUser.Email, body: gonderilecekMail, subject: "Account activation!");
            apRep.Add(appUser); //öncelikle bunu eklemelisiniz. Cnkü AppUser'in ID'si ilk basta olusmalı... Cünkü biz birebir ilişkide AppUser zorunlu alan Profile ise opsiyonel alandır. Dolayısıyla ilk basta AppUser'in ID'si SaveChanges ile olusmalı ki sonra Profile'i rahatca ekleyebilelim...

            if (!string.IsNullOrEmpty(profile.FirstName) || !string.IsNullOrEmpty(profile.LastName) || !string.IsNullOrEmpty(profile.Address))
            {
                profile.ID = appUser.ID;
                apdRep.Add(profile);
            }

            return View("RegisterOk");



        }

        public ActionResult Activation(Guid id)
        {
            AppUser aktifEdilecek = apRep.FirstOrDefault(x => x.ActivationCode == id);
            if (aktifEdilecek != null)
            {
                aktifEdilecek.Active = true;
                apRep.Update(aktifEdilecek);

                TempData["HesapAktifmi"] = "Your account has been activated";
                return RedirectToAction("Login","Home");
            }
            TempData["HesapAktifmi"] = "No account to activate";
            return RedirectToAction("Login","Home");
        }

        public ActionResult RegisterOk()
        {
            return View();
        }
    }
}