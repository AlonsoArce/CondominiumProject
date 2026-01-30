using Infraestructure.Models;
using System;
using System.Collections.Generic;

namespace Infraestructure.Repository
{
    public interface IRepositoryResidencia
    {
        IEnumerable<Residencia> GetResidencias();
        Residencia GetResidenciaByID(int id);
        Residencia GetResidenciaByUsuario(int idUsuario);
        IEnumerable<String> GetEstados();
        Residencia Save(Residencia residencia);
    }
}