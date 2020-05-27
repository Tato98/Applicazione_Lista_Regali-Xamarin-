using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Applicazione_Lista_Regali.Models
{
    public class Contatti
    {
        private string Nome { get; set; }
        private string Numero { get; set; }
        private List<Regalo> Regali { get; set; }

        public Contatti(string nome, string numero, List<Regalo> regali)
        {
            this.Nome = nome;
            this.Numero = numero;
            this.Regali = regali;
        }
    }
}
