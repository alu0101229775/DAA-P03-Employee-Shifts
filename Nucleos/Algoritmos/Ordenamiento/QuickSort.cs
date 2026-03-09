using System;
using DAA_P03.Nucleos.Datos;

namespace DAA_P03.Nucleos.Algoritmos.Ordenamiento
{
    /// <summary>
    /// Implementación del algoritmo QuickSort utilizando la plantilla Divide y Vencerás.
    /// QuickSort es un algoritmo de ordenamiento rápido basado en la partición del array.
    /// Implementa todos los métodos abstractos definidos en DivideYVenceras.
    /// </summary>
    public class QuickSort : DivideYVenceras
    {
        /// <summary>
        /// Umbral para cambiar a ordenamiento por inserción en casos pequeños.
        /// </summary>
        private const int UMBRAL_PEQUENIO = 1;

        /// <summary>
        /// Número de comparaciones realizadas.
        /// </summary>
        private long _comparaciones;

        /// <summary>
        /// Número de intercambios realizados.
        /// </summary>
        private long _intercambios;

        /// <summary>
        /// Constructor que inicializa el algoritmo QuickSort.
        /// </summary>
        public QuickSort()
        {
            Nombre = "QuickSort";
            Descripcion = "Algoritmo de ordenamiento basado en Divide y Vencerás. " +
                         "Selecciona un pivote, particiona el array y ordena recursivamente cada parte.";
            _comparaciones = 0;
            _intercambios = 0;
        }

        /// <summary>
        /// Determina si el array es lo suficientemente pequeño para ser ordenado directamente.
        /// En QuickSort, es pequeño cuando tiene 1 o 0 elementos.
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
            return new SolutionSorting(instance.Numeros, _comparaciones, _intercambios);
        }

        /// <summary>
        /// Divide el array usando la técnica de partición de QuickSort.
        /// Selecciona el último elemento como pivote y particiona el array alrededor de él.
        /// </summary>
        /// <param name="instancia">El array a particionar (InstanceSorting).</param>
        /// <returns>Array con dos instancias (partición izquierda y derecha).</returns>
        protected override object[] Dividir(object instancia)
        {
            InstanceSorting instance = instancia as InstanceSorting ??
                throw new ArgumentException("La instancia debe ser de tipo InstanceSorting.");

            int[] numeros = (int[])instance.Numeros.Clone();
            int[] copia = (int[])numeros.Clone();

            // Usar el último elemento como pivote (estrategia simple)
            int pivote = copia[copia.Length - 1];
            int[] izquierda = new int[0];
            int[] derecha = new int[0];
            int[] igual = new int[0];

            // Particionar en tres partes: menor, igual, mayor
            System.Collections.Generic.List<int> menores = new System.Collections.Generic.List<int>();
            System.Collections.Generic.List<int> iguales = new System.Collections.Generic.List<int>();
            System.Collections.Generic.List<int> mayores = new System.Collections.Generic.List<int>();

            for (int i = 0; i < copia.Length; i++)
            {
                _comparaciones++;
                if (copia[i] < pivote)
                    menores.Add(copia[i]);
                else if (copia[i] == pivote)
                    iguales.Add(copia[i]);
                else
                    mayores.Add(copia[i]);
            }

            // Crear dos subproblemas: menores y (iguales + mayores)
            izquierda = menores.ToArray();
            derecha = new int[iguales.Count + mayores.Count];
            int idx = 0;
            foreach (int val in iguales) derecha[idx++] = val;
            foreach (int val in mayores) derecha[idx++] = val;

            NumOperaciones++;

            return new object[]
            {
                new InstanceSorting(izquierda),
                new InstanceSorting(derecha)
            };
        }

        /// <summary>
        /// Combina dos soluciones ordenadas.
        /// En QuickSort, simplemente concatenamos las particiones ordenadas.
        /// </summary>
        /// <param name="solucion1">Primera partición ordenada (SolutionSorting).</param>
        /// <param name="solucion2">Segunda partición ordenada (SolutionSorting).</param>
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

            // Concatenar: izquierda ordenada + derecha ordenada
            Array.Copy(arr1, resultado, arr1.Length);
            Array.Copy(arr2, 0, resultado, arr1.Length, arr2.Length);

            for (int i = 0; i < arr2.Length; i++)
            {
                _intercambios++;
            }

            NumOperaciones++;

            return new SolutionSorting(resultado, _comparaciones, _intercambios);
        }

        /// <summary>
        /// Resuelve una instancia usando QuickSort.
        /// Sobrescribe el método base para reinicializar contadores.
        /// </summary>
        /// <param name="instancia">La instancia a ordenar.</param>
        /// <returns>La solución ordenada.</returns>
        public override object Resolver(object instancia)
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
