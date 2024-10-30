using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Orders.Frontend.Repositories;
using Orders.Frontend.Shared;
using Orders.Shared.Entities;
using System.Net;

namespace Orders.Frontend.Pages.Countries
{
    [Authorize(Roles = "Admin")]
    public partial class CountryEdit
    {
        //Creamos un atributo privado Country que es Pais que vamos a editar
        private Country? country;

        //Vamos a referenciar el formulario CountryForm, es la representacion del codigo blazor en mi componente c#
        private FormWithName<Country>? countryForm;

        //Inyectamos el repositorio para poder utilizar el post y el put
        [Inject] private IRepository Repository { get; set; } = null!;

        //Inyectamos el NavigationMannager par navegar entre paginas de la solucion
        [Inject] private NavigationManager NavigationManager { get; set; } = null!;

        //Inyectamos el SweetAlert2
        [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;

        //Parametro es el Id del pais a editar
        [EditorRequired, Parameter] public int Id { get; set; }

        //Para capturar el pais a editar cuando haya aceptdo los parametros
        protected override async Task OnParametersSetAsync()
        {
            //Obtenemos el pais con el Id que pasaron de parametro
            var responseHtpp = await Repository.GetAsync<Country>($"/api/countries/{Id}");
            //Si hubo error
            if (responseHtpp.Error)
            {
                if (responseHtpp.HttpResponseMessage.StatusCode == HttpStatusCode.NotFound)
                {
                    //Si no encontro el pais lo devuelva a la pagina principal de countries
                    NavigationManager.NavigateTo("/countries");
                }
                else
                {
                    //Capturamos el error
                    var message = await responseHtpp.GetErrorMessageAsync();
                    //Presentamos el error con SweetAlert2
                    await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                }
            }
            else
            {
                country = responseHtpp.Response;
            }
        }

        private async Task EditAsync()
        {
            var responseHttp = await Repository.PutAsync("/api/countries", country);
            //Si hub error el en PutAsync
            if (responseHttp.Error)
            {
                //Capturamos el error
                var message = await responseHttp.GetErrorMessageAsync();
                //Presentamos el error con SweetAlert2
                await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
            }

            Return();

            var toast = SweetAlertService.Mixin(new SweetAlertOptions
            {
                Toast = true,
                Position = SweetAlertPosition.BottomEnd,
                ShowConfirmButton = true,
                Timer = 3000
            });
            await toast.FireAsync(icon: SweetAlertIcon.Success, message: "Registro editado con exito");
        }

        private void Return()
        {
            //Prendemos que si se posteo correctamente
            countryForm!.FormPostedSuccessfully = true;
            NavigationManager.NavigateTo("/countries");
        }
    }
}