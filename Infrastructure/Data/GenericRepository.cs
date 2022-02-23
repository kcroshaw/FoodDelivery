using ApplicationCore.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly ApplicationDbContext _dbContext;

        public GenericRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void Add(T entity)
        {
            _dbContext.Set<T>().Add(entity);
            _dbContext.SaveChanges();
        }

        public void Delete(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
            _dbContext.SaveChanges();
        }

        public void Delete(IEnumerable<T> entities)
        {
            _dbContext.Set<T>().RemoveRange(entities);
            _dbContext.SaveChanges();
        }

        public virtual async Task GetAsync(Expression predicate, bool asNoTracking = false, string includes = null)
        {
            if (includes == null) //there are no tables to join. Single object
            {
                if (asNoTracking) //read only copy for display purposes
                {

                    return await _dbContext.Set()
                    .AsNoTracking()
                    .Where(predicate)
                    .FirstOrDefaultAsync();

                }
                else //it needs to be tracked
                {
                    return await _dbContext.Set()
                    .Where(predicate)
                    .FirstOrDefaultAsync();
                }
            }
            else //this has includes (other objects or tables)
            {
                IQueryable queryable = _dbContext.Set();
                foreach (var inludeProperty in includes.Split(new char[]
                {','}, StringSplitOptions.RemoveEmptyEntries))
                {
                    queryable = queryable.Include(inludeProperty);
                }
                if (asNoTracking) //read only copy for display purposes
                {
                    return await queryable
                    .AsNoTracking()
                    .Where(predicate)
                    .FirstOrDefaultAsync();
                }
                else //it needs to be tracked
                {
                    return await queryable
                    .Where(predicate)
                    .FirstOrDefaultAsync();
                }
            }
        }

        public virtual async Task<T> GetAsync(Expression<Func<T, bool>> predicate, bool asNoTracking = false, string includes = null)
        {
            {
                if (includes == null) //there are no tables to join. Single object
                {
                    if (asNoTracking) //read only copy for display purposes
                    {

                        return await _dbContext.Set<T>()
                        .AsNoTracking()
                        .Where(predicate)
                        .FirstOrDefaultAsync();

                    }
                    else //it needs to be tracked
                    {
                        return await _dbContext.Set<T>()
                        .Where(predicate)
                        .FirstOrDefaultAsync();
                    }
                }
                else //this has includes (other objects or tables)
                {
                    IQueryable<T> queryable = _dbContext.Set<T>();
                    foreach (var inludeProperty in includes.Split(new char[]
                    {','}, StringSplitOptions.RemoveEmptyEntries))
                    {
                        queryable = queryable.Include(inludeProperty);
                    }
                    if (asNoTracking) //read only copy for display purposes
                    {
                        return await queryable
                        .AsNoTracking()
                        .Where(predicate)
                        .FirstOrDefaultAsync();
                    }
                    else //it needs to be tracked
                    {
                        return await queryable
                        .Where(predicate)
                        .FirstOrDefaultAsync();
                    }
                }
            }
        }


        public virtual T GetById(int id)
        {
            return _dbContext.Set<T>().Find(id);
        }

        public virtual IEnumerable<T> List()
        {
            return _dbContext.Set<T>().ToList().AsEnumerable();
        }

        public virtual IEnumerable<T> List(Expression<Func<T, bool>> predicate, Expression<Func<T, int>> orderBy = null, string includes = null)
        {
            IQueryable<T> queryable = _dbContext.Set<T>();
            if(predicate != null && includes == null)
            {
                return _dbContext.Set<T>()
                    .Where(predicate)
                    .AsEnumerable();
            }
            else if(includes != null)
            {
                foreach(var includeProperty in includes.Split(new char[]
                    {','}, StringSplitOptions.RemoveEmptyEntries))
                {
                    queryable = queryable.Include(includeProperty);
                }
            }
            if (predicate == null)
            {
                if (orderBy == null)
                {
                    return queryable.AsEnumerable();
                }
                else
                {
                    return queryable.OrderBy(orderBy).ToList().AsEnumerable();
                }
            }
            else
            {
                if(orderBy == null)
                {
                    return queryable.Where(predicate).ToList().AsEnumerable();
                }
                else
                {
                    return queryable.Where(predicate).OrderBy(orderBy).ToList().AsEnumerable();
                }
            }
        }

        public virtual async Task<IEnumerable<T>> ListAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, int>> orderBy = null, string includes = null)
        {
            IQueryable<T> queryable = _dbContext.Set<T>();
            if (predicate != null && includes == null)
            {
                return await _dbContext.Set<T>()
                    .Where(predicate)
                    .ToListAsync();
            }
            else if (includes != null)
            {
                foreach (var includeProperty in includes.Split(new char[]
                    {','}, StringSplitOptions.RemoveEmptyEntries))
                {
                    queryable = queryable.Include(includeProperty);
                }
            }
            if (predicate == null)
            {
                if (orderBy == null)
                {
                    return await queryable.ToListAsync(); 
                }
                else
                {
                    return await queryable.OrderBy(orderBy).ToListAsync();
                }
            }
            else
            {
                if (orderBy == null)
                {
                    return await queryable.Where(predicate).ToListAsync();
                }
                else
                {
                    return await queryable.Where(predicate).OrderBy(orderBy).ToListAsync(); 
                }
            }
        }

        public void Update(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            _dbContext.SaveChanges();
        }
    }
}
