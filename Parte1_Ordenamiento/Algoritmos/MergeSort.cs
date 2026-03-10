using System;
using DAA_P03.Parte1_Ordenamiento.Base;
using DAA_P03.Parte1_Ordenamiento.Modelo;

namespace DAA_P03.Parte1_Ordenamiento.Algoritmos
{
    /// <summary>
    /// Implementación del algoritmo MergeSort utilizando la plantilla Divide y Vencerás.
    /// Usa un vector compartido con rangos (start-final) para máxima eficiencia en memoria.
    /// MergeSort es un algoritmo de ordenamiento estable con complejidad O(n log n).
    /// </summary>
    public class MergeSort : AlgoritmoDyV
    {
        private long _comparaciones;
        private long _movimientos;

        /// <summary>
        /// Constructor que inicializa el algoritmo MergeSort.
        /// </summary>
        public MergeSort()
        {
            Nombre = "MergeSort";
            _comparaciones = 0;
            _movimientos = 0;
        }

        /// <summary>
        /// Determina si el rango es lo suficientemente pequeño.
        /// En MergeSort, es pequeño cuando start == final (exactamente 1 elemento).
        /// </summary>
        /// <param name="instancia">La instancia a evaluar (InstanciaMergeSort).</param>
        /// <returns>true si el rango tiene exactamente 1 elemento.</returns>
        protected override bool EsPequenio(Instancia instancia)
        {
            InstanciaMergeSort mergeSortProblem = (InstanciaMergeSort)instancia;
            return mergeSortProblem.Start == mergeSortProblem.Final;
        }

        /// <summary>
        /// Resuelve el problema cuando el rango es pequeño (1 elemento).
        /// Un solo elemento ya está ordenado por definición.
        /// </summary>
        /// <param name="instancia">El problema pequeño (InstanciaMergeSort).</param>
        /// <returns>Una solución con el elemento ya ordenado.</returns>
        protected override Solucion ResolverPequenio(Instancia instancia)
        {
            InstanciaMergeSort mergeSortProblem = (InstanciaMergeSort)instancia;
            NumOperaciones++;
            
            return new SolucionMergeSort(
                mergeSortProblem.Start,
                mergeSortProblem.Final,
                mergeSortProblem.Middle,
                mergeSortProblem.Vector
            );
        }

        /// <summary>
        /// Divide el rango en dos mitades.
        /// Crea dos subproblemas sobre el MISMO vector con rangos [start, middle] y [middle+1, final].
        /// </summary>
        /// <param name="instancia">El problema a dividir (InstanciaMergeSort).</param>
        /// <returns>Array con dos subproblemas que trabajan en rangos disjuntos.</returns>
        protected override Instancia[] Dividir(Instancia instancia)
        {
            InstanciaMergeSort mergeSortProblem = (InstanciaMergeSort)instancia;
            int[] vector = mergeSortProblem.Vector;
            
            int start1 = mergeSortProblem.Start;
            int final1 = mergeSortProblem.Middle;
            int middle1 = (start1 + final1) / 2;
            InstanciaMergeSort subproblem1 = new InstanciaMergeSort(start1, final1, middle1, vector);

            int start2 = mergeSortProblem.Middle + 1;
            int final2 = mergeSortProblem.Final;
            int middle2 = (start2 + final2) / 2;
            InstanciaMergeSort subproblem2 = new InstanciaMergeSort(start2, final2, middle2, vector);

            NumOperaciones++;

            return new Instancia[] { subproblem1, subproblem2 };
        }

        /// <summary>
        /// Combina soluciones ordenadas en una única solución ordenada.
        /// Mezcla los elementos in-place sobre el vector compartido para máxima eficiencia O(n).
        /// </summary>
        /// <param name="soluciones">Array de soluciones parciales ordenadas.</param>
        /// <returns>Una solución con el rango completo ordenado en el vector compartido.</returns>
        protected override Solucion Combinar(Solucion[] soluciones)
        {
            SolucionMergeSort solution1 = (SolucionMergeSort)soluciones[0];
            SolucionMergeSort solution2 = (SolucionMergeSort)soluciones[1];

            int[] vector = solution1.Vector;
            int start1 = solution1.Start;
            int final1 = solution1.Final;
            int start2 = solution2.Start;
            int final2 = solution2.Final;

            int[] result = new int[final2 - start1 + 1];
            int i = start1, j = start2, k = 0;

            while (i <= final1 && j <= final2)
            {
                _comparaciones++;
                if (vector[i] <= vector[j])
                {
                    result[k++] = vector[i++];
                }
                else
                {
                    result[k++] = vector[j++];
                }
                _movimientos++;
            }

            while (i <= final1)
            {
                result[k++] = vector[i++];
                _movimientos++;
            }

            while (j <= final2)
            {
                result[k++] = vector[j++];
                _movimientos++;
            }

            for (int l = 0; l < result.Length; l++)
            {
                vector[start1 + l] = result[l];
            }

            NumOperaciones++;

            return new SolucionMergeSort(start1, final2, (start1 + final2) / 2, vector);
        }

        /// <summary>
        /// Resuelve una instancia usando MergeSort.
        /// Sobrescribe el método base para reinicializar contadores.
        /// </summary>
        /// <param name="instancia">La instancia a ordenar.</param>
        /// <returns>La solución ordenada.</returns>
        public override Solucion Resolver(Instancia instancia)
        {
            _comparaciones = 0;
            _movimientos = 0;
            NumOperaciones = 0;
            return base.Resolver(instancia);
        }

        /// <summary>
        /// Obtiene información detallada del algoritmo MergeSort.
        /// </summary>
        /// <returns>Información formateada.</returns>
        public override string ObtenerInfo()
        {
            return base.ObtenerInfo() + 
                   $"\nComparaciones: {_comparaciones}\n" +
                   $"Movimientos: {_movimientos}\n" +
                   $"Ecuación de recurrencia: {GetRecurrenceEquation()}\n" +
                   $"Profundidad máxima de recursión: {GetMaxRecursiveDepth()}\n" +
                   $"Total de llamadas recursivas: {GetTotalRecursiveCalls()}";
        }

        protected override string GetNumberOfSubproblems() { return "2"; }
        protected override string GetSubproblemSizeDivisor() { return "n/2"; }
        protected override string GetCombineCost() { return "n"; }
    }
}
