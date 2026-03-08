# Arquitectura y Diseño del Sistema

## Diagrama de Componentes

```
┌─────────────────────────────────────────────────────────────────┐
│                        APLICACIÓN (Program.cs)                   │
└────────────────┬────────────────────────────────────────────────┘
                 │
                 ├─→ ServiceContainer (IoC Container)
                 │   └─→ Gestiona la creación de servicios
                 │
     ┌───────────┴────────────────────────────────────────────────┐
     │                                                              │
     ├─→ Repositories                                              │
     │   ├─→ IEmployeeRepository                                   │
     │   │   └─→ EmployeeRepository (en memoria)                  │
     │   └─→ IShiftRepository                                      │
     │       └─→ ShiftRepository (en memoria)                      │
     │                                                              │
     ├─→ Services                                                   │
     │   ├─→ IShiftAssignmentService                              │
     │   │   └─→ ShiftAssignmentService                           │
     │   ├─→ IShiftConflictValidator                              │
     │   │   └─→ ShiftConflictValidator                           │
     │   ├─→ IEmployeeSearchService                               │
     │   │   └─→ EmployeeSearchService                            │
     │   └─→ IReportGenerator                                      │
     │       └─→ ReportGenerator                                   │
     │                                                              │
     └─→ Models                                                     │
         ├─→ Employee                                              │
         ├─→ Shift                                                 │
         ├─→ ShiftConflict                                         │
         ├─→ Enums (EmployeeRole, ShiftType)                      │
         └─→ Exceptions (todas las excepciones personalizadas)    │
```

## Flujo de Inyección de Dependencias

```
ServiceContainer
│
├─→ GetEmployeeRepository()
│   └─→ Crea EmployeeRepository (Singleton)
│
├─→ GetShiftRepository()
│   └─→ Crea ShiftRepository (Singleton)
│
├─→ GetShiftConflictValidator()
│   └─→ Crea ShiftConflictValidator
│       └─→ Inyecta IEmployeeRepository
│
├─→ GetShiftAssignmentService()
│   └─→ Crea ShiftAssignmentService
│       ├─→ Inyecta IEmployeeRepository
│       ├─→ Inyecta IShiftRepository
│       └─→ Inyecta IShiftConflictValidator
│
├─→ GetEmployeeSearchService()
│   └─→ Crea EmployeeSearchService
│       └─→ Inyecta IEmployeeRepository
│
└─→ GetReportGenerator()
    └─→ Crea ReportGenerator
        ├─→ Inyecta IEmployeeRepository
        ├─→ Inyecta IShiftRepository
        └─→ Inyecta IShiftConflictValidator
```

## Relaciones entre Clases

```
┌──────────────────────────────────────────┐
│         Employee                         │
├──────────────────────────────────────────┤
│ - Id: int                                │
│ - Name: string                           │
│ - Email: string                          │
│ - Role: EmployeeRole                     │
│ - AssignedShifts: List<Shift>            │
│ - CreatedDate: DateTime                  │
├──────────────────────────────────────────┤
│ + AssignShift(shift)                     │
│ + RemoveShift(shift)                     │
│ + ValidateEmployee()                     │
└──────────────────────────────────────────┘
           │ 1 ──── * │
           │ Contiene │
           ▼          ▼
┌──────────────────────────────────────────┐
│         Shift                            │
├──────────────────────────────────────────┤
│ - Id: int                                │
│ - Name: string                           │
│ - StartTime: DateTime                    │
│ - EndTime: DateTime                      │
│ - Type: ShiftType                        │
├──────────────────────────────────────────┤
│ + OverlapsWith(shift): bool              │
│ + GetDuration(): TimeSpan                │
│ + ValidateShift()                        │
└──────────────────────────────────────────┘

┌──────────────────────────────────────────┐
│      ShiftConflict                       │
├──────────────────────────────────────────┤
│ - EmployeeId1: int                       │
│ - EmployeeId2: int                       │
│ - ShiftId: int                           │
│ - ConflictReason: string                 │
└──────────────────────────────────────────┘
```

## Flujo de Ejecución Principal

```
Program.Main()
    │
    ├─→ ServiceContainer container = new ServiceContainer()
    │
    ├─→ DemoSystem(container)
    │   │
    │   ├─→ Crear Empleados (Employee, EmployeeRepository)
    │   │   └─→ EmployeeRepository.Add()
    │   │
    │   ├─→ Crear Turnos (Shift, ShiftRepository)
    │   │   └─→ ShiftRepository.Add()
    │   │
    │   ├─→ Asignar Turnos (ShiftAssignmentService)
    │   │   ├─→ ShiftAssignmentService.AssignShiftToEmployee()
    │   │   │   ├─→ EmployeeRepository.GetById()
    │   │   │   ├─→ ShiftRepository.GetById()
    │   │   │   ├─→ ShiftConflictValidator.HasConflict()
    │   │   │   └─→ Employee.AssignShift()
    │   │
    │   ├─→ Buscar Empleados (EmployeeSearchService)
    │   │   ├─→ SearchByRole()
    │   │   ├─→ SearchByName()
    │   │   └─→ SearchByEmail()
    │   │
    │   └─→ Generar Reportes (ReportGenerator)
    │       ├─→ GenerateEmployeeShiftReport()
    │       ├─→ GenerateScheduleReport()
    │       └─→ GenerateConflictReport()
    │           └─→ ShiftConflictValidator.DetectAllConflicts()
    │
    └─→ DemoErrorHandling(container)
        └─→ Mostrar ejemplos de manejo de errores
```

## Patrones de Diseño Utilizados

### 1. Repository Pattern
Aísla la lógica de acceso a datos de la lógica de negocio.

```csharp
IEmployeeRepository employeeRepository = new EmployeeRepository();
employeeRepository.Add(employee);
employeeRepository.GetById(1);
```

### 2. Dependency Injection
Inyecta dependencias a través del constructor.

```csharp
public ShiftAssignmentService(
    IEmployeeRepository employeeRepository,
    IShiftRepository shiftRepository,
    IShiftConflictValidator conflictValidator)
```

### 3. Factory Pattern
`ServiceContainer` actúa como factory para crear servicios.

```csharp
var container = new ServiceContainer();
var service = container.GetShiftAssignmentService();
```

### 4. Singleton Pattern
Los servicios se crean una sola vez y se reutilizan.

```csharp
private IShiftAssignmentService _shiftAssignmentService;

public IShiftAssignmentService GetShiftAssignmentService()
{
    if (_shiftAssignmentService == null)
    {
        _shiftAssignmentService = new ShiftAssignmentService(...);
    }
    return _shiftAssignmentService;
}
```

### 5. Strategy Pattern
Diferentes validadores y generadores pueden ser intercambiados.

```csharp
public interface IShiftConflictValidator { }
public interface IReportGenerator { }
```

## Seguridad y Concurrencia

### Thread Safety

Las operaciones críticas están protegidas con `lock`:

```csharp
private readonly object _lockObject = new object();

public void Add(Employee employee)
{
    lock (_lockObject)
    {
        _employees.Add(employee.Id, employee);
    }
}
```

### Validación de Entrada

Todas las entradas se validan:

```csharp
if (employeeId <= 0)
    throw new ArgumentException("El ID debe ser positivo.");

if (string.IsNullOrWhiteSpace(name))
    throw new ArgumentException("El nombre no puede estar vacío.");
```

### Manejo de Excepciones

Las excepciones se capturan y se lanzan con contexto:

```csharp
try
{
    // operación riesgosa
}
catch (Exception ex)
{
    throw new CustomException("Error contextual", ex);
}
```

## Escalabilidad Futura

### Agregar Base de Datos

```csharp
public class DatabaseEmployeeRepository : IEmployeeRepository
{
    private readonly DbContext _dbContext;
    
    public void Add(Employee employee)
    {
        _dbContext.Employees.Add(employee);
        _dbContext.SaveChanges();
    }
}

// En ServiceContainer:
public IEmployeeRepository GetEmployeeRepository()
{
    return new DatabaseEmployeeRepository(new AppDbContext());
}
```

### Agregar Cache

```csharp
public class CachedEmployeeRepository : IEmployeeRepository
{
    private readonly IEmployeeRepository _innerRepository;
    private readonly IMemoryCache _cache;
    
    public Employee GetById(int employeeId)
    {
        var key = $"employee_{employeeId}";
        return _cache.GetOrCreate(key, 
            entry => _innerRepository.GetById(employeeId));
    }
}
```

### Agregar Logging

```csharp
public class LoggingShiftAssignmentService : IShiftAssignmentService
{
    private readonly IShiftAssignmentService _innerService;
    private readonly ILogger _logger;
    
    public void AssignShiftToEmployee(int employeeId, int shiftId)
    {
        _logger.LogInformation($"Asignando turno {shiftId} a empleado {employeeId}");
        _innerService.AssignShiftToEmployee(employeeId, shiftId);
        _logger.LogInformation("Asignación exitosa");
    }
}
```

## Pruebas Unitarias

Ejemplo de cómo probar con las interfaces:

```csharp
[TestClass]
public class ShiftAssignmentServiceTests
{
    private Mock<IEmployeeRepository> _mockEmployeeRepo;
    private Mock<IShiftRepository> _mockShiftRepo;
    private Mock<IShiftConflictValidator> _mockValidator;
    private ShiftAssignmentService _service;
    
    [TestInitialize]
    public void Setup()
    {
        _mockEmployeeRepo = new Mock<IEmployeeRepository>();
        _mockShiftRepo = new Mock<IShiftRepository>();
        _mockValidator = new Mock<IShiftConflictValidator>();
        
        _service = new ShiftAssignmentService(
            _mockEmployeeRepo.Object,
            _mockShiftRepo.Object,
            _mockValidator.Object);
    }
    
    [TestMethod]
    public void AssignShiftToEmployee_WithValidData_Success()
    {
        // Arrange
        var employee = new Employee(1, "Test", "test@test.com", EmployeeRole.Worker);
        var shift = new Shift(1, "Morning", DateTime.UtcNow.AddDays(1), 
                            DateTime.UtcNow.AddDays(1).AddHours(8), ShiftType.Morning);
        
        _mockEmployeeRepo.Setup(r => r.GetById(1)).Returns(employee);
        _mockShiftRepo.Setup(r => r.GetById(1)).Returns(shift);
        _mockValidator.Setup(v => v.HasConflict(employee, shift)).Returns(false);
        
        // Act
        _service.AssignShiftToEmployee(1, 1);
        
        // Assert
        Assert.AreEqual(1, employee.AssignedShifts.Count);
    }
}
```
