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
    public class RepositoryAsignaPlanCobro : IRepositoryAsignaPlanCobro
    {
        public IEnumerable<AsignaPlanCobro> GetAsignaPlanesCobros()
        {
            try
            {          
                IEnumerable<AsignaPlanCobro> lista = null;
                //IEnumerable<Residencia> listaResidencias = null;
                //Usuario oUsuario = null;             
                using (MyContext ctx = new MyContext())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;

                    lista = ctx.AsignaPlanCobro.
                        Include("PlanCobro.RubroCobro").
                        Include("Residencia.Usuario")
                        .ToList();                 
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


        public AsignaPlanCobro GetAsignaPlanCobroByID(int idPlan, int idRes, string mes)
        {
            AsignaPlanCobro oAsignaPlanCobro = null;
            //RubroCobro oRubroCobro = null;
            //ICollection<AsignaRubroCobro> oAsignaRubroCbro = null;
            try
            {
                using (MyContext ctx = new MyContext())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;
                    oAsignaPlanCobro = ctx.AsignaPlanCobro.
                        Where(p => p.IdPlanCobro == idPlan).
                        Where(p=> p.IdResidencia ==idRes).
                        Where(p => p.MesAsignado == mes).
                        Include("PlanCobro.RubroCobro").
                        Include("Residencia.Usuario").
                        FirstOrDefault();                   
                }
                return oAsignaPlanCobro;
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

        public IEnumerable<AsignaPlanCobro> GetAsignaPlanCobroByMes(string mes)
        {
            IEnumerable<AsignaPlanCobro> lista = null;         
            try
            {
                using (MyContext ctx = new MyContext())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;
                    lista = ctx.AsignaPlanCobro.Include("PlanCobro.RubroCobro").
                        Include("Residencia.Usuario").
                        ToList().FindAll(l => 
                        l.MesAsignado.ToLower().Contains(mes.ToLower())).
                        Where(l => l.Estado == "Cancelado");
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

        public IEnumerable<AsignaPlanCobro> GetAsignaPlanCobroByResidente(string residente)
        {
            IEnumerable<AsignaPlanCobro> lista = null;         
            try
            {
                using (MyContext ctx = new MyContext())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;
                    lista = ctx.AsignaPlanCobro.Include("PlanCobro.RubroCobro").
                        Include("Residencia.Usuario").                        
                        ToList().FindAll(l => 
                        l.Residencia.Usuario.Nombre.ToLower().Contains(residente.ToLower())).
                        Where(l => l.Estado == "Cancelado");
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

        public IEnumerable<AsignaPlanCobro> GetAsignaPlanCobroByResidencia(int idRes)
        {
            IEnumerable<AsignaPlanCobro> lista = null;
            try
            {
                using (MyContext ctx = new MyContext())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;
                    lista = ctx.AsignaPlanCobro.
                        Where(p => p.IdResidencia == idRes).
                        Include("PlanCobro.RubroCobro").
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

        public IEnumerable<AsignaPlanCobro> GetAsignaPlanCobroByEstado(string estado)
        {
            IEnumerable<AsignaPlanCobro> lista = null;           
            try
            {
                using (MyContext ctx = new MyContext())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;
                    if(estado == "Pendiente")
                    {
                        lista = ctx.AsignaPlanCobro.                           
                            Where(p => p.Estado == estado).
                            Include("PlanCobro.RubroCobro").
                            Include("Residencia.Usuario").
                            ToList();
                    }
                    else
                    {
                        lista = ctx.AsignaPlanCobro.                          
                            Where(p => p.Estado == estado).
                            Include("PlanCobro.RubroCobro").
                            Include("Residencia.Usuario").
                            ToList();
                    }
                    
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

        public IEnumerable<String> GetMeses()
        {
            IEnumerable<String> meses = new[] {"Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio",
            "Agosto", "Setiembre", "Octubre", "Noviembre", "Diciembre"};

            return meses;
        }

        public IEnumerable<String> GetEstados()
        {
            IEnumerable<String> estados = new[] {"Pendiente", "Cancelado"};

            return estados;
        }

        public AsignaPlanCobro Save(AsignaPlanCobro asignaPlan)
        {
            int retorno = 0;
            AsignaPlanCobro oAsignaPlan = null;

            using (MyContext ctx = new MyContext())
            {
                ctx.Configuration.LazyLoadingEnabled = false;
                oAsignaPlan = GetAsignaPlanCobroByID((int)asignaPlan.IdPlanCobro, (int)asignaPlan.IdResidencia, (string)asignaPlan.MesAsignado);
                

                if (oAsignaPlan == null)
                {                   
                    //Insertar Asignacion de Plan Cobro
                    ctx.AsignaPlanCobro.Add(asignaPlan);
                    //SaveChanges
                    //guarda todos los cambios realizados en el contexto de la base de datos.
                    retorno = ctx.SaveChanges();
                    //retorna número de filas afectadas
                }
                else
                {             
                    //Actualizar Asignacion Plan Cobro
                    ctx.AsignaPlanCobro.Add(asignaPlan);
                    ctx.Entry(asignaPlan).State = EntityState.Modified;
                    retorno = ctx.SaveChanges();                    
                }
            }

            if (retorno >= 0)
                oAsignaPlan = GetAsignaPlanCobroByID((int)asignaPlan.IdPlanCobro, (int)asignaPlan.IdResidencia, (string)asignaPlan.MesAsignado);

            return oAsignaPlan;
        }

        public bool ValidadMes(AsignaPlanCobro asignaPlan)
        {
            bool valida = false;
            IEnumerable<AsignaPlanCobro> listaAsignados = GetAsignaPlanCobroByMes(asignaPlan.MesAsignado);
            if (listaAsignados == null)
            {
                valida = false;
            }
            else
            {
                foreach(var a in listaAsignados)
                {
                    AsignaPlanCobro asPlan = a;
                    if(asignaPlan.IdPlanCobro == a.IdPlanCobro && asignaPlan.IdResidencia == a.IdResidencia)
                    {
                        return true;
                    }                    
                }
            }
            return valida;
        }
    }
}
