using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DAL.Repository.Base
{
    public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : class, new()
    {
        protected readonly BancoAPIContext Db;
        protected readonly DbSet<TEntity> DbSet;
        protected IDbContextTransaction _transacao { get; set; }
        protected Repository(BancoAPIContext db)
        {
            Db = db;
            DbSet = db.Set<TEntity>();
        }
        public bool Existe(Expression<Func<TEntity, bool>> predicate)
        {
            return DbSet.AsNoTracking().Where(predicate).Any();
        }
        public async Task<IEnumerable<TEntity>> Buscar(Expression<Func<TEntity, bool>> predicate)
        {
            return await DbSet.AsNoTracking().Where(predicate).ToListAsync();
        }

        public async Task<int> GetSequecial(Expression<Func<TEntity, bool>> predicate)
        {
            var sequencial = await DbSet.AsNoTracking().Where(predicate).CountAsync();
            sequencial++;

            return sequencial;
        }

        public IQueryable<TEntity> Paginar(IQueryable<TEntity> query, int pagina, int numeroItens)
        {
            if (pagina == 1)
            {
                return query.Take(numeroItens);
            }
            else
            {
                return query.Skip((pagina - 1) * numeroItens).Take(numeroItens);
            }
        }

        public async Task<IEnumerable<TEntity>> BuscarTracking(Expression<Func<TEntity, bool>> predicate)
        {
            return await DbSet.Where(predicate).ToListAsync();
        }

        public virtual async Task<TEntity> GetObject(Expression<Func<TEntity, bool>> predicate)
        {
            return await DbSet.AsNoTracking().SingleOrDefaultAsync(predicate);
        }

        public virtual async Task<TEntity> GetObjectTracking(Expression<Func<TEntity, bool>> predicate)
        {
            return await DbSet.SingleOrDefaultAsync(predicate);
        }

        public virtual TEntity GetObjectNoAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return DbSet.AsNoTracking().SingleOrDefault(predicate);
        }

        public virtual async Task<TEntity> FindByKey(params object[] key)
        {
            return await DbSet.FindAsync(key);
        }
        public virtual async Task<IEnumerable<TEntity>> ObterTodos()
        {
            return await DbSet.ToListAsync();
        }
        public virtual BancoAPIContext GetConexao()
        {
            return Db;
        }
        public virtual async Task<TEntity> Adicionar(TEntity entity)
        {
            var newEntity = DbSet.Add(entity);
            await SaveChanges();
            return newEntity.Entity;
        }

        public virtual async Task<TEntity> update(TEntity entity)
        {
            var newEntity = DbSet.Update(entity);
            await SaveChanges();
            return newEntity.Entity;

        }

        public void IniciaTransacao()
        {
            _transacao = Db.Database.BeginTransaction();
        }

        public void CommitTransacao()
        {
            _transacao.Commit();
            _transacao?.Dispose();
        }

        public bool JaExisteTrasacao()
        {
            if (_transacao != null)
            {
                return true;
            }
            return false;
        }

        public void RoolbackTransacao()
        {
            Db.Database.RollbackTransaction();
            _transacao?.Dispose();
        }

        public virtual async Task Remover(int id)
        {
            DbSet.Remove(Db.Find<TEntity>(id));
            await SaveChanges();
        }

        public async Task<int> SaveChanges()
        {
            return await Db.SaveChangesAsync();
        }

        public void Dispose()
        {
            Db?.Dispose();
        }
    }
}