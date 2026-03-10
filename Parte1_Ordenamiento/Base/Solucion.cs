using System;

namespace DAA_P03.Parte1_Ordenamiento.Base
{
    /// <summary>
    /// Clase abstracta base que representa una solución genérica a un problema.
    /// Parte del patrón Strategy para definir diferentes tipos de soluciones.
    /// </summary>
    public abstract class Solucion
    {
        public abstract bool EsValida { get; }

        public abstract string ObtenerInfo();

        public abstract override string ToString();
    }
}
