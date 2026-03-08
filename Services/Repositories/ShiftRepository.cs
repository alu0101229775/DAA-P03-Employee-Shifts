using System;
using System.Collections.Generic;
using System.Linq;
using EmployeeShifts.Models;
using EmployeeShifts.Exceptions;

namespace EmployeeShifts.Services.Repositories
{
    /// <summary>
    /// Implementación del repositorio de turnos en memoria.
    /// </summary>
    public class ShiftRepository : IShiftRepository
    {
        private readonly Dictionary<int, Shift> _shifts;
        private readonly object _lockObject = new object();

        public ShiftRepository()
        {
            _shifts = new Dictionary<int, Shift>();
        }

        public void Add(Shift shift)
        {
            if (shift == null)
                throw new ArgumentNullException(nameof(shift), "El turno no puede ser nulo.");

            lock (_lockObject)
            {
                if (_shifts.ContainsKey(shift.Id))
                    throw new ShiftException($"Ya existe un turno con el ID {shift.Id}.");

                try
                {
                    _shifts.Add(shift.Id, shift);
                }
                catch (Exception ex)
                {
                    throw new ShiftException($"Error al agregar el turno: {ex.Message}", ex);
                }
            }
        }

        public void Update(Shift shift)
        {
            if (shift == null)
                throw new ArgumentNullException(nameof(shift), "El turno no puede ser nulo.");

            lock (_lockObject)
            {
                if (!_shifts.ContainsKey(shift.Id))
                    throw new NotFoundException($"No se encontró un turno con el ID {shift.Id}.");

                try
                {
                    _shifts[shift.Id] = shift;
                }
                catch (Exception ex)
                {
                    throw new ShiftException($"Error al actualizar el turno: {ex.Message}", ex);
                }
            }
        }

        public void Delete(int shiftId)
        {
            if (shiftId <= 0)
                throw new ArgumentException("El ID debe ser positivo.", nameof(shiftId));

            lock (_lockObject)
            {
                if (!_shifts.Remove(shiftId))
                    throw new NotFoundException($"No se encontró un turno con el ID {shiftId}.");
            }
        }

        public Shift GetById(int shiftId)
        {
            if (shiftId <= 0)
                throw new ArgumentException("El ID debe ser positivo.", nameof(shiftId));

            lock (_lockObject)
            {
                if (_shifts.TryGetValue(shiftId, out var shift))
                    return shift;

                throw new NotFoundException($"No se encontró un turno con el ID {shiftId}.");
            }
        }

        public IEnumerable<Shift> GetAll()
        {
            lock (_lockObject)
            {
                return _shifts.Values.ToList();
            }
        }

        public IEnumerable<Shift> GetByDateRange(DateTime startDate, DateTime endDate)
        {
            if (startDate >= endDate)
                throw new ArgumentException("La fecha de inicio debe ser anterior a la fecha de fin.");

            lock (_lockObject)
            {
                return _shifts.Values
                    .Where(s => s.StartTime >= startDate && s.EndTime <= endDate)
                    .ToList();
            }
        }
    }
}
