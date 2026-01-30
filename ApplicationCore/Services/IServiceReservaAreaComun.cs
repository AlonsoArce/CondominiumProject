using Infraestructure.Models;
using System;
using System.Collections.Generic;

namespace ApplicationCore.Services
{
    public interface IServiceReservaAreaComun
    {
        IEnumerable<ReservaAreaComun> GetReservas();
        IEnumerable<ReservaAreaComun> GetReservaByResidencia(int idResidencia);
        ReservaAreaComun GetReservaById(int idReserva);
        bool VerificaDisponibilidad(ReservaAreaComun reservaArea);
        IEnumerable<String> GetAreas();
        IEnumerable<String> GetHorarios();
        IEnumerable<String> GetEstados();
        ReservaAreaComun Save(ReservaAreaComun reserva);
        IEnumerable<ReservaAreaComun> GetReservaByEstado(string estado);


    }
}