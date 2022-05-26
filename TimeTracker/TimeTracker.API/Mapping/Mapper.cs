using AutoMapper;
using TimeTracker.Application.DTO;
using TimeTracker.Application.DTO.Create;
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
            cfg.CreateMap<Record, RecordDTO>();
            cfg.CreateMap<RecordCreateDTO, Record>()
                .ForMember(r => r.Date, d => d.MapFrom(r => new DateTime(r.Year, r.Month, r.Day)));

            cfg.CreateMap<SexDTO, Sex>();

            cfg.CreateMap<EmployeeDTO, Employee>();

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

        public Employee Map(EmployeeDTO source)
        {
            return this._mapper.Map<Employee>(source);
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

        public Record Map(RecordCreateDTO source)
        {
            return this._mapper.Map<Record>(source);
        }
    }
}
