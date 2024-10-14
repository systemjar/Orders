using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orders.Shared.Interfaces
{
    public interface IEntityWithName
    {
        //Obliga a que definan la propiedad Name en la entidad
        string Name { get; set; }
    }
}