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
    public class RepositoryIncidencia : IRepositoryIncidencia
    {
        public IEnumerable<Incidencia> GetIncidencias()
        {
            try
            {
                IEnumerable<Incidencia> lista = null;
                using (MyContext ctx = new MyContext())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;                   
                    lista = ctx.Incidencia.
                        Include("Usuario").
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

        public Incidencia GetIncidenciaByID(int id)
        {
            Incidencia oIncidencia = null;
            try
            {
                using (MyContext ctx = new MyContext())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;
                    //Obtener rubro
                    oIncidencia = ctx.Incidencia.
                        Include("Usuario").
                        Where(i => i.IdIncidencia == id).
                        FirstOrDefault();

                }
                return oIncidencia;
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

        public IEnumerable<Incidencia> GetIncidenciaByEstado(string estado)
        {
            IEnumerable<Incidencia> lista = null;
            try
            {
                using (MyContext ctx = new MyContext())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;
                    lista = ctx.Incidencia.Include("Usuario").
                        ToList().FindAll(l =>
                        l.EstadoIncidencia.ToLower().Contains(estado.ToLower()));
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

        public IEnumerable<Incidencia> GetIncidenciasByUsuario(int idUsuario)
        {
            
            try
            {
                IEnumerable<Incidencia> oLista = null;
                using (MyContext ctx = new MyContext())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;
                    //Obtener rubro
                    oLista = ctx.Incidencia.                        
                        Include("Usuario").
                        Where(i => i.IdUsuario == idUsuario).
                        ToList();

                }
                return oLista;
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


        //metodo para guardar o actualizar la incidencia
        public Incidencia Save(Incidencia incidencia)
        {
            //se crea una variable retorno para determinar si se guardaron cambios
            int retorno = 0;
            //Este objeto Incidencia se va a utilizar para retornar el rubro por ID
            Incidencia oIncidencia = null;                      
            using (MyContext ctx = new MyContext())
            {
                ctx.Configuration.LazyLoadingEnabled = false;
                oIncidencia = GetIncidenciaByID((int)incidencia.IdIncidencia);
                //si el rubro no existe devuelve null y se debe crear o insertar uno nuevo
                if (oIncidencia == null)
                {
                    //Insertar la Incidencia
                    IEnumerable<Incidencia> listaI = null;
                    int count = 0;
                    listaI = GetIncidencias();
                    foreach(var u in listaI)
                    {
                        count = u.IdIncidencia;
                    }
                    incidencia.IdIncidencia = count + 1;

                    //se debe guardar el valor del estado en solicitud por defecto
                    //incidencia.EstadoIncidencia = "En Solicitud";
                    //incidencia.FechaIncidencia = DateTime.Now;
                    incidencia.Usuario = null;
                    ctx.Incidencia.Add(incidencia);
                    //SaveChanges
                    //guarda todos los cambios realizados en el contexto de la base de datos.
                    retorno = ctx.SaveChanges();
                }
                else
                {
                    incidencia.Usuario = null;
                    //si el rubro si existe entonces se debe actualizar
                    ctx.Incidencia.Add(incidencia);
                    ctx.Entry(incidencia).State = EntityState.Modified;
                    retorno = ctx.SaveChanges();
                }
                //se debe retornar el rubro a la vista para mostrar los datos en la pantalla
                if (retorno >= 0)
                    oIncidencia = GetIncidenciaByID((int)incidencia.IdIncidencia);

                return oIncidencia;
            }
        }

        public IEnumerable<String> GetEstados()
        {
            IEnumerable<String> estados = new[] { "En proceso", "Finalizada" };

            return estados;
        }
    }
}
