using Android.App;
using Android.Content;
using Android.Gms.Common;
using Android.Gms.Tasks;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Firebase.Auth;
using E_learn.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace E_learn
{
    [Activity(Label = "Login")]
    public class Login : Activity, IOnFailureListener, IOnSuccessListener
    {
        private Button normallogin;
        private FirebaseAuth auth;
        private EditText edtpassword, username;
        private Android.App.AlertDialog dialogs;
        private Android.App.AlertDialog.Builder dialogBuilders;
        private TextView goregister, reset;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.login);
            RequestedOrientation = Android.Content.PM.ScreenOrientation.Portrait;
            normallogin = FindViewById<Button>(Resource.Id.normallogin);
            normallogin.Click += Normallogin_Click;

            edtpassword= FindViewById<EditText>(Resource.Id.edtpassword);
            username = FindViewById<EditText>(Resource.Id.username);

            reset = FindViewById<TextView>(Resource.Id.reset);
            reset.Click += Reset_Click;
            goregister = FindViewById<TextView>(Resource.Id.goregister);
            goregister.Click += Goregister_Click;
            // Create your application here
        }

        private void Goregister_Click(object sender, EventArgs e)
        {
            Intent i = new Intent(this, typeof(addUser));
            StartActivity(i);
        }

        private void Reset_Click(object sender, EventArgs e)
        {
            Intent i = new Intent(this, typeof(forgotpassword));
            StartActivity(i);
        }

        private bool valid()
        {
            bool result = true;
            if (string.IsNullOrEmpty(username.Text.Trim()))
            {
                username.Error = "rectify field";
                result = false;
            }
            if (string.IsNullOrEmpty(edtpassword.Text))
            {
                result = false;
                edtpassword.Error = "rectify field";
            }
            return result;
        }
        private void ProgressShow()
        {
            dialogBuilders = new Android.App.AlertDialog.Builder(this);
            LayoutInflater inflater = LayoutInflater.From(this);
            View dialogView = inflater.Inflate(Resource.Layout.loginDialog, null);
            dialogBuilders.SetView(dialogView);
            dialogBuilders.SetCancelable(false);
            dialogs = dialogBuilders.Create();
            dialogs.ShowEvent += Dialogs_ShowEvent;
            dialogs.Show();
           
            
        }

        private void Dialogs_ShowEvent(object sender, EventArgs e)
        {
            if (dialogs.IsShowing==true)
            {
                auth = new firebase_Helper().GetFirebaseAuth();
                auth.SignInWithEmailAndPassword(username.Text, edtpassword.Text)
                    .AddOnSuccessListener(this)
                    .AddOnFailureListener(this);
            }
        }

        private void ProgressStop()
        {
            dialogBuilders.SetCancelable(true);
            dialogs.Dismiss();
        }
        private void Normallogin_Click(object sender, EventArgs e)
        {
            if(valid())
            {
                ProgressShow(); 
            }
            else
            {
                return;
            }
        }

        public void OnSuccess(Java.Lang.Object result)
        {
            ProgressStop();
            edtpassword.Text = "";
            username.Text = "";
            Intent i = new Intent(this, typeof(MainActivity));
            StartActivity(i);
        }

        public void OnFailure(Java.Lang.Exception e)
        {
            ProgressStop();
            Android.Support.V7.App.AlertDialog.Builder builder = new Android.Support.V7.App.AlertDialog.Builder(this);
            builder.SetTitle("Error");
            builder.SetIcon(Resource.Mipmap.elearn);
            builder.SetMessage("Something went wrong: " + e.Message.ToString());
            builder.SetNeutralButton("OK", delegate
            {
                builder.Dispose();
            });
            builder.Show();
        }
    }
}