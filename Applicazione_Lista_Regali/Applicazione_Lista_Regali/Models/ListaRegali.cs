using System;
using System.Collections.Generic;
using System.Text;

namespace Applicazione_Lista_Regali.Models
{
    class ListaRegali
    {
        public String nome
        {
            get { return nome; }
            set { nome = value; }
        }
        public String descrizione
        {
            get { return descrizione; }
            set { descrizione = value; }
        }
        public String budget
        {
            get { return budget; }
            set { budget = value; }
        }
        public List<Contatti> contatti
        {
            get { return contatti; }
            set { contatti = value; }
        }

        public ListaRegali(String nome, String descrizione, String budget, List<Contatti> contatti)
        {
            this.nome = nome;
            this.descrizione = descrizione;
            this.budget = budget;
            this.contatti = contatti;
        }
    }
}
