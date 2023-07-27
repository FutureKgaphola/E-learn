using Android.App;
using Android.Content;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Util;
using Android.Views;
using Android.Widget;
using E_learn.Adapters;
using E_learn.EventListeners;
using E_learn.Models;
using FFImageLoading;
using Google.Android.Material.Button;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Xamarin.Essentials;

namespace E_learn.Fragments
{
    public class HubsFragment : AndroidX.Fragment.App.Fragment, IOnMapReadyCallback
    {
         View view;
        private GoogleMap GMap;
        private RecyclerView RecyclerHubs, RecyclerProv;
        MaterialButton Enquiries;
        private LinearLayout distance;
        private TextView txtdistance;

        HubsAdapter huAdapter;
        private List<PlacesModel> pmodel = new List<PlacesModel>();
        private PlacesEvent placesData = new PlacesEvent();

        ProvAdapter prAdapter;
        private List<ProvModel> provmodel = new List<ProvModel>();
        private ProvEvent provData = new ProvEvent();

        private CardView cardView1;

        private TextView textname;
        private ImageView globe;
        string mylat, mylon;
        string filter = "";

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            view = inflater.Inflate(Resource.Layout.Hubs, container, false);
            cardView1= view.FindViewById<CardView>(Resource.Id.cardView1);
            cardView1.Visibility = ViewStates.Gone;
            Enquiries = view.FindViewById<MaterialButton>(Resource.Id.booking);
            Enquiries.Click += Enquiries_Click;
            textname = view.FindViewById<TextView>(Resource.Id.textname);
            globe = view.FindViewById<ImageView>(Resource.Id.globe);
            RecyclerHubs = view.FindViewById<RecyclerView>(Resource.Id.RecyclerHubs);
            RecyclerProv = view.FindViewById<RecyclerView>(Resource.Id.provinces);
            distance = view.FindViewById<LinearLayout>(Resource.Id.distance);
            txtdistance = view.FindViewById<TextView>(Resource.Id.txtdistance);
            txtdistance.Text = "0"; 
    
            SetUpMap();
            Get_places(filter);
            Get_provinces();
            return view;
        }

        private async void gemytlocationAsync()
        {
            try
            {
                var location = await Geolocation.GetLastKnownLocationAsync();

                if (location != null)
                {
                    mylat = location.Latitude.ToString();
                    mylon = location.Longitude.ToString();
                    
                    // Console.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}");
                }
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                Toast.MakeText(Application.Context, "Handle not supported on device exception: " + fnsEx.Message, ToastLength.Long).Show();
                // Handle not supported on device exception
            }
            catch (FeatureNotEnabledException fneEx)
            {
                Toast.MakeText(Application.Context, "Handle not enabled on device exception: " + fneEx.Message, ToastLength.Long).Show();
                // Handle not enabled on device exception
            }
            catch (PermissionException pEx)
            {
                Toast.MakeText(Application.Context, "Handle permission exception: " + pEx.Message, ToastLength.Long).Show();
                // Handle permission exception
            }
            catch (Exception ex)
            {
                Toast.MakeText(Application.Context, "Unable to get location: " + ex.Message, ToastLength.Long).Show();
                // Unable to get location
            }
        }

        private void Get_provinces()
        {
            provData.Retrieve_Provinces();
            provData.RetriveProv += ProvData_RetriveProv;
        }

        private void ProvData_RetriveProv(object sender, ProvEvent.RetrivedProvEventHandeler e)
        {
            provmodel = e.ProvList;
            setProvRecycler();
        }
        void setProvRecycler()
        {
            LinearLayoutManager linMan = new LinearLayoutManager(Application.Context, LinearLayoutManager.Vertical, false);
            prAdapter = new ProvAdapter(provmodel);
            RecyclerProv.SetLayoutManager(linMan);
            RecyclerProv.SetAdapter(prAdapter);
            prAdapter.provClick += PrAdapter_provClick;
        }

        private void PrAdapter_provClick(object sender, ProvAdapterClickEventArgs e)
        {      
            if(provmodel[e.Position].provname.Trim().ToLower()=="all")
            {
                filter = "";
            }
            else
            {
                filter = provmodel[e.Position].provname.Trim();
            }
     
            huAdapter=null;
            pmodel = null;

            placesData = new PlacesEvent();

            Get_places(filter);
        }

        private void Get_places(string filter)
        {
            placesData.Retrieve_Data(filter);
            placesData.RetrivePlaces += PlacesData_RetrivePlaces;
        }

        private void PlacesData_RetrivePlaces(object sender, PlacesEvent.RetrivedPlacesEventHandeler e)
        {
            pmodel = e.PlacesList;
            setPlcesRecycler();
        }

        private void setPlcesRecycler()
        {
            LinearLayoutManager linMan = new LinearLayoutManager(Application.Context, LinearLayoutManager.Horizontal, false);
            huAdapter = new HubsAdapter(pmodel);
            RecyclerHubs.SetLayoutManager(linMan);
            RecyclerHubs.SetAdapter(huAdapter);
            huAdapter.HubButtonClick += HuAdapter_HubButtonClick;
        }
        string name = "", email="";
        private void HuAdapter_HubButtonClick(object sender, HubsAdapterClickEventArgs e)
        {
            cardView1.Visibility = ViewStates.Gone;
            cardView1.Visibility = ViewStates.Visible;
            name = pmodel[e.Position].HubName;
            email = pmodel[e.Position].HubEmail;
            textname.Text = name.Trim();
            getimage(pmodel[e.Position].HubImage.Trim(), globe);
            locateAred(pmodel[e.Position].HubName, pmodel[e.Position].Hublat, pmodel[e.Position].Hublon);
        }
        void getimage(string imageUrl, ImageView imageView)
        {
            if (imageUrl.ToLower().Contains("http") == true)
            {
                ImageService.Instance.LoadUrl(imageUrl)
                .Retry(3, 200)
                .DownSample(350, 350)
                .Into(imageView);
            }
            else { imageView.SetBackgroundResource(Resource.Mipmap.elearn); }
        }


        private async void Enquiries_Click(object sender, EventArgs e)
        {
            try
            {
                List<string> recipients = new List<string>();
                recipients.Add(email.ToLower().Trim());
                string subject = "Set Appointment";
                string body = "Hi "+ name+ " Team \n I would like to come to your facility to learn some programming at your facility. \n\n Kind Regards,\n [Someone Surname].";
                var message = new EmailMessage
                {
                    Subject = subject,
                    Body = body,
                    To = recipients,
                    //Cc = ccRecipients,
                    //Bcc = bccRecipients
                };
                await Email.ComposeAsync(message);
            }
            catch (FeatureNotSupportedException fbsEx)
            {
                Toast.MakeText(Application.Context, fbsEx.Message,ToastLength.Long).Show();
                // Email is not supported on this device
            }
            catch (Exception ex)
            {
                Toast.MakeText(Application.Context, ex.Message, ToastLength.Long).Show();
                // Some other exception occurred
            }
        }

        private void locateAred(string area,Double lat, Double lon)
        {
            try
            {
                LatLng latlng = new LatLng(Convert.ToDouble(lat), Convert.ToDouble(lon));
                CameraUpdate camera = CameraUpdateFactory.NewLatLngZoom(latlng, 15);
                GMap.MapType = GoogleMap.MapTypeTerrain;
                GMap.AnimateCamera(camera);
                MarkerOptions options = new MarkerOptions().SetPosition(latlng).SetTitle(area).SetIcon(BitmapDescriptorFactory.FromResource(Resource.Drawable.buildingmini));
                GMap.AddMarker(options);

                NumberFormatInfo provider = new NumberFormatInfo();
                provider.NumberDecimalSeparator = ".";
                provider.NumberGroupSeparator = ",";
                if (mylat != "" && mylon != "")
                {
                    caluculateDistance(lat, lon, Convert.ToDouble(mylat.Replace(',', '.'), provider), Convert.ToDouble(mylon.Replace(',', '.'), provider));
                }
                
            }
            catch (Exception ex)
            {
                Toast.MakeText(Application.Context,ex.Message,ToastLength.Long).Show();
            }
        }
        private void caluculateDistance(Double hlat, Double hlon, Double mlat, Double mlon)
        {
            Location hubArea = new Location(hlat, hlon);
            Location myArea = new Location(mlat, mlon);
            double km = Location.CalculateDistance(hubArea, myArea, DistanceUnits.Kilometers);
            txtdistance.Text = Math.Round(km,1).ToString();
        }

        private void SetUpMap()
        {
            if (GMap == null)
            {
                var mapFragment = (SupportMapFragment)ChildFragmentManager.FindFragmentById(Resource.Id.map);
               mapFragment.GetMapAsync(this);
            }
        }
        public void OnMapReady(GoogleMap googleMap)
        {
            try
            {
                gemytlocationAsync();

                NumberFormatInfo provider = new NumberFormatInfo();
                provider.NumberDecimalSeparator = ".";
                provider.NumberGroupSeparator = ",";
                if (mylat != "" && mylon != "")
                {
                    this.GMap = googleMap;
                    LatLng latlng = new LatLng(Convert.ToDouble(mylat.Replace(',', '.'), provider), Convert.ToDouble(mylon.Replace(',', '.'), provider));
                    CameraUpdate camera = CameraUpdateFactory.NewLatLngZoom(latlng, 14);
                    GMap.MapType = GoogleMap.MapTypeTerrain;
                    GMap.AnimateCamera(camera);
                    MarkerOptions options = new MarkerOptions().SetPosition(latlng).SetTitle("I am Here currently").SetIcon(BitmapDescriptorFactory.FromResource(Resource.Drawable.smallgirl));
                    GMap.AddMarker(options);
                }
                else
                {
                    this.GMap = googleMap;
                }
            }
            catch (Exception mapE)
            {
                //Toast.MakeText(Application.Context, mapE.Message,ToastLength.Long).Show();
            }
            finally
            {
                this.GMap = googleMap;
            }
            
        }
    }
}