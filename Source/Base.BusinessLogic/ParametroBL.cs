using Base.BusinessEntity;
using Base.BusinessLogic.Interfaces;
using Base.Common.Generics;
using Base.DataAccess;

namespace Base.BusinessLogic
{
    public class ParametroBL :Singleton<ParametroBL>, IParametroBL<Parametro, int>
    {
        public int Add(Parametro entity)
        {
            return ParametroRepository.Instancia.Add(entity);
        }
        public int Update(Parametro entity)
        {
            return ParametroRepository.Instancia.Update(entity);
        }
        public Parametro GetAll()
        {
            return ParametroRepository.Instancia.GetAll();
        }

    }
}
