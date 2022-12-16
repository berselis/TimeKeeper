﻿

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeKeeper.Models
{
    public class Empleado
    {

        [Key, Required]
        public int IdEmpleado { get; set; }
        [Required, MaxLength(100)]
        public string Nombre { get; set; }
        [Required, MaxLength(100)]
        public string Departamento { get; set; }
        public bool HasImg { get; set; }

        public ICollection<Tiempo> Tiempos { get; set; }
    }
}