using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web_API_Escuela.DTOs;
using Web_API_Escuela.DTOs.Usuario;
using Web_API_Escuela.Entities;
using Web_API_Escuela.Helpers;

namespace Web_API_Escuela.Controllers
{
    [Route("api/usuarios")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public UsuariosController(ApplicationDbContext context)
        {
            this.context = context;
        }


        //GET: api/usuarios/Administradores
        [HttpGet("Administradores")]
        public async Task<ActionResult<List<UsuarioDTO>>> Administradores()
        {
            //1 administrador
            //2 docente
            var administradores = await context.Usuarios.Where(x => x.IdRol == 1).ToListAsync();

            return administradores.Select(x => new UsuarioDTO
            {
                IdUsuario = x.IdUsuario,
                Nombre = x.Nombre,
                ApellidoPaterno = x.ApellidoPaterno,
                ApellidoMaterno = x.ApellidoMaterno,
                Correo = x.Correo,
                Estado = x.Estado
            }).ToList();
        }


        //GET: api/usuarios/docentes
        [HttpGet("Docentes")]
        public async Task<ActionResult<List<UsuarioDTO>>> Docentes()
        {
            //1 administrador
            //2 docente
            var docentes = await context.Usuarios.Where(x => x.IdRol == 2 && x.Estado == true).ToListAsync();

            return docentes.Select(x => new UsuarioDTO
            {
                IdUsuario = x.IdUsuario,
                Nombre = x.Nombre,
                ApellidoPaterno = x.ApellidoPaterno,
                ApellidoMaterno = x.ApellidoMaterno,
                Correo = x.Correo,
                Estado = x.Estado
            }).OrderBy(x => x.Nombre).ToList();
        }


        //GET: api/usuarios/AdministradoresPaginacion
        [HttpGet("AdministradoresPaginacion")]
        public async Task<ActionResult<List<UsuarioDTO>>> AdministradoresPaginacion([FromQuery] PaginacionDTO paginacionDTO)
        {
            var usuarios = await context.Usuarios.Where(x => x.IdRol == 1).ToListAsync();

            int cantidad = usuarios.Count();

            HttpContext.InsertarParametrosPaginacionEnCabeceraPersonalizado(cantidad);

            var queryable = usuarios.AsQueryable();

            var usuariosPaginado = queryable.Paginar(paginacionDTO).ToList();

            return usuariosPaginado.Select(x => new UsuarioDTO
            {
                IdUsuario = x.IdUsuario,
                Nombre = x.Nombre,
                ApellidoPaterno = x.ApellidoPaterno,
                ApellidoMaterno = x.ApellidoMaterno,
                Correo = x.Correo,
                Estado = x.Estado
            }).ToList();
        }

        //GET: api/usuarios/DocentesPaginacion
        [HttpGet("DocentesPaginacion")]
        public async Task<ActionResult<List<UsuarioDTO>>> DocentesPaginacion([FromQuery] PaginacionDTO paginacionDTO)
        {
            var usuarios = await context.Usuarios.Where(x => x.IdRol == 2).ToListAsync();

            int cantidad = usuarios.Count();

            HttpContext.InsertarParametrosPaginacionEnCabeceraPersonalizado(cantidad);

            var queryable = usuarios.AsQueryable();

            var usuariosPaginado = queryable.Paginar(paginacionDTO).ToList();

            return usuariosPaginado.Select(x => new UsuarioDTO
            {
                IdUsuario = x.IdUsuario,
                Nombre = x.Nombre,
                ApellidoPaterno = x.ApellidoPaterno,
                ApellidoMaterno = x.ApellidoMaterno,
                Correo = x.Correo,
                Estado = x.Estado
            }).ToList();
        }


        //GET: api/usuarios/{id}
        [HttpGet("{id:int}")]
        public async Task<ActionResult<UsuarioDTO>> Get(int id)
        {
            var usuario = await context.Usuarios.FirstOrDefaultAsync(x => x.IdUsuario == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return new UsuarioDTO
            {
                IdUsuario = usuario.IdUsuario,
                Nombre = usuario.Nombre,
                ApellidoPaterno = usuario.ApellidoPaterno,
                ApellidoMaterno = usuario.ApellidoMaterno,
                Correo = usuario.Correo,
                Estado = usuario.Estado
            };

        }


        //POST: api/usuarios/crearDocente
        [HttpPost("CrearDocente")]
        public async Task<ActionResult> CrearDocente([FromBody] UsuarioCreacionDTO usuarioCreacionDTO)
        {
            //Verificar si el email existe
            var correo = usuarioCreacionDTO.Correo.ToLower();
            if (await context.Usuarios.AnyAsync(x => x.Correo == correo))
            {
                return BadRequest("El correo ya existe.");
            }

            Helpers.Helpers.CrearPasswordHash(usuarioCreacionDTO.Password, out byte[] passwordHash, out byte[] passwordSalt);

            Usuario usuario = new()
            {
                IdRol = 2, //Docente
                Nombre = usuarioCreacionDTO.Nombre,
                ApellidoPaterno = usuarioCreacionDTO.ApellidoPaterno,
                ApellidoMaterno = usuarioCreacionDTO.ApellidoMaterno,
                Correo = usuarioCreacionDTO.Correo.ToLower(),
                Password_hash = passwordHash,
                Password_salt = passwordSalt,
                Estado = true
            };

            context.Add(usuario);

            await context.SaveChangesAsync();

            return NoContent();

        }
        
        //POST: api/usuarios/crearAdministador
        [HttpPost("CrearAdministrador")]
        public async Task<ActionResult> CrearAdministrador([FromBody] UsuarioCreacionDTO usuarioCreacionDTO)
        {
            //Verificar si el email existe
            var correo = usuarioCreacionDTO.Correo.ToLower();
            if (await context.Usuarios.AnyAsync(x => x.Correo == correo))
            {
                return BadRequest("El correo ya existe.");
            }

            Helpers.Helpers.CrearPasswordHash(usuarioCreacionDTO.Password, out byte[] passwordHash, out byte[] passwordSalt);

            Usuario usuario = new()
            {
                IdRol = 1, //Administrador
                Nombre = usuarioCreacionDTO.Nombre,
                ApellidoPaterno = usuarioCreacionDTO.ApellidoPaterno,
                ApellidoMaterno = usuarioCreacionDTO.ApellidoMaterno,
                Correo = usuarioCreacionDTO.Correo.ToLower(),
                Password_hash = passwordHash,
                Password_salt = passwordSalt,
                Estado = true
            };

            context.Add(usuario);

            await context.SaveChangesAsync();

            return NoContent();

        }

        //PUT api/usuarios/editar/{id}
        [HttpPut("editar/{id:int}")]
        public async Task<ActionResult> Editar(int id, [FromBody] UsuarioActualizacionDTO usuarioActualizacionDTO)
        {
            var usuario = await context.Usuarios.FirstOrDefaultAsync(x => x.IdUsuario == id);

            if (usuario == null)
            {
                return NotFound();
            }

            var correo = usuarioActualizacionDTO.Correo.ToLower();

            if (correo != usuario.Correo)//Verificar que el correo nuevo no se repita con otro registro
            {
                if (await context.Alumnos.AnyAsync(x => x.Correo == correo))
                {
                    return BadRequest("El correo ya existe.");
                }
            }

            usuario.Nombre = usuarioActualizacionDTO.Nombre;
            usuario.ApellidoPaterno = usuarioActualizacionDTO.ApellidoPaterno;
            usuario.ApellidoMaterno = usuarioActualizacionDTO.ApellidoMaterno;
            usuario.Correo = usuarioActualizacionDTO.Correo.ToLower();

            //Verificar el cambio de contraseña
            if (!string.IsNullOrEmpty(usuarioActualizacionDTO.Password))
            {
                Helpers.Helpers.CrearPasswordHash(usuarioActualizacionDTO.Password, out byte[] passwordHash, out byte[] passwordSalt);
           
                usuario.Password_hash = passwordHash;
                usuario.Password_salt = passwordSalt;
            }

            await context.SaveChangesAsync();

            return NoContent();
        }


        //PUT: api/usuarios/Activar/{id}
        [HttpPut("activar/{id:int}")]
        public async Task<ActionResult> Activar(int id)
        {
            var usuario = await context.Usuarios.FirstOrDefaultAsync(x => x.IdUsuario == id);
            
            if (usuario == null)
            {
                return NotFound();
            }

            usuario.Estado = true;

            await context.SaveChangesAsync();

            return NoContent();
        }
        
        //PUT: api/usuarios/Desactivar/{id}
        [HttpPut("desactivar/{id:int}")]
        public async Task<ActionResult> Desactivar(int id)
        {
            var usuario = await context.Usuarios.FirstOrDefaultAsync(x => x.IdUsuario == id);
            
            if (usuario == null)
            {
                return NotFound();
            }

            usuario.Estado = false;

            await context.SaveChangesAsync();

            return NoContent();
        }

    }
}
