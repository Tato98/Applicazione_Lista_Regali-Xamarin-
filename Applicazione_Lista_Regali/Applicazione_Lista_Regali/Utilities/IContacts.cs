using Applicazione_Lista_Regali.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

//Interfaccia implementata dalla classe ContactHelper del progetto Anfroid.
namespace Applicazione_Lista_Regali.Utilities
{
    public interface IContacts
    {
        Task<List<Contatti>> GetDeviceContactsAsync(List<string> contactName);
    }
}
