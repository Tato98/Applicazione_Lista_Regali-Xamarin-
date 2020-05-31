using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Applicazione_Lista_Regali.Theme
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AppTheme : ResourceDictionary
    {
        public AppTheme()
        {
            InitializeComponent();
        }
    }
}