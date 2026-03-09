using System;
using System.Linq;
using DAA_P03.Parte1_Ordenamiento.Base;

namespace DAA_P03.Parte1_Ordenamiento.Modelo
{
    /// <summary>
    /// Representa una instancia del problema QuickSort.
    /// Contiene un array compartido con referencias start/final que delimitan el rango de trabajo.
    /// Esta implementación permite particionamiento in-place eficiente.
    /// </summary>
    public class InstanciaQuickSort : Instancia
    {
        /// <summary>
        /// Índice inicial del rango a ordenar (inclusive).
        /// </summary>
        public int Start { get; private set; }

        /// <summary>
        /// Índice final del rango a ordenar (inclusive).
        /// </summary>
        public int Final { get; private set; }

        /// <summary>
        /// El array de números (referencia compartida entre subproblemas).
        /// </summary>
        public int[] Vector { get; private set; }

        /// <summary>
        /// Índice del pivote para la partición.
        /// </summary>
        public int Pivot { get; private set; }

        /// <summary>
        /// Tamaño del rango actual (final - start + 1).
        /// </summary>
        public override int Tamaño => Final - Start + 1;

        /// <summary>
        /// Constructor principal que inicializa una instancia con un array completo.
        /// </summary>
        /// <param name="data">Array de números a ordenar.</param>
        public InstanciaQuickSort(int[] data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data), "El array no puede ser nulo.");
            
            Start = 0;
            Final = data.Length - 1;
            Vector = data;
            Pivot = (Start + Final) / 2;
        }

        /// <summary>
        /// Constructor para crear subproblemas con rangos específicos.
        /// Usado internamente por el algoritmo de división.
        /// </summary>
        /// <param name="start">Índice inicial del rango.</param>
        /// <param name="final">Índice final del rango.</param>
        /// <param name="pivot">Índice del pivote.</param>
        /// <param name="vector">Referencia al array compartido.</param>
        public InstanciaQuickSort(int start, int final, int pivot, int[] vector)
        {
            if (vector == null)
                throw new ArgumentNullException(nameof(vector), "El array no puede ser nulo.");
            
            Start = start;
            Final = final;
            Pivot = pivot;
            Vector = vector;
        }

        /// <summary>
        /// Obtiene una copia de la instancia.
        /// Clona el vector para evitar modificaciones no deseadas.
        /// </summary>
        /// <returns>Nueva instancia con un vector clonado.</returns>
        public override Instancia ObtenerCopia()
        {
            int[] vectorClonado = (int[])Vector.Clone();
            return new InstanciaQuickSort(Start, Final, Pivot, vectorClonado);
        }

        /// <summary>
        /// Obtiene una representación en string del rango actual.
        /// Muestra el rango completo si es pequeño, o los extremos si es grande.
        /// </summary>
        /// <returns>Representación formateada del rango.</returns>
        public override string ToString()
        {
            if (Tamaño <= 0)
                return "[]";

            if (Tamaño <= 20)
            {
                var rango = Vector.Skip(Start).Take(Tamaño);
                return $"[{string.Join(", ", rango)}] (índices {Start}-{Final})";
            }
            else
            {
                var primeros = Vector.Skip(Start).Take(5);
                var ultimos = Vector.Skip(Final - 4).Take(5);
                return $"[{string.Join(", ", primeros)}, ... ({Tamaño - 10} elementos) ..., {string.Join(", ", ultimos)}] (índices {Start}-{Final})";
            }
        }

        /// <summary>
        /// Imprime la instancia completa en consola (útil para depuración).
        /// </summary>
        public void ImprimirInstancia()
        {
            Console.WriteLine(ToString());
        }
    }
}
