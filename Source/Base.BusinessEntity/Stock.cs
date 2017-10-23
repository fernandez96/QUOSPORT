using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.BusinessEntity
{
    public class Stock
    {
        public int Id { get; set; }
        public int almac_icod_almacen { get; set; }
        public int prdc_icod_producto { get; set; }
        public decimal stocc_stock_producto { get; set; }
        public int Estado { get; set; }
    }
}
