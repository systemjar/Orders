﻿using Microsoft.EntityFrameworkCore;
using Orders.Backend.Data;
using Orders.Backend.Helpers;
using Orders.Backend.Repositories.Interfaces;
using Orders.Shared.DTOs;
using Orders.Shared.Responses;

namespace Orders.Backend.Repositories.Implementations
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        //Inyectamos el Datacontext
        private readonly DataContext _context;

        //Propiedad para mapear la entidad a trabajar
        private readonly DbSet<T> _entity;

        public GenericRepository(DataContext context)
        {
            _context = context;

            //Mapeamos la entidad a trabajar
            _entity = _context.Set<T>();
        }

        //El atributo virtual sirve para poder sobre escribir el metodo
        public virtual async Task<ActionResponse<T>> AddAsync(T entity)
        {
            _context.Add(entity);
            try
            {
                //Salvar los cambios
                await _context.SaveChangesAsync();

                //Si lo salvó
                return new ActionResponse<T>
                {
                    WasSuccess = true,
                    Result = entity
                };
            }
            catch (DbUpdateException)
            {
                return DbUpdateExceptionActionResponse();
            }
            catch (Exception exception)
            {
                return ExceptionActionResponse(exception);
            }
        }

        public virtual async Task<ActionResponse<T>> DeleteAsync(int id)
        {
            //Buscamos a ver si existe el id de la entidad
            var objeto = await _entity.FindAsync(id);
            if (objeto is null)
            {
                return new ActionResponse<T>
                {
                    WasSuccess = false,
                    Message = "Registro No encontrado"
                };
            }

            try
            {
                _entity.Remove(objeto);

                //Salvar los cambios
                await _context.SaveChangesAsync();

                //Si lo salvó
                return new ActionResponse<T>
                {
                    WasSuccess = true,
                };
            }
            catch
            {
                return new ActionResponse<T>
                {
                    WasSuccess = false,
                    Message = "No se puede borrar porque tiene registros relaionados"
                };
            }
        }

        public virtual async Task<ActionResponse<T>> GetAsync(int id)
        {
            //Buscamos a ver si existe el id de la entidad
            var objeto = await _entity.FindAsync(id);
            if (objeto is null)
            {
                return new ActionResponse<T>
                {
                    WasSuccess = false,
                    Message = "Registro No encontrado"
                };
            }

            return new ActionResponse<T>
            {
                WasSuccess = true,
                Result = objeto
            };
        }

        //{ara obtener toda la lista
        public virtual async Task<ActionResponse<IEnumerable<T>>> GetAsync()
        {
            return new ActionResponse<IEnumerable<T>>
            {
                WasSuccess = true,
                Result = await _entity.ToListAsync()
            };
        }

        //Para obtener toda la lista con paginacion
        public virtual async Task<ActionResponse<IEnumerable<T>>> GetAsync(PaginationDTO pagination)
        {
            //Es la entidad queryable es decir la instruccion para obtener toda la lista pero sin ejecutar
            var queryable = _entity.AsQueryable();

            //Le agregamos al queryable la paginacion y lo ejecutamos, asi nos devuelve solo lo que necesitamos
            return new ActionResponse<IEnumerable<T>>
            {
                WasSuccess = true,
                Result = await queryable
                .Paginate(pagination)
                .ToListAsync()
            };
        }

        public virtual async Task<ActionResponse<int>> GetTotalPagesAsync(PaginationDTO pagination)
        {
            var queryable = _entity.AsQueryable();

            //Contamos los registros
            double count = await queryable.CountAsync();

            //Redondea haci arriba
            int totalPages = (int)Math.Ceiling(count / pagination.RecordsNumber);

            //Regresamos el total de paginas
            return new ActionResponse<int>
            {
                WasSuccess = true,
                Result = totalPages
            };
        }

        public virtual async Task<ActionResponse<T>> UpdateAsync(T entity)
        {
            _context.Update(entity);
            try
            {
                //Salvar los cambios
                await _context.SaveChangesAsync();

                //Si lo salvó
                return new ActionResponse<T>
                {
                    WasSuccess = true,
                    Result = entity
                };
            }
            catch (DbUpdateException)
            {
                return DbUpdateExceptionActionResponse();
            }
            catch (Exception exception)
            {
                return ExceptionActionResponse(exception);
            }
        }

        //Metodo para manejar error
        private ActionResponse<T> DbUpdateExceptionActionResponse()
        {
            return new ActionResponse<T>
            {
                WasSuccess = false,
                Message = "Ya existe el registro que estas intentando crear"
            };
        }

        private ActionResponse<T> ExceptionActionResponse(Exception exception)
        {
            return new ActionResponse<T>
            {
                WasSuccess = false,
                Message = exception.Message
            };
        }
    }
}