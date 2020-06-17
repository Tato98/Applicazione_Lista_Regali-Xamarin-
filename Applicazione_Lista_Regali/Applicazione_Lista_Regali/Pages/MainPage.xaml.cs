﻿using Applicazione_Lista_Regali.Models;
using Applicazione_Lista_Regali.Pages;
using Applicazione_Lista_Regali.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Applicazione_Lista_Regali
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage, ListCreationPage.ISendData
    {
        ObservableCollection<ListaRegali> lista;

        public MainPage(ObservableCollection<ListaRegali> Lista)
        {
            InitializeComponent();

            lista = Lista;
            list.ItemsSource = lista;
        }

        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null) return;
            Navigation.PushAsync(new GiftListPage((ListaRegali)e.SelectedItem, lista));
            ((ListView)sender).SelectedItem = null;
        }

        private void ButtonAdd_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ListCreationPage(GetNameList(), this));
        }

        public void ReceiveData(ListaRegali listaRegali)
        {
            lista.Add(listaRegali);
            list.ItemsSource = lista;
            //Salvataggio della lista nelle shared preferences
            Preferences.Set("Lista_Regali", JsonConvert.SerializeObject(lista));
        }

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