using Infraestructure.Models;
using System;
using System.Collections.Generic;

namespace Infraestructure.Repository
{
    public interface IRepositoryReservaAreaComun
    {
        IEnumerable<ReservaAreaComun> GetReservas();
        IEnumerable<ReservaAreaComun> GetReservaByResidencia(int idResidencia);
        ReservaAreaComun GetReservaById(int idReserva);
        bool VerificaDisponibilidad(ReservaAreaComun reservaArea);
        IEnumerable<String> GetAreas();
        IEnumerable<String> GetHorarios();
        ReservaAreaComun Save(ReservaAreaComun reserva);
        IEnumerable<String> GetEstados();
        IEnumerable<ReservaAreaComun> GetReservaByEstado(string estado);

    }
}