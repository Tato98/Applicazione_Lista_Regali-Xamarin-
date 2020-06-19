using System;
using System.Collections.Generic;
using System.Text;

//Interfaccia che permette di creare Toast. Viene implementata dalla classe MassageAndroid nel progetto Android.
namespace Applicazione_Lista_Regali.Utilities
{
    public interface IMessage
    {
        void LongAlert(String message);
        void ShortAlert(String message);
    }
}
