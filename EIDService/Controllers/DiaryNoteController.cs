using EIDService.Common.DataAccess;
using EIDService.Common.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace EIDService.Controllers
{
    public class DiaryNoteController : ApiController
    {
        // GET api/deal
        public IEnumerable<DiaryNote> Get()
        {
            IEnumerable<DiaryNote> notes = null;

            using (UnitOfWork unit = new UnitOfWork((DbContext)new DataContext()))
            {
                notes = unit.DiaryNoteRepository.All<DiaryNote>(null).ToList();
            }

            return notes;
        }
    }
}
