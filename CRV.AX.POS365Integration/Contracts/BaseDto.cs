using CsvHelper.Configuration.Attributes;
using System;

namespace CRV.AX.POS365Integration.Contracts
{
    public class BaseDto
    {
        public virtual Guid? AXId { get; set; }

        public virtual string StoreNumber { get; set; }

        /// <summary>
        /// POS365 Id.
        /// <para>Noted that: Order detail has no Id.</para>
        /// </summary>
        public virtual long? Id { get; set; }

        [Ignore]
        public virtual string FileName { get; set; }

        public BaseDto() { }

        public BaseDto(BaseParams _base_params)
        {
            AXId = _base_params?.AXId;
            StoreNumber = _base_params?.StoreNumber;
            Id = _base_params?.Id;
            FileName =  _base_params?.FileName;
        }
    }
}
