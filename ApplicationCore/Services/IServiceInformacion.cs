using Infraestructure.Models;
using System.Collections.Generic;

namespace ApplicationCore.Services
{
    public interface IServiceInformacion
    {
        IEnumerable<Informacion> GetInformacion();
        Informacion GetInformacionByID(int id);
        Informacion Save(Informacion informacion);
        IEnumerable<Informacion> GetInfoByTipo(int idTipo);
    }
}