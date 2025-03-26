using AutoMapper;
using CodePulseAdminPortal.API.Models.Domain;
using CodePulseAdminPortal.API.Models.DTO;

namespace CodePulseAdminPortal.API.Mappings
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<AddCategoryRequestDto, Category>().ReverseMap();
            CreateMap<UpdateCategoryRequest, Category>().ReverseMap();

            //CreateMap<AddBlogPostRequestDto, BlogPost>().ReverseMap();
            // Handle BlogPost mapping but ignore Categories
            CreateMap<AddBlogPostRequestDto, BlogPost>()
                .ForMember(dest => dest.Categories, opt => opt.Ignore());

            CreateMap<BlogPost, BlogPostDto>().ReverseMap();
            CreateMap<BlogImage, BlogImageDto>().ReverseMap();
        }
    }
}
