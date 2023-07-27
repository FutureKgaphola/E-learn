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
using System.Linq;
using System.Text;

namespace E_learn.EventListeners
{
    public class IntroEvent : Java.Lang.Object, IValueEventListener
    {
        private List<IntroModel> pPos = new List<IntroModel>();
        public string filter;
        public event EventHandler<RetrivedCodesEventHandeler> RetriveOrders;
        public class RetrivedCodesEventHandeler : EventArgs
        {
            public List<IntroModel> IntroList { get; set; }
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
                        if (data.Child("category").Value.ToString().ToLower().Contains(this.filter))
                        {
                            IntroModel uM = new IntroModel
                            {
                                Id = data.Key,
                                category = data.Child("category").Value.ToString(),
                                img = data.Child("img").Value.ToString(),
                                name = data.Child("name").Value.ToString(),
                                website = data.Child("website").Value.ToString()

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

            RetriveOrders.Invoke(this, new RetrivedCodesEventHandeler
            {
                IntroList = pPos
            });
        }
        public void Retrieve_Data(string filter)
        {
            this.filter = filter;
            DatabaseReference dref = firebase_Helper.GetDatabase().GetReference("Modules");
            dref.AddValueEventListener(this);
        }
    }
}