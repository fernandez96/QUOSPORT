using Base.BusinessEntity;
using Base.BusinessLogic.Interfaces;
using Base.Common;
using Base.Common.Generics;
using Base.DataAccess;
using System.Collections.Generic;
namespace Base.BusinessLogic
{
    public class NotaIngresoBL: Singleton<NotaIngresoBL>, INotaIngresoBL<NotaIngreso,NotaIngresoDetalle, int>
    {
        public int AddNI(NotaIngreso entity)
        {
            return NotaIngresoRepository.Instancia.AddNI(entity);
        }
        public int UpdateNI(NotaIngreso entity)
        {
            return NotaIngresoRepository.Instancia.UpdateNI(entity);
        }
        public int DeleteNI(NotaIngreso entity)
        {
            return NotaIngresoRepository.Instancia.DeleteNI(entity);
        }
        public IList<NotaIngreso> GetAllPaging(PaginationParameter<int> paginationParameters)
        {
            return NotaIngresoRepository.Instancia.GetAllPaging(paginationParameters);
        }
        public NotaIngreso GetById(NotaIngreso entity)
        {
            return NotaIngresoRepository.Instancia.GetById(entity);
        }
        public IList<NotaIngresoDetalle> GetAll(NotaIngreso entity)
        {
            return NotaIngresoRepository.Instancia.GetAll(entity);
        }
        public int GetCorrelativo(NotaIngreso entity)
        {
            return NotaIngresoRepository.Instancia.GetCorrelativo(entity);
        }
    }
}
