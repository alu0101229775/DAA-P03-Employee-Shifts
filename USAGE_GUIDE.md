# Guía de Uso y Ejemplos

## Tabla de Contenidos

1. [Instalación](#instalación)
2. [Configuración Inicial](#configuración-inicial)
3. [Operaciones Básicas](#operaciones-básicas)
4. [Manejo de Errores](#manejo-de-errores)
5. [Validaciones](#validaciones)
6. [Casos de Uso Avanzados](#casos-de-uso-avanzados)

## Instalación

### Requisitos Previos

- .NET SDK 6.0 o superior
- Editor de código (Visual Studio, Visual Studio Code, o similar)

### Compilación

```bash
cd c:\Users\Gerard\Documents\DAA2526\DAA-P03-Employee-Shifts
dotnet restore
dotnet build
```

### Ejecución

```bash
dotnet run
```

## Configuración Inicial

### Crear el Contenedor de Servicios

```csharp
// Crear el contenedor que gestiona todas las dependencias
var container = new ServiceContainer();

// Obtener referencias a los servicios
var employeeRepository = container.GetEmployeeRepository();
var shiftRepository = container.GetShiftRepository();
var assignmentService = container.GetShiftAssignmentService();
var searchService = container.GetEmployeeSearchService();
var reportGenerator = container.GetReportGenerator();
```

## Operaciones Básicas

### 1. Crear un Empleado

```csharp
try
{
    var employee = new Employee(
        id: 1,
        name: "Juan García",
        email: "juan.garcia@company.com",
        role: EmployeeRole.Manager);
    
    employeeRepository.Add(employee);
    Console.WriteLine("Empleado creado exitosamente.");
}
catch (ArgumentException ex)
{
    Console.WriteLine($"Error de validación: {ex.Message}");
}
```

### 2. Obtener un Empleado

```csharp
try
{
    var employee = employeeRepository.GetById(1);
    Console.WriteLine($"Empleado encontrado: {employee.Name}");
}
catch (NotFoundException ex)
{
    Console.WriteLine($"Error: {ex.Message}");
}
```

### 3. Listar Todos los Empleados

```csharp
var employees = employeeRepository.GetAll();
foreach (var emp in employees)
{
    Console.WriteLine($"- {emp.Name} ({emp.Role})");
}
```

### 4. Actualizar un Empleado

```csharp
try
{
    var employee = employeeRepository.GetById(1);
    // Nota: Para actualizar propiedades, sería necesario un patrón builder o setter internos
    // Por ahora, los datos se reasignan
    var updatedEmployee = new Employee(1, "Juan García García", employee.Email, employee.Role);
    employeeRepository.Update(updatedEmployee);
}
catch (NotFoundException ex)
{
    Console.WriteLine($"Error: {ex.Message}");
}
```

### 5. Crear un Turno

```csharp
try
{
    var tomorrow = DateTime.UtcNow.AddDays(1);
    var shift = new Shift(
        id: 1,
        name: "Turno Matutino",
        startTime: tomorrow.Date.AddHours(6),
        endTime: tomorrow.Date.AddHours(14),
        type: ShiftType.Morning);
    
    shiftRepository.Add(shift);
    Console.WriteLine("Turno creado exitosamente.");
}
catch (ArgumentException ex)
{
    Console.WriteLine($"Error de validación: {ex.Message}");
}
```

### 6. Asignar un Turno a un Empleado

```csharp
try
{
    assignmentService.AssignShiftToEmployee(employeeId: 1, shiftId: 1);
    Console.WriteLine("Turno asignado exitosamente.");
}
catch (ShiftAssignmentException ex)
{
    Console.WriteLine($"Error en la asignación: {ex.Message}");
}
```

### 7. Obtener Turnos de un Empleado

```csharp
try
{
    var shifts = assignmentService.GetEmployeeShifts(employeeId: 1);
    foreach (var shift in shifts)
    {
        Console.WriteLine($"- {shift.Name}: {shift.StartTime:yyyy-MM-dd HH:mm}");
    }
}
catch (ShiftAssignmentException ex)
{
    Console.WriteLine($"Error: {ex.Message}");
}
```

### 8. Obtener Empleados de un Turno

```csharp
try
{
    var employees = assignmentService.GetShiftEmployees(shiftId: 1);
    foreach (var emp in employees)
    {
        Console.WriteLine($"- {emp.Name}");
    }
}
catch (ShiftAssignmentException ex)
{
    Console.WriteLine($"Error: {ex.Message}");
}
```

## Manejo de Errores

### Estructura Try-Catch Estándar

```csharp
try
{
    // Operación
}
catch (ArgumentException ex)
{
    // Error de validación de argumentos
    Console.WriteLine($"Argumento inválido: {ex.Message}");
}
catch (ArgumentNullException ex)
{
    // Argumento nulo
    Console.WriteLine($"Argumento nulo: {ex.ParamName}");
}
catch (NotFoundException ex)
{
    // Recurso no encontrado
    Console.WriteLine($"No encontrado: {ex.Message}");
}
catch (ValidationException ex)
{
    // Error de validación
    Console.WriteLine($"Validación fallida: {ex.Message}");
}
catch (EmployeeException ex)
{
    // Error específico de empleado
    Console.WriteLine($"Error de empleado: {ex.Message}");
}
catch (ShiftException ex)
{
    // Error específico de turno
    Console.WriteLine($"Error de turno: {ex.Message}");
}
catch (ShiftAssignmentException ex)
{
    // Error de asignación de turno
    Console.WriteLine($"Error de asignación: {ex.Message}");
}
catch (Exception ex)
{
    // Error genérico no previsto
    Console.WriteLine($"Error inesperado: {ex.Message}");
}
```

### Manejo de Excepciones Internas

```csharp
try
{
    // Operación que puede lanzar una excepción
}
catch (NotFoundException ex)
{
    throw new EmployeeException($"No se pudo procesar: {ex.Message}", ex);
}
```

## Validaciones

### Validaciones de Empleado

#### ID Negativo

```csharp
try
{
    var employee = new Employee(-1, "Juan", "juan@test.com", EmployeeRole.Worker);
}
catch (ArgumentException ex)
{
    // El ID del empleado debe ser positivo.
    Console.WriteLine(ex.Message);
}
```

#### Nombre Vacío

```csharp
try
{
    var employee = new Employee(1, "", "juan@test.com", EmployeeRole.Worker);
}
catch (ArgumentException ex)
{
    // El nombre del empleado no puede estar vacío.
    Console.WriteLine(ex.Message);
}
```

#### Nombre Muy Largo

```csharp
try
{
    var employee = new Employee(1, new string('a', 101), "juan@test.com", EmployeeRole.Worker);
}
catch (ArgumentException ex)
{
    // El nombre del empleado no puede exceder 100 caracteres.
    Console.WriteLine(ex.Message);
}
```

#### Email Inválido

```csharp
try
{
    var employee = new Employee(1, "Juan", "email-invalido", EmployeeRole.Worker);
}
catch (ArgumentException ex)
{
    // El formato del email es inválido.
    Console.WriteLine(ex.Message);
}
```

### Validaciones de Turno

#### Hora de Fin Anterior a Inicio

```csharp
try
{
    var now = DateTime.UtcNow;
    var shift = new Shift(
        1,
        "Turno",
        now.AddDays(2),
        now.AddDays(1),
        ShiftType.Morning);
}
catch (ArgumentException ex)
{
    // La hora de inicio debe ser anterior a la hora de fin.
    Console.WriteLine(ex.Message);
}
```

#### Turno en el Pasado

```csharp
try
{
    var shift = new Shift(
        1,
        "Turno",
        DateTime.UtcNow.AddDays(-1),
        DateTime.UtcNow.AddDays(-1).AddHours(8),
        ShiftType.Morning);
}
catch (ArgumentException ex)
{
    // No se pueden crear turnos en el pasado.
    Console.WriteLine(ex.Message);
}
```

### Validaciones de Asignación de Turnos

#### Turno Solapado

```csharp
var employee = new Employee(1, "Juan", "juan@test.com", EmployeeRole.Worker);
var shift1 = new Shift(1, "Turno 1", 
    DateTime.UtcNow.AddDays(1).Date.AddHours(6),
    DateTime.UtcNow.AddDays(1).Date.AddHours(14),
    ShiftType.Morning);
var shift2 = new Shift(2, "Turno 2",
    DateTime.UtcNow.AddDays(1).Date.AddHours(10),
    DateTime.UtcNow.AddDays(1).Date.AddHours(18),
    ShiftType.Afternoon);

try
{
    employee.AssignShift(shift1);
    employee.AssignShift(shift2); // Esto fallará
}
catch (InvalidOperationException ex)
{
    // El turno entra en conflicto con otro turno asignado.
    Console.WriteLine(ex.Message);
}
```

## Casos de Uso Avanzados

### 1. Búsqueda de Empleados por Rol

```csharp
var managers = searchService.SearchByRole(EmployeeRole.Manager);
Console.WriteLine("Gerentes en el sistema:");
foreach (var manager in managers)
{
    Console.WriteLine($"- {manager.Name}");
}
```

### 2. Búsqueda de Empleados por Nombre

```csharp
var empleados = searchService.SearchByName("Juan");
Console.WriteLine("Empleados que contienen 'Juan' en el nombre:");
foreach (var emp in empleados)
{
    Console.WriteLine($"- {emp.Name}");
}
```

### 3. Búsqueda de Empleado por Email

```csharp
try
{
    var employee = searchService.SearchByEmail("juan@company.com");
    Console.WriteLine($"Empleado encontrado: {employee.Name}");
}
catch (NotFoundException ex)
{
    Console.WriteLine($"No se encontró: {ex.Message}");
}
```

### 4. Detectar Conflictos Globales

```csharp
var conflictValidator = container.GetShiftConflictValidator();
var conflicts = conflictValidator.DetectAllConflicts();

if (conflicts.Any())
{
    Console.WriteLine("Se detectaron los siguientes conflictos:");
    foreach (var conflict in conflicts)
    {
        Console.WriteLine(conflict.ToString());
    }
}
else
{
    Console.WriteLine("No hay conflictos detectados.");
}
```

### 5. Generar Reporte de Turnos de un Empleado

```csharp
var report = reportGenerator.GenerateEmployeeShiftReport(empleadoId: 1);
Console.WriteLine(report);
```

### 6. Generar Reporte de Calendario

```csharp
var startDate = DateTime.UtcNow.Date;
var endDate = startDate.AddDays(7);
var report = reportGenerator.GenerateScheduleReport(startDate, endDate);
Console.WriteLine(report);
```

### 7. Generar Reporte de Conflictos

```csharp
var report = reportGenerator.GenerateConflictReport();
Console.WriteLine(report);
```

### 8. Remoción de Turno

```csharp
try
{
    assignmentService.RemoveShiftFromEmployee(employeeId: 1, shiftId: 1);
    Console.WriteLine("Turno removido exitosamente.");
}
catch (ShiftAssignmentException ex)
{
    Console.WriteLine($"Error: {ex.Message}");
}
```

### 9. Eliminar un Empleado

```csharp
try
{
    employeeRepository.Delete(1);
    Console.WriteLine("Empleado eliminado exitosamente.");
}
catch (NotFoundException ex)
{
    Console.WriteLine($"Error: {ex.Message}");
}
```

### 10. Operación Completa de Ciclo de Vida

```csharp
try
{
    // 1. Crear empleados
    var emp1 = new Employee(1, "Alice Smith", "alice@company.com", EmployeeRole.Manager);
    var emp2 = new Employee(2, "Bob Johnson", "bob@company.com", EmployeeRole.Worker);
    
    employeeRepository.Add(emp1);
    employeeRepository.Add(emp2);
    
    // 2. Crear turnos
    var tomorrow = DateTime.UtcNow.AddDays(1);
    var shift1 = new Shift(1, "Morning", 
        tomorrow.Date.AddHours(6), tomorrow.Date.AddHours(14), ShiftType.Morning);
    var shift2 = new Shift(2, "Afternoon",
        tomorrow.Date.AddHours(14), tomorrow.Date.AddHours(22), ShiftType.Afternoon);
    
    shiftRepository.Add(shift1);
    shiftRepository.Add(shift2);
    
    // 3. Asignar turnos
    assignmentService.AssignShiftToEmployee(1, 1);
    assignmentService.AssignShiftToEmployee(2, 2);
    
    // 4. Generar reportes
    Console.WriteLine(reportGenerator.GenerateScheduleReport(tomorrow.Date, tomorrow.AddDays(1).Date));
    
    // 5. Remover turno
    assignmentService.RemoveShiftFromEmployee(1, 1);
    
    // 6. Eliminar empleado
    employeeRepository.Delete(2);
    
    Console.WriteLine("Ciclo de vida completado exitosamente.");
}
catch (Exception ex)
{
    Console.WriteLine($"Error en el ciclo: {ex.Message}");
}
```

## Mejores Prácticas

### 1. Siempre Usar Try-Catch

```csharp
// ✓ Bien
try
{
    employeeRepository.Add(employee);
}
catch (Exception ex)
{
    LogError(ex);
}

// ✗ Mal
employeeRepository.Add(employee); // Sin manejo de errores
```

### 2. Validar Entrada del Usuario

```csharp
// ✓ Bien
if (string.IsNullOrWhiteSpace(input))
{
    throw new ValidationException("La entrada no puede estar vacía.");
}

// ✗ Mal
var emp = new Employee(1, input, "email@test.com", role); // Sin validación previa
```

### 3. Usar las Excepciones Específicas Correctas

```csharp
// ✓ Bien
throw new NotFoundException($"Empleado con ID {id} no encontrado.");
throw new ValidationException("El email no es válido.");

// ✗ Mal
throw new Exception("Error"); // Demasiado genérico
```

### 4. Inyectar Dependencias

```csharp
// ✓ Bien
public ShiftAssignmentService(
    IEmployeeRepository employeeRepository,
    IShiftRepository shiftRepository)
{
    _employeeRepository = employeeRepository;
    _shiftRepository = shiftRepository;
}

// ✗ Mal
public ShiftAssignmentService()
{
    _employeeRepository = new EmployeeRepository(); // Acoplado
}
```

### 5. Usar Interfaces para Extensibilidad

```csharp
// ✓ Bien
public interface IEmployeeRepository
{
    void Add(Employee employee);
}

// ✗ Mal
public class EmployeeRepository // Clase concreta sin interfaz
{
    public void Add(Employee employee) { }
}
```
