using System;
using System.IO;
using Newtonsoft.Json;

namespace DAA_P03.Core.EmployeeScheduling
{
    /// <summary>
    /// Gestor para cargar instancias de planificación desde archivos JSON.
    /// </summary>
    public class PlanningInstanceManager
    {
        /// <summary>
        /// Carga una instancia desde un archivo JSON.
        /// </summary>
        public static InstancePlanning CargarDesdeJSON(string rutaArchivo)
        {
            if (!File.Exists(rutaArchivo))
                throw new FileNotFoundException($"No existe: {rutaArchivo}");

            try
            {
                string contenido = File.ReadAllText(rutaArchivo);
                var instancia = JsonConvert.DeserializeObject<InstancePlanning>(contenido);

                if (instancia == null)
                    throw new InvalidOperationException("No se deserializó correctamente.");

                if (!instancia.EsValida())
                    throw new InvalidOperationException("La instancia cargada no es válida.");

                return instancia;
            }
            catch (JsonException ex)
            {
                throw new InvalidOperationException($"Error JSON: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Guarda una instancia a un archivo JSON.
        /// </summary>
        public static void GuardarAJSON(InstancePlanning instancia, string rutaArchivo)
        {
            if (instancia == null)
                throw new ArgumentNullException(nameof(instancia));

            try
            {
                var settings = new JsonSerializerSettings { Formatting = Formatting.Indented };
                string contenido = JsonConvert.SerializeObject(instancia, settings);
                File.WriteAllText(rutaArchivo, contenido);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error al guardar: {ex.Message}", ex);
            }
        }
    }
}
