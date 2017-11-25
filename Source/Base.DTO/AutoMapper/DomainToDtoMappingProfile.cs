using AutoMapper;
using Base.BusinessEntity;
using System;

namespace Base.DTO.AutoMapper
{
    public class DomainToDtoMappingProfile : Profile
    {
        public override string ProfileName
        {
            get { return "DomainToDtoMappingProfile"; }
        }
        protected override void Configure()
        {

            Mapper.CreateMap<Usuario, UsuarioLoginDTO>()
               .ForMember(d => d.RolNombre, x => x.MapFrom(p => p.Rol.Nombre));

            Mapper.CreateMap<Usuario, UsuarioDTO>()
                .ForMember(d => d.RolNombre, x => x.MapFrom(p => p.Rol.Nombre));
            Mapper.CreateMap<Rol, RolDTO>();
            Mapper.CreateMap<Cargo, CargoDTO>();
            Mapper.CreateMap<Reporte, ReporteDTO>();
            Mapper.CreateMap<TipoDocumento, TipoDocumentoDTO>();
            Mapper.CreateMap<Modulo, ModuloDTO>();
            Mapper.CreateMap<TablaRegistro, TablaRegistroDTO>();
            Mapper.CreateMap<Parametro, ParametroDTO>();
            Mapper.CreateMap<Almacen, AlmacenDTO>();
            Mapper.CreateMap<UnidadMedida, UnidadMedidaDTO>();
            Mapper.CreateMap<Categoria, CategoriaDTO>()
                             .ForMember(d => d.detalleLinea, x => x.MapFrom(p => p.detalleLinea))
                             .ForMember(d => d.detalleSubLinea, x => x.MapFrom(p => p.detalleSubLinea));
            Mapper.CreateMap<Linea, LineaDTO>();
            Mapper.CreateMap<SubLinea, SubLineaDTO>();
            Mapper.CreateMap<Producto, ProductoDTO>();
            Mapper.CreateMap<Status, StatusDTO>();
            Mapper.CreateMap<Cliente, ClienteDTO>();
            Mapper.CreateMap<Proveedor, ProveedorDTO>();
            Mapper.CreateMap<Kardex, KardexDTO>()
                           .ForMember(e => e.kardc_fecha_movimiento, x => x.MapFrom(p => p.kardc_fecha_movimiento.Equals(default(DateTime)) ? string.Empty : p.kardc_fecha_movimiento.ToShortDateString()));

            Mapper.CreateMap<Stock, StockDTO>();
            Mapper.CreateMap<NotaIngreso, NotaIngresoDTO>()
           .ForMember(e => e.fecha, x => x.MapFrom(p => p.ningc_fecha_nota_ingreso.Equals(default(DateTime)) ? string.Empty : p.ningc_fecha_nota_ingreso.ToShortDateString()));
            Mapper.CreateMap<NotaIngresoDetalle, NotaIngresoDetalleDTO>();
            Mapper.CreateMap<StockXAlmacen, StockXAlmacenDTO>();

            Mapper.CreateMap<NotaSalida, NotaSalidaDTO>()
          .ForMember(e => e.fecha, x => x.MapFrom(p => p.nsalc_fecha_nota_salida.Equals(default(DateTime)) ? string.Empty : p.nsalc_fecha_nota_salida.ToShortDateString()));

            Mapper.CreateMap<NotaSalidaDetalle, NotaSalidaDetalleDTO>();

            Mapper.CreateMap<Transferencia, TransferenciaDTO>()
                 .ForMember(e => e.fecha, x => x.MapFrom(p => p.trfc_sfecha_transf.Equals(default(DateTime)) ? string.Empty : p.trfc_sfecha_transf.ToShortDateString()));

            Mapper.CreateMap<TransferenciaDetalle, TransferenciaDetalleDTO>();
        }
    }
}
