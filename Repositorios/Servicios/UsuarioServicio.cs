using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BD;
using BD.Modelos;
using DTO.DTOs_Usuarios;
using Repositorios.Implementaciones;
using Microsoft.EntityFrameworkCore;

namespace Repositorios.Servicios
{
    public class UsuarioServicio : IUsuarioServicio
    {
        private readonly AppDbContext baseDeDatos;

        public UsuarioServicio(AppDbContext baseDeDatos)
        {
            this.baseDeDatos = baseDeDatos;
        }

        public async Task<>

        public async Task<(bool, List<VerAdministradorDTO>)> ObtenerTodosLosAdministradores()
        {
            //try
            //{
            //    List<string> AdminIDs = (await gestorUsuarios.GetUsersInRoleAsync("Administrador")).Select(u => u.Id).ToList();
            //    List<string> SuperAdminIDs =
            //        (await gestorUsuarios.GetUsersInRoleAsync("Superadministrador")).Select(u => u.Id).ToList();
            //    List<string> TodosLosIds = AdminIDs.Union(SuperAdminIDs).ToList();

            //    List<VerAdministradorDTO> usuarios = [];

            //    foreach (Usuario u in
            //        baseDeDatos.Usuarios.Where(u => TodosLosIds.Contains(u.Id)).Include(u => u.Empresa).ToList())
            //    {
            //        usuarios.Add(new VerAdministradorDTO()
            //        {
            //            Id = u.Id,
            //            NombreUsuario = u.UserName,
            //            Email = u.Email,
            //            Telefono = u.PhoneNumber,
            //            Estado = u.Estado ? "Activo" : "Inactivo",
            //            EmpresaId = u.EmpresaId,
            //            NombreEmpresa = u.Empresa.NombreEmpresa
            //        }
            //        );
            //    }

            //    return (true, usuarios);
            //}
            //catch (Exception ex)
            //{
            //    Debug.WriteLine($"Error: {ex.Message}");
            //    return (false, null);
            //}
            throw new NotImplementedException();
        }

        public async Task<List<Usuario>> ObtenerTodosLosAdministradoresDeEmpresa(string nombreEmpresa)
        {
            return await baseDeDatos.Usuarios.Include(u => u.Empresa)
                .Where(u => u.Empresa.NombreEmpresa == nombreEmpresa).ToListAsync();
        }

        public async Task<(bool, List<VerUsuarioDTO>)> ObtenerUsuariosPorEmpresaId(int id)
        {
            //List<Usuario> usuariosBBDD = await baseDeDatos.Usuarios.Where(u => u.EmpresaId == id)
            //                                .ToListAsync();

            //if (usuariosBBDD.Count == 0) return (false, null);

            //List<VerUsuarioDTO> usuarios = new List<VerUsuarioDTO>();

            //foreach (var usuario in usuariosBBDD)
            //{
            //    var roles = await gestorUsuarios.GetRolesAsync(usuario);

            //    usuarios.Add(new VerUsuarioDTO()
            //    {
            //        Id = usuario.Id,
            //        NombreUsuario = usuario.UserName,
            //        Estado = usuario.Estado ? "Activo" : "Desactivado",
            //        Email = usuario.Email,
            //        Telefono = usuario.PhoneNumber,
            //        Roles = roles.ToList()
            //    });
            //}

            //return (true, usuarios);
            throw new NotImplementedException();
        }

        public async Task<(bool, VerUsuarioDTO)> ObtenerUsuarioPorId(string id)
        {
            //Usuario? usuarioBBDD = await gestorUsuarios.FindByIdAsync(id);
            //if (usuarioBBDD == null) return (false, null);
            //var roles = await gestorUsuarios.GetRolesAsync(usuarioBBDD);
            //return (true, new VerUsuarioDTO()
            //{
            //    Id = usuarioBBDD.Id,
            //    NombreUsuario = usuarioBBDD.UserName,
            //    Estado = usuarioBBDD.Estado ? "Activo" : "Desactivado",
            //    Email = usuarioBBDD.Email,
            //    Telefono = usuarioBBDD.PhoneNumber,
            //    Roles = roles.ToList()
            //});
            throw new NotImplementedException();
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
