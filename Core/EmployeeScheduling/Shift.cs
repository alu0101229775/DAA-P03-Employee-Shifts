using System;
using Newtonsoft.Json;

namespace DAA_P03.Core.EmployeeScheduling
{
    /// <summary>
    /// Representa un turno en el problema de planificación.
    /// </summary>
    public class Shift
    {
        /// <summary>
        /// Nombre del turno (ej: "T0", "Mañana", etc).
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Cobertura mínima requerida para este turno (puede variar por día).
        /// </summary>
        [JsonProperty("minimumCoverage")]
        public int MinimumCoverage { get; set; } = 1;

        public Shift() { }

        public Shift(string name, int minimumCoverage = 1)
        {
            Name = name;
            MinimumCoverage = minimumCoverage;
        }

        public override string ToString() => $"{Name} (mín: {MinimumCoverage})";
    }
}
