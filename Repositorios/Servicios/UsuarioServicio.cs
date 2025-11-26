using BD;
using BD.Modelos;
using DTO.DTOs_Response;
using DTO.DTOs_Usuarios;
using Microsoft.EntityFrameworkCore;
using Repositorios.Implementaciones;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        private readonly IRolesServicio rolesServicio;

        public UsuarioServicio(AppDbContext baseDeDatos, IRolesServicio rolesServicio)
        {
            this.baseDeDatos = baseDeDatos;
            this.rolesServicio = rolesServicio;
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

        public async Task<Response<string>> CrearUsuario(CrearUsuarioDTO usuario)
        {
            try
            {
                bool existe = await baseDeDatos.Usuarios.AnyAsync(u => u.Email == usuario.Email);
                if (existe) return new Response<string>()
                {
                    Estado = true,
                    Mensaje = "Ya existe un usuario con ese email en el sistema. Pruebe con otro.",
                    Objeto = null
                };
                bool existeUnRol = await baseDeDatos.Roles.Where(r => usuario.Roles.Contains(r.NombreRol)).AnyAsync();
                if (existeUnRol == false)
                {
                    return new Response<string>()
                    {
                        Estado = true,
                        Mensaje = "Uno o más de los roles que se envió no existen.",
                        Objeto = null
                    };
                }

                var usuarioBD = new Usuario()
                {
                    NombreUsuario = usuario.NombreUsuario,
                    Contrasena = usuario.Contrasena,
                    Email = usuario.Email,
                    EmpresaId = usuario.EmpresaId,
                    Telefono = usuario.Celular,
                    Estado = true
                };

                baseDeDatos.Usuarios.Add(usuarioBD);
                await baseDeDatos.SaveChangesAsync();

                var roles = await baseDeDatos.Roles
                                .Where(r => usuario.Roles.Contains(r.NombreRol))
                                .ToListAsync();

                Console.WriteLine("ROLES DE BBDD: " + string.Join(", ", roles.Select(r => r.NombreRol)));

                baseDeDatos.RolesUsuarios.AddRange(roles.Select(r => new RolesUsuario()
                {
                    UsuarioId = usuarioBD.Id,
                    RolId = r.Id
                }));
                await baseDeDatos.SaveChangesAsync();

                if (usuario.Roles.Contains("JEFEDEDEPOSITO"))
                {
                    var depositos = await baseDeDatos.Depositos
                                        .Where(d => usuario.DepositosId.Contains(d.Id))
                                        .ToListAsync();

                    baseDeDatos.DepositosUsuario.AddRange(
                        depositos.Select(d => new DepositoUsuario()
                        {
                            UsuarioId = usuarioBD.Id,
                            DepositoId = d.Id
                        })
                    );

                    await baseDeDatos.SaveChangesAsync();
                }
                else if (usuario.Roles.Contains("JEFEDEOBRA"))
                {
                    var obras = await baseDeDatos.Obras
                                    .Where(o => usuario.ObrasId.Contains(o.Id))
                                    .ToListAsync();

                    baseDeDatos.ObraUsuarios.AddRange(
                        obras.Select(o => new ObraUsuario()
                        {
                            UsuarioId = usuarioBD.Id,
                            ObraId = o.Id
                        }));

                    await baseDeDatos.SaveChangesAsync();
                }

                return new Response<string>()
                {
                    Estado = true,
                    Mensaje = null,
                    Objeto = "¡Usuario creado con éxito!"
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return new Response<string>()
                {
                    Estado = false,
                    Mensaje = "¡Hubo un error desde el servidor al crear el usuario!",
                    Objeto = null
                };
            }
        }

        public async Task<Response<string>> ActualizarUsuario(long id, ActualizarUsuarioDTO usuario)
        {
            try
            {
                Usuario? usuarioBBDD = await baseDeDatos.Usuarios.FirstOrDefaultAsync(u => u.Id == id);

                if (usuarioBBDD == null) return new Response<string>()
                {
                    Estado = true,
                    Mensaje = "El usuario con ese ID no existe.",
                    Objeto = null
                };

                bool emailEnUso = await baseDeDatos.Usuarios
                                        .AnyAsync(u => u.Email == usuario.Email && u.Id != id);

                if (emailEnUso) return new Response<string>()
                {
                    Estado = true,
                    Mensaje = "El email ya está en uso.",
                    Objeto = null
                };

                await GestionarRolesDeUsuario(usuarioBBDD, usuario);
                await GestionarObrasYDepositosDeUsuario(usuarioBBDD, usuario);

                usuarioBBDD.Email = usuario.Email;
                usuarioBBDD.NombreUsuario = usuario.NombreUsuario;

                if (!string.IsNullOrEmpty(usuario.Contrasena)) usuarioBBDD.Contrasena = usuario.Contrasena;
                if (!string.IsNullOrEmpty(usuario.Celular)) usuarioBBDD.Telefono = usuario.Celular;

                await baseDeDatos.SaveChangesAsync();

                return new Response<string>()
                {
                    Estado = true,
                    Mensaje = null,
                    Objeto = "¡Usuario actualizado con éxito!"
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return new Response<string>()
                {
                    Estado = false,
                    Mensaje = "¡Hubo un error desde el servidor al actualizar el usuario!",
                    Objeto = null
                };
            }
        }

        public async Task<Response<string>> CambiarEstadoUsuario(long id)
        {
            try
            {
                Usuario? usuarioBBDD = await baseDeDatos.Usuarios.FirstOrDefaultAsync(u => u.Id == id);

                if (usuarioBBDD == null) return new Response<string>()
                {
                    Estado = true,
                    Mensaje = "El usuario que se desea cambiar de estado no existe",
                    Objeto = null
                };

                usuarioBBDD.Estado = !usuarioBBDD.Estado;
                string estado = usuarioBBDD.Estado ? "reactivado" : "desactivado";
                await baseDeDatos.SaveChangesAsync();

                return new Response<string>()
                {
                    Estado = true,
                    Objeto = $"Usuario {estado} con éxito",
                    Mensaje = null
                };
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                return new Response<string>()
                {
                    Estado = false,
                    Mensaje = "¡Hubo un error desde el servidor al cambiar el estado del usuario!",
                    Objeto = null
                };
            }
        }

        private async Task GestionarRolesDeUsuario(Usuario usuarioBBDD, ActualizarUsuarioDTO usuario)
        {
            var rolesActuales = await baseDeDatos.RolesUsuarios
                                        .Where(r => r.UsuarioId == usuarioBBDD.Id)
                                        .Include(r => r.Rol)
                                        .Select(r => r.Rol.NombreRol)
                                        .ToListAsync();

            var nombresRolesAEliminar = rolesActuales.Except(usuario.Roles).ToList();
            var rolesAEliminar = await rolesServicio.BuscarRolesPorNombres(nombresRolesAEliminar);

            if (rolesAEliminar.Count > 0)
            {
                var registrosAEliminar = await baseDeDatos.RolesUsuarios
                    .Where(u => u.UsuarioId == usuarioBBDD.Id &&
                            rolesAEliminar.Select(r => r.Id).Contains(u.RolId))
                    .ToListAsync();

                baseDeDatos.RolesUsuarios.RemoveRange(registrosAEliminar);
                await baseDeDatos.SaveChangesAsync();
            }

            var nombresRolesAAnadir = usuario.Roles.Except(rolesActuales).ToList();
            var rolesAAnadir = await rolesServicio.BuscarRolesPorNombres(nombresRolesAAnadir);

            if (rolesAAnadir.Count > 0)
            {
                foreach (var rol in rolesAAnadir)
                {
                    baseDeDatos.RolesUsuarios
                        .Add(new RolesUsuario() { UsuarioId = usuarioBBDD.Id, RolId = rol.Id });
                }
                await baseDeDatos.SaveChangesAsync();
            }
        }

        private async Task GestionarObrasYDepositosDeUsuario(Usuario usuarioBBDD, ActualizarUsuarioDTO usuario)
        {
            if (usuario.Roles.Contains("JEFEDEOBRA"))
            {
                var obrasUsuario = await baseDeDatos.ObraUsuarios
                                            .Where(o => o.UsuarioId == usuarioBBDD.Id)
                                            .Select(o => o.ObraId)
                                            .ToListAsync();

                var obrasAEliminar = obrasUsuario.Except(usuario.ObrasId).ToList();
                var obrasAAnadir = usuario.ObrasId.Except(obrasUsuario).ToList();

                if (obrasAEliminar.Count > 0)
                {
                    baseDeDatos.ObraUsuarios.RemoveRange(
                        await baseDeDatos.ObraUsuarios
                            .Where(o => o.UsuarioId == usuarioBBDD.Id && obrasAEliminar.Contains(o.ObraId))
                            .ToListAsync()
                    );
                    await baseDeDatos.SaveChangesAsync();
                }

                if (obrasAAnadir.Count > 0)
                {
                    baseDeDatos.ObraUsuarios.AddRange(
                        await baseDeDatos.Obras
                              .Where(o => obrasAAnadir.Contains(o.Id))
                              .Select(o => new ObraUsuario()
                              {
                                  UsuarioId = usuarioBBDD.Id,
                                  ObraId = o.Id
                              })
                              .ToListAsync()
                              );
                    await baseDeDatos.SaveChangesAsync();
                }

                var depositosUsuario = await baseDeDatos.DepositosUsuario
                                            .Where(d => d.UsuarioId == usuarioBBDD.Id)
                                            .ToListAsync();
                baseDeDatos.DepositosUsuario.RemoveRange(depositosUsuario);
                await baseDeDatos.SaveChangesAsync();
            }
            else if (usuario.Roles.Contains("JEFEDEDEPOSITO"))
            {
                var depositosUsuario = await baseDeDatos.DepositosUsuario
                                            .Where(d => d.UsuarioId == usuarioBBDD.Id)
                                            .Select(d => d.DepositoId)
                                            .ToListAsync();

                var depositosAEliminar = depositosUsuario.Except(usuario.DepositosId).ToList();
                var depositosAAnadir = usuario.DepositosId.Except(depositosUsuario).ToList();

                if (depositosAEliminar.Count > 0)
                {
                    baseDeDatos.DepositosUsuario.RemoveRange(
                        await baseDeDatos.DepositosUsuario
                            .Where(d => d.UsuarioId == usuarioBBDD.Id && depositosAEliminar.Contains(d.DepositoId))
                            .ToListAsync()
                    );
                    await baseDeDatos.SaveChangesAsync();
                }

                if (depositosAAnadir.Count > 0)
                {
                    baseDeDatos.DepositosUsuario.AddRange(
                        await baseDeDatos.Depositos
                            .Where(d => depositosAAnadir.Contains(d.Id))
                            .Select(d => new DepositoUsuario()
                            {
                                UsuarioId = usuarioBBDD.Id,
                                DepositoId = d.Id
                            }).ToListAsync()
                    );
                    await baseDeDatos.SaveChangesAsync();
                }

                var obrasUsuario = await baseDeDatos.ObraUsuarios
                                            .Where(o => o.UsuarioId == usuarioBBDD.Id)
                                            .ToListAsync();
                baseDeDatos.ObraUsuarios.RemoveRange(obrasUsuario);
                await baseDeDatos.SaveChangesAsync();
            }
            else
            {
                var depositosUsuario = await baseDeDatos.DepositosUsuario
                                            .Where(d => d.UsuarioId == usuarioBBDD.Id)
                                            .ToListAsync();
                var obrasUsuario = await baseDeDatos.ObraUsuarios
                                            .Where(o => o.UsuarioId == usuarioBBDD.Id)
                                            .ToListAsync();

                baseDeDatos.DepositosUsuario.RemoveRange(depositosUsuario);
                await baseDeDatos.SaveChangesAsync();
                baseDeDatos.ObraUsuarios.RemoveRange(obrasUsuario);
                await baseDeDatos.SaveChangesAsync();
            }
        }
    }
}