using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TestNetCoreX.AccessData.Entities
{
    public class Usuario
    {
        [Key]
        public int ID { get; set; }
        [Required]
        [StringLength(20, ErrorMessage = "El campo nombre debe tener {1} caracteres o menos")]
        public string Nombre { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [StringLength(20, ErrorMessage = "El campo nombre debe tener {1} caracteres o menos")]
        public string Sexo { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(32, MinimumLength = 10)]
        public string PassWord { get; set; }

        public DateTime FechaRegistro { get; set; }
        public bool Activo { get; set; }

    }
}
