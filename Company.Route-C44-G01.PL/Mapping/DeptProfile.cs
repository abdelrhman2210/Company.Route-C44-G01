using AutoMapper;
using Company.Route_C44_G01.DAL.Models;
using Company.Route_C44_G01.PL.DTOS;

namespace Company.Route_C44_G01.PL.Mapping
{
    public class DeptProfile : Profile
    {
        public DeptProfile()
        {
            CreateMap<Department, DepartmentDTO>();
            CreateMap<DepartmentDTO, Department>();
        }
    }
}
