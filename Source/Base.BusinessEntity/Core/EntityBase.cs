using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.BusinessEntity.Core
{
  public class EntityBase<T>
    {
        public T Id { get; set; }
        public int Estado { get; set; }
        public T Cantidad { get; set; }
    }
}
