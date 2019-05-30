using Android.App;
using Android.Widget;
using Android.OS;
using Android.Content;
using Android.Views;

using System.Collections.Generic;

using Project_4.Helpers;

namespace Project_4
{
    [Activity(Label = "Flat Alexander")]
    public class RegisterActivity : Activity
    {
        private UserManagert um = UserManagert.Instance;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Register);
            ActionBar.SetDisplayHomeAsUpEnabled(true);


            Button submitRegistration = FindViewById<Button>(Resource.Id.button1);
            EditText username = FindViewById<EditText>(Resource.Id.editText1);
            EditText password = FindViewById<EditText>(Resource.Id.editText2);
            EditText repeatPassword = FindViewById<EditText>(Resource.Id.repeatPass);
            EditText name = FindViewById<EditText>(Resource.Id.editText4);
            EditText unitNumberInput = FindViewById<EditText>(Resource.Id.editText3);
            Spinner unitAddition = FindViewById<Spinner>(Resource.Id.spinner1);

            List<string> additions = new List<string> { "Geen", "A", "B", "C", "D", "E", "F"};
            var adapter = new ArrayAdapter<string>(this, Resource.Layout.spinner_item, additions);
            unitAddition.Adapter = adapter;

            submitRegistration.Click += (o, e) =>
            {
             if (password.Text != repeatPassword.Text)
              {
                    Toast.MakeText(this, "Wachtwoorden komen niet overeen.", ToastLength.Short).Show();
                }
                else
                {
                    if (!string.IsNullOrEmpty(username.Text) && !string.IsNullOrEmpty(password.Text) && !string.IsNullOrEmpty(unitNumberInput.Text) && !string.IsNullOrEmpty(name.Text))
                    {

                        var unitNumberSelectedId = unitAddition.SelectedItemId;
                        string completeUnitNumber;
                        if (!unitAddition.SelectedItem.ToString().Equals("Geen"))
                        {
                            completeUnitNumber = unitNumberInput.Text + unitAddition.SelectedItem.ToString();
                        }
                        else
                        {
                            completeUnitNumber = unitNumberInput.Text;
                        }

                        if (um.RegisterUser(username.Text, password.Text, name.Text, completeUnitNumber))
                        {
                            Toast.MakeText(this, "Registratie successvol, log in met je inlog gegevens.", ToastLength.Short).Show();
                            Intent startMain = new Intent(this, typeof(MainActivity));
                            StartActivity(startMain);
                        }
                        else
                        {
                            Toast.MakeText(this, "Gebruiker bestaat al.", ToastLength.Short).Show();
                        }
                    }
                    else
                    {
                        Toast.MakeText(this, "Alle velden zijn verplicht.", ToastLength.Long).Show();
                    }
                }
                

            };
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.ItemId == Android.Resource.Id.Home)
            {
                this.OnBackPressed();
            }
            return base.OnOptionsItemSelected(item);
        }
    }
}

