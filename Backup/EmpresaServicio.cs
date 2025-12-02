using Backend.BD;
using Backend.BD.Modelos;
using Backend.BD.Models;
using Backend.DTO.DTOs_Empresas;
using Backend.Repositorios.Implementaciones;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Backend.Repositorios.Servicios
{
    public class EmpresaServicio : IEmpresaServicio
    {
        private readonly AppDbContext baseDeDatos;
        private readonly UserManager<Usuario> gestorUsuarios;

        public EmpresaServicio(AppDbContext baseDeDatos, UserManager<Usuario> gestorUsuarios)
        {
            this.baseDeDatos = baseDeDatos;
            this.gestorUsuarios = gestorUsuarios;
        }

        public async Task<(bool, List<VerEmpresaDTO>)> ObtenerEmpresas()
        {
            try
            {
                List<VerEmpresaDTO> empresasVer = [];
                List<Empresa> empresas = await baseDeDatos.Empresa.ToListAsync();

                foreach (Empresa e in empresas)
                {
                    empresasVer.Add(new VerEmpresaDTO
                    {
                        Id = e.Id,
                        Nombre = e.NombreEmpresa,
                        CUIT = e.CUIT,
                        RazonSocial = e.RazonSocial,
                        Estado = e.Estado ? "Activo" : "Inactivo",
                        Telefono = e.Celular,
                        Email = e.Email
                    });
                }

                return (true, empresasVer);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error: {ex.Message}");
                return (false, null);
            }
        }

        public async Task<(bool, List<EmpresaAsociarDTO>)> ObtenerEmpresasParaAsociar()
        {
            try
            {
                List<Empresa> empresas = await baseDeDatos.Empresa.ToListAsync();
                List<EmpresaAsociarDTO> empresaAsociar = [];
                foreach (Empresa e in empresas) empresaAsociar.Add(new EmpresaAsociarDTO
                {
                    Id = e.Id,
                    NombreEmpresa = e.NombreEmpresa,
                });
                return (true, empresaAsociar);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error: {ex.Message}");
                return (false, null);
            }
        }

        public async Task<(bool, VerEmpresaDTO)> ObtenerEmpresaPorId(int id)
        {
            try
            {
                Empresa e = await baseDeDatos.Empresa.FirstOrDefaultAsync(e => e.Id == id);
                if (e == null) return (true, null);
                else return (true, new VerEmpresaDTO()
                {
                    Id = e.Id,
                    CUIT = e.CUIT,
                    Nombre = e.NombreEmpresa,
                    RazonSocial = e.RazonSocial,
                    Estado = e.Estado ? "Activo" : "Inactivo",
                    Telefono = e.Celular,
                    Email = e.Email
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error en ObtenerEmpresaPorId: {ex.Message}");
                return (false, null);
            }
        }

        public async Task<(bool, string)> CrearEmpresa(Empresa e)
        {
            try
            {
                e.Id = 0;
                e.Estado = true;

                baseDeDatos.Empresa.Add(e);
                int res = await baseDeDatos.SaveChangesAsync();
                if (res > 0)
                {
                    Usuario nuevoUsuario =
                        new(Regex.Replace(e.NombreEmpresa, "[ !\"#$%&'()*+,\\-./:;<=>?@[\\\\\\]^_`{|}~]", ""), e.Id, e.Email, e.Celular);
                    IdentityResult resultado = await gestorUsuarios.CreateAsync(nuevoUsuario, e.CUIT);

                    if (resultado.Succeeded) resultado = await gestorUsuarios.AddToRoleAsync(nuevoUsuario, "Administrador");
                    else
                    {
                        string errores = "";
                        foreach (IdentityError error in resultado.Errors) errores += $" {error.Description}";
                        throw new Exception($"Error al crear el usuario para la empresa. Errores: {errores}");
                    }

                    return (true, "Se creó con éxito la empresa. También se añadio un administrador con el mismo nombre para esa empresa " +
                        "y su contraseña es el CUIT de la empresa. Una vez se inicie sesión con esa cuenta se hará el cambio de contraseña. " +
                        "\n\nEn caso de querer modificar algún dato ir a: Usuarios -> Gestionar administradores de cada empresa -> Editar usuario");
                }
                else return (false, "No se pudo crear la empresa, intentelo más tarde.");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error: {ex.Message}");
                string errorMSG = ex.InnerException?.Message ?? ex.Message;

                if (errorMSG != null && errorMSG.Contains("Duplicate entry") && errorMSG.Contains("IX_Empresa_CUIT"))
                    return (false, "Ese CUIT ya existe, debe ingresar otro.");
                else if (errorMSG.Contains("usuario"))
                    return (false, "Fallo al intentar crear el usuario asociado a la empresa, consulte con el administrador");
                else
                    return (false, "Fallo al intentar crear la empresa, consulte con el administrador");
            }
        }

        public async Task<(bool, string)> ActualizarEmpresa(Empresa e)
        {
            try
            {
                Empresa empresaUpdate = await baseDeDatos.Empresa.FirstOrDefaultAsync(em => em.Id == e.Id);
                if (empresaUpdate != null)
                {
                    empresaUpdate.CUIT = e.CUIT;
                    empresaUpdate.NombreEmpresa = e.NombreEmpresa;
                    empresaUpdate.RazonSocial = e.RazonSocial;
                    empresaUpdate.Email = e.Email;
                    empresaUpdate.Celular = e.Celular;

                    await baseDeDatos.SaveChangesAsync();
                    return (true, "¡Se actualizó el registro con éxito!");
                }
                return (false, "Ese registro no existe.");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error: {ex.Message}");
                string errorMSG = ex.InnerException?.Message ?? ex.Message;

                if (errorMSG != null && errorMSG.Contains("Duplicate entry") && errorMSG.Contains("IX_Empresa_CUIT"))
                    return (false, "Ese CUIT ya existe, debe ingresar otro.");
                return (false, "Fallo al intentar actualizar la empresa, consulte con el administrador");
            }
        }

        public async Task<(bool, string)> EstablecerEstadoEmpresa(int id)
        {
            try
            {
                Empresa empresaEstado = await baseDeDatos.Empresa.FirstOrDefaultAsync(em => em.Id == id);
                if (empresaEstado != null)
                {
                    empresaEstado.Estado = !empresaEstado.Estado;
                    await baseDeDatos.SaveChangesAsync();
                    return (true, $"El registro se {(empresaEstado.Estado ? "activó" : "desactivó")} con éxito");
                }
                else return (false, "Ese registro no existe");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error: {ex.Message}");
                return (false, "Fallo al intentar cambiar el estado de la empresa, consulte con el administrador");
            }
        }
    }
}