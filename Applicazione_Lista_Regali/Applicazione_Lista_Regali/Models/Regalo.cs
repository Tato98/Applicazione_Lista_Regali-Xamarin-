using System;
using System.Collections.Generic;
using System.Text;

namespace Applicazione_Lista_Regali.Models
{
    public class Regalo
    {
        public string Nome { get; set; }
        public string Prezzo { get; set; }
        public string NumeroContatto { get; set; }

        public Regalo(string nome, string prezzo, string numeroContatto)
        {
            this.Nome = nome;
            this.Prezzo = prezzo + " €";
            this.NumeroContatto = numeroContatto;
        }
    }
}
