using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Security.Cryptography.X509Certificates;
using System.Text;

//Classe che modella i contatti
namespace Applicazione_Lista_Regali.Models
{
    public class Contatti
    {
        //________________________________ Proprietà della classe _________________________________________
        public string Nome { get; set; }
        public string Numero { get; set; }
        public ObservableCollection<Regalo> Regali { get; set; }
        public bool Enable { get; set; }
        public bool Selected { get; set; }
        public bool Visible { get; set; }
        public int NumeroRegali { get; set; }
        public string TotPrezzo { get; set; }
        //__________________________________________________________________________________________________

        //______________________________________ Costruttori _______________________________________________
        public Contatti(string nome, string numero, ObservableCollection<Regalo> regali)
        {
            this.Nome = nome;
            this.Numero = numero;
            this.Regali = regali;
        }

        public Contatti()
        {
        }
        //__________________________________________________________________________________________________

        //Metodo che assegna alla proprietà NumeroRegali il numero di elementi della lista
        public void SizeGiftList()
        {
            this.NumeroRegali = this.Regali.Count;
        }

        //Metodo che calcola e restituisce il prezzo totale della lista regali
        public void TotPrice()
        {
            decimal tot = 0;
            foreach(Regalo r in this.Regali)
            {
                tot += decimal.Parse(GetOnlyDecimal(r.Prezzo));
            }
            this.TotPrezzo = tot.ToString("0.##") + " €";
        }

        //Metodo che prende in ingresso una stringa e restituisce la stessa stringa meno gli ultimi due caratteri.
        //Viene utilizzata per rimuovere i caratteri ' €' dal prezzo passato in ingresso quando necessario.
        public string GetOnlyDecimal(string prezzo)
        {
            return prezzo.Remove(prezzo.Length - 1);
        }
    }
}
