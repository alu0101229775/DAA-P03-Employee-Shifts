using System;

namespace EmployeeShifts.Models
{
    /// <summary>
    /// Representa un turno de trabajo.
    /// </summary>
    public class Shift
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public DateTime StartTime { get; private set; }
        public DateTime EndTime { get; private set; }
        public ShiftType Type { get; private set; }

        public Shift(int id, string name, DateTime startTime, DateTime endTime, ShiftType type)
        {
            ValidateShift(id, name, startTime, endTime);
            
            Id = id;
            Name = name;
            StartTime = startTime;
            EndTime = endTime;
            Type = type;
        }

        private static void ValidateShift(int id, string name, DateTime startTime, DateTime endTime)
        {
            if (id <= 0)
                throw new ArgumentException("El ID del turno debe ser positivo.", nameof(id));

            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("El nombre del turno no puede estar vacío.", nameof(name));

            if (name.Length > 50)
                throw new ArgumentException("El nombre del turno no puede exceder 50 caracteres.", nameof(name));

            if (startTime >= endTime)
                throw new ArgumentException("La hora de inicio debe ser anterior a la hora de fin.", nameof(startTime));

            if (startTime < DateTime.UtcNow)
                throw new ArgumentException("No se pueden crear turnos en el pasado.", nameof(startTime));
        }

        public bool OverlapsWith(Shift other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other), "El turno de comparación no puede ser nulo.");

            return StartTime < other.EndTime && EndTime > other.StartTime;
        }

        public TimeSpan GetDuration()
        {
            return EndTime - StartTime;
        }

        public override bool Equals(object obj)
        {
            if (obj is Shift shift)
                return Id == shift.Id;
            return false;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public override string ToString()
        {
            return $"Turno: {Name} ({StartTime:yyyy-MM-dd HH:mm} - {EndTime:yyyy-MM-dd HH:mm})";
        }
    }
}
