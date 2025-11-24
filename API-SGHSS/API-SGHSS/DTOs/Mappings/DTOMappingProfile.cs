using API_SGHSS.DTOs.AppointmentDTOs;
using API_SGHSS.DTOs.DoctorDTOs;
using API_SGHSS.DTOs.PatientDTOs;
using API_SGHSS.Models;
using AutoMapper;

namespace API_SGHSS.DTOs.Mappings
{
    public class DTOMappingProfile : Profile
    {
        public DTOMappingProfile()
        {
            //Patient
            CreateMap<Patient, PatientDTO>();
            CreateMap<PatientCreateDTO, Patient>();
            CreateMap<PatientUpdateDTO, Patient>();

            //Doctor
            CreateMap<Doctor, DoctorDTO>();
            CreateMap<DoctorCreateDTO, Doctor>();
            CreateMap<DoctorUpdateDTO, Doctor>();

            //Appointment
            CreateMap<Appointment, AppointmentDTO>();
            CreateMap<AppointmentCreateDTO, Appointment>();
            CreateMap<AppointmentUpdateDTO, Appointment>();
        }
    }
}
