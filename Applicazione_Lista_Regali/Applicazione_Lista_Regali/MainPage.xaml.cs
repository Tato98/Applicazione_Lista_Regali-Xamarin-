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
    public partial class MainPage : ContentPage
    {
        List<ListaRegali> lista = new List<ListaRegali>();
        public ListaRegali ListaRegali
        {
            get { return ListaRegali; }
            set { ListaRegali = value; }
        }

        public String name
        {
            get { return name; }
            set { name = value; }
        }

        public MainPage()
        {
            InitializeComponent();
        }

        private void ButtonAdd_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ListCreationPage(this, GetNameList()));
        }

        public List<String> GetNameList()
        {
            List<String> listaNomi = new List<String>();
            foreach (ListaRegali lr in this.lista)
            {
                listaNomi.Add(lr.nome);
            }
            return listaNomi;
        }
    }
}
