using System;

namespace DAA_P03.Parte1_Ordenamiento.Base
{
    /// <summary>
    /// Clase abstracta base que representa una solución genérica a un problema.
    /// Parte del patrón Strategy para definir diferentes tipos de soluciones.
    /// </summary>
    public abstract class Solucion
    {
        /// <summary>
        /// Indica si la solución es válida.
        /// </summary>
        public abstract bool EsValida { get; }

        /// <summary>
        /// Obtiene información detallada de la solución.
        /// </summary>
        /// <returns>Información formateada de la solución.</returns>
        public abstract string ObtenerInfo();

        /// <summary>
        /// Obtiene una representación en string de la solución.
        /// </summary>
        /// <returns>Representación formateada de la solución.</returns>
        public abstract override string ToString();
    }
}
