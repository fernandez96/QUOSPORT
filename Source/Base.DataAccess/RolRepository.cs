using Base.BusinessEntity;
using Base.Common.Generics;
using Base.DataAccess.Core;
using Base.DataAccess.Interfaces;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Collections.Generic;

namespace Base.DataAccess
{
    public class RolRepository:Singleton<RolRepository>,IRolRepository<Rol>
    {
        #region Attributos

        private readonly Database _database = new DatabaseProviderFactory().Create(ConectionStringRepository.ConnectionStringNameSQL);

        #endregion

        public IList<Rol> GetAllActives()
        {
            List<Rol> roles = new List<Rol>();
            using (var comando = _database.GetStoredProcCommand(string.Format("{0}{1}", ConectionStringRepository.EsquemaName, "RolGetAllActives")))
            {
                using (var lector = _database.ExecuteReader(comando))
                {
                    while (lector.Read())
                    {
                        roles.Add(new Rol
                        {
                            Id = lector.IsDBNull(lector.GetOrdinal("Id")) ? default(int) : lector.GetInt32(lector.GetOrdinal("Id")),
                            Nombre = lector.IsDBNull(lector.GetOrdinal("Nombre")) ? default(string) : lector.GetString(lector.GetOrdinal("Nombre")),
                        });
                    }
                }
            }

            return roles;
        }
    }
}
