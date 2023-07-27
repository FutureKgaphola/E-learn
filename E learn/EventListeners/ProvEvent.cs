using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using E_learn.Helpers;
using E_learn.Models;
using Firebase.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace E_learn.EventListeners
{
    public class ProvEvent: Java.Lang.Object, IValueEventListener
    {

        private List<ProvModel> pPos = new List<ProvModel>();

        public event EventHandler<RetrivedProvEventHandeler> RetriveProv;
        public class RetrivedProvEventHandeler : EventArgs
        {
            public List<ProvModel> ProvList { get; set; }
        }
        public void OnCancelled(DatabaseError error)
        {
            return;
        }

        public void OnDataChange(DataSnapshot snapshot)
        {
            try
            {
                if (snapshot.Value != null)
                {
                    pPos.Clear();
                    var child = snapshot.Children.ToEnumerable<DataSnapshot>();

                    foreach (DataSnapshot data in child)
                    {

                        ProvModel uM = new ProvModel
                        {
                            prvid = data.Key,
                            provname = data.Child("provname").Value.ToString(),
                        };
                        pPos.Add(uM);
                    }
                }
            }
            catch (System.Exception fetch)
            {
                Toast.MakeText(Application.Context, "Error: couldn't fetch data as expected: " + fetch.Message.ToString(), ToastLength.Long).Show();
            }

            RetriveProv.Invoke(this, new RetrivedProvEventHandeler
            {
                ProvList = pPos
            });
        }
        public void Retrieve_Provinces()
        {
            DatabaseReference dref = firebase_Helper.GetDatabase().GetReference("Provinces");
            dref.AddValueEventListener(this);
        }

    }
}