using System;
using System.Diagnostics;

namespace DAA_P03.Core.Algorithms
{
    /// <summary>
    /// Clase base abstracta que representa un algoritmo genérico.
    /// Proporciona funciones comunes para medir tiempos de ejecución y gestionar instancias.
    /// Esta clase sirve como punto de partida para todos los algoritmos implementados.
    /// </summary>
    public abstract class Algoritmo
    {
        /// <summary>
        /// Nombre descriptivo del algoritmo.
        /// </summary>
        public string Nombre { get; protected set; }

        /// <summary>
        /// Descripción detallada del algoritmo.
        /// </summary>
        public string Descripcion { get; protected set; }

        /// <summary>
        /// Tiempo de ejecución en milisegundos de la última ejecución.
        /// </summary>
        public long TiempoEjecucion { get; protected set; }

        /// <summary>
        /// Contador de operaciones realizadas (útil para análisis).
        /// </summary>
        public long NumOperaciones { get; protected set; }

        /// <summary>
        /// Inicializa el número de operaciones a cero.
        /// </summary>
        protected Algoritmo()
        {
            NumOperaciones = 0;
            TiempoEjecucion = 0;
        }

        /// <summary>
        /// Resuelve un problema dado. Este método debe ser implementado por cada algoritmo específico.
        /// </summary>
        /// <param name="instancia">La instancia del problema a resolver.</param>
        /// <returns>La solución del problema.</returns>
        public abstract object Resolver(object instancia);

        /// <summary>
        /// Resuelve un problema y mide el tiempo de ejecución.
        /// Este método es el encargado de llamar al método Resolver y medir el tiempo.
        /// </summary>
        /// <param name="instancia">La instancia del problema a resolver.</param>
        /// <returns>La solución del problema.</returns>
        public virtual object ResolverConTiempo(object instancia)
        {
            Stopwatch sw = Stopwatch.StartNew();
            object solucion = Resolver(instancia);
            sw.Stop();
            TiempoEjecucion = sw.ElapsedMilliseconds;
            return solucion;
        }

        /// <summary>
        /// Obtiene la información del algoritmo en formato string legible.
        /// </summary>
        /// <returns>Información del algoritmo.</returns>
        public virtual string ObtenerInfo()
        {
            return $"Algoritmo: {Nombre}\nDescripción: {Descripcion}\nÚltimo tiempo de ejecución: {TiempoEjecucion} ms";
        }

        /// <summary>
        /// Reinicia los contadores internos del algoritmo.
        /// </summary>
        public virtual void Reiniciar()
        {
            NumOperaciones = 0;
            TiempoEjecucion = 0;
        }
    }
}
