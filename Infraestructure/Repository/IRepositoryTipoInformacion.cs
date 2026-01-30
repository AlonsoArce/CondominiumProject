using Infraestructure.Models;
using System.Collections.Generic;

namespace Infraestructure.Repository
{
    public interface IRepositoryTipoInformacion
    {
        IEnumerable<TipoInformacion> GetTipoInformacion();
    }
}