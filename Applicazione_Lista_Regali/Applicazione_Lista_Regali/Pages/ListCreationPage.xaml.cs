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

//Classe che gestisce la creazione di una lista regali
namespace Applicazione_Lista_Regali
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListCreationPage : ContentPage, SelectedContactsPage.ISendSelectedContact
    {
        List<String> nomi;                                                                  //lista dei nomi già esistenti per la lista regali
        ObservableCollection<Contatti> contatti = new ObservableCollection<Contatti>();     //lista contatti
        ISendData sendData;                                                                 //interfaccia per inviare i contatti scelti

        //Costruttore
        public ListCreationPage(List<String> nomi, ISendData sendData)
        {
            InitializeComponent();
            this.nomi = nomi;
            this.sendData = sendData;
        }

        //Metodo che gestisce l'annullamento della creazione della lista
        private void ButtonCancel_Clicked(object sender, EventArgs e)
        {
            Navigation.PopToRootAsync();
        }

        //Metodo che gestise la creazione della lista
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

                //controlla se il nome inserito esiste già
                if (!nomi.Contains(nome))
                {
                    ListaRegali listaRegali = new ListaRegali(nome, descrizione, value.ToString("0.##") + " €", contatti);
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

        //Metodo che gestisce il click del bottone che permette di navigare verso la pagina di selezione dei contatti dalla rubrica
        private void ButtonContacts_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new SelectedContactsPage(contatti, this));
        }

        //Metodo dell'interfaccia SelectedContactsPage.ISendSelectedContact che riceve i contatti selezionati
        public void ReceiveContacts(List<Contatti> selectedContact)
        {
            foreach (Contatti cnt in selectedContact)
            {
                contatti.Add(cnt);
            }
            contactList.ItemsSource = contatti;
        }

        //interfaccia che permette di inviare la lista regali alla MainPage per il suo inserimento nella lista di 'lista regali'
        public interface ISendData
        {
            void ReceiveData(ListaRegali listaRegali);
        }

        //Metodo che gestisce l'eliminazione di uno dei contatti scelti dopo la fase di selezione dei contatti dalla rubrica
        private void ClearButton_Clicked(object sender, EventArgs e)
        {
            var b = (ImageButton)sender;
            var cnt = (Contatti)b.CommandParameter;
            contatti.Remove(cnt);
        }
    }

}