using System;

namespace DAA_P03.Parte1_Ordenamiento.Base
{
    /// <summary>
    /// Clase abstracta que implementa el patrón Template para algoritmos de Divide y Vencerás binarios.
    /// Define el esqueleto del algoritmo D&C con métodos abstractos que deben ser implementados
    /// por las clases derivadas para problemas específicos.
    /// 
    /// El pseudocódigo base es:
    /// Solve(Problema p, int tamaño) {
    ///    if (Small(p))
    ///       return SolveSmall(p)
    ///    else {
    ///       Problema m[] = Divide(p, tamaño)
    ///       Solucion S1 = Solve(m[0], tamaño/2)
    ///       Solucion S2 = Solve(m[1], tamaño/2)
    ///       Solucion S = Combine(S1, S2)
    ///       return S
    ///    }
    /// }
    /// </summary>
    public abstract class AlgoritmoDyV : Algoritmo
    {
        /// <summary>
        /// Constructor que inicializa el algoritmo Template de Divide y Vencerás.
        /// </summary>
        protected AlgoritmoDyV()
        {
            Nombre = "Divide y Vencerás (Template)";
            Descripcion = "Patrón plantilla para algoritmos de Divide y Vencerás binarios";
        }

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
        /// Divide la instancia en dos subproblemas.
        /// Debe ser implementado por cada algoritmo específico.
        /// </summary>
        /// <param name="instancia">La instancia a dividir.</param>
        /// <returns>Un array con dos subproblemas.</returns>
        protected abstract Instancia[] Dividir(Instancia instancia);

        /// <summary>
        /// Combina dos soluciones en una solución completa.
        /// Debe ser implementado por cada algoritmo específico.
        /// </summary>
        /// <param name="solucion1">Primera solución parcial.</param>
        /// <param name="solucion2">Segunda solución parcial.</param>
        /// <returns>La solución combinada.</returns>
        protected abstract Solucion Combinar(Solucion solucion1, Solucion solucion2);

        /// <summary>
        /// Método plantilla que implementa el algoritmo de Divide y Vencerás.
        /// Este método NO debe ser sobrescrito. Define el flujo general del algoritmo.
        /// </summary>
        /// <param name="instancia">La instancia del problema.</param>
        /// <returns>La solución completa.</returns>
        protected Solucion ResolverDivideYVenceras(Instancia instancia)
        {
            if (EsPequenio(instancia))
            {
                return ResolverPequenio(instancia);
            }
            else
            {
                Instancia[] subproblemas = Dividir(instancia);

                Solucion solucion1 = ResolverDivideYVenceras(subproblemas[0]);
                Solucion solucion2 = ResolverDivideYVenceras(subproblemas[1]);

                Solucion solucionFinal = Combinar(solucion1, solucion2);

                return solucionFinal;
            }
        }

        /// <summary>
        /// Implementación del método abstracto Resolver (requerido por Algoritmo).
        /// Delega la resolución al método ResolverDivideYVenceras.
        /// </summary>
        /// <param name="instancia">La instancia a resolver.</param>
        /// <returns>La solución completa.</returns>
        public override Solucion Resolver(Instancia instancia)
        {
            return ResolverDivideYVenceras(instancia);
        }
    }
}
