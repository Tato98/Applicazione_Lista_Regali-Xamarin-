using Applicazione_Lista_Regali.Models;
using Applicazione_Lista_Regali.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

//Classe che permette la gestione della lista di contatti tramite l'aggiunta o la rimozione di elementi
// o di regali per ogni elemento, e che permette la visualizzazione e la gestione del budget di ogni lista. 
namespace Applicazione_Lista_Regali.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GiftListPage : ContentPage, SelectedContactsPage.ISendSelectedContact
    {
        public ListaRegali listaRegali;                                                             //rappresenta la lista regali selezionata
        public ObservableCollection<Contatti> contatti = new ObservableCollection<Contatti>();      //lista di contatti della lista regali
        public ObservableCollection<ListaRegali> lista;                                             //lista di 'lista regali' usata per salvare le modifiche al momento dello spegnimento dell'app
        public Contatti cnt = new Contatti();                                                       //contatto utilizzato nel metodo di aggiunta di un regalo

        private bool flag1, flag2;                                                                  //variabili booleane utilizzate nel metodo di controllo del budget


        //______________ Variabili utilizzate nel metodo che gestisce l'apertura del pannello di controllo del budget ______________
        
        double? layoutHeight;                           //definisce l'altezza corrente del layout
        double layoutBoundsHeight;                      //definisce i limiti del layout
        int direction;                                  //definisce la direzione verso la quale si trascina il pannello (alto o basso)
        const double layoutPropHeightMax = 0.5;         //definisce l'altezza massima alla quale arriva il layout (apertura)
        const double layoutPropHeightMin = 0.08;        //definisce l'altezza minima alla quale arriva il layout (chiusura)
        //__________________________________________________________________________________________________________________________


        //Costruttore della classe dove si inizializzano alcune delle variabili definite sopra.
        public GiftListPage(ListaRegali listaRegali, ObservableCollection<ListaRegali> lista)
        {
            InitializeComponent();

            flag1 = true;
            flag2 = true;
            this.lista = lista;
            contentPage.Title = listaRegali.Nome;
            this.listaRegali = listaRegali;
            contatti = listaRegali.Contatti;
            list.ItemsSource = contatti;

            //Esegue un controllo sul budget rimanente all'apertura della lista al fine di notificare lo stato del budget attuale all'utente
            ControlRemainingBudget(Decimal.Parse(GetOnlyDecimal(listaRegali.Budget)) - Decimal.Parse(GetTotSpent()));
        }

        //Metodo che gestisce l'eliminazione di un contatto dalla lista
        private async void OnDeleteSwipeItem_Invoked(object sender, EventArgs e)
        {
            bool answer = await DisplayAlert("Attenzione!", "Sei sicuro di voler eliminare questo contatto?", "Si", "No");
            if (answer)
            {
                var s = (SwipeItem)sender;
                var cnt = (Contatti)s.CommandParameter;
                contatti.Remove(cnt);

                //Effettua un controllo sul budget per aggiornare il suo status dopo l'eliminazione di un contatto
                ControlRemainingBudget(Decimal.Parse(GetOnlyDecimal(listaRegali.Budget)) - Decimal.Parse(GetTotSpent()));
                
                //Salvataggio della lista nelle shared preferences
                Preferences.Set("Lista_Regali", JsonConvert.SerializeObject(lista));
            }
        }

        //Metodo che gestisce l'aggiunta dei regali ai contatti
        private async void AddGiftSwipeItem_Invoked(object sender, EventArgs e)
        {
            popupAddGiftView.IsVisible = true;      //Rende visibile il popup per l'aggiunta del regalo
            var s = (SwipeItem)sender;
            cnt = (Contatti)s.CommandParameter;     //Si inizializza la variabile cnt della classe con il contatto al quale si vuole aggiungere il regalo
        }

        //Gestisce il click del bottone del popup che annulla l'operazione di aggiunta del regalo
        private void CancelButton_Clicked(object sender, EventArgs e)
        {
            popupAddGiftView.IsVisible = false;     //Imposta a false la visibilità del popoup
            nomeRegalo.Text = null;                 //Svuota il campo nome regalo
            prezzoRegalo.Text = null;               //Svuota il campo prezzo regalo
        }

        //Gestisce il click del bottone del popup che permette l'aggiunta del regalo
        private void AddGiftButton_Clicked(object sender, EventArgs e)
        {
            //Se tutti i campi sono riempiti esegue l'aggiunta...
            if((nomeRegalo.Text != null && nomeRegalo.Text != "") && (prezzoRegalo.Text != null && prezzoRegalo.Text != ""))
            {
                //Si aggiunge il regalo
                decimal value = decimal.Parse(prezzoRegalo.Text);
                cnt.Regali.Add(new Regalo(nomeRegalo.Text, value.ToString("0.##") + " €", cnt.Numero));

                //Si effettuano controlli sul budget e sul numero totale di regali e del loro prezzo per il contatto di interesse
                cnt.TotPrice();
                cnt.SizeGiftList();
                UpdateContacts(cnt);
                ControlRemainingBudget(Decimal.Parse(GetOnlyDecimal(listaRegali.Budget)) - Decimal.Parse(GetTotSpent()));

                //Si riporta la visibilità del popup a false
                popupAddGiftView.IsVisible = false;
                nomeRegalo.Text = null;
                prezzoRegalo.Text = null;

                //Salvataggio della lista nelle shared preferences
                Preferences.Set("Lista_Regali", JsonConvert.SerializeObject(lista));
            }
            //... altrimenti crea un toast
            else
            {
                DependencyService.Get<IMessage>().ShortAlert("Inserisci tutti i campi richiesti");
            }
        }

        //Metodo che gestisce la visibilità della lista regali per ogni contatto
        private void ShowGiftListButton_Clicked(object sender, EventArgs e)
        {
            var b = (ImageButton)sender;
            var cnt = (Contatti)b.CommandParameter;

            HideOrShowGiftList(cnt);        //Richiama il metodo che mostra o nasconde la lista dei regali
        }

        //Metodo che mostra o nasconde la lista dei regali di un contatto impostando la sua visibilità
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
            UpdateContacts(cnt);        //Effettua un aggiornamento del contatto
        }

        //Effettua un aggiornamento del contatto rimuovendo e successivamente inserendo il contatto nella stessa posizione.
        //In questo modo sarà possibile visualizzare subito le modifiche apportate al contatto
        public void UpdateContacts(Contatti cnt)
        {
            var index = contatti.IndexOf(cnt);
            contatti.Remove(cnt);
            contatti.Insert(index, cnt);
        }

        //Metodo che gestisce la modifica del regalo
        private async void ModifyGiftButton_Clicked(object sender, EventArgs e)
        {
            var b = (ImageButton)sender;
            var gift = (Regalo)b.CommandParameter;
            var cnt = PickContactByNumber(gift.NumeroContatto);         //permette di recuperare il contatto che contiene il regalo che si sta modificando

            string action = await DisplayActionSheet("Seleziona l'elemento da modificare", "Cancella", null, "Nome", "Prezzo");
            if (action.Equals("Nome"))
            {
                string result = await DisplayPromptAsync("Modifica", "Aggiungi un nuovo nome", "Ok", "Annulla", initialValue: gift.Nome, maxLength: 20, keyboard: Keyboard.Text);
                if (result != "" && result != null)
                {
                    gift.Nome = result;
                    UpdateContacts(cnt);        //aggiornamento

                    //Salvataggio della lista nelle shared preferences
                    Preferences.Set("Lista_Regali", JsonConvert.SerializeObject(lista));
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

                    //Aggiornamento
                    cnt.TotPrice();
                    cnt.SizeGiftList();
                    UpdateContacts(cnt);
                    ControlRemainingBudget(Decimal.Parse(GetOnlyDecimal(listaRegali.Budget)) - Decimal.Parse(GetTotSpent()));
                    
                    //Salvataggio della lista nelle shared preferences
                    Preferences.Set("Lista_Regali", JsonConvert.SerializeObject(lista));
                }
                else if (result == "")
                {
                    DependencyService.Get<IMessage>().ShortAlert("Non hai inserito nessun prezzo");
                }
            }
        }

        //Gestisce l'eliminazione di un regalo
        private async void DeleteGiftButton_Clicked(object sender, EventArgs e)
        {
            bool answer = await DisplayAlert("Attenzione!", "Sei sicuro di voler eliminare questo regalo?", "Si", "No");
            if (answer)
            {
                var b = (ImageButton)sender;
                var gift = (Regalo)b.CommandParameter;
                var cnt = PickContactByNumber(gift.NumeroContatto);

                cnt.Regali.Remove(gift);

                //Aggiornamento
                cnt.TotPrice();
                cnt.SizeGiftList();
                UpdateContacts(cnt);
                ControlRemainingBudget(Decimal.Parse(GetOnlyDecimal(listaRegali.Budget)) - Decimal.Parse(GetTotSpent()));
                
                //Salvataggio della lista nelle shared preferences
                Preferences.Set("Lista_Regali", JsonConvert.SerializeObject(lista));
            }
        }

        //Metodo che permette di recuperare il contatto dal suo numero
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

        //Metodo che prende in ingresso una stringa e che restituisce la stessa meno gli ultimi tre caratteri.
        public string GetOnlyDecimal(string prezzo)
        {
            return prezzo.Remove(prezzo.Length - 2);
        }

        //Metodo che gestisce l'apertura e la chiusura del pannello di controllo del budget
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
                    var yProp = layoutBoundsHeight + (-e.TotalY / (double)layoutHeight);
                    if ((yProp > layoutPropHeightMin) & (yProp < layoutPropHeightMax))
                    {
                        AbsoluteLayout.SetLayoutBounds(bottomDrawer, new Rectangle(0.00, 1.00, 1.00, yProp));
                    }
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
                
                //Salvataggio della lista nelle shared preferences
                Preferences.Set("Lista_Regali", JsonConvert.SerializeObject(lista));
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

        //Metodo che permette la gestione del click del bottone che apre la lista selezione contatti della rubrica
        private void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new SelectedContactsPage(contatti, this));
        }

        //Mostra degli alert in relazione al totale che si è speso e modifica il valore del budget rimasto nel pannello in basso
        private void ControlRemainingBudget(decimal value)
        {
            decimal budget = Decimal.Parse(GetOnlyDecimal(listaRegali.Budget));
            decimal totSpent = Decimal.Parse(GetTotSpent());

            //Se il totale speso è compreso tra la metà del budget e il suo 90% allora stampa un alert di notifica.
            //Il flag serve a evitare che l'alert sia stampato ogni volta che il totale speso si trova in questo range.
            if (flag1 && totSpent > (budget * 50/100) && totSpent <= (budget * 90/100))
            {
                DependencyService.Get<IMessage>().LongAlert("Hai superato la metà del budget");
                flag1 = false;
                flag2 = true;
                textBudget.Text = "Hai ancora a disposizione:";
                budgetRimasto.TextColor = Color.White;
                budgetRimasto.Text = value.ToString("0.##") + " €";
            }
            //Caso in cui il totale speso supera il 90% del budget ma si mantiene comunque al di sotto di esso...
            else if(flag2 && totSpent > (budget * 90/100) && totSpent < budget)
            {
                DependencyService.Get<IMessage>().LongAlert("Hai quasi esaurito il tuo budget");
                flag1 = true;
                flag2 = false;
                textBudget.Text = "Hai ancora a disposizione:";
                budgetRimasto.TextColor = Color.White;
                budgetRimasto.Text = value.ToString("0.##") + " €";
            }
            //Caso in cui si supera il budget a disposizione...
            else if(totSpent > budget)
            {
                DisplayAlert("Attenzione!", "Hai superato il budget a disposizione. Puoi controllare il tuo budget nella sezione in basso.", "OK");
                flag1 = true;
                flag2 = true;
                textBudget.Text = "Hai superato il budget di:";
                budgetRimasto.TextColor = Color.Red;
                budgetRimasto.Text = (-1 * value).ToString("0.##") + " €";
            }
            //Caso in cui il totale speso è minore della metà del budget
            else
            {
                flag1 = true;
                flag2 = true;
                textBudget.Text = "Hai ancora a disposizione:";
                budgetRimasto.TextColor = Color.White;
                budgetRimasto.Text = value.ToString("0.##") + " €";
            }
        }

        //Metodo dell'interfaccia SelectedContactsPage.ISendSelectedContact che riceve i contatti selezionati dalla pagina di selezione dei contatti
        public void ReceiveContacts(List<Contatti> selectedContact)
        {
            foreach (Contatti cnt in selectedContact)
            {
                contatti.Add(cnt);

                //Salvataggio della lista nelle shared preferences
                Preferences.Set("Lista_Regali", JsonConvert.SerializeObject(lista));
            }
        }
    }
}