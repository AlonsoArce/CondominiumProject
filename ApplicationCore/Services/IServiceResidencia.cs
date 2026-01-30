using Infraestructure.Models;
using System;
using System.Collections.Generic;

namespace ApplicationCore.Services
{
    public interface IServiceResidencia
    {
        IEnumerable<Residencia> GetResidencias();
        Residencia GetResidenciaByID(int id);
        Residencia GetResidenciaByUsuario(int idUsuario);
        Residencia Save(Residencia residencia);
        IEnumerable<String> GetEstados();
    }
}