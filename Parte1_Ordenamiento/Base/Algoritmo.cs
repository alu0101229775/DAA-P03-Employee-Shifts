using System;
using System.Diagnostics;

namespace DAA_P03.Parte1_Ordenamiento.Base
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
        public abstract Solucion Resolver(Instancia instancia);

        /// <summary>
        /// Resuelve un problema y mide el tiempo de ejecución.
        /// </summary>
        /// <param name="instancia">La instancia a resolver.</param>
        /// <returns>La solución obtenida.</returns>
        public Solucion ResolverConTiempo(Instancia instancia)
        {
            var stopwatch = Stopwatch.StartNew();
            Solucion resultado = Resolver(instancia);
            stopwatch.Stop();
            TiempoEjecucion = stopwatch.ElapsedMilliseconds;
            return resultado;
        }

        /// <summary>
        /// Obtiene información detallada del algoritmo.
        /// </summary>
        /// <returns>Información formateada del algoritmo.</returns>
        public virtual string ObtenerInfo()
        {
            return $"Algoritmo: {Nombre}\n" +
                   $"Descripción: {Descripcion}\n" +
                   $"Tiempo de ejecución: {TiempoEjecucion}ms\n" +
                   $"Operaciones: {NumOperaciones}";
        }

        /// <summary>
        /// Obtiene una representación en string del algoritmo.
        /// </summary>
        /// <returns>Información breve del algoritmo.</returns>
        public override string ToString()
        {
            return $"{Nombre} ({TiempoEjecucion}ms)";
        }
    }
}
