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
    /// - Caso base: 1 día (resuelto con algoritmo voraz)
    /// - División: Divide los días en dos mitades
    /// - Conquista: Resuelve recursivamente cada mitad
    /// - Combinación: Mezcla las soluciones
    /// </summary>
    public class PlanificacionDivideYVenceras : AlgoritmoDyV
    {
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
        /// Resuelve el caso base (1 día) con algoritmo voraz.
        /// Para cada turno, asigna los empleados más satisfechos hasta cubrir el mínimo.
        /// </summary>
        protected override Solucion ResolverPequenio(Instancia instancia)
        {
            var inst = instancia as InstanciaPlanificacion ??
                throw new ArgumentException("Debe ser InstanciaPlanificacion.");

            NumOperaciones++;
            var sol = new SolucionPlanificacion(inst.NumDias, inst.NumTurnos, inst.NumEmpleados);

            double satisfaccionTotal = 0;
            int turnosCubiertos = 0;

            // Voraz: para cada turno, asignar empleados más satisfechos
            for (int d = 0; d < inst.NumDias; d++)
            {
                for (int t = 0; t < inst.NumTurnos; t++)
                {
                    int coberturaMínima = inst.CoberturaMínima[d, t];

                    // Obtener empleados ordenados por satisfacción descendente
                    var empleados = new List<(int id, double sat)>();
                    for (int e = 0; e < inst.NumEmpleados; e++)
                    {
                        empleados.Add((e, inst.Satisfaccion[d, e, t]));
                    }
                    empleados = empleados.OrderByDescending(x => x.sat).ToList();

                    // Asignar los mejores empleados hasta cubrir el mínimo
                    int asignados = 0;
                    for (int i = 0; i < empleados.Count && asignados < coberturaMínima; i++)
                    {
                        sol.AsignarEmpleado(d, t, empleados[i].id);
                        satisfaccionTotal += empleados[i].sat;
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

            return sol1.Combinar(sol2);
        }

        /// <summary>
        /// Resuelve una instancia de planificación.
        /// </summary>
        public override Solucion Resolver(Instancia instancia)
        {
            NumOperaciones = 0;
            var inst = instancia as InstanciaPlanificacion;

            if (inst == null)
                throw new ArgumentException("Debe ser InstanciaPlanificacion.");
            if (!inst.EsValida())
                throw new InvalidOperationException("Instancia inválida.");

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
