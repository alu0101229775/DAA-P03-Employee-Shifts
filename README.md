# PRÁCTICA 3: DIVIDE Y VENCERÁS

- **Autores**: Marcos Llinares Montes y Gerard
- **Asignatura**: Diseño y Análisis de Algoritmos (DAA)
- **Fecha**: Marzo 2026

## Configuración de Versión .NET

Este proyecto requiere **.NET 8.0**. Si tienes una versión diferente instalada:

1. Verifica tu versión instalada: `dotnet --list-sdks`
2. Edita el archivo `EmployeeShifts.csproj`
3. Cambia `<TargetFramework>net8.0</TargetFramework>` por tu versión (ej: `net6.0`, `net7.0`, etc.)
4. Ejecuta: `dotnet build` para compilar y `dotnet run` para ejecutar

---

## Descripción General

Consta de dos partes principales:

- **PARTE 1**: Implementación del patrón Template para algoritmos de Divide y Vencerás, con dos implementaciones específicas: MergeSort y QuickSort.
- **PARTE 2**: Modelado y resolución del problema real de planificación de empleados usando el patrón Template de la Parte 1.

---

## ESTRUCTURA DEL PROYECTO

```
DAA-P03-Employee-Shifts/
│
├── Parte1_Ordenamiento/
│   ├── Base/
│   │   ├── Algoritmo.cs                    # Clase base abstracta de algoritmos
│   │   ├── AlgoritmoDyV.cs                 # Plantilla Template D&C
│   │   ├── Instancia.cs                    # Clase base abstracta de instancias
│   │   └── Solucion.cs                     # Clase base abstracta de soluciones
│   │
│   ├── Algoritmos/
│   │   ├── MergeSort.cs                    # Implementación MergeSort
│   │   └── QuickSort.cs                    # Implementación QuickSort
│   │
│   └── Modelo/
│       ├── InstanciaOrdenamiento.cs        # Instancia genérica (generación)
│       ├── InstanciaMergeSort.cs           # Instancia con vector compartido
│       ├── InstanciaQuickSort.cs           # Instancia con vector compartido
│       ├── SolucionMergeSort.cs            # Solución MergeSort
│       └── SolucionQuickSort.cs            # Solución QuickSort
│
├── Parte2_Planificacion/
│   ├── Algoritmos/
│   │   └── PlanificacionDivideYVenceras.cs # Algoritmo D&C planificación
│   │
│   ├── Dominio/
│   │   ├── Empleado.cs                     # Modelo empleado
│   │   └── Turno.cs                        # Modelo turno
│   │
│   ├── Modelo/
│   │   ├── InstanciaPlanificacion.cs       # Instancia problema planificación
│   │   └── SolucionPlanificacion.cs        # Solución problema planificación
│   │
│   └── Servicios/
│       └── GestorInstanciasPlanificacion.cs # Gestor carga/guardado JSON
│
├── ejemplos/
│   ├── instancia_prueba_pequeña.json       # Ejemplo JSON pequeño
│   └── instance_horizon*.json              # Instancias de prueba variadas
│
├── Program.cs                              # Aplicación principal (3 modos)
├── EmployeeShifts.csproj                   # Configuración del proyecto
├── README.md                               # Este archivo
├── ARCHITECTURE.md                         # Arquitectura del sistema
├── INFORME_DISEÑO.md                       # Informe detallado
└── USAGE_GUIDE.md                          # Guía de uso
```

---

## PARTE 1: ALGORITMOS DE ORDENAMIENTO

### Características

✓ **Patrón Template**: `AlgoritmoDyV` define estructura común
✓ **Tipado Fuerte**: Uso de clases abstractas `Instancia`/`Solucion` (polimorfismo)
✓ **Vector Compartido**: Instancias usan rangos sobre mismo array (eficiencia)
✓ **Dos Implementaciones**: MergeSort y QuickSort in-place
✓ **Medición de Tiempos**: Uso de Stopwatch para precisión
✓ **Tres Modos de Ejecución**:
  - Modo Comparativo: MergeSort vs QuickSort
  - Modo Normal: Un algoritmo, múltiples tamaños
  - Modo Debug: Instancia y solución detalladas

### Jerarquía de Clases

#### Clases Base

**`Instancia` (Abstracta)**
```csharp
public abstract class Instancia
{
    public abstract int Tamaño { get; }
    public abstract Instancia ObtenerCopia();
    public abstract string ToString();
}
```

**`Solucion` (Abstracta)**
```csharp
public abstract class Solucion
{
    public abstract bool EsValida { get; }
    public abstract string ObtenerInfo();
    public abstract string ToString();
}
```

**`Algoritmo` (Abstracta)**
```csharp
public abstract class Algoritmo
{
    public string Nombre { get; protected set; }
    public string Descripcion { get; protected set; }
    public abstract Solucion Resolver(Instancia instancia);
    public virtual Solucion ResolverConTiempo(Instancia instancia);
    public long TiempoEjecucion { get; protected set; }
    public long NumOperaciones { get; protected set; }
}
```

**`AlgoritmoDyV` (Abstracta - Template)**
```csharp
public abstract class AlgoritmoDyV : Algoritmo
{
    protected abstract bool EsPequenio(Instancia instancia);
    protected abstract Solucion ResolverPequenio(Instancia instancia);
    protected abstract Instancia[] Dividir(Instancia instancia);
    protected abstract Solucion Combinar(Solucion sol1, Solucion sol2);
    
    public override Solucion Resolver(Instancia instancia)
    {
        if (EsPequenio(instancia))
            return ResolverPequenio(instancia);
        
        Instancia[] subproblemas = Dividir(instancia);
        Solucion sol1 = Resolver(subproblemas[0]);
        Solucion sol2 = Resolver(subproblemas[1]);
        return Combinar(sol1, sol2);
    }
}
```

#### Implementaciones Concretas

**`MergeSort`**
- Complejidad: O(n log n) garantizado
- Estable: Sí
- Espacio: O(n) auxiliar en Combinar
- Estrategia Divide: Mitad exacta por índice medio
- Estrategia Conquista: Mezcla ordenada in-place

**`QuickSort`**
- Complejidad: O(n log n) promedio, O(n²) peor caso
- Estable: No
- Espacio: O(log n) pila recursión
- Estrategia Divide: Partición two-pointer (pivote = último)
- Estrategia Conquista: Ordenamiento in-place

### Ejemplo de Uso (PARTE 1)

```csharp
// Generar instancia base
var instanciaBase = new InstanciaOrdenamiento(1000, seed: 42);

// Crear instancia específica para MergeSort
var instanciaMerge = new InstanciaMergeSort(instanciaBase.Numeros);
var mergeSort = new MergeSort();
Solucion solucionMerge = mergeSort.ResolverConTiempo(instanciaMerge);

// Crear instancia específica para QuickSort
var instanciaQuick = new InstanciaQuickSort(instanciaBase.Numeros);
var quickSort = new QuickSort();
Solucion solucionQuick = quickSort.ResolverConTiempo(instanciaQuick);

// Verificar y comparar
Console.WriteLine($"MergeSort válido: {solucionMerge.EsValida}");
Console.WriteLine($"MergeSort tiempo: {mergeSort.TiempoEjecucion}ms");
Console.WriteLine($"QuickSort tiempo: {quickSort.TiempoEjecucion}ms");
```

---

## PARTE 2: PLANIFICACIÓN DE EMPLEADOS

### Descripción del Problema

**Objetivo:** Asignar turnos a empleados maximizando satisfacción y cumpliendo restricciones.

**Datos de Entrada:**
- E: Conjunto de empleados
- D: Número de días a planificar (horizonte temporal)
- T: Conjunto de turnos por día
- Satisfaccion[e][d][t]: Satisfacción del empleado e en turno t del día d (0-10)
- CoberturaMínima[d][t]: Empleados mínimos requeridos en turno t del día d
- DiasDescanso[e]: Días de descanso requeridos para empleado e

**Restricciones:**
1. Cobertura mínima en cada turno (restricción dura)
2. Respetar días de descanso de cada empleado
3. Un empleado trabaja máximo 1 turno por día
4. No asignar más empleados de los disponibles

**Función Objetivo:**
```
f(x) = Satisfacción_Total + (Turnos_Cubiertos * 100)
```

### Algoritmo Divide y Vencerás

**`PlanificacionDivideYVenceras`**

Extiende `AlgoritmoDyV` para resolver planificación:

**Caso Base (1 Día):**
- Aplica algoritmo voraz por turno
- Ordena empleados por satisfacción descendente
- Asigna los más satisfechos hasta cubrir mínimo
- Complejidad: O(E * T * log E)

**División (D > 1):**
- Divide horizonte temporal en mitades
- Crea subinstancias [0, mitad-1] y [mitad, D-1]
- Preserva datos de empleados y turnos

**Conquista:**
- Resuelve recursivamente cada subinstancia
- T(n) = 2·T(n/2) + O(combine)

**Combinación:**
- Concatena planes de ambas mitades
- Recalcula función objetivo global
- Valida restricciones de descanso

**Complejidad Total:** O(D · E · T · log D · log E)

---

## MODOS DE EJECUCIÓN

### 1. Modo Comparativo (Parte 1)

Compara MergeSort vs QuickSort side-by-side:

- Tamaños: 100, 500, 1K, 5K, 10K, 50K, 100K
- Repeticiones configurables (default: 5-10)
- Tabla comparativa con ganador por tamaño

**Ejemplo de Salida:**
```
┌────────────┬──────────────────┬──────────────────┬────────────┐
│   Tamaño   │   MergeSort (ms) │  QuickSort (ms)  │   Ganador  │
├────────────┼──────────────────┼──────────────────┼────────────┤
│        100 │            0.052 │            0.031 │  QuickSort │
│        500 │            0.284 │            0.178 │  QuickSort │
│       1000 │            0.598 │            0.365 │  QuickSort │
└────────────┴──────────────────┴──────────────────┴────────────┘
```

### 2. Modo Normal (Parte 1)

Ejecuta un algoritmo con múltiples tamaños:

- Selecciona MergeSort o QuickSort
- Ejecuta con 7 tamaños diferentes
- Muestra tiempos promediados
- Útil para análisis individual

### 3. Modo Debug (Parte 1)

Muestra instancia y solución completas:

- Selecciona algoritmo y tamaño
- Muestra array original
- Muestra array ordenado
- Muestra estadísticas (comparaciones, tiempo)
- Valida que la solución sea correcta
**Recomendación:** Usar tamaños 10-20 elementos para ver datos completos sin scroll excesivo.

### 4. Modo Planificación (Parte 2)

**Nota:** Actualmente en desarrollo. Menú preparado para futura implementación.

---

### Navegación del Menú

1. **Menú Principal**
   - Opción 1: Parte 1 (Ordenamiento)
   - Opción 2: Parte 2 (Planificación - placeholder)
   - Opción 0: Salir

2. **Submenú Parte 1**
   - Opción 1: Modo Comparativo
   - Opción 2: Modo Normal
   - Opción 3: Modo Debug
   - Opción 0: Volver

