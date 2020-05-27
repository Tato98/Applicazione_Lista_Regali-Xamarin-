using System;
using System.Collections.Generic;
using System.Text;

namespace Applicazione_Lista_Regali.Utilities
{
    public interface IMessage
    {
        void LongAlert(String message);
        void ShortAlert(String message);
    }
}
