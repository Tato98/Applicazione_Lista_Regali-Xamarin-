using System;
using System.Collections.Generic;
using System.Text;

namespace Applicazione_Lista_Regali.Models
{
    public class ListaRegali
    {
        public string Nome { get; set; }
        public string Descrizione { get; set; }
        public string Budget { get; set; }
        public List<Contatti> Contatti { get; set; }

        public ListaRegali(string nome, string descrizione, string budget, List<Contatti> contatti)
        {
            this.Nome = nome;
            this.Descrizione = descrizione;
            this.Budget = budget;
            this.Contatti = contatti;
        }
    }
}
