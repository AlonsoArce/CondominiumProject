using Infraestructure.Models;
using Infraestructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public class ServiceIncidencia : IServiceIncidencia
    {
        public IEnumerable<Incidencia> GetIncidencias()
        {
            IRepositoryIncidencia repository = new RepositoryIncidencia();
            return repository.GetIncidencias();
        }

        public Incidencia GetIncidenciaByID(int id)
        {
            IRepositoryIncidencia repository = new RepositoryIncidencia();
            return repository.GetIncidenciaByID(id);
        }
        public Incidencia Save(Incidencia incidencia)
        {
            IRepositoryIncidencia repository = new RepositoryIncidencia();
            return repository.Save(incidencia);
        }

        public IEnumerable<Incidencia> GetIncidenciasByUsuario(int idUsuario)
        {
            IRepositoryIncidencia repository = new RepositoryIncidencia();
            return repository.GetIncidenciasByUsuario(idUsuario);
        }

        public IEnumerable<String> GetEstados()
        {
            IRepositoryIncidencia repository = new RepositoryIncidencia();
            return repository.GetEstados();
        }

        public IEnumerable<Incidencia> GetIncidenciaByEstado(string estado)
        {
            IRepositoryIncidencia repository = new RepositoryIncidencia();
            return repository.GetIncidenciaByEstado(estado);
        }
    }
}
