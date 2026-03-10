using System;

namespace DAA_P03.Parte1_Ordenamiento.Base
{
    /// <summary>
    /// Clase abstracta base que representa una instancia genérica de un problema.
    /// Parte del patrón Strategy para definir diferentes tipos de problemas.
    /// </summary>
    public abstract class Instancia
    {
        public abstract int Tamaño { get; }

        public abstract Instancia ObtenerCopia();

        public abstract override string ToString();
    }
}
