using Orders.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orders.Shared.Entities
{
    public class State : IEntityWithName
    {
        public int Id { get; set; }

        //Data Notation
        [Display(Name = "Departamento")]  //EL nombre del campo {0}
        [MaxLength(100, ErrorMessage = "El campo {0} no puede tener mas de {1} carácteres")]  //EL largo maximo del campo
        [Required(ErrorMessage = "El campo {0} es obligatorio")]  //El campo es obligatorio
        public string Name { get; set; } = null!; //null! indica que no puede ser null

        //Relacionar la tabla con la tabla Country por medio del ID
        //Hay que definir otra propiedad en la tabla con que se relaciona en esta caso Country
        public int CountryId { get; set; }

        public Country? Country { get; set; }

        //Para la realacion de la tabla con Ciudades
        public ICollection<City>? Cities { get; set; }

        [Display(Name = "Ciudades")]
        public int CitiesNumber => Cities == null ? 0 : Cities.Count;
    }
}