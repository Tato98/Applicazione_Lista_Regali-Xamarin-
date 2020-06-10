using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Security.Cryptography.X509Certificates;
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
        public int NumeroRegali { get; set; }
        public string TotPrezzo { get; set; }

        public Contatti(string nome, string numero, ObservableCollection<Regalo> regali)
        {
            this.Nome = nome;
            this.Numero = numero;
            this.Regali = regali;
        }

        public Contatti()
        {
        }

        public void SizeGiftList()
        {
            this.NumeroRegali = this.Regali.Count;
        }

        public void TotPrice()
        {
            decimal tot = 0;
            foreach(Regalo r in this.Regali)
            {
                tot += decimal.Parse(GetOnlyDecimal(r.Prezzo));
            }
            this.TotPrezzo = tot.ToString("0.##") + " €";
        }

        public string GetOnlyDecimal(string prezzo)
        {
            return prezzo.Remove(prezzo.Length - 1);
        }
    }
}
