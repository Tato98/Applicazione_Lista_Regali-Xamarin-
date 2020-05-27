using System;
using System.Collections.Generic;
using System.Text;

namespace Applicazione_Lista_Regali.Models
{
    public class Regalo
    {
        private string Nome { get; set; }
        private string Prezzo { get; set; }

        public Regalo(string nome, string prezzo)
        {
            this.Nome = nome;
            this.Prezzo = prezzo;
        }
    }
}
