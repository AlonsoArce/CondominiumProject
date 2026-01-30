using Infraestructure.Models;
using Infraestructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public class ServiceRubroCobro : IServiceRubroCobro
    {
        public IEnumerable<RubroCobro> GetRubroCobros()
        {
            IRepositoryRubroCobro repository = new RepositoryRubroCobro();
            return repository.GetRubroCobros();
        }

        public RubroCobro Save(RubroCobro rubroCobro)
        {
            IRepositoryRubroCobro repository = new RepositoryRubroCobro();
            return repository.Save(rubroCobro);
        }

        public RubroCobro GetRubroByID(int id)
        {
            IRepositoryRubroCobro repository = new RepositoryRubroCobro();
            return repository.GetRubroByID(id);
        }
    }
}
