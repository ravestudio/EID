using EIDClient.Library;
using EIDService.Common.DataAccess;
using EIDService.Common.ISS;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EIDService.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult PrepareData(string sec, DateTime from)
        {
            MicexISSClient client = new MicexISSClient(new WebApiClient());
            var candles = client.GetCandles(sec, from, 1).Result;


            //using (DataContext context = new DataContext())
            //{
            //    context.Database.
            //}

            using (UnitOfWork unit = new UnitOfWork((DbContext)new DataContext()))
            {
                int key = 0;

                if (unit.CandleRepository.All<EIDService.Common.Entities.Candle>(null).Count() > 0)
                {
                    key = unit.CandleRepository.All<EIDService.Common.Entities.Candle>(null).Max(c => c.Id);
                }

                foreach (Candle candle in candles)
                {
                    key++;
                    EIDService.Common.Entities.Candle entity = new Common.Entities.Candle();
                    entity.Id = key;
                    entity.CandleDate = candle.begin.ToString("yyyyMMdd");
                    entity.CandleTime = candle.begin.ToString("HHmm00");
                    entity.MaxPrice = candle.high;
                    entity.OpenPrice = candle.open;
                    entity.ClosePrice = candle.close;
                    entity.Code = sec;

                    unit.CandleRepository.Create(entity);
                }

                unit.Commit();
            }

            return Json("ok", JsonRequestBehavior.AllowGet);
        }
    }
}