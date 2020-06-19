using Applicazione_Lista_Regali.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Applicazione_Lista_Regali
{
    public partial class App : Application
    {
        private ObservableCollection<ListaRegali> Lista;

        public App()
        {
            InitializeComponent();

            //Prova a inizializzare 'Lista' con il valore salvato nelle shared preferences...
            try
            {
                Lista = JsonConvert.DeserializeObject<ObservableCollection<ListaRegali>>(Preferences.Get("Lista_Regali", "defaultValue"));
            }
            //... se prende l'eccezione seguente inizializza 'Lista' con una lista vuota.
            catch (JsonReaderException jre)
            {
                Lista = new ObservableCollection<ListaRegali>();
            }
            
            //Imposta MainPage come la pagina radice e gli passa il valore di 'Lista'
            MainPage = new NavigationPage(new MainPage(Lista));
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
