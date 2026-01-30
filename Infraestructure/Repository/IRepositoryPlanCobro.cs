using Infraestructure.Models;
using System.Collections.Generic;

namespace Infraestructure.Repository
{
    public interface IRepositoryPlanCobro
    {
        IEnumerable<PlanCobro> GetPlanesCobros();
        PlanCobro GetPlanCobroByID(int id);

        PlanCobro Save(PlanCobro planCobro, string[] selectedRubros);
    }
}