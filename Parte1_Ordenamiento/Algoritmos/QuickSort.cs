using System;
using DAA_P03.Parte1_Ordenamiento.Base;
using DAA_P03.Parte1_Ordenamiento.Modelo;

namespace DAA_P03.Parte1_Ordenamiento.Algoritmos
{
    /// <summary>
    /// Implementación del algoritmo QuickSort utilizando la plantilla Divide y Vencerás.
    /// QuickSort es un algoritmo de ordenamiento rápido basado en la partición del array.
    /// Implementa todos los métodos abstractos definidos en AlgoritmoDyV.
    /// </summary>
    public class QuickSort : AlgoritmoDyV
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
        /// <param name="instancia">La instancia a evaluar (InstanciaOrdenamiento).</param>
        /// <returns>true si el tamaño es <= UMBRAL_PEQUENIO.</returns>
        protected override bool EsPequenio(object instancia)
        {
            InstanciaOrdenamiento instance = instancia as InstanciaOrdenamiento ??
                throw new ArgumentException("La instancia debe ser de tipo InstanciaOrdenamiento.");
            return instance.Tamaño <= UMBRAL_PEQUENIO;
        }

        /// <summary>
        /// Resuelve el problema cuando el array es pequeño.
        /// Para un array de 0 o 1 elementos, ya está "ordenado".
        /// </summary>
        /// <param name="instancia">El array pequeño (InstanciaOrdenamiento).</param>
        /// <returns>Una solución con el array ya ordenado.</returns>
        protected override object ResolverPequenio(object instancia)
        {
            InstanciaOrdenamiento instance = instancia as InstanciaOrdenamiento ??
                throw new ArgumentException("La instancia debe ser de tipo InstanciaOrdenamiento.");

            NumOperaciones++;
            return new SolucionOrdenamiento(instance.Numeros, _comparaciones, _intercambios);
        }

        /// <summary>
        /// Divide el array usando la técnica de partición in-place de QuickSort (Algoritmo de Lomuto).
        /// Reorganiza el array de forma que los elementos menores al pivote queden a la izquierda
        /// y los mayores a la derecha, usando intercambios (swaps).
        /// </summary>
        /// <param name="instancia">El array a particionar (InstanciaOrdenamiento).</param>
        /// <returns>Array con dos instancias (partición izquierda e índice de partición).</returns>
        protected override object[] Dividir(object instancia)
        {
            InstanciaOrdenamiento instance = instancia as InstanciaOrdenamiento ??
                throw new ArgumentException("La instancia debe ser de tipo InstanciaOrdenamiento.");

            int[] numeros = (int[])instance.Numeros.Clone();
            
            // Particionamiento in-place usando el algoritmo de Lomuto
            int pivotIndex = ParticionarLomuto(numeros, 0, numeros.Length - 1);
            
            NumOperaciones++;

            // Crear dos subproblemas: izquierda (0 a pivotIndex-1) y derecha (pivotIndex+1 a fin)
            int[] izquierda = new int[pivotIndex];
            int[] derecha = new int[numeros.Length - pivotIndex - 1];
            
            Array.Copy(numeros, 0, izquierda, 0, pivotIndex);
            Array.Copy(numeros, pivotIndex + 1, derecha, 0, numeros.Length - pivotIndex - 1);

            return new object[]
            {
                new InstanciaOrdenamiento(izquierda),
                new InstanciaOrdenamiento(derecha)
            };
        }

        /// <summary>
        /// Algoritmo de partición in-place de Lomuto.
        /// Reorganiza el array de forma que todos los elementos menores al pivote
        /// quedan a la izquierda y los mayores a la derecha.
        /// </summary>
        /// <param name="arr">Array a particionar.</param>
        /// <param name="bajo">Índice inferior.</param>
        /// <param name="alto">Índice superior (elemento pivote).</param>
        /// <returns>Índice final del pivote después de la partición.</returns>
        private int ParticionarLomuto(int[] arr, int bajo, int alto)
        {
            int pivote = arr[alto];
            int i = bajo - 1;

            for (int j = bajo; j < alto; j++)
            {
                _comparaciones++;
                if (arr[j] < pivote)
                {
                    i++;
                    // Intercambiar arr[i] y arr[j]
                    Intercambiar(arr, i, j);
                    _intercambios++;
                }
            }
            
            // Intercambiar arr[i+1] y arr[alto] (pivote)
            Intercambiar(arr, i + 1, alto);
            _intercambios++;
            
            return i + 1;
        }

        /// <summary>
        /// Intercambia dos elementos en el array.
        /// </summary>
        private void Intercambiar(int[] arr, int i, int j)
        {
            int temp = arr[i];
            arr[i] = arr[j];
            arr[j] = temp;
        }

        /// <summary>
        /// Combina dos soluciones ordenadas.
        /// En QuickSort, simplemente concatenamos las particiones ordenadas.
        /// </summary>
        /// <param name="solucion1">Primera partición ordenada (SolucionOrdenamiento).</param>
        /// <param name="solucion2">Segunda partición ordenada (SolucionOrdenamiento).</param>
        /// <returns>Una nueva solución con los elementos combinados y ordenados.</returns>
        protected override object Combinar(object solucion1, object solucion2)
        {
            SolucionOrdenamiento sol1 = solucion1 as SolucionOrdenamiento ??
                throw new ArgumentException("Solución 1 debe ser de tipo SolucionOrdenamiento.");
            SolucionOrdenamiento sol2 = solucion2 as SolucionOrdenamiento ??
                throw new ArgumentException("Solución 2 debe ser de tipo SolucionOrdenamiento.");

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

            return new SolucionOrdenamiento(resultado, _comparaciones, _intercambios);
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
