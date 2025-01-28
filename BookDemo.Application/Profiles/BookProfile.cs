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
            CreateMap<Cart, CartDTO>().ReverseMap();
            CreateMap<CartItem, CartItemDTO>().ReverseMap();
            CreateMap<Cart, CartDTO>()
           .ForMember(dest => dest.CartItems, opt => opt.MapFrom(src => src.CartItem));

            CreateMap<CartItem, CartItemDTO>()
                .ForMember(dest => dest.BookId, opt => opt.MapFrom(src => src.BookId))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price));

        }
    }
}
