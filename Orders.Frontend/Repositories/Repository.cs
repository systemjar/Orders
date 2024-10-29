using System.Text;
using System.Text.Json;

namespace Orders.Frontend.Repositories
{
    //Implementa la interfase IRepository
    public class Repository : IRepository
    {
        //Le vamos a inyectar el Http para hacer conexiones y funciona la definicion del builder service que se hizo en program con el puerto del servidor
        private readonly HttpClient _httpClient;

        //Creamos un atributo para json, la flecha indica que es de lectura
        private JsonSerializerOptions _jsonDefaultOptions => new JsonSerializerOptions
        {
            //Para evitar el problema de las mayusculas porque en json vienen en minusculas y en c# estan en mayusculas
            PropertyNameCaseInsensitive = true,
        };

        public Repository(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<HttpResponseWrapper<object>> DeleteAsync<T>(string url)
        {
            //Mandamos la url y el modelo ya codificado
            var responsehttp = await _httpClient.DeleteAsync(url);

            //Como no regresa nada, mandamos null, si hubo error regresamos la negacion sel IsSuccessStatusCode y la respuesta del http
            return new HttpResponseWrapper<object>(null, !responsehttp.IsSuccessStatusCode, responsehttp);
        }

        public async Task<HttpResponseWrapper<T>> GetAsync<T>(string url)
        {
            var responsehttp = await _httpClient.GetAsync(url);
            if (responsehttp.IsSuccessStatusCode)
            {
                // Si funciona la peticion hay que deserializar la respuesta porque viene en json y hay que pasarla a un objeto mediane un metodo privado creado por nosostros
                var response = await UnserializedAnswerAsync<T>(responsehttp);

                //Regresamos el objeto deserializado, false porque funciono y la respuesta del http
                return new HttpResponseWrapper<T>(response, false, responsehttp);
            }

            //Como hubo error regresamos el default, true porque hubo error y la respuesta del http
            return new HttpResponseWrapper<T>(default, true, responsehttp);
        }

        //Para la respuesta del PostAsync NoContent
        public async Task<HttpResponseWrapper<object>> PostAsync<T>(string url, T model)
        {
            //Serializamos el modelo para volverlo string json
            var messageJson = JsonSerializer.Serialize(model);

            //Convertimos para utilizar el alfabeto y codificar segun enumeracion y formato json
            var messageContent = new StringContent(messageJson, Encoding.UTF8, "application/json");

            //Mandamos la url y el modelo ya codificado
            var responsehttp = await _httpClient.PostAsync(url, messageContent);

            //Como no regresa nada, mandamos null, si hubo error regresamos la negacion sel IsSuccessStatusCode y la respuesta del http
            return new HttpResponseWrapper<object>(null, !responsehttp.IsSuccessStatusCode, responsehttp);
        }

        //Recibe de parametros la url y el modelo para la respuesta del PostAsync OK()
        public async Task<HttpResponseWrapper<TActionResponse>> PostAsync<T, TActionResponse>(string url, T model)
        {
            //Serializamos el modelo para volverlo string json
            var messageJson = JsonSerializer.Serialize(model);

            //Convertimos para utilizar el alfabeto y codificar segun enumeracion y formato json
            var messageContent = new StringContent(messageJson, Encoding.UTF8, "application/json");

            //Mandamos la url y el modelo ya codificado
            var responsehttp = await _httpClient.PostAsync(url, messageContent);
            if (responsehttp.IsSuccessStatusCode)
            {
                // Si funciona la peticion hay que deserializar la respuesta porque viene en json y hay que pasarla a un objeto mediane un metodo privado creado por nosostros. Deserializamos lo que esperamos que nos regrese que ya no es <T> sino <TActionResponse>
                var response = await UnserializedAnswerAsync<TActionResponse>(responsehttp);

                //Regresamos el objeto deserializado, false porque funciono y la respuesta del http
                return new HttpResponseWrapper<TActionResponse>(response, false, responsehttp);
            }

            //Como hubo error regresamos el default, true porque hubo error y la respuesta del http
            return new HttpResponseWrapper<TActionResponse>(default, true, responsehttp);
        }

        //Para la respuesta del PutAsync NoContent
        public async Task<HttpResponseWrapper<object>> PutAsync<T>(string url, T model)
        {
            //Serializamos el modelo para volverlo string json
            var messageJson = JsonSerializer.Serialize(model);

            //Convertimos para utilizar el alfabeto y codificar segun enumeracion y formato json
            var messageContent = new StringContent(messageJson, Encoding.UTF8, "application/json");

            //Mandamos la url y el modelo ya codificado
            var responsehttp = await _httpClient.PutAsync(url, messageContent);

            //Como no regresa nada, mandamos null, si hubo error regresamos la negacion sel IsSuccessStatusCode y la respuesta del http
            return new HttpResponseWrapper<object>(null, !responsehttp.IsSuccessStatusCode, responsehttp);
        }

        //Recibe de parametros la url y el modelo para la respuesta del PutAsync OK()
        public async Task<HttpResponseWrapper<TActionResponse>> PutAsync<T, TActionResponse>(string url, T model)
        {
            //Serializamos el modelo para volverlo string json
            var messageJson = JsonSerializer.Serialize(model);

            //Convertimos para utilizar el alfabeto y codificar segun enumeracion y formato json
            var messageContent = new StringContent(messageJson, Encoding.UTF8, "application/json");

            //Mandamos la url y el modelo ya codificado
            var responsehttp = await _httpClient.PutAsync(url, messageContent);
            if (responsehttp.IsSuccessStatusCode)
            {
                // Si funciona la peticion hay que deserializar la respuesta porque viene en json y hay que pasarla a un objeto mediane un metodo privado creado por nosostros. Deserializamos lo que esperamos que nos regrese que ya no es <T> sino <TActionResponse>
                var response = await UnserializedAnswerAsync<TActionResponse>(responsehttp);

                //Regresamos el objeto deserializado, false porque funciono y la respuesta del http
                return new HttpResponseWrapper<TActionResponse>(response, false, responsehttp);
            }

            //Como hubo error regresamos el default, true porque hubo error y la respuesta del http
            return new HttpResponseWrapper<TActionResponse>(default, true, responsehttp);
        }

        //Para transformar la respuesta
        private async Task<T> UnserializedAnswerAsync<T>(HttpResponseMessage responsehttp)
        {
            //Leemos el contenido de la lectura como un string
            var response = await responsehttp.Content.ReadAsStringAsync();

            //Regresamos la respuesta como un objeto tipo <T> tomado en cuenta las opciones que definimos en _jsonDefaultOptions
            return JsonSerializer.Deserialize<T>(response, _jsonDefaultOptions)!;
        }
    }
}