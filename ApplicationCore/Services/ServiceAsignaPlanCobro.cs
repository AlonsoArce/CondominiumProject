using Infraestructure.Models;
using Infraestructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public class ServiceAsignaPlanCobro : IServiceAsignaPlanCobro
    {
        public IEnumerable<AsignaPlanCobro> GetAsignaPlanesCobros()
        {
            IRepositoryAsignaPlanCobro repository = new RepositoryAsignaPlanCobro();
            return repository.GetAsignaPlanesCobros();
        }
        public AsignaPlanCobro GetAsignaPlanCobroByID(int idPlan, int idRes, string mes)
        {
            IRepositoryAsignaPlanCobro repository = new RepositoryAsignaPlanCobro();
            return repository.GetAsignaPlanCobroByID(idPlan, idRes, mes);
        }
        public IEnumerable<AsignaPlanCobro> GetAsignaPlanCobroByMes(string mes)
        {
            IRepositoryAsignaPlanCobro repository = new RepositoryAsignaPlanCobro();
            return repository.GetAsignaPlanCobroByMes(mes);
        }

        public IEnumerable<String> GetMeses()
        {
            IRepositoryAsignaPlanCobro repository = new RepositoryAsignaPlanCobro();
            return repository.GetMeses();
        }

        public IEnumerable<String> GetEstados()
        {
            IRepositoryAsignaPlanCobro repository = new RepositoryAsignaPlanCobro();
            return repository.GetEstados();
        }

        public AsignaPlanCobro Save(AsignaPlanCobro asignaPlan)
        {
            IRepositoryAsignaPlanCobro repository = new RepositoryAsignaPlanCobro();
            return repository.Save(asignaPlan);
        }

        public bool ValidadMes(AsignaPlanCobro asignaPlan)
        {
            IRepositoryAsignaPlanCobro repository = new RepositoryAsignaPlanCobro();
            return repository.ValidadMes(asignaPlan);
        }
        public IEnumerable<AsignaPlanCobro> GetAsignaPlanCobroByEstado(string estado)
        {
            IRepositoryAsignaPlanCobro repository = new RepositoryAsignaPlanCobro();
            return repository.GetAsignaPlanCobroByEstado(estado);
        }
        public IEnumerable<AsignaPlanCobro> GetAsignaPlanCobroByResidencia(int idRes)
        {
            IRepositoryAsignaPlanCobro repository = new RepositoryAsignaPlanCobro();
            return repository.GetAsignaPlanCobroByResidencia(idRes);
        }
        public IEnumerable<AsignaPlanCobro> GetAsignaPlanCobroByResidente(string residente)
        {
            IRepositoryAsignaPlanCobro repository = new RepositoryAsignaPlanCobro();
            return repository.GetAsignaPlanCobroByResidente(residente);
        }
    }
}
