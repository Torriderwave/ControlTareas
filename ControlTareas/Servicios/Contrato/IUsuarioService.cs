using Microsoft.EntityFrameworkCore;
using ControlTareas.Models;

namespace ControlTareas.Servicios.Contrato
{
    public interface IUsuarioService
    {
        Task<Usuario> GetUsuarios(string correo, string clave);
        Task<Usuario> SaveUsuario(Usuario modelo);
    }
}
