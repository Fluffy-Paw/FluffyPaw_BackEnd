using FluffyPaw_Application.Utils.Pagination;
using FluffyPaw_Domain.Interfaces;
using FluffyPaw_Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Infrastructure.Repository
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        internal MyDbContext context;
        internal DbSet<TEntity> dbSet;

        public GenericRepository(MyDbContext context)
        {
            this.context = context;
            this.dbSet = context.Set<TEntity>();
        }

        public virtual IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "",
            int? pageIndex = null,
            int? pageSize = null)
        {
            IQueryable<TEntity> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            if (pageIndex.HasValue && pageSize.HasValue)
            {
                int validPageIndex = pageIndex.Value > 0 ? pageIndex.Value - 1 : 0;
                int validPageSize = pageSize.Value > 0 ? pageSize.Value : 10;

                query = query.Skip(validPageIndex * validPageSize).Take(validPageSize);
            }

            return query.ToList();
        }

        public virtual TEntity GetByID(object id)
        {
            return dbSet.Find(id);
        }

        public virtual void Insert(TEntity entity)
        {
            dbSet.Add(entity);
            context.SaveChanges();
        }

        public virtual void Delete(object id)
        {
            TEntity entityToDelete = dbSet.Find(id);
            Delete(entityToDelete);
        }

        public virtual void Delete(TEntity entityToDelete)
        {
            if (context.Entry(entityToDelete).State == EntityState.Detached)
            {
                dbSet.Attach(entityToDelete);
            }
            dbSet.Remove(entityToDelete);
        }

        public virtual void Update(TEntity entityToUpdate)
        {
            dbSet.Attach(entityToUpdate);
            context.Entry(entityToUpdate).State = EntityState.Modified;
        }


        public bool Exists(Expression<Func<TEntity, bool>> filter)
        {
            return dbSet.Any(filter);
        }

        public IEnumerable<TEntity> GetAll()
        {
            return dbSet.AsEnumerable();
        }

        public async Task<IList<TEntity>> GetAllAsync()
        {
            return await dbSet.ToListAsync();
        }

        public async Task<TEntity?> GetByIdAsync(object id)
        {
            return await dbSet.FindAsync(id);
        }

        public async Task<IPaginatedList<TEntity>> GetPagging(IQueryable<TEntity> query, int? pageIndex, int? pageSize)
        {
            // Kiểm tra xem query có hỗ trợ IAsyncQueryProvider hay không
            if (query.Provider is IAsyncQueryProvider)
            {
                // Đảm bảo rằng truy vấn đang được thực hiện trên cơ sở dữ liệu với Entity Framework
                query = query.AsNoTracking();

                // Nếu pageIndex hoặc pageSize không được truyền, trả về tất cả các bản ghi
                if (!pageIndex.HasValue || !pageSize.HasValue)
                {
                    var allItems = await query.ToListAsync();  // Đổi tên items thành allItems để tránh trùng lặp
                    return new PaginatedList<TEntity>(allItems, allItems.Count, 1, allItems.Count); // Trả về toàn bộ dữ liệu
                }

                // Nếu có truyền pageIndex và pageSize, thực hiện phân trang
                int count = await query.CountAsync();  // Đếm tổng số bản ghi
                var paginatedItems = await query  // Đổi tên items thành paginatedItems
                    .Skip((pageIndex.Value - 1) * pageSize.Value)
                    .Take(pageSize.Value)
                    .ToListAsync();

                return new PaginatedList<TEntity>(paginatedItems, count, pageIndex.Value, pageSize.Value);
            }
            else
            {
                // Nếu không hỗ trợ IAsyncQueryProvider, thực hiện truy vấn đồng bộ
                if (!pageIndex.HasValue || !pageSize.HasValue)
                {
                    var allItems = query.ToList();  // Đổi tên items thành allItems
                    return new PaginatedList<TEntity>(allItems, allItems.Count, 1, allItems.Count); // Trả về toàn bộ dữ liệu
                }

                int count = query.Count();
                var paginatedItems = query  // Đổi tên items thành paginatedItems
                    .Skip((pageIndex.Value - 1) * pageSize.Value)
                    .Take(pageSize.Value)
                    .ToList();

                return new PaginatedList<TEntity>(paginatedItems, count, pageIndex.Value, pageSize.Value);
            }
        }


        public async Task InsertAsync(TEntity obj)
        {
            await dbSet.AddAsync(obj);
        }

        public void InsertRange(IList<TEntity> obj)
        {
            dbSet.AddRange(obj);
        }
        public void Save()
        {
            context.SaveChanges();
        }

        public async Task SaveAsync()
        {
            await context.SaveChangesAsync();
        }
        public Task UpdateAsync(TEntity obj)
        {
            return Task.FromResult(dbSet.Update(obj));
        }

        public async Task DeleteAsync(object id)
        {
            TEntity entity = await dbSet.FindAsync(id) ?? throw new Exception();
            dbSet.Remove(entity);
        }
        public async Task<IEnumerable<TEntity>> GetAsync(
        Expression<Func<TEntity, bool>> filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
        string includeProperties = "",
        int? pageIndex = null,
        int? pageSize = null)
        {
            IQueryable<TEntity> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }
            foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }
            if (orderBy != null)
            {
                query = orderBy(query);
            }

            if (pageIndex.HasValue && pageSize.HasValue)
            {
                query = query.Skip((pageIndex.Value - 1) * pageSize.Value).Take(pageSize.Value);
            }
            return await query.ToListAsync();
        }
    }
}
