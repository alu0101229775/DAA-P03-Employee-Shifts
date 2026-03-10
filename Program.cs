using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using DAA_P03.Parte1_Ordenamiento.Base;
using DAA_P03.Parte1_Ordenamiento.Algoritmos;
using DAA_P03.Parte1_Ordenamiento.Modelo;
using DAA_P03.Parte2_Planificacion.Modelo;
using DAA_P03.Parte2_Planificacion.Algoritmos;
using DAA_P03.Parte2_Planificacion.Servicios;

namespace DAA_P03
{
    /// <summary>
    /// Aplicación principal para demostrar los algoritmos de Divide y Vencerás.
    /// Implementa:
    /// - PARTE 1: MergeSort y QuickSort
    /// - PARTE 2: Planificación de empleados
    /// 
    /// Modos de ejecución:
    /// - Modo Comparativo: Compara MergeSort vs QuickSort con diferentes tamaños
    /// - Modo Normal: Elige algoritmo, ejecuta con diferentes tamaños, muestra tiempos
    /// - Modo Debug: Elige algoritmo y tamaño, muestra instancia y solución completa
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            MostrarMenuPrincipal();
        }

        static int[] GenerarArrayAleatorio(int tamaño, int seed)
        {
            Random random = new Random(seed);
            int[] array = new int[tamaño];
            for (int i = 0; i < tamaño; i++)
            {
                array[i] = random.Next(1000000);
            }
            return array;
        }

        static void MostrarMenuPrincipal()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("═══════════════════════════════════════════════════════");
                Console.WriteLine("   PRÁCTICA 3 - ALGORITMOS DIVIDE Y VENCERÁS");
                Console.WriteLine("═══════════════════════════════════════════════════════");
                Console.WriteLine();
                Console.WriteLine("Seleccione la parte a ejecutar:");
                Console.WriteLine();
                Console.WriteLine("  [1] PARTE 1: Algoritmos de Ordenamiento");
                Console.WriteLine("  [2] PARTE 2: Planificación de Empleados");
                Console.WriteLine("  [0] Salir");
                Console.WriteLine();
                Console.Write("Opción: ");

                string opcion = Console.ReadLine();

                switch (opcion)
                {
                    case "1":
                        MostrarMenuParte1();
                        break;
                    case "2":
                        MostrarMenuParte2();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("\nOpción inválida. Presione cualquier tecla...");
                        Console.ReadKey();
                        break;
                }
            }
        }

        static void MostrarMenuParte1()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("═══════════════════════════════════════════════════════");
                Console.WriteLine("   PARTE 1: ALGORITMOS DE ORDENAMIENTO");
                Console.WriteLine("═══════════════════════════════════════════════════════");
                Console.WriteLine();
                Console.WriteLine("Seleccione el modo de ejecución:");
                Console.WriteLine();
                Console.WriteLine("  [1] Modo Comparativo (MergeSort vs QuickSort)");
                Console.WriteLine("  [2] Modo Normal (un algoritmo, múltiples tamaños)");
                Console.WriteLine("  [3] Modo Debug (instancia y solución detalladas)");
                Console.WriteLine("  [0] Volver");
                Console.WriteLine();
                Console.Write("Opción: ");

                string opcion = Console.ReadLine();

                switch (opcion)
                {
                    case "1":
                        EjecutarModoComparativo();
                        break;
                    case "2":
                        EjecutarModoNormal();
                        break;
                    case "3":
                        EjecutarModoDebug();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("\nOpción inválida. Presione cualquier tecla...");
                        Console.ReadKey();
                        break;
                }
            }
        }

        static void MostrarMenuParte2()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("═══════════════════════════════════════════════════════");
                Console.WriteLine("   PARTE 2: PLANIFICACIÓN DE EMPLEADOS");
                Console.WriteLine("═══════════════════════════════════════════════════════");
                Console.WriteLine();
                Console.WriteLine("Seleccione una opción:");
                Console.WriteLine();
                Console.WriteLine("  [1] Prueba de Carga de Instancias JSON");
                Console.WriteLine("  [2] Ejecutar Planificación (seleccionar instancia)");
                Console.WriteLine("  [0] Volver");
                Console.WriteLine();
                Console.Write("Opción: ");

                string opcion = Console.ReadLine();

                switch (opcion)
                {
                    case "1":
                        PruebaCargarInstanciasJSON();
                        break;
                    case "2":
                        EjecutarPlanificacion();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("\nOpción inválida. Presione cualquier tecla...");
                        Console.ReadKey();
                        break;
                }
            }
        }

        static void PruebaCargarInstanciasJSON()
        {
            Console.Clear();
            Console.WriteLine("═══════════════════════════════════════════════════════");
            Console.WriteLine("  PRUEBA DE CARGA DE INSTANCIAS JSON");
            Console.WriteLine("═══════════════════════════════════════════════════════\n");

            string ejemplosDir = "Ejemplos";
            
            if (!Directory.Exists(ejemplosDir))
            {
                Console.WriteLine("❌ ERROR: No se encontró la carpeta 'Ejemplos'");
                Console.WriteLine("Presione cualquier tecla...");
                Console.ReadKey();
                return;
            }

            string[] archivosJSON = Directory.GetFiles(ejemplosDir, "*.json");
            
            if (archivosJSON.Length == 0)
            {
                Console.WriteLine("❌ ERROR: No hay archivos JSON en la carpeta 'Ejemplos'");
                Console.WriteLine("Presione cualquier tecla...");
                Console.ReadKey();
                return;
            }

            Console.WriteLine($"✓ Se encontraron {archivosJSON.Length} archivos JSON\n");

            int exitosos = 0;
            int errores = 0;

            foreach (string ruta in archivosJSON)
            {
                string nombre = Path.GetFileName(ruta);
                Console.Write($"Cargando: {nombre,55} ... ");

                try
                {
                    // Intentar cargar la instancia
                    InstanciaPlanificacion instancia = GestorInstanciasPlanificacion.CargarDesdeJSON(ruta);

                    // Validaciones básicas
                    if (!instancia.EsValida())
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("❌ INVÁLIDA");
                        Console.ResetColor();
                        errores++;
                        continue;
                    }

                    // Mostrar información de la instancia
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("✓ OK");
                    Console.ResetColor();
                    
                    Console.WriteLine($"  ├─ Días: {instancia.NumDias}  Empleados: {instancia.NumEmpleados}  Turnos: {instancia.NumTurnos}");
                    Console.WriteLine($"  └─ Tamaño: {instancia.Tamaño}");

                    // Validar matrices
                    if (instancia.Satisfaccion == null || instancia.CoberturaMínima == null)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine($"  ⚠ ADVERTENCIA: Matrices nulas");
                        Console.ResetColor();
                        errores++;
                        continue;
                    }

                    exitosos++;
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"❌ ERROR");
                    Console.ResetColor();
                    Console.WriteLine($"  └─ {ex.Message.Substring(0, Math.Min(50, ex.Message.Length))}...");
                    errores++;
                }
            }

            // Resumen
            Console.WriteLine("\n" + new string('═', 63));
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"✓ Exitosos: {exitosos}/{archivosJSON.Length}");
            Console.ResetColor();
            
            if (errores > 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"❌ Errores: {errores}/{archivosJSON.Length}");
                Console.ResetColor();
            }

            if (exitosos == archivosJSON.Length)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\n✓ TODAS LAS INSTANCIAS CARGARON CORRECTAMENTE");
                Console.ResetColor();
            }

            Console.WriteLine();
            Console.WriteLine("Presione cualquier tecla para volver...");
            Console.ReadKey();
        }

        static void EjecutarPlanificacion()
        {
            Console.Clear();
            Console.WriteLine("═══════════════════════════════════════════════════════");
            Console.WriteLine("   EJECUTAR PLANIFICACIÓN");
            Console.WriteLine("═══════════════════════════════════════════════════════\n");

            string ejemplosDir = "Ejemplos";
            string[] archivosJSON = Directory.GetFiles(ejemplosDir, "*.json");

            if (archivosJSON.Length == 0)
            {
                Console.WriteLine("❌ No hay instancias disponibles");
                Console.WriteLine("Presione cualquier tecla...");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("Instancias disponibles:\n");
            for (int i = 0; i < archivosJSON.Length; i++)
            {
                Console.WriteLine($"  [{i + 1}] {Path.GetFileName(archivosJSON[i])}");
            }

            Console.WriteLine();
            Console.Write("Seleccione una instancia (número): ");
            if (!int.TryParse(Console.ReadLine(), out int opcion) || opcion < 1 || opcion > archivosJSON.Length)
            {
                Console.WriteLine("Opción inválida.");
                Console.WriteLine("Presione cualquier tecla...");
                Console.ReadKey();
                return;
            }

            string rutaSeleccionada = archivosJSON[opcion - 1];
            Console.WriteLine($"\nCargando: {Path.GetFileName(rutaSeleccionada)}...\n");

            try
            {
                InstanciaPlanificacion instancia = GestorInstanciasPlanificacion.CargarDesdeJSON(rutaSeleccionada);
                
                if (!instancia.EsValida())
                {
                    Console.WriteLine("❌ La instancia no es válida");
                    Console.WriteLine("Presione cualquier tecla...");
                    Console.ReadKey();
                    return;
                }

                Console.WriteLine($"✓ Instancia cargada correctamente");
                Console.WriteLine($"  Días: {instancia.NumDias}, Empleados: {instancia.NumEmpleados}, Turnos: {instancia.NumTurnos}\n");

                Console.WriteLine("Ejecutando algoritmo D&C Optimizado...\n");
                
                var algoritmo = new PlanificacionDivideYVenceras();
                var cronometro = Stopwatch.StartNew();
                Solucion solucion = algoritmo.Resolver(instancia);
                cronometro.Stop();

                Console.Clear();
                Console.WriteLine("═══════════════════════════════════════════════════════");
                Console.WriteLine("   RESULTADO");
                Console.WriteLine("═══════════════════════════════════════════════════════");

                if (solucion is SolucionPlanificacion solPlanificacion)
                {
                    Console.WriteLine(solPlanificacion.ObtenerRepresentacionTabla(instancia));
                    Console.WriteLine($"\n⏱ Tiempo de ejecución: {cronometro.ElapsedMilliseconds}ms");
                    Console.WriteLine($"📊 Operaciones realizadas: {algoritmo.NumOperaciones}");
                }

                Console.WriteLine("\nPresione cualquier tecla para volver...");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"❌ ERROR: {ex.Message}");
                Console.ResetColor();
                Console.WriteLine("\nPresione cualquier tecla...");
                Console.ReadKey();
            }
        }

        static void EjecutarModoComparativo()
        {
            Console.Clear();
            Console.WriteLine("═══════════════════════════════════════════════════════");
            Console.WriteLine("   MODO COMPARATIVO: MergeSort vs QuickSort");
            Console.WriteLine("═══════════════════════════════════════════════════════");
            Console.WriteLine();

            Console.Write("Número de repeticiones por tamaño (sugerido: 5-10): ");
            if (!int.TryParse(Console.ReadLine(), out int repeticiones) || repeticiones < 1)
            {
                Console.WriteLine("\nNúmero de repeticiones inválido.");
                Console.WriteLine("Presione cualquier tecla...");
                Console.ReadKey();
                return;
            }

            int[] tamaños = { 100, 500, 1000, 5000, 10000, 50000, 100000 };
            var resultados = new List<(int tamaño, double tiempoMerge, double tiempoQuick)>();

            Console.WriteLine();
            Console.WriteLine("Ejecutando pruebas...");
            Console.WriteLine();

            var mergeSort = new MergeSort();
            var quickSort = new QuickSort();

            foreach (int tamaño in tamaños)
            {
                long sumaTiemposMerge = 0;
                long sumaTiemposQuick = 0;

                for (int rep = 0; rep < repeticiones; rep++)
                {
                    int[] arrayBase = GenerarArrayAleatorio(tamaño, tamaño + rep);

                    var instanciaMerge = new InstanciaMergeSort(arrayBase);
                    mergeSort.ResolverConTiempo(instanciaMerge);
                    sumaTiemposMerge += mergeSort.TiempoEjecucion;

                    var instanciaQuick = new InstanciaQuickSort((int[])arrayBase.Clone());
                    quickSort.ResolverConTiempo(instanciaQuick);
                    sumaTiemposQuick += quickSort.TiempoEjecucion;
                }

                double promedioMerge = sumaTiemposMerge / (double)repeticiones;
                double promedioQuick = sumaTiemposQuick / (double)repeticiones;
                resultados.Add((tamaño, promedioMerge, promedioQuick));
            }

            Console.Clear();
            Console.WriteLine("═══════════════════════════════════════════════════════");
            Console.WriteLine("   RESULTADOS COMPARATIVOS");
            Console.WriteLine("═══════════════════════════════════════════════════════");
            Console.WriteLine();
            Console.WriteLine($"Repeticiones por tamaño: {repeticiones}");
            Console.WriteLine();
            Console.WriteLine("┌────────────┬──────────────────┬──────────────────┬────────────┐");
            Console.WriteLine("│   Tamaño   │   MergeSort (ms) │  QuickSort (ms)  │   Ganador  │");
            Console.WriteLine("├────────────┼──────────────────┼──────────────────┼────────────┤");

            foreach (var (tamaño, tiempoMerge, tiempoQuick) in resultados)
            {
                string ganador = tiempoMerge < tiempoQuick ? "MergeSort" : "QuickSort";
                Console.WriteLine($"│ {tamaño,10} │ {tiempoMerge,16:F3} │ {tiempoQuick,16:F3} │ {ganador,10} │");
            }

            Console.WriteLine("└────────────┴──────────────────┴──────────────────┴────────────┘");
            Console.WriteLine();
            Console.WriteLine("Presione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        static void EjecutarModoNormal()
        {
            Console.Clear();
            Console.WriteLine("═══════════════════════════════════════════════════════");
            Console.WriteLine("   MODO NORMAL: Ejecución con Múltiples Tamaños");
            Console.WriteLine("═══════════════════════════════════════════════════════");
            Console.WriteLine();

            Console.WriteLine("Seleccione el algoritmo:");
            Console.WriteLine();
            Console.WriteLine("  [1] MergeSort");
            Console.WriteLine("  [2] QuickSort");
            Console.WriteLine();
            Console.Write("Opción: ");

            string opcionAlgo = Console.ReadLine();
            Algoritmo algoritmo = null;

            switch (opcionAlgo)
            {
                case "1":
                    algoritmo = new MergeSort();
                    break;
                case "2":
                    algoritmo = new QuickSort();
                    break;
                default:
                    Console.WriteLine("\nOpción inválida.");
                    Console.WriteLine("Presione cualquier tecla...");
                    Console.ReadKey();
                    return;
            }

            Console.WriteLine();
            Console.Write("Número de repeticiones por tamaño (sugerido: 5-10): ");
            if (!int.TryParse(Console.ReadLine(), out int repeticiones) || repeticiones < 1)
            {
                Console.WriteLine("\nNúmero de repeticiones inválido.");
                Console.WriteLine("Presione cualquier tecla...");
                Console.ReadKey();
                return;
            }

            int[] tamaños = { 100, 500, 1000, 5000, 10000, 50000, 100000 };
            var resultados = new List<(int tamaño, double tiempoPromedio)>();

            Console.WriteLine();
            Console.WriteLine($"Ejecutando {algoritmo.Nombre} con diferentes tamaños...");
            Console.WriteLine();

            foreach (int tamaño in tamaños)
            {
                long sumaTiempos = 0;

                for (int rep = 0; rep < repeticiones; rep++)
                {
                    int[] arrayBase = GenerarArrayAleatorio(tamaño, tamaño + rep);
                    Instancia instancia = null;

                    if (algoritmo is MergeSort)
                    {
                        instancia = new InstanciaMergeSort(arrayBase);
                    }
                    else if (algoritmo is QuickSort)
                    {
                        instancia = new InstanciaQuickSort(arrayBase);
                    }

                    algoritmo.ResolverConTiempo(instancia);
                    sumaTiempos += algoritmo.TiempoEjecucion;
                }

                double promedio = sumaTiempos / (double)repeticiones;
                resultados.Add((tamaño, promedio));
            }

            Console.Clear();
            Console.WriteLine("═══════════════════════════════════════════════════════");
            Console.WriteLine($"   RESULTADOS: {algoritmo.Nombre}");
            Console.WriteLine("═══════════════════════════════════════════════════════");
            Console.WriteLine();
            Console.WriteLine($"Repeticiones por tamaño: {repeticiones}");
            Console.WriteLine();
            Console.WriteLine("┌────────────┬──────────────────┐");
            Console.WriteLine("│   Tamaño   │   Tiempo (ms)    │");
            Console.WriteLine("├────────────┼──────────────────┤");

            foreach (var (tamaño, tiempo) in resultados)
            {
                Console.WriteLine($"│ {tamaño,10} │ {tiempo,16:F3} │");
            }

            Console.WriteLine("└────────────┴──────────────────┘");
            Console.WriteLine();
            Console.WriteLine("Presione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        static void EjecutarModoDebug()
        {
            Console.Clear();
            Console.WriteLine("═══════════════════════════════════════════════════════");
            Console.WriteLine("   MODO DEBUG: Instancia y Solución Detalladas");
            Console.WriteLine("═══════════════════════════════════════════════════════");
            Console.WriteLine();

            Console.WriteLine("Seleccione el algoritmo:");
            Console.WriteLine();
            Console.WriteLine("  [1] MergeSort");
            Console.WriteLine("  [2] QuickSort");
            Console.WriteLine();
            Console.Write("Opción: ");

            string opcionAlgo = Console.ReadLine();
            Algoritmo algoritmo = null;

            switch (opcionAlgo)
            {
                case "1":
                    algoritmo = new MergeSort();
                    break;
                case "2":
                    algoritmo = new QuickSort();
                    break;
                default:
                    Console.WriteLine("\nOpción inválida.");
                    Console.WriteLine("Presione cualquier tecla...");
                    Console.ReadKey();
                    return;
            }

            Console.WriteLine();
            Console.Write("Ingrese el tamaño de la instancia (10-100 recomendado): ");
            if (!int.TryParse(Console.ReadLine(), out int tamaño) || tamaño < 1)
            {
                Console.WriteLine("\nTamaño inválido.");
                Console.WriteLine("Presione cualquier tecla...");
                Console.ReadKey();
                return;
            }

            Console.Clear();
            Console.WriteLine("═══════════════════════════════════════════════════════");
            Console.WriteLine($"   MODO DEBUG: {algoritmo.Nombre}");
            Console.WriteLine("═══════════════════════════════════════════════════════");
            Console.WriteLine();

            int[] arrayBase = GenerarArrayAleatorio(tamaño, 42);

            Instancia instancia = null;
            if (algoritmo is MergeSort)
            {
                instancia = new InstanciaMergeSort(arrayBase);
            }
            else if (algoritmo is QuickSort)
            {
                instancia = new InstanciaQuickSort(arrayBase);
            }

            Console.WriteLine($"INSTANCIA GENERADA (Tamaño: {instancia.Tamaño})");
            Console.WriteLine("─────────────────────────────────────────────────────");
            Console.WriteLine(instancia.ToString());
            Console.WriteLine();

            Console.WriteLine("Resolviendo...");
            Solucion solucion = algoritmo.ResolverConTiempo(instancia);

            Console.WriteLine();
            Console.WriteLine($"SOLUCIÓN OBTENIDA (Tiempo: {algoritmo.TiempoEjecucion}ms)");
            Console.WriteLine("─────────────────────────────────────────────────────");
            Console.WriteLine(solucion.ToString());
            Console.WriteLine();
            Console.WriteLine($"Información del Algoritmo:");
            Console.WriteLine(algoritmo.ObtenerInfo());
            Console.WriteLine();

            if (solucion.EsValida)
            {
                Console.WriteLine("✓ La solución es VÁLIDA (array ordenado correctamente)");
            }
            else
            {
                Console.WriteLine("✗ La solución es INVÁLIDA");
            }

            Console.WriteLine();
            Console.WriteLine("Presione cualquier tecla para continuar...");
            Console.ReadKey();
        }
    }
}

