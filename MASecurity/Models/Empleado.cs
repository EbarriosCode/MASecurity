using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MASecurity.Models
{
    public class Empleado
    {
        public int EmpleadoID { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public string Nombre { get; set; }
        
        [Required(ErrorMessage = "El campo {0} es obligatorio")]       
        [DataType(DataType.PhoneNumber, ErrorMessage = "{0} Inválido")]
        [RegularExpression(@"^[-0-9A-Za-zá-úÁ-Ú ]+$", ErrorMessage = "El campo {0} solo debe contener carácteres alfanúmericos")]
        [Display(Name = "Teléfono")]
        public int Telefono { get; set; }
        
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [Display(Name = "Dirección")]
        public string Direccion { get; set; }

        [Display(Name = "Puesto")]
        public int PuestoID { get; set; }
        public Puesto Puesto { get; set; }
    }
}
