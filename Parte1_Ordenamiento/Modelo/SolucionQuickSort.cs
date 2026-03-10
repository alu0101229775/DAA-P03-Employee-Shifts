using System;
using System.Linq;
using DAA_P03.Parte1_Ordenamiento.Base;

namespace DAA_P03.Parte1_Ordenamiento.Modelo
{
    /// <summary>
    /// Representa una solución al problema QuickSort.
    /// Mantiene una referencia al vector compartido con el rango ordenado (start-final).
    /// Esta implementación permite modificación in-place del vector.
    /// </summary>
    public class SolucionQuickSort : Solucion
    {
        public int Start { get; private set; }
        public int Final { get; private set; }
        public int[] Vector { get; private set; }
        public int Tamaño => Final - Start + 1;

        /// <summary>
        /// Constructor que inicializa una solución QuickSort.
        /// </summary>
        /// <param name="start">Índice inicial del rango ordenado.</param>
        /// <param name="final">Índice final del rango ordenado.</param>
        /// <param name="vector">Vector con el rango ordenado (referencia compartida).</param>
        public SolucionQuickSort(int start, int final, int[] vector)
        {
            if (vector == null)
                throw new ArgumentNullException(nameof(vector), "El vector no puede ser nulo.");
            
            Start = start;
            Final = final;
            Vector = vector;
        }

        /// <summary>
        /// Indica si la solución es válida (rango ordenado correctamente).
        /// </summary>
        public override bool EsValida
        {
            get
            {
                for (int i = Start; i < Final; i++)
                {
                    if (Vector[i] > Vector[i + 1])
                        return false;
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
            return $"QuickSort - Rango [{Start}-{Final}], Tamaño: {Tamaño}, Ordenado: {EsValida}";
        }

        /// <summary>
        /// Obtiene una representación en string del rango ordenado.
        /// </summary>
        /// <returns>Representación formateada.</returns>
        public override string ToString()
        {
            if (Tamaño <= 0)
                return "[]";

            if (Tamaño <= 20)
            {
                var rango = Vector.Skip(Start).Take(Tamaño);
                return $"[{string.Join(", ", rango)}] (índices {Start}-{Final}, ordenado: {EsValida})";
            }
            else
            {
                var primeros = Vector.Skip(Start).Take(5);
                var ultimos = Vector.Skip(Final - 4).Take(5);
                return $"[{string.Join(", ", primeros)}, ... ({Tamaño - 10} elementos) ..., {string.Join(", ", ultimos)}] (índices {Start}-{Final}, ordenado: {EsValida})";
            }
        }
    }
}
