using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Orders.Backend.Data;
using Orders.Shared.Entities;

namespace Orders.Backend.Controllers
{
    //Data Notation para que funcione como controlador
    [ApiController]
    [Route("api/[controller]")]
    public class CountriesController : ControllerBase
    {
        //Campo para que se pueda usar en todo el controlador
        private readonly DataContext _context;

        public CountriesController(DataContext context)
        {
            _context = context;
        }

        //Metodo que regresa un ActionResult, pero como el metodo es Async entonces regresa un Task y hay que poner un await

        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            return Ok(await _context.Countries.ToListAsync());
        }

        //Sobrecarga del metodo Get para obtener solo un resultado
        //Hay tres formas de pasar parametros
        // 1. Por ruta, 2. Por Query String, 3. Por void
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(int id)
        {
            var country = await _context.Countries.FirstOrDefaultAsync(c => c.Id == id);
            if (country == null)
            {
                return NotFound();
            }

            return Ok(country);
        }

        //Para grabar un nuevo registro
        [HttpPost]
        public async Task<IActionResult> PostAsync(Country country)
        {
            _context.Add(country);
            await _context.SaveChangesAsync();
            return Ok(country);  //Ok es la respuesta del http y regresamos el objeto para saber como quedo al agregarlo
        }

        //Para modificar en la base de datos
        [HttpPut]
        public async Task<IActionResult> PutAsync(Country country)
        {
            _context.Update(country);
            await _context.SaveChangesAsync();
            return NoContent(); //No regesa nada porque ya lo logro actualizar
        }

        //Para eliminar un registro
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var country = await _context.Countries.FirstOrDefaultAsync(x => x.Id == id);
            if (country == null)
            {
                return NotFound();
            }

            _context.Remove(country);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}