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

        private bool flag1, flag2;

        double? layoutHeight;
        double layoutBoundsHeight;
        int direction;
        const double layoutPropHeightMax = 0.5;
        const double layoutPropHeightMin = 0.08;

        public GiftListPage(ListaRegali listaRegali)
        {
            InitializeComponent();
            flag1 = true;
            flag2 = true;
            contentPage.Title = listaRegali.Nome;
            this.listaRegali = listaRegali;
            contatti = listaRegali.Contatti;
            list.ItemsSource = contatti;
            ControlRemainingBudget(Decimal.Parse(GetOnlyDecimal(listaRegali.Budget)) - Decimal.Parse(GetTotSpent()));
        }

        private async void OnDeleteSwipeItem_Invoked(object sender, EventArgs e)
        {
            bool answer = await DisplayAlert("Attenzione!", "Sei sicuro di voler eliminare questo contatto?", "Si", "No");
            if (answer)
            {
                var s = (SwipeItem)sender;
                var cnt = (Contatti)s.CommandParameter;
                contatti.Remove(cnt);
                ControlRemainingBudget(Decimal.Parse(GetOnlyDecimal(listaRegali.Budget)) - Decimal.Parse(GetTotSpent()));
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
                ControlRemainingBudget(Decimal.Parse(GetOnlyDecimal(listaRegali.Budget)) - Decimal.Parse(GetTotSpent()));
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
                    ControlRemainingBudget(Decimal.Parse(GetOnlyDecimal(listaRegali.Budget)) - Decimal.Parse(GetTotSpent()));
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
                ControlRemainingBudget(Decimal.Parse(GetOnlyDecimal(listaRegali.Budget)) - Decimal.Parse(GetTotSpent()));
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

        private void PanGestureHandler(object sender, PanUpdatedEventArgs e)
        {
            layoutHeight = layoutHeight ?? ((sender as StackLayout).Parent as AbsoluteLayout).Height;
            switch (e.StatusType)
            {
                case GestureStatus.Started:
                    layoutBoundsHeight = AbsoluteLayout.GetLayoutBounds(sender as StackLayout).Height;
                    break;

                case GestureStatus.Running:
                    direction = e.TotalY < 0 ? 1 : -1;
                    //var yProp = layoutBoundsHeight + (-e.TotalY / (double)layoutHeight);
                    //if ((yProp > layoutPropHeightMin) & (yProp < layoutPropHeightMax))
                    //{
                        //AbsoluteLayout.SetLayoutBounds(bottomDrawer, new Rectangle(0.5, 1.00, 0.9, yProp));
                    //}
                    break;

                case GestureStatus.Completed:
                    if (direction > 0)
                    {
                        AbsoluteLayout.SetLayoutBounds(bottomDrawer, new Rectangle(0.00, 1.00, 1.00, layoutPropHeightMax));
                    }
                    else
                    {
                        AbsoluteLayout.SetLayoutBounds(bottomDrawer, new Rectangle(0.00, 1.00, 1.00, layoutPropHeightMin));
                    }
                    break;
            }
        }

        //Metodo che gestisce la modifica del budget una volta cliccato il relativo bottone
        private async void ModifyBudgetButton_Clicked(object sender, EventArgs e)
        {
            string result = await DisplayPromptAsync("Modifica", "Aggiungi un nuovo budget", "Ok", "Annulla", initialValue: GetOnlyDecimal(listaRegali.Budget), maxLength: 10, keyboard: Keyboard.Numeric);
            if (result != "" && result != null)
            {
                decimal value = Decimal.Parse(result);
                listaRegali.Budget = value.ToString("0.##") + " €";
                ControlRemainingBudget(Decimal.Parse(GetOnlyDecimal(listaRegali.Budget)) - Decimal.Parse(GetTotSpent()));
            }
            else if (result == "")
            {
                DependencyService.Get<IMessage>().ShortAlert("Non hai inserito nessun budget");
            }
        }

        //Serve a calcolare il totale dei soldi spesi per la lista
        private string GetTotSpent()
        {
            decimal tot = 0;
            foreach(Contatti cnt in contatti)
            {
                tot += Decimal.Parse(GetOnlyDecimal(cnt.TotPrezzo));
            }
            return tot.ToString("0.##");
        }

        //Mostra degli alert in relazione al totale che si è speso e modificail valore del budget rimasto nel pannello in basso
        private void ControlRemainingBudget(decimal value)
        {
            decimal budget = Decimal.Parse(GetOnlyDecimal(listaRegali.Budget));
            decimal totSpent = Decimal.Parse(GetTotSpent());
            if (flag1 && totSpent > (budget * 50/100) && totSpent <= (budget * 90/100))
            {
                DependencyService.Get<IMessage>().LongAlert("Hai superato la metà del budget");
                flag1 = false;
                flag2 = true;
                textBudget.Text = "Hai ancora a disposizione:";
                budgetRimasto.TextColor = Color.White;
                budgetRimasto.Text = value.ToString("0.##") + " €";
            }
            else if(flag2 && totSpent > (budget * 90/100) && totSpent < budget)
            {
                DependencyService.Get<IMessage>().LongAlert("Hai quasi esaurito il tuo budget");
                flag1 = true;
                flag2 = false;
                textBudget.Text = "Hai ancora a disposizione:";
                budgetRimasto.TextColor = Color.White;
                budgetRimasto.Text = value.ToString("0.##") + " €";
            }
            else if(totSpent > budget)
            {
                DisplayAlert("Attenzione!", "Hai superato il budget a disposizione. Puoi controllare il tuo budget nella sezione in basso.", "OK");
                flag1 = true;
                flag2 = true;
                textBudget.Text = "Hai superato il budget di:";
                budgetRimasto.TextColor = Color.Red;
                budgetRimasto.Text = (-1 * value).ToString("0.##") + " €";
            }
            else
            {
                flag1 = true;
                flag2 = true;
                textBudget.Text = "Hai ancora a disposizione:";
                budgetRimasto.TextColor = Color.White;
                budgetRimasto.Text = value.ToString("0.##") + " €";
            }
        }
    }
}