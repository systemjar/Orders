using Microsoft.EntityFrameworkCore;
using Orders.Backend.Data;
using Orders.Backend.Repositories.Interfaces;
using Orders.Shared.Responses;
using System;
using static System.Runtime.InteropServices.JavaScript.JSType;

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

        public async Task<ActionResponse<T>> AddAsync(T entity)
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

        public async Task<ActionResponse<T>> DeleteAsync(int id)
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

        public async Task<ActionResponse<T>> GetAsync(int id)
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

        public async Task<ActionResponse<IEnumerable<T>>> GetAsync()
        {
            return new ActionResponse<IEnumerable<T>>
            {
                WasSuccess = true,
                Result = await _entity.ToListAsync()
            };
        }

        public async Task<ActionResponse<T>> UpdateAsync(T entity)
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