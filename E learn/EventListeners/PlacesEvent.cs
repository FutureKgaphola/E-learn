using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using E_learn.Models;
using Firebase.Database;
using E_learn.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace E_learn.EventListeners
{
    public class PlacesEvent : Java.Lang.Object, IValueEventListener
    {
        private string filter;
        private List<PlacesModel> pPos = new List<PlacesModel>();

        public event EventHandler<RetrivedPlacesEventHandeler> RetrivePlaces;
        public class RetrivedPlacesEventHandeler : EventArgs
        {
            public List<PlacesModel> PlacesList { get; set; }
        }
        public void OnCancelled(DatabaseError error)
        {
            return;
        }

        public void OnDataChange(DataSnapshot snapshot)
        {
            try
            {
                NumberFormatInfo provider = new NumberFormatInfo();
                provider.NumberDecimalSeparator = ".";
                provider.NumberGroupSeparator = ",";

                if (snapshot.Value != null)
                {
                    pPos.Clear();
                    var child = snapshot.Children.ToEnumerable<DataSnapshot>();

                    foreach (DataSnapshot data in child)
                    {
                        if (this.filter!="" && data.Child("provname").Value.ToString().ToLower().Contains(this.filter.ToLower()))
                        {
                            PlacesModel uM = new PlacesModel
                            {

                                hubid = data.Key,
                                HubName = data.Child("hubname").Value.ToString(),
                                HubImage = data.Child("image").Value.ToString(),
                                HubEmail = data.Child("email").Value.ToString(),
                                Hublat = Convert.ToDouble(data.Child("lat").Value.ToString().Replace(',', '.'), provider),
                                Hublon = Convert.ToDouble(data.Child("lon").Value.ToString().Replace(',', '.'), provider)

                            };
                            pPos.Add(uM);
                        }
                        else if(this.filter == "")
                        {
                            PlacesModel uM = new PlacesModel
                            {

                                hubid = data.Key,
                                HubName = data.Child("hubname").Value.ToString(),
                                HubImage = data.Child("image").Value.ToString(),
                                HubEmail = data.Child("email").Value.ToString(),
                                Hublat = Convert.ToDouble(data.Child("lat").Value.ToString().Replace(',', '.'), provider),
                                Hublon = Convert.ToDouble(data.Child("lon").Value.ToString().Replace(',', '.'), provider)

                            };
                            pPos.Add(uM);
                        }
                            

                    }
                }
            }
            catch (System.Exception fetch)
            {
                Toast.MakeText(Application.Context, "Error: couldn't fetch data as expected: " + fetch.Message.ToString(), ToastLength.Long).Show();
            }

            RetrivePlaces.Invoke(this, new RetrivedPlacesEventHandeler
            {
                PlacesList = pPos
            });
        }
        public void Retrieve_Data(string filter)
        {
            this.filter = filter;
            DatabaseReference dref = firebase_Helper.GetDatabase().GetReference("Hubs");
            dref.AddValueEventListener(this);
        }

    }
}