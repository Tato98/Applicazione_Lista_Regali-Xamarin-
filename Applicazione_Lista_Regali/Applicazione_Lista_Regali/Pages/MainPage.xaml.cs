﻿using Applicazione_Lista_Regali.Cell;
using Applicazione_Lista_Regali.Models;
using Applicazione_Lista_Regali.Pages;
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
        Button button;
        ListView listView;
        StackLayout stackLayout;

        public MainPage()
        {
            InitializeComponent();

            listView = new ListView
            {
                HorizontalOptions = LayoutOptions.StartAndExpand,
                VerticalOptions = LayoutOptions.StartAndExpand,
            };
            listView.ItemSelected += ListView_ItemSelected;

            button = new Button
            {
                Text = "Premi",
                HorizontalOptions = LayoutOptions.End,
                VerticalOptions = LayoutOptions.End
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
            Navigation.PushAsync(new GiftListPage((ListaRegali)e.SelectedItem));
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
            if(answer)
            {
                lista.Remove(listaRegali);
            }     
        }

        public async void ModifyItem(ListaRegali listaRegali)
        {
            string result = await DisplayPromptAsync("Modifica", "Aggiungi un nome");
            listaRegali.Nome = result;
        }
    }
}