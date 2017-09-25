using Base.BusinessEntity;
using Base.Common;
using Base.Common.Generics;
using Base.DataAccess.Core;
using Base.DataAccess.Interfaces;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.DataAccess
{
   public class CargoRepository:Singleton<CargoRepository>, ICargoRepository<Cargo,int>
    {
        private readonly Database _database = new DatabaseProviderFactory().Create(ConectionStringRepository.ConnectionStringNameSQL);

        public IList<Cargo> GetAll(PaginationParameter<int> paginationParameter)
        {
            List<Cargo> cargo = new List<Cargo>();

            using (var comando = _database.GetStoredProcCommand(string.Format("{0}{1}", ConectionStringRepository.EsquemaName, "CargoGetAllFilter")))
            {
                _database.AddInParameter(comando, "@WhereFilters", DbType.String, paginationParameter.WhereFilter);
                using (var lector = _database.ExecuteReader(comando))
                {
                    if (lector != null)
                    {
                        while (lector.Read())
                        {
                            cargo.Add(new Cargo
                            {
                                Id = lector.IsDBNull(lector.GetOrdinal("Id")) ? default(Int32) : lector.GetInt32(lector.GetOrdinal("Id")),
                                Nombre = lector.IsDBNull(lector.GetOrdinal("Nombre")) ? default(String) : lector.GetString(lector.GetOrdinal("Nombre")),
                                Descripcion = lector.IsDBNull(lector.GetOrdinal("Descripcion")) ? default(String) : lector.GetString(lector.GetOrdinal("Descripcion")),
                                Estado = lector.IsDBNull(lector.GetOrdinal("Estado")) ? default(Int32) : lector.GetInt32(lector.GetOrdinal("Estado"))
                            });
                        }
                    }
                }
            }
            return cargo;

        }

        public IList<Cargo> GetAllPaging(int Id)
        {
            List<Cargo> cargo = new List<Cargo>();

            using (var comando = _database.GetStoredProcCommand(string.Format("{0}{1}"), ConectionStringRepository.EsquemaName, "CargoGetById"))
            {
                _database.AddInParameter(comando, "@Id", DbType.Int32, Id);
                using (var lector = _database.ExecuteReader(comando))
                {
                    if (lector != null)
                    {
                        while (lector.Read())
                        {
                            cargo.Add(new Cargo
                            {
                                Id = lector.IsDBNull(lector.GetOrdinal("Id")) ? default(Int32) : lector.GetInt32(lector.GetOrdinal("Id")),
                                Nombre = lector.IsDBNull(lector.GetOrdinal("Nombre")) ? default(String) : lector.GetString(lector.GetOrdinal("Nombre")),
                                Descripcion = lector.IsDBNull(lector.GetOrdinal("Descripcion")) ? default(String) : lector.GetString(lector.GetOrdinal("Descripcion")),
                                Estado = lector.IsDBNull(lector.GetOrdinal("Estado")) ? default(Int32) : lector.GetInt32(lector.GetOrdinal("Estado"))
                            });
                        }
                    }
                }
            }
            return cargo;

        }
    }
}
