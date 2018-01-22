using EIDClient.Core.Entities;
using EIDClient.Core.Messages;
using EIDClient.Core.Repository;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIDClient.Core.DataModel
{
    public class PortfolioModel
    {
        private PortfolioItemRepository _porfolioItemRepository = null;
        private SecurityDataRepository _securityDataRepository = null;
        private IncomeRepository _incomeRepository = null;

        public PortfolioModel(PortfolioItemRepository portfolioItemRepository, SecurityDataRepository securityDataRepository, IncomeRepository incomeRepository)
        {
            _porfolioItemRepository = portfolioItemRepository;
            _securityDataRepository = securityDataRepository;
            _incomeRepository = incomeRepository;

            Messenger.Default.Register<LoadPortfolioMessage>(this, async (msg) =>
            {
                IEnumerable<PortfolioItem> portfolioItemList = await this._porfolioItemRepository.GetAll();
                IEnumerable<Income> incomeList = await this._incomeRepository.GetAll();

                foreach(PortfolioItem item in portfolioItemList)
                {
                    SecurityData data = null;

                    if (item.Code != "RUR")
                    {
                        data = await this._securityDataRepository.GetById(item.Code);
                    }
                    else
                    {
                        data = new SecurityData()
                        {
                            MarketData = new MarketData() { LCURRENTPRICE = 1m }
                        };
                    }

                    item.Price = data.MarketData.LCURRENTPRICE != 0 ? data.MarketData.LCURRENTPRICE : data.SecurityInfo.PREVLEGALCLOSEPRICE;
                }

                decimal PorfolioPrice = portfolioItemList.Sum(p => p.Value);
                decimal IncomeTotal = incomeList.Sum(i => i.Value);

                foreach (PortfolioItem item in portfolioItemList)
                {
                    item.Perc = Math.Round((item.Value / PorfolioPrice) * 100, 2);
                }

                Messenger.Default.Send<PortfolioLoadedMessage>(new PortfolioLoadedMessage()
                {
                    PortfolioItems = portfolioItemList,
                    PortfolioPrice = PorfolioPrice,
                    IncomeTotal = IncomeTotal
                });
            });
        }
    }
}
