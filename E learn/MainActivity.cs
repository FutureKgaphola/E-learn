using Android.App;
using Android.OS;
using Android.Runtime;

using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using E_learn.Fragments;
using Google.Android.Material.BottomNavigation;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace E_learn
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = false)]
    public class MainActivity : AppCompatActivity, BottomNavigationView.IOnNavigationItemSelectedListener
    {
        private AndroidX.Fragment.App.Fragment Currentfragment;
        private HomeFragment homeFragment;
        private HubsFragment hubsFragment;

        private Stack<AndroidX.Fragment.App.Fragment> mstackFragment;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);
            RequestedOrientation = Android.Content.PM.ScreenOrientation.Portrait;
            BottomNavigationView navigation = FindViewById<BottomNavigationView>(Resource.Id.navigation);
            navigation.SetOnNavigationItemSelectedListener(this);

            homeFragment = new HomeFragment();
            hubsFragment = new HubsFragment();

            mstackFragment = new Stack<AndroidX.Fragment.App.Fragment>();

            var trans = SupportFragmentManager.BeginTransaction();

            trans.Add(Resource.Id.fragmentContainer, hubsFragment, "Fragment2");
            trans.Hide(hubsFragment);
            trans.Add(Resource.Id.fragmentContainer, homeFragment, "Fragment1");
            trans.Commit();
            Currentfragment = homeFragment;
            
        }

        

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
        private void ShowFragment(AndroidX.Fragment.App.Fragment fragment)
        {
            var trans = SupportFragmentManager.BeginTransaction();
            trans.Hide(Currentfragment);
            trans.Show(fragment);
            trans.AddToBackStack(null);
            trans.Commit();
            mstackFragment.Push(Currentfragment);
            Currentfragment = fragment;
        }
        public override void OnBackPressed() { }
        public bool OnNavigationItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.navigation_home:
                    ShowFragment(homeFragment);
                    return true;
                case Resource.Id.navigation_dashboard:
                    ShowFragment(hubsFragment);
                    return true;
                case Resource.Id.navigation_notifications:
                    Toast.MakeText(this, "Bye, hope we learn soon.", ToastLength.Long).Show();
                    Finish();
                    return true;
            }
            return false;
        }
    }
}

