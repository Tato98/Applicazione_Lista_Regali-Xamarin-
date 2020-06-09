using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;

namespace Applicazione_Lista_Regali.Models
{
    public class Contatti
    {
        public string Nome { get; set; }
        public string Numero { get; set; }
        public ObservableCollection<Regalo> Regali { get; set; }
        public bool Enable { get; set; }
        public bool Selected { get; set; }
        public bool Visible { get; set; }

        public Contatti(string nome, string numero, ObservableCollection<Regalo> regali)
        {
            this.Nome = nome;
            this.Numero = numero;
            this.Regali = regali;
        }

        public Contatti()
        {
        }
    }
}
