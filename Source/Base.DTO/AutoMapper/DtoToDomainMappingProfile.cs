using AutoMapper;
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

        }
    }
}
