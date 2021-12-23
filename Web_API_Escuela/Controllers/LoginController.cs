using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Web_API_Escuela.DTOs.Login;
using Web_API_Escuela.Entities;

namespace Web_API_Escuela.Controllers
{
    [Route("api/login")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IConfiguration configuration;

        public LoginController(ApplicationDbContext context, IConfiguration configuration)
        {
            this.context = context;
            this.configuration = configuration;
        }


        //POST: api/login/usuario
        [HttpPost("usuario")]
        public async Task<ActionResult<RespuestaAutenticacionDTO>> Usuario(LoginUsuarioDTO model)
        {
            var correo = model.Correo.ToLower();

            var usuario = await context.Usuarios.FirstOrDefaultAsync(x => x.Correo == correo && x.Estado == true);

            if (usuario == null)
            {
                return BadRequest("El usuario no existe.");
            }

            //Verififcar contraseña 
            if (!VerificarPasswordHash(model.Password, usuario.Password_hash, usuario.Password_salt))
            {
                return BadRequest("Login incorrecto");
            }

            return ConstruirToken(usuario);
        }

        //POST : api/login/alumno
        [HttpPost("alumno")]
        public async Task<ActionResult<RespuestaAutenticacionDTO>> Alumno(LoginAlumnoDTO model)
        {
            var curp = model.Curp.ToUpper();

            var alumno = await context.Alumnos.Include(x => x.Grupo).FirstOrDefaultAsync(x => x.Curp == curp && x.Estado == true);
            if (alumno == null)
            {
                return BadRequest("El alumno no existe.");
            }

            //Verifico contraseña
            if (!VerificarPasswordHash(model.Password, alumno.Password_hash, alumno.Password_salt))
            {
                return BadRequest("Login incorrecto");
            }

            return ConstruirTokenAlumno(alumno);

        }

        [HttpGet("DatosUsuario/{idUsuario:int}/{rol}")]
        public async Task<ActionResult<DatosUsuarioDTO>> DatosUsuario([FromRoute] int idUsuario, string rol)
        {
            //consulto
            if (rol == "Administrador" || rol == "Docente")//es usuario (administrador/docente)
            {
                var usuario = await context.Usuarios.FirstOrDefaultAsync(x => x.IdUsuario == idUsuario);

                if (usuario == null)
                {
                    return NotFound();
                }

                DatosUsuarioDTO datos = new()
                {
                    Nombre = usuario.Nombre,
                    ApellidoPaterno = usuario.ApellidoPaterno,
                    ApellidoMaterno = usuario.ApellidoMaterno,
                    Rol = rol,
                    Correo = usuario.Correo
                };

                return datos;

            }
            else if (rol == "Alumno")
            {
                var alumno = await context.Alumnos.FirstOrDefaultAsync(x => x.IdAlumno == idUsuario);

                if (alumno == null)
                {
                    return NoContent();
                }

                DatosUsuarioDTO datosAlumno = new()
                {
                    Nombre = alumno.Nombre,
                    ApellidoPaterno = alumno.ApellidoPaterno,
                    ApellidoMaterno = alumno.ApellidoMaterno,
                    Rol = rol,
                    Correo = alumno.Correo
                };

                return datosAlumno;

            }

            return NotFound();

        }

        [HttpPut("CambiarPassword/{idUsuario:int}/{rol}")]
        public async Task<ActionResult> CambiarPassword([FromRoute] int idUsuario, string rol, [FromBody] ActualizarPasswordDTO actualizarPasswordDTO)
        {
            //consulto
            if (rol == "Administrador" || rol == "Docente")//es usuario (administrador/docente)
            {
                var usuario = await context.Usuarios.FirstOrDefaultAsync(x => x.IdUsuario == idUsuario);

                if (usuario == null)
                {
                    return NotFound();
                }

                if (!string.IsNullOrEmpty(actualizarPasswordDTO.Password))
                {
                    Helpers.Helpers.CrearPasswordHash(actualizarPasswordDTO.Password, out byte[] passwordHash, out byte[] passwordSalt);

                    usuario.Password_hash = passwordHash;
                    usuario.Password_salt = passwordSalt;

                    await context.SaveChangesAsync();

                    return Ok();
                }

                return BadRequest();
            }
            else if (rol == "Alumno")
            {
                var alumno = await context.Alumnos.FirstOrDefaultAsync(x => x.IdAlumno == idUsuario);

                if (alumno == null)
                {
                    return NoContent();
                }

                if (!string.IsNullOrEmpty(actualizarPasswordDTO.Password))
                {
                    Helpers.Helpers.CrearPasswordHash(actualizarPasswordDTO.Password, out byte[] passwordHash, out byte[] passwordSalt);

                    alumno.Password_hash = passwordHash;
                    alumno.Password_salt = passwordSalt;

                    await context.SaveChangesAsync();

                    return Ok();
                }

                return BadRequest();
            }

            return NotFound();
        }

        private RespuestaAutenticacionDTO ConstruirToken(Usuario usuario)
        {
            var claims = new List<Claim>()
            {
                new Claim("idUsuario",usuario.IdUsuario.ToString()),
                new Claim("rol",usuario.IdRol == 1 ? "Administrador" : usuario.IdRol == 2 ? "Docente" : "Indefinido"),
                //new Claim("nombre", usuario.Nombre),
                //new Claim("apellidoPaterno", usuario.ApellidoPaterno),
                //new Claim("apellidoMaterno", usuario.ApellidoMaterno),
                new Claim("correo", usuario.Correo)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["keyjwt"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddHours(1);

            var token = new JwtSecurityToken(issuer: null, audience: null, claims: claims, expires: expires, signingCredentials: credentials);

            return new RespuestaAutenticacionDTO()
            {
                Nombre = usuario.Nombre,
                ApellidoPaterno = usuario.ApellidoPaterno,
                ApellidoMaterno = usuario.ApellidoMaterno,
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiracion = expires
            };
        }

        private RespuestaAutenticacionDTO ConstruirTokenAlumno(Alumno alumno)
        {
            var claims = new List<Claim>()
            {
                new Claim("idAlumno",alumno.IdAlumno.ToString()),
                new Claim("rol", "Alumno"),
                //new Claim("nombre", alumno.Nombre),
                //new Claim("apellidoPaterno", alumno.ApellidoPaterno),
                //new Claim("apellidoMaterno", alumno.ApellidoMaterno),
                new Claim("idGrupo", alumno.IdGrupo.ToString()),
                new Claim("nombreGrupo", alumno.Grupo.Nombre),
                new Claim("correo", alumno.Correo)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["keyjwt"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddHours(1);

            var token = new JwtSecurityToken(issuer: null, audience: null, claims: claims, expires: expires, signingCredentials: credentials);

            return new RespuestaAutenticacionDTO()
            {
                Nombre = alumno.Nombre,
                ApellidoPaterno = alumno.ApellidoPaterno,
                ApellidoMaterno = alumno.ApellidoMaterno,
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiracion = expires
            };
        }

        private bool VerificarPasswordHash(string password, byte[] passwordHashAlmacenado, byte[] passwordSalt)
        {
            //Recibe el password, lo encripta y lo compara con el password almacenado.

            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var passwordHashNuevo = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return new ReadOnlySpan<byte>(passwordHashAlmacenado).SequenceEqual(new ReadOnlySpan<byte>(passwordHashNuevo)); //Compara
            }
        }
    }


}
