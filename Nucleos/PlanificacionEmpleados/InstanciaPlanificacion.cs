using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace DAA_P03.Nucleos.PlanificacionEmpleados
{
    /// <summary>
    /// Representa una instancia del problema de planificación de empleados.
    /// Contiene empleados, turnos y matrices de satisfacción/cobertura.
    /// </summary>
    public class InstanciaPlanificacion
    {
        /// <summary>
        /// Lista de empleados con sus propiedades (nombre, días de descanso).
        /// </summary>
        [JsonProperty("employees")]
        public List<Empleado> Empleados { get; set; } = new List<Empleado>();

        /// <summary>
        /// Número de días a planificar (horizonte de planificación).
        /// </summary>
        [JsonProperty("planningHorizon")]
        public int NumDias { get; set; }

        /// <summary>
        /// Lista de turnos disponibles.
        /// </summary>
        [JsonProperty("shifts")]
        public List<string> Turnos { get; set; } = new List<string>();

        /// <summary>
        /// Matriz de satisfacción: Satisfaccion[e][d][t] = satisfacción del empleado e, día d, turno t.
        /// </summary>
        [JsonIgnore]
        public int[,,] Satisfaccion { get; set; }

        /// <summary>
        /// Matriz de cobertura mínima: CoberturaMínima[d][t] = mínimo de empleados para día d, turno t.
        /// </summary>
        [JsonIgnore]
        public int[,] CoberturaMínima { get; set; }

        public int NumEmpleados => Empleados?.Count ?? 0;
        public int NumTurnos => Turnos?.Count ?? 0;

        /// <summary>
        /// Constructor predeterminado.
        /// </summary>
        public InstanciaPlanificacion()
        {
        }

        /// <summary>
        /// Constructor que inicializa las matrices de la instancia.
        /// </summary>
        public InstanciaPlanificacion(int numEmpleados, int numDias, int numTurnos)
        {
            NumDias = numDias;
            Turnos = new List<string>();
            Empleados = new List<Empleado>();
            Satisfaccion = new int[numEmpleados, numDias, numTurnos];
            CoberturaMínima = new int[numDias, numTurnos];
        }

        /// <summary>
        /// Obtiene los nombres de los empleados.
        /// </summary>
        public List<string> ObtenerNombresEmpleados()
        {
            var nombres = new List<string>();
            foreach (var emp in Empleados)
                nombres.Add(emp.Nombre);
            return nombres;
        }

        /// <summary>
        /// Obtiene los días de descanso de cada empleado.
        /// </summary>
        public int[] ObtenerDiasDescansoEmpleados()
        {
            var diasDescanso = new int[NumEmpleados];
            for (int i = 0; i < NumEmpleados; i++)
                diasDescanso[i] = Empleados[i].DiasDescanso;
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

        public InstanciaPlanificacion ObtenerSubinstancia(int diaInicio, int diaFin)
        {
            if (diaInicio < 0 || diaFin >= NumDias || diaInicio > diaFin)
                throw new ArgumentException("Rango de días inválido.");

            int numDiasNuevos = diaFin - diaInicio + 1;
            var sub = new InstanciaPlanificacion
            {
                Empleados = new List<Empleado>(Empleados),
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
