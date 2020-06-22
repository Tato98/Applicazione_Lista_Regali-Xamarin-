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
using Applicazione_Lista_Regali.Droid;
using Xamarin.Forms;
using Uri = Android.Net.Uri;

//Classe che implementa il metodo di IWebPage
[assembly: Xamarin.Forms.Dependency(typeof(WebPage))]
namespace Applicazione_Lista_Regali.Droid
{
    public class WebPage : Utilities.IWebPage
    {
        public void Internet(string url)
        {
            //Crea un intent che permette di collegarsi al sito web specificato dall'url indicato
            Intent browserIntent = new Intent(Intent.ActionView);
            browserIntent.SetData(Uri.Parse(url));
            Forms.Context.StartActivity(browserIntent);
        }
    }
}