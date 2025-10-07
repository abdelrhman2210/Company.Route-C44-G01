using AutoMapper;
using Company.Route_C44_G01.DAL.Models;
using Company.Route_C44_G01.PL.DTOS;

namespace Company.Route_C44_G01.PL.Mapping
{
    public class EmpProfile : Profile 
    {
        public EmpProfile()
        {
            /*
                 CreateMap<CreateEmployeeDTO, Employee>().ReverseMap(); // to use in both directions
                 CreateMap<CreateEmployeeDTO, Employee>()
                 .ForMember(d => d.Name , o => o.MapFrom(S => S.Name)); // Example of custom mapping if there is difference in variable names
            */

            CreateMap<CreateEmployeeDTO, Employee>();
            CreateMap<Employee, CreateEmployeeDTO>();
        }
    }
}
