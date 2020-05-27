using System;
using System.Collections.Generic;
using System.Text;

namespace Applicazione_Lista_Regali.Models
{
    public class ListaRegali
    {
        public string Nome
        {
            get { return Nome; }
            set { Nome = value; }
        }
        public string Descrizione
        {
            get { return Descrizione; }
            set { Descrizione = value; }
        }
        public string Budget
        {
            get { return Budget; }
            set { Budget = value; }
        }
        public List<Contatti> Contatti
        {
            get { return Contatti; }
            set { Contatti = value; }
        }

        public ListaRegali(string nome, string descrizione, string budget, List<Contatti> contatti)
        {
            this.Nome = nome;
            this.Descrizione = descrizione;
            this.Budget = budget;
            this.Contatti = contatti;
        }
    }
}
