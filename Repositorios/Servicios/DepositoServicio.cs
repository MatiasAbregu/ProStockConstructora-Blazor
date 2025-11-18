using BD;
using BD.Modelos;
using DTO.DTOs_Depositos;
using DTO.DTOs_Obras;
using DTO.DTOs_Response;
using DTO.DTOs_Usuarios;
using Microsoft.EntityFrameworkCore;
using Repositorios.Implementaciones;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositorios.Servicios
{
    public class DepositoServicio : IDepositoServicio
    {
        private readonly AppDbContext baseDeDatos;

        public DepositoServicio(AppDbContext BaseDeDatos)
        {
            this.baseDeDatos = BaseDeDatos;
        }

        public async Task<Response<List<DepositoEmpresaDTO>>> ObtenerDepositosDeEmpresa(long EmpresaId)
        {
            try
            {
                var depositos = await baseDeDatos.Depositos.Include(d => d.Obra)
                    .Where(o => o.Obra.EmpresaId == EmpresaId).ToListAsync();

                if (depositos.Count == 0)
                {
                    return new Response<List<DepositoEmpresaDTO>>()
                    { Objeto = [], Mensaje = "Aún no existen depósitos para esta empresa.", Estado = true };
                }

                return new Response<List<DepositoEmpresaDTO>>()
                {
                    Objeto = depositos.Select(d => new DepositoEmpresaDTO()
                    {
                        Id = d.Id,
                        CodigoDeposito = d.CodigoDeposito,
                        NombreDeposito = d.NombreDeposito,
                        TipoDeposito = d.TipoDeposito.ToString() == "EnUso" ? "En uso" : d.TipoDeposito.ToString(),
                        Obra = $"{d.Obra.CodigoObra}"
                    }).ToList(),
                    Estado = true,
                    Mensaje = "¡Obras cargadas con éxito!"
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.InnerException.Message}");
                return new Response<List<DepositoEmpresaDTO>>()
                {
                    Objeto = null,
                    Estado = false,
                    Mensaje = $"¡Hubo un error desde el servidor al cargar los depósitos!"
                };
            }
        }

        public async Task<Response<DepositoActualizarDTO>> ObtenerDepositoPorId(long id)
        {
            try
            {
                var deposito = await baseDeDatos.Depositos.FirstOrDefaultAsync(d => d.Id == id);

                if (deposito == null)
                    return new Response<DepositoActualizarDTO>()
                    {
                        Objeto = null,
                        Mensaje = "No existe el depósito con el ID proporcionado.",
                        Estado = true
                    };

                var depositoDTO = new DepositoActualizarDTO()
                {
                    Id = deposito.Id,
                    CodigoDeposito = deposito.CodigoDeposito,
                    NombreDeposito = deposito.NombreDeposito,
                    Domicilio = deposito.Domicilio,
                    TipoDeposito = (DTO.Enum.EnumTipoDeposito)deposito.TipoDeposito,
                };

                return new Response<DepositoActualizarDTO>
                {
                    Objeto = depositoDTO,
                    Mensaje = "Depósito obtenido con éxito.",
                    Estado = true
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.InnerException.Message}");
                return new Response<DepositoActualizarDTO>
                {
                    Objeto = null,
                    Mensaje = "¡Hubo un error desde el servidor al cargar la obra!",
                    Estado = false
                };
            }
        }

        public async Task<Response<List<VerDepositoDTO>>> ObtenerDepositosPorUsuario(DatosUsuario usuario, long? ObraId)
        {
            try
            {
                if (usuario == null)
                    return new Response<List<VerDepositoDTO>>()
                    {
                        Objeto = null,
                        Estado = true,
                        Mensaje = "No hay un usuario logueado."
                    };

                if (usuario.Roles.Contains("ADMINISTRADOR") || usuario.Roles.Contains("JEFEDEOBRA"))
                {
                    if (ObraId == null) return new Response<List<VerDepositoDTO>>()
                    {
                        Objeto = null,
                        Estado = true,
                        Mensaje = "Es obligatorio pasar una ID de obra en los roles administrador o jefe de obra."
                    };

                    var depositos = await baseDeDatos.Depositos.Include(d => d.Obra)
                        .Where(d => d.Obra.Id == ObraId)
                        .Select(d => new VerDepositoDTO()
                        {
                            Id = d.Id,
                            CodigoDeposito = d.CodigoDeposito,
                            NombreDeposito = d.NombreDeposito,
                            TipoDeposito = d.TipoDeposito.ToString() == "EnUso" ? "En uso" : d.TipoDeposito.ToString(),
                            Domicilio = d.Domicilio
                        })
                        .ToListAsync();
                    return new Response<List<VerDepositoDTO>>()
                    {
                        Objeto = depositos,
                        Estado = true,
                        Mensaje = depositos.Count > 0 ? "¡Depósitos cargados con exito!" : "¡Aún no hay depósitos cargados en el sistema!"
                    };
                }
                else if (usuario.Roles.Contains("JEFEDEDEPOSITO"))
                {
                    var depositos = await baseDeDatos.Depositos.
                        Where(d => usuario.DepositosId.Contains(d.Id)).Include(d => d.Obra)
                        .Select(d => new VerDepositoDTO()
                        {
                            Id = d.Id,
                            CodigoDeposito = d.CodigoDeposito,
                            NombreDeposito = d.NombreDeposito,
                            TipoDeposito = d.TipoDeposito.ToString() == "EnUso" ? "En uso" : d.TipoDeposito.ToString(),
                            Domicilio = d.Domicilio,
                            ObraId = d.Obra.Id,
                            Obra = $"{d.Obra.NombreObra}({d.Obra.CodigoObra})"
                        })
                        .ToListAsync();
                    return new Response<List<VerDepositoDTO>>()
                    {
                        Objeto = depositos,
                        Estado = true,
                        Mensaje = depositos.Count > 0 ? "¡Depósitos cargados con exito!" : "¡Aún no hay depósitos cargados en el sistema!"
                    };
                }
                else
                {
                    return new Response<List<VerDepositoDTO>>()
                    {
                        Objeto = null,
                        Estado = true,
                        Mensaje = "Acceso denegado."
                    };
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error:{ex.Message}");
                return new Response<List<VerDepositoDTO>>()
                {
                    Objeto = null,
                    Estado = false,
                    Mensaje = "Hubo un error desde el servidor al cargar los depositos."
                };
            }
        }

        public async Task<Response<string>> CrearDeposito(DepositoAsociarDTO e)
        {
            try
            {
                bool existe = await baseDeDatos.Depositos.AnyAsync(d => d.CodigoDeposito == e.CodigoDeposito);
                if (existe) return new Response<string>
                {
                    Objeto = null,
                    Mensaje = "Ese código ya está en uso",
                    Estado = true
                };

                var nuevoDeposito = new Deposito
                {
                    CodigoDeposito = e.CodigoDeposito,
                    NombreDeposito = e.NombreDeposito,
                    Domicilio = e.Domicilio,
                    ObraId = e.ObraId,
                    TipoDeposito = (BD.Enums.EnumTipoDeposito)e.TipoDeposito,
                };
                baseDeDatos.Depositos.Add(nuevoDeposito);
                await baseDeDatos.SaveChangesAsync();

                return new Response<string>
                {
                    Objeto = "¡Depósito creado con éxito!",
                    Mensaje = null,
                    Estado = true
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return new Response<string>
                {
                    Objeto = null,
                    Mensaje = "¡Hubo un error desde el servidor al crear el usuario!",
                    Estado = false
                };
            }
        }

        public async Task<Response<string>> ActualizarDeposito(long id, DepositoAsociarDTO e)
        {
            try
            {
                Deposito? deposito = await baseDeDatos.Depositos
                    .FirstOrDefaultAsync(d => d.Id == e.Id);
                if (deposito == null)
                    return new Response<string>
                    {
                        Objeto = null,
                        Mensaje = "No existe un depósito con ese ID.",
                        Estado = true
                    };

                bool existeCodigo = await baseDeDatos.Depositos
                            .AnyAsync(d => d.CodigoDeposito == e.CodigoDeposito && d.Id != e.Id);
                if (existeCodigo)
                    return new Response<string>
                    {
                        Objeto = null,
                        Mensaje = "Ya existe un depósito con ese código.",
                        Estado = true
                    };

                deposito.CodigoDeposito = e.CodigoDeposito;
                deposito.NombreDeposito = e.NombreDeposito;
                deposito.Domicilio = e.Domicilio;
                deposito.TipoDeposito = (BD.Enums.EnumTipoDeposito)e.TipoDeposito;

                await baseDeDatos.SaveChangesAsync();
                return new Response<string>
                {
                    Objeto = "Depósito actualizado con éxito.",
                    Mensaje = null,
                    Estado = true
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.InnerException.Message}");
                return new Response<string>
                {
                    Objeto = null,
                    Mensaje = "¡Hubo un error desde el servidor al actualizar el depósito!",
                    Estado = false
                };
            }
        }
    }
}