using Applicazione_Lista_Regali.Models;
using Applicazione_Lista_Regali.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Applicazione_Lista_Regali
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListCreationPage : ContentPage, SelectedContactsPage.ISendSelectedContact
    {
        List<String> nomi;
        ObservableCollection<Contatti> contatti = new ObservableCollection<Contatti>();
        ISendData sendData;

        public ListCreationPage(List<String> nomi, ISendData sendData)
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
            if (descrizioneLista.Text != null && descrizioneLista.Text != "")
            {
                descrizione = descrizioneLista.Text;
            }
            else
            {
                descrizione = "Nessuna descrizione";
            }

            //controllo budget
            if (budgetLista.Text != null && budgetLista.Text != "")
            {
                value = decimal.Parse(budgetLista.Text);
            }
            else
            {
                value = 0;
            }

            //controlli nome lista
            if (nomeLista.Text != null && nomeLista.Text != "")
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
            foreach(Contatti cnt in selectedContact)
            {
                contatti.Add(cnt);
            }
            contactList.ItemsSource = contatti;
        }

        public interface ISendData
        {
            void ReceiveData(ListaRegali listaRegali);
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            var b = (Button)sender;
            var cnt = (Contatti)b.CommandParameter;
            contatti.Remove(cnt);
        }
    }
}