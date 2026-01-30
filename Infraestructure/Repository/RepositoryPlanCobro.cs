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
    public class RepositoryPlanCobro : IRepositoryPlanCobro
    {
        public IEnumerable<PlanCobro> GetPlanesCobros()
        {
            try
            {
                IEnumerable<PlanCobro> lista = null;
                using (MyContext ctx = new MyContext())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;
                    lista = ctx.PlanCobro.
                        Include("RubroCobro").
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

        public PlanCobro GetPlanCobroByID(int id)
        {
            PlanCobro oPlanCobro = null;
            //RubroCobro oRubroCobro = null;
            //ICollection<AsignaRubroCobro> oAsignaRubroCbro = null;
            try
            {
                using (MyContext ctx = new MyContext())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;
                    oPlanCobro = ctx.PlanCobro.
                        Where(p => p.IdPlanCobro == id).
                        Include("RubroCobro").FirstOrDefault();
                    /*oAsignaRubroCbro = ctx.AsignaRubroCobro.
                        Where(p => p.IdPlanCobro == id).
                        Include("PlanCobro").
                        Include("RubroCobro").ToList();
                    oPlanCobro.AsignaRubroCobro = oAsignaRubroCbro;*/

                }
                return oPlanCobro;
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

        //Metodo para guardar o actualizar 
        public PlanCobro Save(PlanCobro plan, string[] selectedRubros)
        {
            int retorno = 0;
            PlanCobro oPlan = null;

            using (MyContext ctx = new MyContext())
            {
                ctx.Configuration.LazyLoadingEnabled = false;
                oPlan = GetPlanCobroByID((int)plan.IdPlanCobro);
                IRepositoryRubroCobro _RepositoryRubroCobro = new RepositoryRubroCobro();

                if (oPlan == null)
                {

                    //Insertar
                    IEnumerable<PlanCobro> listaPlanCobro = null;
                    int count = 0;
                    listaPlanCobro = GetPlanesCobros();
                    foreach (var u in listaPlanCobro)
                    {
                        count = u.IdPlanCobro;
                    }
                    plan.IdPlanCobro = count + 1;
                    //Logica para agregar los rubros al plan de cobro
                    decimal montoTotal = 0;
                    if (selectedRubros != null)
                    {
                        plan.RubroCobro = new List<RubroCobro>();
                        foreach (var rubro in selectedRubros)
                        {
                            var rubroToAdd = _RepositoryRubroCobro.GetRubroByID(int.Parse(rubro));
                            ctx.RubroCobro.Attach(rubroToAdd); //sin esto, EF intentará crear una categoría
                            plan.RubroCobro.Add(rubroToAdd);// asociar a la categoría existente con el libro
                            montoTotal += (decimal)rubroToAdd.MontoRubro;
                        }
                    }
                    plan.MontoTotal = montoTotal;
                    //Insertar Libro
                    ctx.PlanCobro.Add(plan);
                    //SaveChanges
                    //guarda todos los cambios realizados en el contexto de la base de datos.
                    retorno = ctx.SaveChanges();
                    //retorna número de filas afectadas
                }
                else
                {
                    //Registradas: 1,2,3
                    //Actualizar: 1,3,4

                    //Actualizar Plan
                    ctx.PlanCobro.Add(plan);
                    ctx.Entry(plan).State = EntityState.Modified;
                    retorno = ctx.SaveChanges();

                    //Logica para actualizar Rubros
                    decimal montoTotal = 0;
                    var selectedRubrosID = new HashSet<string>(selectedRubros);
                    if (selectedRubros != null)
                    {
                        ctx.Entry(plan).Collection(p => p.RubroCobro).Load();
                        var newRubroForPlan = ctx.RubroCobro
                         .Where(x => selectedRubrosID.Contains(x.IdRubro.ToString())).ToList();
                        plan.RubroCobro = newRubroForPlan;
                        foreach (var rubro in plan.RubroCobro)
                        {
                            montoTotal += (decimal)rubro.MontoRubro;
                        }
                        plan.MontoTotal = montoTotal;
                        ctx.Entry(plan).State = EntityState.Modified;
                        retorno = ctx.SaveChanges();
                    }
                }
                if (retorno >= 0)
                    oPlan = GetPlanCobroByID((int)plan.IdPlanCobro);

                return oPlan;
            }
        }
    }
}
