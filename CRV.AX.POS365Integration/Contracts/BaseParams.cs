using CsvHelper.Configuration.Attributes;
using System;

namespace CRV.AX.POS365Integration.Contracts
{
    public class BaseParams
    {
        public BaseParams() { }

        public BaseParams(string session, Guid? aXId, string storeNumber, long? id, string fileName)
        {
            Session = session;
            AXId = aXId;
            StoreNumber = storeNumber;
            Id = id;
            FileName = fileName;
        }

        public BaseParams(string session, Guid? aXId, string storeNumber, string fileName)
        {
            Session = session;
            AXId = aXId;
            StoreNumber = storeNumber;
            Id = 0;
            FileName = fileName;
        }

        [Ignore]
        public string Session { get; set; }

        [Ignore]
        public string FileName { get; set; }

        public Guid? AXId { get; set; }

        public string StoreNumber { get; set; }

        public long? Id { get; set; }
    }
}
