using System;
using System.IO;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using Android.Graphics;
using Android.Text;

using SQLite;

namespace Project_4
{
    [Activity(Label = "Roteb afsprakenscherm")]
    public class RotebActivity : Activity
    {
        private readonly string dbPath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "project4.sqlite3");

        DatePicker datePicker;
        ListView listnames;
        List<string> itemlist;

        SQLiteConnection db;
        int aantalAfsprakenInSysteem = 0;


        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            db = new SQLiteConnection(dbPath);
            Roteb rotebDb = new Roteb();
            db.CreateTable<Roteb>();

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Roteb);
            ActionBar.SetDisplayHomeAsUpEnabled(true);


            // aanmaken variabelen
            datePicker = FindViewById<DatePicker>(Resource.Id.datePicker);
            listnames = FindViewById<ListView>(Resource.Id.listView1);
            var btnChange2 = FindViewById<Button>(Resource.Id.knop2);
            var btnChange3 = FindViewById<Button>(Resource.Id.deleteAfspraak);

            Button callRoteb = FindViewById<Button>(Resource.Id.dialAppointment);
            
            //maak een array met afspraken en vul de int waarden van maand, dag en jaar in. Later zal hier een afspraak aan kunnen worden toegevoegd

            //afspraak datum momenteel maximaal 1 datum die na knop 2 wordt opgeslagen
            int maand = datePicker.Month + 1;
            int dag = datePicker.DayOfMonth;
            int jaar = datePicker.Year;
            //maak lijst voor 10 afsrpaken (elke datum heeft 3 waarden dag/maand/jaar)
            int[] arrayAfspraken = new int[30];
            itemlist = new List<string>();

            //Here we fill the listview for dates 
            ArrayAdapter<string> fillDates = new ArrayAdapter<string>(this, Resource.Layout.RotebListView, fetchDatesFromDatabase());
            listnames.Adapter = fillDates;

            btnChange3.Click += (s, e) =>
            {
                int verwijderLocatie = (itemlist.Count - 1);
                //Getting the last item in the database
                var kaas = db.Query<Roteb>("SELECT Id, DateOfReservation FROM Roteb ORDER BY Id DESC LIMIT 1");
                int id;
                foreach(var d in kaas)
                {
                    if (d.Id > 0)
                    {
                        id = d.Id;
                        db.Query<Roteb>("DELETE FROM Roteb WHERE Id = " + id);
                    }
                }
                //Getting the new list from the database.
                ArrayAdapter<string> adapter = new ArrayAdapter<string>(this, Resource.Layout.RotebListView, fetchDatesFromDatabase());
                aantalAfsprakenInSysteem = (aantalAfsprakenInSysteem - 3);
                listnames.Adapter = adapter;
            };

            //button 2
            btnChange2.Click += (s, e) =>
             {
                 //zorgen dat knop 1 wordt unlocked:
                 //Knop 2 (btnChange2)zal ervoor zorgen dat de geselecteerde datum in de afsprakenlijst zal worden gezet

                 if (jaar < datePicker.Year)
                 {
                     Toast.MakeText(this, "De afspraak kan niet in het verleden worden geplaatst", ToastLength.Long).Show();
                 }
                 if ((datePicker.Month + 1) < maand)
                 {
                     Toast.MakeText(this, "De afspraak kan niet in het verleden worden geplaatst", ToastLength.Long).Show();
                 }
                 else
                 {
                     //maakt waarden aan voor de ingeplande datum
                     jaar = datePicker.Year;
                     maand = (datePicker.Month + 1);
                     dag = (datePicker.DayOfMonth);
                     
                     int i = aantalAfsprakenInSysteem;
                     // voorkom dat de array out of boundries gaat bij een volledig gevulde lijst 
                     if (i <= 27)
                     {
                         arrayAfspraken[i] = (datePicker.DayOfMonth);
                         arrayAfspraken[i + 1] = (datePicker.Month + 1);
                         arrayAfspraken[i + 2] = datePicker.Year;
                         string dateOfReservationMade = arrayAfspraken[i] + "/" + arrayAfspraken[i + 1] + "/" + arrayAfspraken[i + 2];
                         Toast.MakeText(this, "De afspraak is met succes geplaatst op " + dateOfReservationMade, ToastLength.Long).Show();

                         //  afspraken totaal = new DateTime(jaar, maand, dag);
                         // zet datum om in een leesbare string
                         string addDatum = "Roteb afspraak datum: " + dateOfReservationMade;


                         // maak afspraken in de lijst aan en toon deze op de pagina

                         Roteb dateInsertionData = new Roteb
                         {
                             DateOfReservation = dateOfReservationMade
                         };
                         db.Insert(dateInsertionData);

                         itemlist.Add(addDatum);

                         ArrayAdapter<string> adapter = new ArrayAdapter<string>(this, Resource.Layout.RotebListView, fetchDatesFromDatabase());
                         listnames.Adapter = adapter;
                      //   listnames.SetBackgroundColor(Color.ParseColor("#2e2e2e"));
                     }

                     aantalAfsprakenInSysteem = (aantalAfsprakenInSysteem + 3);
                 }
             };

            callRoteb.Click += (s, e) =>
            {
                Intent startDialer = new Intent(Intent.ActionDial);
                startDialer.SetData(Android.Net.Uri.Parse("tel:14010"));
                StartActivity(startDialer);
            };
        }


        private List<string> fetchDatesFromDatabase()
        {
            var dataFromDb = db.Query<Roteb>("SELECT * FROM Roteb");
            List<string> datesInDb = new List<string>();
            aantalAfsprakenInSysteem = dataFromDb.Count;
            if (dataFromDb.Count > 0)
            {
                //Ok so we have data in the database for dates, put em in a list and return that
                foreach(var data in dataFromDb)
                {
                    datesInDb.Add("Roteb afspraak datum: " + data.DateOfReservation);
                }
            }
            Console.WriteLine("------------------------- " + aantalAfsprakenInSysteem);
            return datesInDb;
        }

        private string getDate()
        {
            StringBuilder strCurrentDate = new StringBuilder();
            int month = datePicker.Month + 1;
            strCurrentDate.Append(datePicker.DayOfMonth + "/" + month  + "/" + datePicker.Year);
            return strCurrentDate.ToString();
        }



        private int[] afspraakScherm(int a, int b, int c)
        {
            int[] datumAfspraak = { a, b, c };
            return datumAfspraak;
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


