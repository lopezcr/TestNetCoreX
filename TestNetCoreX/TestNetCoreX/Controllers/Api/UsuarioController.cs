using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestNetCoreX.AccessData;
using TestNetCoreX.AccessData.Entities;
using TestNetCoreX.BusinessObject.Model;
using TestNetCoreX.Common.Helpers;

namespace TestNetCoreX.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        public UsuarioController(ApplicationDbContext context)
        {
            this.context = context;
        }
 
        [HttpGet]
        public ActionResult<IEnumerable<Usuario>> Get()
        {
            return context.Usuario.Where(x => x.Activo == true).ToList();
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] UsuarioCreacionDTO UsuarioDtoNuevo)
        {
            if (context.Usuario.Where(x => x.Nombre == UsuarioDtoNuevo.Nombre).Count() > 0)
            {

                return BadRequest("El nombre de usuario ya existe");
            }

            if (context.Usuario.Where(x => x.Email == UsuarioDtoNuevo.Email).Count() > 0)
            {

                return BadRequest("El email ya ha sido registrado");
            }

            string passwordEncritado= Encrypt.GetMD5(UsuarioDtoNuevo.PassWord);
            var UsuarioNuevo = new Usuario()
            {
                Nombre = UsuarioDtoNuevo.Nombre,
                FechaRegistro = DateTime.Now,
                PassWord = passwordEncritado,
                Activo = true,
                Email = UsuarioDtoNuevo.Email,
                Sexo = UsuarioDtoNuevo.Sexo,
            };

            context.Usuario.Add(UsuarioNuevo);
            await context.SaveChangesAsync();

             
            return Ok("Se genero correctamente el usuario");
        }

        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] UsuarioCreacionDTO AutorActualizar)
        {
             
            var Usuario = context.Usuario.Where(x => x.ID == id).FirstOrDefault();

            if (Usuario == null)
            {
                return BadRequest("Usuario no encontrado");
            }

            Usuario.Nombre = AutorActualizar.Nombre;
            Usuario.Email = AutorActualizar.Email;
            Usuario.Sexo = AutorActualizar.Sexo;
            Usuario.PassWord = AutorActualizar.PassWord;

            context.SaveChanges();
            return Ok();
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult<Usuario>> Delete(int id)
        {
            int autorId = await context.Usuario.Select(x => x.ID).FirstOrDefaultAsync(x => x == id);
            if (autorId == default(int))
            {
                return NotFound();
            }
            context.Usuario.Remove(new Usuario { ID = autorId });
            await context.SaveChangesAsync();
            return Ok("Usuario eliminado correctamente");
        }


        [HttpPost("Login")]
        public async Task<ActionResult> PostLogin([FromBody] UsuarioCreacionDTO UsuarioDtoNuevo)
        {
            if (context.Usuario.Where(x => x.Nombre == UsuarioDtoNuevo.Nombre).Count() > 0)
            {

                return BadRequest("El nombre de usuario ya existe");
            }

            if (context.Usuario.Where(x => x.Email == UsuarioDtoNuevo.Email).Count() > 0)
            {

                return BadRequest("El email ya ha sido registrado");
            }

            string passwordEncritado = Encrypt.GetMD5(UsuarioDtoNuevo.PassWord);
            var UsuarioNuevo = new Usuario()
            {
                Nombre = UsuarioDtoNuevo.Nombre,
                FechaRegistro = DateTime.Now,
                PassWord = passwordEncritado,
                Activo = true,
                Email = UsuarioDtoNuevo.Email,
                Sexo = UsuarioDtoNuevo.Sexo,
            };

            context.Usuario.Add(UsuarioNuevo);
            await context.SaveChangesAsync();


            return Ok("Se genero correctamente el usuario");
        }
    }
}
