using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Applicazione_Lista_Regali.Droid;

//Classe che implementa i metodi dell'interfaccia IMessage
[assembly: Xamarin.Forms.Dependency(typeof(MessageAndroid))]
namespace Applicazione_Lista_Regali.Droid
{
    class MessageAndroid : Utilities.IMessage
    {
        //Genera un LongAlert
        public void LongAlert(string message)
        {
            Toast.MakeText(Android.App.Application.Context, message, ToastLength.Long).Show();
        }

        //Genera uno ShortAlert
        public void ShortAlert(string message)
        {
            Toast.MakeText(Android.App.Application.Context, message, ToastLength.Short).Show();
        }
    }
}