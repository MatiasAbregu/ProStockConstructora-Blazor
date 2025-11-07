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

        [HttpGet("obtener-usuario/{id}")]
        public async Task<ActionResult> ObtenerUsuarioPorId(string id)
        {
            ValueTuple<bool, DatosUsuario> res = await usuarioServicio.ObtenerUsuarioPorId(id);
            if (res.Item1) return StatusCode(200, res.Item2);
            return StatusCode(404, "Ese usuario no existe.");
        }
        
        //public async Task<ActionResult> CrearUsuario(CrearUsuarioDTO usuario)
        //{
        //    IdentityResult resultado = await usuarioServicio.CrearUsuario(usuario);

        //    if (resultado.Succeeded) return StatusCode(200, "¡Usuario creado con éxito!");
        //    else
        //    {
        //        string error = "";
        //        foreach (IdentityError errorListado in resultado.Errors)
        //        {
        //            if (errorListado.Code == "DuplicateUserName")
        //            {
        //                error = "¡Error, el nombre de usuario ya está en uso!";
        //                break;
        //            }
        //        }

        //        return StatusCode(400, error);
        //    }
        //}

        [HttpPut("{id}")]
        public async Task<ActionResult> ActualizarUsuario(string id, ActualizarUsuarioDTO usuario)
        {
            if (id != usuario.Id) return StatusCode(409, "Hubo un error al querer actualizar el usuario.");

            ValueTuple<bool, string, Usuario> res = await usuarioServicio.ActualizarUsuario(id, usuario);

            if (res.Item2.Contains("Error")) return StatusCode(500, res.Item2);
            else if(!res.Item1) return StatusCode(409, res.Item2);
            return StatusCode(200, res.Item2);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> CambiarEstadoUsuario(string id)
        {
            ValueTuple<bool, string> res = await usuarioServicio.CambiarEstadoUsuario(id);

            if(res.Item2.Contains("Error")) return StatusCode(500, res.Item2);
            else if(!res.Item1) return StatusCode(404, res.Item2);
            return StatusCode(200, res.Item2);
        }
    }
}