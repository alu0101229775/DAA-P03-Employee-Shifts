using System;
using Newtonsoft.Json;

namespace DAA_P03.Core.EmployeeScheduling
{
    /// <summary>
    /// Representa un empleado en el problema de planificación.
    /// </summary>
    public class Employee
    {
        /// <summary>
        /// Nombre del empleado.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Número de días de descanso requeridos para este empleado.
        /// </summary>
        [JsonProperty("daysOff")]
        public int DaysOff { get; set; } = 1;

        public Employee() { }

        public Employee(string name, int daysOff = 1)
        {
            Name = name;
            DaysOff = daysOff;
        }

        public override string ToString() => $"{Name} ({DaysOff} descansos)";
    }
}
