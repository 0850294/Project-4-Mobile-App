using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using Android.Graphics;

using Project_4.Helpers;
using Android.Views;

using System;

namespace Project_4
{
    [Activity(Label = "Reparatieverzoek")]
    public class RequestReparation : Activity
    {
        UserManagert userManager = UserManagert.Instance;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.RequestReparation);
            ActionBar.SetDisplayHomeAsUpEnabled(true);

            EditText editOmschrijving = FindViewById<EditText>(Resource.Id.editOmschrijving);
            TextView textViewCounter = FindViewById<TextView>(Resource.Id.textViewCounter);
            Button sendButton = FindViewById<Button>(Resource.Id.buttonVerzenden);
            //Deactivating the button initially and only activates if everything is filled out.

            editOmschrijving.TextChanged += (s, e) =>
            {
                textViewCounter.Text = (editOmschrijving.Text.Length.ToString() + "/500");
                if (editOmschrijving.Text.Length >= 10)
                {
                    sendButton.Enabled = true;
                    sendButton.SetTextColor(Color.Rgb(240, 240, 240));
                }
                else if (editOmschrijving.Text.Length <= 10)
                {
                    sendButton.Enabled = false;
                    sendButton.SetTextColor(Color.Rgb(135, 135, 135));
                }
            };

            sendButton.Click += (s, e) =>
            {
                SendInputtedDataToMailClient();
            };
        }

        protected override void OnStart()
        {
            base.OnStart();
            EditText unitNummer = FindViewById<EditText>(Resource.Id.unitNummer);
            EditText name = FindViewById<EditText>(Resource.Id.naam);
            if(userManager.IsUserLoggedIn)
            {
                name.Text = userManager.Name.ToString();
                unitNummer.Text = userManager.UserunitNummer.ToString();
            }
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if(item.ItemId == Android.Resource.Id.Home)
            {
                this.OnBackPressed();
            }
            return base.OnOptionsItemSelected(item);
        }

        public void SendInputtedDataToMailClient()
        {
            TextView textViewCounter = FindViewById<TextView>(Resource.Id.textViewCounter);
            EditText editOmschrijving = FindViewById<EditText>(Resource.Id.editOmschrijving);
            EditText unitNummer = FindViewById<EditText>(Resource.Id.unitNummer);
            EditText name = FindViewById<EditText>(Resource.Id.naam);
            RadioGroup radioUrgentie = FindViewById<RadioGroup>(Resource.Id.radioGroupUrgentie);
            RadioGroup radioToestemming = FindViewById<RadioGroup>(Resource.Id.radioGroupToestemming);
            RadioButton radioUrgentieSelected = FindViewById<RadioButton>(radioUrgentie.CheckedRadioButtonId);
            RadioButton radioToestemmingSelected = FindViewById<RadioButton>(radioToestemming.CheckedRadioButtonId);
            Button buttonVerzenden = FindViewById<Button>(Resource.Id.buttonVerzenden);

            if (editOmschrijving.Text.Length != 0)
            {
                if (unitNummer.Text.Length != 0)
                {
                    var emailSubj = "Reparatieverzoek, Unit: " + unitNummer.Text + " Urgentie: " + radioUrgentieSelected.Text;
                    var emailBody =
                    "Beste Woco, " + "\n\n" +
                    "Unit nummer: " + unitNummer.Text + "\n\n" +
                    "Probleem omschrijving: " + editOmschrijving.Text + "\n\n" +
                    "Toestemming om de unit te betreden: " + radioToestemmingSelected.Text + "\n\n" +
                    "Groetjes, " + "\n" + name.Text;

                    var emailTemplate =
                    "mailto:woco.test.acc@gmail.com" +
                    "?cc=" +
                    "&subject=" + emailSubj +
                    "&body=" + emailBody;
                    Intent setupEmail = new Intent(Intent.ActionSendto);
                    setupEmail.SetData(Android.Net.Uri.Parse(emailTemplate));
                    StartActivity(setupEmail);
                    Finish();
                }
                else
                {
                    // error voor leeg unitnummer veld
                    Toast.MakeText(this, "Unitnummer moet nog worden ingevuld", ToastLength.Long).Show();
                }
            }
            else
            {
                Toast.MakeText(this, "Alle velden zijn verplicht.", ToastLength.Long).Show();
            }
        }
    }
}