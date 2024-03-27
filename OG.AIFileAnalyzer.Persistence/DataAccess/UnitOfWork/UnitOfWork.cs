using Microsoft.EntityFrameworkCore;
using OG.AIFileAnalyzer.Common.Entities;
using OG.AIFileAnalyzer.Persistence.DataAccess.Contexts;
using OG.AIFileAnalyzer.Persistence.DataAccess.Repositories.BaseRepository;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OG.AIFileAnalyzer.Persistence.DataAccess.UnitOfWork
{
    public class UnitOfWork(AIFileAnalyzerDbContext context) : IUnitOfWork
    {
        private readonly AIFileAnalyzerDbContext _context = context;

        private Hashtable _repositories;
        public async Task<int> Complete()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose() => _context.Dispose();

        public IBaseRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
        {
            _repositories ??= [];

            var type = typeof(TEntity).Name;

            if (!_repositories.ContainsKey(type))
            {
                var repositoryType = typeof(BaseRepository<>);
                var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TEntity)), _context);
                _repositories.Add(type, repositoryInstance);
            }

            return (IBaseRepository<TEntity>)_repositories[type];
        }

        ~UnitOfWork() => Dispose();
    }
}
