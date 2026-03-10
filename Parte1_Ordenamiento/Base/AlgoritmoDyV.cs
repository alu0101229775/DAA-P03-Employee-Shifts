using System;

namespace DAA_P03.Parte1_Ordenamiento.Base
{
    /// <summary>
    /// Clase abstracta que implementa el patrón Template para algoritmos de Divide y Vencerás.
    /// Define el esqueleto del algoritmo D&C con métodos abstractos que deben ser implementados
    /// por las clases derivadas para problemas específicos.
    /// Soporta división n-aria (no solo binaria) y tracking de métricas de recursión.
    /// </summary>
    public abstract class AlgoritmoDyV : Algoritmo
    {
        private int maxRecursiveDepth_ = 0;
        private int totalRecursiveCalls_ = 0;

        /// <summary>
        /// Constructor que inicializa el algoritmo Template de Divide y Vencerás.
        /// </summary>
        protected AlgoritmoDyV()
        {
            Nombre = "Divide y Vencerás (Template)";
        }

        public int GetMaxRecursiveDepth() { return maxRecursiveDepth_; }
        public int GetTotalRecursiveCalls() { return totalRecursiveCalls_; }

        /// <summary>
        /// Determina si la instancia es lo suficientemente pequeña para ser resuelta directamente.
        /// Debe ser implementado por cada algoritmo específico.
        /// </summary>
        /// <param name="instancia">La instancia a evaluar.</param>
        /// <returns>true si es pequeña, false en caso contrario.</returns>
        protected abstract bool EsPequenio(Instancia instancia);

        /// <summary>
        /// Resuelve el problema cuando es lo suficientemente pequeño.
        /// Debe ser implementado por cada algoritmo específico.
        /// </summary>
        /// <param name="instancia">La instancia pequeña a resolver.</param>
        /// <returns>La solución para el caso base.</returns>
        protected abstract Solucion ResolverPequenio(Instancia instancia);

        /// <summary>
        /// Divide la instancia en n subproblemas.
        /// Debe ser implementado por cada algoritmo específico.
        /// </summary>
        /// <param name="instancia">La instancia a dividir.</param>
        /// <returns>Un array con n subproblemas.</returns>
        protected abstract Instancia[] Dividir(Instancia instancia);

        /// <summary>
        /// Combina n soluciones en una solución completa.
        /// Debe ser implementado por cada algoritmo específico.
        /// </summary>
        /// <param name="soluciones">Array de soluciones parciales.</param>
        /// <returns>La solución combinada.</returns>
        protected abstract Solucion Combinar(Solucion[] soluciones);

        /// <summary>
        /// Método plantilla que implementa el algoritmo de Divide y Vencerás.
        /// Este método NO debe ser sobrescrito. Define el flujo general del algoritmo.
        /// </summary>
        /// <param name="instancia">La instancia del problema.</param>
        /// <param name="profundidadActual">Profundidad actual de recursión (uso interno).</param>
        /// <returns>La solución completa.</returns>
        protected Solucion ResolverDivideYVenceras(Instancia instancia, int profundidadActual = 0)
        {
            if (EsPequenio(instancia))
            {
                if (profundidadActual > maxRecursiveDepth_)
                {
                    maxRecursiveDepth_ = profundidadActual;
                }
                return ResolverPequenio(instancia);
            }
            else
            {
                Instancia[] subproblemas = Dividir(instancia);
                profundidadActual++;
                Solucion[] soluciones = new Solucion[subproblemas.Length];
                
                for (int i = 0; i < subproblemas.Length; i++)
                {
                    soluciones[i] = ResolverDivideYVenceras(subproblemas[i], profundidadActual);
                    totalRecursiveCalls_++;
                }

                Solucion solucionFinal = Combinar(soluciones);
                return solucionFinal;
            }
        }

        protected abstract string GetNumberOfSubproblems();
        protected abstract string GetSubproblemSizeDivisor();
        protected abstract string GetCombineCost();

        public string GetRecurrenceEquation()
        {
            return $"T(n) = {GetNumberOfSubproblems()} * T({GetSubproblemSizeDivisor()}) + O({GetCombineCost()})";
        }

        /// <summary>
        /// Implementación del método abstracto Resolver (requerido por Algoritmo).
        /// Delega la resolución al método ResolverDivideYVenceras y reinicia las métricas.
        /// </summary>
        /// <param name="instancia">La instancia a resolver.</param>
        /// <returns>La solución completa.</returns>
        public override Solucion Resolver(Instancia instancia)
        {
            maxRecursiveDepth_ = 0;
            totalRecursiveCalls_ = 0;
            return ResolverDivideYVenceras(instancia);
        }
    }
}
