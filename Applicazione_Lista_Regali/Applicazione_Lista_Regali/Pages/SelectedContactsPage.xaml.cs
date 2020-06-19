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

//Classe che gestisce la selezione dei contatti dalla rubrica del dispositivo
namespace Applicazione_Lista_Regali
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SelectedContactsPage : ContentPage
    {
        private List<Contatti> contactList = new List<Contatti>();          //lista dei contatti della rubrica
        private List<Contatti> selectedContact = new List<Contatti>();      //lista dei contatti selezionati dalla rubrica
        private ISendSelectedContact sendSelected;                          //interfaccia per l'invio dei contatti scelti

        //Costruttore
        public SelectedContactsPage(ObservableCollection<Contatti> contatti, ISendSelectedContact sendSelected)
        {
            InitializeComponent();
            this.sendSelected = sendSelected;
            ShowContact(contatti);
        }

        //Metodo che richiama l'interfaccia IContacts che restituisce tramite il suo metodo i contatti della rubrica
        private async void ShowContact(ObservableCollection<Contatti> contatti)
        {
            var ContactList = await DependencyService.Get<IContacts>().GetDeviceContactsAsync(GetContactsName(contatti));

            var status = await Permissions.CheckStatusAsync<Permissions.ContactsRead>();
            //Se il permesso è stato negato viene visualizzato un alert che notifica l'utente e gli suggerisce di abilitare i permessi
            if (status == PermissionStatus.Denied)
            {
                DisplayAlert("Attenzione!", "Per poter usufruire a pieno delle funzionalità di quest'app assicurati che i permessi richiesti siano garantiti.", "OK");
                Navigation.PopAsync();
            }
            //Se invece il permesso è stato garantito ma la lista è vuota viene notificato all'utente che la rubrica è vuota
            else if(ContactList.Count == 0 && status == PermissionStatus.Granted)
            {
                DisplayAlert("Nessun contatto", "Non è stato trovato nessun contatto nella rubrica di questo dispositivo.", "OK");
                Navigation.PopAsync();
            }
            //Altrimenti se la lista non è vuota la si assegna alla ListView
            else if(ContactList.Count > 0)
            {
                listContact.ItemsSource = ContactList;
                contactList = ContactList;
            }
        }

        //Interfaccia per l'invio dei contatti selezionati
        public interface ISendSelectedContact
        {
            void ReceiveContacts(List<Contatti> selectedContact);
        }

        //Matodo che restituisce la lista dei nomi dei contatti selezionati. Viene passata al metodo dell'interfaccia IContacts
        //al fine di disabilitare i contatti già selezionati
        private List<string> GetContactsName(ObservableCollection<Contatti> contatti)
        {
            List<string> contactName = new List<string>();
            foreach(Contatti cnt in contatti)
            {
                contactName.Add(cnt.Nome);
            }
            return contactName;
        }

        //Metodo che gestisce il click del bottone per l'invio dei contatti selezionati
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