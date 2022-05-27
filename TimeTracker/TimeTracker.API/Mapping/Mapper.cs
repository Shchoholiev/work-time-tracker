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
            cfg.CreateMap<Role, RoleDTO>();

            cfg.CreateMap<ActivityTypeDTO, ActivityType>();
            cfg.CreateMap<ActivityType, ActivityTypeDTO>();

            cfg.CreateMap<RecordDTO, Record>();
            cfg.CreateMap<Record, RecordDTO>();

            cfg.CreateMap<SexDTO, Sex>();
            cfg.CreateMap<Sex, SexDTO>();

            cfg.CreateMap<EmployeeDTO, Employee>();
            cfg.CreateMap<Employee, EmployeeDTO>();

            cfg.CreateMap<ProjectDTO, Project>();
            cfg.CreateMap<Project, ProjectDTO>();

        }).CreateMapper();

        public Project Map(ProjectDTO source)
        {
            return this._mapper.Map<Project>(source);
        }

        public Project Map(ProjectDTO source, Project destination)
        {
            return this._mapper.Map(source, destination);
        }

        public IEnumerable<ProjectDTO> Map(IEnumerable<Project> source)
        {
            return this._mapper.Map<IEnumerable<ProjectDTO>>(source);
        }

        public Record Map(RecordDTO source)
        {
            return this._mapper.Map<Record>(source);
        }

        public IEnumerable<RecordDTO> Map(IEnumerable<Record> source)
        {
            return this._mapper.Map<IEnumerable<RecordDTO>>(source);
        }

        public Record Map(RecordDTO source, Record destination)
        {
            return this._mapper.Map(source, destination);
        }

        public Employee Map(EmployeeDTO source)
        {
            return this._mapper.Map<Employee>(source);
        }

        public IEnumerable<EmployeeDTO> Map(IEnumerable<Employee> source)
        {
            return this._mapper.Map<IEnumerable<EmployeeDTO>>(source);
        }

        public Employee Map(EmployeeDTO source, Employee destination)
        {
            return this._mapper.Map(source, destination);
        }

        public Role Map(RoleDTO source)
        {
            return this._mapper.Map<Role>(source);
        }

        public RoleDTO Map(Role source)
        {
            return this._mapper.Map<RoleDTO>(source);
        }

        public IEnumerable<RoleDTO> Map(IEnumerable<Role> source)
        {
            return this._mapper.Map<IEnumerable<RoleDTO>>(source);
        }

        public Role Map(RoleDTO source, Role destination)
        {
            return this._mapper.Map(source, destination);
        }

        public ActivityType Map(ActivityTypeDTO source)
        {
            return this._mapper.Map<ActivityType>(source);
        }

        public ActivityTypeDTO Map(ActivityType source)
        {
            return this._mapper.Map<ActivityTypeDTO>(source);
        }

        public IEnumerable<ActivityTypeDTO> Map(IEnumerable<ActivityType> source)
        {
            return this._mapper.Map<IEnumerable<ActivityTypeDTO>>(source);
        }

        public ActivityType Map(ActivityTypeDTO source, ActivityType destination)
        {
            return this._mapper.Map(source, destination);
        }

        public Sex Map(SexDTO source)
        {
            return this._mapper.Map<Sex>(source);
        }

        public SexDTO Map(Sex source)
        {
            return this._mapper.Map<SexDTO>(source);
        }

        public IEnumerable<SexDTO> Map(IEnumerable<Sex> source)
        {
            return this._mapper.Map<IEnumerable<SexDTO>>(source);
        }

        public Sex Map(SexDTO source, Sex destination)
        {
            return this._mapper.Map(source, destination);
        }
    }
}
