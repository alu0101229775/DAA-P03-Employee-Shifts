using System;
using System.Linq;

namespace DAA_P03.Core.Data
{
    /// <summary>
    /// Representa la solución a un problema de ordenamiento.
    /// Contiene el array ordenado y metadatos adicionales.
    /// </summary>
    public class SolutionSorting
    {
        /// <summary>
        /// El array ordenado.
        /// </summary>
        public int[] NumerosOrdenados { get; set; }

        /// <summary>
        /// Número de comparaciones realizadas durante el ordenamiento.
        /// </summary>
        public long NumComparaciones { get; set; }

        /// <summary>
        /// Número de intercambios realizados durante el ordenamiento.
        /// </summary>
        public long NumIntercambios { get; set; }

        /// <summary>
        /// Indica si la solución es válida (array ordenado correctamente).
        /// </summary>
        public bool EsValida
        {
            get
            {
                if (NumerosOrdenados == null || NumerosOrdenados.Length == 0)
                    return true;

                for (int i = 0; i < NumerosOrdenados.Length - 1; i++)
                {
                    if (NumerosOrdenados[i] > NumerosOrdenados[i + 1])
                        return false;
                }
                return true;
            }
        }

        /// <summary>
        /// Constructor que inicializa la solución.
        /// </summary>
        /// <param name="numerosOrdenados">Array ordenado.</param>
        /// <param name="comparaciones">Número de comparaciones realizadas.</param>
        /// <param name="intercambios">Número de intercambios realizados.</param>
        public SolutionSorting(int[] numerosOrdenados, long comparaciones = 0, long intercambios = 0)
        {
            if (numerosOrdenados == null)
                throw new ArgumentNullException(nameof(numerosOrdenados), "El array no puede ser nulo.");

            NumerosOrdenados = (int[])numerosOrdenados.Clone();
            NumComparaciones = comparaciones;
            NumIntercambios = intercambios;
        }

        /// <summary>
        /// Obtiene una representación en string de la solución.
        /// </summary>
        /// <returns>Representación formateada de la solución.</returns>
        public override string ToString()
        {
            string arrayStr;
            if (NumerosOrdenados.Length <= 20)
            {
                arrayStr = $"[{string.Join(", ", NumerosOrdenados)}]";
            }
            else
            {
                string primeros = string.Join(", ", NumerosOrdenados.Take(5));
                string ultimos = string.Join(", ", NumerosOrdenados.TakeLast(5));
                arrayStr = $"[{primeros}, ... ({NumerosOrdenados.Length - 10} elementos) ..., {ultimos}]";
            }

            return $"Array Ordenado: {arrayStr}\n" +
                   $"Comparaciones: {NumComparaciones}\n" +
                   $"Intercambios: {NumIntercambios}\n" +
                   $"Válida: {(EsValida ? "Sí" : "No")}";
        }

        /// <summary>
        /// Obtiene una representación sumaria de la solución.
        /// </summary>
        /// <returns>Información resumida de la solución.</returns>
        public string ObtenerResumen()
        {
            return $"Tamaño: {NumerosOrdenados.Length}, " +
                   $"Comparaciones: {NumComparaciones}, " +
                   $"Intercambios: {NumIntercambios}, " +
                   $"Válida: {(EsValida ? "Sí" : "No")}";
        }
    }
}
