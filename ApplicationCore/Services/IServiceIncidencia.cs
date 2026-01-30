using Infraestructure.Models;
using System;
using System.Collections.Generic;

namespace ApplicationCore.Services
{
    public interface IServiceIncidencia
    {
        IEnumerable<Incidencia> GetIncidencias();
        Incidencia GetIncidenciaByID(int id);
        Incidencia Save(Incidencia incidencia);
        IEnumerable<Incidencia> GetIncidenciasByUsuario(int idUsuario);
        IEnumerable<String> GetEstados();
        IEnumerable<Incidencia> GetIncidenciaByEstado(string estado);
    }
}