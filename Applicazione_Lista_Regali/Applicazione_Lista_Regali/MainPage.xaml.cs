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
        public ListaRegali ListaRegali
        {
            get { return ListaRegali; }
            set { ListaRegali = value; }
        }

        public MainPage(ListaRegali listaRegali)
        {
            InitializeComponent();
        }

        private void ButtonAdd_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ListCreationPage(this, GetNameList(), this));
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
            label.Text = listaRegali.Nome;
        }
    }
}
