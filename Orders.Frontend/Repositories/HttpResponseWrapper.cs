using System.Net;

namespace Orders.Frontend.Repositories
{
    //Es una clase generica por eso utilizamos <T> y se va a usar para hacer peticiones de cualquier entidad, [HttpDeleted], [HttpGet], [HttpPost] y [HttpPut]
    public class HttpResponseWrapper<T>
    {
        //Se definen solo get, para tener acceso a ellas solamente desde el constructor
        public T? Response { get; }
        public bool Error { get; }
        public HttpResponseMessage HttpResponseMessage { get; }

        //Se le pasa un parametro opcional generico tipo T y lo vamos a llamar response, error por si hay error y el mensaje de la peticion
        public HttpResponseWrapper(T? response, bool error, HttpResponseMessage httpResponseMessage)
        {
            Response = response;
            Error = error;
            HttpResponseMessage = httpResponseMessage;
        }

        //Creamos un metodo para saber si hubo error en la peticion y puede ser opcional por eso se usa <string?> 
        public async Task<string?> GetErrorMessageAsync()
        {
            if(!Error)
            {
                return null;
            }

            //Obtenemos el resultado de la peticion
            var statusCode = HttpResponseMessage.StatusCode;
            if (statusCode == HttpStatusCode.NotFound)
            {
                //Como esta clase resgresa un string
                return "Recurso no encontrado";
            }

            //No se sabe cual fue el error, entonces usamo el metodo async
            if (statusCode == HttpStatusCode.BadRequest)
            {
                return await HttpResponseMessage.Content.ReadAsStringAsync();
            }

            //No autorizado (login) para ejecutar esta operacion
            if (statusCode == HttpStatusCode.Unauthorized)
            {
                return "Tiene que hacer login para ejecutar esta operacion";
            }

            //No autorizado (permisos) para ejecutar esta operacion
            if (statusCode == HttpStatusCode.Forbidden)
            {
                return "No tiene permiso para ejecutar esta operacion";
            }

            return "Ha ocurrido un error inesperado";
        }
    }
}
