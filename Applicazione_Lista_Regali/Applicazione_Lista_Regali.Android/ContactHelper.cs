using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Provider;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Applicazione_Lista_Regali.Droid;
using Applicazione_Lista_Regali.Models;
using Xamarin.Forms;

[assembly: Dependency(typeof(ContactHelper))]
namespace Applicazione_Lista_Regali.Droid
{
    class ContactHelper : Utilities.IContacts
    {
        public async Task<List<Contatti>> GetDeviceContactsAsync()
        {
            List<Contatti> contactList = new List<Contatti>();
            var uri = ContactsContract.CommonDataKinds.Phone.ContentUri;
            string[] projection =
            {
                ContactsContract.Contacts.InterfaceConsts.Id,
                ContactsContract.Contacts.InterfaceConsts.DisplayName,
                ContactsContract.CommonDataKinds.Phone.Number
            };

            var cursor = Xamarin.Forms.Forms.Context.ContentResolver.Query(uri, projection, null, null, null);
            if (cursor.MoveToFirst())
            {
                do
                {
                    contactList.Add(new Contatti()
                    {
                        Nome = cursor.GetString(cursor.GetColumnIndex(projection[1])),
                        Numero = cursor.GetString(cursor.GetColumnIndex(projection[2]))
                    });
                } while (cursor.MoveToNext());
            }
            return contactList;
        }

        private object ManagedQuery(Android.Net.Uri uri, string[] projection, object p1, object p2, object p3)
        {
            throw new NotImplementedException();
        }
    }
}