using System;
using DAA_P03.Parte1_Ordenamiento.Base;
using DAA_P03.Parte1_Ordenamiento.Modelo;

namespace DAA_P03.Parte1_Ordenamiento.Algoritmos
{
    /// <summary>
    /// Implementación del algoritmo QuickSort utilizando la plantilla Divide y Vencerás.
    /// Usa particionamiento in-place para máxima eficiencia.
    /// Los subproblemas comparten el mismo array pero trabajan en rangos diferentes.
    /// </summary>
    public class QuickSort : AlgoritmoDyV
    {
        private long _comparaciones;
        private long _intercambios;

        /// <summary>
        /// Constructor que inicializa el algoritmo QuickSort.
        /// </summary>
        public QuickSort()
        {
            Nombre = "QuickSort";
            _comparaciones = 0;
            _intercambios = 0;
        }

        /// <summary>
        /// Determina si el rango es lo suficientemente pequeño.
        /// En QuickSort, es pequeño cuando start >= final (0 o 1 elementos).
        /// </summary>
        /// <param name="instancia">La instancia a evaluar (InstanciaQuickSort).</param>
        /// <returns>true si el rango tiene 0 o 1 elementos.</returns>
        protected override bool EsPequenio(Instancia instancia)
        {
            InstanciaQuickSort quickSortProblem = (InstanciaQuickSort)instancia;
            return quickSortProblem.Start >= quickSortProblem.Final;
        }

        /// <summary>
        /// Resuelve el problema cuando el rango es pequeño.
        /// Para un rango de 0 o 1 elementos, ya está ordenado.
        /// </summary>
        /// <param name="instancia">El problema pequeño (InstanciaQuickSort).</param>
        /// <returns>Una solución con el vector ya ordenado en ese rango.</returns>
        protected override Solucion ResolverPequenio(Instancia instancia)
        {
            InstanciaQuickSort quickSortProblem = (InstanciaQuickSort)instancia;
            NumOperaciones++;
            return new SolucionQuickSort(
                quickSortProblem.Start,
                quickSortProblem.Final,
                quickSortProblem.Vector
            );
        }

        /// <summary>
        /// Divide el array usando particionamiento in-place.
        /// Reorganiza el array de forma que elementos menores al pivote quedan a la izquierda
        /// y los mayores a la derecha. Ambos subproblemas comparten el mismo vector.
        /// </summary>
        /// <param name="instancia">El problema a particionar (InstanciaQuickSort).</param>
        /// <returns>Array con dos subproblemas que trabajan en rangos disjuntos.</returns>
        protected override Instancia[] Dividir(Instancia instancia)
        {
            InstanciaQuickSort quickSortProblem = (InstanciaQuickSort)instancia;
            int[] vector = quickSortProblem.Vector;
            int start = quickSortProblem.Start;
            int final = quickSortProblem.Final;
            int pivot = quickSortProblem.Pivot;

            int i = start;
            int f = final;
            int p = vector[pivot];

            while (i <= f)
            {
                while (vector[i] < p)
                {
                    i++;
                    _comparaciones++;
                }
                while (vector[f] > p)
                {
                    f--;
                    _comparaciones++;
                }
                _comparaciones += 2;

                if (i <= f)
                {
                    int temp = vector[i];
                    vector[i] = vector[f];
                    vector[f] = temp;
                    _intercambios++;
                    
                    i++;
                    f--;
                }
            }

            NumOperaciones++;

            InstanciaQuickSort leftProblem = new InstanciaQuickSort(start, f, (start + f) / 2, vector);
            InstanciaQuickSort rightProblem = new InstanciaQuickSort(i, final, (i + final) / 2, vector);

            return new Instancia[] { leftProblem, rightProblem };
        }

        /// <summary>
        /// Combina dos soluciones.
        /// En QuickSort con particionamiento in-place, los subarrays ya están ordenados
        /// en sus posiciones finales, por lo que no se necesita combinar.
        /// </summary>
        /// <param name="solucion1">Primera solución parcial.</param>
        /// <param name="solucion2">Segunda solución parcial.</param>
        /// <returns>La solución (el vector ya está completamente ordenado).</returns>
        protected override Solucion Combinar(Solucion solucion1, Solucion solucion2)
        {
            SolucionQuickSort sol1 = (SolucionQuickSort)solucion1;
            SolucionQuickSort sol2 = (SolucionQuickSort)solucion2;

            NumOperaciones++;
            
            return new SolucionQuickSort(
                sol1.Start,
                sol2.Final,
                sol1.Vector
            );
        }

        /// <summary>
        /// Resuelve una instancia usando QuickSort.
        /// Sobrescribe el método base para reinicializar contadores.
        /// </summary>
        /// <param name="instancia">La instancia a ordenar.</param>
        /// <returns>La solución ordenada.</returns>
        public override Solucion Resolver(Instancia instancia)
        {
            _comparaciones = 0;
            _intercambios = 0;
            NumOperaciones = 0;
            return base.Resolver(instancia);
        }

        /// <summary>
        /// Obtiene información detallada del algoritmo QuickSort.
        /// </summary>
        /// <returns>Información formateada.</returns>
        public override string ObtenerInfo()
        {
            return base.ObtenerInfo() + 
                   $"\nComparaciones: {_comparaciones}\n" +
                   $"Intercambios: {_intercambios}";
        }
    }
}
