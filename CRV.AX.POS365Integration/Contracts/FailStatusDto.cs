using System;

namespace CRV.AX.POS365Integration.Contracts
{
    public class FailStatusDto: BaseDto
    {
        public FailStatusDto(BaseParams _base_params) : base(_base_params) => ResponseStatus = new FailStatusInformationDto();

        public FailStatusInformationDto ResponseStatus { get; set; }
    }

    public class FailStatusInformationDto
    {
        public string ErrorCode { get; set; }

        public string Message { get; set; }
    }
}
