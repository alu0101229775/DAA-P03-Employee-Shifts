using System;
using DAA_P03.Nucleos.Datos;

namespace DAA_P03.Nucleos.Algoritmos.Ordenamiento
{
    /// <summary>
    /// Implementación del algoritmo MergeSort utilizando la plantilla Divide y Vencerás.
    /// MergeSort es un algoritmo de ordenamiento estable bastante eficiente con complejidad O(n log n).
    /// Implementa todos los métodos abstractos definidos en DivideYVenceras.
    /// </summary>
    public class MergeSort : DivideYVenceras
    {
        /// <summary>
        /// Umbral para cambiar a ordenamiento por región pequeña.
        /// </summary>
        private const int UMBRAL_PEQUENIO = 1;

        /// <summary>
        /// Número de comparaciones realizadas.
        /// </summary>
        private long _comparaciones;

        /// <summary>
        /// Número de intercambios (en MergeSort es el movimiento de elementos).
        /// </summary>
        private long _movimientos;

        /// <summary>
        /// Constructor que inicializa el algoritmo MergeSort.
        /// </summary>
        public MergeSort()
        {
            Nombre = "MergeSort";
            Descripcion = "Algoritmo de ordenamiento basado en Divide y Vencerás. " +
                         "Divide el array en mitades, ordena recursivamente cada mitad y luego mezcla.";
            _comparaciones = 0;
            _movimientos = 0;
        }

        /// <summary>
        /// Determina si el array es lo suficientemente pequeño para ser ordenado directamente.
        /// En MergeSort, es pequeño cuando tiene 1 o 0 elementos.
        /// </summary>
        /// <param name="instancia">La instancia a evaluar (InstanceSorting).</param>
        /// <returns>true si el tamaño es <= UMBRAL_PEQUENIO.</returns>
        protected override bool EsPequenio(object instancia)
        {
            InstanceSorting instance = instancia as InstanceSorting ??
                throw new ArgumentException("La instancia debe ser de tipo InstanceSorting.");
            return instance.Tamaño <= UMBRAL_PEQUENIO;
        }

        /// <summary>
        /// Resuelve el problema cuando el array es pequeño.
        /// Para un array de 0 o 1 elementos, ya está "ordenado".
        /// </summary>
        /// <param name="instancia">El array pequeño (InstanceSorting).</param>
        /// <returns>Una solución con el array ya ordenado.</returns>
        protected override object ResolverPequenio(object instancia)
        {
            InstanceSorting instance = instancia as InstanceSorting ??
                throw new ArgumentException("La instancia debe ser de tipo InstanceSorting.");

            NumOperaciones++;
            return new SolutionSorting(instance.Numeros, _comparaciones, _movimientos);
        }

        /// <summary>
        /// Divide el array en dos mitades.
        /// </summary>
        /// <param name="instancia">El array a dividir (InstanceSorting).</param>
        /// <returns>Array con dos SubInstances (mitad izquierda y derecha).</returns>
        protected override object[] Dividir(object instancia)
        {
            InstanceSorting instance = instancia as InstanceSorting ??
                throw new ArgumentException("La instancia debe ser de tipo InstanceSorting.");

            int mitad = instance.Tamaño / 2;
            int[] izquierda = new int[mitad];
            int[] derecha = new int[instance.Tamaño - mitad];

            Array.Copy(instance.Numeros, 0, izquierda, 0, mitad);
            Array.Copy(instance.Numeros, mitad, derecha, 0, instance.Tamaño - mitad);

            NumOperaciones++;

            return new object[]
            {
                new InstanceSorting(izquierda),
                new InstanceSorting(derecha)
            };
        }

        /// <summary>
        /// Combina dos soluciones ordenadas en una única solución ordenada.
        /// Realiza el proceso de mezcla típico de MergeSort.
        /// </summary>
        /// <param name="solucion1">Primera mitad ordenada (SolutionSorting).</param>
        /// <param name="solucion2">Segunda mitad ordenada (SolutionSorting).</param>
        /// <returns>Una nueva solución con los elementos combinados y ordenados.</returns>
        protected override object Combinar(object solucion1, object solucion2)
        {
            SolutionSorting sol1 = solucion1 as SolutionSorting ??
                throw new ArgumentException("Solución 1 debe ser de tipo SolutionSorting.");
            SolutionSorting sol2 = solucion2 as SolutionSorting ??
                throw new ArgumentException("Solución 2 debe ser de tipo SolutionSorting.");

            int[] arr1 = sol1.NumerosOrdenados;
            int[] arr2 = sol2.NumerosOrdenados;
            int[] resultado = new int[arr1.Length + arr2.Length];

            int i = 0, j = 0, k = 0;

            // Mezcla de dos arrays ordenados
            while (i < arr1.Length && j < arr2.Length)
            {
                _comparaciones++;
                if (arr1[i] <= arr2[j])
                {
                    resultado[k++] = arr1[i++];
                }
                else
                {
                    resultado[k++] = arr2[j++];
                }
                _movimientos++;
            }

            // Copia los elementos restantes
            while (i < arr1.Length)
            {
                resultado[k++] = arr1[i++];
                _movimientos++;
            }

            while (j < arr2.Length)
            {
                resultado[k++] = arr2[j++];
                _movimientos++;
            }

            NumOperaciones++;

            return new SolutionSorting(resultado, _comparaciones, _movimientos);
        }

        /// <summary>
        /// Resuelve una instancia usando MergeSort.
        /// Sobrescribe el método base para reinicializar contadores.
        /// </summary>
        /// <param name="instancia">La instancia a ordenar.</param>
        /// <returns>La solución ordenada.</returns>
        public override object Resolver(object instancia)
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
                   $"Movimientos: {_movimientos}";
        }
    }
}
