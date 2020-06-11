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

namespace Applicazione_Lista_Regali.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GiftListPage : ContentPage
    {
        public ListaRegali listaRegali;
        public ObservableCollection<Contatti> contatti = new ObservableCollection<Contatti>();
        public Contatti cnt = new Contatti();

        public GiftListPage(ListaRegali listaRegali)
        {
            InitializeComponent();
            this.listaRegali = listaRegali;
            contatti = listaRegali.Contatti;
            list.ItemsSource = contatti;
        }

        private async void OnDeleteSwipeItem_Invoked(object sender, EventArgs e)
        {
            bool answer = await DisplayAlert("Attenzione!", "Sei sicuro di voler eliminare questo contatto?", "Si", "No");
            if (answer)
            {
                var s = (SwipeItem)sender;
                var cnt = (Contatti)s.CommandParameter;
                contatti.Remove(cnt);
            }
        }

        private async void AddGiftSwipeItem_Invoked(object sender, EventArgs e)
        {
            popupAddGiftView.IsVisible = true;
            var s = (SwipeItem)sender;
            cnt = (Contatti)s.CommandParameter;
        }

        private void CancelButton_Clicked(object sender, EventArgs e)
        {
            popupAddGiftView.IsVisible = false;
            nomeRegalo.Text = null;
            prezzoRegalo.Text = null;
        }

        private void AddGiftButton_Clicked(object sender, EventArgs e)
        {
            if((nomeRegalo.Text != null && nomeRegalo.Text != "") && (prezzoRegalo.Text != null && prezzoRegalo.Text != ""))
            {
                decimal value = decimal.Parse(prezzoRegalo.Text);
                cnt.Regali.Add(new Regalo(nomeRegalo.Text, value.ToString("0.##"), cnt.Numero));
                cnt.TotPrice();
                cnt.SizeGiftList();
                UpdateContacts(cnt);
                popupAddGiftView.IsVisible = false;
                nomeRegalo.Text = null;
                prezzoRegalo.Text = null;
            }
            else
            {
                DependencyService.Get<IMessage>().ShortAlert("Inserisci tutti i campi richiesti");
            }
        }

        private void ShowGiftListButton_Clicked(object sender, EventArgs e)
        {
            var b = (Button)sender;
            var cnt = (Contatti)b.CommandParameter;

            HideOrShowGiftList(cnt);
        }

        public void HideOrShowGiftList(Contatti cnt)
        {
            if (cnt.Visible)
            {
                cnt.Visible = false;
            }
            else
            {
                cnt.Visible = true;
            }
            UpdateContacts(cnt);
        }

        public void UpdateContacts(Contatti cnt)
        {
            var index = contatti.IndexOf(cnt);
            contatti.Remove(cnt);
            contatti.Insert(index, cnt);
        }

        private async void ModifyGiftButton_Clicked(object sender, EventArgs e)
        {
            var b = (Button)sender;
            var gift = (Regalo)b.CommandParameter;
            var cnt = PickContactByNumber(gift.NumeroContatto);

            string action = await DisplayActionSheet("Seleziona l'elemento da modificare", "Cancella", null, "Nome", "Prezzo");
            if (action.Equals("Nome"))
            {
                string result = await DisplayPromptAsync("Modifica", "Aggiungi un nuovo nome", "Ok", "Annulla", initialValue: gift.Nome, maxLength: 20, keyboard: Keyboard.Text);
                if (result != "" && result != null)
                {
                    gift.Nome = result;
                    UpdateContacts(cnt);
                }
                else if (result == "")
                {
                    DependencyService.Get<IMessage>().ShortAlert("Non hai inserito nessun nome");
                }
            }
            else if (action.Equals("Prezzo"))
            {
                string result = await DisplayPromptAsync("Modifica", "Aggiungi un nuovo prezzo", "Ok", "Annulla", initialValue: GetOnlyDecimal(gift.Prezzo), maxLength: 10, keyboard: Keyboard.Numeric);
                if (result != "" && result != null)
                {
                    decimal value = Decimal.Parse(result);
                    gift.Prezzo = value.ToString("0.##") + " €";
                    cnt.TotPrice();
                    cnt.SizeGiftList();
                    UpdateContacts(cnt);
                }
                else if (result == "")
                {
                    DependencyService.Get<IMessage>().ShortAlert("Non hai inserito nessun prezzo");
                }
            }
        }

        private async void DeleteGiftButton_Clicked(object sender, EventArgs e)
        {
            bool answer = await DisplayAlert("Attenzione!", "Sei sicuro di voler eliminare questo regalo?", "Si", "No");
            if (answer)
            {
                var b = (Button)sender;
                var gift = (Regalo)b.CommandParameter;
                var cnt = PickContactByNumber(gift.NumeroContatto);
                cnt.Regali.Remove(gift);
                cnt.TotPrice();
                cnt.SizeGiftList();
                UpdateContacts(cnt);
            }
        }

        private Contatti PickContactByNumber(string number)
        {
            Contatti contact = new Contatti();
            foreach(Contatti cnt in contatti)
            {
                if(cnt.Numero == number)
                {
                    contact = cnt;
                }
            }
            return contact;
        }

        public string GetOnlyDecimal(string prezzo)
        {
            return prezzo.Remove(prezzo.Length - 2);
        }
    }
}