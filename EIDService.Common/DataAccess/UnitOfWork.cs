using EIDService.Common.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIDService.Common.DataAccess
{
    public class UnitOfWork : IDisposable
    {
        private readonly DbContext _context;

        private GenericRepository<Emitent> emitentRepository;
        private GenericRepository<Security> securityRepository;

        public UnitOfWork(DbContext context)
        {
            this._context = context;
        }

        public GenericRepository<Emitent> EmitentRepository
        {
            get
            {
                return this.emitentRepository ?? new GenericRepository<Emitent>(_context);
            }
        }

        public GenericRepository<Security> SecurityRepository
        {
            get
            {
                return this.securityRepository ?? new GenericRepository<Security>(_context);
            }
        }


        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Commit()
        {
            this._context.SaveChanges();
        }
    }
}
