using Applicazione_Lista_Regali.Models;
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

        public GiftListPage(ListaRegali listaRegali)
        {
            InitializeComponent();
            this.listaRegali = listaRegali;
            list.ItemsSource = listaRegali.Contatti;
        }
    }
}