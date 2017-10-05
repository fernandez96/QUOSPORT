using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.DTO
{
   public class StatusDTO
    {
        public int Estado { get; set; }
        public int Id { get; set; }
        public string tabla { get; set; }
        public string setStatus { get; set; }
        public string where { get; set; }
    }
}
