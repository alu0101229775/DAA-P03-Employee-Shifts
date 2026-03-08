using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAA_P03.Core.EmployeeScheduling
{
    /// <summary>
    /// Representa la solución al problema de planificación de empleados.
    /// Contiene la asignación de turnos a empleados para cada día.
    /// </summary>
    public class SolutionPlanning
    {
        /// <summary>
        /// Matriz de asignación: Plan[d][t] = lista de índices de empleados asignados
        /// d = día (0 a NumDias-1)
        /// t = turno (0 a NumTurnos-1)
        /// </summary>
        public List<int>[][] Plan { get; set; }

        /// <summary>
        /// Número de días de la planificación.
        /// </summary>
        public int NumDias { get; private set; }

        /// <summary>
        /// Número de turnos por día.
        /// </summary>
        public int NumTurnos { get; private set; }

        /// <summary>
        /// Número de empleados.
        /// </summary>
        public int NumEmpleados { get; private set; }

        /// <summary>
        /// Suma total de satisfacción de los empleados.
        /// </summary>
        public double SatisfaccionTotal { get; set; }

        /// <summary>
        /// Número de turnos cubiertos (que cumplen cobertura mínima).
        /// </summary>
        public int TurnosCubiertos { get; set; }

        /// <summary>
        /// Número de turnos no cubiertos (que NO cumplen cobertura mínima).
        /// </summary>
        public int TurnosNoCubiertos { get; set; }

        /// <summary>
        /// Valor de la función objetivo: f(x) = Satisfacción + TurnosCubiertos * 100
        /// </summary>
        public double FuncionObjetivo { get; set; }

        /// <summary>
        /// Indicador de si todas las restricciones se cumplen.
        /// </summary>
        public bool RestriccionesValidas { get; set; }

        /// <summary>
        /// Constructor que inicializa la solución.
        /// </summary>
        /// <param name="numDias">Número de días.</param>
        /// <param name="numTurnos">Número de turnos.</param>
        /// <param name="numEmpleados">Número de empleados.</param>
        public SolutionPlanning(int numDias, int numTurnos, int numEmpleados)
        {
            NumDias = numDias;
            NumTurnos = numTurnos;
            NumEmpleados = numEmpleados;

            // Inicializar la matriz de asignación
            Plan = new List<int>[numDias][];
            for (int d = 0; d < numDias; d++)
            {
                Plan[d] = new List<int>[numTurnos];
                for (int t = 0; t < numTurnos; t++)
                {
                    Plan[d][t] = new List<int>();
                }
            }

            SatisfaccionTotal = 0;
            TurnosCubiertos = 0;
            TurnosNoCubiertos = 0;
            FuncionObjetivo = 0;
            RestriccionesValidas = true;
        }

        /// <summary>
        /// Asigna un empleado a un turno específico.
        /// </summary>
        /// <param name="dia">Día (0-indexado).</param>
        /// <param name="turno">Turno (0-indexado).</param>
        /// <param name="empleado">Índice del empleado.</param>
        public void AsignarEmpleado(int dia, int turno, int empleado)
        {
            if (dia < 0 || dia >= NumDias)
                throw new ArgumentException("Día inválido.", nameof(dia));
            if (turno < 0 || turno >= NumTurnos)
                throw new ArgumentException("Turno inválido.", nameof(turno));
            if (empleado < 0 || empleado >= NumEmpleados)
                throw new ArgumentException("Empleado inválido.", nameof(empleado));

            if (!Plan[dia][turno].Contains(empleado))
            {
                Plan[dia][turno].Add(empleado);
            }
        }

        /// <summary>
        /// Obtiene los empleados asignados a un turno específico.
        /// </summary>
        /// <param name="dia">Día (0-indexado).</param>
        /// <param name="turno">Turno (0-indexado).</param>
        /// <returns>Lista de índices de empleados asignados.</returns>
        public List<int> ObtenerEmpleadosDelTurno(int dia, int turno)
        {
            if (dia < 0 || dia >= NumDias)
                throw new ArgumentException("Día inválido.", nameof(dia));
            if (turno < 0 || turno >= NumTurnos)
                throw new ArgumentException("Turno inválido.", nameof(turno));

            return new List<int>(Plan[dia][turno]);
        }

        /// <summary>
        /// Obtiene los turnos asignados a un empleado específico.
        /// </summary>
        /// <param name="empleado">Índice del empleado.</param>
        /// <returns>Lista de tuplas (día, turno).</returns>
        public List<(int dia, int turno)> ObtenerTurnosDelEmpleado(int empleado)
        {
            var turnos = new List<(int, int)>();
            for (int d = 0; d < NumDias; d++)
            {
                for (int t = 0; t < NumTurnos; t++)
                {
                    if (Plan[d][t].Contains(empleado))
                    {
                        turnos.Add((d, t));
                    }
                }
            }
            return turnos;
        }

        /// <summary>
        /// Obtiene el número de días de descanso de un empleado.
        /// </summary>
        /// <param name="empleado">Índice del empleado.</param>
        /// <returns>Número de días sin turnos asignados.</returns>
        public int ObtenerDiasDescanso(int empleado)
        {
            var diasTrabajo = new HashSet<int>();
            for (int d = 0; d < NumDias; d++)
            {
                for (int t = 0; t < NumTurnos; t++)
                {
                    if (Plan[d][t].Contains(empleado))
                    {
                        diasTrabajo.Add(d);
                    }
                }
            }
            return NumDias - diasTrabajo.Count;
        }

        /// <summary>
        /// Obtiene una representación formateada de la solución en forma de tabla.
        /// </summary>
        /// <param name="empleados">Lista de nombres de empleados.</param>
        /// <param name="turnos">Lista de nombres de turnos.</param>
        /// <returns>Representación formateada de la solución.</returns>
        public string ObtenerRepresentacionTabla(List<string> empleados, List<string> turnos)
        {
            var sb = new StringBuilder();

            sb.AppendLine("\n" + new string('=', 80));
            sb.AppendLine("PLAN DE ASIGNACIÓN DE TURNOS");
            sb.AppendLine(new string('=', 80));

            for (int d = 0; d < NumDias; d++)
            {
                sb.AppendLine($"\nDía {d + 1}:");
                sb.AppendLine(new string('-', 80));

                for (int t = 0; t < NumTurnos; t++)
                {
                    string turnoNombre = t < turnos.Count ? turnos[t] : $"Turno {t}";
                    var empleadosAsignados = Plan[d][t];
                    string asignacion = empleadosAsignados.Count > 0
                        ? string.Join(", ", empleadosAsignados.Select(e => 
                            e < empleados.Count ? empleados[e] : $"Empleado {e}"))
                        : "SIN ASIGNAR";

                    sb.AppendLine($"  {turnoNombre,20}: {asignacion}");
                }
            }

            sb.AppendLine("\n" + new string('=', 80));
            sb.AppendLine("INFORMACIÓN DE LA SOLUCIÓN");
            sb.AppendLine(new string('=', 80));
            sb.AppendLine($"Satisfacción Total: {SatisfaccionTotal}");
            sb.AppendLine($"Turnos Cubiertos: {TurnosCubiertos}");
            sb.AppendLine($"Turnos No Cubiertos: {TurnosNoCubiertos}");
            sb.AppendLine($"Función Objetivo: {FuncionObjetivo}");
            sb.AppendLine($"Restricciones Válidas: {(RestriccionesValidas ? "Sí" : "No")}");

            // Información de descansos
            sb.AppendLine("\nDías de Descanso por Empleado:");
            for (int e = 0; e < NumEmpleados; e++)
            {
                int diasDescanso = ObtenerDiasDescanso(e);
                string nombreEmpleado = e < empleados.Count ? empleados[e] : $"Empleado {e}";
                sb.AppendLine($"  {nombreEmpleado,20}: {diasDescanso} días");
            }

            sb.AppendLine(new string('=', 80));

            return sb.ToString();
        }

        /// <summary>
        /// Obtiene un resumen de la solución.
        /// </summary>
        /// <returns>Información resumida de la solución.</returns>
        public string ObtenerResumen()
        {
            return $"Solución: Satisfacción={SatisfaccionTotal}, Cubiertos={TurnosCubiertos}, " +
                   $"Objetivo={FuncionObjetivo}, Válida={RestriccionesValidas}";
        }

        /// <summary>
        /// Crea una copia profunda de la solución.
        /// </summary>
        /// <returns>Una nueva instancia de SolutionPlanning con los mismos datos.</returns>
        public SolutionPlanning ObtenerCopia()
        {
            var copia = new SolutionPlanning(NumDias, NumTurnos, NumEmpleados)
            {
                SatisfaccionTotal = SatisfaccionTotal,
                TurnosCubiertos = TurnosCubiertos,
                TurnosNoCubiertos = TurnosNoCubiertos,
                FuncionObjetivo = FuncionObjetivo,
                RestriccionesValidas = RestriccionesValidas
            };

            // Copiar el plan
            for (int d = 0; d < NumDias; d++)
            {
                for (int t = 0; t < NumTurnos; t++)
                {
                    copia.Plan[d][t] = new List<int>(Plan[d][t]);
                }
            }

            return copia;
        }

        /// <summary>
        /// Combina dos soluciones (de días consecutivos) en una solución integrada.
        /// </summary>
        /// <param name="otra">La otra solución (siguiente rango de días).</param>
        /// <returns>Una nueva solución con ambos rangos combinados.</returns>
        public SolutionPlanning Combinar(SolutionPlanning otra)
        {
            if (otra == null)
                throw new ArgumentNullException(nameof(otra));

            int numDiasCombinado = NumDias + otra.NumDias;
            var combinada = new SolutionPlanning(numDiasCombinado, NumTurnos, NumEmpleados);

            // Copiar la primera solución
            for (int d = 0; d < NumDias; d++)
            {
                for (int t = 0; t < NumTurnos; t++)
                {
                    combinada.Plan[d][t] = new List<int>(Plan[d][t]);
                }
            }

            // Copiar la segunda solución con offset
            for (int d = 0; d < otra.NumDias; d++)
            {
                for (int t = 0; t < NumTurnos; t++)
                {
                    combinada.Plan[NumDias + d][t] = new List<int>(otra.Plan[d][t]);
                }
            }

            // Actualizar métricas (suma simple para ahora)
            combinada.SatisfaccionTotal = SatisfaccionTotal + otra.SatisfaccionTotal;
            combinada.TurnosCubiertos = TurnosCubiertos + otra.TurnosCubiertos;
            combinada.TurnosNoCubiertos = TurnosNoCubiertos + otra.TurnosNoCubiertos;
            combinada.FuncionObjetivo = combinada.SatisfaccionTotal + 
                                       (combinada.TurnosCubiertos * 100.0);

            return combinada;
        }
    }
}
