using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdListing;
using Microsoft.AspNetCore.SignalR;

namespace _3_30_Ads_MVC_with_session.Models
{
    public class AdViewModel
    {
        public List<AdPost> Ads { get; set; }
        public List<int> Session { get; set; }
            }
}
