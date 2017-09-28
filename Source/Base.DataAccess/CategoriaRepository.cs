using Base.BusinessEntity;
using Base.Common;
using Base.Common.Generics;
using Base.DataAccess.Core;
using Base.DataAccess.Interfaces;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Security.Principal;
namespace Base.DataAccess
{
    public class CategoriaRepository: Singleton<CategoriaRepository>, ICategoriaRepository<Categoria, int>
    {
        #region Attributos

        private readonly Database _database = new DatabaseProviderFactory().Create(ConectionStringRepository.ConnectionStringNameSQL);

        public const int add = 1;
        public const int update = 2;
        public const int delete = 3;
        #endregion

        public int Add(Categoria entity)
        {
            int idcategoria;
            int idlinea;
            int idsublinea;
            using (var comando= _database.GetStoredProcCommand(string.Format("{0}{1}",ConectionStringRepository.EsquemaName, "SGE_CATEGORIAS_PRODUCTO_INSERT")))
            {
                _database.AddInParameter(comando, "@ctgcc_vcod_categoria", DbType.String, entity.ctgcc_vcod_categoria);
                _database.AddInParameter(comando, "@ctgcc_vdescripcion", DbType.String, entity.ctgcc_vdescripcion);
                _database.AddInParameter(comando, "@ctgc_vusuario_crea", DbType.String, entity.UsuarioCreacion);
                _database.AddInParameter(comando, "@ctgc_vpc_crea", DbType.String, WindowsIdentity.GetCurrent().Name);
                _database.AddInParameter(comando, "@ctgcc_iflag_estado", DbType.Int32,1);
                _database.AddOutParameter(comando, "@Response", DbType.Int32, 11);
                _database.ExecuteNonQuery(comando);

                idcategoria =Convert.ToInt32(_database.GetParameterValue(comando, "@Response"));
            }
            foreach (var item in entity.detalleLinea)
            {
                if (item.status==add)
                {
                    using (var comando = _database.GetStoredProcCommand(string.Format("{0}{1}", ConectionStringRepository.EsquemaName, "SGE_LINEA_PRODUCTO_CAB_INSERT")))
                    {
                        _database.AddInParameter(comando, "@ctgcc_iid_categoria", DbType.Int32, idcategoria);
                        _database.AddInParameter(comando, "@linc_vcod_linea", DbType.String, item.linc_vcod_linea);
                        _database.AddInParameter(comando, "@linc_vdescripcion", DbType.String, item.linc_vdescripcion);
                        _database.AddInParameter(comando, "@linc_vusuario_crea", DbType.String, entity.UsuarioCreacion);
                        _database.AddInParameter(comando, "@linc_vpc_crea", DbType.String, WindowsIdentity.GetCurrent().Name);
                        _database.AddInParameter(comando, "@linc_iflag_estado", DbType.Int32, 1);
                        _database.AddOutParameter(comando, "@Response", DbType.Int32, 11);
                        _database.ExecuteNonQuery(comando);

                        idlinea = Convert.ToInt32(_database.GetParameterValue(comando, "@Response"));
                    }
                    foreach (var itemsublinea in entity.detalleSubLinea)
                    {
                        if (itemsublinea.status == add)
                        {
                            using (var comando = _database.GetStoredProcCommand(string.Format("{0}{1}", ConectionStringRepository.EsquemaName, "SGE_LINEA_PRODUCTO_DET_INSERT")))
                            {
                                _database.AddInParameter(comando, "@ctgcc_iid_categoria", DbType.Int32, idcategoria);
                                _database.AddInParameter(comando, "@linc_iid_linea", DbType.Int32, idlinea);
                                _database.AddInParameter(comando, "@lind_vcod_sublinea", DbType.String, itemsublinea.lind_vcod_sublinea);
                                _database.AddInParameter(comando, "@lind_vdescripcion", DbType.String, itemsublinea.lind_vdescripcion);
                                _database.AddInParameter(comando, "@lind_vusuario_crea", DbType.String, entity.UsuarioCreacion);
                                _database.AddInParameter(comando, "@lind_vpc_crea", DbType.String, WindowsIdentity.GetCurrent().Name);
                                _database.AddInParameter(comando, "@lind_iflag_estado", DbType.Int32, 1);
                                _database.AddOutParameter(comando, "@Response", DbType.Int32, 11);
                                _database.ExecuteNonQuery(comando);

                                idsublinea = Convert.ToInt32(_database.GetParameterValue(comando, "@Response"));
                            }
                        }

                    }
                }
               
               
            }
        
            return idcategoria;
        }
    }
}
