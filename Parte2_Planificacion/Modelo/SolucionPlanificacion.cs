using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAA_P03.Parte1_Ordenamiento.Base;

namespace DAA_P03.Parte2_Planificacion.Modelo
{
    /// <summary>
    /// Representa la solución al problema de planificación de empleados.
    /// Contiene la asignación de turnos a empleados para cada día.
    /// plan[día][turno] = lista de índices de empleados asignados al turno t del día d.
    /// </summary>
    public class SolucionPlanificacion : Solucion
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

        /// <summary>
        /// Indica si la solución es válida (todas las estructuras están inicializadas correctamente).
        /// </summary>
        public override bool EsValida
        {
            get
            {
                if (Plan == null) return false;
                if (Plan.Length != NumDias) return false;
                
                for (int d = 0; d < NumDias; d++)
                {
                    if (Plan[d] == null || Plan[d].Length != NumTurnos) return false;
                    for (int t = 0; t < NumTurnos; t++)
                    {
                        if (Plan[d][t] == null) return false;
                    }
                }
                
                return true;
            }
        }

        /// <summary>
        /// Obtiene información detallada de la solución.
        /// </summary>
        /// <returns>Información formateada de la solución.</returns>
        public override string ObtenerInfo()
        {
            var sb = new StringBuilder();
            sb.AppendLine("=== INFORMACIÓN DE LA SOLUCIÓN ===");
            sb.AppendLine($"Días planificados: {NumDias}");
            sb.AppendLine($"Turnos por día: {NumTurnos}");
            sb.AppendLine($"Empleados: {NumEmpleados}");
            sb.AppendLine($"Satisfacción Total: {SatisfaccionTotal:F2}");
            sb.AppendLine($"Turnos Cubiertos: {TurnosCubiertos}");
            sb.AppendLine($"Función Objetivo: {FuncionObjetivo:F2}");
            sb.AppendLine($"Válida: {(EsValida ? "Sí" : "No")}");
            return sb.ToString();
        }

        public SolucionPlanificacion(int numDias, int numTurnos, int numEmpleados)
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
        public string ObtenerRepresentacionTabla(InstanciaPlanificacion instancia)
        {
            var empleados = instancia.ObtenerNombresEmpleados();
            var turnos = instancia.Turnos;
            var sb = new StringBuilder();

            // ========== TABLA PRINCIPAL: EMPLEADOS Y ASIGNACIONES ==========
            sb.AppendLine();
            sb.AppendLine("Employee" + String.Format("{0," + (NumDias * 14 + 30) + "}", "").Replace(" ", " ").TrimStart());
            
            // Encabezado de días
            sb.Append("Employee".PadRight(28));
            for (int d = 0; d < NumDias; d++)
            {
                sb.Append($"Day {d} |".PadRight(14));
            }
            sb.AppendLine();

            // Filas de empleados
            for (int e = 0; e < NumEmpleados; e++)
            {
                string nombreEmpleado = (e < empleados.Count ? empleados[e] : $"E{e}") + $" ({e})";
                sb.Append(nombreEmpleado.PadRight(28));
                
                // Para cada día, mostrar asignación
                for (int d = 0; d < NumDias; d++)
                {
                    string asignacion = "";
                    
                    // Buscar qué turno tiene el empleado en este día
                    int turnoDelEmpleado = -1;
                    for (int t = 0; t < NumTurnos; t++)
                    {
                        if (Plan[d][t].Contains(e))
                        {
                            turnoDelEmpleado = t;
                            break;
                        }
                    }

                    // Armar representación del día
                    for (int t = 0; t < NumTurnos; t++)
                    {
                        if (t > 0) asignacion += " - ";
                        if (t == turnoDelEmpleado)
                            asignacion += $"T{t}";
                        else
                            asignacion += "*";
                    }

                    sb.Append(asignacion.PadRight(14));
                }
                sb.AppendLine();
            }

            // ========== INDICATORS: TURNOS NO CUBIERTOS ==========
            sb.AppendLine();
            sb.AppendLine("=========== INDICATORS");
            sb.AppendLine();
            sb.Append("Shifts not covered".PadRight(28));
            
            for (int d = 0; d < NumDias; d++)
            {
                string noCubiertos = "";
                for (int t = 0; t < NumTurnos; t++)
                {
                    if (t > 0) noCubiertos += " - ";
                    int asignados = Plan[d][t].Count;
                    int requerido = instancia.CoberturaMínima[d, t];
                    int deficit = Math.Max(0, requerido - asignados);
                    noCubiertos += deficit;
                }
                sb.Append(noCubiertos.PadRight(14));
            }
            sb.AppendLine();

            // ========== REQUIRED EMPLOYEES: COBERTURA ==========
            sb.AppendLine();
            sb.AppendLine("=========== REQUIRED EMPLOYEES");
            sb.AppendLine();
            sb.Append("Day Available/Required".PadRight(28));
            
            for (int d = 0; d < NumDias; d++)
            {
                string cobertura = "";
                for (int t = 0; t < NumTurnos; t++)
                {
                    if (t > 0) cobertura += " - ";
                    int asignados = Plan[d][t].Count;
                    int requerido = instancia.CoberturaMínima[d, t];
                    cobertura += $"{asignados}/{requerido}";
                }
                sb.Append(cobertura.PadRight(14));
            }
            sb.AppendLine();

            // ========== FUNCIÓN OBJETIVO ==========
            sb.AppendLine();
            sb.AppendLine($"Objective function value ===> {FuncionObjetivo:F1}");
            sb.AppendLine(new string('-', 64));

            // ========== INFORMACIÓN ADICIONAL ==========
            sb.AppendLine();
            sb.AppendLine("=== INFORMACIÓN DE LA SOLUCIÓN ===");
            sb.AppendLine($"Días planificados: {NumDias}");
            sb.AppendLine($"Turnos por día: {NumTurnos}");
            sb.AppendLine($"Empleados: {NumEmpleados}");
            sb.AppendLine($"Satisfacción Total: {SatisfaccionTotal:F2}");
            sb.AppendLine($"Turnos Cubiertos: {TurnosCubiertos}");
            sb.AppendLine($"Función Objetivo: {FuncionObjetivo:F2}");
            sb.AppendLine($"Válida: {(EsValida ? "Sí" : "No")}");
            
            sb.AppendLine();
            sb.AppendLine("=== DÍAS DE DESCANSO POR EMPLEADO ===");
            for (int e = 0; e < NumEmpleados; e++)
            {
                int diasDescanso = ObtenerDiasDescanso(e);
                string nombreEmpleado = e < empleados.Count ? empleados[e] : $"E{e}";
                sb.AppendLine($"  {nombreEmpleado,20}: {diasDescanso} días");
            }

            sb.AppendLine();
            sb.AppendLine(new string('=', 64));

            return sb.ToString();
        }

        /// <summary>
        /// Combina dos soluciones (de días consecutivos).
        /// </summary>
        public SolucionPlanificacion Combinar(SolucionPlanificacion otra)
        {
            if (otra == null)
                throw new ArgumentNullException(nameof(otra));

            var combinada = new SolucionPlanificacion(NumDias + otra.NumDias, NumTurnos, NumEmpleados);

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
