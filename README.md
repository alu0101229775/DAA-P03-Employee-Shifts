# PRÁCTICA 3: DIVIDE Y VENCERÁS

- **Autores**: Marcos Llinares Montes & Gerard Caramazza Vilá
- **Asignatura**: Diseño y Análisis de Algoritmos (DAA)
- **Fecha**: 10 Marzo 2026

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

## MODOS DE EJECUCIÓN

1. Modo Comparativo (Parte 1)
2. Modo Normal (Parte 1)
3. Modo Debug (Parte 1)
4. Modo Planificación (Parte 2)
