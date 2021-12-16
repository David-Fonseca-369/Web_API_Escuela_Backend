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


        //POST: api/login/usario
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
        public async Task<ActionResult<RespuestaAutenticacionDTO>>Alumno(LoginAlumnoDTO model)
        {
            var curp = model.Curp.ToUpper();

            var alumno = await context.Alumnos.FirstOrDefaultAsync(x => x.Curp == curp && x.Estado == true);
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

        private RespuestaAutenticacionDTO ConstruirToken(Usuario usuario)
        {
            var claims = new List<Claim>()
            {
                new Claim("idUsuario",usuario.IdUsuario.ToString()),
                new Claim("rol",usuario.IdRol == 1 ? "Administrador" : usuario.IdRol == 2 ? "Docente" : "Indefinido"),
                new Claim("nombre", usuario.Nombre),
                new Claim("apellidoPaterno", usuario.ApellidoPaterno),
                new Claim("apellidoMaterno", usuario.ApellidoMaterno),
                new Claim("correo", usuario.Correo)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["keyjwt"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddHours(1);

            var token = new JwtSecurityToken(issuer: null, audience: null, claims: claims, expires: expires, signingCredentials: credentials);

            return new RespuestaAutenticacionDTO()
            {
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
                new Claim("nombre", alumno.Nombre),
                new Claim("apellidoPaterno", alumno.ApellidoPaterno),
                new Claim("apellidoMaterno", alumno.ApellidoMaterno),
                new Claim("idGrupo", alumno.IdGrupo.ToString()),
                new Claim("correo", alumno.Correo)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["keyjwt"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddHours(1);

            var token = new JwtSecurityToken(issuer: null, audience: null, claims: claims, expires: expires, signingCredentials: credentials);

            return new RespuestaAutenticacionDTO()
            {
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
