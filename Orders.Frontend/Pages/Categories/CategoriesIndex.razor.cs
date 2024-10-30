using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Orders.Frontend.Repositories;
using Orders.Shared.Entities;
using System.Net;

namespace Orders.Frontend.Pages.Categories
{
    [Authorize(Roles = "Admin")]
    public partial class CategoriesIndex
    {
        private int currentPage = 1;
        private int totalPages;

        /*Inyectamos el Repositorio pero el objeto tiene que ir en con la primera en Mayuscula tipo propiedad con {get; y set;} lo injectamos en la clase _Imports.razor */
        [Inject] private IRepository Repository { get; set; } = null!;

        //Inyectamos el NavigationMannager par navegar entre paginas de la solucion
        [Inject] private NavigationManager NavigationManager { get; set; } = null!;

        //Inyectamos el SweetAlert2
        [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;

        [Parameter, SupplyParameterFromQuery] public string Page { get; set; } = string.Empty;
        [Parameter, SupplyParameterFromQuery] public string Filter { get; set; } = string.Empty;

        //Creamos una lista tipo countries con ? porque puede ser null
        public List<Category>? LCategories { get; set; }

        //Sobre cargamos el metodo qe se ejecuta cuando carga la pagina
        protected override async Task OnInitializedAsync()
        {
            await LoadAsync();
        }

        //Metodo para cargar la pagina
        private async Task SelectedPageAsync(int page)
        {
            currentPage = page;
            await LoadAsync(page);
        }

        private async Task LoadAsync(int page = 1)
        {
            if (!string.IsNullOrWhiteSpace(Page))
            {
                page = Convert.ToInt32(Page);
            }

            var ok = await LoadListAsync(page);
            if (ok)
            {
                await LoadPagesAsync();
            }
        }

        private async Task<bool> LoadListAsync(int page)
        {
            var url = $"api/categories/?page={page}";
            if (!string.IsNullOrEmpty(Filter))
            {
                url += $"&filter={Filter}";
            }

            var responseHttp = await Repository.GetAsync<List<Category>>(url);
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return false;
            }
            LCategories = responseHttp.Response;
            return true;
        }

        private async Task LoadPagesAsync()
        {
            var url = $"api/categories/totalPages";
            if (!string.IsNullOrEmpty(Filter))
            {
                url += $"?filter={Filter}";
            }

            var responseHttp = await Repository.GetAsync<int>(url);
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
            }
            totalPages =
            responseHttp.Response;
        }

        private async Task CleanFilterAsync()
        {
            Filter = string.Empty;
            await ApplyFilterAsync();
        }

        private async Task ApplyFilterAsync()
        {
            int page = 1;
            await LoadAsync(page);
            await SelectedPageAsync(page);
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