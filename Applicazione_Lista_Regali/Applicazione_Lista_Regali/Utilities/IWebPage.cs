using System;
using System.Collections.Generic;
using System.Text;

//Interfaccia che permette di collegarsi alla pagina web specificata.
namespace Applicazione_Lista_Regali.Utilities
{
    public interface IWebPage
    {
        void Internet(string url);
    }
}
