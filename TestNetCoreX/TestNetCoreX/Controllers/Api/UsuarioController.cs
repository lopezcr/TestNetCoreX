using Microsoft.AspNetCore.Authorization;
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
            return context.Usuario.ToList();
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] UsuarioCreacionDTO UsuarioDtoNuevo)
        {
            var respuesta = new Response<string>();

            if (context.Usuario.Where(x => x.Nombre == UsuarioDtoNuevo.Nombre).Count() > 0)
            {
                respuesta.IsSuccess = false;
                respuesta.Message = "El nombre del usuario ya existe";
                return Ok(respuesta);
            }

            if (context.Usuario.Where(x => x.Email == UsuarioDtoNuevo.Email).Count() > 0)
            {
                respuesta.IsSuccess = false;
                respuesta.Message = "El email ya ha sido registrado";
                return Ok(respuesta);
            }

            try
            {
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
            }
            catch(Exception ex)
            {
                respuesta.IsSuccess = false;
                respuesta.Message = ex.Message;
                return Ok(respuesta);
            }

            respuesta.IsSuccess = true;
            return Ok(respuesta);
        }

        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] UsuarioCreacionDTO AutorActualizar)
        {
            var respuesta = new Response<string>();
             
            try
            {
                var Usuario = context.Usuario.Where(x => x.ID == id).FirstOrDefault();
                if (Usuario == null)
                {
                    respuesta.IsSuccess = false;
                    respuesta.Message = "El usuario no existe";
                    return Ok(respuesta);
                }

                if(context.Usuario.Where(x => x.ID != id && x.Email == AutorActualizar.Email).FirstOrDefault() != null)
                {
                    respuesta.IsSuccess = false;
                    respuesta.Message = "Existe otro usuario registrado con el mismo email";
                    return Ok(respuesta);
                }

                if (context.Usuario.Where(x => x.ID != id && x.Nombre == AutorActualizar.Nombre).FirstOrDefault() != null)
                {
                    respuesta.IsSuccess = false;
                    respuesta.Message = "El nombre de usuario ya esta registrado";
                    return Ok(respuesta);
                }
                string passwordEncritado = Encrypt.GetMD5(AutorActualizar.PassWord);
                Usuario.Nombre = AutorActualizar.Nombre;
                Usuario.Email = AutorActualizar.Email;
                Usuario.Sexo = AutorActualizar.Sexo;
                Usuario.PassWord = passwordEncritado;
                Usuario.Activo= true;

                context.SaveChanges();
            }
            catch(Exception ex)
            {
                respuesta.IsSuccess = false;
                respuesta.Message = ex.Message;
                return Ok(respuesta);
            }
             
            respuesta.IsSuccess = true;
            return Ok(respuesta);
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult<Usuario>> Delete(int id)
        {

            var respuesta = new Response<Usuario>();             
            try
            {
                var usuario = await context.Usuario.Where(x => x.ID == id).FirstOrDefaultAsync();

                if (usuario == null)
                {
                    respuesta.IsSuccess = false;
                    respuesta.Message = "No se encontro el usuario";
                    return Ok(respuesta);
                }
                usuario.Activo = false; 
                await context.SaveChangesAsync();
            }
            catch
            {
                respuesta.IsSuccess = false;
                respuesta.Message = "Error al eliminar el usuario";
                return Ok(respuesta);
            }
             
            respuesta.IsSuccess = true; 
            return Ok(respuesta);
        }


        
        [HttpPost("Login")]
        public ActionResult PostLogin([FromBody] LoginDTO LoginDTO)
        {
            var respuesta = new Response<string>();

            string passwordEncritado = Encrypt.GetMD5(LoginDTO.Password);
            var usuario = context.Usuario.Where(x => x.Email == LoginDTO.Email && x.PassWord == passwordEncritado).FirstOrDefault();
            if (usuario == null)
            {
                respuesta.IsSuccess = false;
                respuesta.Message = "El usuario no existe";
                return Ok(respuesta);
            }

            HttpContext.Session.SetString("IsLogin", "true");
            respuesta.IsSuccess = true;
            return Ok(respuesta);
        }

        [HttpGet("{id}", Name = "ObtenerUsuario")] 
        public async Task<ActionResult<Response<Usuario>>> Get(int id)
        {
            var respuesta = new Response<Usuario>();
            var usuario = await context.Usuario.Where(x => x.ID == id).FirstOrDefaultAsync();

            if (usuario == null)
            {
                respuesta.IsSuccess = false;
                respuesta.Message="No se encontro el usuario";
                return Ok(respuesta);
            }
            respuesta.IsSuccess = true;
            respuesta.Result = usuario;
            return Ok(respuesta);
        }


        [HttpPost("Registro")]
        public async Task<ActionResult> PostRegistro([FromBody] UsuarioCreacionDTO UsuarioDtoNuevo)
        {
            var respuesta = new Response<string>();

            if (context.Usuario.Where(x => x.Nombre == UsuarioDtoNuevo.Nombre).Count() > 0)
            {
                respuesta.IsSuccess = false;
                respuesta.Message = "El nombre del usuario ya existe";
                return Ok(respuesta);
            }

            if (context.Usuario.Where(x => x.Email == UsuarioDtoNuevo.Email).Count() > 0)
            {
                respuesta.IsSuccess = false;
                respuesta.Message = "El email ya ha sido registrado";
                return Ok(respuesta);
            }

            try
            {
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
                HttpContext.Session.SetString("IsLogin", "true");
            }
            catch (Exception ex)
            {
                respuesta.IsSuccess = false;
                respuesta.Message = ex.Message;
                return Ok(respuesta);
            }

            respuesta.IsSuccess = true;
            return Ok(respuesta);
        }
    }
}
