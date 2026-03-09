using System;
using System.Linq;

namespace DAA_P03.Nucleos.Datos
{
    /// <summary>
    /// Representa una instancia de un problema de ordenamiento.
    /// Contiene un array de números enteros a ser ordenados.
    /// </summary>
    public class InstanceSorting
    {
        /// <summary>
        /// El array de números a ordenar.
        /// </summary>
        public int[] Numeros { get; set; }

        /// <summary>
        /// Tamaño del array.
        /// </summary>
        public int Tamaño => Numeros?.Length ?? 0;

        /// <summary>
        /// Constructor que inicializa la instancia con un array específico.
        /// </summary>
        /// <param name="numeros">Array de números a ordenar.</param>
        public InstanceSorting(int[] numeros)
        {
            if (numeros == null)
                throw new ArgumentNullException(nameof(numeros), "El array no puede ser nulo.");
            Numeros = (int[])numeros.Clone();
        }

        /// <summary>
        /// Constructor que genera una instancia con números aleatorios.
        /// </summary>
        /// <param name="tamaño">Número de elementos a generar.</param>
        /// <param name="seed">Semilla para la generación aleatoria (opcional).</param>
        public InstanceSorting(int tamaño, int seed = 0)
        {
            if (tamaño <= 0)
                throw new ArgumentException("El tamaño debe ser positivo.", nameof(tamaño));

            Random random = seed == 0 ? new Random() : new Random(seed);
            Numeros = new int[tamaño];
            for (int i = 0; i < tamaño; i++)
            {
                Numeros[i] = random.Next(1000000);
            }
        }

        /// <summary>
        /// Obtiene una copia de la instancia.
        /// </summary>
        /// <returns>Una nueva instancia con los mismos datos.</returns>
        public InstanceSorting ObtenerCopia()
        {
            return new InstanceSorting((int[])Numeros.Clone());
        }

        /// <summary>
        /// Verifica si el array está ordenado.
        /// </summary>
        /// <returns>true si está ordenado, false en caso contrario.</returns>
        public bool EstaOrdenado()
        {
            for (int i = 0; i < Numeros.Length - 1; i++)
            {
                if (Numeros[i] > Numeros[i + 1])
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Obtiene una representación en string del array.
        /// Muestra los primeros y últimos elementos si es muy grande.
        /// </summary>
        /// <returns>Representación formateada del array.</returns>
        public override string ToString()
        {
            if (Tamaño <= 20)
            {
                return $"[{string.Join(", ", Numeros)}]";
            }
            else
            {
                string primeros = string.Join(", ", Numeros.Take(5));
                string ultimos = string.Join(", ", Numeros.TakeLast(5));
                return $"[{primeros}, ... ({Tamaño - 10} elementos) ..., {ultimos}]";
            }
        }
    }
}
