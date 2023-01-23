using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ToDoProject.Server.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        private DatabaseContext _ctx;
        private DbSet<TEntity> _dbSet;
        //quindi nel costruttore dovrò istanziare le mie variabili d'istanza, il context diventerà
        //argomento del costruttore
        public GenericRepository(DatabaseContext ctx)
        {
            this._ctx = ctx;
            this._dbSet = ctx.Set<TEntity>();

            _ctx.Database.EnsureCreated();
        }
        // Tecnicamente non dovrebbe servire, ma la tengo in caso
        // servisse per alleggerire il db
        public void Delete(object id)
        {
            //la implemento come mi serve, al momento la facciamo fisica
            TEntity entityToDelete = _dbSet.Find(id);
            _dbSet.Remove(entityToDelete);
        }

        public void DeleteLogical(TEntity entity)
        {
            _dbSet.Attach(entity);
            _ctx.Entry(entity).State = EntityState.Modified;
        }

        public List<TEntity> Get(Expression<Func<TEntity, bool>>? filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = _dbSet;

            foreach (var include in includes)
            {
                query = query.Include(include);
            }
            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (orderBy != null)
            {
                query = orderBy(query);
            }
            return query.ToList();
        }

        public TEntity GetById(object id)
        {
            return _dbSet.Find(id);
        }

        public TEntity GetFirstOrDefault(Expression<Func<TEntity, bool>>? filter = null, params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = _dbSet;

            foreach (var include in includes)
            {
                query = query.Include(include);
            }
            //si, rischio di trovarlo null ma se ho scritto un FirstOrDefault senza condizione
            //mi merito l'errore :D
            return query.FirstOrDefault(filter);
        }

        public void Insert(TEntity entity)
        {
            _dbSet.Add(entity);
        }

        public IQueryable<TEntity> Query(Expression<Func<TEntity, bool>>? filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null)
        {
            IQueryable<TEntity> query = _dbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (orderBy != null)
            {
                query = orderBy(query);
            }
            return query;
        }

        public void Update(TEntity entity)
        {
            _dbSet.Attach(entity);
            _ctx.Entry(entity).State = EntityState.Modified;
        }
    }
}
