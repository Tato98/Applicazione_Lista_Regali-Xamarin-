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
    public partial class ListCreationPage : ContentPage, SelectedContactsPage.SendSelectedContact
    {
        List<String> nomi;
        List<Contatti> contatti = new List<Contatti>();
        SendData sendData;

        public ListCreationPage(List<String> nomi, SendData sendData)
        {
            InitializeComponent();
            this.nomi = nomi;
            this.sendData = sendData;
        }

        private void ButtonCancel_Clicked(object sender, EventArgs e)
        {
            Navigation.PopToRootAsync();
        }

        private void ButtonCreate_Clicked(object sender, EventArgs e)
        {
            String nome, descrizione;
            decimal value;

            //controlli descrizione
            if (descrizioneLista.Text != null)
            {
                descrizione = descrizioneLista.Text;
            }
            else
            {
                descrizione = "Nessuna descrizione";
            }

            //controllo budget
            if (budgetLista.Text != null)
            {
                value = decimal.Parse(budgetLista.Text);
            }
            else
            {
                value = 0;
            }

            //controlli nome lista
            if (nomeLista.Text != null)
            {
                nome = nomeLista.Text;
                if (!nomi.Contains(nome))
                {
                    ListaRegali listaRegali = new ListaRegali(nome, descrizione, value.ToString("0.##"), contatti);
                    sendData.ReceiveData(listaRegali);
                    Navigation.PopToRootAsync();
                }
                else
                {
                    DependencyService.Get<IMessage>().ShortAlert("Il nome della lista è già esistente"); 
                }
            }
            else
            {
                DependencyService.Get<IMessage>().ShortAlert("Inserire il nome della lista"); 
            }
        }

        private void ButtonContacts_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new SelectedContactsPage(contatti, this));
        }

        public void ReceiveContacts(List<Contatti> selectedContact)
        {
            contatti.AddRange(selectedContact);
            contactList.ItemsSource = contatti;
        }

        public interface SendData
        {
            void ReceiveData(ListaRegali listaRegali);
        }
    }
}