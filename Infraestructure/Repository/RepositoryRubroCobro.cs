using Infraestructure.Models;
using Infraestructure.Utils;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Repository
{
    public class RepositoryRubroCobro : IRepositoryRubroCobro
    {
        public IEnumerable<RubroCobro> GetRubroCobros()
        {
            try
            {
                IEnumerable<RubroCobro> lista = null;
                using (MyContext ctx = new MyContext())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;
                    //Select * from Autor 
                    lista = ctx.RubroCobro.ToList();
                    //lista = ctx.Autor.ToList();
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

        //Metodo para retornar el rubro por ID
        public RubroCobro GetRubroByID(int id)
        {
            RubroCobro oRubro = null;
            try
            {
                using (MyContext ctx = new MyContext())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;
                    //Obtener rubro
                    oRubro = ctx.RubroCobro.
                        Where(r => r.IdRubro == id).                                             
                        FirstOrDefault();

                }
                return oRubro;
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



        //aqui va el metodo para actualizar o crear
        public RubroCobro Save(RubroCobro rubroCobro)
        {
            //se crea una variable retorno para determinar si se guardaron cambios
            int retorno = 0;
            //Este objeto RubroCobro se va a utilizar para retornar el rubro por ID
            RubroCobro oRubro = null;

            using (MyContext ctx = new MyContext())
            {
                ctx.Configuration.LazyLoadingEnabled = false;
                oRubro = GetRubroByID((int)rubroCobro.IdRubro);
                //si el rubro no existe devuelve null y se debe crear o insertar uno nuevo
                if(oRubro == null)
                {
                    //Insertar RubroCobro                  
                    ctx.RubroCobro.Add(rubroCobro);
                    //SaveChanges
                    //guarda todos los cambios realizados en el contexto de la base de datos.
                    retorno = ctx.SaveChanges();
                }
                else
                {
                    //si el rubro si existe entonces se debe actualizar
                    ctx.RubroCobro.Add(rubroCobro);
                    ctx.Entry(rubroCobro).State = EntityState.Modified;
                    retorno = ctx.SaveChanges();
                }
                //se debe retornar el rubro a la vista para mostrar los datos en la pantalla
                if (retorno >= 0)
                    oRubro = GetRubroByID((int)rubroCobro.IdRubro);

                return oRubro;
            }
        }
    }
}
