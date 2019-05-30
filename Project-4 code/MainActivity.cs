using Android.App;
using Android.Widget;
using Android.OS;

using System;
using System.IO;

using SQLite;

using Project_4.Helpers;
using Android.Content;

namespace Project_4
{
    [Activity(Label = "Flat Alexander", MainLauncher = true)]
    public class MainActivity : Activity
    {
        private readonly string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "project4.sqlite3");


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Main);

            var db = new SQLiteConnection(dbPath);
            Users user = new Users();
            db.CreateTable<Users>();

            UserManagert um = UserManagert.Instance;
            Button loginButton = FindViewById<Button>(Resource.Id.button1);
            Button RegistreerButton = FindViewById<Button>(Resource.Id.button2);

            loginButton.Click += (o, e) =>
            {
                EditText username = FindViewById<EditText>(Resource.Id.editText1);
                EditText password = FindViewById<EditText>(Resource.Id.editText2);
                if (!string.IsNullOrEmpty(username.Text) && !string.IsNullOrEmpty(password.Text))
                {
                    if (um.LogUserIn(username.Text, password.Text))
                    {
                        Intent startMainMenu = new Intent(this, typeof(MainMenuActivity));
                        StartActivity(startMainMenu);
                    }
                    else
                    {
                        Toast.MakeText(this, "Gebruikersnaam of wachtwoord is incorrect", ToastLength.Long).Show();
                    }
                }
                else
                {
                    // error voor lege velden
                    Toast.MakeText(this, "Gebruikersnaam of wachtwoord is niet ingevuld", ToastLength.Long).Show(); ;
                }
            };

            RegistreerButton.Click += (o, e) =>
            {
                Intent registerActivityIntent = new Intent(this, typeof(RegisterActivity));
                StartActivity(registerActivityIntent);
            };
        }
    public override void OnBackPressed()
        {
            // If the user is logged in you aren't allowed to go back to the login page, might have to make a logout button cuz school project...
            if (UserManagert.Instance.IsUserLoggedIn == false)
            {
                
            }
            else
            {
                base.OnBackPressed();
            }
        }
        }
    
}

