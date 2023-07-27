using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace E_learn.Models
{
    public class PlacesModel
    {
        public string hubid { get; set; }
        public string HubName { get; set; }
        public string HubImage { get; set; }
        public string HubEmail { get; set; }
        public string provname { get; set; }
        public Double Hublat { get; set; }
        public Double Hublon { get; set; }
    }
}