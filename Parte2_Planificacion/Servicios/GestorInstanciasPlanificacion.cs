using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using DAA_P03.Parte2_Planificacion.Modelo;
using DAA_P03.Parte2_Planificacion.Dominio;

namespace DAA_P03.Parte2_Planificacion.Servicios
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
                            DiasDescanso = (int)(empToken["freeDays"]?.ToObject<double>() ?? 0)
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
                // JSON trae: [{shift, employee, day, value}, ...]
                // Matriz: Satisfaccion[day, employee, shift]
                var satisfaccionArray = obj["satisfaction"] as JArray;
                if (satisfaccionArray != null)
                {
                    foreach (var item in satisfaccionArray)
                    {
                        // Verificar si este item tiene "employee" (matriz de satisfacción) o no (cobertura)
                        var empToken = item["employee"];
                        if (empToken != null)
                        {
                            int emp = empToken.ToObject<int>();
                            int shift = item["shift"].ToObject<int>();
                            int day = item["day"].ToObject<int>();
                            int value = item["value"].ToObject<int>();

                            if (day < numDias && emp < empleados.Count && shift < turnos.Count)
                            {
                                instancia.Satisfaccion[day, emp, shift] = value;
                            }
                        }
                    }
                }

                // Extraer y rellenar la matriz de coberturaMinima
                // JSON trae: [ {shift, day, value}, ... ]
                // Matriz: CoberturaMínima[day, shift]
                var coberturaArray = obj["requiredEmployees"] as JArray;
                if (coberturaArray != null)
                {
                    foreach (var item in coberturaArray)
                    {
                        int shift = item["shift"].ToObject<int>();
                        int day = item["day"].ToObject<int>();
                        int value = item["value"].ToObject<int>();

                        if (day < numDias && shift < turnos.Count)
                        {
                            instancia.CoberturaMínima[day, shift] = value;
                        }
                    }
                }
                // Si no existe cobertura, inicializar a 1 para todos
                if (coberturaArray == null)
                {
                    for (int d = 0; d < numDias; d++)
                    {
                        for (int t = 0; t < turnos.Count; t++)
                        {
                            instancia.CoberturaMínima[d, t] = 1;
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
