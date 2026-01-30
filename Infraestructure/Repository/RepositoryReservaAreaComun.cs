using Infraestructure.Models;
using Infraestructure.Utils;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Repository
{
    public class RepositoryReservaAreaComun : IRepositoryReservaAreaComun
    {
        public IEnumerable<ReservaAreaComun> GetReservas()
        {
            try
            {
                IEnumerable<ReservaAreaComun> lista = null;
                using (MyContext ctx = new MyContext())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;
                    lista = ctx.ReservaAreaComun.
                        Include("Residencia.Usuario").
                        ToList();
                }
                return lista;
            }
            catch (DbUpdateException dbEx)
            {
                string mensaje = "";
                Log.Error(dbEx, System.Reflection.MethodBase.GetCurrentMethod(), ref mensaje);
                throw new Exception(mensaje);
            }
            catch (Exception ex)
            {
                string mensaje = "";
                Log.Error(ex, System.Reflection.MethodBase.GetCurrentMethod(), ref mensaje);
                throw;
            }
        }

        public IEnumerable<ReservaAreaComun> GetReservaByResidencia(int idResidencia)
        {
            try
            {
                IEnumerable<ReservaAreaComun> reserva = null;
                using (MyContext ctx = new MyContext())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;
                    reserva = ctx.ReservaAreaComun.
                        Where(r => r.IdResidencia == idResidencia).
                        Include("Residencia").
                        ToList();
                }
                return reserva;
            }
            catch (DbUpdateException dbEx)
            {
                string mensaje = "";
                Log.Error(dbEx, System.Reflection.MethodBase.GetCurrentMethod(), ref mensaje);
                throw new Exception(mensaje);
            }
            catch (Exception ex)
            {
                string mensaje = "";
                Log.Error(ex, System.Reflection.MethodBase.GetCurrentMethod(), ref mensaje);
                throw;
            }
        }

        public IEnumerable<ReservaAreaComun> GetReservaByEstado(string estado)
        {
            try
            {
                IEnumerable<ReservaAreaComun> reserva = null;
                using (MyContext ctx = new MyContext())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;
                    reserva = ctx.ReservaAreaComun.
                        Where(r => r.Estado == estado).
                        Include("Residencia").
                        ToList();
                }
                return reserva;
            }
            catch (DbUpdateException dbEx)
            {
                string mensaje = "";
                Log.Error(dbEx, System.Reflection.MethodBase.GetCurrentMethod(), ref mensaje);
                throw new Exception(mensaje);
            }
            catch (Exception ex)
            {
                string mensaje = "";
                Log.Error(ex, System.Reflection.MethodBase.GetCurrentMethod(), ref mensaje);
                throw;
            }
        }

        public ReservaAreaComun GetReservaById(int idReserva)
        {
            try
            {
                ReservaAreaComun reserva = null;
                using (MyContext ctx = new MyContext())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;
                    reserva = ctx.ReservaAreaComun.
                        Where(r => r.IdReserva == idReserva).
                        Include("Residencia.Usuario").
                        FirstOrDefault();
                }
                return reserva;
            }
            catch (DbUpdateException dbEx)
            {
                string mensaje = "";
                Log.Error(dbEx, System.Reflection.MethodBase.GetCurrentMethod(), ref mensaje);
                throw new Exception(mensaje);
            }
            catch (Exception ex)
            {
                string mensaje = "";
                Log.Error(ex, System.Reflection.MethodBase.GetCurrentMethod(), ref mensaje);
                throw;
            }
        }

        public bool VerificaDisponibilidad(ReservaAreaComun reservaArea)
        {
            try
            {
                bool verifica = false;
                ReservaAreaComun reserva = null;
                using (MyContext ctx = new MyContext())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;
                    reserva = ctx.ReservaAreaComun.
                        Where(r => r.AreaComun == reservaArea.AreaComun).
                        Where(r => r.FechaReserva == reservaArea.FechaReserva).
                        Where(r => r.Horario == reservaArea.Horario).
                        Include("Residencia.Usuario").
                        FirstOrDefault();
                }
                if (reserva == null)
                    verifica = true;
                return verifica;
            }
            catch (DbUpdateException dbEx)
            {
                string mensaje = "";
                Log.Error(dbEx, System.Reflection.MethodBase.GetCurrentMethod(), ref mensaje);
                throw new Exception(mensaje);
            }
            catch (Exception ex)
            {
                string mensaje = "";
                Log.Error(ex, System.Reflection.MethodBase.GetCurrentMethod(), ref mensaje);
                throw;
            }
        }

        public IEnumerable<String> GetAreas()
        {
            IEnumerable<String> areas = new[] {"Area de Piscina", "Terraza para parrillada", "Salon de actividades"};

            return areas;
        }

        public IEnumerable<String> GetHorarios()
        {
            IEnumerable<String> areas = new[] { "De 10:00 a.m. a 2:00 p.m", "De 3:00 p.m. a 7:00 pm", "De 7:00 p.m. a 11:00 p.m." };

            return areas;
        }

        public IEnumerable<String> GetEstados()
        {
            IEnumerable<String> estados = new[] { "En proceso", "Aprobada", "Rechazada" };

            return estados;
        }

        public ReservaAreaComun Save(ReservaAreaComun reserva)
        {
            int retorno = 0;
            ReservaAreaComun oReserva = null;

            using (MyContext ctx = new MyContext())
            {
                ctx.Configuration.LazyLoadingEnabled = false;
                oReserva = GetReservaById((int)reserva.IdReserva);


                if (oReserva == null)
                {
                    
                        //Insertar ReservaAreaComun
                        ctx.ReservaAreaComun.Add(reserva);
                        //SaveChanges
                        //guarda todos los cambios realizados en el contexto de la base de datos.
                        retorno = ctx.SaveChanges();
                        //retorna número de filas afectadas
                                    
                }
                else
                {
                    //Actualizar ReservaAreaComun
                    ctx.ReservaAreaComun.Add(reserva);
                    ctx.Entry(reserva).State = EntityState.Modified;
                    retorno = ctx.SaveChanges();
                }
            }

            if (retorno >= 0)
                oReserva = GetReservaById((int)reserva.IdReserva);

            return oReserva;
        }
    }
}
