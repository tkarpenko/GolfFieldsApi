using Golf.Fields.Shared;
using AutoMapper;

namespace Golf.Fields.Api
{
    public class AutoMapperProfileConfiguration : Profile
    {
        public AutoMapperProfileConfiguration()
        {
            CreateMap(typeof(AuthTokenModel), typeof(AuthToken)).ReverseMap();

            //CreateMap(typeof(SearchResultModel<FieldModel>), typeof(SearchResult<Field>)).ReverseMap();

            //CreateMap(typeof(FieldModel), typeof(Field)).ReverseMap();

            //CreateMap(typeof(CityModel), typeof(City)).ReverseMap();

            //CreateMap(typeof(CountryModel), typeof(Country)).ReverseMap();
        }
    }
}

