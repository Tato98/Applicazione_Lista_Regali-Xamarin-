using Applicazione_Lista_Regali.Cell;
using Applicazione_Lista_Regali.Models;
using Applicazione_Lista_Regali.Pages;
using Applicazione_Lista_Regali.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Xamarin.Forms;

namespace Applicazione_Lista_Regali
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage, ListCreationPage.ISendData, GiftListCell.IMenuItem
    {
        ObservableCollection<ListaRegali> lista = new ObservableCollection<ListaRegali>();
        ImageButton button;
        ListView listView;
        StackLayout stackLayout;

        public MainPage()
        {
            InitializeComponent();

            listView = new ListView
            {
                HorizontalOptions = LayoutOptions.StartAndExpand,
                VerticalOptions = LayoutOptions.StartAndExpand,
                SeparatorVisibility = SeparatorVisibility.Default,
                SeparatorColor = Color.Black
            };
            listView.ItemSelected += ListView_ItemSelected;

            button = new ImageButton
            {
                Padding = 15,
                Source = "plus.png",
                WidthRequest = 90,
                HeightRequest = 90,
                BackgroundColor = Color.Transparent,
                HorizontalOptions = LayoutOptions.EndAndExpand,
                VerticalOptions = LayoutOptions.EndAndExpand
            };
            button.Clicked += ButtonAdd_Clicked;

            stackLayout = new StackLayout
            {
                Children =
                {
                    listView,
                    button
                }
            };
            this.Content = stackLayout;
        }

        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null) return;
            Navigation.PushAsync(new GiftListPage((ListaRegali)e.SelectedItem));
            ((ListView)sender).SelectedItem = null;
        }

        private void ButtonAdd_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ListCreationPage(GetNameList(), this));
        }

        public List<String> GetNameList()
        {
            List<String> listaNomi = new List<String>();
            foreach (ListaRegali lr in this.lista)
            {
                listaNomi.Add(lr.Nome);
            }
            return listaNomi;
        }

        public void ReceiveData(ListaRegali listaRegali)
        {
            lista.Add(listaRegali);
            listView.ItemsSource = lista;
            listView.RowHeight = 100;
            var cell = new DataTemplate(() => { return new GiftListCell(this); });
            listView.ItemTemplate = cell;
        }

        public async void RemoveItem(ListaRegali listaRegali)
        {
            bool answer = await DisplayAlert("Attenzione!", "Sei sicuro di voler eliminare questa lista?", "Si", "No");
            if (answer)
            {
                lista.Remove(listaRegali);
            }
        }

        public async void ModifyItem(ListaRegali listaRegali)
        {
            string action = await DisplayActionSheet("Seleziona l'elemento da modificare", "Cancella", null, "Nome", "Descrizione");
            if (action.Equals("Nome"))
            {
                string result = await DisplayPromptAsync("Modifica", "Aggiungi un nuovo nome", "Ok", "Annulla",initialValue: listaRegali.Nome, maxLength: 20, keyboard: Keyboard.Text);
                if (result != "" && result != null)
                {
                    List<string> nameList = GetNameList();
                    if (!nameList.Contains(result))
                    {
                        listaRegali.Nome = result;
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
                }
                else if (result == "")
                {
                    DependencyService.Get<IMessage>().ShortAlert("Non hai inserito nessuna descrizione");
                }
            }
        }
    }
}