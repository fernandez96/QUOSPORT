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
using System.Globalization;
namespace Base.DataAccess
{
    public class TransferenciaRepository: Singleton<TransferenciaRepository>, ITransferenciaRepository<Transferencia,TransferenciaDetalle, int>
    {
        #region Attributos

        private readonly Database _database = new DatabaseProviderFactory().Create(ConectionStringRepository.ConnectionStringNameSQL);
        public const int add = 1;
        public const int update = 2;
        public const int delete = 3;
        public int IdCorrelativo_kardex_Ing = 0;
        public int IdCorrelativo_kardex_sal = 0;
        public int length = 2;
        #endregion

        public int Add(Transferencia entity)
        {
            int id;
            int idND;
            int idStock;

            using (DbConnection conexion = _database.CreateConnection())
            {
                conexion.Open();
                using (var transaction = conexion.BeginTransaction(IsolationLevel.Serializable))
                {
                    try
                    {
                        using (var comando = _database.GetStoredProcCommand(string.Format("{0}{1}", ConectionStringRepository.EsquemaName, "SGE_TRANSFERENCIA_INSERT")))
                        {
                            _database.AddInParameter(comando, "@trfc_inum_transf", DbType.String, entity.trfc_inum_transf);
                            _database.AddInParameter(comando, "@almac_icod_alm_sal", DbType.Int32, entity.almac_icod_alm_sal);
                            _database.AddInParameter(comando, "@almac_icod_alm_ing", DbType.Int32, entity.almac_icod_alm_ing);
                            _database.AddInParameter(comando, "@trfc_sfecha_transf", DbType.String, DateTime.ParseExact(entity.trfc_sfecha_transf_, "dd/mm/yyyy", CultureInfo.InvariantCulture).ToString("yyyy/mm/dd"));
                            _database.AddInParameter(comando, "@trnfc_iid_motivo", DbType.Int32, entity.trnfc_iid_motivo);
                            _database.AddInParameter(comando, "@trnfc_vobservaciones", DbType.String, entity.trnfc_vobservaciones);
                            _database.AddInParameter(comando, "@trfc_vusuario_crea", DbType.String, entity.UsuarioCreacion);
                            _database.AddInParameter(comando, "@trfc_vpc_crea", DbType.String, WindowsIdentity.GetCurrent().Name);
                            _database.AddInParameter(comando, "@trfc_flag_estado", DbType.Int32, entity.Estado);
                            _database.AddOutParameter(comando, "@Response", DbType.Int32, 11);


                            _database.ExecuteNonQuery(comando, transaction);
                            id = Convert.ToInt32(_database.GetParameterValue(comando, "@Response"));
                            if (id == 0) throw new Exception("Error al ingresar transferencia");
                        }
                        //Insertacion de de kardex almacen(1,2)
                        foreach (var itemdetalle in entity.transferenciaDetalle)
                        {
                            //Insertar kardex alamcen salida
                            using (var comandoKardex = _database.GetStoredProcCommand(string.Format("{0}{1}", ConectionStringRepository.EsquemaName, "SGE_KARDEX_INSERT")))
                            {
                                _database.AddInParameter(comandoKardex, "@kardc_fecha_movimiento", DbType.String, DateTime.ParseExact(entity.trfc_sfecha_transf_, "dd/mm/yyyy", CultureInfo.InvariantCulture).ToString("yyyy/mm/dd"));
                                _database.AddInParameter(comandoKardex, "@ningc_icod_nota_ingreso", DbType.Int32, id);
                                _database.AddInParameter(comandoKardex, "@almac_icod_almacen", DbType.Int32, entity.almac_icod_alm_sal);
                                _database.AddInParameter(comandoKardex, "@prdc_icod_producto", DbType.Int32, itemdetalle.prdc_icod_producto);
                                _database.AddInParameter(comandoKardex, "@kardc_icantidad_prod", DbType.Decimal, itemdetalle.trfd_ncantidad);
                                _database.AddInParameter(comandoKardex, "@tdocc_icod_tipo_doc", DbType.Int32, 2);
                                _database.AddInParameter(comandoKardex, "@kardc_numero_doc", DbType.String, entity.trfc_inum_transf);
                                _database.AddInParameter(comandoKardex, "@kardc_tipo_movimiento", DbType.Int32, 2);
                                _database.AddInParameter(comandoKardex, "@kardc_monto_unitario_compra", DbType.Decimal, 0);
                                _database.AddInParameter(comandoKardex, "@kardc_iid_motivo", DbType.Int32, entity.trnfc_iid_motivo);
                                _database.AddInParameter(comandoKardex, "@kardc_beneficiario", DbType.String, "");
                                _database.AddInParameter(comandoKardex, "@kardc_observaciones", DbType.String, entity.trnfc_vobservaciones);
                                _database.AddInParameter(comandoKardex, "@kardc_vusuario_crea", DbType.String, entity.UsuarioCreacion);
                                _database.AddInParameter(comandoKardex, "@kardc_vpc_crea", DbType.String, WindowsIdentity.GetCurrent().Name);
                                _database.AddInParameter(comandoKardex, "@kardc_ilag_estado", DbType.Int32, entity.Estado);
                                _database.AddOutParameter(comandoKardex, "@Response", DbType.Int32, 11);

                                _database.ExecuteNonQuery(comandoKardex, transaction);
                                IdCorrelativo_kardex_sal = Convert.ToInt32(_database.GetParameterValue(comandoKardex, "@Response"));
                                if (IdCorrelativo_kardex_sal == 0) throw new Exception("Error al ingresar kardex");
                            }

                            //Insertar kardex alamcen Ingreso
                            using (var comandoKardex = _database.GetStoredProcCommand(string.Format("{0}{1}", ConectionStringRepository.EsquemaName, "SGE_KARDEX_INSERT")))
                            {
                                _database.AddInParameter(comandoKardex, "@kardc_fecha_movimiento", DbType.String, DateTime.ParseExact(entity.trfc_sfecha_transf_, "dd/mm/yyyy", CultureInfo.InvariantCulture).ToString("yyyy/mm/dd"));
                                _database.AddInParameter(comandoKardex, "@ningc_icod_nota_ingreso", DbType.Int32, id);
                                _database.AddInParameter(comandoKardex, "@almac_icod_almacen", DbType.Int32, entity.almac_icod_alm_ing);
                                _database.AddInParameter(comandoKardex, "@prdc_icod_producto", DbType.Int32, itemdetalle.prdc_icod_producto);
                                _database.AddInParameter(comandoKardex, "@kardc_icantidad_prod", DbType.Decimal, itemdetalle.trfd_ncantidad);
                                _database.AddInParameter(comandoKardex, "@tdocc_icod_tipo_doc", DbType.Int32, 1);
                                _database.AddInParameter(comandoKardex, "@kardc_numero_doc", DbType.String, entity.trfc_inum_transf);
                                _database.AddInParameter(comandoKardex, "@kardc_tipo_movimiento", DbType.Int32, 1);
                                _database.AddInParameter(comandoKardex, "@kardc_monto_unitario_compra", DbType.Decimal, 0);
                                _database.AddInParameter(comandoKardex, "@kardc_iid_motivo", DbType.Int32, entity.trnfc_iid_motivo);
                                _database.AddInParameter(comandoKardex, "@kardc_beneficiario", DbType.String, "");
                                _database.AddInParameter(comandoKardex, "@kardc_observaciones", DbType.String, entity.trnfc_vobservaciones);
                                _database.AddInParameter(comandoKardex, "@kardc_vusuario_crea", DbType.String, entity.UsuarioCreacion);
                                _database.AddInParameter(comandoKardex, "@kardc_vpc_crea", DbType.String, WindowsIdentity.GetCurrent().Name);
                                _database.AddInParameter(comandoKardex, "@kardc_ilag_estado", DbType.Int32, entity.Estado);
                                _database.AddOutParameter(comandoKardex, "@Response", DbType.Int32, 11);

                                _database.ExecuteNonQuery(comandoKardex, transaction);
                                IdCorrelativo_kardex_Ing = Convert.ToInt32(_database.GetParameterValue(comandoKardex, "@Response"));
                                if (IdCorrelativo_kardex_Ing == 0) throw new Exception("Error al ingresar kardex");
                            }
                            //Insertar transferencia detalle
                            using (var comando = _database.GetStoredProcCommand(string.Format("{0}{1}", ConectionStringRepository.EsquemaName, "SGE_TRANSFERENCIA_DET_INSERT")))
                            {
                                _database.AddInParameter(comando, "@trfc_icod_transf", DbType.Int32, id);
                                _database.AddInParameter(comando, "@trfd_nro_item", DbType.String, itemdetalle.trfd_nro_item);
                                _database.AddInParameter(comando, "@prdc_icod_producto", DbType.Int32, itemdetalle.prdc_icod_producto);
                                _database.AddInParameter(comando, "@trfd_ncantidad", DbType.Decimal, itemdetalle.trfd_ncantidad);
                                _database.AddInParameter(comando, "@kardc_icod_correlativo_sal", DbType.Int32, IdCorrelativo_kardex_sal);
                                _database.AddInParameter(comando, "@kardc_icod_correlativo_ing", DbType.Int32, IdCorrelativo_kardex_Ing);
                                _database.AddInParameter(comando, "@trfd_iusuario_crea", DbType.String, entity.UsuarioCreacion);
                                _database.AddInParameter(comando, "@trfd_vpc_crea", DbType.String, WindowsIdentity.GetCurrent().Name);
                                _database.AddInParameter(comando, "@trfd_flag_estado", DbType.Int32, entity.Estado);
                                _database.AddOutParameter(comando, "@Response", DbType.Int32, 11);

                                _database.ExecuteNonQuery(comando, transaction);
                                idND = Convert.ToInt32(_database.GetParameterValue(comando, "@Response"));
                                if (idND == 0) throw new Exception("Error al ingresar transferencia detalle");
                            }

                            //Actualizar estock alamcen salida
                            using (var ComandoStock = _database.GetStoredProcCommand(string.Format("{0}{1}", ConectionStringRepository.EsquemaName, "SGE_STOCK_UPDATE")))
                            {
                                _database.AddInParameter(ComandoStock, "@almac_icod_almacen", DbType.Int32, entity.almac_icod_alm_sal);
                                _database.AddInParameter(ComandoStock, "@prdc_icod_producto", DbType.Int32, itemdetalle.prdc_icod_producto);
                                _database.AddInParameter(ComandoStock, "@stocc_stock_producto", DbType.Decimal, itemdetalle.trfd_ncantidad);
                                _database.AddInParameter(ComandoStock, "@stocc_ilag_estado", DbType.Int32, entity.Estado);
                                _database.AddInParameter(ComandoStock, "@tipo_movimiento", DbType.Int32, 2);
                                _database.AddOutParameter(ComandoStock, "@Response", DbType.Int32, 11);

                                _database.ExecuteNonQuery(ComandoStock, transaction);
                                idStock = Convert.ToInt32(_database.GetParameterValue(ComandoStock, "@Response"));
                                if (idStock == 0) throw new Exception("Error al ingresar stock");
                            }

                            //Actualizar estock alamcen Ingreso
                            using (var ComandoStock = _database.GetStoredProcCommand(string.Format("{0}{1}", ConectionStringRepository.EsquemaName, "SGE_STOCK_UPDATE")))
                            {
                                _database.AddInParameter(ComandoStock, "@almac_icod_almacen", DbType.Int32, entity.almac_icod_alm_ing);
                                _database.AddInParameter(ComandoStock, "@prdc_icod_producto", DbType.Int32, itemdetalle.prdc_icod_producto);
                                _database.AddInParameter(ComandoStock, "@stocc_stock_producto", DbType.Decimal, itemdetalle.trfd_ncantidad);
                                _database.AddInParameter(ComandoStock, "@stocc_ilag_estado", DbType.Int32, entity.Estado);
                                _database.AddInParameter(ComandoStock, "@tipo_movimiento", DbType.Int32,1);
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
        public int Update(Transferencia entity)
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
                        using (var comando = _database.GetStoredProcCommand(string.Format("{0}{1}", ConectionStringRepository.EsquemaName, "SGE_TRANSFERENCIA_UPDATE")))
                        {
                            _database.AddInParameter(comando, "@trfc_inum_transf", DbType.String, entity.trfc_inum_transf);
                            _database.AddInParameter(comando, "@almac_icod_alm_sal", DbType.String, entity.almac_icod_alm_sal);
                            _database.AddInParameter(comando, "@almac_icod_alm_ing", DbType.String, entity.almac_icod_alm_ing);
                            _database.AddInParameter(comando, "@trfc_sfecha_transf", DbType.String, DateTime.ParseExact(entity.trfc_sfecha_transf_, "dd/mm/yyyy", CultureInfo.InvariantCulture).ToString("yyyy/mm/dd"));
                            _database.AddInParameter(comando, "@trnfc_iid_motivo", DbType.Int32, entity.trnfc_iid_motivo);
                            _database.AddInParameter(comando, "@trnfc_vobservaciones", DbType.String, entity.trnfc_vobservaciones);
                            _database.AddInParameter(comando, "@trfc_vusuario_modifica", DbType.String, entity.UsuarioModificacion);
                            _database.AddInParameter(comando, "@trfc_vpc_modifica", DbType.String, WindowsIdentity.GetCurrent().Name);
                            _database.AddInParameter(comando, "@trfc_flag_estado", DbType.Int32, 1);
                            _database.AddInParameter(comando, "@id", DbType.Int32, entity.Id);
                            _database.AddOutParameter(comando, "@Response", DbType.Int32, 11);

                            _database.ExecuteNonQuery(comando, transaction);
                            id = Convert.ToInt32(_database.GetParameterValue(comando, "@Response"));
                            if (id == -1) throw new Exception("Error al modificar Transferencia");
                        }
                        foreach (var itemdetalle in entity.transferenciaDetalle)
                        {
                            if (itemdetalle.status == update && itemdetalle.kardc_tipo_movimiento == 0)
                            {
                                //actualizar alamcen de salida
                                using (var comandoKardex = _database.GetStoredProcCommand(string.Format("{0}{1}", ConectionStringRepository.EsquemaName, "SGE_KARDEX_UPDATE")))
                                {
                                    _database.AddInParameter(comandoKardex, "@kardc_fecha_movimiento", DbType.String, DateTime.ParseExact(entity.trfc_sfecha_transf_, "dd/mm/yyyy", CultureInfo.InvariantCulture).ToString("yyyy/mm/dd"));
                                    _database.AddInParameter(comandoKardex, "@ningc_icod_nota_ingreso", DbType.Int32, id);
                                    _database.AddInParameter(comandoKardex, "@almac_icod_almacen", DbType.Int32, entity.almac_icod_alm_sal);
                                    _database.AddInParameter(comandoKardex, "@prdc_icod_producto", DbType.Int32, itemdetalle.prdc_icod_producto);
                                    _database.AddInParameter(comandoKardex, "@kardc_icantidad_prod", DbType.Decimal, itemdetalle.trfd_ncantidad);
                                    _database.AddInParameter(comandoKardex, "@tdocc_icod_tipo_doc", DbType.Int32, 0);
                                    _database.AddInParameter(comandoKardex, "@kardc_numero_doc", DbType.String, entity.trfc_inum_transf);
                                    _database.AddInParameter(comandoKardex, "@kardc_tipo_movimiento", DbType.Int32, 2);
                                    _database.AddInParameter(comandoKardex, "@kardc_iid_motivo", DbType.Int32, entity.trnfc_iid_motivo);
                                    _database.AddInParameter(comandoKardex, "@kardc_beneficiario", DbType.String, "");
                                    _database.AddInParameter(comandoKardex, "@kardc_observaciones", DbType.String, entity.trnfc_vobservaciones);
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

                                //actualizar almacen de ingreso
                                using (var comandoKardex = _database.GetStoredProcCommand(string.Format("{0}{1}", ConectionStringRepository.EsquemaName, "SGE_KARDEX_UPDATE")))
                                {
                                    _database.AddInParameter(comandoKardex, "@kardc_fecha_movimiento", DbType.String, DateTime.ParseExact(entity.trfc_sfecha_transf_, "dd/mm/yyyy", CultureInfo.InvariantCulture).ToString("yyyy/mm/dd"));
                                    _database.AddInParameter(comandoKardex, "@ningc_icod_nota_ingreso", DbType.Int32, id);
                                    _database.AddInParameter(comandoKardex, "@almac_icod_almacen", DbType.Int32, entity.almac_icod_alm_ing);
                                    _database.AddInParameter(comandoKardex, "@prdc_icod_producto", DbType.Int32, itemdetalle.prdc_icod_producto);
                                    _database.AddInParameter(comandoKardex, "@kardc_icantidad_prod", DbType.Decimal, itemdetalle.trfd_ncantidad);
                                    _database.AddInParameter(comandoKardex, "@tdocc_icod_tipo_doc", DbType.Int32, 0);
                                    _database.AddInParameter(comandoKardex, "@kardc_numero_doc", DbType.String, entity.trfc_inum_transf);
                                    _database.AddInParameter(comandoKardex, "@kardc_tipo_movimiento", DbType.Int32, 2);
                                    _database.AddInParameter(comandoKardex, "@kardc_iid_motivo", DbType.Int32, entity.trnfc_iid_motivo);
                                    _database.AddInParameter(comandoKardex, "@kardc_beneficiario", DbType.String, "");
                                    _database.AddInParameter(comandoKardex, "@kardc_observaciones", DbType.String, entity.trnfc_vobservaciones);
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


                                using (var comandoND = _database.GetStoredProcCommand(string.Format("{0}{1}", ConectionStringRepository.EsquemaName, "SGE_TRANSFERENCIA_DET_UPDATE")))
                                {
                                    _database.AddInParameter(comandoND, "@trfc_icod_transf", DbType.Int32, id);
                                    _database.AddInParameter(comandoND, "@trfd_nro_item", DbType.String, itemdetalle.trfd_nro_item);
                                    _database.AddInParameter(comandoND, "@prdc_icod_producto", DbType.Int32, itemdetalle.prdc_icod_producto);
                                    _database.AddInParameter(comandoND, "@trfd_ncantidad", DbType.Decimal, itemdetalle.trfd_ncantidad);
                                    _database.AddInParameter(comandoND, "@kardc_icod_correlativo_sal", DbType.Int32, idKardex);
                                    _database.AddInParameter(comandoND, "@kardc_icod_correlativo_ing", DbType.Int32, idKardex);
                                    _database.AddInParameter(comandoND, "@trfd_iusuario_modifica", DbType.String, entity.UsuarioCreacion);
                                    _database.AddInParameter(comandoND, "@trfd_vpc_modifica", DbType.String, WindowsIdentity.GetCurrent().Name);
                                    _database.AddInParameter(comandoND, "@trfd_flag_estado", DbType.Int32, entity.Estado);
                                    _database.AddInParameter(comandoND, "@id", DbType.Int32, itemdetalle.Id);
                                    _database.AddOutParameter(comandoND, "@Response", DbType.Int32, 11);

                                    _database.ExecuteNonQuery(comandoND, transaction);
                                    idND = Convert.ToInt32(_database.GetParameterValue(comandoND, "@Response"));
                                    if (idND == 0) throw new Exception("Error al modifcar Transferencia detalle");
                                }
                                using (var ComandoStock = _database.GetStoredProcCommand(string.Format("{0}{1}", ConectionStringRepository.EsquemaName, "SGE_STOCK_UPDATE")))
                                {
                                    _database.AddInParameter(ComandoStock, "@almac_icod_almacen", DbType.Int32, entity.almac_icod_alm_ing);
                                    _database.AddInParameter(ComandoStock, "@prdc_icod_producto", DbType.Int32, itemdetalle.prdc_icod_producto);
                                    _database.AddInParameter(ComandoStock, "@stocc_stock_producto", DbType.Decimal, itemdetalle.trfd_ncantidad);
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
                                    _database.AddInParameter(comandoKardex, "@kardc_fecha_movimiento", DbType.String, DateTime.ParseExact(entity.trfc_sfecha_transf_, "dd/mm/yyyy", CultureInfo.InvariantCulture).ToString("yyyy/mm/dd"));
                                    _database.AddInParameter(comandoKardex, "@ningc_icod_nota_ingreso", DbType.Int32, id);
                                    _database.AddInParameter(comandoKardex, "@almac_icod_almacen", DbType.Int32, entity.almac_icod_alm_ing);
                                    _database.AddInParameter(comandoKardex, "@prdc_icod_producto", DbType.Int32, itemdetalle.prdc_icod_producto);
                                    _database.AddInParameter(comandoKardex, "@kardc_icantidad_prod", DbType.Decimal, itemdetalle.trfd_ncantidad);
                                    _database.AddInParameter(comandoKardex, "@tdocc_icod_tipo_doc", DbType.Int32, 0);
                                    _database.AddInParameter(comandoKardex, "@kardc_numero_doc", DbType.String, entity.trfc_inum_transf);
                                    _database.AddInParameter(comandoKardex, "@kardc_tipo_movimiento", DbType.Int32, 2);
                                    _database.AddInParameter(comandoKardex, "@kardc_iid_motivo", DbType.Int32, entity.trnfc_iid_motivo);
                                    _database.AddInParameter(comandoKardex, "@kardc_beneficiario", DbType.String, "");
                                    _database.AddInParameter(comandoKardex, "@kardc_observaciones", DbType.String, entity.trnfc_vobservaciones);
                                    _database.AddInParameter(comandoKardex, "@kardc_monto_unitario_compra", DbType.Decimal, 0);
                                    _database.AddInParameter(comandoKardex, "@kardc_vusuario_crea", DbType.String, entity.UsuarioCreacion);
                                    _database.AddInParameter(comandoKardex, "@kardc_vpc_crea", DbType.String, WindowsIdentity.GetCurrent().Name);
                                    _database.AddInParameter(comandoKardex, "@kardc_ilag_estado", DbType.Int32, entity.Estado);
                                    _database.AddOutParameter(comandoKardex, "@Response", DbType.Int32, 11);

                                    _database.ExecuteNonQuery(comandoKardex, transaction);
                                    idKardex = Convert.ToInt32(_database.GetParameterValue(comandoKardex, "@Response"));
                                    if (idKardex == 0) throw new Exception("Error al ingresar kardex");
                                }
                                using (var comando = _database.GetStoredProcCommand(string.Format("{0}{1}", ConectionStringRepository.EsquemaName, "SGE_TRANSFERENCIA_DET_INSERT")))
                                {

                                    _database.AddInParameter(comando, "@trfc_icod_transf", DbType.Int32, id);
                                    _database.AddInParameter(comando, "@trfd_nro_item", DbType.String, itemdetalle.trfd_nro_item);
                                    _database.AddInParameter(comando, "@prdc_icod_producto", DbType.Int32, itemdetalle.prdc_icod_producto);
                                    _database.AddInParameter(comando, "@trfd_ncantidad", DbType.Decimal, itemdetalle.trfd_ncantidad);
                                    _database.AddInParameter(comando, "@kardc_icod_correlativo_sal", DbType.Int32, idKardex);
                                    _database.AddInParameter(comando, "@kardc_icod_correlativo_ing", DbType.Int32, idKardex);
                                    _database.AddInParameter(comando, "@trfd_iusuario_crea", DbType.String, entity.UsuarioCreacion);
                                    _database.AddInParameter(comando, "@trfd_vpc_crea", DbType.String, WindowsIdentity.GetCurrent().Name);
                                    _database.AddInParameter(comando, "@trfd_flag_estado", DbType.Int32, entity.Estado);
                                    _database.AddOutParameter(comando, "@Response", DbType.Int32, 11);

                                    _database.ExecuteNonQuery(comando, transaction);
                                    idND = Convert.ToInt32(_database.GetParameterValue(comando, "@Response"));
                                    if (idND == 0) throw new Exception("Error al ingresar transferencia detalle");
                                }
                                using (var ComandoStock = _database.GetStoredProcCommand(string.Format("{0}{1}", ConectionStringRepository.EsquemaName, "SGE_STOCK_UPDATE")))
                                {
                                    _database.AddInParameter(ComandoStock, "@almac_icod_almacen", DbType.Int32, entity.almac_icod_alm_ing);
                                    _database.AddInParameter(ComandoStock, "@prdc_icod_producto", DbType.Int32, itemdetalle.prdc_icod_producto);
                                    _database.AddInParameter(ComandoStock, "@stocc_stock_producto", DbType.Decimal, itemdetalle.trfd_ncantidad);
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
                                using (var comandonotadetalleEliminar = _database.GetStoredProcCommand(string.Format("{0}{1}", ConectionStringRepository.EsquemaName, "SGE_TRANSFERENCIA_DET_DELETE")))
                                {
                                    _database.AddInParameter(comandonotadetalleEliminar, "@Id", DbType.Int32, itemdetalle.Id);
                                    _database.AddOutParameter(comandonotadetalleEliminar, "@Response", DbType.Int32, 11);
                                    _database.ExecuteNonQuery(comandonotadetalleEliminar, transaction);
                                    idND = Convert.ToInt32(_database.GetParameterValue(comandonotadetalleEliminar, "@Response"));
                                    if (idND == 0) throw new Exception("Error al eliminar Transferencia detalle.");
                                }
                                using (var ComandoStock = _database.GetStoredProcCommand(string.Format("{0}{1}", ConectionStringRepository.EsquemaName, "SGE_STOCK_UPDATE")))
                                {
                                    _database.AddInParameter(ComandoStock, "@almac_icod_almacen", DbType.Int32, entity.almac_icod_alm_ing);
                                    _database.AddInParameter(ComandoStock, "@prdc_icod_producto", DbType.Int32, itemdetalle.prdc_icod_producto);
                                    _database.AddInParameter(ComandoStock, "@stocc_stock_producto", DbType.Decimal, itemdetalle.trfd_ncantidad);
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
        public int Delete(Transferencia entity)
        {
            int id;
            int idKardex;
            int idNID;
            using (var comando = _database.GetStoredProcCommand(string.Format("{0}{1}", ConectionStringRepository.EsquemaName, "SGE_TRANSFERENCIA_DELETE")))
            {
                _database.AddInParameter(comando, "@Id", DbType.Int32, entity.Id);
                _database.AddOutParameter(comando, "@Response", DbType.Int32, 11);

                _database.ExecuteNonQuery(comando);
                id = Convert.ToInt32(_database.GetParameterValue(comando, "@Response"));
                if (id == 0) throw new Exception("Error al eliminar transferencia.");

            }
            using (var comando = _database.GetStoredProcCommand(string.Format("{0}{1}", ConectionStringRepository.EsquemaName, "SGE_KARDEX_DELETE_IDNI")))
            {
                _database.AddInParameter(comando, "@Id", DbType.Int32, entity.Id);
                _database.AddOutParameter(comando, "@Response", DbType.Int32, 11);

                _database.ExecuteNonQuery(comando);
                idKardex = Convert.ToInt32(_database.GetParameterValue(comando, "@Response"));
                if (idKardex == 0) throw new Exception("Error al eliminar kardex.");

            }
            using (var comando = _database.GetStoredProcCommand(string.Format("{0}{1}", ConectionStringRepository.EsquemaName, "SGE_TRANSFERENCIA_DET_DELETE_TC")))
            {
                _database.AddInParameter(comando, "@Id", DbType.Int32, entity.Id);
                _database.AddOutParameter(comando, "@Response", DbType.Int32, 11);

                _database.ExecuteNonQuery(comando);
                idNID = Convert.ToInt32(_database.GetParameterValue(comando, "@Response"));
                if (idNID == 0) throw new Exception("Error al eliminar transferencia detalle.");

            }
            return id;
        }
        public IList<Transferencia> GetAllPaging(PaginationParameter<int> paginationParameters)
        {
            List<Transferencia> transferencia = new List<Transferencia>();
            using (var comando = _database.GetStoredProcCommand(string.Format("{0}{1}", ConectionStringRepository.EsquemaName, "SGE_TRANSFERENCIA_GetAllFilter")))
            {
                _database.AddInParameter(comando, "@WhereFilters", DbType.String, string.IsNullOrWhiteSpace(paginationParameters.WhereFilter) ? string.Empty : paginationParameters.WhereFilter);
                _database.AddInParameter(comando, "@OrderBy", DbType.String, string.IsNullOrWhiteSpace(paginationParameters.OrderBy) ? string.Empty : paginationParameters.OrderBy);
                _database.AddInParameter(comando, "@Start", DbType.Int32, paginationParameters.Start);
                _database.AddInParameter(comando, "@Rows", DbType.Int32, paginationParameters.AmountRows);

                using (var lector = _database.ExecuteReader(comando))
                {
                    while (lector.Read())
                    {
                        transferencia.Add(new Transferencia
                        {
                            Id = lector.IsDBNull(lector.GetOrdinal("Id")) ? default(int) : lector.GetInt32(lector.GetOrdinal("Id")),
                            trfc_inum_transf = lector.IsDBNull(lector.GetOrdinal("trfc_inum_transf")) ? default(string) : lector.GetString(lector.GetOrdinal("trfc_inum_transf")),
                            trfc_sfecha_transf = lector.IsDBNull(lector.GetOrdinal("trfc_sfecha_transf")) ? default(DateTime) : lector.GetDateTime(lector.GetOrdinal("trfc_sfecha_transf")),
                            trnfc_vobservaciones = lector.IsDBNull(lector.GetOrdinal("trnfc_vobservaciones")) ? default(string) : lector.GetString(lector.GetOrdinal("trnfc_vobservaciones")),
                            trnfc_v_motivo = lector.IsDBNull(lector.GetOrdinal("motivo")) ? default(string) : lector.GetString(lector.GetOrdinal("motivo")),
                            almac_v_alm_sal = lector.IsDBNull(lector.GetOrdinal("almacenSalida")) ? default(string) : lector.GetString(lector.GetOrdinal("almacenSalida")),
                            almac_v_alm_ing = lector.IsDBNull(lector.GetOrdinal("almacenIngreso")) ? default(string) : lector.GetString(lector.GetOrdinal("almacenIngreso")),
                            Estado = lector.IsDBNull(lector.GetOrdinal("trfc_flag_estado")) ? default(int) : lector.GetInt32(lector.GetOrdinal("trfc_flag_estado")),
                            Cantidad = lector.IsDBNull(lector.GetOrdinal("Cantidad")) ? default(int) : lector.GetInt32(lector.GetOrdinal("Cantidad"))
                        });
                    }
                }
            }

            return transferencia;
        }
        public Transferencia GetById(Transferencia entity)
        {
            Transferencia transferencia = null;
            using (var comando = _database.GetStoredProcCommand(string.Format("{0}{1}", ConectionStringRepository.EsquemaName, "SGE_TRANSFERENCIA_GetById")))
            {
                _database.AddInParameter(comando, "@Id", DbType.Int32, entity.Id);

                using (var lector = _database.ExecuteReader(comando))
                {
                    if (lector.Read())
                    {
                        transferencia = new Transferencia
                        {
                            Id = lector.IsDBNull(lector.GetOrdinal("trfc_icod_transf")) ? default(int) : lector.GetInt32(lector.GetOrdinal("trfc_icod_transf")),
                            trfc_inum_transf = lector.IsDBNull(lector.GetOrdinal("trfc_inum_transf")) ? default(string) : lector.GetString(lector.GetOrdinal("trfc_inum_transf")),
                            trfc_sfecha_transf = lector.IsDBNull(lector.GetOrdinal("trfc_sfecha_transf")) ? default(DateTime) : lector.GetDateTime(lector.GetOrdinal("trfc_sfecha_transf")),
                            trnfc_vobservaciones = lector.IsDBNull(lector.GetOrdinal("trnfc_vobservaciones")) ? default(string) : lector.GetString(lector.GetOrdinal("trnfc_vobservaciones")),
                            trnfc_iid_motivo = lector.IsDBNull(lector.GetOrdinal("trnfc_iid_motivo")) ? default(int) : lector.GetInt32(lector.GetOrdinal("trnfc_iid_motivo")),
                            almac_icod_alm_sal = lector.IsDBNull(lector.GetOrdinal("almac_icod_alm_sal")) ? default(int) : lector.GetInt32(lector.GetOrdinal("almac_icod_alm_sal")),
                            almac_icod_alm_ing = lector.IsDBNull(lector.GetOrdinal("almac_icod_alm_ing")) ? default(int) : lector.GetInt32(lector.GetOrdinal("almac_icod_alm_ing")),
                            Estado = lector.IsDBNull(lector.GetOrdinal("trfc_flag_estado")) ? default(int) : lector.GetInt32(lector.GetOrdinal("trfc_flag_estado"))
                        };
                    }
                }
            }

            return transferencia;
        }
        public IList<TransferenciaDetalle> GetAll(Transferencia entiy)
        {
            List<TransferenciaDetalle> transferenciaDetalle = new List<TransferenciaDetalle>();
            using (var comando = _database.GetStoredProcCommand(string.Format("{0}{1}", ConectionStringRepository.EsquemaName, "SGE_TRANSFERENCIA_ALMACEN_DET_GetAll")))
            {
                _database.AddInParameter(comando, "@Id", DbType.Int32, entiy.Id);

                using (var lector = _database.ExecuteReader(comando))
                {
                    while (lector.Read())
                    {
                        transferenciaDetalle.Add(new TransferenciaDetalle
                        {
                            Id = lector.IsDBNull(lector.GetOrdinal("trfd_icod_detalle_transf")) ? default(int) : lector.GetInt32(lector.GetOrdinal("trfd_icod_detalle_transf")),
                            trfc_icod_transf = lector.IsDBNull(lector.GetOrdinal("trfc_icod_transf")) ? default(int) : lector.GetInt32(lector.GetOrdinal("trfc_icod_transf")),
                            trfd_nro_item = lector.IsDBNull(lector.GetOrdinal("trfd_nro_item")) ? default(string) : lector.GetString(lector.GetOrdinal("trfd_nro_item")),
                            trfd_v_unidad = lector.IsDBNull(lector.GetOrdinal("Unidad")) ? default(string) : lector.GetString(lector.GetOrdinal("Unidad")),
                            trfd_ncantidad = lector.IsDBNull(lector.GetOrdinal("trfd_ncantidad")) ? default(decimal) : lector.GetDecimal(lector.GetOrdinal("trfd_ncantidad")),
                            prdc_icod_producto = lector.IsDBNull(lector.GetOrdinal("prdc_icod_producto")) ? default(int) : lector.GetInt32(lector.GetOrdinal("prdc_icod_producto")),
                            prdc_vdescripcion = lector.IsDBNull(lector.GetOrdinal("prdc_vdescripcion")) ? default(string) : lector.GetString(lector.GetOrdinal("prdc_vdescripcion")),
                            kardc_icod_correlativo_sal = lector.IsDBNull(lector.GetOrdinal("kardc_icod_correlativo_sal")) ? default(int) : lector.GetInt32(lector.GetOrdinal("kardc_icod_correlativo_sal")),
                            kardc_icod_correlativo_ing = lector.IsDBNull(lector.GetOrdinal("kardc_icod_correlativo_sal")) ? default(int) : lector.GetInt32(lector.GetOrdinal("kardc_icod_correlativo_sal")),
                        });
                    }
                }
            }

            return transferenciaDetalle;
        }

        public int GetCorrelativo(Transferencia entity)
        {
            int Correlativo;
            using (var comando = _database.GetStoredProcCommand(string.Format("{0}{1}", ConectionStringRepository.EsquemaName, "SGE_NOTA_SALIDA_CORRELATIVA")))
            {
                _database.AddOutParameter(comando, "@Response", DbType.Int32, 11);

                _database.ExecuteNonQuery(comando);
                Correlativo = Convert.ToInt32(_database.GetParameterValue(comando, "@Response"));
            }

            return Correlativo;
        }
    }
}
