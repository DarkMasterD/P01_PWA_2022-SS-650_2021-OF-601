using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using P01_2022_SS_650_2021_OF_601.Models;
using System.Collections;

namespace P01_2022_SS_650_2021_OF_601.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class sucursalController : ControllerBase
    {
        private readonly parqueoContext _parqueoContext;

        public sucursalController(parqueoContext parqueoContext)
        {
            _parqueoContext = parqueoContext;
        }

        [HttpGet]
        [Route("GetAllSucursales")]
        public IActionResult ListarSucursal()
        {
            var sucursales = (from s in _parqueoContext.sucursal
                                         join u in _parqueoContext.usuario
                                         on s.id_usuario equals u.id_usuario
                                              select new
                                              {
                                                  s.id_sucursal,
                                                  administrador = u.nombre,
                                                  s.nombre,
                                                  s.direccion,
                                                  s.telefono,
                                                  s.numero_espacios
                                              }).ToList();

            if (sucursales.Count() == 0)
            {
                return NotFound();
            }

            return Ok(sucursales);
        }

        [HttpPost]
        [Route("AddSucursal")]
        public IActionResult GuardarSucursal([FromBody] sucursal sucur)
        {
            try
            {
                _parqueoContext.sucursal.Add(sucur);
                _parqueoContext.SaveChanges();
                return Ok(sucur);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("UpdateSucursal/{id}")]
        public IActionResult ActualizarSucursal(int id, [FromBody] sucursal sucursalModificar)
        {
            var sucursalActual = (from s in _parqueoContext.sucursal
                                 where s.id_sucursal == id
                                 select s).FirstOrDefault();

            if (sucursalActual == null)
            {
                return NotFound();
            }

            sucursalActual.nombre = sucursalModificar.nombre;
            sucursalActual.direccion = sucursalModificar.direccion;
            sucursalActual.telefono = sucursalModificar.telefono;
            sucursalActual.id_usuario = sucursalModificar.id_usuario;
            sucursalActual.numero_espacios = sucursalModificar.numero_espacios;

            _parqueoContext.Entry(sucursalActual).State = EntityState.Modified;
            _parqueoContext.SaveChanges();

            return Ok(sucursalModificar);
        }

        [HttpDelete]
        [Route("DeleteSucursal/{id}")]
        public IActionResult EliminarSucursal(int id)
        {
            var usuario = (from s in _parqueoContext.sucursal
                           where s.id_sucursal == id
                           select s).FirstOrDefault();

            if (usuario == null)
            {
                return NotFound();
            }

            _parqueoContext.sucursal.Attach(usuario);
            _parqueoContext.sucursal.Remove(usuario);
            _parqueoContext.SaveChanges();

            return Ok(usuario);
        }

        [HttpGet]
        [Route("UnoccupiedEspacio/{id_sucursal}/{dias}")]
        public IActionResult BuscarEspacioDisp(int id_sucursal, int dias)
        {
            DateTime fechahoy = DateTime.Now;
            DateOnly fechanew = DateOnly.FromDateTime(fechahoy);
            var espacios = (from e in _parqueoContext.espacios where e.id_sucursal == id_sucursal select e).ToList();
            var reservas = (from e in _parqueoContext.espacios
                            join r in _parqueoContext.reserva
                            on e.id_espacios equals r.id_espacios
                            where e.id_sucursal == id_sucursal && r.fecha == fechanew
                            select new
                            {
                                e.id_espacios,
                                e.numero,
                                Fecha = r.fecha.ToDateTime(TimeOnly.MinValue),
                                r.hora_reserva,
                                r.horas,
                                r.estado
                            }).OrderBy(x => x.Fecha).ToList();

            if (espacios == null || reservas == null)
            {
                return NotFound();
            }

            List<string> lista = new List<string>();

            foreach(var esp in espacios)
            {
                int num = 1;
                for (int i = 0; i <= reservas.Count; i++) 
                {
                    string desocupado = "";
                    TimeOnly horaIni = new TimeOnly(0,0);
                    TimeOnly horaRes = reservas[i].hora_reserva;
                    TimeOnly horaFin = horaRes.AddHours(reservas[i].horas);
                    if (horaIni <= horaRes)
                    {
                        desocupado = "Espacio numero " + reservas[i].numero + " puede reservar el " + reservas[i].Fecha + " a desde las " + horaIni + " hasta las " + horaRes;
                    }
                    else
                    {
                        horaIni = horaFin;
                    }

                    lista.Add(desocupado);
                }
            }

            return Ok(lista);
        }

        [HttpPost]
        [Route("AddEspacio")]
        public IActionResult NuevoEspacio([FromBody] espacios espacio)
        {
            try
            {
                _parqueoContext.espacios.Add(espacio);
                _parqueoContext.SaveChanges();
                return Ok(espacio);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("UpdateEspacio/{id}")]
        public IActionResult ModificarEspacio( int id, [FromBody] espacios espacioModificar)
        {
            try
            {
                var espacioActual = (from e in _parqueoContext.espacios where e.id_espacios == id select e).FirstOrDefault();

                if (espacioActual == null) 
                {
                    return NotFound();                
                }

                espacioActual.id_sucursal = espacioModificar.id_sucursal;
                espacioActual.numero = espacioModificar.numero;
                espacioActual.ubicacion = espacioModificar.ubicacion;
                espacioActual.costo_hora = espacioModificar.costo_hora;

                _parqueoContext.Entry(espacioActual).State = EntityState.Modified;
                _parqueoContext.SaveChanges();

                return Ok(espacioModificar);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("DeleteEspacio/{id}")]
        public IActionResult EliminarEspacio(int id)
        {
            var espacio = (from e in _parqueoContext.espacios where e.id_espacios == id select e).FirstOrDefault();

            if(espacio == null)
            {
                return NotFound();
            }

            _parqueoContext.espacios.Attach(espacio);
            _parqueoContext.espacios.Remove(espacio);
            _parqueoContext.SaveChanges();

            return Ok(espacio);
        }
    }
}
