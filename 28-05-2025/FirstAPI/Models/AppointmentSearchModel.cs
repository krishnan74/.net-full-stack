using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstAPI.Models
{
    public class AppointmentSearchModel
    {
        public string? PatientName { get; set; }
        public DateTime? AppointmentDate { get; set; }
        public Range<int>? AgeRange { get; set; }
        public string? DoctorName { get; set; }
        public string? Diagnosis { get; set; }
    }

    public class Range<T>
    {
        public T? MinAge { get; set; }
        public T? MaxAge { get; set; }
    }
}
