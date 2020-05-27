using System;
using System.Collections.Generic;
using System.Text;

namespace Applicazione_Lista_Regali.Models
{
    public class Regalo
    {
        private string Nome
        {
            get { return Nome; }
            set { Nome = value; }
        }
        private string Prezzo
        {
            get { return Prezzo; }
            set { Prezzo = value; }
        }

        public Regalo(string nome, string prezzo)
        {
            this.Nome = nome;
            this.Prezzo = prezzo;
        }
    }
}
