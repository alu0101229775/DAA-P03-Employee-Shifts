using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json.Linq;

namespace DAA_P03.Core.EmployeeScheduling
{
    /// <summary>
    /// Gestor para cargar instancias de planificación desde archivos JSON.
    /// Maneja la deserialización y construcción de matrices internas.
    /// </summary>
    public class PlanningInstanceManager
    {
        /// <summary>
        /// Carga una instancia desde un archivo JSON.
        /// Convierte la estructura JSON a matrices internas (Satisfacción, CoberturaMínima).
        /// </summary>
        public static InstancePlanning CargarDesdeJSON(string rutaArchivo)
        {
            if (!File.Exists(rutaArchivo))
                throw new FileNotFoundException($"No existe: {rutaArchivo}");

            try
            {
                string contenido = File.ReadAllText(rutaArchivo);
                var json = JObject.Parse(contenido);

                // Cargar datos básicos
                var instancia = new InstancePlanning();
                
                // Empleados
                instancia.Empleados = new List<Employee>();
                var empleadosArray = json["employees"] as JArray ?? new JArray();
                foreach (var empJson in empleadosArray)
                {
                    string name = empJson["name"]?.Value<string>() ?? "Sin nombre";
                    int daysOff = empJson["daysOff"]?.Value<int>() ?? 1;
                    instancia.Empleados.Add(new Employee(name, daysOff));
                }

                // Turnos
                instancia.Turnos = new List<string>();
                var turnosArray = json["shifts"] as JArray ?? new JArray();
                foreach (var turnoJson in turnosArray)
                {
                    instancia.Turnos.Add(turnoJson.Value<string>());
                }

                // Horizonte de planificación
                instancia.NumDias = json["planningHorizon"]?.Value<int>() ?? 7;

                // Satisfacción (convertir array plano a matriz 3D)
                var satisfaccionArray = json["satisfaction"] as JArray ?? new JArray();
                instancia.Satisfaccion = new int[instancia.NumEmpleados, instancia.NumDias, instancia.NumTurnos];
                
                int index = 0;
                for (int e = 0; e < instancia.NumEmpleados; e++)
                {
                    for (int d = 0; d < instancia.NumDias; d++)
                    {
                        for (int t = 0; t < instancia.NumTurnos; t++)
                        {
                            if (index < satisfaccionArray.Count)
                            {
                                int valor = satisfaccionArray[index]["value"]?.Value<int>() ?? 0;
                                instancia.Satisfaccion[e, d, t] = valor;
                            }
                            index++;
                        }
                    }
                }

                // Cobertura mínima (por defecto 1, poder ser especificado en requiredEmployees o como estructura)
                instancia.CoberturaMínima = new int[instancia.NumDias, instancia.NumTurnos];
                for (int d = 0; d < instancia.NumDias; d++)
                    for (int t = 0; t < instancia.NumTurnos; t++)
                        instancia.CoberturaMínima[d, t] = 1; // Por defecto 1 empleado por turno

                if (!instancia.EsValida())
                    throw new InvalidOperationException("La instancia cargada no es válida.");

                return instancia;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error al cargar JSON: {ex.Message}", ex);
            }
        }
    }
}
