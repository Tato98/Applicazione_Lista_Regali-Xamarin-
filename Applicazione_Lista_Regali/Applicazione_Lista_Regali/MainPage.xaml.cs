using Applicazione_Lista_Regali.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;

namespace Applicazione_Lista_Regali
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage, ListCreationPage.SendData
    {
        List<ListaRegali> lista = new List<ListaRegali>();
        Button button;
        ListView listView;
        StackLayout stackLayout;

        public MainPage()
        {
            InitializeComponent();

            listView = new ListView
            {
                HorizontalOptions = LayoutOptions.StartAndExpand,
                VerticalOptions = LayoutOptions.StartAndExpand
            };

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
            listView.ItemTemplate = new DataTemplate(typeof(TextCell));
            listView.ItemTemplate.SetBinding(TextCell.TextProperty, "Nome");
            listView.ItemTemplate.SetBinding(TextCell.DetailProperty, "Descrizione");
        }
    }
}