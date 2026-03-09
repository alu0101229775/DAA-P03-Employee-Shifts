using System;

namespace DAA_P03.Parte1_Ordenamiento.Base
{
    /// <summary>
    /// Clase abstracta base que representa una instancia genérica de un problema.
    /// Parte del patrón Strategy para definir diferentes tipos de problemas.
    /// </summary>
    public abstract class Instancia
    {
        /// <summary>
        /// Obtiene el tamaño de la instancia del problema.
        /// </summary>
        public abstract int Tamaño { get; }

        /// <summary>
        /// Obtiene una copia de la instancia.
        /// </summary>
        /// <returns>Una nueva instancia con los mismos datos.</returns>
        public abstract Instancia ObtenerCopia();

        /// <summary>
        /// Obtiene una representación en string de la instancia.
        /// </summary>
        /// <returns>Representación formateada de la instancia.</returns>
        public abstract override string ToString();
    }
}
