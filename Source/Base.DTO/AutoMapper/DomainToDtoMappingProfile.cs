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

        }
    }
}
