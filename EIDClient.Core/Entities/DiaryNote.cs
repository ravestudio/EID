using EID.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;

namespace EIDClient.Core.Entities
{
    public class DiaryNote : Entity<int>
    {
        public string Code { get; set; }

        public NoteType NoteType { get; set; }

        public DateTime? Open { get; set; }
        public DateTime? Close { get; set; }

        public decimal? OpenPrice { get; set; }
        public decimal? ClosePrice { get; set; }

        public decimal? OpenValue { get; set; }
        public decimal? CloseValue { get; set; }

        public int Count { get; set; }

        public decimal Profit
        {
            get
            {
                decimal value = 0;

                if (this.Close.HasValue)
                {
                    value = this.NoteType == NoteType.Long ? CloseValue.Value - OpenValue.Value : OpenValue.Value - CloseValue.Value;
                }

                return value;
            }
        }

        public override void ReadData(Windows.Data.Json.JsonObject jsonObj)
        {
            this.Id = (int)jsonObj["Id"].GetNumber();
            this.Code = jsonObj["Code"].GetString();

            this.NoteType = (NoteType)jsonObj["NoteType"].GetNumber();

            if (jsonObj["Open"].ValueType != JsonValueType.Null)
            {
                this.Open = DateTime.Parse(jsonObj["Open"].GetString());
            }

            if (jsonObj["Close"].ValueType != JsonValueType.Null)
            {
                this.Close = DateTime.Parse(jsonObj["Close"].GetString());
            }

            if (jsonObj["OpenPrice"].ValueType != JsonValueType.Null)
            {
                this.OpenPrice = (decimal)jsonObj["OpenPrice"].GetNumber();
            }

            if (jsonObj["ClosePrice"].ValueType != JsonValueType.Null)
            {
                this.ClosePrice = (decimal)jsonObj["ClosePrice"].GetNumber();
            }


            if (jsonObj["OpenValue"].ValueType != JsonValueType.Null)
            {
                this.OpenValue = (decimal)jsonObj["OpenValue"].GetNumber();
            }

            if (jsonObj["CloseValue"].ValueType != JsonValueType.Null)
            {
                this.CloseValue = (decimal)jsonObj["CloseValue"].GetNumber();
            }

            this.Count = (int)jsonObj["Count"].GetNumber();
        }
    }
}
