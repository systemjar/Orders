using Orders.Shared.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Orders.Shared.Entities
{
    public class Category : IEntityWithName
    {
        public int Id { get; set; }

        //Data Notation
        [Display(Name = "Pais")]  //EL nombre del campo {0}
        [MaxLength(100, ErrorMessage = "El campo {0} no puede tener mas de {1} carácteres")]  //EL largo maximo del campo
        [Required(ErrorMessage = "El campo {0} es obligatorio")]  //El campo es obligatorio
        public string Name { get; set; } = null!; //null! indica que no puede ser null
    }
}