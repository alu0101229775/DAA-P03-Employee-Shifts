# INFORME: PLANIFICACIÓN DE EMPLEADOS CON DIVIDE Y VENCERÁS

**Autores**: Marcos Llinares Montes & Gerard Caramazza Vilá  
**Asignatura**: Diseño y Análisis de Algoritmos (DAA)  
**Fecha**: 10 de Marzo de 2026

---

## 1. DECISIONES DE MODELADO

### 1.1 Estructura de Datos

#### **InstanciaPlanificacion**
```
Empleados: List<Empleado>
  ├─ Nombre: string
  └─ DiasDescanso: int (restricción de descanso mínimo)

Turnos: List<string> (ej: ["T0", "T1", "T2"])

NumDias: int (horizonte de planificación)

Satisfaccion[día, empleado, turno]: int[,,]
  └─ Valor de satisfacción del empleado en ese turno-día

CoberturaMínima[día, turno]: int[,]
  └─ Número mínimo de empleados requeridos
```

**Justificación**:
- Matriz 3D de satisfacción permite acceso O(1) a valores individuales
- CoberturaMínima 2D separada simplifica restricciones de cobertura
- Estructura permite representar cualquier tamaño de instancia

#### **SolucionPlanificacion**
```
Plan[día][turno]: List<int>[][]
  └─ Índices de empleados asignados a cada (día, turno)

SatisfaccionTotal: double
TurnosCubiertos: int
FuncionObjetivo: double
  = SatisfaccionTotal + (TurnosCubiertos × 100)
```

**Justificación**:
- Lista de listas permite inserción/eliminación flexible
- Función objetivo combina dos criterios: satisfacción + cobertura 
- Peso de 100 para cobertura asegura que un turno no cubierto >= que satisfacción de 1 empleado

---

### 1.2 Algoritmo Divide y Vencerás

#### **Estrategia de División**
```
ENTRADA: Instancia con N días
DIVISIÓN: Partir días por la mitad → [0, N/2-1] y [N/2, N-1]
CONQUISTA: Resolver recursivamente cada mitad
COMBINACIÓN: Concatenar soluciones + búsqueda local 2-opt para optimizar
```

**Justificación**:
- División por días es natural: cada día es independiente en caso base
- Combinar concatena los planes y aplica búsqueda local para mejorar la satisfacción global

#### **Caso Base - Voraz Mejorado**
```
PARA cada día (1 día):
  PARA cada turno:
    1. Listar empleados válidos (respetan días de descanso)
    2. Ordenar por satisfacción DESC, días de descanso DESC
    3. Asignar top-K empleados hasta cobertura mínima
    4. Sumar satisfacción
```

**Justificación**:
- Voraz simple →  por turno
- Considera restricción de descanso → Aumenta validez de soluciones
- Priorizar días de descanso pendientes → Equilibra carga

---

### 1.3 Optimización: Búsqueda Local Post-Procesamiento

#### **Algoritmo 2-Opt**
```
REPETIR hasta convergencia (máx N iteraciones):
  PARA cada turno-día (d1, t1):
    PARA cada turno-día (d2, t2):
      PARA cada par de empleados (emp1, emp2):
        calcularMejora = Sat[d1,emp2,t1] + Sat[d2,emp1,t2]
                       - Sat[d1,emp1,t1] - Sat[d2,emp2,t2]
        SI calcularMejora > 0.001:
          intercambiar(emp1, emp2)
          mejora = true
```

**Justificación**:
- Post-procesamiento no altera estructura D&C
- Búsqueda local 2-opt: vecindario O(D²·T²) pero limitado a N iteraciones
- Converge cuando no hay mejoras → Óptimo local
- Umbral 0.001 evita oscilaciones por errores de redondeo

---

## 2. EXPERIMENTACIÓN CON INSTANCIAS

### 2.1 Conjunto de Prueba Disponible

| Instancia | Días | Empleados | Turnos | Tamaño | Complejidad |
|-----------|------|-----------|--------|--------|-------------|
| instance_horizon7_employees5_shifts3 | 7 | 5 | 3 | PEQUEÑA | 105 |
| instance_horizon7_employees10_shifts6 | 7 | 10 | 6 | PEQUEÑA | 420 |
| instance_horizon14_employees10_shifts6 | 14 | 10 | 6 | MEDIANA | 840 |
| instance_horizon14_employees20_shifts10 | 14 | 20 | 10 | MEDIANA | 2800 |
| instance_horizon30_employees20_shifts10 | 30 | 20 | 10 | GRANDE | 6000 |
| instance_horizon30_employees30_shifts20 | 30 | 30 | 20 | MUY GRANDE | 18000 |

**Complejidad**: N_días × N_empleados × N_turnos

### 2.2 Resultados de Ejecución

#### **Prueba 1: Carga de Instancias**
```
✓ Cargadas exitosamente: 12/12 archivos JSON
  ├─ Validación estructura: 100%
  ├─ Matrices satisfacción: Correctamente mapeadas
  └─ Cobertura mínima: Inicializada o cargada
```

#### **Prueba 2: Instancia Pequeña (7 días × 5 empleados × 3 turnos)**
```
instancia_horizon7_employees5_shifts3_000.json:
  ├─ Tiempo ejecución: 8 ms
  ├─ Operaciones: 56
  ├─ Función Objetivo: 2257.00
  ├─ Turnos Cubiertos: 21/21 (100%)
  └─ Satisfacción Total: 157.00

instancia_horizon7_employees5_shifts3_001.json:
  ├─ Tiempo ejecución: 0 ms
  ├─ Operaciones: 50
  ├─ Función Objetivo: 2265.00
  ├─ Turnos Cubiertos: 21/21 (100%)
  └─ Satisfacción Total: 165.00
```

**Análisis**:
- Cobertura perfecta (100%) en ambas instancias
- Operaciones bajas (~50-56) gracias a la profundidad mínima de recursión (log₂ 7 ≈ 3)
- Tiempo negligible → Excelente para interactividad

#### **Prueba 3: Instancia Mediana (14 días × 10 empleados × 6 turnos)**
```
instance_horizon14_employees10_shifts6_001.json:
  ├─ Tiempo ejecución: 23 ms
  ├─ Operaciones: 282
  ├─ Función Objetivo: 9096.00
  ├─ Turnos Cubiertos: 84/84 (100%)
  └─ Satisfacción Total: 696.00

instance_horizon14_employees10_shifts6_000.json:
  ├─ Tiempo ejecución: 8 ms
  ├─ Operaciones: 302
  ├─ Función Objetivo: 9096.00
  ├─ Turnos Cubiertos: 84/84 (100%)
  └─ Satisfacción Total: 696.00
```

**Análisis**:
- Cobertura perfecta (100%) en ambas instancias
- Operaciones en rango ~280-300, coherente con profundidad log₂ 14 ≈ 4 niveles
- Misma función objetivo en ambas variantes: algoritmo determinista para esta combinación

#### **Prueba 4: Instancia Muy Grande (30 días × 30 empleados × 20 turnos)**
```
instance_horizon30_employees30_shifts20_000.json:
  ├─ Tiempo ejecución: 6808 ms
  ├─ Operaciones: 2948
  ├─ Función Objetivo: 65313.00
  ├─ Turnos Cubiertos: 600/600 (100%)
  └─ Satisfacción Total: 5313.00

instance_horizon30_employees30_shifts20_001.json:
  ├─ Tiempo ejecución: 6979 ms
  ├─ Operaciones: 2975
  ├─ Función Objetivo: 65322.00
  ├─ Turnos Cubiertos: 600/600 (100%)
  └─ Satisfacción Total: 5322.00
```
**Análisis**:
- Cobertura perfecta (100%) en ambas instancias
- Operaciones ~2950-2975: el crecimiento respecto a instancias medianas refleja el mayor espacio de búsqueda local
- Tiempo de ~7 segundos: dominado por la búsqueda local 2-opt sobre 30×20 = 600 turnos a planificar
- Profundidad recursión: ~5 niveles (log₂ 30 ≈ 4.9)

---

## 3. ANÁLISIS COMPARATIVO

### 3.1 Resumen de Resultados

| Instancia | Días×Emp×Turnos | Tiempo (ms) | Operaciones | F. Objetivo | Turnos Cubiertos |
|-----------|----------------|-------------|-------------|-------------|------------------|
| horizon7_emp5_t3_000 | 7×5×3 | 8 | 56 | 2257.00 | 21/21 (100%) |
| horizon7_emp5_t3_001 | 7×5×3 | 0 | 50 | 2265.00 | 21/21 (100%) |
| horizon14_emp10_t6_001 | 14×10×6 | 23 | 282 | 9096.00 | 84/84 (100%) |
| horizon14_emp10_t6_000 | 14×10×6 | 8 | 302 | 9096.00 | 84/84 (100%) |
| horizon30_emp30_t20_000 | 30×30×20 | 6808 | 2948 | 65313.00 | 600/600 (100%) |
| horizon30_emp30_t20_001 | 30×30×20 | 6979 | 2975 | 65322.00 | 600/600 (100%) |

**Conclusión**: Cobertura 100% en todas las instancias. La función objetivo escala proporcionalmente al tamaño del problema.


---

## 4. DECISIONES DE DISEÑO JUSTIFICADAS

### 4.3 ¿Por qué Búsqueda Local?

| Aspecto | Justificación |
|--------|---------------|
| **Post-procesamiento** | No viola estructura D&C |
| **Mejora garantizada** | Floor de convergencia = óptimo local |
| **Limitado** | N iteraciones máximo → Tiempo acotado |
| **Efectivo** | Cobertura 100% en todas las instancias probadas |
