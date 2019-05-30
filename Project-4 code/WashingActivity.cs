using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Webkit;

namespace Project_4
{
    [Activity(Label = "Wassen en Drogen")]
    public class WashingActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Washing);
            ActionBar.SetDisplayHomeAsUpEnabled(true);

            WebView webView = FindViewById<WebView>(Resource.Id.webView1);
            webView.LoadUrl("http://flatalexander.nl/booked/Web/index.php?redirect=%2Fbooked%2FWeb%2Fdashboard.php%3F");
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