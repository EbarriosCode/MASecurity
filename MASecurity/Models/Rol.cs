using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MASecurity.Models
{
    public class Rol
    {
        [Key]
        public int RolId { get; set; }
        public string RolName { get; set; }
        public string Permisos { get; set; }

        public ICollection<Usuario> Usuarios { get; set; }
    }
}
