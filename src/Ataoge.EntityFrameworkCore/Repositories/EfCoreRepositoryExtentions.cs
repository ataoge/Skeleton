using System.Linq;
using Ataoge.Data;
using Ataoge.Repositories;
using Ataoge.Utilities;
using Microsoft.EntityFrameworkCore;

namespace Ataoge.EntityFrameworkCore.Repositories
{
    public static class EfCoreRepositoryExentions
    {
        public static IQueryable<TEntity> AddExtentFilter<TEntity>(this IRepository<TEntity> repository,  IQueryable<TEntity> query, string geom,  int srid = 4326, int dbSrid = 4326, string spatialRelationShip = null)
            where TEntity : class, IEntity, IHasEwkb
        {
            if (string.IsNullOrEmpty(geom) || !repository.RepositoryContext.SupportSpatial)
                return query;

            if (char.IsDigit(geom[0]))
            {
                string[] xy = geom.Split(',');
                geom = string.Format("POLYGON(({0} {1}, {2} {1}, {2} {3}, {0} {3}, {0} {1}))", xy[0], xy[1], xy[2],xy[3]);
            }
            else if (geom[0] == '{') //json Extent
            {
               
                geom = geom.TrimEnd('}');
                double[] xy = geom.Split(',').Select(s => double.Parse(s.Substring(s.IndexOf(":") + 1))).ToArray();
                if (srid != dbSrid && (srid == 102100 || srid == 3857))
                {
                     double[] latlon = new double[4];
                     CoordinateTransform.WebMercatorToWGS84(xy[1], xy[0], out latlon[1], out latlon[0]);
                     CoordinateTransform.WebMercatorToWGS84(xy[3], xy[2], out latlon[3], out latlon[2]);
                     geom = string.Format("POLYGON(({0} {1}, {2} {1}, {2} {3}, {0} {3}, {0} {1}))", latlon[0], latlon[1], latlon[2],latlon[3]);
                 }
                 else
                 {
                    geom = string.Format("POLYGON(({0} {1}, {2} {1}, {2} {3}, {0} {3}, {0} {1}))", xy[0], xy[1], xy[2],xy[3]);
                 }

            }
            
             return query.Where(t => AtaogeDbContext.ST_Contains(AtaogeDbContext.ST_GeomFromText(geom, dbSrid), t.Shape));
        }

        public static IQueryable<TEntity> AddDistanceFilter<TEntity>(this IRepository<TEntity> repository, IQueryable<TEntity> query, string geom,  int srid = 4326, int buffer = 0, string unit = "meter")
             where TEntity : class, IEntity, IHasEwkb
        {
            if (string.IsNullOrEmpty(geom) || !repository.RepositoryContext.SupportSpatial)
                return query;
            
            
            if (unit == "meter" && srid == 4326)
            {
                var dbBuffer = buffer * 0.000008983153;
                return query.Where(t => AtaogeDbContext.ST_DWithin(AtaogeDbContext.ST_GeomFromText(geom, srid), t.Shape, dbBuffer));
            }
          
            return query.Where(t => AtaogeDbContext.ST_DWithin(AtaogeDbContext.ST_GeomFromText(geom, srid), t.Shape, buffer));
        }

        public static string ConverNameToDb(this IRepository repository, string name)
        {
            IRepositoryHelper helper = repository.RepositoryContext as IRepositoryHelper;
            if (helper!=null)
            {
               return RDFacadeExtensions.ConvertName(helper.ProviderName, name);
            }
            return name;
        }
    }
}