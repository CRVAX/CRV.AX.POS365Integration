namespace CRV.AX.POS365Integration.Contracts.Stores
{
    /// <summary>
    /// User / Password and URL to login to POS365
    /// </summary>
    public class StoreInputDto
    {
        public string StoreNumber { get; set; }

        public string URL { get; set; }

        public string User { get; set; }

        public string Password { get; set; }
    }
}
