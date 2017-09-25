using Base.BusinessEntity;
using Base.Common.Generics;
using Base.DataAccess.Core;
using Base.DataAccess.Interfaces;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.DataAccess
{
  public  class ModuloRepository: Singleton<ModuloRepository>, IModuloRepository<Modulo>
    {
        #region Attributos

        private readonly Database _database = new DatabaseProviderFactory().Create(ConectionStringRepository.ConnectionStringNameSQL);

        #endregion
        public IList<Modulo> GetAllActives()
        {
            List<Modulo> modulos = new List<Modulo>();
            using (var comando = _database.GetStoredProcCommand(string.Format("{0}{1}", ConectionStringRepository.EsquemaName, "SGE_TABLA_MODULO_GetAllFilter")))
            {
                using (var lector = _database.ExecuteReader(comando))
                {
                    while (lector.Read())
                    {
                       modulos.Add(new Modulo
                        {
                            Id = lector.IsDBNull(lector.GetOrdinal("Id")) ? default(int) : lector.GetInt32(lector.GetOrdinal("Id")),
                            tablc_vdescripcion = lector.IsDBNull(lector.GetOrdinal("tablc_vdescripcion")) ? default(string) : lector.GetString(lector.GetOrdinal("tablc_vdescripcion")),
                        });
                    }
                }
            }

            return modulos;
        }
    }
}
