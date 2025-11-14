using BD;
using BD.Modelos;
using DTO.DTOs_Depositos;
using Repositorios.Implementaciones;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO.DTOs_Response;
using DTO.DTOs_Usuarios;
using System.ComponentModel.Design;

namespace Repositorios.Servicios
{
    public class DepositoServicio : IDepositoServicio
    {
        private readonly AppDbContext baseDeDatos;

        public DepositoServicio(AppDbContext BaseDeDatos)
        {
            this.baseDeDatos = BaseDeDatos;
        }

        public async Task<Response<List<VerDepositoDTO>>> ObtenerDepositoPorId(int id)
        {
            try
            {
                var deposito = await baseDeDatos.Depositos.FirstOrDefaultAsync(d => d.Id == id);

                if (deposito != null)
                {
                    var depositoDTO = new VerDepositoDTO
                    {
                        Id = deposito.Id,
                        CodigoDeposito = deposito.CodigoDeposito,
                        NombreDeposito = deposito.NombreDeposito,
                        TipoDeposito = deposito.TipoDeposito.ToString()
                        == "EnUso" ? "En uso" : deposito.TipoDeposito.ToString(),
                    };
                    return new Response<List<VerDepositoDTO>>
                    {
                        Objeto = new List<VerDepositoDTO> { depositoDTO },
                        Mensaje = "Depósito encontrado.",
                        Estado = true
                    };
                }
                else
                {
                    return new Response<List<VerDepositoDTO>>
                    {
                        Objeto = null,
                        Mensaje = "No existe un depósito con ese ID.",
                        Estado = true
                    };
                }
            }
            catch (Exception ex)
            {
                return new Response<List<VerDepositoDTO>>
                {
                    Objeto = null,
                    Mensaje = "Error al obtener el depósito.",
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
                            TipoDeposito = d.TipoDeposito.ToString(),
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

        public async Task<Response<string>> ActualizarDeposito(int id, DepositoAsociarDTO e)
        {
            try
            {
                Deposito? deposito = await baseDeDatos.Depositos
                    .FirstOrDefaultAsync(d => d.Id == e.Id);
                if (deposito == null)
                {
                    return new Response<string>
                    {
                        Objeto = null,
                        Mensaje = "El depósito no existe.",
                        Estado = false
                    };
                }

                bool existeCodigo = await baseDeDatos.Depositos
                            .AnyAsync(d => d.CodigoDeposito == e.CodigoDeposito && d.Id != e.Id);

                if (existeCodigo)
                {
                    return new Response<string>
                    {
                        Objeto = null,
                        Mensaje = "Ese código ya está en uso",
                        Estado = false
                    };
                }

                deposito.CodigoDeposito = e.CodigoDeposito;
                deposito.NombreDeposito = e.NombreDeposito;
                deposito.ObraId = e.ObraId;
                deposito.TipoDeposito = (BD.Enums.EnumTipoDeposito)e.TipoDeposito;

                baseDeDatos.Depositos.Update(deposito);
                await baseDeDatos.SaveChangesAsync();
                return new Response<string>
                {
                    Objeto = deposito.Id.ToString(),
                    Mensaje = "Depósito actualizado exitosamente.",
                    Estado = true
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return new Response<string>
                {
                    Objeto = null,
                    Mensaje = "Error al actualizar el depósito.",
                    Estado = false
                };
            }
        }
    }
}