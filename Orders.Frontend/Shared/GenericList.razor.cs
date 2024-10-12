using Microsoft.AspNetCore.Components;

namespace Orders.Frontend.Shared
{
    //Lista generica con Titem de parametro
    public partial class GenericList<Titem>
    {
        //Este parametro sirve para deplegar cuando carga el componente
        [Parameter] public RenderFragment? Loading { get; set; }

        //Esto es lo que va a desplegar cuando la lista este vacia
        [Parameter] public RenderFragment? NoRecords { get; set; }

        //Este sirve para desplegar en el body y es obligatorio
        [EditorRequired, Parameter] public RenderFragment? Body { get; set; } = null!;

        //Cual es la lista que se desea desplegar
        [EditorRequired, Parameter] public List<Titem>? MyList { get; set; } = null!;

    }
}
