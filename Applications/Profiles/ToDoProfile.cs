using AutoMapper;
using OasisoftTask.Applications.Dtos.ToDoDtos;
using OasisoftTask.Core.DomainModels;

namespace OasisoftTask.Applications.Profiles
{
    public class ToDoProfile : Profile
    {
        public ToDoProfile()
        {
            CreateMap<CreateToDo, ToDo>();
            CreateMap<ToDo, ListToDo>();
        }
    }
}
