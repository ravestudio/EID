﻿using EID.Library;
using EIDClient.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIDClient.Core.Repository
{
    public class DiaryNoteRepository : Repository<DiaryNote, int>
    {
        public DiaryNoteRepository(WebApiClient apiClient)
            : base(apiClient)
        {
            this.apiPath = "api/DiaryNote/";
        }
    }
}
