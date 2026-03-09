using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace DAA_P03.Core.EmployeeScheduling
{
    /// <summary>
    /// Representa una instancia del problema de planificación de empleados.
    /// Contiene:
    /// - E: Conjunto de empleados disponibles.
    /// - D: Número de días a planificar.
    /// - T: Conjunto de turnos a cubrir en un día.
    /// - A[e][d][t]: Matriz de satisfacción del empleado e, día d, turno t.
    /// - B[d][t]: Matriz de cobertura mínima (número mínimo de empleados).
    /// - C[e]: Array de días de descanso requeridos por empleado.
    /// </summary>
    public class InstancePlanning
    {
        [JsonProperty("empleados")]
        public List<string> Empleados { get; set; } = new List<string>();

        [JsonProperty("dias")]
        public int NumDias { get; set; }

        [JsonProperty("turnos")]
        public List<string> Turnos { get; set; } = new List<string>();

        [JsonProperty("satisfaccion")]
        public int[,,] Satisfaccion { get; set; }

        [JsonProperty("coberturaminima")]
        public int[,] CoberturaMínima { get; set; }

        [JsonProperty("diasdescanso")]
        public int[] DiasDescanso { get; set; }

        public int NumEmpleados => Empleados?.Count ?? 0;
        public int NumTurnos => Turnos?.Count ?? 0;

        /// <summary>
        /// Representa la instancia en formato texto.
        /// </summary>
        public override string ToString()
        {
            return $"Instancia de Planificación: {NumEmpleados} empleados, " +
                   $"{NumDias} días, {NumTurnos} turnos/día";
        }

        /// <summary>
        /// Valida que la instancia tenga todos los datos correctos.
        /// </summary>
        public bool EsValida()
        {
            if (Empleados == null || Empleados.Count == 0) return false;
            if (NumDias <= 0) return false;
            if (Turnos == null || Turnos.Count == 0) return false;
            if (Satisfaccion == null || Satisfaccion.GetLength(0) != NumEmpleados 
                || Satisfaccion.GetLength(1) != NumDias 
                || Satisfaccion.GetLength(2) != NumTurnos) return false;
            if (CoberturaMínima == null || CoberturaMínima.GetLength(0) != NumDias 
                || CoberturaMínima.GetLength(1) != NumTurnos) return false;
            if (DiasDescanso == null || DiasDescanso.Length != NumEmpleados) return false;
            return true;
        }

        /// <summary>
        /// Obtiene una subinstancia para un rango de días (para Divide y Vencerás).
        /// </summary>
        public InstancePlanning ObtenerSubinstancia(int diaInicio, int diaFin)
        {
            if (diaInicio < 0 || diaFin >= NumDias || diaInicio > diaFin)
                throw new ArgumentException("Rango de días inválido.");

            int numDiasNuevos = diaFin - diaInicio + 1;
            var sub = new InstancePlanning
            {
                Empleados = new List<string>(Empleados),
                NumDias = numDiasNuevos,
                Turnos = new List<string>(Turnos),
                Satisfaccion = new int[NumEmpleados, numDiasNuevos, NumTurnos],
                CoberturaMínima = new int[numDiasNuevos, NumTurnos],
                DiasDescanso = (int[])DiasDescanso.Clone()
            };

            for (int e = 0; e < NumEmpleados; e++)
                for (int d = diaInicio; d <= diaFin; d++)
                    for (int t = 0; t < NumTurnos; t++)
                        sub.Satisfaccion[e, d - diaInicio, t] = Satisfaccion[e, d, t];

            for (int d = diaInicio; d <= diaFin; d++)
                for (int t = 0; t < NumTurnos; t++)
                    sub.CoberturaMínima[d - diaInicio, t] = CoberturaMínima[d, t];

            return sub;
        }
    }
}
