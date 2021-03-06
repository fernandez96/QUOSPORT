﻿using Base.BusinessEntity;
using Base.BusinessLogic.Interfaces;
using Base.Common;
using Base.Common.Generics;
using Base.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.BusinessLogic
{
    public class CategoriaBL: Singleton<CategoriaBL>, ICategoriaBL<Categoria,Linea,SubLinea, int>
    {
        public int Add(Categoria entity)
        {
            return CategoriaRepository.Instancia.Add(entity);
        }
        public int Update(Categoria entity)
        {
            return CategoriaRepository.Instancia.Update(entity);
        }
        public int Delete(Categoria entity)
        {
            return CategoriaRepository.Instancia.Delete(entity);
        }
        public IList<Categoria> GetAll()
        {
            return CategoriaRepository.Instancia.GetAll();
        }
        public IList<Linea> GetAllLinea(Linea entity)
        {
            return CategoriaRepository.Instancia.GetAllLinea(entity);
        }
        public IList<SubLinea> GetAllSubLinea(SubLinea entity)
        {
            return CategoriaRepository.Instancia.GetAllSubLinea(entity);
        }

        public IList<Linea> AllLinea()
        {
            return CategoriaRepository.Instancia.AllLinea();
        }
        public IList<SubLinea> AllSubLinea()
        {
            return CategoriaRepository.Instancia.AllSubLinea();
        }
        public IList<SubLinea> AllSubLineaIdCategoria(SubLinea entity)
        {
            return CategoriaRepository.Instancia.AllSubLineaIdCategoria(entity);
        }
        public IList<Categoria> GetAllPaging(PaginationParameter<int> paginationParameters)
        {
            return CategoriaRepository.Instancia.GetAllPaging(paginationParameters);
        }

        public Categoria GetById(Categoria entity)
        {
            return CategoriaRepository.Instancia.GetById(entity);
        }
        public Categoria GetCorrelativaCab( )
        {
            return CategoriaRepository.Instancia.GetCorrelativaCab();
        }
    }
}
