using System;
using System.Collections.Generic;
using System.Text;

//Classe che modella i regali
namespace Applicazione_Lista_Regali.Models
{
    public class Regalo
    {
        //_______________________________ Proprietà _______________________________________
        public string Nome { get; set; }
        public string Prezzo { get; set; }
        public string NumeroContatto { get; set; }
        //_________________________________________________________________________________

        //Costruttore
        public Regalo(string nome, string prezzo, string numeroContatto)
        {
            this.Nome = nome;
            this.Prezzo = prezzo;
            this.NumeroContatto = numeroContatto;
        }
    }
}
