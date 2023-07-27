using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Util;
using Android.Views;
using Android.Widget;
using E_learn.Adapters;
using E_learn.EventListeners;
using E_learn.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Essentials;

namespace E_learn.Fragments
{
    public class HomeFragment : AndroidX.Fragment.App.Fragment
    {
        View view;
        private Button getoffer;

        private RecyclerView Recyclerintro, RecyclerFront, RecyclerBack, RecyclerCyber;
        IntroAdapter vAdapter;
        private List<IntroModel> vmodel = new List<IntroModel>();
        private IntroEvent userData = new IntroEvent();

        FrontAdapter fAdapter;
        private List<FrontModel> fmodel = new List<FrontModel>();
        private FrontEvent fData = new FrontEvent();

        BackAdapter bAdapter;
        private List<BackendModel> bmodel = new List<BackendModel>();
        private EndEvent bData = new EndEvent();

        CyberAdapter cyAdapter;
        private List<CyberModel> cymodel = new List<CyberModel>();
        private CyberEvent cyData = new CyberEvent();

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            view = inflater.Inflate(Resource.Layout.Home, container, false);

            getoffer= view.FindViewById<Button>(Resource.Id.getoffer);
            getoffer.Click += Getoffer_Click;
            Recyclerintro= view.FindViewById<RecyclerView>(Resource.Id.Recyclerintro);
            RecyclerFront = view.FindViewById<RecyclerView>(Resource.Id.RecyclerFront);
            RecyclerBack = view.FindViewById<RecyclerView>(Resource.Id.RecyclerBack);
            RecyclerCyber = view.FindViewById<RecyclerView>(Resource.Id.RecyclerCyber);
            Retrieve_Intro();
            Retrieve_Front();
            Retrieve_Back();
            Retrieve_Cyber();
            return view;
        }
        private void setintroRecycler()
        {
            LinearLayoutManager linMan = new LinearLayoutManager(Application.Context, LinearLayoutManager.Horizontal, false);
            vAdapter = new IntroAdapter(vmodel);
            Recyclerintro.SetLayoutManager(linMan);
            Recyclerintro.SetAdapter(vAdapter);
            vAdapter.ItemClick += VAdapter_ItemClick;
        }

        private async void VAdapter_ItemClick(object sender, IntroAdapterClickEventArgs e)
        {
            try
            {
                await Browser.OpenAsync(vmodel[e.Position].website.Trim(), BrowserLaunchMode.SystemPreferred);
            }
            catch (Exception ex)
            {
                Toast.MakeText(Application.Context, ex.Message, ToastLength.Long).Show();
                // An unexpected error occured. No browser may be installed on the device.
            }
        }

        private void setBackRecycler()
        {
            LinearLayoutManager linMan = new LinearLayoutManager(Application.Context, LinearLayoutManager.Horizontal, false);
            bAdapter = new BackAdapter(bmodel);
            RecyclerBack.SetLayoutManager(linMan);
            RecyclerBack.SetAdapter(bAdapter);
            bAdapter.ItemClick += BAdapter_ItemClick;
        }

        private async void BAdapter_ItemClick(object sender, BackAdapterClickEventArgs e)
        {
            try
            {
                await Browser.OpenAsync(bmodel[e.Position].website.Trim(), BrowserLaunchMode.SystemPreferred);
            }
            catch (Exception ex)
            {
                Toast.MakeText(Application.Context, ex.Message, ToastLength.Long).Show();
                // An unexpected error occured. No browser may be installed on the device.
            }
        }

        private void setCyberRecycler()
        {
            LinearLayoutManager linMan = new LinearLayoutManager(Application.Context, LinearLayoutManager.Horizontal, false);
            cyAdapter = new CyberAdapter(cymodel);
            RecyclerCyber.SetLayoutManager(linMan);
            RecyclerCyber.SetAdapter(cyAdapter);
            cyAdapter.ItemClick += CyAdapter_ItemClick;
        }

        private async void CyAdapter_ItemClick(object sender, CyberAdapterClickEventArgs e)
        {
            try
            {
                await Browser.OpenAsync(cymodel[e.Position].website.Trim(), BrowserLaunchMode.SystemPreferred);
            }
            catch (Exception ex)
            {
                Toast.MakeText(Application.Context, ex.Message, ToastLength.Long).Show();
                // An unexpected error occured. No browser may be installed on the device.
            }
        }

        private void setFrontRecycler()
        {
            LinearLayoutManager linMan = new LinearLayoutManager(Application.Context, LinearLayoutManager.Horizontal, false);
            fAdapter = new FrontAdapter(fmodel);
            RecyclerFront.SetLayoutManager(linMan);
            RecyclerFront.SetAdapter(fAdapter);
            fAdapter.ItemClick += FAdapter_ItemClick;
        }

        private async void FAdapter_ItemClick(object sender, FrontAdapterClickEventArgs e)
        {
            try
            {
                await Browser.OpenAsync(fmodel[e.Position].website.Trim(), BrowserLaunchMode.SystemPreferred);
            }
            catch (Exception ex)
            {
                Toast.MakeText(Application.Context, ex.Message, ToastLength.Long).Show();
                // An unexpected error occured. No browser may be installed on the device.
            }
            
        }

        private void Retrieve_Back()
        {
            bData.Retrieve_Data("backend development");
            bData.RetriveBack += BData_RetriveBack;
        }

        private void BData_RetriveBack(object sender, EndEvent.RetrivedBackEventHandeler e)
        {
            bmodel = e.endList;
            setBackRecycler();
        }

        private void Retrieve_Cyber()
        {
            cyData.Retrieve_Data("cyber security");
            cyData.Retrivecyber += CyData_Retrivecyber;
        }

        private void CyData_Retrivecyber(object sender, CyberEvent.RetrivedCyberEventHandeler e)
        {
            cymodel = e.cyberList;
            setCyberRecycler();
        }

        public void Retrieve_Front()
        {
            fData.Retrieve_Data("frontend development");
            fData.RetriveFront += FData_RetriveFront;
            
        }

        private void FData_RetriveFront(object sender, FrontEvent.RetrivedFrontEventHandeler e)
        {
            fmodel = e.frontList;
            setFrontRecycler();
        }

        public void Retrieve_Intro()
        {
            userData.Retrieve_Data("introduction to computers");
            userData.RetriveOrders += UserData_RetriveOrders;
        }

        private void UserData_RetriveOrders(object sender, IntroEvent.RetrivedCodesEventHandeler e)
        {
            vmodel = e.IntroList;

            setintroRecycler();
            
        }

        private void Getoffer_Click(object sender, EventArgs e)
        {
            try
            {
                PhoneDialer.Open("0712344000");
            }
            catch (ArgumentNullException anEx)
            {
                Toast.MakeText(Application.Context, anEx.ToString(), ToastLength.Long).Show();
                // Number was null or white space
            }
            catch (FeatureNotSupportedException ex)
            {
                Toast.MakeText(Application.Context, ex.ToString(), ToastLength.Long).Show();
                // Phone Dialer is not supported on this device.
            }
            catch (Exception ex)
            {
                Toast.MakeText(Application.Context, ex.ToString(), ToastLength.Long).Show();
                // Other error has occurred.
            }
        }

    }
}