using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;

//Classe che modella le liste regali
namespace Applicazione_Lista_Regali.Models
{
    public class ListaRegali : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;   //evento utilizzato per aggiornare la lista delle modifiche fatte
        private string nome;                                        //nome lista
        private string descrizione;                                 //descrizione lista
        private string budget;                                      //budget lista

        //_____________________________________ Proprietà _________________________________________________
        public string Nome
        {
            get
            {
                return nome;
            }
            set
            {
                if(!value.Equals(nome, StringComparison.Ordinal))
                {
                    nome = value;
                    OnPropertyChanged("Nome"); //Richiama il metodo qualora si modificasse il valore della proprietà
                }
                
            }
        }

        public string Descrizione
        {
            get
            {
                return descrizione;
            }
            set
            {
                if (!value.Equals(descrizione, StringComparison.Ordinal))
                {
                    descrizione = value;
                    OnPropertyChanged("Descrizione"); //Richiama il metodo qualora si modificasse il valore della proprietà
                }

            }
        }
        public string Budget
        {
            get
            {
                return budget;
            }
            set
            {
                if(!value.Equals(budget, StringComparison.Ordinal))
                {
                    budget = value;
                    OnPropertyChanged("Budget"); //Richiama il metodo qualora si modificasse il valore della proprietà
                }
            }
        }
        public ObservableCollection<Contatti> Contatti { get; set; }
        //________________________________________________________________________________________________________________

        //Metodo che permette di aggiornare la proprietà della classe qualora fosse stata modificata
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        //__________________________________________ Costruttore _________________________________________________________
        public ListaRegali(string nome, string descrizione, string budget, ObservableCollection<Contatti> contatti)
        {
            this.Nome = nome;
            this.Descrizione = descrizione;
            this.Budget = budget;
            this.Contatti = contatti;
        }
        //________________________________________________________________________________________________________________
    }
}
