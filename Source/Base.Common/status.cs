namespace Base.Common
{
    public class status
    {
        #region Producto
        public static string TablaProducto = "Base.SGE_PRODUCTO";
        public static string setStatusProducto = "SET prdc_bflag_estado=";
        public static string WhereProducto = "WHERE prdc_iid_producto=";
        #endregion Producto

        #region Almacen
        public static string TablaAlmacen = "Base.SGE_ALMACEN";
        public static string setStatusAlmacen = "SET almac_bflag_estado=";
        public static string WhereAlmacen = "WHERE almac_iid_almacen=";
        #endregion Almacen

        #region Unidad Medida
        public static string TablaUnidadMedida = "Base.SGE_UNIDAD_MEDIDA";
        public static string setStatusUnidadMedida = "SET umec_bflag_estado=";
        public static string WhereUnidadMedida = "WHERE umec_iid_unidad_medida=";
        #endregion Unidad Medida

        #region Categoria
        public static string TablaCategoria = "Base.SGE_CATEGORIAS_PRODUCTO";
        public static string setStatusCategoria = "SET ctgcc_iflag_estado=";
        public static string WhereCategoria = "WHERE ctgcc_iid_categoria=";
        #endregion
        #region Transportista
        public static string TablaTransportista = "Base.SGE_TRANSPORTISTA";
        public static string setStatusTransportista = "SET tranc_flag_estado=";
        public static string WhereTransportista = "WHERE tranc_icod_transportista=";
        #endregion Almacen
    }
}
