using ApiServer.Models;
using EntityGraphQL.Schema;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ApiServer.Controllers
{
    public class CityMutations
    {
        private readonly olympicsContext _context;

        public CityMutations(olympicsContext context)
        {
            _context = context;
        }

        [GraphQLMutation("Add a new City to the system")]
        public Expression<Func<olympicsContext, City>> AddNewCity(olympicsContext context, long id, string cityName)
        {
            if (context.Cities.Any(x => x.Id == id))
                return (ctx) => null;
            var item = new City
            {
                Id = id,
                CityName = cityName,
            };
            context.Cities.Add(item);
            context.SaveChanges();

            return (ctx) => ctx.Cities.First(p => p.Id == item.Id);
        }

        [GraphQLMutation("Update City in the system")]
        public Expression<Func<olympicsContext, City>> UpdateCity(olympicsContext context, long id, string cityName)
        {
            if (!context.Cities.Any(x => x.Id == id))
                return (ctx) => null;
            var item = context.Cities.First(x => x.Id == id);
            item.CityName = cityName;
            context.Update(item);
            context.SaveChanges();
            return (ctx) => ctx.Cities.First(p => p.Id == id);
        }

        [GraphQLMutation("Delete City in the system")]
        public Expression<Func<olympicsContext, City>> DeleteCity(olympicsContext context, long id)
        {
            if (!context.Cities.Any(x => x.Id == id))
                return (ctx) => null;
            var item = context.Cities.First(x => x.Id == id);
            context.Remove(item);
            context.SaveChanges();
            return (ctx) => item;
        }
    }
}
