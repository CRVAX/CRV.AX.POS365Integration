namespace CRV.AX.POS365Integration.Contracts
{
    public class BaseRequestDto: BaseDto
    {        
        public virtual string Session { get; set; }

        public BaseRequestDto(BaseParams _base_params) : base(_base_params)
        {
            Session = _base_params?.Session;
        }
    }
}
