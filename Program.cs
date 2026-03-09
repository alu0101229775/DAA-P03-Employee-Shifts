using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using DAA_P03.Core.Algorithms;
using DAA_P03.Core.Algorithms.Sorting;
using DAA_P03.Core.Data;
using DAA_P03.Core.EmployeeScheduling;

namespace DAA_P03
{
    /// <summary>
    /// Aplicación principal para demostrar los algoritmos de Divide y Vencerás.
    /// Implementa:
    /// - PARTE 1: MergeSort y QuickSort
    /// - PARTE 2: Planificación de empleados
    /// 
    /// Modos de ejecución:
    /// - Modo Normal: Selecciona algoritmo, ejecuta con diferentes tamaños, muestra tiempos
    /// - Modo Debug: Selecciona algoritmo, tamaño, muestra instancia y solución completa
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            MostrarMenuPrincipal();
        }

        /// <summary>
        /// Muestra el menú principal de la aplicación.
        /// </summary>
        static void MostrarMenuPrincipal()
        {
            bool salir = false;

            while (!salir)
            {
                Console.Clear();
                Console.WriteLine("╔" + new string('═', 78) + "╗");
                Console.WriteLine("║" + "SISTEMA DE ANÁLISIS DE ALGORITMOS - DIVIDE Y VENCERÁS".PadLeft(78) + "║");
                Console.WriteLine("╚" + new string('═', 78) + "╝");
                Console.WriteLine();
                Console.WriteLine("Seleccione la parte a ejecutar:");
                Console.WriteLine();
                Console.WriteLine("  [1] PARTE 1: Algoritmos de Ordenamiento (MergeSort, QuickSort)");
                Console.WriteLine("  [2] PARTE 2: Planificación de Empleados");
                Console.WriteLine("  [0] Salir");
                Console.WriteLine();
                Console.Write("Opción: ");

                string opcion = Console.ReadLine();

                switch (opcion)
                {
                    case "1":
                        MenuParte1();
                        break;
                    case "2":
                        MenuParte2();
                        break;
                    case "0":
                        salir = true;
                        break;
                    default:
                        Console.WriteLine("Opción no válida. Presione cualquier tecla...");
                        Console.ReadKey();
                        break;
                }
            }
        }

        /// <summary>
        /// Menú para la PARTE 1: Algoritmos de Ordenamiento.
        /// </summary>
        static void MenuParte1()
        {
            bool volver = false;

            while (!volver)
            {
                Console.Clear();
                Console.WriteLine("╔" + new string('═', 78) + "╗");
                Console.WriteLine("║" + "PARTE 1: ALGORITMOS DE ORDENAMIENTO".PadLeft(78) + "║");
                Console.WriteLine("╚" + new string('═', 78) + "╝");
                Console.WriteLine();
                Console.WriteLine("Seleccione el modo de ejecución:");
                Console.WriteLine();
                Console.WriteLine("  [1] Modo Normal (Comparar tiempos con múltiples tamaños)");
                Console.WriteLine("  [2] Modo Debug (Ver instancia y solución)");
                Console.WriteLine("  [0] Volver al menú principal");
                Console.WriteLine();
                Console.Write("Opción: ");

                string opcion = Console.ReadLine();

                switch (opcion)
                {
                    case "1":
                        ModoNormalOrdenamiento();
                        break;
                    case "2":
                        ModoDebugOrdenamiento();
                        break;
                    case "0":
                        volver = true;
                        break;
                    default:
                        Console.WriteLine("Opción no válida. Presione cualquier tecla...");
                        Console.ReadKey();
                        break;
                }
            }
        }

        /// <summary>
        /// Modo Normal: Compara el rendimiento de MergeSort y QuickSort con múltiples tamaños.
        /// </summary>
        static void ModoNormalOrdenamiento()
        {
            Console.Clear();
            Console.WriteLine("╔" + new string('═', 78) + "╗");
            Console.WriteLine("║" + "MODO NORMAL - COMPARACIÓN DE ALGORITMOS".PadLeft(78) + "║");
            Console.WriteLine("╚" + new string('═', 78) + "╝");
            Console.WriteLine();

            // Tamaños a probar
            int[] tamaños = { 100, 500, 1000, 5000, 10000, 50000, 100000 };
            int repeticiones = 3;

            Console.WriteLine($"Se ejecutarán los algoritmos con tamaños: {string.Join(", ", tamaños)}");
            Console.WriteLine($"Repeticiones por tamaño: {repeticiones}");
            Console.WriteLine();
            Console.WriteLine("Procesando...\n");

            var mergeSort = new MergeSort();
            var quickSort = new QuickSort();

            var resultados = new List<(int tamaño, double tiempoMerge, double tiempoQuick)>();

            for (int i = 0; i < tamaños.Length; i++)
            {
                int tamaño = tamaños[i];
                double sumaTimesMerge = 0;
                double sumaTimesQuick = 0;

                for (int rep = 0; rep < repeticiones; rep++)
                {
                    // Crear la misma instancia para ambos algoritmos
                    var instancia = new InstanceSorting(tamaño, tamaño + rep);

                    // Ejecutar MergeSort
                    mergeSort.ResolverConTiempo(instancia.ObtenerCopia());
                    sumaTimesMerge += mergeSort.TiempoEjecucion;

                    // Ejecutar QuickSort
                    quickSort.ResolverConTiempo(instancia.ObtenerCopia());
                    sumaTimesQuick += quickSort.TiempoEjecucion;
                }

                double promedioMerge = sumaTimesMerge / repeticiones;
                double promedioQuick = sumaTimesQuick / repeticiones;
                resultados.Add((tamaño, promedioMerge, promedioQuick));

                Console.Write($"Tamaño {tamaño,6}: MergeSort={promedioMerge,8:F2}ms, " +
                             $"QuickSort={promedioQuick,8:F2}ms\r");
            }

            Console.WriteLine("\n");
            MostrarTablaComparativa(resultados);

            Console.WriteLine("\nPresione cualquier tecla para volver...");
            Console.ReadKey();
        }

        /// <summary>
        /// Modo Debug: Permite ver una instancia específica y su solución.
        /// </summary>
        static void ModoDebugOrdenamiento()
        {
            Console.Clear();
            Console.WriteLine("╔" + new string('═', 78) + "╗");
            Console.WriteLine("║" + "MODO DEBUG - VER INSTANCIA Y SOLUCIÓN".PadLeft(78) + "║");
            Console.WriteLine("╚" + new string('═', 78) + "╝");
            Console.WriteLine();

            Console.WriteLine("Seleccione el algoritmo:");
            Console.WriteLine("  [1] MergeSort");
            Console.WriteLine("  [2] QuickSort");
            Console.Write("\nOpción: ");

            string algoritmo = Console.ReadLine();

            Console.Write("Ingrese el tamaño de la instancia (10-1000): ");
            if (!int.TryParse(Console.ReadLine(), out int tamaño) || tamaño < 10 || tamaño > 1000)
            {
                Console.WriteLine("Tamaño inválido.");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("\nGenerando instancia...");

            var instancia = new InstanceSorting(tamaño, 42);
            Algoritmo algo = algoritmo == "1" ? new MergeSort() : new QuickSort();

            Console.WriteLine($"\nInstancia (Tamaño: {instancia.Tamaño}):");
            Console.WriteLine(instancia.ToString());

            Console.WriteLine("\nResolviendo...");
            var inicio = DateTime.Now;
            object resultado = algo.ResolverConTiempo(instancia);
            var duracion = DateTime.Now - inicio;

            if (resultado is SolutionSorting solucion)
            {
                Console.WriteLine($"\nSolución obtenida en {algo.TiempoEjecucion}ms:");
                Console.WriteLine(solucion.ToString());

                if (solucion.EsValida)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("✓ La solución es VÁLIDA (array correctamente ordenado)");
                    Console.ResetColor();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("✗ La solución es INVÁLIDA (array NO correctamente ordenado)");
                    Console.ResetColor();
                }
            }

            Console.WriteLine("\nPresione cualquier tecla para volver...");
            Console.ReadKey();
        }

        /// <summary>
        /// Menú para la PARTE 2: Planificación de Empleados.
        /// </summary>
        static void MenuParte2()
        {
            bool volver = false;

            while (!volver)
            {
                Console.Clear();
                Console.WriteLine("╔" + new string('═', 78) + "╗");
                Console.WriteLine("║" + "PARTE 2: PLANIFICACIÓN DE EMPLEADOS".PadLeft(78) + "║");
                Console.WriteLine("╚" + new string('═', 78) + "╝");
                Console.WriteLine();
                Console.WriteLine("Seleccione una opción:");
                Console.WriteLine();
                Console.WriteLine("  [1] Ejecutar con instancia de prueba");
                Console.WriteLine("  [2] Cargar instancia desde archivo JSON");
                Console.WriteLine("  [3] Generar y exportar instancia de prueba");
                Console.WriteLine("  [0] Volver al menú principal");
                Console.WriteLine();
                Console.Write("Opción: ");

                string opcion = Console.ReadLine();

                switch (opcion)
                {
                    case "1":
                        CargarYEjecutarPlanificacion();
                        break;
                    case "2":
                        ExportarInstanciaPrueba();
                        break;
                    case "0":
                        volver = true;
                        break;
                    default:
                        Console.WriteLine("Opción no válida. Presione cualquier tecla...");
                        Console.ReadKey();
                        break;
                }
            }
        }

        /// <summary>
        /// Ejecuta el algoritmo de planificación con una instancia específica.
        /// </summary>
        static void EjecutarPlanificacion(InstancePlanning instancia)
        {
            if (instancia == null)
            {
                Console.WriteLine("Error: Instancia nula.");
                Console.ReadKey();
                return;
            }

            Console.Clear();
            Console.WriteLine("╔" + new string('═', 78) + "╗");
            Console.WriteLine("║" + "EJECUCIÓN DEL ALGORITMO DE PLANIFICACIÓN".PadLeft(78) + "║");
            Console.WriteLine("╚" + new string('═', 78) + "╝");
            Console.WriteLine();

            Console.WriteLine("Instancia:");
            Console.WriteLine(instancia.ToString());
            Console.WriteLine();

            Console.WriteLine("Resolviendo...");
            var algo = new PlanificacionDivideYVenceras();

            var inicio = DateTime.Now;
            var solucion = algo.ResolverConTiempo(instancia) as SolutionPlanning;
            var duracion = (DateTime.Now - inicio).TotalMilliseconds;

            if (solucion != null)
            {
                Console.WriteLine($"✓ Solución encontrada en {algo.TiempoEjecucion}ms");
                Console.WriteLine(solucion.ObtenerRepresentacionTabla(instancia.Empleados, instancia.Turnos));
            }
            else
            {
                Console.WriteLine("✗ Error: No se pudo generar la solución.");
            }

            Console.WriteLine("\nPresione cualquier tecla para volver...");
            Console.ReadKey();
        }

        /// <summary>
        /// Carga una instancia desde un archivo JSON y la ejecuta.
        /// </summary>
        static void CargarYEjecutarPlanificacion()
        {
            Console.Clear();
            Console.Write("Ingrese la ruta del archivo JSON: ");
            string ruta = Console.ReadLine();

            try
            {
                var instancia = PlanningInstanceManager.CargarDesdeJSON(ruta);
                EjecutarPlanificacion(instancia);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                Console.ReadKey();
            }
        }

        /// <summary>
        /// Exporta una instancia de prueba a un archivo JSON.
        /// </summary>
        static void ExportarInstanciaPrueba()
        {
            Console.Clear();
            Console.WriteLine("Para exportar una instancia, primero cargala desde un archivo JSON.");
            Console.WriteLine("Usa la opción anterior para cargar y ejecutar una instancia.");
            Console.ReadKey();
        }

        /// <summary>
        /// Muestra una tabla comparativa de tiempos de ejecución.
        /// </summary>
        static void MostrarTablaComparativa(List<(int tamaño, double tiempoMerge, double tiempoQuick)> resultados)
        {
            Console.WriteLine(new string('═', 85));
            Console.WriteLine("TABLA COMPARATIVA DE TIEMPOS DE EJECUCIÓN");
            Console.WriteLine(new string('═', 85));
            Console.WriteLine($"{"Tamaño",-15} {"MergeSort (ms)",-20} {"QuickSort (ms)",-20} {"Diferencia (ms)",-15}");
            Console.WriteLine(new string('─', 85));

            foreach (var (tamaño, tiempoMerge, tiempoQuick) in resultados)
            {
                double diferencia = tiempoMerge - tiempoQuick;
                string mejora = diferencia > 0 ? "QuickSort" : "MergeSort";

                Console.WriteLine($"{tamaño,-15} {tiempoMerge,-20:F2} {tiempoQuick,-20:F2} {Math.Abs(diferencia),-15:F2}");
            }

            Console.WriteLine(new string('═', 85));
        }
    }
}

