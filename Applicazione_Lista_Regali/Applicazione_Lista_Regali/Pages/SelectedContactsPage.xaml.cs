using Applicazione_Lista_Regali.Models;
using Applicazione_Lista_Regali.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Applicazione_Lista_Regali
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SelectedContactsPage : ContentPage
    {
        private List<Contatti> contactList = new List<Contatti>();
        private List<Contatti> selectedContact = new List<Contatti>();
        private ISendSelectedContact sendSelected;

        public SelectedContactsPage(ObservableCollection<Contatti> contatti, ISendSelectedContact sendSelected)
        {
            InitializeComponent();
            this.sendSelected = sendSelected;
            ShowContact(contatti);
        }

        private async void ShowContact(ObservableCollection<Contatti> contatti)
        {
            var ContactList = await DependencyService.Get<IContacts>().GetDeviceContactsAsync(GetContactsName(contatti));
            var status = await Permissions.CheckStatusAsync<Permissions.ContactsRead>();
            if (status == PermissionStatus.Denied)
            {
                DisplayAlert("Attenzione!", "Per poter usufruire a pieno delle funzionalità di quest'app assicurati che i permessi richiesti siano garantiti.", "OK");
                Navigation.PopAsync();
            }
            else if(ContactList.Count == 0 && status == PermissionStatus.Granted)
            {
                DisplayAlert("Nessun contatto", "Non è stato trovato nessun contatto nella rubrica di questo dispositivo.", "OK");
                Navigation.PopAsync();
            }
            else if(ContactList.Count > 0)
            {
                listContact.ItemsSource = ContactList;
                contactList = ContactList;
            }
        }

        public interface ISendSelectedContact
        {
            void ReceiveContacts(List<Contatti> selectedContact);
        }

        private List<string> GetContactsName(ObservableCollection<Contatti> contatti)
        {
            List<string> contactName = new List<string>();
            foreach(Contatti cnt in contatti)
            {
                contactName.Add(cnt.Nome);
            }
            return contactName;
        }

        private void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            foreach (Contatti cnt in contactList)
            {
                if (cnt.Selected)
                {
                    selectedContact.Add(cnt);
                }
            }

            sendSelected.ReceiveContacts(selectedContact);
            Navigation.PopAsync();
        }
    }
}