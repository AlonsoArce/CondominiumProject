using Infraestructure.Models;
using System.Collections.Generic;

namespace Infraestructure.Repository
{
    public interface IRepositoryUsuario
    {
        Usuario GetUsuarioByID(int id);
        Usuario Save(Usuario usuario);
        Usuario GetUsuario(string email, string password);
        IEnumerable<Usuario> GetUsuarios();
    }
}