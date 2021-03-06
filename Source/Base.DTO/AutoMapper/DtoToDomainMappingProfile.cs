﻿using AutoMapper;
using Base.BusinessEntity;

namespace Base.DTO.AutoMapper
{
    public class DtoToDomainMappingProfile : Profile
    {
        public override string ProfileName
        {
            get { return "DtoToDomainMappingProfile"; }
        }

        protected override void Configure()
        {
            Mapper.CreateMap<UsuarioDTO, Usuario>()
                .ForMember(p => p.UsuarioModificacion, x => x.Condition(p => p.Id != 0))
                .ForMember(p => p.UsuarioModificacion, x => x.MapFrom(p => p.UsuarioRegistro))
                .ForMember(p => p.UsuarioCreacion, x => x.Condition(p => p.Id == 0))
                .ForMember(p => p.UsuarioCreacion, x => x.MapFrom(p => p.UsuarioRegistro));

            Mapper.CreateMap<EnvioEmailDTO, EnvioEmail>()
               .ForMember(p => p.UsuarioModificacion, x => x.Condition(p => p.Id != 0))
               .ForMember(p => p.UsuarioModificacion, x => x.MapFrom(p => p.UsuarioRegistro))
               .ForMember(p => p.UsuarioCreacion, x => x.Condition(p => p.Id == 0))
               .ForMember(p => p.UsuarioCreacion, x => x.MapFrom(p => p.UsuarioRegistro));

            Mapper.CreateMap<TipoDocumentoDTO, TipoDocumento>()
               .ForMember(p => p.UsuarioModificacion, x => x.Condition(p => p.Id != 0))
               .ForMember(p => p.UsuarioModificacion, x => x.MapFrom(p => p.UsuarioRegistro))
               .ForMember(p => p.UsuarioCreacion, x => x.Condition(p => p.Id == 0))
               .ForMember(p => p.UsuarioCreacion, x => x.MapFrom(p => p.UsuarioRegistro));


            Mapper.CreateMap<TablaRegistroDTO, TablaRegistro>()
               .ForMember(p => p.UsuarioModificacion, x => x.Condition(p => p.Id != 0))
               .ForMember(p => p.UsuarioModificacion, x => x.MapFrom(p => p.UsuarioRegistro))
               .ForMember(p => p.UsuarioCreacion, x => x.Condition(p => p.Id == 0))
               .ForMember(p => p.UsuarioCreacion, x => x.MapFrom(p => p.UsuarioRegistro));

            Mapper.CreateMap<ParametroDTO, Parametro>()
                     .ForMember(p => p.UsuarioModificacion, x => x.Condition(p => p.Id != 0))
                     .ForMember(p => p.UsuarioModificacion, x => x.MapFrom(p => p.UsuarioRegistro))
                     .ForMember(p => p.UsuarioCreacion, x => x.Condition(p => p.Id == 0))
                     .ForMember(p => p.UsuarioCreacion, x => x.MapFrom(p => p.UsuarioRegistro));

            Mapper.CreateMap<AlmacenDTO, Almacen>()
                  .ForMember(p => p.UsuarioModificacion, x => x.Condition(p => p.Id != 0))
                  .ForMember(p => p.UsuarioModificacion, x => x.MapFrom(p => p.UsuarioRegistro))
                  .ForMember(p => p.UsuarioCreacion, x => x.Condition(p => p.Id == 0))
                  .ForMember(p => p.UsuarioCreacion, x => x.MapFrom(p => p.UsuarioRegistro));

            Mapper.CreateMap<UnidadMedidaDTO, UnidadMedida>()
                  .ForMember(p => p.UsuarioModificacion, x => x.Condition(p => p.Id != 0))
                  .ForMember(p => p.UsuarioModificacion, x => x.MapFrom(p => p.UsuarioRegistro))
                  .ForMember(p => p.UsuarioCreacion, x => x.Condition(p => p.Id == 0))
                  .ForMember(p => p.UsuarioCreacion, x => x.MapFrom(p => p.UsuarioRegistro));

            Mapper.CreateMap<LineaDTO, Linea>()
                   .ForMember(p => p.UsuarioModificacion, x => x.Condition(p => p.Id != 0))
                   .ForMember(p => p.UsuarioModificacion, x => x.MapFrom(p => p.UsuarioRegistro))
                   .ForMember(p => p.UsuarioCreacion, x => x.Condition(p => p.Id == 0))
                   .ForMember(p => p.UsuarioCreacion, x => x.MapFrom(p => p.UsuarioRegistro));

            Mapper.CreateMap<SubLineaDTO, SubLinea>()
                   .ForMember(p => p.UsuarioModificacion, x => x.Condition(p => p.Id != 0))
                   .ForMember(p => p.UsuarioModificacion, x => x.MapFrom(p => p.UsuarioRegistro))
                   .ForMember(p => p.UsuarioCreacion, x => x.Condition(p => p.Id == 0))
                   .ForMember(p => p.UsuarioCreacion, x => x.MapFrom(p => p.UsuarioRegistro));

            Mapper.CreateMap<CategoriaDTO, Categoria>()
                  .ForMember(p => p.UsuarioModificacion, x => x.Condition(p => p.Id != 0))
                  .ForMember(p => p.UsuarioModificacion, x => x.MapFrom(p => p.UsuarioRegistro))
                  .ForMember(p => p.UsuarioCreacion, x => x.Condition(p => p.Id == 0))
                  .ForMember(p => p.UsuarioCreacion, x => x.MapFrom(p => p.UsuarioRegistro));

            Mapper.CreateMap<ProductoDTO, Producto>()
                  .ForMember(p => p.UsuarioModificacion, x => x.Condition(p => p.Id != 0))
                  .ForMember(p => p.UsuarioModificacion, x => x.MapFrom(p => p.UsuarioRegistro))
                  .ForMember(p => p.UsuarioCreacion, x => x.Condition(p => p.Id == 0))
                  .ForMember(p => p.UsuarioCreacion, x => x.MapFrom(p => p.UsuarioRegistro));

            Mapper.CreateMap<NotaIngresoDTO, NotaIngreso>()
                  .ForMember(p => p.UsuarioModificacion, x => x.Condition(p => p.Id != 0))
                  .ForMember(p => p.UsuarioModificacion, x => x.MapFrom(p => p.UsuarioRegistro))
                  .ForMember(p => p.UsuarioCreacion, x => x.Condition(p => p.Id == 0))
                  .ForMember(p => p.UsuarioCreacion, x => x.MapFrom(p => p.UsuarioRegistro));

            Mapper.CreateMap<NotaSalidaDTO, NotaSalida>()
                  .ForMember(p => p.UsuarioModificacion, x => x.Condition(p => p.Id != 0))
                  .ForMember(p => p.UsuarioModificacion, x => x.MapFrom(p => p.UsuarioRegistro))
                  .ForMember(p => p.UsuarioCreacion, x => x.Condition(p => p.Id == 0))
                  .ForMember(p => p.UsuarioCreacion, x => x.MapFrom(p => p.UsuarioRegistro));

            Mapper.CreateMap<TransferenciaDTO, Transferencia>()
                 .ForMember(p => p.UsuarioModificacion, x => x.Condition(p => p.Id != 0))
                 .ForMember(p => p.UsuarioModificacion, x => x.MapFrom(p => p.UsuarioRegistro))
                 .ForMember(p => p.UsuarioCreacion, x => x.Condition(p => p.Id == 0))
                 .ForMember(p => p.UsuarioCreacion, x => x.MapFrom(p => p.UsuarioRegistro));

            Mapper.CreateMap<TransportistaDTO, Transportista>()
                 .ForMember(p => p.UsuarioModificacion, x => x.Condition(p => p.Id != 0))
                 .ForMember(p => p.UsuarioModificacion, x => x.MapFrom(p => p.UsuarioRegistro))
                 .ForMember(p => p.UsuarioCreacion, x => x.Condition(p => p.Id == 0))
                 .ForMember(p => p.UsuarioCreacion, x => x.MapFrom(p => p.UsuarioRegistro));

            Mapper.CreateMap<StatusDTO, Status>();
            Mapper.CreateMap<KardexDTO, Kardex>();


        }
    }
}
