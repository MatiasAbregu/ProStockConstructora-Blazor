using Repositorios.Implementaciones;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ProStockConstructora.Controllers
{
    [Route("api/roles")]
    [ApiController]
    public class ControladorRoles : ControllerBase
    {
        private readonly IRolesServicio rolesServicio;
        public ControladorRoles(IRolesServicio rolesServicio)
        {
            this.rolesServicio = rolesServicio;
        }

        [HttpGet]
        public async Task<ActionResult> ObtenerRoles()
        {
            var res = await rolesServicio.ObtenerRoles();
            if(res.Estado) return StatusCode(200, res);
            else return StatusCode(500, res);
        }
    }
}
