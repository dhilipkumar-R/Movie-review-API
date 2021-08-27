using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoRepository.Persistence.IRepository;
using MovieReviewApp.Repository;

namespace MongoRepository.Persistence.Repositories
{
    public abstract class BaseMongoRepository<TEntity> : IRepository<TEntity, string> where TEntity : IEntity
    {
        protected abstract IMongoCollection<TEntity> Collection { get; }

        public virtual async Task<TEntity> GetByIdAsync(string id)
        {
            return await Collection.Find(x => x.Id.Equals(id)).FirstOrDefaultAsync();
        }
        public virtual async Task<IEnumerable<TEntity>> GetAll()
        {
            return await Collection.Find(_ => true).ToListAsync();
        }

        public virtual async Task<TEntity> SaveAsync(TEntity entity)
        {

            await Collection.ReplaceOneAsync(
                x => x.Id.Equals(entity.Id),
                entity,
                new UpdateOptions
                {
                    IsUpsert = true
                });

            return entity;
        }

        public virtual async Task DeleteAsync(string id)
        {
            await Collection.DeleteOneAsync(x => x.Id.Equals(new ObjectId(id)));
        }

        public virtual async Task<ICollection<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> predicate)
        {
            var x = await Collection.Find(predicate).ToListAsync();
            return x;
        }

        public virtual async Task<TEntity> FindAllFilesAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await Collection.Find(predicate).FirstOrDefaultAsync();
        }
        public async virtual void UpdateData(Expression<Func<TEntity, bool>> filter, UpdateDefinition<TEntity> update)
        {
            await Collection.UpdateOneAsync(filter, update);
        }

        public virtual async Task<TCollection> FindOneAsync<TCollection>(FilterDefinition<TEntity> filterExpression, ProjectionDefinition<TEntity, TCollection> projectionDefinition)
        {
            return await Collection.Find(filterExpression).Project(projectionDefinition).FirstOrDefaultAsync();
        }

        public virtual async Task<TCollection> FindOneAsync<TCollection>(Expression<Func<TEntity, bool>> filterExpression, ProjectionDefinition<TEntity, TCollection> projectionDefinition)
        {
            return await Collection.Find(filterExpression).Project(projectionDefinition).FirstOrDefaultAsync();
        }

        public virtual async Task<TEntity> FindOneAsync(Expression<Func<TEntity, bool>> filterExpression)
        {
            return await Collection.Find(filterExpression).FirstOrDefaultAsync();
        }
    }
}
