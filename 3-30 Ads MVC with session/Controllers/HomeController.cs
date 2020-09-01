using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using _3_30_Ads_MVC_with_session.Models;
using AdListing;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc.Formatters;
using System.Runtime.InteropServices.ComTypes;

namespace _3_30_Ads_MVC_with_session.Controllers
{

    public class HomeController : Controller
    {

        private string _conn = @"Data Source=.\sqlexpress;Initial Catalog=Ads;Integrated Security=True;";
        List<int> idList;
        public IActionResult Index()
        {
            AdViewModel vm = new AdViewModel();
            ManageAds db = new ManageAds(_conn);
            vm.Ads = db.GetPosts();
            vm.Session = HttpContext.Session.Get<List<int>>("adIdSession");
            return View(vm);
        }

        public IActionResult PostNewForm()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddPost(AdPost newAd)
        {
            ManageAds db = new ManageAds(_conn);
            AdViewModel vm = new AdViewModel();
            db.AddNewAd(newAd);

          idList = HttpContext.Session.Get<List<int>>("adIdSession");

            if (idList == null)
            {
                idList = new List<int> { newAd.Id };
            }

           else
            {
                idList.Add(newAd.Id);
            }
        
            HttpContext.Session.Set<List<int>>("adIdSession",idList);
       
            return Redirect("/Home/Index");
        }

        [HttpPost]
        public IActionResult DeletePost(int Id)
        {
            idList = HttpContext.Session.Get<List<int>>("adIdSession");
            if (idList != null && idList.Contains(Id))
            {
                ManageAds db = new ManageAds(_conn);
                db.DeletePost(Id);
            }
    
            return Redirect("/Home/Index");
        }
    }

    public static class SessionExtensions
    {
        public static void Set<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T Get<T>(this ISession session, string key)
        {
            string value = session.GetString(key);

            return value == null ? default(T) :
                JsonConvert.DeserializeObject<T>(value);
        }
    }

    
}
