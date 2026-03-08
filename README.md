# PRÁCTICA 3: DIVIDE Y VENCERÁS

## Descripción General

Este proyecto implementa una solución completa para la Práctica 3 de Algoritmos (DAA2526) sobre Divide y Vencerás. Consta de dos partes principales:

- **PARTE 1**: Implementación del patrón Template para algoritmos de Divide y Vencerás, con dos implementaciones específicas: MergeSort y QuickSort.
- **PARTE 2**: Modelado y resolución del problema real de planificación de empleados usando el patrón Template de la Parte 1.

El proyecto está completamente desarrollado en **C#** utilizando .NET 6.0, con énfasis en:
- Programación orientada a objetos
- Patrón Template Method
- Algoritmos eficientes de Divide y Vencerás
- Validaciones y manejo de errores robustos

---

## ESTRUCTURA DEL PROYECTO

```
DAA-P03-Employee-Shifts/
│
├── Core/
│   ├── Algorithms/
│   │   ├── Algoritmo.cs                    # Clase base de algoritmos
│   │   ├── DivideYVenceras.cs              # Plantilla (Template) D&C
│   │   └── Sorting/
│   │       ├── MergeSort.cs                # Implementación MergeSort
│   │       └── QuickSort.cs                # Implementación QuickSort
│   │
│   ├── Data/
│   │   ├── InstanceSorting.cs              # Instancia problema ordenamiento
│   │   └── SolutionSorting.cs              # Solución problema ordenamiento
│   │
│   └── EmployeeScheduling/
│       ├── InstancePlanning.cs             # Instancia problema planificación
│       ├── SolutionPlanning.cs             # Solución problema planificación
│       ├── PlanificacionDivideYVenceras.cs # Algoritmo D&C para planificación
│       └── PlanningInstanceManager.cs      # Gestor de cargas JSON
│
├── ejemplos/
│   └── instancia_prueba_pequeña.json       # Ejemplo de instancia JSON
│
├── Program.cs                              # Aplicación principal
├── EmployeeShifts.csproj                  # Configuración del proyecto
├── README.md                               # Este archivo
├── INFORME_DISEÑO.md                      # Informe detallado
└── .gitignore                             # Configuración Git
```

---

## PARTE 1: ALGORITMOS DE ORDENAMIENTO

### Características

✓ **Patrón Template**: Clase abstracta `DivideYVenceras` define la estructura
✓ **Dos Implementaciones**: MergeSort y QuickSort extienden el template
✓ **Medición de Tiempos**: Uso de Stopwatch para precisión
✓ **Conteo de Operaciones**: Registro de comparaciones e intercambios
✓ **Dos Modos de Ejecución**:
  - Modo Normal: Compara rendimiento con múltiples tamaños
  - Modo Debug: Muestra instancia y solución para un tamaño específico

### Clases Principales

#### `Algoritmo` (Clase Base)
```csharp
public abstract class Algoritmo
{
    public string Nombre { get; protected set; }
    public abstract object Resolver(object instancia);
    public virtual object ResolverConTiempo(object instancia);
    public long TiempoEjecucion { get; protected set; }
    public long NumOperaciones { get; protected set; }
}
```

#### `DivideYVenceras` (Plantilla Template)
```csharp
public abstract class DivideYVenceras : Algoritmo
{
    protected abstract bool EsPequenio(object instancia);
    protected abstract object ResolverPequenio(object instancia);
    protected abstract object[] Dividir(object instancia);
    protected abstract object Combinar(object solucion1, object solucion2);
    
    protected object ResolverDivideYVenceras(object instancia)
    {
        if (EsPequenio(instancia))
            return ResolverPequenio(instancia);
        else
        {
            object[] subproblemas = Dividir(instancia);
            object sol1 = ResolverDivideYVenceras(subproblemas[0]);
            object sol2 = ResolverDivideYVenceras(subproblemas[1]);
            return Combinar(sol1, sol2);
        }
    }
}
```

#### `MergeSort`
- Complejidad: O(n log n) siempre
- Estable: Sí
- In-Place: No (requiere O(n) espacio)
- Estrategia Divide: Dividir en dos mitades iguales
- Estrategia Conquista: Mezcla ordenada de dos arrays

#### `QuickSort`
- Complejidad: O(n log n) promedio, O(n²) peor caso
- Estable: No
- In-Place: Sí (O(log n) stack)
- Estrategia Divide: Partición alrededor de pivote (último elemento)
- Estrategia Conquista: Concatenación de particiones

### Ejemplo de Uso (PARTE 1)

```csharp
// Crear una instancia de ordenamiento
var instancia = new InstanceSorting(1000, seed: 42);

// Usar MergeSort
var mergeSort = new MergeSort();
var solucion = mergeSort.ResolverConTiempo(instancia);

// Usar QuickSort
var quickSort = new QuickSort();
var solucion2 = quickSort.ResolverConTiempo(instancia);

// Verificar validez
Console.WriteLine($"MergeSort válido: {(solucion as SolutionSorting).EsValida}");
Console.WriteLine($"Tiempo: {mergeSort.TiempoEjecucion}ms");
```

---

## PARTE 2: PLANIFICACIÓN DE EMPLEADOS

### Descripción del Problema

**Objetivo:** Planificar la asignación de turno a empleados de forma óptima.

**Datos de Entrada:**
- E: Conjunto de empleados
- D: Número de días a planificar
- T: Conjunto de turnos por día
- A[e][d][t]: Satisfacción del empleado e si hace turno t en día d
- B[d][t]: Cobertura mínima para turno t en día d
- C[e]: Días de descanso requeridos para empleado e

**Restricciones:**
1. Se debe cubrir la cobertura mínima en cada turno (crítico)
2. Respetardescansos requeridos para cada empleado
3. Un empleado no puede tener dos turnos en el mismo día

**Función Objetivo:**
```
f(x) = SUMA(satisfaccion) + SUMA(turnos_cubiertos) * 100

Importancia relativa: 100:1 a favor de cobertura
```

### Clases del Problema

#### `InstancePlanning`
```csharp
public class InstancePlanning
{
    public List<string> Empleados { get; set; }
    public int NumDias { get; set; }
    public List<string> Turnos { get; set; }
    public int[,,] Satisfaccion { get; set; }      // A[e][d][t]
    public int[,] CoberturaMínima { get; set; }    // B[d][t]
    public int[] DiasDescanso { get; set; }        // C[e]
    
    public InstancePlanning ObtenerSubinstancia(int diaInicio, int diaFin);
    public bool EsValida();
}
```

#### `SolutionPlanning`
```csharp
public class SolutionPlanning
{
    public List<int>[][] Plan { get; set; }        // Asignaciones
    public double SatisfaccionTotal { get; set; }
    public int TurnosCubiertos { get; set; }
    public int TurnosNoCubiertos { get; set; }
    public double FuncionObjetivo { get; set; }
    public bool RestriccionesValidas { get; set; }
    
    public string ObtenerRepresentacionTabla(List<string> empleados, List<string> turnos);
    public SolutionPlanning Combinar(SolutionPlanning otra);
}
```

### Algoritmo Divide y Vencerás para Planificación

#### `PlanificacionDivideYVenceras`

Extiende `DivideYVenceras` para resolver el problema:

**Caso Base (1 Día):**
```
Algoritmo Voraz:
  FOR cada turno (d, t):
    1. Ordenar empleados por satisfacción DESC
    2. Asignar top B[d][t] empleados
    3. Contar como "cubierto"
```
Complejidad: O(e log e) por turno

**División:**
- Divide días en dos mitades
- Mantiene instancias válidas

**Conquista:**
- Resuelve recursivamente cada mitad
- T(n) = 2*T(n/2) + O(n*e*t)

**Combinación:**
- Concatena planes manteniendo validez
- Recalcula función objetivo
- Optimiza respetando descansos

**Complejidad Total:** O(n * e * t * log n)

### Carga de Instancias desde JSON

```csharp
// Cargar desde JSON
var instancia = PlanningInstanceManager.CargarDesdeJSON("instancia.json");

// Crear instancia de prueba
var prueba = PlanningInstanceManager.CrearInstanciaPrueba();

// Guardar a JSON
PlanningInstanceManager.GuardarAJSON(instancia, "salida.json");
```

**Formato JSON:**
```json
{
  "empleados": ["Alice", "Bob", "Carlos"],
  "dias": 5,
  "turnos": ["Mañana", "Tarde", "Noche"],
  "satisfaccion": [[[8, 5, 2], ...], ...],
  "coberturaminima": [[2, 2, 1], ...],
  "diasdescanso": [1, 1, 1]
}
```

---

## MODOS DE EJECUCIÓN

### Modo Normal (Ordenamiento)

Compara el rendimiento de MergeSort y QuickSort:

1. Genera instancias de tamaños: 100, 500, 1K, 5K, 10K, 50K, 100K
2. Ejecuta cada algoritmo 3 veces por tamaño
3. Calcula promedio de tiempos
4. Muestra tabla comparativa

**Ejemplo de Salida:**
```
═════════════════════════════════════════════════════════════════════
TABLA COMPARATIVA DE TIEMPOS DE EJECUCIÓN
═════════════════════════════════════════════════════════════════════
Tamaño       MergeSort (ms)   QuickSort (ms)   Diferencia (ms)
─────────────────────────────────────────────────────────────────────
100          0.05             0.03             0.02
500          0.18             0.08             0.10
1000         0.35             0.15             0.20
...
═════════════════════════════════════════════════════════════════════
```

### Modo Debug (Ordenamiento)

Muestra instancia y solución completa:

1. Selecciona algoritmo (MergeSort / QuickSort)
2. Selecciona tamaño
3. Genera instancia aleatoria
4. Ejecuta y muestra:
   - Array original (primeros 5 y últimos 5 elementos)
   - Array ordenado (primeros 5 y últimos 5 elementos)
   - Número de comparaciones e intercambios
   - Validez de la solución
   - Tiempo de ejecución

### Modo Planificación

1. Selecciona modo:
   - Instancia de prueba incluida
   - Cargar desde JSON
   - Generar y exportar JSON
2. Ejecuta algoritmo Divide y Vencerás
3. Muestra solución en tabla formateada con:
   - Asignaciones por día y turno
   - Cálculo de función objetivo
   - Días de descanso por empleado
   - Validez de restricciones

---

## COMPILACIÓN Y EJECUCIÓN

### Requisitos

- .NET SDK 6.0 o superior
- Visual Studio, Visual Studio Code, o similar

### Compilación

```bash
cd c:\Users\Gerard\Documents\DAA2526\DAA-P03-Employee-Shifts
dotnet restore      # Descargar dependencias
dotnet build        # Compilar
```

### Ejecución

```bash
dotnet run          # Ejecutar aplicación interactiva
```

### Compilación Release (Optimizada)

```bash
dotnet build -c Release
dotnet run -c Release
```

---

## DEPENDENCIAS

- **Newtonsoft.Json 13.0.3**: Serialización JSON para instancias de planificación

```xml
<ItemGroup>
    <PackageReference Include="Newtonsoft.Json" Version="13.0.3" />
</ItemGroup>
```

---

## RESULTADOS ESPERADOS

### Para Ordenamiento

MergeSort y QuickSort producen arrays ordenados idénticos pero con diferentes tiempos:
- **MergeSort**: ~2.6x más lento pero consistente
- **QuickSort**: Más rápido en práctica, mejor localidad de memoria

### Para Planificación

El algoritmo D&C produce soluciones válidas que:
- Respetan cobertura mínima en la mayoría de turnos
- Maximizan satisfacción sujeta a restricciones
- Se ejecutan en tiempo O(n * e * t * log n)

---

## ARCHIVO INFORME

Ver **INFORME_DISEÑO.md** para:
- Decisiones de diseño detalladas
- Análisis completo de complejidad
- Resultados experimentales
- Recomendaciones futuras
- Referencias académicas

---

## NOTAS IMPORTANTES

1. **Validaciones**: Todas las entradas se validan en tiempo de ejecución
2. **Manejo de Errores**: Excepciones específicas para cada tipo de error
3. **Extensibilidad**: Fácil agregar nuevos algoritmos D&C
4. **Documentación**: Código completamente comentado en español

---

## LICENCIA Y AUTORÍA

- **Autor**: Gerard
- **Asignatura**: Diseño y Análisis de Algoritmos (DAA2526)
- **Institución**: Universidad Politécnica de Valencia (UPV)
- **Fecha**: 8 de Marzo de 2026
- **Versión**: 1.0

---

## CONTACTO Y SOPORTE

Para preguntas sobre la implementación, consulte:
- Archivos de código fuente (comentados)
- INFORME_DISEÑO.md para decisiones de diseño
- ARCHITECTURE.md para diagramas de arquitectura

