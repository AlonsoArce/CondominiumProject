using Infraestructure.Models;
using Infraestructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public class ServiceResidencia : IServiceResidencia 
    {
        public IEnumerable<Residencia> GetResidencias()
        {
            IRepositoryResidencia repository = new RepositoryResidencia();
            return repository.GetResidencias();
        }

        public Residencia GetResidenciaByID(int id)
        {
            IRepositoryResidencia repository = new RepositoryResidencia();
            return repository.GetResidenciaByID(id);
        }
        public Residencia GetResidenciaByUsuario(int idUsuario)
        {
            IRepositoryResidencia repository = new RepositoryResidencia();
            return repository.GetResidenciaByUsuario(idUsuario);
        }

        public Residencia Save(Residencia residencia)
        {
            IRepositoryResidencia repository = new RepositoryResidencia();
            return repository.Save(residencia);
        }

        public IEnumerable<String> GetEstados()
        {
            IRepositoryResidencia repository = new RepositoryResidencia();
            return repository.GetEstados();
        }
    }
}
