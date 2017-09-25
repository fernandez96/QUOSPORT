using Base.BusinessEntity;
using Base.BusinessLogic.Interfaces;
using Base.Common;
using Base.Common.Generics;
using Base.DataAccess;
using System;
using System.Collections.Generic;

namespace Base.BusinessLogic
{
    public class UsuarioBL:Singleton<UsuarioBL>,IUsuarioBL<Usuario,int>
    {
        public int Add(Usuario entity)
        {
            return UsuarioRepository.Instancia.Add(entity);
        }

        public int Delete(Usuario entity)
        {
            return UsuarioRepository.Instancia.Delete(entity);
        }

        public bool Exists(Usuario entity)
        {
            return UsuarioRepository.Instancia.Exists(entity);
        }

        public IList<Usuario> GetAll(string whereFilters)
        {
            throw new NotImplementedException();
        }

        public IList<Usuario> GetAllPaging(PaginationParameter<int> paginationParameters)
        {
            return UsuarioRepository.Instancia.GetAllPaging(paginationParameters);
        }

        public Usuario GetById(Usuario entity)
        {
            return UsuarioRepository.Instancia.GetByIdGetById(entity);
        }

        public Usuario GetByUsername(string username, string pass)
        {
            return UsuarioRepository.Instancia.GetByUsername(username, pass);
        }

        public int Update(Usuario entity)
        {
            return UsuarioRepository.Instancia.Update(entity);
        }
    }
}
