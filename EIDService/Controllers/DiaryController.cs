using EID.Library;
using EIDService.Common.DataAccess;
using EIDService.Common.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EIDService.Controllers
{
    public class DiaryController : Controller
    {
        // GET: Diary
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult CreateNotes()
        {
            using (UnitOfWork unit = new UnitOfWork((DbContext)new DataContext()))
            {
                var deals = unit.DealRepository.Query<Deal>(d => d.Processed == false, null).OrderBy(d => d.OrderNumber).ToList();

                IEnumerable<string> codes = deals.GroupBy(d => d.Code).Select(g => g.Key);

                foreach(string code in codes)
                {

                    AssignNote(deals.Where(d => d.Code == code).ToList(), unit);

                    unit.Commit();
                }
            }

            return Json("ok");
        }

        private void AssignNote(IList<Deal> deals, UnitOfWork unit)
        {
            string code = deals.First().Code;

            DiaryNote note = null;

            note = unit.DiaryNoteRepository.Query<DiaryNote>(n => n.Code == code && n.Close == null, new string[] { "NotePositions.Deal" }).SingleOrDefault();

            foreach (Deal deal in deals)
            { 

                if (note == null)
                {
                    note = new DiaryNote()
                    {
                        Code = code
                    };
                    unit.DiaryNoteRepository.Create(note);
                }

                if (note.Open == null)
                {
                    note.Open = deal.DateTime;

                    note.NoteType = deal.OrderOperation == OrderOperationEnum.Buy ? NoteType.Long : NoteType.Short;
                }

                int posCount = deal.Count;

                note.NotePositions.Add(new NotePosition()
                {
                    Deal = deal,
                    Count = posCount
                });

                //размер позиции
                note.Count = note.NoteType == NoteType.Long ? note.NotePositions.Where(p => p.Deal.OrderOperation == OrderOperationEnum.Buy).Sum(p => p.Count) : note.NotePositions.Where(p => p.Deal.OrderOperation == OrderOperationEnum.Sell).Sum(p => p.Count);

                int overFlow = note.NoteType == NoteType.Long ? note.NotePositions.Where(p => p.Deal.OrderOperation == OrderOperationEnum.Sell).Sum(p => p.Count) - note.Count : note.NotePositions.Where(p => p.Deal.OrderOperation == OrderOperationEnum.Buy).Sum(p => p.Count) - note.Count;

                //переполнение
                if (overFlow > 0)
                {
                    note.NotePositions.Last().Count -= overFlow;
                }

                if (note.NotePositions.Where(p => p.Deal.OrderOperation == OrderOperationEnum.Buy).Sum(p => p.Count) == note.NotePositions.Where(p => p.Deal.OrderOperation == OrderOperationEnum.Sell).Sum(p => p.Count))
                {
                    note.Close = deal.DateTime;

                    decimal openPrice = note.NoteType == NoteType.Long ? note.NotePositions.Where(p => p.Deal.OrderOperation == OrderOperationEnum.Buy).Sum(p => p.Deal.Price * p.Count) / note.Count : note.NotePositions.Where(p => p.Deal.OrderOperation == OrderOperationEnum.Sell).Sum(p => p.Deal.Price * p.Count) / note.Count;

                    decimal closePrice = note.NoteType == NoteType.Long ? note.NotePositions.Where(p => p.Deal.OrderOperation == OrderOperationEnum.Sell).Sum(p => p.Deal.Price * p.Count) / note.Count : note.NotePositions.Where(p => p.Deal.OrderOperation == OrderOperationEnum.Buy).Sum(p => p.Deal.Price * p.Count) / note.Count;

                    note.OpenPrice = openPrice;
                    note.ClosePrice = closePrice;

                    int lotSize = unit.SecurityRepository.Query<Security>(s => s.Code == code, null).Single().LotSize;

                    note.OpenValue = note.NoteType == NoteType.Long ? note.NotePositions.Where(p => p.Deal.OrderOperation == OrderOperationEnum.Buy).Sum(p => p.Deal.Price * p.Count * lotSize) : note.NotePositions.Where(p => p.Deal.OrderOperation == OrderOperationEnum.Sell).Sum(p => p.Deal.Price * p.Count * lotSize);
                    note.CloseValue = note.NoteType == NoteType.Long ? note.NotePositions.Where(p => p.Deal.OrderOperation == OrderOperationEnum.Sell).Sum(p => p.Deal.Price * p.Count * lotSize) : note.NotePositions.Where(p => p.Deal.OrderOperation == OrderOperationEnum.Buy).Sum(p => p.Deal.Price * p.Count * lotSize);
                    //create new note
                    note = null;
                }

                if (overFlow > 0)
                {
                    note = new DiaryNote()
                    {
                        Code = code
                    };
                    unit.DiaryNoteRepository.Create(note);

                    note.Open = deal.DateTime;

                    note.NoteType = deal.OrderOperation == OrderOperationEnum.Buy ? NoteType.Long : NoteType.Short;

                    note.NotePositions.Add(new NotePosition()
                    {
                        Deal = deal,
                        Count = overFlow
                    });
                }

                deal.Processed = true;
            }

        }
    }
}