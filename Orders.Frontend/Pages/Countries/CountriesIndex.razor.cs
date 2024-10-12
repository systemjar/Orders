using Microsoft.AspNetCore.Components;
using Orders.Frontend.Repositories;
using Orders.Shared.Entities;

namespace Orders.Frontend.Pages.Countries
{
    public partial class CountriesIndex
    {
        /*Inyectamos el Repositorio pero el objeto tiene que ir en con la primera en Mayuscula tipo propiedad con {get; y set;} lo injectamos en la clase _Imports.razor */
        [Inject] private IRepository Repository { get; set; } = null!;

        //Creamos una lista tipo countries con ? porque puede ser null
        public List<Country>? LCountries { get; set; }

        //Sobre cargamos el metodo qe se ejecuta cuando carga la pagina
        protected async override Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            //Obtenemos una lista de paises utilizando el componente repository generico que creamos
            //Utilizamos el GetAsync al que se le manda la url "api/countries" y el devuelve el responseHttp con todo el contenido de respuesta, en este caso una lista
            var responseHttp = await Repository.GetAsync<List<Country>>("api/countries");
            LCountries = responseHttp.Response;
        }

    }
}

