using Infraestructure.Models;
using System.Collections.Generic;

namespace Infraestructure.Repository
{
    public interface IRepositoryInformacion
    {
        IEnumerable<Informacion> GetInformacion();
        Informacion GetInformacionByID(int id);
        Informacion Save(Informacion informacion);
        IEnumerable<Informacion> GetInfoByTipo(int idTipo);
    }
}