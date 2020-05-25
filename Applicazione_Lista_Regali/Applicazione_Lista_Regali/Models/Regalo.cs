using System;
using System.Collections.Generic;
using System.Text;

namespace Applicazione_Lista_Regali.Models
{
    public class Regalo
    {
        private String nome
        {
            get { return nome; }
            set { nome = value; }
        }
        private String prezzo
        {
            get { return prezzo; }
            set { prezzo = value; }
        }

        public Regalo(String nome, String prezzo)
        {
            this.nome = nome;
            this.prezzo = prezzo;
        }
    }
}
