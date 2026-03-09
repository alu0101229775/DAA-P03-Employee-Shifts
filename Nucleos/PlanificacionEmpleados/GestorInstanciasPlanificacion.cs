using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;

namespace DAA_P03.Nucleos.PlanificacionEmpleados
{
    /// <summary>
    /// Gestor para cargar y deserializar instancias de planificación desde archivos JSON.
    /// </summary>
    public class GestorInstanciasPlanificacion
    {
        /// <summary>
        /// Carga una instancia de planificación desde un archivo JSON.
        /// </summary>
        public static InstanciaPlanificacion CargarDesdeJSON(string rutaArchivo)
        {
            if (!File.Exists(rutaArchivo))
                throw new FileNotFoundException($"El archivo {rutaArchivo} no existe.");

            try
            {
                string json = File.ReadAllText(rutaArchivo);
                JObject obj = JObject.Parse(json);

                // Extraer empleados
                var empleados = new List<Empleado>();
                var empleadosArray = obj["employees"] as JArray;
                if (empleadosArray != null)
                {
                    foreach (var empToken in empleadosArray)
                    {
                        var emp = new Empleado
                        {
                            Nombre = empToken["name"]?.ToString() ?? "",
                            DiasDescanso = (int)(empToken["daysOff"]?.ToObject<double>() ?? 0)
                        };
                        empleados.Add(emp);
                    }
                }

                // Extraer turnos
                var turnos = new List<string>();
                var turnosArray = obj["shifts"] as JArray;
                if (turnosArray != null)
                {
                    foreach (var turnoToken in turnosArray)
                    {
                        turnos.Add(turnoToken.ToString());
                    }
                }

                // Leer horizonte de planificación
                int numDias = obj["planningHorizon"]?.ToObject<int>() ?? 1;

                // Crear instancia
                var instancia = new InstanciaPlanificacion(
                    empleados.Count,
                    numDias,
                    turnos.Count
                )
                {
                    Empleados = empleados,
                    Turnos = turnos
                };

                // Extraer y rellenar la matriz de satisfacción
                var satisfaccionArray = obj["satisfaction"] as JArray;
                if (satisfaccionArray != null)
                {
                    int index = 0;
                    int numEmpleados = empleados.Count;
                    int numTurnos = turnos.Count;

                    for (int d = 0; d < numDias; d++)
                    {
                        for (int e = 0; e < numEmpleados; e++)
                        {
                            for (int t = 0; t < numTurnos; t++)
                            {
                                if (index < satisfaccionArray.Count)
                                {
                                    double valor = satisfaccionArray[index]["value"]?.ToObject<double>() ?? 0;
                                    instancia.Satisfaccion[d, e, t] = (int)valor;
                                    index++;
                                }
                            }
                        }
                    }
                }

                return instancia;
            }
            catch (JsonException ex)
            {
                throw new InvalidOperationException(
                    $"Error al deserializar JSON en {rutaArchivo}: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(
                    $"Error al cargar instancia desde {rutaArchivo}: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Carga todas las instancias de un directorio.
        /// </summary>
        public static List<InstanciaPlanificacion> CargarInstanciasDelDirectorio(string rutaDirectorio)
        {
            var instancias = new List<InstanciaPlanificacion>();

            if (!Directory.Exists(rutaDirectorio))
                throw new DirectoryNotFoundException($"El directorio {rutaDirectorio} no existe.");

            var archivosJSON = Directory.GetFiles(rutaDirectorio, "*.json");

            foreach (var archivo in archivosJSON)
            {
                try
                {
                    var instancia = CargarDesdeJSON(archivo);
                    instancias.Add(instancia);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Advertencia: No se pudo cargar {archivo}: {ex.Message}");
                }
            }

            return instancias;
        }
    }
}
