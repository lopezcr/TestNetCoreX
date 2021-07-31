using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TestNetCoreX.BusinessObject.Model
{
    class UsuarioDTO
    {
        [Required]
        [StringLength(20, ErrorMessage = "El campo nombre debe tener {1} caracteres o menos")]
        public string Nombre { get; set; }

        [Range(18, 20)]
        public int Edad { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }


        [Required]
        [DataType(DataType.Password)]
        [StringLength(20, MinimumLength = 10)]
        public string PassWord { get; set; }
    }
}
