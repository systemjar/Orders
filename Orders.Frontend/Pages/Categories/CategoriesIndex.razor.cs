using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Orders.Frontend.Repositories;
using Orders.Shared.Entities;
using System.Net;

namespace Orders.Frontend.Pages.Categories
{
    public partial class CategoriesIndex
    {
        /*Inyectamos el Repositorio pero el objeto tiene que ir en con la primera en Mayuscula tipo propiedad con {get; y set;} lo injectamos en la clase _Imports.razor */
        [Inject] private IRepository Repository { get; set; } = null!;

        //Inyectamos el NavigationMannager par navegar entre paginas de la solucion
        [Inject] private NavigationManager NavigationManager { get; set; } = null!;

        //Inyectamos el SweetAlert2
        [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;

        //Creamos una lista tipo countries con ? porque puede ser null
        public List<Category>? LCategories { get; set; }

        //Sobre cargamos el metodo qe se ejecuta cuando carga la pagina
        protected override async Task OnInitializedAsync()
        {
            await LoadAsync();
        }

        //Metodo para cargar la pagina
        private async Task LoadAsync()
        {
            //Obtenemos una lista de categorias utilizando el componente repository generico que creamos
            //Utilizamos el GetAsync al que se le manda la url "api/categories" y el devuelve el responseHttp con todo el contenido de respuesta, en este caso una lista
            var responseHttp = await Repository.GetAsync<List<Category>>("api/categories");

            //Revisamos si hay error
            if (responseHttp.Error)
            {
                //Leemos el error
                var message = await responseHttp.GetErrorMessageAsync();
                //Desplegamos el error
                await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
            }

            //Si no hubo error asignamos la respuesta a la lista LCountries
            LCategories = responseHttp.Response;
        }

        //Metodo para borrar el pais
        private async Task DeleteAsync(Category category)
        {
            //Preguntamos si quiere borrar el registro
            var result = await SweetAlertService.FireAsync(new SweetAlertOptions
            {
                Title = "Confirmación",
                Text = $"Desea eliminar la categori: {category.Name}?",
                Icon = SweetAlertIcon.Question,
                ShowCancelButton = true,
            });

            //Preguntamos si lo quiere borrar por lo que negamos el resultado porque si no quiere regresamos de una vez
            var confirm = string.IsNullOrEmpty(result.Value);
            if (confirm)
            {
                return;
            }

            //Llamamos el metodo DeleteAsync
            var responseHttp = await Repository.DeleteAsync<Category>($"api/categories/{category.Id}");
            //Preguntamos si hay error, por ejemplo si tiene ciudades asociadas
            if (responseHttp.Error)
            {
                //Preguntamos si hubo error porque el usuario cambio el query string
                if (responseHttp.HttpResponseMessage.StatusCode == HttpStatusCode.NotFound)
                {
                    //Lo regresamos a la pagina principal
                    NavigationManager.NavigateTo("/categories");
                }
                else
                {
                    //SI fue otro el error
                    var messageErorr = await responseHttp.GetErrorMessageAsync();
                    await SweetAlertService.FireAsync("Error", messageErorr, SweetAlertIcon.Error);
                }

                return;
            }

            //Si logro borrar el pais hacemos la recarga de la pagina
            await LoadAsync();

            //Mostramos mensaje de borrado
            var toast = SweetAlertService.Mixin(new SweetAlertOptions
            {
                Toast = true,
                Position = SweetAlertPosition.BottomEnd,
                ShowConfirmButton = true,
                Timer = 3000
            });
            await toast.FireAsync(icon: SweetAlertIcon.Success, message: "Registro borrado con exito");
        }
    }
}