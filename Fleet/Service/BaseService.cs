﻿using Fleet.Interfaces.Repository;

namespace Fleet.Service
{
    public class BaseService<T> where T : class
    {
        private readonly IBaseRepository<T> _baseRepository;

        public BaseService(IBaseRepository<T> baseRepository)
        {
            _baseRepository = baseRepository;
        }

        public T Inserir(T objeto)
        {
            if (Validar(objeto))
                _baseRepository.Inserir(objeto);

            return objeto;
        }

        public void Atualizar(T objeto)
        {
            if (Validar(objeto))
                _baseRepository.Atualizar(objeto);
        }
        public void Deletar(int id)
        {
            if (Buscar(id) != null)
                _baseRepository.Deletar(id);
        }
        public List<T> Buscar()
          => _baseRepository.Buscar();

        public T? Buscar(int id)
        => _baseRepository.Buscar(id);

        public virtual bool Validar(T objeto) //Cada método sobrescreve seu validar
        => true;


    }



}