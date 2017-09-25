using Base.BusinessEntity;
using Base.BusinessLogic.Interfaces;
using Base.Common.Generics;
using Base.DataAccess;
using System.Collections.Generic;

namespace Base.BusinessLogic
{
  public  class SeguridadBL: Singleton<SeguridadBL>, ISeguridadBL<Formulario>
    {
        public IList<Formulario> GetFormulario()
        {
            return SeguridadRepository.Instancia.GetFormulario();
        }
    }
}
