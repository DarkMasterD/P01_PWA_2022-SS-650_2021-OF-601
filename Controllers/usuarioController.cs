using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using P01_2022_SS_650_2021_OF_601.Models;

namespace P01_2022_SS_650_2021_OF_601.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class usuarioController : ControllerBase
    {
        private readonly parqueoContext _parqueoContext;

        public usuarioController(parqueoContext parqueoContext)
        {
            _parqueoContext = parqueoContext;
        }

        [HttpGet]
        [Route("GetAll")]
        public IActionResult Get()
        {
            List<usuario> listadoUsuarios = (from u in _parqueoContext.usuario
                                              select u).ToList();

            if (listadoUsuarios.Count() == 0)
            {
                return NotFound();
            }

            return Ok(listadoUsuarios);
        }

        [HttpPost]
        [Route("Add")]
        public IActionResult GuardarUsuario([FromBody] usuario usuario)
        {
            try
            {
                _parqueoContext.usuario.Add(usuario);
                _parqueoContext.SaveChanges();
                return Ok(usuario);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("actualizar/{id}")]
        public IActionResult ActualizarUsuario(int id, [FromBody] usuario usuarioModificar)
        {
            var usuarioActual = (from u in _parqueoContext.usuario
                                 where u.id_usuario == id
                                 select u).FirstOrDefault();

            if (usuarioActual == null)
            {
                return NotFound();
            }

            usuarioActual.nombre = usuarioModificar.nombre;
            usuarioActual.telefono = usuarioModificar.telefono;
            usuarioActual.contrasenia = usuarioModificar.contrasenia;

            _parqueoContext.Entry(usuarioActual).State = EntityState.Modified;
            _parqueoContext.SaveChanges();

            return Ok(usuarioModificar);
        }

        [HttpDelete]
        [Route("eliminar/{id}")]
        public IActionResult EliminarUsuario(int id)
        {
            var usuario = (from u in _parqueoContext.usuario
                           where u.id_usuario == id
                           select u).FirstOrDefault();

            if (usuario == null)
            {
                return NotFound();
            }

            _parqueoContext.usuario.Attach(usuario);
            _parqueoContext.usuario.Remove(usuario);
            _parqueoContext.SaveChanges();

            return Ok(usuario);
        }

        [HttpGet]
        [Route("GetCredenciales")]
        public IActionResult GetCredenciales(string nombre = null, string contrasenia = null)
        {
            var usuario = from u in _parqueoContext.usuario
                          select u;

            if (!string.IsNullOrEmpty(nombre))
            {
                usuario = usuario.Where(u => u.nombre.Contains(nombre));
            }
            if (!string.IsNullOrEmpty(contrasenia))
            {
                usuario = usuario.Where(u => u.contrasenia.Contains(contrasenia));
            }

            var usuariofiltrado = usuario.ToList();

            if (usuariofiltrado.Count == 0)
            {
                return NotFound("Credenciales incorrectas.");
            }

            return Ok("Credenciales correctas");
        }
    }
}
