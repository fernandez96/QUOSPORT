using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.BusinessEntity
{
    public class Status
    {
        public int Estado { get; set; }
        public int Id { get; set; }
        public string tabla { get; set; }
        public string  setStatus { get; set; }
        public string where { get; set; }
    }
}
