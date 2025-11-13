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

        public async Task<Response<List<DepositoEmpresaDTO>>> ObtenerDepositosDeEmpresa(long EmpresaId)
        {
            try
            {
                var res = await baseDeDatos.Depositos.Include(d => d.Obra)
                                .Where(d => d.Obra.EmpresaId == EmpresaId).ToListAsync();

                if (res.Count == 0) return new Response<List<DepositoEmpresaDTO>>()
                { Objeto = [], Estado = true, Mensaje = "Aún no existen depósitos en esta empresa." };

                return new Response<List<DepositoEmpresaDTO>>()
                {
                    Objeto = res.Select(d => new DepositoEmpresaDTO()
                    {
                        Id = d.Id,
                        CodigoDeposito = d.CodigoDeposito,
                        NombreDeposito = d.NombreDeposito,
                        TipoDeposito = d.TipoDeposito.ToString()
                    }).ToList(),
                    Estado = true,
                    Mensaje = "¡Depósitos cargados con éxito!"
                };
            }
            catch (Exception ex) { return new Response<List<DepositoEmpresaDTO>>() 
            { Objeto = null, Estado = false, Mensaje = "¡Hubo un error desde el servidor al cargar los depositos!" }; }
        }

        public async Task<Response<List<VerDepositoDTO>>>ObtenerDepositoPorId(int id)
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

        public async Task<Response<List<VerDepositoDTO>>> ObtenerDepositosPorObraId(int obraId)
        {
            try
            {
                var depositos = await baseDeDatos.Depositos.Where(o => o.ObraId == obraId).ToListAsync();

                if (depositos != null && depositos.Count > 0)
                {
                
                    return new Response<List<VerDepositoDTO>>
                    {
                        Objeto = depositos.Select(deposito => new VerDepositoDTO
                        {
                            Id = deposito.Id,
                            CodigoDeposito = deposito.CodigoDeposito,
                            NombreDeposito = deposito.NombreDeposito,
                            TipoDeposito = deposito.TipoDeposito.ToString() 
                            == "EnUso" ? "En uso" : deposito.TipoDeposito.ToString(),
                        }).ToList(),
                        Mensaje = "Depósitos obtenidos exitosamente.",
                        Estado = true
                    };
                }
                else
                {
                    return new Response<List<VerDepositoDTO>>
                    {
                        Objeto = null,
                        Mensaje = "No existen depósitos para esta obra.",
                        Estado = true
                    };
                }
            }
            catch (Exception ex) 
            { 
                return new Response<List<VerDepositoDTO>>
                {
                    Objeto = null,
                    Mensaje = "Error al obtener los depósitos.",
                    Estado = false
                };
            }
            
        }

        public async Task<Response<int>>CrearDeposito(DepositoAsociarDTO e)
        {
            try
            {
                var nuevoDeposito = new Deposito
                {
                    CodigoDeposito = e.CodigoDeposito,
                    NombreDeposito = e.NombreDeposito,
                    Domicilio = e.Domicilio,
                    ObraId = e.ObraId,
                    TipoDeposito = (BD.Enums.EnumTipoDeposito)e.TipoDeposito,
                };
                await baseDeDatos.Depositos.AddAsync(nuevoDeposito);
                await baseDeDatos.SaveChangesAsync();

               return new Response<int>
                {
                    Objeto = (int)nuevoDeposito.Id,
                    Mensaje = "Depósito creado exitosamente.",
                    Estado = true
                };
            }
            catch (Exception )
            {

                return new Response<int>
                {
                    Objeto = 0,
                    Mensaje = "Error al obtener los depósitos.",
                    Estado = false
                };
            }
        }
        public async Task<Response<List<VerDepositoDTO>>>ObtenerDepositosPorUsuario(DatosUsuario usuario) 
        {
            try
            {
                if (usuario == null) 
                return new Response<List<VerDepositoDTO>>()
                {
                    Objeto = null,
                    Estado = true,
                    Mensaje = "No hay un usuario logueado"
                };
                
                if (usuario.Roles.Contains("ADMINISTRADOR")) 
                { 
                    var depositos = await baseDeDatos.Depositos.
                        Where(d => d.ObraId == usuario.EmpresaId)
                        .Select(d => new VerDepositoDTO()
                        {
                            Id = d.Id,
                            CodigoDeposito = d.CodigoDeposito,
                            NombreDeposito = d.NombreDeposito,
                            TipoDeposito = d.TipoDeposito.ToString()
                        })
                        .ToListAsync();
                    return new Response<List<VerDepositoDTO>>()
                    {
                        Objeto = depositos,
                        Estado = true,
                        Mensaje = "¡Depositos cargados con exito!"
                    };
                }
                else if (usuario.Roles.Contains("JEFEDEOBRA")) 
                {
                    var depositos = await baseDeDatos.Depositos.
                        Where(d => usuario.DepositosId.Contains(d.Id))
                        .Select(d => new VerDepositoDTO()
                        {
                            Id = d.Id,
                            CodigoDeposito = d.CodigoDeposito,
                            NombreDeposito = d.NombreDeposito,
                            TipoDeposito = d.TipoDeposito.ToString()
                        })
                        .ToListAsync();
                    return new Response<List<VerDepositoDTO>>()
                    {
                        Objeto = depositos,
                        Estado = true,
                        Mensaje = "Depositos cargados con exito."
                    };
                }
                else if (usuario.Roles.Contains("JEFEDEDEPOSITO")) 
                {
                    var depositos = await baseDeDatos.Depositos.
                        Where(d => usuario.DepositosId.Contains(d.Id))
                        .Select(d => new VerDepositoDTO()
                        {
                            Id = d.Id,
                            CodigoDeposito = d.CodigoDeposito,
                            NombreDeposito = d.NombreDeposito,
                            TipoDeposito = d.TipoDeposito.ToString()
                        })
                        .ToListAsync();
                    return new Response<List<VerDepositoDTO>>()
                    {
                        Objeto = depositos,
                        Estado = true,
                        Mensaje = "Depositos cargados con exito."
                    };
                }
                else
                {
                    return new Response<List<VerDepositoDTO>>()
                    {
                        Objeto = null,
                        Estado = true,
                        Mensaje = "Acceso Denegado."
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

        public async Task<Response<string>> ActualizarDeposito(int id,DepositoAsociarDTO e)
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

        public async Task<Response<string>> EliminarDeposito(long id)
        {
            try
            {
                var deposito = await baseDeDatos.Depositos.FirstOrDefaultAsync(d => d.Id == id);
                if (deposito == null)
                {
                    return new Response<string>
                    {
                        Objeto = null,
                        Mensaje = "No existe un depósito con ese ID.",
                        Estado = false
                    };
                }

                baseDeDatos.Depositos.Remove(deposito);
                await baseDeDatos.SaveChangesAsync();

                return new Response<string>
                {
                    Objeto = deposito.Id.ToString(),
                    Mensaje = "Depósito eliminado exitosamente.",
                    Estado = true
                };
            }
            catch (Exception)
            {
                return new Response<string>
                {
                    Objeto = null,
                    Mensaje = "Error al eliminar el depósito.",
                    Estado = false
                };
            }
        }
    }
}