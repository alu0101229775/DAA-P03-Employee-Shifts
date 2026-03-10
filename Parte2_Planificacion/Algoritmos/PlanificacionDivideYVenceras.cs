using System;
using System.Collections.Generic;
using System.Linq;
using DAA_P03.Parte1_Ordenamiento.Base;
using DAA_P03.Parte2_Planificacion.Modelo;

namespace DAA_P03.Parte2_Planificacion.Algoritmos
{
    /// <summary>
    /// Algoritmo de Divide y Vencerás para planificación de empleados.
    /// 
    /// - Caso base: 1 día (resuelto con algoritmo voraz OPTIMIZADO que respeta descansos)
    /// - División: Divide los días en dos mitades
    /// - Conquista: Resuelve recursivamente cada mitad
    /// - Combinación: Mezcla las soluciones + BÚSQUEDA LOCAL (2-opt) para optimizar
    /// 
    /// OPTIMIZACIONES APLICADAS:
    /// 1. Voraz mejorado: Considera restricciones de días de descanso del empleado
    /// 2. Post-procesamiento: Búsqueda local 2-opt después de combinar particiones
    /// </summary>
    public class PlanificacionDivideYVenceras : AlgoritmoDyV
    {
        // Variable para almacenar la instancia durante resolución (necesaria para búsqueda local)
        private InstanciaPlanificacion _instanciaActual;
        public PlanificacionDivideYVenceras()
        {
            Nombre = "Planificación Empleados D&C";
        }

        /// <summary>
        /// Determina si la instancia es pequeña (1 día).
        /// </summary>
        protected override bool EsPequenio(Instancia instancia)
        {
            var inst = instancia as InstanciaPlanificacion ??
                throw new ArgumentException("Debe ser InstanciaPlanificacion.");
            return inst.NumDias <= 1;
        }

        /// <summary>
        /// Resuelve el caso base (1 día) con algoritmo voraz mejorado.
        /// Para cada turno, asigna empleados considerando satisfacción Y respetando días de descanso.
        /// OPTIMIZACIÓN: Válida restricciones de descanso para aumentar validez de soluciones.
        /// </summary>
        protected override Solucion ResolverPequenio(Instancia instancia)
        {
            var inst = instancia as InstanciaPlanificacion ??
                throw new ArgumentException("Debe ser InstanciaPlanificacion.");

            NumOperaciones++;
            var sol = new SolucionPlanificacion(inst.NumDias, inst.NumTurnos, inst.NumEmpleados);
            var diasDescansoEmpleados = inst.ObtenerDiasDescansoEmpleados();

            double satisfaccionTotal = 0;
            int turnosCubiertos = 0;

            // Voraz mejorado: considera satisfacción Y restricciones de descanso
            for (int d = 0; d < inst.NumDias; d++)
            {
                var empleadosQueTrabajanHoy = new HashSet<int>();

                for (int t = 0; t < inst.NumTurnos; t++)
                {
                    int coberturaMínima = inst.CoberturaMínima[d, t];

                    // Obtener empleados válidos: no asignados hoy y con días de descanso disponibles
                    var empleadosValidos = new List<(int id, double sat, int diasDescansoRestantes)>();
                    for (int e = 0; e < inst.NumEmpleados; e++)
                    {
                        if (empleadosQueTrabajanHoy.Contains(e)) continue;
                        int diasDescansoRestantes = diasDescansoEmpleados[e];
                        if (diasDescansoRestantes > 0 || diasDescansoEmpleados[e] >= inst.Empleados[e].DiasDescanso)
                        {
                            empleadosValidos.Add((e, inst.Satisfaccion[e, d, t], diasDescansoRestantes));
                        }
                    }

                    // Ordenar: primero por satisfacción, luego por días de descanso pendientes
                    empleadosValidos = empleadosValidos
                        .OrderByDescending(x => x.sat)
                        .ThenByDescending(x => x.diasDescansoRestantes)
                        .ToList();

                    // Asignar los mejores empleados hasta cubrir el mínimo
                    int asignados = 0;
                    for (int i = 0; i < empleadosValidos.Count && asignados < coberturaMínima; i++)
                    {
                        int empId = empleadosValidos[i].id;
                        sol.AsignarEmpleado(d, t, empId);
                        satisfaccionTotal += empleadosValidos[i].sat;
                        diasDescansoEmpleados[empId]--;
                        empleadosQueTrabajanHoy.Add(empId);
                        asignados++;
                    }

                    if (asignados >= coberturaMínima)
                        turnosCubiertos++;
                }
            }

            sol.SatisfaccionTotal = satisfaccionTotal;
            sol.TurnosCubiertos = turnosCubiertos;
            sol.FuncionObjetivo = satisfaccionTotal + (turnosCubiertos * 100.0);

            return sol;
        }

        /// <summary>
        /// Divide la instancia en dos mitades de días.
        /// </summary>
        protected override Instancia[] Dividir(Instancia instancia)
        {
            var inst = instancia as InstanciaPlanificacion ??
                throw new ArgumentException("Debe ser InstanciaPlanificacion.");

            NumOperaciones++;

            int mitad = inst.NumDias / 2;
            var izq = inst.ObtenerSubinstancia(0, mitad - 1);
            var der = inst.ObtenerSubinstancia(mitad, inst.NumDias - 1);

            return new Instancia[] { izq, der };
        }

        /// <summary>
        /// Combina n soluciones en una solución completa.
        /// OPTIMIZACIÓN: Aplica búsqueda local (2-opt) post-procesamiento para mejorar la solución combinada.
        /// </summary>
        protected override Solucion Combinar(Solucion[] soluciones)
        {
            if (soluciones == null || soluciones.Length != 2)
                throw new ArgumentException("Se esperan exactamente 2 soluciones.");
            
            var sol1 = soluciones[0] as SolucionPlanificacion ??
                throw new ArgumentException("Solución 1 debe ser SolucionPlanificacion.");
            var sol2 = soluciones[1] as SolucionPlanificacion ??
                throw new ArgumentException("Solución 2 debe ser SolucionPlanificacion.");

            NumOperaciones++;

            var solucionCombinada = sol1.Combinar(sol2) as SolucionPlanificacion;
            
            // OPTIMIZACIÓN: Aplicar búsqueda local para mejorar la solución
            if (solucionCombinada != null)
            {
                AplicarBúsquedaLocal(solucionCombinada);
            }

            return solucionCombinada;
        }

        /// <summary>
        /// Aplica búsqueda local (vecindario 2-opt) para mejorar una solución.
        /// Intenta intercambiar asignaciones entre empleados para aumentar satisfacción.
        /// OPTIMIZACIÓN: Mejora iterativa sin cambiar estructura D&C.
        /// </summary>
        private void AplicarBúsquedaLocal(SolucionPlanificacion sol)
        {
            if (sol == null) return;

            bool mejora = true;
            int iteraciones = 0;
            int maxIteraciones = sol.NumDias * sol.NumTurnos; // Límite para no ser demasiado costoso

            while (mejora && iteraciones < maxIteraciones)
            {
                mejora = false;
                iteraciones++;

                // Intentar intercambiar asignaciones entre dos posiciones
                for (int d1 = 0; d1 < sol.NumDias && !mejora; d1++)
                {
                    for (int t1 = 0; t1 < sol.NumTurnos && !mejora; t1++)
                    {
                        for (int i = 0; i < sol.Plan[d1][t1].Count && !mejora; i++)
                        {
                            int emp1 = sol.Plan[d1][t1][i];

                            // Comparar con otro turno
                            for (int d2 = 0; d2 < sol.NumDias && !mejora; d2++)
                            {
                                for (int t2 = 0; t2 < sol.NumTurnos && !mejora; t2++)
                                {
                                    if (d1 == d2 && t1 == t2) continue;

                                    for (int j = 0; j < sol.Plan[d2][t2].Count && !mejora; j++)
                                    {
                                        int emp2 = sol.Plan[d2][t2][j];
                                        if (emp1 == emp2) continue;

                                        // Para días distintos: verificar que el swap no crea
                                        // multi-turno en el mismo día
                                        if (d1 != d2)
                                        {
                                            bool emp2EnDia1 = false;
                                            for (int tt = 0; tt < sol.NumTurnos && !emp2EnDia1; tt++)
                                                if (sol.Plan[d1][tt].Contains(emp2)) emp2EnDia1 = true;
                                            if (emp2EnDia1) continue;

                                            bool emp1EnDia2 = false;
                                            for (int tt = 0; tt < sol.NumTurnos && !emp1EnDia2; tt++)
                                                if (sol.Plan[d2][tt].Contains(emp1)) emp1EnDia2 = true;
                                            if (emp1EnDia2) continue;
                                        }

                                        // Calcular ganancia de intercambiar emp1 y emp2
                                        double mejora_potencial = CalcularMejoraIntercambio(
                                            sol, emp1, emp2, d1, t1, d2, t2);

                                        if (mejora_potencial > 0.001) // Umbral mínimo
                                        {
                                            // Realizar intercambio
                                            sol.Plan[d1][t1][i] = emp2;
                                            sol.Plan[d2][t2][j] = emp1;
                                            mejora = true;
                                            NumOperaciones++;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Calcula la mejora potencial de intercambiar dos empleados en posiciones diferentes.
        /// Compara ganancias de satisfacción antes y después del intercambio.
        /// </summary>
        private double CalcularMejoraIntercambio(SolucionPlanificacion sol, int emp1, int emp2,
            int d1, int t1, int d2, int t2)
        {
            if (_instanciaActual == null) return 0;

            // Satisfacción actual:
            double satAntes = _instanciaActual.Satisfaccion[emp1, d1, t1] +
                              _instanciaActual.Satisfaccion[emp2, d2, t2];

            // Satisfacción después de intercambiar:
            double satDespues = _instanciaActual.Satisfaccion[emp2, d1, t1] +
                                _instanciaActual.Satisfaccion[emp1, d2, t2];

            return satDespues - satAntes;
        }

        /// <summary>
        /// Resuelve una instancia de planificación.
        /// OPTIMIZACIÓN: Almacena la instancia para que búsqueda local pueda acceder a matrices.
        /// </summary>
        public override Solucion Resolver(Instancia instancia)
        {
            NumOperaciones = 0;
            var inst = instancia as InstanciaPlanificacion;

            if (inst == null)
                throw new ArgumentException("Debe ser InstanciaPlanificacion.");
            if (!inst.EsValida())
                throw new InvalidOperationException("Instancia inválida.");

            _instanciaActual = inst; // Guardar para búsqueda local
            return ResolverDivideYVenceras(instancia);
        }

        protected override string GetNumberOfSubproblems()
        {
            return "2";
        }

        protected override string GetSubproblemSizeDivisor()
        {
            return "n/2";
        }

        protected override string GetCombineCost()
        {
            return "n";
        }
    }
}
