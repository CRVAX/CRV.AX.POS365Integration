using System.Collections.Generic;

namespace CRV.AX.POS365Integration.Contracts.Partners
{
    public class PartnerGetSuccessDto
    {
        public int __Count { get; set; }

        public List<PartnerGetResultDto> Results { get; set; }
    }

    public class PartnerGetResultDto
    {
        public long Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public string Phone { get; set; }
    }
}
