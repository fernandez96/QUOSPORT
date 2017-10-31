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
    public class ProductoRepository: Singleton<ProductoRepository>, IProductoRepository<Producto, int>
    {
        #region Attributos

        private readonly Database _database = new DatabaseProviderFactory().Create(ConectionStringRepository.ConnectionStringNameSQL);

        #endregion

        #region Métodos Públicos
        public int Add(Producto entity)
        {
            int id;

            using (var comando = _database.GetStoredProcCommand(string.Format("{0}{1}", ConectionStringRepository.EsquemaName, "SGE_PRODUCTO_INSERT")))
            {
                _database.AddInParameter(comando, "@ctgc_iid_categoria", DbType.Int32, entity.ctgc_iid_categoria);
                _database.AddInParameter(comando, "@linc_iid_linea", DbType.Int32, entity.linc_iid_linea);
                _database.AddInParameter(comando, "@lind_iid_sublinea", DbType.Int32, entity.lind_iid_sublinea);
                _database.AddInParameter(comando, "@prdc_vcod_producto", DbType.String, entity.prdc_vcod_producto);
                _database.AddInParameter(comando, "@prdc_vdescripcion", DbType.String, entity.prdc_vdescripcion);
                _database.AddInParameter(comando, "@tablc_iid_iclasif_prod", DbType.Int32, entity.tablc_iid_iclasif_prod);
                _database.AddInParameter(comando, "@prdc_dstock_minimo", DbType.Decimal, entity.prdc_dstock_minimo);
                _database.AddInParameter(comando, "@prdc_dpeso_unitario", DbType.Decimal, entity.prdc_dpeso_unitario);
                _database.AddInParameter(comando, "@umec_iid_unidad_medida", DbType.Int32, entity.umec_iid_unidad_medida);
                _database.AddInParameter(comando, "@prdc_vmaterial1", DbType.String, entity.prdc_vmaterial1);
                _database.AddInParameter(comando, "@prdc_vmaterial2", DbType.String, entity.prdc_vmaterial2);
                _database.AddInParameter(comando, "@prdc_vcodigo_ean", DbType.String, entity.prdc_vcodigo_ean);
                _database.AddInParameter(comando, "@prdc_vcodigo_upc", DbType.String, entity.prdc_vcodigo_upc);
                _database.AddInParameter(comando, "@prdc_vorder_code", DbType.String, entity.prdc_vorder_code);
                _database.AddInParameter(comando, "@prdc_vcolor", DbType.String, entity.prdc_vcolor);
                _database.AddInParameter(comando, "@prdc_vnumero_serie", DbType.String, entity.prdc_vnumero_serie);
                _database.AddInParameter(comando, "@prdc_vcaracteristicas", DbType.String, entity.prdc_vcaracteristicas);
                _database.AddInParameter(comando, "@prdc_bflag_estado", DbType.Int32, entity.Estado);
                _database.AddInParameter(comando, "@prdc_vpc_crea", DbType.String,WindowsIdentity.GetCurrent().Name);
                _database.AddInParameter(comando, "@prdc_vusuario_crea", DbType.String, entity.UsuarioCreacion);
                _database.AddOutParameter(comando, "@Response", DbType.Int32, 11);

                _database.ExecuteNonQuery(comando);
                id = Convert.ToInt32(_database.GetParameterValue(comando, "@Response"));
            }

            return id;
        }
        public int Delete(Producto entity)
        {
            int idResult;

            using (var comando = _database.GetStoredProcCommand(string.Format("{0}{1}", ConectionStringRepository.EsquemaName, "SGE_PRODUCTO_DELETE")))
            {
                _database.AddInParameter(comando, "@Id", DbType.Int32, entity.Id);
                _database.AddOutParameter(comando, "@Response", DbType.Int32, 11);
                _database.ExecuteNonQuery(comando);
                idResult = Convert.ToInt32(_database.GetParameterValue(comando, "@Response"));
            }

            return idResult;
        }
        public IList<Producto> GetAllPaging(PaginationParameter<int> paginationParameters)
        {
            List<Producto> productos = new List<Producto>();
            using (var comando = _database.GetStoredProcCommand(string.Format("{0}{1}", ConectionStringRepository.EsquemaName, "SGE_PRODUCTO_GetAllFilter")))
            {
                _database.AddInParameter(comando, "@WhereFilters", DbType.String, string.IsNullOrWhiteSpace(paginationParameters.WhereFilter) ? string.Empty : paginationParameters.WhereFilter);
                _database.AddInParameter(comando, "@OrderBy", DbType.String, string.IsNullOrWhiteSpace(paginationParameters.OrderBy) ? string.Empty : paginationParameters.OrderBy);
                _database.AddInParameter(comando, "@Start", DbType.Int32, paginationParameters.Start);
                _database.AddInParameter(comando, "@Rows", DbType.Int32, paginationParameters.AmountRows);

                using (var lector = _database.ExecuteReader(comando))
                {
                    while (lector.Read())
                    {
                        productos.Add(new Producto
                        {
                            Id = lector.IsDBNull(lector.GetOrdinal("Id")) ? default(int) : lector.GetInt32(lector.GetOrdinal("Id")),
                            prdc_vcod_producto = lector.IsDBNull(lector.GetOrdinal("prdc_vcod_producto")) ? default(string) : lector.GetString(lector.GetOrdinal("prdc_vcod_producto")),
                            prdc_vdescripcion = lector.IsDBNull(lector.GetOrdinal("prdc_vdescripcion")) ? default(string) : lector.GetString(lector.GetOrdinal("prdc_vdescripcion")),
                            prdc_vorder_code = lector.IsDBNull(lector.GetOrdinal("prdc_vorder_code")) ? default(string) : lector.GetString(lector.GetOrdinal("prdc_vorder_code")),
                            umec_v_unidad_medida = lector.IsDBNull(lector.GetOrdinal("umec_v_unidad_medida")) ? default(string) : lector.GetString(lector.GetOrdinal("umec_v_unidad_medida")),
                            tablc_v_iclasif_prod = lector.IsDBNull(lector.GetOrdinal("tablc_v_iclasif_prod")) ? default(string) : lector.GetString(lector.GetOrdinal("tablc_v_iclasif_prod")),
                            umec_iid_unidad_medida = lector.IsDBNull(lector.GetOrdinal("umec_iid_unidad_medida")) ? default(int) : lector.GetInt32(lector.GetOrdinal("umec_iid_unidad_medida")),
                            tablc_iid_iclasif_prod = lector.IsDBNull(lector.GetOrdinal("tablc_iid_iclasif_prod")) ? default(int) : lector.GetInt32(lector.GetOrdinal("tablc_iid_iclasif_prod")),
                            ctgc_v_categoria = lector.IsDBNull(lector.GetOrdinal("ctgc_v_categoria")) ? default(string) : lector.GetString(lector.GetOrdinal("ctgc_v_categoria")),
                            linc_v_linea = lector.IsDBNull(lector.GetOrdinal("linc_v_linea")) ? default(string) : lector.GetString(lector.GetOrdinal("linc_v_linea")),
                            lind_v_sublinea = lector.IsDBNull(lector.GetOrdinal("lind_v_sublinea")) ? default(string) : lector.GetString(lector.GetOrdinal("lind_v_sublinea")),
                            Estado = lector.IsDBNull(lector.GetOrdinal("prdc_bflag_estado")) ? default(int) : lector.GetInt32(lector.GetOrdinal("prdc_bflag_estado")),
                            Cantidad = lector.IsDBNull(lector.GetOrdinal("Cantidad")) ? default(int) : lector.GetInt32(lector.GetOrdinal("Cantidad"))
                        });
                    }
                }
            }

            return productos;
        } 
        public Producto GetById(Producto entity)
        {
            Producto producto = null;
            using (var comando = _database.GetStoredProcCommand(string.Format("{0}{1}", ConectionStringRepository.EsquemaName, "SGE_PRODUCTO_GetById")))
            {
                _database.AddInParameter(comando, "@Id", DbType.Int32, entity.Id);

                using (var lector = _database.ExecuteReader(comando))
                {
                    if (lector.Read())
                    {
                        producto = new Producto
                        {
                            Id = lector.IsDBNull(lector.GetOrdinal("prdc_iid_producto")) ? default(int) : lector.GetInt32(lector.GetOrdinal("prdc_iid_producto")),
                            ctgc_iid_categoria = lector.IsDBNull(lector.GetOrdinal("ctgc_iid_categoria")) ? default(int) : lector.GetInt32(lector.GetOrdinal("ctgc_iid_categoria")),
                            linc_iid_linea = lector.IsDBNull(lector.GetOrdinal("linc_iid_linea")) ? default(int) : lector.GetInt32(lector.GetOrdinal("linc_iid_linea")),
                            lind_iid_sublinea = lector.IsDBNull(lector.GetOrdinal("lind_iid_sublinea")) ? default(int) : lector.GetInt32(lector.GetOrdinal("lind_iid_sublinea")),
                            prdc_vcod_producto = lector.IsDBNull(lector.GetOrdinal("prdc_vcod_producto")) ? default(string) : lector.GetString(lector.GetOrdinal("prdc_vcod_producto")),
                            prdc_vdescripcion = lector.IsDBNull(lector.GetOrdinal("prdc_vdescripcion")) ? default(string) : lector.GetString(lector.GetOrdinal("prdc_vdescripcion")),
                            prdc_vorder_code = lector.IsDBNull(lector.GetOrdinal("prdc_vorder_code")) ? default(string) : lector.GetString(lector.GetOrdinal("prdc_vorder_code")),
                            prdc_vmaterial1 = lector.IsDBNull(lector.GetOrdinal("prdc_vmaterial1")) ? default(string) : lector.GetString(lector.GetOrdinal("prdc_vmaterial1")),
                            prdc_vmaterial2 = lector.IsDBNull(lector.GetOrdinal("prdc_vmaterial2")) ? default(string) : lector.GetString(lector.GetOrdinal("prdc_vmaterial2")),
                            prdc_vcodigo_ean = lector.IsDBNull(lector.GetOrdinal("prdc_vcodigo_ean")) ? default(string) : lector.GetString(lector.GetOrdinal("prdc_vcodigo_ean")),
                            prdc_vcodigo_upc = lector.IsDBNull(lector.GetOrdinal("prdc_vcodigo_upc")) ? default(string) : lector.GetString(lector.GetOrdinal("prdc_vcodigo_upc")),
                            prdc_vcolor = lector.IsDBNull(lector.GetOrdinal("prdc_vcolor")) ? default(string) : lector.GetString(lector.GetOrdinal("prdc_vcolor")),
                            prdc_vnumero_serie = lector.IsDBNull(lector.GetOrdinal("prdc_vnumero_serie")) ? default(string) : lector.GetString(lector.GetOrdinal("prdc_vnumero_serie")),
                            prdc_vcaracteristicas = lector.IsDBNull(lector.GetOrdinal("prdc_vcaracteristicas")) ? default(string) : lector.GetString(lector.GetOrdinal("prdc_vcaracteristicas")),
                            prdc_dstock_minimo = lector.IsDBNull(lector.GetOrdinal("prdc_dstock_minimo")) ? default(decimal) : lector.GetDecimal(lector.GetOrdinal("prdc_dstock_minimo")),
                            prdc_dpeso_unitario = lector.IsDBNull(lector.GetOrdinal("prdc_dpeso_unitario")) ? default(decimal) : lector.GetDecimal(lector.GetOrdinal("prdc_dpeso_unitario")),
                            tablc_iid_iclasif_prod = lector.IsDBNull(lector.GetOrdinal("tablc_iid_iclasif_prod")) ? default(int) : lector.GetInt32(lector.GetOrdinal("tablc_iid_iclasif_prod")),
                            umec_iid_unidad_medida = lector.IsDBNull(lector.GetOrdinal("umec_iid_unidad_medida")) ? default(int) : lector.GetInt32(lector.GetOrdinal("umec_iid_unidad_medida")),
                            Estado = lector.IsDBNull(lector.GetOrdinal("prdc_bflag_estado")) ? default(int) : lector.GetInt32(lector.GetOrdinal("prdc_bflag_estado"))
                        };
                    }
                }
            }

            return producto;
        }
        public int Update(Producto entity)
        {
            int id;

            using (var comando = _database.GetStoredProcCommand(string.Format("{0}{1}", ConectionStringRepository.EsquemaName, "SGE_SGE_PRODUCTO_UPDATE")))
            {
                _database.AddInParameter(comando, "@ctgc_iid_categoria", DbType.Int32, entity.ctgc_iid_categoria);
                _database.AddInParameter(comando, "@linc_iid_linea", DbType.Int32, entity.linc_iid_linea);
                _database.AddInParameter(comando, "@lind_iid_sublinea", DbType.Int32, entity.lind_iid_sublinea);
                _database.AddInParameter(comando, "@prdc_vcod_producto", DbType.String, entity.prdc_vcod_producto);
                _database.AddInParameter(comando, "@prdc_vdescripcion", DbType.String, entity.prdc_vdescripcion);
                _database.AddInParameter(comando, "@tablc_iid_iclasif_prod", DbType.Int32, entity.tablc_iid_iclasif_prod);
                _database.AddInParameter(comando, "@prdc_dstock_minimo", DbType.Decimal, entity.prdc_dstock_minimo);
                _database.AddInParameter(comando, "@prdc_dpeso_unitario", DbType.Decimal, entity.prdc_dpeso_unitario);
                _database.AddInParameter(comando, "@umec_iid_unidad_medida", DbType.Int32, entity.umec_iid_unidad_medida);
                _database.AddInParameter(comando, "@prdc_vmaterial1", DbType.String, entity.prdc_vmaterial1);
                _database.AddInParameter(comando, "@prdc_vmaterial2", DbType.String, entity.prdc_vmaterial2);
                _database.AddInParameter(comando, "@prdc_vcodigo_ean", DbType.String, entity.prdc_vcodigo_ean);
                _database.AddInParameter(comando, "@prdc_vcodigo_upc", DbType.String, entity.prdc_vcodigo_upc);
                _database.AddInParameter(comando, "@prdc_vorder_code", DbType.String, entity.prdc_vorder_code);
                _database.AddInParameter(comando, "@prdc_vcolor", DbType.String, entity.prdc_vcolor);
                _database.AddInParameter(comando, "@prdc_vnumero_serie", DbType.String, entity.prdc_vnumero_serie);
                _database.AddInParameter(comando, "@prdc_vcaracteristicas", DbType.String, entity.prdc_vcaracteristicas);
                _database.AddInParameter(comando, "@prdc_vpc_modifica", DbType.String, WindowsIdentity.GetCurrent().Name);
                _database.AddInParameter(comando, "@prdc_vusuario_modifica", DbType.String, entity.UsuarioModificacion);
                _database.AddInParameter(comando, "@prdc_bflag_estado", DbType.Int32, entity.Estado);
                _database.AddInParameter(comando, "@id", DbType.Int32, entity.Id);
                _database.AddOutParameter(comando, "@Response", DbType.Int32, 11);

                _database.ExecuteNonQuery(comando);
                id = Convert.ToInt32(_database.GetParameterValue(comando, "@Response"));
            }

            return id;
        }
        public IList<Producto> GetAllPagingStock(PaginationParameter<int> paginationParameters)
        {
            List<Producto> productos = new List<Producto>();
            using (var comando = _database.GetStoredProcCommand(string.Format("{0}{1}", ConectionStringRepository.EsquemaName, "SGE_PRODUCTO_STOCK_GetAllFilter")))
            {
                _database.AddInParameter(comando, "@WhereFilters", DbType.String, string.IsNullOrWhiteSpace(paginationParameters.WhereFilter) ? string.Empty : paginationParameters.WhereFilter);
                _database.AddInParameter(comando, "@OrderBy", DbType.String, string.IsNullOrWhiteSpace(paginationParameters.OrderBy) ? string.Empty : paginationParameters.OrderBy);
                _database.AddInParameter(comando, "@Start", DbType.Int32, paginationParameters.Start);
                _database.AddInParameter(comando, "@Rows", DbType.Int32, paginationParameters.AmountRows);

                using (var lector = _database.ExecuteReader(comando))
                {
                    while (lector.Read())
                    {
                        productos.Add(new Producto
                        {
                            Id = lector.IsDBNull(lector.GetOrdinal("Id")) ? default(int) : lector.GetInt32(lector.GetOrdinal("Id")),
                            prdc_vcod_producto = lector.IsDBNull(lector.GetOrdinal("prdc_vcod_producto")) ? default(string) : lector.GetString(lector.GetOrdinal("prdc_vcod_producto")),
                            prdc_vdescripcion = lector.IsDBNull(lector.GetOrdinal("prdc_vdescripcion")) ? default(string) : lector.GetString(lector.GetOrdinal("prdc_vdescripcion")),
                            umec_v_unidad_medida = lector.IsDBNull(lector.GetOrdinal("umec_vdescripcion")) ? default(string) : lector.GetString(lector.GetOrdinal("umec_vdescripcion")),
                            prdc_dstock_minimo= lector.IsDBNull(lector.GetOrdinal("stocc_stock_producto")) ? default(decimal) : lector.GetDecimal(lector.GetOrdinal("stocc_stock_producto")),
                            Cantidad = lector.IsDBNull(lector.GetOrdinal("Cantidad")) ? default(int) : lector.GetInt32(lector.GetOrdinal("Cantidad"))
                        });
                    }
                }
            }

            return productos;
        }

        public decimal GetStockProducto(int idproducto)
        {
            decimal stock;

            using (var comando = _database.GetStoredProcCommand(string.Format("{0}{1}", ConectionStringRepository.EsquemaName, "SGE_EVALUAR_STOCK")))
            {
                _database.AddInParameter(comando, "@idProducto", DbType.Int32, idproducto);
                _database.AddOutParameter(comando, "@Response", DbType.Decimal, 11);

                _database.ExecuteNonQuery(comando);
                stock = Convert.ToInt32(_database.GetParameterValue(comando, "@Response"));
            }

            return stock;
        }


        #endregion
    }
}
