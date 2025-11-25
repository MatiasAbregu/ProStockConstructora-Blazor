using BD.Modelos;
using DTO.DTOs_Response;
using DTO.DTOs_Usuarios;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Repositorios.Implementaciones;
using Repositorios.Servicios;
using System.Collections.Generic;
using System.Diagnostics;

namespace ProStockConstructora.Controllers
{
    [Route("api/usuario")]
    [ApiController]
    public class ControladorUsuario : ControllerBase
    {
        private readonly IUsuarioServicio usuarioServicio;

        public ControladorUsuario(IUsuarioServicio usuarioServicio) 
        {
            this.usuarioServicio = usuarioServicio;
        }

        [HttpPost("iniciar-sesion")]
        public async Task<ActionResult> IniciarSesion(IniciarSesionDTO usuario)
        {
            var res = await usuarioServicio.IniciarSesion(usuario);

            if (res.Estado) return StatusCode(200, res);
            else return StatusCode(500, res);
        }

        [HttpGet("{EmpresaId:long}")]
        public async Task<ActionResult> ObtenerUsuariosDeEmpresa(long EmpresaId)
        {
            Response<List<DatosUsuario>> res = await usuarioServicio.ObtenerUsuariosPorEmpresaId(EmpresaId);

            if (res.Estado) return StatusCode(200, res);
            return StatusCode(500, res);
        }

        [HttpGet("obtener-usuario/{id:long}")]
        public async Task<ActionResult> ObtenerUsuarioPorId(long id)
        {
            var res = await usuarioServicio.ObtenerUsuarioPorId(id);
            if (res.Estado) return StatusCode(200, res);
            return StatusCode(500, res);
        }

        [HttpPost]
        public async Task<ActionResult<Response<string>>> CrearUsuario(CrearUsuarioDTO usuario)
        {
            var res = await usuarioServicio.CrearUsuario(usuario);

            if (res.Estado) return StatusCode(200, res);
            else return StatusCode(500, res);
        }

        [HttpPut("{id:long}")]
        public async Task<ActionResult> ActualizarUsuario(long id, ActualizarUsuarioDTO usuario)
        {
            if (id != usuario.Id)
                return StatusCode(409, new Response<string>()
                {
                    Estado = false,
                    Mensaje = "Ocurrió un error al intentar actualizar el usuario.",
                    Objeto = null
                });

            var res = await usuarioServicio.ActualizarUsuario(id, usuario);

            if (res.Estado) return StatusCode(200, res);
            else return StatusCode(500, res);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> CambiarEstadoUsuario(long id)
        {
            var res = await usuarioServicio.CambiarEstadoUsuario(id);

            if(res.Estado) return StatusCode(200, res);
            else return StatusCode(500, res);
        }
    }
}