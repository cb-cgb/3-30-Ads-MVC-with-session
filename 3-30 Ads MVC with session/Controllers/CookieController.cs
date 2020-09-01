using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using _3_30_Ads_MVC_with_session.Models;
using AdListing;
//using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc.Formatters;
using System.Net;

namespace _3_30_Ads_MVC_with_session.Controllers
{

    public class CookieController  : Controller
    {
       
        private string _conn = @"Data Source=.\sqlexpress;Initial Catalog=Ads;Integrated Security=True;";

        public IActionResult Index()
        {
            AdViewModelCookies vm = new AdViewModelCookies();
            ManageAds db = new ManageAds(_conn);
            vm.Ads = db.GetPosts();
            string ids =   Request.Cookies["ids"];
            if (!String.IsNullOrEmpty(ids)) //(ids != null)
            {
                vm.Cookie = ids.Split(',').ToList();
            }
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

            string idList = Request.Cookies["ids"];

            if (idList == null)
            {
                idList = $"{newAd.Id}";
            }
            else
            {
                idList += $",{newAd.Id}";
            }

            Response.Cookies.Append("ids", $"{idList}");
        
            return Redirect("/Cookie/Index");
        }

        [HttpPost]
        public IActionResult DeletePost(int Id)
        {
            ManageAds db = new ManageAds(_conn);
            db.DeletePost(Id);
            return Redirect("/Cookie/Index");
        }
    }


    
}
