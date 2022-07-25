using Golf.Fields.Database;
using Microsoft.EntityFrameworkCore;
using Golf.Fields.Shared;

namespace Golf.Fields.Services
{
    public class FieldServices
    {

        public async Task<SearchResult<Field>> Find(int skip, int limit, string? country = null, string? city = null, string? orderBy = null)
        {
            var result = new SearchResult<Field>();

            using (var context = new GolfDbContext())
            {

                if (context.Fields == null)
                    throw new ArgumentNullException(nameof(context.Fields));


                var query = context.Fields.AsQueryable();


                if (!string.IsNullOrWhiteSpace(city))
                {
                    city = city.ToLower();

                    query = query
                        .Where(field => field.City != null && field.City.Name != null && field.City.Name.ToLower().Contains(city))
                        .Include(field => field.City)
                        .ThenInclude(city => city!.Country);
                }


                else if (!string.IsNullOrWhiteSpace(country))
                {
                    country = country.ToLower();

                    query = query
                        .Where(field => field.City != null &&
                                        field.City.Country != null &&
                                        !string.IsNullOrWhiteSpace(field.City.Country.Name) &&
                                        field.City.Country.Name.ToLower().Contains(country))
                        .Include(field => field.City)
                        .ThenInclude(city => city!.Country);
                }

                else
                {
                    query = query
                        .Include(field => field.City)
                        .ThenInclude(city => city!.Country);
                }



                if (!string.IsNullOrWhiteSpace(orderBy))
                {
                    orderBy = orderBy.ToLower();

                    switch (orderBy)
                    {
                        case "city":
                            query = query.OrderBy(field => field.City.Name);
                            break;
                        case "country":
                            query = query.OrderBy(field => field.City.Country.Name);
                            break;
                        default:
                            query = query.OrderBy(field => field.ID);
                            break;
                    }
                }

                result.Total = await query.CountAsync();

                query = query.Skip(skip).Take(limit);

                result.Result = await query.ToListAsync();

            }

            return result;
        }
    }
}

