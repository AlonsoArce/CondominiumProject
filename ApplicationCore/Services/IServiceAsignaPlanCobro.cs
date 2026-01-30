using Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public interface IServiceAsignaPlanCobro
    {
        IEnumerable<AsignaPlanCobro> GetAsignaPlanesCobros();
        AsignaPlanCobro GetAsignaPlanCobroByID(int idPlan, int idRes, string mes);
        IEnumerable<AsignaPlanCobro> GetAsignaPlanCobroByMes(string mes);
        IEnumerable<AsignaPlanCobro> GetAsignaPlanCobroByResidente(string residente);
        IEnumerable<String> GetMeses();
        IEnumerable<String> GetEstados();
        AsignaPlanCobro Save(AsignaPlanCobro asignaPlan);
        bool ValidadMes(AsignaPlanCobro asignaPlan);
        IEnumerable<AsignaPlanCobro> GetAsignaPlanCobroByEstado(string estado);
        IEnumerable<AsignaPlanCobro> GetAsignaPlanCobroByResidencia(int idRes);
    }
}
