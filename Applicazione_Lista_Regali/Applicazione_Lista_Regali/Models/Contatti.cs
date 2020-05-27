using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Applicazione_Lista_Regali.Models
{
    public class Contatti
    {
        private string Nome
        {
            get { return Nome; }
            set { Nome = value; }
        }
        private string Numero
        {
            get { return Numero; }
            set { Numero = value; }
        }
        private List<Regalo> Regali
        {
            get { return Regali; }
            set { Regali = value; }
        }

        public Contatti(string nome, string numero, List<Regalo> regali)
        {
            this.Nome = nome;
            this.Numero = numero;
            this.Regali = regali;
        }
    }
}
