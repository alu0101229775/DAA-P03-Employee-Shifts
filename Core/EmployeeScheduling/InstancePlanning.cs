using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace DAA_P03.Core.EmployeeScheduling
{
    /// <summary>
    /// Representa una instancia del problema de planificación de empleados.
    /// Contiene empleados, turnos y matrices de satisfacción/cobertura.
    /// </summary>
    public class InstancePlanning
    {
        /// <summary>
        /// Lista de empleados con sus propiedades (nombre, días de descanso).
        /// </summary>
        [JsonProperty("employees")]
        public List<Employee> Empleados { get; set; } = new List<Employee>();

        /// <summary>
        /// Número de días a planificar (horizonte de planificación).
        /// </summary>
        [JsonProperty("planningHorizon")]
        public int NumDias { get; set; }

        /// <summary>
        /// Lista de turnos disponibles, cada uno con nombre y cobertura mínima.
        /// </summary>
        [JsonProperty("shifts")]
        public List<string> Turnos { get; set; } = new List<string>();

        /// <summary>
        /// Matriz de satisfacción: A[e][d][t] = satisfacción del empleado e, día d, turno t.
        /// </summary>
        [JsonIgnore]
        public int[,,] Satisfaccion { get; set; }

        /// <summary>
        /// Matriz de cobertura mínima: B[d][t] = mínimo de empleados para día d, turno t.
        /// </summary>
        [JsonIgnore]
        public int[,] CoberturaMínima { get; set; }

        /// <summary>
        /// Array de días de descanso: C[e] = días de descanso del empleado e.
        /// </summary>
        [JsonIgnore]
        public int[] DiasDescanso { get; set; }

        public int NumEmpleados => Empleados?.Count ?? 0;
        public int NumTurnos => Turnos?.Count ?? 0;

        /// <summary>
        /// Obtiene los nombres de los empleados para compatibilidad.
        /// </summary>
        public List<string> ObtenerNombresEmpleados()
        {
            var nombres = new List<string>();
            foreach (var emp in Empleados)
                nombres.Add(emp.Name);
            return nombres;
        }

        /// <summary>
        /// Obtiene los días de descanso de cada empleado.
        /// </summary>
        public int[] ObtenerDiasDescansoEmpleados()
        {
            var diasDescanso = new int[NumEmpleados];
            for (int i = 0; i < NumEmpleados; i++)
                diasDescanso[i] = Empleados[i].DaysOff;
            return diasDescanso;
        }

        public override string ToString()
        {
            return $"Instancia: {NumEmpleados} empleados, {NumDias} días, {NumTurnos} turnos";
        }

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
            return true;
        }

        public InstancePlanning ObtenerSubinstancia(int diaInicio, int diaFin)
        {
            if (diaInicio < 0 || diaFin >= NumDias || diaInicio > diaFin)
                throw new ArgumentException("Rango de días inválido.");

            int numDiasNuevos = diaFin - diaInicio + 1;
            var sub = new InstancePlanning
            {
                Empleados = new List<Employee>(Empleados),
                NumDias = numDiasNuevos,
                Turnos = new List<string>(Turnos),
                Satisfaccion = new int[NumEmpleados, numDiasNuevos, NumTurnos],
                CoberturaMínima = new int[numDiasNuevos, NumTurnos]
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
