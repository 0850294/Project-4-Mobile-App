using Android.App;
using Android.Widget;
using Android.OS;
using Android.Content;

using System;
using System.IO;

using Project_4.Helpers;

namespace Project_4
{
    [Activity(Label = "Hoofdmenu")]
    public class MainMenuActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.MainMenu);

            Button RequestReparation = FindViewById<Button>(Resource.Id.button1);
            Button Roteb = FindViewById<Button>(Resource.Id.button2);
            Button Washing = FindViewById<Button>(Resource.Id.button3);
            Button logout = FindViewById<Button>(Resource.Id.button4);

            RequestReparation.Click += (o, e) =>
            {
                Intent startReparationRequest = new Intent(this, typeof(RequestReparation));
                StartActivity(startReparationRequest);
            };

            Roteb.Click += (o, e) =>
            {
                Intent rotebActivityIntent = new Intent(this, typeof(RotebActivity));
                StartActivity(rotebActivityIntent);
            };

            Washing.Click += (o, e) =>
            {
                Intent washingActivityIntent = new Intent(this, typeof(WashingActivity));
                StartActivity(washingActivityIntent);
            };

            logout.Click += (o, e) =>
            {
                if(UserManagert.Instance.LogUserOut())
                {
                    Intent logUserOut = new Intent(this, typeof(MainActivity));
                    StartActivity(logUserOut);
                } else
                {
                    Toast.MakeText(this, "Hoe heb je dit voor mekaar gekregen?", ToastLength.Long).Show();
                }
            };
        }

        public override void OnBackPressed()
        {
            // If the user is logged in you aren't allowed to go back to the login page, might have to make a logout button cuz school project...
            if(!UserManagert.Instance.IsUserLoggedIn)
            {
                base.OnBackPressed();
            }

            
        }

    }
}

