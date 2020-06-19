using Applicazione_Lista_Regali.Models;
using Applicazione_Lista_Regali.Pages;
using Applicazione_Lista_Regali.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Xamarin.Essentials;
using Xamarin.Forms;

//Classe che gestisce visualizzazione della lista di 'lista regali'
namespace Applicazione_Lista_Regali
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage, ListCreationPage.ISendData
    {
        ObservableCollection<ListaRegali> lista;        //lista di 'lista regali'

        //Costruttore. Riceve dalla classe App la lista caricata dalle shared preferences
        public MainPage(ObservableCollection<ListaRegali> Lista)
        {
            InitializeComponent();

            lista = Lista;
            list.ItemsSource = lista;
        }

        //Metodo che gestisce il click di uno degli elementi della lista
        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null) return;
            Navigation.PushAsync(new GiftListPage((ListaRegali)e.SelectedItem, lista));
            ((ListView)sender).SelectedItem = null;
        }

        //Metodo che gestisce il click del bottone che permette di inserire una nuova lista regali
        private void ButtonAdd_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ListCreationPage(GetNameList(), this));
        }

        //Metodo dell'interfaccia ListCreationPage.ISendData che gestisce il ricevimento della lista regali dopo la sua creazione
        public void ReceiveData(ListaRegali listaRegali)
        {
            lista.Add(listaRegali);
            list.ItemsSource = lista;   //serve ad aggiornare la lista e visualizzare i suoi elementi

            //Salvataggio della lista nelle shared preferences
            Preferences.Set("Lista_Regali", JsonConvert.SerializeObject(lista));
        }

        //Metodo che gestisce il click del bottone che permette di eliminare la lista regali selezionata
        private async void Delete_Clicked(object sender, EventArgs e)
        {
            var mi = (MenuItem)sender;
            var listaRegali = (ListaRegali)mi.CommandParameter;

            bool answer = await DisplayAlert("Attenzione!", "Sei sicuro di voler eliminare questa lista?", "Si", "No");
            if (answer)
            {
                lista.Remove(listaRegali);

                //Salvataggio della lista nelle shared preferences
                Preferences.Set("Lista_Regali", JsonConvert.SerializeObject(lista));
            }
        }

        //Metodo che gestisce il click del bottone che permette di modificare la lista regali selezionata
        private async void Modify_Clicked(object sender, EventArgs e)
        {
            var mi = (MenuItem)sender;
            var listaRegali = (ListaRegali)mi.CommandParameter;

            string action = await DisplayActionSheet("Seleziona l'elemento da modificare", "Cancella", null, "Nome", "Descrizione");
            if (action.Equals("Nome"))
            {
                string result = await DisplayPromptAsync("Modifica", "Aggiungi un nuovo nome", "Ok", "Annulla", initialValue: listaRegali.Nome, maxLength: 20, keyboard: Keyboard.Text);
                if (result != "" && result != null)
                {
                    List<string> nameList = GetNameList();
                    if (!nameList.Contains(result))
                    {
                        listaRegali.Nome = result;

                        //Salvataggio della lista nelle shared preferences
                        Preferences.Set("Lista_Regali", JsonConvert.SerializeObject(lista));
                    }
                    else
                    {
                        DependencyService.Get<IMessage>().ShortAlert("Il nome inserito è già esistente");
                    }
                }
                else if (result == "")
                {
                    DependencyService.Get<IMessage>().ShortAlert("Non hai inserito nessun nome");
                }
            }
            else if (action.Equals("Descrizione"))
            {
                string result = await DisplayPromptAsync("Modifica", "Aggiungi una nuova descrizione", "Ok", "Annulla", initialValue: listaRegali.Descrizione, maxLength: 50, keyboard: Keyboard.Text);
                if (result != "" && result != null)
                {
                    listaRegali.Descrizione = result;

                    //Salvataggio della lista nelle shared preferences
                    Preferences.Set("Lista_Regali", JsonConvert.SerializeObject(lista));
                }
                else if (result == "")
                {
                    DependencyService.Get<IMessage>().ShortAlert("Non hai inserito nessuna descrizione");
                }
            }
        }

        //Metodo che restituisce una lista di stringhe rappresentanti i nomi delle liste regali già esistenti.
        //Il valore viene poi inviato alla ListCreationPage per evitare che venga inserito un nome già esistente.
        public List<String> GetNameList()
        {
            List<String> listaNomi = new List<String>();
            foreach (ListaRegali lr in this.lista)
            {
                listaNomi.Add(lr.Nome);
            }
            return listaNomi;
        }
    }
}