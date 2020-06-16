using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Provider;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Support.V4.Content;
using Android.Views;
using Android.Widget;
using Applicazione_Lista_Regali.Droid;
using Applicazione_Lista_Regali.Models;
using Applicazione_Lista_Regali.Utilities;
using Xamarin.Essentials;
using Xamarin.Forms;

[assembly: Dependency(typeof(ContactHelper))]
namespace Applicazione_Lista_Regali.Droid
{
    class ContactHelper : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity, Utilities.IContacts
    {
        public async Task<List<Contatti>> GetDeviceContactsAsync(List<string> contactName)
        {
            List<Contatti> contactList = new List<Contatti>();
            var status = await Permissions.CheckStatusAsync<Permissions.ContactsRead>();

            if (status != PermissionStatus.Granted)
            {
                status = await Permissions.RequestAsync<Permissions.ContactsRead>();

                if(status == PermissionStatus.Denied)
                {
                    return contactList;
                }
                else
                {
                    return GetAllContacts(contactList, contactName);
                } 
            }
            else
            {
                return GetAllContacts(contactList, contactName);
            }
        }

        private object ManagedQuery(Android.Net.Uri uri, string[] projection, object p1, object p2, object p3)
        {
            throw new NotImplementedException();
        }

        private List<Contatti> GetAllContacts(List<Contatti> contactList, List<string> contactName)
        {
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
                    string name = cursor.GetString(cursor.GetColumnIndex(projection[1]));
                    string number = cursor.GetString(cursor.GetColumnIndex(projection[2]));

                    if (contactName.Contains(name))
                    {
                        contactList.Add(new Contatti()
                        {
                            Nome = name,
                            Numero = number,
                            Regali = new ObservableCollection<Regalo>(),
                            Enable = false,
                            Selected = false,
                            Visible = false,
                            NumeroRegali = 0,
                            TotPrezzo = "0.00 €"
                        });
                    }
                    else
                    {
                        contactList.Add(new Contatti()
                        {
                            Nome = name,
                            Numero = number,
                            Regali = new ObservableCollection<Regalo>(),
                            Enable = true,
                            Selected = false,
                            Visible = false,
                            NumeroRegali = 0,
                            TotPrezzo = "0.00 €"
                        });
                    }

                } while (cursor.MoveToNext());
            }
            return contactList;
        }
    }
}