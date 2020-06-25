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
using Android.Views;
using Android.Widget;
using Applicazione_Lista_Regali.Droid;
using Applicazione_Lista_Regali.Models;
using Applicazione_Lista_Regali.Utilities;
using Xamarin.Essentials;
using Xamarin.Forms;

//Classe che implementa il metodo dell'interfaccia IContacts.
[assembly: Dependency(typeof(ContactHelper))]
namespace Applicazione_Lista_Regali.Droid
{
    class ContactHelper : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity, Utilities.IContacts
    {
        //Il seguente metodo restituisce una lista di contatti presa direttamente dalla rubrica del dispositivo.
        public async Task<List<Contatti>> GetDeviceContactsAsync(List<string> contactName)
        {
            List<Contatti> contactList = new List<Contatti>();
            var status = await Permissions.CheckStatusAsync<Permissions.ContactsRead>();

            //Se il permesso non e garantito...
            if (status != PermissionStatus.Granted)
            {
                //... viene richiesto all'utente di abilitarlo.
                status = await Permissions.RequestAsync<Permissions.ContactsRead>();

                //Se l'utente dovesse rifiutare di abilitarlo...
                if(status == PermissionStatus.Denied)
                {
                    return contactList;     //... il metodo ritorna una lista vuota
                }
                //... altrimenti...
                else
                {
                    return GetAllContacts(contactList, contactName);        //... il metodo ritorna la lista di contatti della rubrica
                } 
            }
            //Se invece il permesso è garantito dall'inizio allora...
            else
            {
                return GetAllContacts(contactList, contactName);        //... il metodo ritorna la lista di contatti della rubrica
            }
        }

        private object ManagedQuery(Android.Net.Uri uri, string[] projection, object p1, object p2, object p3)
        {
            throw new NotImplementedException();
        }

        //Il metodo seguente ritorna la lista dei contatti della rubrica
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

                    //Se il contatto era stato già selezionato...
                    if (contactName.Contains(name))
                    {
                        //... si aggiunge alla lista quel contatto disabilitato
                        contactList.Add(new Contatti()
                        {
                            Nome = name,
                            Numero = number,
                            Regali = new ObservableCollection<Regalo>(),
                            Enable = false,                                 //Contatto disabilitato
                            Selected = false,
                            Visible = false,
                            NumeroRegali = 0,
                            TotPrezzo = "0.00 €"
                        });
                    }
                    //... altrimenti...
                    else
                    {
                        // ... si aggiunge alla lista quel contatto abilitato
                        contactList.Add(new Contatti()
                        {
                            Nome = name,
                            Numero = number,
                            Regali = new ObservableCollection<Regalo>(),
                            Enable = true,                                  //Contatto abilitato
                            Selected = false,
                            Visible = false,
                            NumeroRegali = 0,
                            TotPrezzo = "0.00 €"
                        });
                    }

                } while (cursor.MoveToNext());      //Fintanto che il cursore si muove verso un nuovo contatto della rubrica
            }
            return contactList;
        }
    }
}