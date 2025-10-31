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
        public ActionResult ObtenerRoles()
        {
            var res = rolesServicio.ObtenerRoles();
            if(res.Item1) return StatusCode(200, res.Item2);
            else return StatusCode(200, "No existen roles aún.");
        }
    }
}
