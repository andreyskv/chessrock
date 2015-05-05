using App.Common;
using App.Models;
using App.ViewModels;
using AutoMapper;

namespace App
{
    public partial class Startup
    {
        public static void ConfigureMapper(IConfiguration config)
        {
            config.CreateMap<TodoItem, TodoItemViewModel>().ReverseMap();
            config.CreateMap<TodoList, TodoListViewModel>().ReverseMap();
        }
    }
}