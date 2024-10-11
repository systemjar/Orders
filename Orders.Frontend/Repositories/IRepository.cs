namespace Orders.Frontend.Repositories
{
    public interface IRepository
    {
        //Los metodos son genericos <T> para cualquier entidad
        //La url es la del controlador
        //El va a devolver un HttpResponseWrapper <T> para cualquier entidad
        Task<HttpResponseWrapper<T>> GetAsync<T>(string url);

        //Se sobreescribe el PostAsync parque hay Post que no regresan nada y otros que si
        Task<HttpResponseWrapper<object>> PostAsync<T>(string url, T model);
        Task<HttpResponseWrapper<TActionResponse>> PostAsync<T, TActionResponse>(string url, T model);
    }
}
