using System;
using EmployeeShifts.Services.Repositories;
using EmployeeShifts.Exceptions;

namespace EmployeeShifts.Services
{
    /// <summary>
    /// Contenedor de inyección de dependencias (IoC Container).
    /// Facilita la crear instancias de los servicios con todas sus dependencias correctamente wired.
    /// Implementa el patrón Factory y IoC para mantener bajo acoplamiento.
    /// </summary>
    public class ServiceContainer
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IShiftRepository _shiftRepository;
        private IShiftConflictValidator _shiftConflictValidator;
        private IShiftAssignmentService _shiftAssignmentService;
        private IEmployeeSearchService _employeeSearchService;
        private IReportGenerator _reportGenerator;

        public ServiceContainer()
        {
            try
            {
                _employeeRepository = new EmployeeRepository();
                _shiftRepository = new ShiftRepository();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error al inicializar los repositorios del contenedor de servicios.", ex);
            }
        }

        /// <summary>
        /// Obtiene o crea una instancia del validador de conflictos (Singleton).
        /// </summary>
        public IShiftConflictValidator GetShiftConflictValidator()
        {
            if (_shiftConflictValidator == null)
            {
                try
                {
                    _shiftConflictValidator = new ShiftConflictValidator(_employeeRepository);
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException("Error al crear el validador de conflictos.", ex);
                }
            }
            return _shiftConflictValidator;
        }

        /// <summary>
        /// Obtiene o crea una instancia del servicio de asignación de turnos (Singleton).
        /// </summary>
        public IShiftAssignmentService GetShiftAssignmentService()
        {
            if (_shiftAssignmentService == null)
            {
                try
                {
                    var conflictValidator = GetShiftConflictValidator();
                    _shiftAssignmentService = new ShiftAssignmentService(
                        _employeeRepository,
                        _shiftRepository,
                        conflictValidator);
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException("Error al crear el servicio de asignación de turnos.", ex);
                }
            }
            return _shiftAssignmentService;
        }

        /// <summary>
        /// Obtiene o crea una instancia del servicio de búsqueda de empleados (Singleton).
        /// </summary>
        public IEmployeeSearchService GetEmployeeSearchService()
        {
            if (_employeeSearchService == null)
            {
                try
                {
                    _employeeSearchService = new EmployeeSearchService(_employeeRepository);
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException("Error al crear el servicio de búsqueda de empleados.", ex);
                }
            }
            return _employeeSearchService;
        }

        /// <summary>
        /// Obtiene o crea una instancia del generador de reportes (Singleton).
        /// </summary>
        public IReportGenerator GetReportGenerator()
        {
            if (_reportGenerator == null)
            {
                try
                {
                    var conflictValidator = GetShiftConflictValidator();
                    _reportGenerator = new ReportGenerator(
                        _employeeRepository,
                        _shiftRepository,
                        conflictValidator);
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException("Error al crear el generador de reportes.", ex);
                }
            }
            return _reportGenerator;
        }

        /// <summary>
        /// Obtiene el repositorio de empleados.
        /// </summary>
        public IEmployeeRepository GetEmployeeRepository()
        {
            return _employeeRepository;
        }

        /// <summary>
        /// Obtiene el repositorio de turnos.
        /// </summary>
        public IShiftRepository GetShiftRepository()
        {
            return _shiftRepository;
        }
    }
}
