using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.ActiveDirectory.Intefaces
{
   public interface IUsuarioAD
    {
        bool AutenticarEnDominio(string username, string password);
    }
}
