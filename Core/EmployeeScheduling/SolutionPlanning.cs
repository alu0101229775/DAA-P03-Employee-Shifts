using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAA_P03.Core.EmployeeScheduling
{
    /// <summary>
    /// Representa la solución al problema de planificación de empleados.
    /// Contiene la asignación de turnos a empleados para cada día.
    /// plan[d][t] = lista de índices de empleados asignados al turno t del día d.
    /// </summary>
    public class SolutionPlanning
    {
        /// <summary>
        /// Plan de asignación: plan[día][turno] = lista de índices de empleados.
        /// </summary>
        public List<int>[][] Plan { get; set; }

        public int NumDias { get; private set; }
        public int NumTurnos { get; private set; }
        public int NumEmpleados { get; private set; }

        /// <summary>
        /// Suma total de satisfacción de los empleados.
        /// </summary>
        public double SatisfaccionTotal { get; set; }

        /// <summary>
        /// Número de turnos que cumplen la cobertura mínima.
        /// </summary>
        public int TurnosCubiertos { get; set; }

        /// <summary>
        /// Función objetivo: f(x) = SUMA(satisfaccion) + SUMA(turnos_cubiertos) * 100
        /// </summary>
        public double FuncionObjetivo { get; set; }

        public SolutionPlanning(int numDias, int numTurnos, int numEmpleados)
        {
            NumDias = numDias;
            NumTurnos = numTurnos;
            NumEmpleados = numEmpleados;

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
            FuncionObjetivo = 0;
        }

        /// <summary>
        /// Asigna un empleado a un turno específico.
        /// </summary>
        public void AsignarEmpleado(int dia, int turno, int empleado)
        {
            if (dia < 0 || dia >= NumDias || turno < 0 || turno >= NumTurnos 
                || empleado < 0 || empleado >= NumEmpleados)
                throw new ArgumentException("Índices inválidos.");

            if (!Plan[dia][turno].Contains(empleado))
                Plan[dia][turno].Add(empleado);
        }

        /// <summary>
        /// Obtiene los empleados asignados a un turno.
        /// </summary>
        public List<int> ObtenerEmpleadosDelTurno(int dia, int turno)
        {
            return new List<int>(Plan[dia][turno]);
        }

        /// <summary>
        /// Calcula los días de descanso de un empleado.
        /// </summary>
        public int ObtenerDiasDescanso(int empleado)
        {
            var diasTrabajo = new HashSet<int>();
            for (int d = 0; d < NumDias; d++)
            {
                for (int t = 0; t < NumTurnos; t++)
                {
                    if (Plan[d][t].Contains(empleado))
                        diasTrabajo.Add(d);
                }
            }
            return NumDias - diasTrabajo.Count;
        }

        /// <summary>
        /// Representa la solución en forma de tabla legible.
        /// </summary>
        public string ObtenerRepresentacionTabla(List<string> empleados, List<string> turnos)
        {
            var sb = new StringBuilder();

            sb.AppendLine("\n" + new string('=', 100));
            sb.AppendLine("PLAN DE ASIGNACIÓN DE TURNOS");
            sb.AppendLine(new string('=', 100));

            for (int d = 0; d < NumDias; d++)
            {
                sb.AppendLine($"\n--- Día {d + 1} ---");
                for (int t = 0; t < NumTurnos; t++)
                {
                    string turnoNombre = t < turnos.Count ? turnos[t] : $"Turno {t}";
                    var asignados = Plan[d][t];
                    string empleadosStr = asignados.Count > 0
                        ? string.Join(", ", asignados.Select(e => 
                            e < empleados.Count ? empleados[e] : $"E{e}"))
                        : "SIN ASIGNAR";

                    sb.AppendLine($"  {turnoNombre,15}: {empleadosStr}");
                }
            }

            sb.AppendLine("\n" + new string('=', 100));
            sb.AppendLine("RESUMEN DE LA SOLUCIÓN");
            sb.AppendLine(new string('=', 100));
            sb.AppendLine($"Satisfacción Total: {SatisfaccionTotal}");
            sb.AppendLine($"Turnos Cubiertos: {TurnosCubiertos}");
            sb.AppendLine($"Función Objetivo: {FuncionObjetivo}");

            sb.AppendLine("\nDías de Descanso por Empleado:");
            for (int e = 0; e < NumEmpleados; e++)
            {
                int diasDescanso = ObtenerDiasDescanso(e);
                string nombreEmpleado = e < empleados.Count ? empleados[e] : $"E{e}";
                sb.AppendLine($"  {nombreEmpleado,15}: {diasDescanso} días");
            }

            sb.AppendLine(new string('=', 100));

            return sb.ToString();
        }

        /// <summary>
        /// Combina dos soluciones (de días consecutivos).
        /// </summary>
        public SolutionPlanning Combinar(SolutionPlanning otra)
        {
            if (otra == null)
                throw new ArgumentNullException(nameof(otra));

            var combinada = new SolutionPlanning(NumDias + otra.NumDias, NumTurnos, NumEmpleados);

            // Copiar la primera solución
            for (int d = 0; d < NumDias; d++)
                for (int t = 0; t < NumTurnos; t++)
                    combinada.Plan[d][t] = new List<int>(Plan[d][t]);

            // Copiar la segunda solución con offset
            for (int d = 0; d < otra.NumDias; d++)
                for (int t = 0; t < NumTurnos; t++)
                    combinada.Plan[NumDias + d][t] = new List<int>(otra.Plan[d][t]);

            combinada.SatisfaccionTotal = SatisfaccionTotal + otra.SatisfaccionTotal;
            combinada.TurnosCubiertos = TurnosCubiertos + otra.TurnosCubiertos;
            combinada.FuncionObjetivo = combinada.SatisfaccionTotal + 
                                       (combinada.TurnosCubiertos * 100.0);

            return combinada;
        }

        public override string ToString()
        {
            return $"Solución: Satisfacción={SatisfaccionTotal}, Cubiertos={TurnosCubiertos}, " +
                   $"Objetivo={FuncionObjetivo}";
        }
    }
}
