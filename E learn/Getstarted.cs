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

namespace E_learn
{
    [Activity(Label = "Getstarted",NoHistory =true)]
    public class Getstarted : Activity
    {
        private Button getStarted;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.firstsceen);
            RequestedOrientation = Android.Content.PM.ScreenOrientation.Portrait;
            getStarted = FindViewById<Button>(Resource.Id.getstarted);
            getStarted.Click += GetStarted_Click;
            // Create your application here
        }

        private void GetStarted_Click(object sender, EventArgs e)
        {
            Intent i = new Intent(this, typeof(Login));
            StartActivity(i);
        }
    }
}