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
    public class RepositoryInformacion : IRepositoryInformacion
    {
        public IEnumerable<Informacion> GetInformacion()
        {
            try
            {
                IEnumerable<Informacion> lista = null;
                using (MyContext ctx = new MyContext())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;                   
                    lista = ctx.Informacion.
                        Include("TipoInformacion").
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

        public Informacion GetInformacionByID(int id)
        {
            Informacion oInformacion = null;
            try
            {
                using (MyContext ctx = new MyContext())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;
                    //Obtener libro por ID incluyendo el autor y todas sus categorías
                    oInformacion = ctx.Informacion.
                        Where(i => i.IdInformacion == id).
                        Include("TipoInformacion").                      
                        FirstOrDefault();

                }
                return oInformacion;
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

        public IEnumerable<Informacion> GetInfoByTipo(int idTipo)
        {
            IEnumerable<Informacion> oInformacion = null;
            try
            {
                using (MyContext ctx = new MyContext())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;
                    //Obtener libros por Autor
                    oInformacion = ctx.Informacion.
                        Where(i => i.IdTipoInformacion == idTipo).
                        Include("TipoInformacion").ToList();

                }
                return oInformacion;
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

        public Informacion Save(Informacion informacion)
        {
            int retorno = 0;
            Informacion oInformacion = null;

            using (MyContext ctx = new MyContext())
            {
                ctx.Configuration.LazyLoadingEnabled = false;
                oInformacion = GetInformacionByID((int)informacion.IdInformacion);
                //IRepositoryCategoria _RepositoryCategoria = new RepositoryCategoria();

                if (oInformacion == null)
                {

                    //Insertar
                    //Logica para agregar las categorias al libro
                    /*if (selectedCategorias != null)
                    {

                        libro.Categoria = new List<Categoria>();
                        foreach (var categoria in selectedCategorias)
                        {
                            var categoriaToAdd = _RepositoryCategoria.GetCategoriaByID(int.Parse(categoria));
                            ctx.Categoria.Attach(categoriaToAdd); //sin esto, EF intentará crear una categoría
                            libro.Categoria.Add(categoriaToAdd);// asociar a la categoría existente con el libro


                        }
                    }*/
                    //Insertar Libro
                    ctx.Informacion.Add(informacion);
                    //SaveChanges
                    //guarda todos los cambios realizados en el contexto de la base de datos.
                    retorno = ctx.SaveChanges();
                    //retorna número de filas afectadas
                }
                else
                {
                    //Registradas: 1,2,3
                    //Actualizar: 1,3,4

                    //Actualizar Libro
                    ctx.Informacion.Add(informacion);
                    ctx.Entry(informacion).State = EntityState.Modified;
                    retorno = ctx.SaveChanges();

                    //Logica para actualizar Categorias
                    /*var selectedCategoriasID = new HashSet<string>(selectedCategorias);
                    if (selectedCategorias != null)
                    {
                        ctx.Entry(libro).Collection(p => p.Categoria).Load();
                        var newCategoriaForLibro = ctx.Categoria
                         .Where(x => selectedCategoriasID.Contains(x.IdCategoria.ToString())).ToList();
                        libro.Categoria = newCategoriaForLibro;

                        ctx.Entry(libro).State = EntityState.Modified;
                        retorno = ctx.SaveChanges();
                    }*/
                }
            }

            if (retorno >= 0)
                oInformacion = GetInformacionByID((int)informacion.IdInformacion);

            return oInformacion;
        }
    }
}
