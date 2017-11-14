using Base.BusinessEntity;
using Base.Common;
using Base.Common.Generics;
using Base.DataAccess.Core;
using Base.DataAccess.Interfaces;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Security.Principal;
namespace Base.DataAccess
{
    public class TransferenciaRepository
    {
        #region Attributos

        private readonly Database _database = new DatabaseProviderFactory().Create(ConectionStringRepository.ConnectionStringNameSQL);
        public const int add = 1;
        public const int update = 2;
        public const int delete = 3;
        #endregion

        public int AddNS(NotaSalida entity)
        {
            int id;
            int idKardex;
            int idND;
            int idStock;
            using (DbConnection conexion = _database.CreateConnection())
            {
                conexion.Open();
                using (var transaction = conexion.BeginTransaction(IsolationLevel.Serializable))
                {
                    try
                    {
                        using (var comando = _database.GetStoredProcCommand(string.Format("{0}{1}", ConectionStringRepository.EsquemaName, "SGE_NOTA_SALIDA_INSERT")))
                        {
                            _database.AddInParameter(comando, "@nsalc_numero_nota_salida", DbType.String, entity.nsalc_numero_nota_salida);
                            _database.AddInParameter(comando, "@almac_icod_almacen", DbType.Int32, entity.almac_icod_almacen);
                            _database.AddInParameter(comando, "@nsalc_iid_motivo", DbType.Int32, entity.nsalc_iid_motivo);
                            _database.AddInParameter(comando, "@nsalc_fecha_nota_salida", DbType.String, entity.nsalc_fecha_nota_salida_);
                            _database.AddInParameter(comando, "@tdocc_icod_tipo_doc", DbType.Int32, entity.tdocc_icod_tipo_doc);
                            _database.AddInParameter(comando, "@nsalc_referencia", DbType.String, entity.nsalc_referencia);
                            _database.AddInParameter(comando, "@nsalc_observaciones", DbType.String, entity.nsalc_observaciones);
                            _database.AddInParameter(comando, "@nsalc_usuario_crea", DbType.String, entity.UsuarioCreacion);
                            _database.AddInParameter(comando, "@nsalc_pc_crea", DbType.String, WindowsIdentity.GetCurrent().Name);
                            _database.AddInParameter(comando, "@nsalc_ilag_estado", DbType.Int32, entity.Estado);
                            _database.AddOutParameter(comando, "@Response", DbType.Int32, 11);


                            _database.ExecuteNonQuery(comando, transaction);
                            id = Convert.ToInt32(_database.GetParameterValue(comando, "@Response"));
                            if (id == 0) throw new Exception("Error al ingresar Nota Ingreso");
                        }

                        foreach (var itemdetalle in entity.listaDetalleNS)
                        {
                            using (var comandoKardex = _database.GetStoredProcCommand(string.Format("{0}{1}", ConectionStringRepository.EsquemaName, "SGE_KARDEX_INSERT")))
                            {
                                _database.AddInParameter(comandoKardex, "@kardc_fecha_movimiento", DbType.String, entity.nsalc_fecha_nota_salida_);
                                _database.AddInParameter(comandoKardex, "@ningc_icod_nota_ingreso", DbType.Int32, id);
                                _database.AddInParameter(comandoKardex, "@almac_icod_almacen", DbType.Int32, entity.almac_icod_almacen);
                                _database.AddInParameter(comandoKardex, "@prdc_icod_producto", DbType.Int32, itemdetalle.prdc_icod_producto);
                                _database.AddInParameter(comandoKardex, "@kardc_icantidad_prod", DbType.Decimal, itemdetalle.nsald_cantidad);
                                _database.AddInParameter(comandoKardex, "@tdocc_icod_tipo_doc", DbType.Int32, entity.tdocc_icod_tipo_doc);
                                _database.AddInParameter(comandoKardex, "@kardc_numero_doc", DbType.String, entity.nsalc_numero_nota_salida);
                                _database.AddInParameter(comandoKardex, "@kardc_tipo_movimiento", DbType.Int32, 2);
                                _database.AddInParameter(comandoKardex, "@kardc_monto_unitario_compra", DbType.Decimal, 0);
                                _database.AddInParameter(comandoKardex, "@kardc_iid_motivo", DbType.Int32, entity.nsalc_iid_motivo);
                                _database.AddInParameter(comandoKardex, "@kardc_beneficiario", DbType.String, entity.nsalc_referencia);
                                _database.AddInParameter(comandoKardex, "@kardc_observaciones", DbType.String, entity.nsalc_observaciones);
                                _database.AddInParameter(comandoKardex, "@kardc_vusuario_crea", DbType.String, entity.UsuarioCreacion);
                                _database.AddInParameter(comandoKardex, "@kardc_vpc_crea", DbType.String, WindowsIdentity.GetCurrent().Name);
                                _database.AddInParameter(comandoKardex, "@kardc_ilag_estado", DbType.Int32, entity.Estado);
                                _database.AddOutParameter(comandoKardex, "@Response", DbType.Int32, 11);

                                _database.ExecuteNonQuery(comandoKardex, transaction);
                                idKardex = Convert.ToInt32(_database.GetParameterValue(comandoKardex, "@Response"));
                                if (idKardex == 0) throw new Exception("Error al ingresar kardex");
                            }
                            using (var comando = _database.GetStoredProcCommand(string.Format("{0}{1}", ConectionStringRepository.EsquemaName, "SGE_NOTA_SALIDA_DETALLE_INSERT")))
                            {
                                _database.AddInParameter(comando, "@nsalc_icod_nota_salida", DbType.Int32, id);
                                _database.AddInParameter(comando, "@nsald_nro_item", DbType.String, itemdetalle.nsald_nro_item);
                                _database.AddInParameter(comando, "@prdc_icod_producto", DbType.Int32, itemdetalle.prdc_icod_producto);
                                _database.AddInParameter(comando, "@nsald_cantidad", DbType.Decimal, itemdetalle.nsald_cantidad);
                                _database.AddInParameter(comando, "@kardc_icod_correlativo", DbType.Int32, idKardex);
                                _database.AddInParameter(comando, "@nsald_usuario_crea", DbType.String, entity.UsuarioCreacion);
                                _database.AddInParameter(comando, "@nsald_pc_crea", DbType.String, WindowsIdentity.GetCurrent().Name);
                                _database.AddInParameter(comando, "@nsald_ilag_estado", DbType.Int32, entity.Estado);
                                _database.AddOutParameter(comando, "@Response", DbType.Int32, 11);

                                _database.ExecuteNonQuery(comando, transaction);
                                idND = Convert.ToInt32(_database.GetParameterValue(comando, "@Response"));
                                if (idND == 0) throw new Exception("Error al ingresar nota de ingreso detalle");
                            }
                            using (var ComandoStock = _database.GetStoredProcCommand(string.Format("{0}{1}", ConectionStringRepository.EsquemaName, "SGE_STOCK_UPDATE")))
                            {
                                _database.AddInParameter(ComandoStock, "@almac_icod_almacen", DbType.Int32, entity.almac_icod_almacen);
                                _database.AddInParameter(ComandoStock, "@prdc_icod_producto", DbType.Int32, itemdetalle.prdc_icod_producto);
                                _database.AddInParameter(ComandoStock, "@stocc_stock_producto", DbType.Decimal, itemdetalle.nsald_cantidad);
                                _database.AddInParameter(ComandoStock, "@stocc_ilag_estado", DbType.Int32, entity.Estado);
                                _database.AddInParameter(ComandoStock, "@tipo_movimiento", DbType.Int32, 2);
                                _database.AddOutParameter(ComandoStock, "@Response", DbType.Int32, 11);

                                _database.ExecuteNonQuery(ComandoStock, transaction);
                                idStock = Convert.ToInt32(_database.GetParameterValue(ComandoStock, "@Response"));
                                if (idStock == 0) throw new Exception("Error al ingresar stock");
                            }
                        }

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception(ex.Message);
                    }

                }
            }
            return id;
        }
        public int UpdateNS(NotaSalida entity)
        {
            int id;
            int idKardex;
            int idND;
            int idStock;
            using (DbConnection conexion = _database.CreateConnection())
            {
                conexion.Open();
                using (var transaction = conexion.BeginTransaction(IsolationLevel.Serializable))
                {
                    try
                    {
                        using (var comando = _database.GetStoredProcCommand(string.Format("{0}{1}", ConectionStringRepository.EsquemaName, "SGE_NOTA_SALIDA_UPDATE")))
                        {
                            _database.AddInParameter(comando, "@nsalc_numero_nota_salida", DbType.String, entity.nsalc_numero_nota_salida);
                            _database.AddInParameter(comando, "@almac_icod_almacen", DbType.String, entity.almac_icod_almacen);
                            _database.AddInParameter(comando, "@nsalc_iid_motivo", DbType.String, entity.nsalc_iid_motivo);
                            _database.AddInParameter(comando, "@nsalc_fecha_nota_salida", DbType.String, entity.nsalc_fecha_nota_salida_);
                            _database.AddInParameter(comando, "@tdocc_icod_tipo_doc", DbType.Int32, entity.tdocc_icod_tipo_doc);
                            _database.AddInParameter(comando, "@nsalc_referencia", DbType.String, entity.nsalc_referencia);
                            _database.AddInParameter(comando, "@nsalc_observaciones", DbType.String, entity.nsalc_observaciones);
                            _database.AddInParameter(comando, "@nsalc_usuario_modifica", DbType.String, entity.UsuarioModificacion);
                            _database.AddInParameter(comando, "@nsalc_pc_modifica", DbType.String, WindowsIdentity.GetCurrent().Name);
                            _database.AddInParameter(comando, "@nsalc_ilag_estado", DbType.Int32, 1);
                            _database.AddInParameter(comando, "@id", DbType.Int32, entity.Id);
                            _database.AddOutParameter(comando, "@Response", DbType.Int32, 11);

                            _database.ExecuteNonQuery(comando, transaction);
                            id = Convert.ToInt32(_database.GetParameterValue(comando, "@Response"));
                            if (id == -1) throw new Exception("Error al modificar nota de salida");
                        }
                        foreach (var itemdetalle in entity.listaDetalleNS)
                        {
                            if (itemdetalle.status == update && itemdetalle.kardc_tipo_movimiento == 0)
                            {
                                using (var comandoKardex = _database.GetStoredProcCommand(string.Format("{0}{1}", ConectionStringRepository.EsquemaName, "SGE_KARDEX_UPDATE")))
                                {
                                    _database.AddInParameter(comandoKardex, "@kardc_fecha_movimiento", DbType.String, entity.nsalc_fecha_nota_salida_);
                                    _database.AddInParameter(comandoKardex, "@ningc_icod_nota_ingreso", DbType.Int32, id);
                                    _database.AddInParameter(comandoKardex, "@almac_icod_almacen", DbType.Int32, entity.almac_icod_almacen);
                                    _database.AddInParameter(comandoKardex, "@prdc_icod_producto", DbType.Int32, itemdetalle.prdc_icod_producto);
                                    _database.AddInParameter(comandoKardex, "@kardc_icantidad_prod", DbType.Decimal, itemdetalle.nsald_cantidad);
                                    _database.AddInParameter(comandoKardex, "@tdocc_icod_tipo_doc", DbType.Int32, entity.tdocc_icod_tipo_doc);
                                    _database.AddInParameter(comandoKardex, "@kardc_numero_doc", DbType.String, entity.nsalc_numero_nota_salida);
                                    _database.AddInParameter(comandoKardex, "@kardc_tipo_movimiento", DbType.Int32, 2);
                                    _database.AddInParameter(comandoKardex, "@kardc_iid_motivo", DbType.Int32, entity.nsalc_iid_motivo);
                                    _database.AddInParameter(comandoKardex, "@kardc_beneficiario", DbType.String, entity.nsalc_referencia);
                                    _database.AddInParameter(comandoKardex, "@kardc_observaciones", DbType.String, entity.nsalc_observaciones);
                                    _database.AddInParameter(comandoKardex, "@kardc_monto_unitario_compra", DbType.Decimal, 0);
                                    _database.AddInParameter(comandoKardex, "@kardc_vusuario_modifica", DbType.String, entity.UsuarioCreacion);
                                    _database.AddInParameter(comandoKardex, "@kardc_vpc_modifica", DbType.String, WindowsIdentity.GetCurrent().Name);
                                    _database.AddInParameter(comandoKardex, "@kardc_ilag_estado", DbType.Int32, entity.Estado);
                                    _database.AddInParameter(comandoKardex, "@Id", DbType.Int32, itemdetalle.kardc_icod_correlativo);
                                    _database.AddOutParameter(comandoKardex, "@Response", DbType.Int32, 11);

                                    _database.ExecuteNonQuery(comandoKardex, transaction);
                                    idKardex = Convert.ToInt32(_database.GetParameterValue(comandoKardex, "@Response"));
                                    if (idKardex == 0) throw new Exception("Error al modificar kardex");
                                }
                                using (var comandoND = _database.GetStoredProcCommand(string.Format("{0}{1}", ConectionStringRepository.EsquemaName, "SGE_NOTA_SALIDA_DETALLE_UPDATE")))
                                {
                                    _database.AddInParameter(comandoND, "@nsalc_icod_nota_salida", DbType.Int32, id);
                                    _database.AddInParameter(comandoND, "@nsald_nro_item", DbType.String, itemdetalle.nsald_nro_item);
                                    _database.AddInParameter(comandoND, "@prdc_icod_producto", DbType.Int32, itemdetalle.prdc_icod_producto);
                                    _database.AddInParameter(comandoND, "@nsald_cantidad", DbType.Decimal, itemdetalle.nsald_cantidad);
                                    _database.AddInParameter(comandoND, "@kardc_icod_correlativo", DbType.Int32, idKardex);
                                    _database.AddInParameter(comandoND, "@nsald_usuario_modifca", DbType.String, entity.UsuarioCreacion);
                                    _database.AddInParameter(comandoND, "@nsald_pc_modifica", DbType.String, WindowsIdentity.GetCurrent().Name);
                                    _database.AddInParameter(comandoND, "@nsald_ilag_estado", DbType.Int32, entity.Estado);
                                    _database.AddInParameter(comandoND, "@id", DbType.Int32, itemdetalle.Id);
                                    _database.AddOutParameter(comandoND, "@Response", DbType.Int32, 11);

                                    _database.ExecuteNonQuery(comandoND, transaction);
                                    idND = Convert.ToInt32(_database.GetParameterValue(comandoND, "@Response"));
                                    if (idND == 0) throw new Exception("Error al modifcar nota de salida detalle");
                                }
                                using (var ComandoStock = _database.GetStoredProcCommand(string.Format("{0}{1}", ConectionStringRepository.EsquemaName, "SGE_STOCK_UPDATE")))
                                {
                                    _database.AddInParameter(ComandoStock, "@almac_icod_almacen", DbType.Int32, entity.almac_icod_almacen);
                                    _database.AddInParameter(ComandoStock, "@prdc_icod_producto", DbType.Int32, itemdetalle.prdc_icod_producto);
                                    _database.AddInParameter(ComandoStock, "@stocc_stock_producto", DbType.Decimal, itemdetalle.nsald_cantidad);
                                    _database.AddInParameter(ComandoStock, "@stocc_ilag_estado", DbType.Int32, entity.Estado);
                                    _database.AddInParameter(ComandoStock, "@tipo_movimiento", DbType.Int32, 2);
                                    _database.AddOutParameter(ComandoStock, "@Response", DbType.Int32, 11);

                                    _database.ExecuteNonQuery(ComandoStock, transaction);
                                    idStock = Convert.ToInt32(_database.GetParameterValue(ComandoStock, "@Response"));
                                    if (idStock == 0) throw new Exception("Error al actualizar stock");
                                }
                            }
                            else if (itemdetalle.status == add && itemdetalle.kardc_tipo_movimiento != 0)
                            {
                                using (var comandoKardex = _database.GetStoredProcCommand(string.Format("{0}{1}", ConectionStringRepository.EsquemaName, "SGE_KARDEX_INSERT")))
                                {
                                    _database.AddInParameter(comandoKardex, "@kardc_fecha_movimiento", DbType.String, entity.nsalc_fecha_nota_salida_);
                                    _database.AddInParameter(comandoKardex, "@ningc_icod_nota_ingreso", DbType.Int32, id);
                                    _database.AddInParameter(comandoKardex, "@almac_icod_almacen", DbType.Int32, entity.almac_icod_almacen);
                                    _database.AddInParameter(comandoKardex, "@prdc_icod_producto", DbType.Int32, itemdetalle.prdc_icod_producto);
                                    _database.AddInParameter(comandoKardex, "@kardc_icantidad_prod", DbType.Decimal, itemdetalle.nsald_cantidad);
                                    _database.AddInParameter(comandoKardex, "@tdocc_icod_tipo_doc", DbType.Int32, entity.tdocc_icod_tipo_doc);
                                    _database.AddInParameter(comandoKardex, "@kardc_numero_doc", DbType.String, entity.nsalc_numero_nota_salida);
                                    _database.AddInParameter(comandoKardex, "@kardc_tipo_movimiento", DbType.Int32, 2);
                                    _database.AddInParameter(comandoKardex, "@kardc_iid_motivo", DbType.Int32, entity.nsalc_iid_motivo);
                                    _database.AddInParameter(comandoKardex, "@kardc_beneficiario", DbType.String, entity.nsalc_referencia);
                                    _database.AddInParameter(comandoKardex, "@kardc_observaciones", DbType.String, entity.nsalc_observaciones);
                                    _database.AddInParameter(comandoKardex, "@kardc_monto_unitario_compra", DbType.Decimal, 0);
                                    _database.AddInParameter(comandoKardex, "@kardc_vusuario_crea", DbType.String, entity.UsuarioCreacion);
                                    _database.AddInParameter(comandoKardex, "@kardc_vpc_crea", DbType.String, WindowsIdentity.GetCurrent().Name);
                                    _database.AddInParameter(comandoKardex, "@kardc_ilag_estado", DbType.Int32, entity.Estado);
                                    _database.AddOutParameter(comandoKardex, "@Response", DbType.Int32, 11);

                                    _database.ExecuteNonQuery(comandoKardex, transaction);
                                    idKardex = Convert.ToInt32(_database.GetParameterValue(comandoKardex, "@Response"));
                                    if (idKardex == 0) throw new Exception("Error al ingresar kardex");
                                }
                                using (var comando = _database.GetStoredProcCommand(string.Format("{0}{1}", ConectionStringRepository.EsquemaName, "SGE_NOTA_SALIDA_DETALLE_INSERT")))
                                {

                                    _database.AddInParameter(comando, "@nsalc_icod_nota_salida", DbType.Int32, id);
                                    _database.AddInParameter(comando, "@nsald_nro_item", DbType.String, itemdetalle.nsald_nro_item);
                                    _database.AddInParameter(comando, "@prdc_icod_producto", DbType.Int32, itemdetalle.prdc_icod_producto);
                                    _database.AddInParameter(comando, "@nsald_cantidad", DbType.Decimal, itemdetalle.nsald_cantidad);
                                    _database.AddInParameter(comando, "@kardc_icod_correlativo", DbType.Int32, idKardex);
                                    _database.AddInParameter(comando, "@nsald_usuario_crea", DbType.String, entity.UsuarioCreacion);
                                    _database.AddInParameter(comando, "@nsald_pc_crea", DbType.String, WindowsIdentity.GetCurrent().Name);
                                    _database.AddInParameter(comando, "@nsald_ilag_estado", DbType.Int32, entity.Estado);
                                    _database.AddOutParameter(comando, "@Response", DbType.Int32, 11);

                                    _database.ExecuteNonQuery(comando, transaction);
                                    idND = Convert.ToInt32(_database.GetParameterValue(comando, "@Response"));
                                    if (idND == 0) throw new Exception("Error al ingresar nota de salida detalle");
                                }
                                using (var ComandoStock = _database.GetStoredProcCommand(string.Format("{0}{1}", ConectionStringRepository.EsquemaName, "SGE_STOCK_UPDATE")))
                                {
                                    _database.AddInParameter(ComandoStock, "@almac_icod_almacen", DbType.Int32, entity.almac_icod_almacen);
                                    _database.AddInParameter(ComandoStock, "@prdc_icod_producto", DbType.Int32, itemdetalle.prdc_icod_producto);
                                    _database.AddInParameter(ComandoStock, "@stocc_stock_producto", DbType.Decimal, itemdetalle.nsald_cantidad);
                                    _database.AddInParameter(ComandoStock, "@stocc_ilag_estado", DbType.Int32, entity.Estado);
                                    _database.AddInParameter(ComandoStock, "@tipo_movimiento", DbType.Int32, 2);
                                    _database.AddOutParameter(ComandoStock, "@Response", DbType.Int32, 11);

                                    _database.ExecuteNonQuery(ComandoStock, transaction);
                                    idStock = Convert.ToInt32(_database.GetParameterValue(ComandoStock, "@Response"));
                                    if (idStock == 0) throw new Exception("Error al actualizar stock");
                                }
                            }
                            else if (itemdetalle.status == delete && itemdetalle.kardc_tipo_movimiento == 0)
                            {
                                using (var comandokardexElimnar = _database.GetStoredProcCommand(string.Format("{0}{1}", ConectionStringRepository.EsquemaName, "SGE_KARDEX_DELETE")))
                                {
                                    _database.AddInParameter(comandokardexElimnar, "@Id", DbType.Int32, itemdetalle.kardc_icod_correlativo);
                                    _database.AddOutParameter(comandokardexElimnar, "@Response", DbType.Int32, 11);
                                    _database.ExecuteNonQuery(comandokardexElimnar, transaction);
                                    idKardex = Convert.ToInt32(_database.GetParameterValue(comandokardexElimnar, "@Response"));
                                    if (idKardex == 0) throw new Exception("Error al eliminar kardex.");
                                }
                                using (var comandonotadetalleEliminar = _database.GetStoredProcCommand(string.Format("{0}{1}", ConectionStringRepository.EsquemaName, "SGE_NOTA_SALIDA_DETALLE_DELETE")))
                                {
                                    _database.AddInParameter(comandonotadetalleEliminar, "@Id", DbType.Int32, itemdetalle.Id);
                                    _database.AddOutParameter(comandonotadetalleEliminar, "@Response", DbType.Int32, 11);
                                    _database.ExecuteNonQuery(comandonotadetalleEliminar, transaction);
                                    idND = Convert.ToInt32(_database.GetParameterValue(comandonotadetalleEliminar, "@Response"));
                                    if (idND == 0) throw new Exception("Error al eliminar Nota salida detalle.");
                                }
                                using (var ComandoStock = _database.GetStoredProcCommand(string.Format("{0}{1}", ConectionStringRepository.EsquemaName, "SGE_STOCK_UPDATE")))
                                {
                                    _database.AddInParameter(ComandoStock, "@almac_icod_almacen", DbType.Int32, entity.almac_icod_almacen);
                                    _database.AddInParameter(ComandoStock, "@prdc_icod_producto", DbType.Int32, itemdetalle.prdc_icod_producto);
                                    _database.AddInParameter(ComandoStock, "@stocc_stock_producto", DbType.Decimal, itemdetalle.nsald_cantidad);
                                    _database.AddInParameter(ComandoStock, "@stocc_ilag_estado", DbType.Int32, entity.Estado);
                                    _database.AddInParameter(ComandoStock, "@tipo_movimiento", DbType.Int32, 2);
                                    _database.AddOutParameter(ComandoStock, "@Response", DbType.Int32, 11);

                                    _database.ExecuteNonQuery(ComandoStock, transaction);
                                    idStock = Convert.ToInt32(_database.GetParameterValue(ComandoStock, "@Response"));
                                    if (idStock == 0) throw new Exception("Error al actualizar stock");
                                }
                            }

                        }

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception(ex.Message);
                    }
                }
            }
            return id;
        }
        public int DeleteNS(NotaSalida entity)
        {
            int id;
            int idKardex;
            int idNID;
            using (var comando = _database.GetStoredProcCommand(string.Format("{0}{1}", ConectionStringRepository.EsquemaName, "SGE_NOTA_SALIDA_DELETE")))
            {
                _database.AddInParameter(comando, "@Id", DbType.Int32, entity.Id);
                _database.AddOutParameter(comando, "@Response", DbType.Int32, 11);

                _database.ExecuteNonQuery(comando);
                id = Convert.ToInt32(_database.GetParameterValue(comando, "@Response"));
                if (id == 0) throw new Exception("Error al eliminar nota salida.");

            }
            using (var comando = _database.GetStoredProcCommand(string.Format("{0}{1}", ConectionStringRepository.EsquemaName, "SGE_KARDEX_DELETE_IDNI")))
            {
                _database.AddInParameter(comando, "@Id", DbType.Int32, entity.Id);
                _database.AddOutParameter(comando, "@Response", DbType.Int32, 11);

                _database.ExecuteNonQuery(comando);
                idKardex = Convert.ToInt32(_database.GetParameterValue(comando, "@Response"));
                if (idKardex == 0) throw new Exception("Error al eliminar kardex.");

            }
            using (var comando = _database.GetStoredProcCommand(string.Format("{0}{1}", ConectionStringRepository.EsquemaName, "SGE_NOTA_SALIDA_DETALLE_DELETE_IDNS")))
            {
                _database.AddInParameter(comando, "@Id", DbType.Int32, entity.Id);
                _database.AddOutParameter(comando, "@Response", DbType.Int32, 11);

                _database.ExecuteNonQuery(comando);
                idNID = Convert.ToInt32(_database.GetParameterValue(comando, "@Response"));
                if (idNID == 0) throw new Exception("Error al eliminar nota salida detalle.");

            }
            return id;
        }
        public IList<NotaSalida> GetAllPaging(PaginationParameter<int> paginationParameters)
        {
            List<NotaSalida> notasalida = new List<NotaSalida>();
            using (var comando = _database.GetStoredProcCommand(string.Format("{0}{1}", ConectionStringRepository.EsquemaName, "SGE_NOTA SALIDA_GetAllFilter")))
            {
                _database.AddInParameter(comando, "@WhereFilters", DbType.String, string.IsNullOrWhiteSpace(paginationParameters.WhereFilter) ? string.Empty : paginationParameters.WhereFilter);
                _database.AddInParameter(comando, "@OrderBy", DbType.String, string.IsNullOrWhiteSpace(paginationParameters.OrderBy) ? string.Empty : paginationParameters.OrderBy);
                _database.AddInParameter(comando, "@Start", DbType.Int32, paginationParameters.Start);
                _database.AddInParameter(comando, "@Rows", DbType.Int32, paginationParameters.AmountRows);

                using (var lector = _database.ExecuteReader(comando))
                {
                    while (lector.Read())
                    {
                        notasalida.Add(new NotaSalida
                        {
                            Id = lector.IsDBNull(lector.GetOrdinal("Id")) ? default(int) : lector.GetInt32(lector.GetOrdinal("Id")),
                            nsalc_numero_nota_salida = lector.IsDBNull(lector.GetOrdinal("nsalc_numero_nota_salida")) ? default(string) : lector.GetString(lector.GetOrdinal("nsalc_numero_nota_salida")),
                            nsalc_fecha_nota_salida = lector.IsDBNull(lector.GetOrdinal("nsalc_fecha_nota_salida")) ? default(DateTime) : lector.GetDateTime(lector.GetOrdinal("nsalc_fecha_nota_salida")),
                            nsalc_observaciones = lector.IsDBNull(lector.GetOrdinal("nsalc_observaciones")) ? default(string) : lector.GetString(lector.GetOrdinal("nsalc_observaciones")),
                            ningc_v_motivo = lector.IsDBNull(lector.GetOrdinal("motivo")) ? default(string) : lector.GetString(lector.GetOrdinal("motivo")),
                            almac_vdescripcion = lector.IsDBNull(lector.GetOrdinal("almac_vdescripcion")) ? default(string) : lector.GetString(lector.GetOrdinal("almac_vdescripcion")),
                            Estado = lector.IsDBNull(lector.GetOrdinal("nsalc_ilag_estado")) ? default(int) : lector.GetInt32(lector.GetOrdinal("nsalc_ilag_estado")),
                            Cantidad = lector.IsDBNull(lector.GetOrdinal("Cantidad")) ? default(int) : lector.GetInt32(lector.GetOrdinal("Cantidad"))
                        });
                    }
                }
            }

            return notasalida;
        }
        public NotaSalida GetById(NotaSalida entity)
        {
            NotaSalida notasalida = null;
            using (var comando = _database.GetStoredProcCommand(string.Format("{0}{1}", ConectionStringRepository.EsquemaName, "SGE_NOTA_SALIDA_GetById")))
            {
                _database.AddInParameter(comando, "@Id", DbType.Int32, entity.Id);

                using (var lector = _database.ExecuteReader(comando))
                {
                    if (lector.Read())
                    {
                        notasalida = new NotaSalida
                        {
                            Id = lector.IsDBNull(lector.GetOrdinal("nsalc_icod_nota_salida")) ? default(int) : lector.GetInt32(lector.GetOrdinal("nsalc_icod_nota_salida")),
                            nsalc_numero_nota_salida = lector.IsDBNull(lector.GetOrdinal("nsalc_numero_nota_salida")) ? default(string) : lector.GetString(lector.GetOrdinal("nsalc_numero_nota_salida")),
                            nsalc_fecha_nota_salida = lector.IsDBNull(lector.GetOrdinal("nsalc_fecha_nota_salida")) ? default(DateTime) : lector.GetDateTime(lector.GetOrdinal("nsalc_fecha_nota_salida")),
                            nsalc_observaciones = lector.IsDBNull(lector.GetOrdinal("nsalc_observaciones")) ? default(string) : lector.GetString(lector.GetOrdinal("nsalc_observaciones")),
                            nsalc_iid_motivo = lector.IsDBNull(lector.GetOrdinal("nsalc_iid_motivo")) ? default(int) : lector.GetInt32(lector.GetOrdinal("nsalc_iid_motivo")),
                            almac_icod_almacen = lector.IsDBNull(lector.GetOrdinal("almac_icod_almacen")) ? default(int) : lector.GetInt32(lector.GetOrdinal("almac_icod_almacen")),
                            Estado = lector.IsDBNull(lector.GetOrdinal("nsalc_ilag_estado")) ? default(int) : lector.GetInt32(lector.GetOrdinal("nsalc_ilag_estado"))
                        };
                    }
                }
            }

            return notasalida;
        }
        public IList<NotaSalidaDetalle> GetAll(NotaSalida entiy)
        {
            List<NotaSalidaDetalle> notaSalidaDetalle = new List<NotaSalidaDetalle>();
            using (var comando = _database.GetStoredProcCommand(string.Format("{0}{1}", ConectionStringRepository.EsquemaName, "SGE_NOTA_SALIDA_DETALLE_GetAll")))
            {
                _database.AddInParameter(comando, "@Id", DbType.Int32, entiy.Id);

                using (var lector = _database.ExecuteReader(comando))
                {
                    while (lector.Read())
                    {
                        notaSalidaDetalle.Add(new NotaSalidaDetalle
                        {
                            Id = lector.IsDBNull(lector.GetOrdinal("nsald_icod_detalle_salida")) ? default(int) : lector.GetInt32(lector.GetOrdinal("nsald_icod_detalle_salida")),
                            nsalc_icod_nota_salida = lector.IsDBNull(lector.GetOrdinal("nsalc_icod_nota_salida")) ? default(int) : lector.GetInt32(lector.GetOrdinal("nsalc_icod_nota_salida")),
                            nsald_nro_item = lector.IsDBNull(lector.GetOrdinal("nsald_nro_item")) ? default(string) : lector.GetString(lector.GetOrdinal("nsald_nro_item")),
                            dninc_v_unidad = lector.IsDBNull(lector.GetOrdinal("Unidad")) ? default(string) : lector.GetString(lector.GetOrdinal("Unidad")),
                            nsald_cantidad = lector.IsDBNull(lector.GetOrdinal("nsald_cantidad")) ? default(decimal) : lector.GetDecimal(lector.GetOrdinal("nsald_cantidad")),
                            prdc_icod_producto = lector.IsDBNull(lector.GetOrdinal("prdc_icod_producto")) ? default(int) : lector.GetInt32(lector.GetOrdinal("prdc_icod_producto")),
                            prdc_vdescripcion = lector.IsDBNull(lector.GetOrdinal("prdc_vdescripcion")) ? default(string) : lector.GetString(lector.GetOrdinal("prdc_vdescripcion")),
                            kardc_icod_correlativo = lector.IsDBNull(lector.GetOrdinal("kardc_icod_correlativo")) ? default(int) : lector.GetInt32(lector.GetOrdinal("kardc_icod_correlativo")),
                        });
                    }
                }
            }

            return notaSalidaDetalle;
        }

        public int GetCorrelativo(NotaSalida entity)
        {
            int Correlativo;
            using (var comando = _database.GetStoredProcCommand(string.Format("{0}{1}", ConectionStringRepository.EsquemaName, "SGE_NOTA_SALIDA_CORRELATIVA")))
            {
                _database.AddInParameter(comando, "@id", DbType.Int32, entity.almac_icod_almacen);
                _database.AddOutParameter(comando, "@Response", DbType.Int32, 11);

                _database.ExecuteNonQuery(comando);
                Correlativo = Convert.ToInt32(_database.GetParameterValue(comando, "@Response"));
            }

            return Correlativo;
        }
    }
}
