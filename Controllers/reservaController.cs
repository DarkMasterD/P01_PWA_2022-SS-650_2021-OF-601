using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using P01_2022_SS_650_2021_OF_601.Models;

namespace P01_2022_SS_650_2021_OF_601.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class reservaController : ControllerBase
    {
        private readonly parqueoContext _parqueoContext;

        public reservaController(parqueoContext parqueoContext)
        {
            _parqueoContext = parqueoContext;
        }

        [HttpPost]
        [Route("Add")]
        public IActionResult GuardarReserva([FromBody] reserva reserva)
        {
            try
            {
                _parqueoContext.reserva.Add(reserva);
                _parqueoContext.SaveChanges();
                return Ok(reserva);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetById/{id}")]
        public IActionResult Get(int id)
        {
            var reservas = (from r in _parqueoContext.reserva
                            join u in _parqueoContext.usuario on r.id_usuario equals u.id_usuario
                            where r.id_usuario == id
                            select new
                            {
                                u.nombre,
                                r.fecha,
                                r.hora_reserva,
                                r.horas
                            }).ToList();

            if (!reservas.Any())
            {
                return NotFound("No se encontraron reservas para este usuario.");
            }
            return Ok(reservas);
        }


        [HttpDelete]
        [Route("CancelarReserva/{id}")]
        public IActionResult CancelarReserva(int id)
        {
            var reserva = _parqueoContext.reserva.FirstOrDefault(r => r.id_reserva == id);

            if (reserva == null)
            {
                return NotFound("Reserva no encontrada.");
            }

            var ahora = DateOnly.FromDateTime(DateTime.Now);
            var horaActual = TimeOnly.FromDateTime(DateTime.Now);

            if (reserva.fecha < ahora || (reserva.fecha == ahora && reserva.hora_reserva <= horaActual))
            {
                return BadRequest("No se puede cancelar una reserva que ya ha iniciado.");
            }

            try
            {
                reserva.estado = 'C';
                _parqueoContext.SaveChanges();
                return Ok("Reserva cancelada exitosamente.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet]
        [Route("EspaciosReservadosPorDia")]
        public IActionResult EspaciosReservadosPorDia([FromQuery] DateTime fecha)
        {
            try
            {
                var fechaSinHora = DateOnly.FromDateTime(fecha);

                var reservas = _parqueoContext.reserva
                    .Where(r => r.fecha == fechaSinHora) 
                    .ToList();

                return Ok(reservas);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet]
        [Route("EspaciosReservadosEntreFechas")]
        public IActionResult ObtenerEspaciosReservadosEntreFechas([FromQuery] DateTime fechaInicio, [FromQuery] DateTime fechaFin)
        {
            try
            {
                var fechaInicioOnly = DateOnly.FromDateTime(fechaInicio); 
                var fechaFinOnly = DateOnly.FromDateTime(fechaFin);      

                var reservas = _parqueoContext.reserva
                    .Where(r => r.fecha >= fechaInicioOnly && r.fecha <= fechaFinOnly) 
                    .ToList();

                return Ok(reservas);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


    }
}
