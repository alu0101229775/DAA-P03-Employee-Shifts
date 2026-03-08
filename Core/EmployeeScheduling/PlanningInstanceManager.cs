using System;
using System.IO;
using Newtonsoft.Json;

namespace DAA_P03.Core.EmployeeScheduling
{
    /// <summary>
    /// Gestor para cargar instancias del problema de planificación desde archivos JSON.
    /// </summary>
    public class PlanningInstanceManager
    {
        /// <summary>
        /// Carga una instancia desde un archivo JSON.
        /// </summary>
        /// <param name="rutaArchivo">Ruta al archivo JSON.</param>
        /// <returns>La instancia cargada.</returns>
        public static InstancePlanning CargarDesdeJSON(string rutaArchivo)
        {
            if (!File.Exists(rutaArchivo))
                throw new FileNotFoundException($"El archivo {rutaArchivo} no existe.");

            try
            {
                string contenido = File.ReadAllText(rutaArchivo);
                var instancia = JsonConvert.DeserializeObject<InstancePlanning>(contenido);

                if (instancia == null)
                    throw new InvalidOperationException("No se pudo deserializar la instancia.");

                if (!instancia.EsValida())
                    throw new InvalidOperationException("La instancia cargada no es válida.");

                return instancia;
            }
            catch (JsonException ex)
            {
                throw new InvalidOperationException($"Error al parsear JSON: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error al cargar la instancia: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Guarda una instancia a un archivo JSON.
        /// </summary>
        /// <param name="instancia">La instancia a guardar.</param>
        /// <param name="rutaArchivo">Ruta del archivo de salida.</param>
        public static void GuardarAJSON(InstancePlanning instancia, string rutaArchivo)
        {
            if (instancia == null)
                throw new ArgumentNullException(nameof(instancia));

            try
            {
                var settings = new JsonSerializerSettings
                {
                    Formatting = Formatting.Indented
                };

                string contenido = JsonConvert.SerializeObject(instancia, settings);
                File.WriteAllText(rutaArchivo, contenido);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error al guardar la instancia: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Crea una instancia de prueba pequeña para testing.
        /// </summary>
        /// <returns>Una instancia de prueba.</returns>
        public static InstancePlanning CrearInstanciaPrueba()
        {
            var instancia = new InstancePlanning
            {
                Empleados = new System.Collections.Generic.List<string> 
                { 
                    "Alice", "Bob", "Carlos", "Diana", "Elena" 
                },
                NumDias = 3,
                Turnos = new System.Collections.Generic.List<string> 
                { 
                    "Mañana", "Tarde", "Noche" 
                },
                Satisfaccion = new int[5, 3, 3]
                {
                    // Alice
                    {
                        { 8, 5, 2 },  // Día 1: Mañana=8, Tarde=5, Noche=2
                        { 7, 6, 3 },  // Día 2
                        { 9, 4, 1 }   // Día 3
                    },
                    // Bob
                    {
                        { 5, 7, 8 },
                        { 4, 8, 9 },
                        { 3, 9, 10 }
                    },
                    // Carlos
                    {
                        { 7, 7, 6 },
                        { 8, 7, 5 },
                        { 7, 8, 6 }
                    },
                    // Diana
                    {
                        { 3, 9, 7 },
                        { 2, 10, 6 },
                        { 3, 9, 8 }
                    },
                    // Elena
                    {
                        { 6, 6, 8 },
                        { 5, 7, 9 },
                        { 6, 6, 7 }
                    }
                },
                CoberturaMínima = new int[3, 3]
                {
                    { 2, 2, 1 },  // Día 1: Mañana=2, Tarde=2, Noche=1
                    { 2, 1, 2 },  // Día 2
                    { 1, 2, 2 }   // Día 3
                },
                DiasDescanso = new int[5] { 1, 1, 1, 1, 1 }
            };

            return instancia;
        }
    }
}
