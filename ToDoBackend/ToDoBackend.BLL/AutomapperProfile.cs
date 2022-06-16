using AutoMapper;
using Microsoft.VisualBasic.CompilerServices;
using ToDoBackend.BLL.Models;
using ToDoBackend.DAL.Entities;

namespace ToDoBackend.BLL
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            CreateMap<Project, ProjectModel>().ReverseMap();
            CreateMap<Case, CaseModel>()
                .ForMember(cm => cm.Priority, o =>
                    o.MapFrom(c => (int) c.Priority))
                .ForMember(cm => cm.Status, o =>
                    o.MapFrom(c => (int) c.Status))
                .ReverseMap();
            CreateMap<ProjectUser, ProjectUserModel>().ReverseMap();
        }
    }
}