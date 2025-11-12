using BD;
using BD.Modelos;
using DTO.DTOs_Response;
using DTO.DTOs_Usuarios;
using Microsoft.EntityFrameworkCore;
using Repositorios.Implementaciones;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Repositorios.Servicios
{
    public class UsuarioServicio : IUsuarioServicio
    {
        private readonly AppDbContext baseDeDatos;

        public UsuarioServicio(AppDbContext baseDeDatos)
        {
            this.baseDeDatos = baseDeDatos;
        }

        public async Task<Response<DatosUsuario>> IniciarSesion(IniciarSesionDTO usuarioDTO)
        {
            try
            {
                Usuario? usuario = await baseDeDatos.Usuarios.FirstOrDefaultAsync(u => u.Email == usuarioDTO.Email
                                                              && u.Contrasena == usuarioDTO.Contrasena);

                if (usuario == null)
                {
                    return new Response<DatosUsuario>()
                    {
                        Objeto = null,
                        Estado = true,
                        Mensaje = "Correo o contraseña incorrectos."
                    };
                }
                else if (!usuario.Estado)
                {
                    return new Response<DatosUsuario>()
                    {
                        Objeto = null,
                        Estado = true,
                        Mensaje = "El usuario no está disponible."
                    };
                }

                var roles = await baseDeDatos.RolesUsuarios.Where(u => u.UsuarioId == usuario.Id)
                                .Include(u => u.Rol).Select(r => r.Rol.NombreRol).ToListAsync();

                var obrasId = await baseDeDatos.ObraUsuarios.Where(u => u.UsuarioId == usuario.Id)
                                    .Select(o => o.ObraId).ToListAsync();

                var depositosId = await baseDeDatos.DepositosUsuario.Where(u => u.UsuarioId == usuario.Id)
                                        .Select(d => d.DepositoId).ToListAsync();

                return new Response<DatosUsuario>()
                {
                    Objeto = new DatosUsuario()
                    {
                        Id = usuario.Id,
                        Email = usuario.Email,
                        NombreUsuario = usuario.NombreUsuario,
                        Estado = "Activo",
                        Telefono = usuario.Telefono,
                        EmpresaId = usuario.EmpresaId,
                        Roles = roles,
                        ObrasId = obrasId,
                        DepositosId = depositosId
                    },
                    Estado = true,
                    Mensaje = "¡Inicio de sesión éxitoso!"
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new Response<DatosUsuario>()
                {
                    Objeto = null,
                    Estado = false,
                    Mensaje = $"¡Hubo un error desde el servidor al iniciar sesión!"
                };
            }
        }

        public async Task<Response<List<DatosUsuario>>> ObtenerUsuariosPorEmpresaId(long id)
        {
            try
            {
                var usuariosBD = await baseDeDatos.Usuarios.Where(u => u.EmpresaId == id && u.Estado)
                .ToListAsync();

                if (usuariosBD.Count == 0) return new Response<List<DatosUsuario>>()
                {
                    Objeto = null,
                    Mensaje = "No hay usuarios aún en el sistema",
                    Estado = true
                };

                var usuarios = new List<DatosUsuario>();
                foreach (var u in usuariosBD)
                {
                    var roles = await baseDeDatos.RolesUsuarios
                                .Include(r => r.Rol)
                                .Where(r => r.UsuarioId == u.Id)
                                .Select(r => r.Rol.NombreRol)
                                .ToListAsync();

                    var obrasId = await baseDeDatos.ObraUsuarios
                                        .Where(o => o.UsuarioId == u.Id)
                                        .Select(o => o.ObraId)
                                        .ToListAsync();

                    var depositosId = await baseDeDatos.DepositosUsuario
                                            .Where(d => d.UsuarioId == u.Id)
                                            .Select(d => d.DepositoId)
                                            .ToListAsync();

                    usuarios.Add(new DatosUsuario()
                    {
                        Id = u.Id,
                        Email = u.Email,
                        NombreUsuario = u.NombreUsuario,
                        Estado = "Activo",
                        Telefono = u.Telefono,
                        EmpresaId = u.EmpresaId,
                        Roles = roles,
                        ObrasId = obrasId,
                        DepositosId = depositosId
                    });
                }

                return new Response<List<DatosUsuario>>()
                { Estado = true, Mensaje = "¡Usuarios cargados con éxito!", Objeto = usuarios };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new Response<List<DatosUsuario>>()
                { Estado = false, Mensaje = "¡Hubo un error desde el servidor al cargar usuarios!", Objeto = null };
            }
        }

        public async Task<Response<DatosUsuario>> ObtenerUsuarioPorId(long Id)
        {
            try
            {
                var usuariosBD = await baseDeDatos.Usuarios.FirstOrDefaultAsync(u => u.Id == Id && u.Estado);

                if (usuariosBD == null) return new Response<DatosUsuario>()
                {
                    Objeto = null,
                    Mensaje = "Ese usuario no existe en el sistema o no está disponible.",
                    Estado = true
                };

                var roles = await baseDeDatos.RolesUsuarios
                            .Include(r => r.Rol)
                            .Where(r => r.UsuarioId == usuariosBD.Id)
                            .Select(r => r.Rol.NombreRol)
                            .ToListAsync();

                var obrasId = await baseDeDatos.ObraUsuarios
                                    .Where(o => o.UsuarioId == usuariosBD.Id)
                                    .Select(o => o.ObraId)
                                    .ToListAsync();

                var depositosId = await baseDeDatos.DepositosUsuario
                                        .Where(d => d.UsuarioId == usuariosBD.Id)
                                        .Select(d => d.DepositoId)
                                        .ToListAsync();

                var usuario = new DatosUsuario()
                {
                    Id = usuariosBD.Id,
                    Email = usuariosBD.Email,
                    NombreUsuario = usuariosBD.NombreUsuario,
                    Estado = "Activo",
                    Telefono = usuariosBD.Telefono,
                    EmpresaId = usuariosBD.EmpresaId,
                    Roles = roles,
                    ObrasId = obrasId,
                    DepositosId = depositosId
                };

                return new Response<DatosUsuario>()
                { Estado = true, Mensaje = "¡Usuario cargado con éxito!", Objeto = usuario };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new Response<DatosUsuario>()
                { Estado = false, Mensaje = "¡Hubo un error desde el servidor al cargar el usuario!", Objeto = null };
            }
        }

        public Task<Usuario> ObtenerUsuarioPorNombreUsuario()
        {
            throw new NotImplementedException();
        }

        public Task<List<Usuario>> ObtenerUsuariosPorCategoria()
        {
            throw new NotImplementedException();
        }

        //public async Task<IdentityResult> CrearUsuario(CrearUsuarioDTO usuario)
        //{
        //    using var transaction = await baseDeDatos.Database.BeginTransactionAsync();

        //    Usuario UsuarioBD = new(usuario.NombreUsuario, usuario.EmpresaId, usuario.Email, usuario.Celular);
        //    IdentityResult resultado = await gestorUsuarios.CreateAsync(UsuarioBD, usuario.NombreUsuario);

        //    if (resultado.Succeeded)
        //    {
        //        resultado = await gestorUsuarios.AddToRolesAsync(UsuarioBD, usuario.Roles);
        //        if (!resultado.Succeeded)
        //        {
        //            await transaction.RollbackAsync();
        //            return resultado;
        //        }
        //    }
        //    else
        //    {
        //        foreach (IdentityError error in resultado.Errors)
        //        {
        //            Debug.WriteLine(error.Description);
        //        }
        //        await transaction.RollbackAsync();
        //        return resultado;
        //    }
        //    await transaction.CommitAsync();
        //    return resultado;
        //}

        public async Task<(bool, string, Usuario)> ActualizarUsuario(string id, ActualizarUsuarioDTO usuario)
        {
            //Usuario? usuarioBBDD = await gestorUsuarios.FindByIdAsync(id);

            //using var transaction = await baseDeDatos.Database.BeginTransactionAsync();

            //if (usuarioBBDD == null) return (false, "El usuario que se desea actualizar no existe", null);

            //if (!string.IsNullOrEmpty(usuario.NombreUsuario))
            //{
            //    if (usuarioBBDD.UserName != usuario.NombreUsuario)
            //    {
            //        var resName = await gestorUsuarios.FindByNameAsync(usuario.NombreUsuario);
            //        if (resName != null) return (false, "El usuario con ese nombre ya existe", null);
            //        usuarioBBDD.UserName = usuario.NombreUsuario;
            //    }
            //}

            //if (!string.IsNullOrEmpty(usuario.Celular))
            //    usuarioBBDD.PhoneNumber = usuario.Celular;

            //if (!string.IsNullOrEmpty(usuario.Email))
            //    usuarioBBDD.Email = usuario.Email;

            //IdentityResult res;

            //if (usuario.Roles.Count > 0)
            //{
            //    var rolesActuales = await gestorUsuarios.GetRolesAsync(usuarioBBDD);

            //    var rolesNormalizados =
            //        rolesActuales.Select(r => Regex.Replace(r.Replace(" ", "").ToUpperInvariant()
            //        .Normalize(NormalizationForm.FormD), "[\u0300-\u036f]", string.Empty
            //        )).ToList();

            //    var rolesAEliminar = rolesNormalizados.Except(usuario.Roles).ToList();
            //    var rolesAAnadir = usuario.Roles.Except(rolesNormalizados).ToList();

            //    if (rolesAEliminar.Count > 0)
            //    {
            //        res = await gestorUsuarios.RemoveFromRolesAsync(usuarioBBDD, rolesAEliminar);
            //        if (!res.Succeeded)
            //        {
            //            foreach (var error in res.Errors) Debug.WriteLine($"Error: {error.Description}");
            //            await transaction.RollbackAsync();
            //            return (false, "Error al eliminar roles", null);
            //        }
            //    }

            //    if (rolesAAnadir.Count > 0)
            //    {
            //        res = await gestorUsuarios.AddToRolesAsync(usuarioBBDD, rolesAAnadir);
            //        if (!res.Succeeded)
            //        {
            //            await transaction.RollbackAsync();
            //            return (false, "Error al añadir roles", null);
            //        }
            //    }
            //}

            //res = await gestorUsuarios.UpdateAsync(usuarioBBDD);
            //if (!res.Succeeded) await transaction.RollbackAsync();

            //await transaction.CommitAsync();
            //return (true, "Usuario actualizado con éxito", usuarioBBDD);
            throw new NotImplementedException();
        }

        public async Task<(bool, string)> CambiarEstadoUsuario(string id)
        {
            //Usuario? usuarioBBDD = await gestorUsuarios.FindByIdAsync(id);

            //if (usuarioBBDD == null) return (false, "El usuario que se desea cambiar de estado no existe");

            //usuarioBBDD.Estado = !usuarioBBDD.Estado;
            //string estado = usuarioBBDD.Estado ? "reactivado" : "desactivado";

            //var res = await gestorUsuarios.UpdateAsync(usuarioBBDD);

            //if (!res.Succeeded) return (false, "Ocurrio un error al cambiar estado del usuario");
            //return (true, $"Usuario {estado} con éxito");
            throw new NotImplementedException();
        }
    }
}
