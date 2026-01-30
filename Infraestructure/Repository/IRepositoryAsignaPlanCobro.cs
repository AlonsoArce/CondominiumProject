using Infraestructure.Models;
using System;
using System.Collections.Generic;

namespace Infraestructure.Repository
{
    public interface IRepositoryAsignaPlanCobro
    {
        IEnumerable<AsignaPlanCobro> GetAsignaPlanesCobros();
        IEnumerable<AsignaPlanCobro> GetAsignaPlanCobroByMes(string mes);
        IEnumerable<AsignaPlanCobro> GetAsignaPlanCobroByResidente(string residente);
        AsignaPlanCobro GetAsignaPlanCobroByID(int idPlan, int idRes, string mes);
        IEnumerable<String> GetMeses();
        IEnumerable<String> GetEstados();
        AsignaPlanCobro Save(AsignaPlanCobro asignaPlan);
        bool ValidadMes(AsignaPlanCobro asignaPlan);
        IEnumerable<AsignaPlanCobro> GetAsignaPlanCobroByEstado(string estado);
        IEnumerable<AsignaPlanCobro> GetAsignaPlanCobroByResidencia(int idRes);
    }
}