using Base.BusinessEntity;
using Base.Common.Generics;
using Base.DataAccess.Core;
using Base.DataAccess.Interfaces;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Collections.Generic;

namespace Base.DataAccess
{
    public class SeguridadRepository: Singleton<SeguridadRepository>, ISeguridadRepository<Formulario>
    {
        #region Attributos

        private readonly Database _database = new DatabaseProviderFactory().Create(ConectionStringRepository.ConnectionStringNameSQL);

        #endregion


        public IList<Formulario> GetFormulario()
        {
            List<Formulario> formularios = new List<Formulario>();
            using (var comando= _database.GetStoredProcCommand(string.Format("{0}{1}", ConectionStringRepository.EsquemaName, "SGE_FORMULARIO_GETALL")))
            {
                using (var lector=_database.ExecuteReader(comando))
                {
                    if (lector!=null)
                    {
                        while (lector.Read())
                        {
                            formularios.Add(new Formulario
                            {
                                Direccion=lector.IsDBNull(lector.GetOrdinal("direccion"))?default(string):lector.GetString(lector.GetOrdinal("direccion")),
                                Nombre = lector.IsDBNull(lector.GetOrdinal("nombre")) ? default(string) : lector.GetString(lector.GetOrdinal("nombre")),
                                RolId = lector.IsDBNull(lector.GetOrdinal("RolId")) ? default(int) : lector.GetInt32(lector.GetOrdinal("RolId")),
                                Orden = lector.IsDBNull(lector.GetOrdinal("orden")) ? default(int) : lector.GetInt32(lector.GetOrdinal("orden")),
                                Id = lector.IsDBNull(lector.GetOrdinal("id")) ? default(int) : lector.GetInt32(lector.GetOrdinal("id")),
                                idModulo = lector.IsDBNull(lector.GetOrdinal("idModulo")) ? default(int) : lector.GetInt32(lector.GetOrdinal("idModulo")),
                            });
                        }
                    }
                }
            }

            return formularios;
        }

    }
}
