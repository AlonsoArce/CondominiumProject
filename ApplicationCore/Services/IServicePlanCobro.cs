using Infraestructure.Models;
using System.Collections.Generic;

namespace ApplicationCore.Services
{
    public interface IServicePlanCobro
    {
        IEnumerable<PlanCobro> GetPlanesCobros();
        PlanCobro GetPlanCobroByID(int id);

        PlanCobro Save(PlanCobro planCobro, string[] selectedRubros);
    }
}