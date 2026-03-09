using System;
using Newtonsoft.Json;

namespace DAA_P03.Nucleos.PlanificacionEmpleados
{
    /// <summary>
    /// Representa un empleado en el problema de planificación.
    /// </summary>
    public class Empleado
    {
        /// <summary>
        /// Nombre del empleado.
        /// </summary>
        [JsonProperty("name")]
        public string Nombre { get; set; }

        /// <summary>
        /// Número de días de descanso requeridos para este empleado.
        /// </summary>
        [JsonProperty("daysOff")]
        public int DiasDescanso { get; set; } = 1;

        public Empleado() { }

        public Empleado(string nombre, int diasDescanso = 1)
        {
            Nombre = nombre;
            DiasDescanso = diasDescanso;
        }

        public override string ToString() => $"{Nombre} ({DiasDescanso} descansos)";
    }
}
