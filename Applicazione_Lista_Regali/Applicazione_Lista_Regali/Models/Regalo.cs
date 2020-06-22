using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.TizenSpecific;

//Classe che modella i regali
namespace Applicazione_Lista_Regali.Models
{
    public class Regalo
    {
        //_______________________________ Proprietà _______________________________________
        public string Nome { get; set; }
        public string Prezzo { get; set; }
        public string NumeroContatto { get; set; }
        public string ShoppingCart { get; set; }
        public bool NoNComprato { get; set; }
        //_________________________________________________________________________________

        //Costruttore
        public Regalo(string nome, string prezzo, string numeroContatto, bool comprato)
        {
            this.Nome = nome;
            this.Prezzo = prezzo;
            this.NumeroContatto = numeroContatto;
            this.ShoppingCart = "shopping_cart_red.png";
            this.NoNComprato = comprato;
        }
    }
}
