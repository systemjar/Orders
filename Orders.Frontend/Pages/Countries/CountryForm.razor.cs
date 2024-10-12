using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Orders.Shared.Entities;

namespace Orders.Frontend.Pages.Countries
{
    public partial class CountryForm
    {
        //Definimos el contexto del formulario
        private EditContext editContext = null!;

        //Override para cargar el contexto
        protected override void OnInitialized()
        {
            editContext = new(Country);
        }

        //Le pasamos de parameto requerido al objeto Country
        [EditorRequired, Parameter] public Country Country { get; set; } = null!;

        //Le vamos a pasar codigo del evento cuando le de guardar
        [EditorRequired, Parameter] public EventCallback OnValidSubmit { get; set; }

        //Le vamos a pasar codigo del evento regresar o cancelar
        [EditorRequired, Parameter] public EventCallback ReturnAction { get; set; }

        //Vamos a inyectar el servicio de SweetAlert2 (el de los mensajes tipo modal)
        [Inject] public SweetAlertService SweetAlertService { get; set; } = null!;

        //Creamos una propiedad que nos va a indicar si el formulario fue Post exitosamente
        public bool FormPostedSuccessfully { get; set; }

        //Creamos un metodo que nos va a indicar si se salio del formulario sin guardar los cambios
        private async Task OnBeforeInternalNavigation(LocationChangingContext context)
        {
            //Creamos una variable que nos indica si el contexto o formulario fue modificado
            var formWasEdited = editContext.IsModified();
            if (!formWasEdited || FormPostedSuccessfully)
            {
                return;
            }

            //Si llegó hasta aquie es una salida por error preparamos la alerta
            var result = await SweetAlertService.FireAsync(new SweetAlertOptions
            {
                Title = "Confirmación",
                Text = "Desea salirse sin guardar los cambios?",
                Icon = SweetAlertIcon.Question,
                ShowCancelButton = true
            });

            //En cofirm guardamos la respuesta de la alerta result, si regresa true es que si quiere perder los cambion, por eso lo negamos
            var confirm = !string.IsNullOrEmpty(result.Value);
            if (confirm)
            {
                return;
            }

            //No quiere perder los cambios y lo obligamos quedarse en el formulario
            context.PreventNavigation();
        }
    }
}