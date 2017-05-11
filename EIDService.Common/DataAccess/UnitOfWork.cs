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
        private GenericRepository<Financial> financialRepository;
        private GenericRepository<TradeSession> tradeSessionRepository;
        private GenericRepository<Candle> candleRepository;
        private GenericRepository<Settings> settingsRepository;
        private GenericRepository<Order> orderRepository;
        private GenericRepository<Position> positionRepository;
        private GenericRepository<StopOrder> stopOrderRepository;
        private GenericRepository<Transaction> transactionRepository;

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

        public GenericRepository<Financial> FinancialRepository
        {
            get
            {
                return this.financialRepository ?? new GenericRepository<Financial>(_context);
            }
        }

        public GenericRepository<TradeSession> TradeSessionRepository
        {
            get
            {
                return this.tradeSessionRepository ?? new GenericRepository<TradeSession>(_context);
            }
        }

        public GenericRepository<Candle> CandleRepository
        {
            get
            {
                return this.candleRepository ?? new GenericRepository<Candle>(_context);
            }
        }

        public GenericRepository<Settings> SettingsRepository
        {
            get
            {
                return this.settingsRepository ?? new GenericRepository<Settings>(_context);
            }
        }

        public GenericRepository<Order> OrderRepository
        {
            get
            {
                return this.orderRepository ?? new GenericRepository<Order>(_context);
            }
        }

        public GenericRepository<Position> PositionRepository
        {
            get
            {
                return this.positionRepository ?? new GenericRepository<Position>(_context);
            }
        }

        public GenericRepository<StopOrder> StopOrderRepository
        {
            get
            {
                return this.stopOrderRepository ?? new GenericRepository<StopOrder>(_context);
            }
        }

        public GenericRepository<Transaction> TransactionRepository
        {
            get
            {
                return this.transactionRepository ?? new GenericRepository<Transaction>(_context);
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
