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
            popupLoginView.IsVisible = true;
            var s = (SwipeItem)sender;
            cnt = (Contatti)s.CommandParameter;
        }

        private void CancelButton_Clicked(object sender, EventArgs e)
        {
            popupLoginView.IsVisible = false;
            nomeRegalo.Text = null;
            prezzoRegalo.Text = null;
        }

        private void AddGiftButton_Clicked(object sender, EventArgs e)
        {
            if((nomeRegalo.Text != null && nomeRegalo.Text != "") && (prezzoRegalo.Text != null && prezzoRegalo.Text != ""))
            {
                cnt.Regali.Add(new Regalo(nomeRegalo.Text, prezzoRegalo.Text));
                popupLoginView.IsVisible = false;
                nomeRegalo.Text = null;
                prezzoRegalo.Text = null;
            }
            else
            {
                DependencyService.Get<IMessage>().ShortAlert("Inserisci tutti i campi richiesti");
            }
        }
    }
}