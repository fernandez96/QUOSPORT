using Base.BusinessEntity;
using Base.BusinessLogic.Interfaces;
using Base.Common;
using Base.Common.Generics;
using Base.DataAccess;
using System.Collections.Generic;
namespace Base.BusinessLogic
{
    public class NotaSalidaBL: Singleton<NotaSalidaBL>, INotaSalidaBL<NotaSalida, NotaSalidaDetalle, int>
    {
        public int AddNS(NotaSalida entity)
        {
            return NotaSalidaRepositoy.Instancia.AddNS(entity);
        }
        public int UpdateNS(NotaSalida entity)
        {
            return NotaSalidaRepositoy.Instancia.UpdateNS(entity);
        }
        public int DeleteNS(NotaSalida entity)
        {
            return NotaSalidaRepositoy.Instancia.DeleteNS(entity);
        }
        public IList<NotaSalida> GetAllPaging(PaginationParameter<int> paginationParameters)
        {
            return NotaSalidaRepositoy.Instancia.GetAllPaging(paginationParameters);
        }
        public NotaSalida GetById(NotaSalida entity)
        {
            return NotaSalidaRepositoy.Instancia.GetById(entity);
        }
        public IList<NotaSalidaDetalle> GetAll(NotaSalida entity)
        {
            return NotaSalidaRepositoy.Instancia.GetAll(entity);
        }
        public int GetCorrelativo(NotaSalida entity)
        {
            return NotaSalidaRepositoy.Instancia.GetCorrelativo(entity);
        }
    }
}
