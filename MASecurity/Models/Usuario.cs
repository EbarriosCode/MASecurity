using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MASecurity.Models
{
    public class Usuario
    {
        [Key]
        public Guid UserId { get; set; }
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [Display(Name = "Usuario")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }
        public string Name { get; set; }

        public int RolId { get; set; }
        public Rol Rol { get; set; }
    }
}
