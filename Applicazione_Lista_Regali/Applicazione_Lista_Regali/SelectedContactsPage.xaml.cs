using Applicazione_Lista_Regali.Utilities;
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
    public partial class SelectedContactsPage : ContentPage
    {
        public SelectedContactsPage()
        {
            InitializeComponent();
            ShowContact();
        }

        private async void ShowContact()
        {
            var ContactList = await DependencyService.Get<IContacts>().GetDeviceContactsAsync();
            listContact.ItemsSource = ContactList;
        }
    }
}