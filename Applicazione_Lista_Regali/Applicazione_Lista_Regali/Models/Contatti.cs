using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Applicazione_Lista_Regali.Models
{
    public class Contatti
    {
        private String nome
        {
            get { return nome; }
            set { nome = value; }
        }
        private String numero
        {
            get { return numero; }
            set { numero = value; }
        }
        private List<Regalo> regali
        {
            get { return regali; }
            set { regali = value; }
        }

        public Contatti(String nome, String numero, List<Regalo> regali)
        {
            this.nome = nome;
            this.numero = numero;
            this.regali = regali;
        }
    }
}
