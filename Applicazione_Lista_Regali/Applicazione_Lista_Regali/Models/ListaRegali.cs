using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;

namespace Applicazione_Lista_Regali.Models
{
    public class ListaRegali : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private string nome;
        private string descrizione;

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
                    OnPropertyChanged("Nome");
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
                    OnPropertyChanged("Descrizione");
                }

            }
        }
        public string Budget { get; set; }
        public ObservableCollection<Contatti> Contatti { get; set; }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public ListaRegali(string nome, string descrizione, string budget, ObservableCollection<Contatti> contatti)
        {
            this.Nome = nome;
            this.Descrizione = descrizione;
            this.Budget = budget;
            this.Contatti = contatti;
        }
    }
}
