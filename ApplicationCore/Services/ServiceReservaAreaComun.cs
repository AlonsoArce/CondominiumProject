using Infraestructure.Models;
using Infraestructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public class ServiceReservaAreaComun : IServiceReservaAreaComun
    {
        public IEnumerable<ReservaAreaComun> GetReservas()
        {
            IRepositoryReservaAreaComun repository = new RepositoryReservaAreaComun();
            return repository.GetReservas();
        }

        public IEnumerable<ReservaAreaComun> GetReservaByResidencia(int idResidencia)
        {
            IRepositoryReservaAreaComun repository = new RepositoryReservaAreaComun();
            return repository.GetReservaByResidencia(idResidencia);
        }

        public ReservaAreaComun GetReservaById(int idReserva)
        {
            IRepositoryReservaAreaComun repository = new RepositoryReservaAreaComun();
            return repository.GetReservaById(idReserva);
        }

        public bool VerificaDisponibilidad(ReservaAreaComun reservaArea)
        {
            IRepositoryReservaAreaComun repository = new RepositoryReservaAreaComun();
            return repository.VerificaDisponibilidad(reservaArea);
        }

        public IEnumerable<String> GetAreas()
        {
            IRepositoryReservaAreaComun repository = new RepositoryReservaAreaComun();
            return repository.GetAreas();
        }
        public IEnumerable<String> GetHorarios()
        {
            IRepositoryReservaAreaComun repository = new RepositoryReservaAreaComun();
            return repository.GetHorarios();
        }
        public IEnumerable<String> GetEstados()
        {
            IRepositoryReservaAreaComun repository = new RepositoryReservaAreaComun();
            return repository.GetEstados();
        }
        public ReservaAreaComun Save(ReservaAreaComun reserva)
        {
            IRepositoryReservaAreaComun repository = new RepositoryReservaAreaComun();
            return repository.Save(reserva);
        }

        public IEnumerable<ReservaAreaComun> GetReservaByEstado(string estado)
        {
            IRepositoryReservaAreaComun repository = new RepositoryReservaAreaComun();
            return repository.GetReservaByEstado(estado);
        }
    }
}
