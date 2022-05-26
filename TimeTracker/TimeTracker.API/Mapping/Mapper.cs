using AutoMapper;
using TimeTracker.Application.DTO;
using TimeTracker.Core.Entities;

namespace TimeTracker.API.Mapping
{
    public class Mapper
    {
        private readonly IMapper _mapper = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<RoleDTO, Role>();

            cfg.CreateMap<ActivityTypeDTO, ActivityType>();

            cfg.CreateMap<RecordDTO, Record>();

            cfg.CreateMap<SexDTO, Sex>();

            cfg.CreateMap<EmployeeDTO, Employee>();

            cfg.CreateMap<ProjectDTO, Project>();

        }).CreateMapper();

        public Project Map(ProjectDTO source)
        {
            return this._mapper.Map<Project>(source);
        }

        public Project Map(ProjectDTO source, Project destination)
        {
            return this._mapper.Map(source, destination);
        }

        public Employee Map(EmployeeDTO source)
        {
            return this._mapper.Map<Employee>(source);
        }

        public Record Map(RecordDTO source)
        {
            return this._mapper.Map<Record>(source);
        }
    }
}
