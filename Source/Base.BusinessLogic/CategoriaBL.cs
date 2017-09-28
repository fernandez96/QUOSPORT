using Base.BusinessEntity;
using Base.BusinessLogic.Interfaces;
using Base.Common.Generics;
using Base.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.BusinessLogic
{
    public class CategoriaBL: Singleton<CategoriaBL>, ICategoriaBL<Categoria, int>
    {
        public int Add(Categoria entity)
        {
            return CategoriaRepository.Instancia.Add(entity);
        }
    }
}
