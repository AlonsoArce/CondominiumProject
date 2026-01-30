using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Models
{
    internal partial class UsuarioMetadata
    {
        [Display(Name = "Número de Usuario")]
        public int IdUsuario { get; set; }
        [Display(Name = "Email")]
        public string Correo { get; set; }
        public string Password { get; set; }
        [Display(Name = "Nombre Usuario")]
        public string Nombre { get; set; }
        [Display(Name = "Apellidos")]
        public string Apellidos { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Incidencia> Incidencia { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Residencia> Residencia { get; set; }
    }

    internal partial class ResidenciaMetadata
    {
        [Display(Name = "Condominio Número")]
        [RegularExpression(@"^\d+$", ErrorMessage = "{0} solo acepta números")]
        [Required(ErrorMessage = "{0} es un dato requerido")]
        public int IdResidencia { get; set; }

        [Display(Name = "ID Usuario")]
        [RegularExpression(@"^\d+$", ErrorMessage = "{0} solo acepta números")]
        [Required(ErrorMessage = "{0} es un dato requerido")]
        public int IdUsuario { get; set; }

        [Display(Name = "Cantidad Personas")]
        [RegularExpression(@"^\d+$", ErrorMessage = "{0} solo acepta números")]
        [Required(ErrorMessage = "{0} es un dato requerido")]
        public Nullable<int> CantPersonas { get; set; }

        [Display(Name = "Cantidad Autos")]
        [RegularExpression(@"^\d+$", ErrorMessage = "{0} solo acepta números")]
        [Required(ErrorMessage = "{0} es un dato requerido")]
        public Nullable<int> CantAutos { get; set; }

        [Display(Name = "Fecha de Inicio")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
        //[Required(ErrorMessage = "{0} es un dato requerido")]
        public Nullable<System.DateTime> FechaInicio { get; set; }

        [Display(Name = "Estado")]
        [Required(ErrorMessage = "{0} es un dato requerido")]
        public string Estado { get; set; }

        [Display(Name = "Asigna Plan Cobro")]
        public virtual ICollection<AsignaPlanCobro> AsignaPlanCobro { get; set; }
        [Display(Name = "Reserva")]
        public virtual ICollection<ReservaAreaComun> ReservaAreaComun { get; set; }
        [Display(Name = "Usuario")]
        public virtual Usuario Usuario { get; set; }
    }

    internal partial class PlanCobroMetadata
    {
        [Display(Name = "Plan de Cobro")]
        //[Required(ErrorMessage = "{0} es un dato requerido")]
        public int IdPlanCobro { get; set; }

        [Display(Name = "Descripción del Plan")]
        [Required(ErrorMessage = "{0} es un dato requerido")]
        public string Descripcion { get; set; }

        [Display(Name = "Monto total")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        [RegularExpression(@"^[0-9]+(\.[0-9]{1,2})?$", ErrorMessage = "solo acepta números, con dos decimales")]
        //[Required(ErrorMessage = "{0} es un dato requerido")]
        public Nullable<decimal> MontoTotal { get; set; }

        public virtual ICollection<AsignaPlanCobro> AsignaPlanCobro { get; set; }

        [Display(Name = "Rubro")]
        public virtual ICollection<RubroCobro> RubroCobro { get; set; }
    }

    internal partial class IncidenciaMetadata
    {
        [Display(Name = "Número de Incidencia")]
        [RegularExpression(@"^\d+$", ErrorMessage = "{0} solo acepta números")]
        [Required(ErrorMessage = "{0} es un dato requerido")]
        public int IdIncidencia { get; set; }

        [Display(Name = "Número de Usuario")]
        [RegularExpression(@"^\d+$", ErrorMessage = "{0} solo acepta números")]
        [Required(ErrorMessage = "{0} es un dato requerido")]
        public int IdUsuario { get; set; }

        [Display(Name = "Estado")]
        [Required(ErrorMessage = "{0} es un dato requerido")]
        public string EstadoIncidencia { get; set; }


        [Display(Name = "Descripción de Incidencia")]
        [Required(ErrorMessage = "{0} es un dato requerido")]
        public string Descripcion { get; set; }

        [Display(Name = "Fecha de Incidencia")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
        public Nullable<System.DateTime> FechaIncidencia { get; set; }

        [Display(Name = "Usuario")]
        public virtual Usuario Usuario { get; set; }
    }

    internal partial class RubroCobroMetadata
    {
        [Display(Name = "Rubro")]
        [RegularExpression(@"^\d+$", ErrorMessage = "{0} solo acepta números")]
        [Required(ErrorMessage = "{0} es un dato requerido")]
        public int IdRubro { get; set; }

        [Display(Name = "Descripción")]
        [Required(ErrorMessage = "{0} es un dato requerido")]
        public string Descripcion { get; set; }

        [Display(Name = "Monto de Rubro")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        [RegularExpression(@"^[0-9]+(\.[0-9]{1,2})?$", ErrorMessage = "solo acepta números, con dos decimales")]
        [Required(ErrorMessage = "{0} es un dato requerido")]
        public Nullable<decimal> MontoRubro { get; set; }
        public virtual ICollection<PlanCobro> PlanCobro { get; set; }
    }

    internal partial class InformacionMetadata
    {
        [Display(Name = "Número de Información")]
        [RegularExpression(@"^\d+$", ErrorMessage = "{0} solo acepta números")]
        [Required(ErrorMessage = "{0} es un dato requerido")]
        public int IdInformacion { get; set; }

        [Display(Name = "Tipo")]
        [Required(ErrorMessage = "{0} es un dato requerido")]
        public int IdTipoInformacion { get; set; }

        [Display(Name = "Descripción")]
        [Required(ErrorMessage = "{0} es un dato requerido")]
        public string Descripcion { get; set; }

        [Display(Name = "Imagen Información")]
        [Required(ErrorMessage = "{0} es un dato requerido")]
        public byte[] Imagen { get; set; }
        public virtual TipoInformacion TipoInformacion { get; set; }
    }

    internal partial class TipoInformacionMetadata
    {
        [Display(Name = "Tipo de Información")]
        [Required(ErrorMessage = "{0} es un dato requerido")]
        public int IdTipo { get; set; }

        [Display(Name = "Descripción de Tipo")]
        [Required(ErrorMessage = "{0} es un dato requerido")]
        public string Descripcion { get; set; }      
        public virtual ICollection<Informacion> Informacion { get; set; }
    }

    internal partial class AsignaPlanCobroMetadata
    {
        [Display(Name = "ID Plan")]
        [Required(ErrorMessage = "{0} es un dato requerido")]
        public int IdPlanCobro { get; set; }

        [Display(Name = "ID Residencia")]
        [Required(ErrorMessage = "{0} es un dato requerido")]
        public int IdResidencia { get; set; }

        [Display(Name = "Mes Asignado")]
        [Required(ErrorMessage = "{0} es un dato requerido")]
        public string MesAsignado { get; set; }

        [Display(Name = "Estado")]
        [Required(ErrorMessage = "{0} es un dato requerido")]
        public string Estado { get; set; }

        public virtual PlanCobro PlanCobro { get; set; }
        public virtual Residencia Residencia { get; set; }
    }

    internal partial class ReservaAreaComunMetadata
    {
        [Display(Name = "ID Reserva")]
        [Required(ErrorMessage = "{0} es un dato requerido")]
        public int IdReserva { get; set; }

        [Display(Name = "ID Residencia")]
        [Required(ErrorMessage = "{0} es un dato requerido")]
        public int IdResidencia { get; set; }

        [Display(Name = "Área común")]
        [Required(ErrorMessage = "{0} es un dato requerido")]
        public string AreaComun { get; set; }

        [Display(Name = "Fecha de Solicitud")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
        //[Required(ErrorMessage = "{0} es un dato requerido")]
        public System.DateTime FechaSolicitud { get; set; }

        [Display(Name = "Fecha de Reserva")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
        //[Required(ErrorMessage = "{0} es un dato requerido")]
        public System.DateTime FechaReserva { get; set; }

        [Display(Name = "Horarios")]
        [Required(ErrorMessage = "{0} es un dato requerido")]
        public string Horario { get; set; }

        [Display(Name = "Estado")]
        [Required(ErrorMessage = "{0} es un dato requerido")]
        public string Estado { get; set; }

        public virtual Residencia Residencia { get; set; }
    }

}
