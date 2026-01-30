using Infraestructure.Models;
using Infraestructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public class ServicePlanCobro : IServicePlanCobro
    {
        public IEnumerable<PlanCobro> GetPlanesCobros()
        {
            IRepositoryPlanCobro repository = new RepositoryPlanCobro();
            return repository.GetPlanesCobros();
        }
        public PlanCobro GetPlanCobroByID(int id)
        {
            IRepositoryPlanCobro repository = new RepositoryPlanCobro();
            return repository.GetPlanCobroByID(id);
        }

        public PlanCobro Save(PlanCobro planCobro, string[] selectedRubros)
        {
            IRepositoryPlanCobro repository = new RepositoryPlanCobro();
            return repository.Save(planCobro, selectedRubros);
        }
    }
}
