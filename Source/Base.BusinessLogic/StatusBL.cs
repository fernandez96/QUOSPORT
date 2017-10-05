using Base.BusinessEntity;
using Base.BusinessLogic.Interfaces;
using Base.Common.Generics;
using Base.DataAccess;

namespace Base.BusinessLogic
{
    public class StatusBL: Singleton<StatusBL>, IStatusBL<Status, int>
    {
        public int status(Status entity)
        {
            return StatusRepository.Instancia.status(entity);
        }
    }
}
