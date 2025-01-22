using AutoMapper;
using BookDemo.Core.Entities;
using BookDemo.Core.Models;

namespace BookDemo.Application.Profiles
{
    public class BookProfile : Profile
    {
        public BookProfile()
        {
            //CreateMap<Book, BookDTO>()
            //    .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
            //    .ReverseMap();
            CreateMap<Book, BookDTO>().ReverseMap();
            CreateMap<Category, CategoryDTO>().ReverseMap();
            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<ApiResponse<BookDTO>, BookDTO>()
                    .ConvertUsing(src => src.Data);

        }
    }
}
