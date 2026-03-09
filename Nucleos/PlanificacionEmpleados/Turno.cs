using System;
using Newtonsoft.Json;

namespace DAA_P03.Nucleos.PlanificacionEmpleados
{
    /// <summary>
    /// Representa un turno en el problema de planificación.
    /// </summary>
    public class Turno
    {
        /// <summary>
        /// Nombre del turno (ej: "T0", "Mañana", etc).
        /// </summary>
        [JsonProperty("name")]
        public string Nombre { get; set; }

        /// <summary>
        /// Cobertura mínima requerida para este turno.
        /// </summary>
        [JsonProperty("minimumCoverage")]
        public int CoberturaMínima { get; set; } = 1;

        public Turno() { }

        public Turno(string nombre, int coberturaMínima = 1)
        {
            Nombre = nombre;
            CoberturaMínima = coberturaMínima;
        }

        public override string ToString() => $"{Nombre} (mín: {CoberturaMínima})";
    }
}
