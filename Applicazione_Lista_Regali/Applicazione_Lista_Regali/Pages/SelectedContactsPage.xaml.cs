using Applicazione_Lista_Regali.Models;
using Applicazione_Lista_Regali.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Applicazione_Lista_Regali
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SelectedContactsPage : ContentPage
    {
        private List<Contatti> contactList = new List<Contatti>();
        private List<Contatti> selectedContact = new List<Contatti>();
        private SendSelectedContact sendSelected;

        public SelectedContactsPage(List<Contatti> contatti, SendSelectedContact sendSelected)
        {
            InitializeComponent();
            this.sendSelected = sendSelected;
            ShowContact(contatti);
        }

        private async void ShowContact(List<Contatti> contatti)
        {
            var ContactList = await DependencyService.Get<IContacts>().GetDeviceContactsAsync(GetContactsName(contatti));
            listContact.ItemsSource = ContactList;
            contactList = ContactList;
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            foreach(Contatti cnt in contactList)
            {
                if (cnt.Selected)
                {
                    selectedContact.Add(cnt);
                }
            }

            sendSelected.ReceiveContacts(selectedContact);
            Navigation.PopAsync();
        }

        public interface SendSelectedContact
        {
            void ReceiveContacts(List<Contatti> selectedContact);
        }

        private List<string> GetContactsName(List<Contatti> contatti)
        {
            List<string> contactName = new List<string>();
            foreach(Contatti cnt in contatti)
            {
                contactName.Add(cnt.Nome);
            }
            return contactName;
        }
    }
}