using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace DAA_P03.Core.EmployeeScheduling
{
    /// <summary>
    /// Representa una instancia del problema de planificación de empleados.
    /// Contiene todos los datos necesarios para resolver el problema.
    /// </summary>
    public class InstancePlanning
    {
        /// <summary>
        /// Conjunto de empleados disponibles: E.
        /// </summary>
        [JsonProperty("empleados")]
        public List<string> Empleados { get; set; } = new List<string>();

        /// <summary>
        /// Número de días a planificar: D.
        /// </summary>
        [JsonProperty("dias")]
        public int NumDias { get; set; }

        /// <summary>
        /// Conjunto de turnos a cubrir en un día: T.
        /// </summary>
        [JsonProperty("turnos")]
        public List<string> Turnos { get; set; } = new List<string>();

        /// <summary>
        /// Matriz de satisfacción: A[e][d][t]
        /// Valor que indica la satisfacción del empleado e, si el día d hace el turno t.
        /// </summary>
        [JsonProperty("satisfaccion")]
        public int[,,] Satisfaccion { get; set; }

        /// <summary>
        /// Matriz de cobertura mínima: B[d][t]
        /// Número mínimo de empleados que se necesitan para cubrir el turno t el día d.
        /// </summary>
        [JsonProperty("coberturaminima")]
        public int[,] CoberturaMínima { get; set; }

        /// <summary>
        /// Array de días de descanso: C[e]
        /// Número de días de descanso que debe tener el empleado e en la planificación.
        /// </summary>
        [JsonProperty("diasdescanso")]
        public int[] DiasDescanso { get; set; }

        /// <summary>
        /// Obtiene el número de empleados.
        /// </summary>
        public int NumEmpleados => Empleados?.Count ?? 0;

        /// <summary>
        /// Obtiene el número de turnos por día.
        /// </summary>
        public int NumTurnos => Turnos?.Count ?? 0;

        /// <summary>
        /// Obtiene información sobre la instancia en formato string.
        /// </summary>
        /// <returns>Información formateada de la instancia.</returns>
        public override string ToString()
        {
            return $"Problema de Planificación de Empleados\n" +
                   $"  Empleados: {NumEmpleados}\n" +
                   $"  Días: {NumDias}\n" +
                   $"  Turnos por día: {NumTurnos}\n" +
                   $"  Total de turnos: {NumDias * NumTurnos}";
        }

        /// <summary>
        /// Valida que la instancia tenga todos los datos necesarios.
        /// </summary>
        /// <returns>true si la instancia es válida, false en caso contrario.</returns>
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
        /// Obtiene una copia profunda de la instancia.
        /// </summary>
        /// <returns>Una nueva instancia con los mismos datos.</returns>
        public InstancePlanning ObtenerCopia()
        {
            var copia = new InstancePlanning
            {
                Empleados = new List<string>(Empleados),
                NumDias = NumDias,
                Turnos = new List<string>(Turnos),
                Satisfaccion = (int[,,])Satisfaccion.Clone(),
                CoberturaMínima = (int[,])CoberturaMínima.Clone(),
                DiasDescanso = (int[])DiasDescanso.Clone()
            };
            return copia;
        }

        /// <summary>
        /// Obtiene una subinstancia para un rango de días específico.
        /// </summary>
        /// <param name="diaInicio">Día de inicio (0-indexado).</param>
        /// <param name="diaFin">Día de fin (0-indexado, inclusive).</param>
        /// <returns>Una nueva instancia con solo los días especificados.</returns>
        public InstancePlanning ObtenerSubinstancia(int diaInicio, int diaFin)
        {
            if (diaInicio < 0 || diaFin >= NumDias || diaInicio > diaFin)
                throw new ArgumentException("Rango de días inválido.");

            int numDiasNuevos = diaFin - diaInicio + 1;
            var subinstancia = new InstancePlanning
            {
                Empleados = new List<string>(Empleados),
                NumDias = numDiasNuevos,
                Turnos = new List<string>(Turnos),
                Satisfaccion = new int[NumEmpleados, numDiasNuevos, NumTurnos],
                CoberturaMínima = new int[numDiasNuevos, NumTurnos],
                DiasDescanso = (int[])DiasDescanso.Clone()
            };

            // Copiar solo los días especificados
            for (int e = 0; e < NumEmpleados; e++)
            {
                for (int d = diaInicio; d <= diaFin; d++)
                {
                    for (int t = 0; t < NumTurnos; t++)
                    {
                        subinstancia.Satisfaccion[e, d - diaInicio, t] = Satisfaccion[e, d, t];
                    }
                }
            }

            for (int d = diaInicio; d <= diaFin; d++)
            {
                for (int t = 0; t < NumTurnos; t++)
                {
                    subinstancia.CoberturaMínima[d - diaInicio, t] = CoberturaMínima[d, t];
                }
            }

            return subinstancia;
        }
    }
}
