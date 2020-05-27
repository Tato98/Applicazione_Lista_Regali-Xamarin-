using Applicazione_Lista_Regali.Models;
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
    public partial class ListCreationPage : ContentPage
    {
        MainPage mainPage;
        List<String> nomi;
        List<Contatti> contatti;
        public ListCreationPage(MainPage mainPage, List<String> nomi)
        {
            InitializeComponent();
            this.mainPage = mainPage;
            this.nomi = nomi;
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
                    //mainPage.ListaRegali = new ListaRegali(nome, descrizione, value.ToString("0.##"), new List<Contatti>());
                    //Navigation.PopToRootAsync();
                }
                else
                {
                    //Toast message: "Il nome della lista è già esistente"
                }
            }
            else
            {
                //Toast message: "Inserire il nome della lista"
            }

            //this.mainPage.name = nomeLista.Text;
            //Navigation.PopToRootAsync();
        }

        private void ButtonContacts_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new SelectedContactsPage());
        }
    }
}