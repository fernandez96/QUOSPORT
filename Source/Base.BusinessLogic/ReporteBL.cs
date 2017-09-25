using Base.BusinessEntity;
using Base.BusinessLogic.Interfaces;
using Base.Common;
using Base.Common.Generics;
using Base.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.BusinessLogic
{
   public class ReporteBL:Singleton<ReporteBL>, IReporteBL<Reporte,int>
    {
        public IList<Reporte> VentaGetAllFilter(PaginationParameter<int> paginationParameter)
        {
            return ReporteRepository.Instancia.VentaGetAllFilter(paginationParameter);
        }
        public IList<Reporte> VentaGetAllReport(PaginationParameter<int> paginationParameter)
        {
            return ReporteRepository.Instancia.VentaGetAllReport(paginationParameter);
        }

    }
}
