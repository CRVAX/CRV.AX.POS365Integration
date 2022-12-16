namespace CRV.AX.POS365Integration.Common
{
    public static class POS365URL
    {
        public static string BaseURL { get; set; }

        public static POS365_Store Store { get => new POS365_Store(BaseURL); }

        public static POS365URL_Partner Partner { get => new POS365URL_Partner(BaseURL); }

        public static POS365URL_Product Product { get => new POS365URL_Product(BaseURL); }

        public static POS365URL_Account Account { get => new POS365URL_Account(BaseURL); }

        public static POS365URL_Order Order { get => new POS365URL_Order(BaseURL); }
    }

    public class POS365_Store
    {
        private readonly string _baseURL;

        public POS365_Store(string baseURL) => _baseURL = baseURL;

        public string Login { get => $"{_baseURL}/{BaseURL.Store.Login}"; }
    }

    public class POS365URL_Partner
    {
        private readonly string _baseURL;

        public POS365URL_Partner(string baseURL) => _baseURL = baseURL;

        public string Get { get => $"{_baseURL}/{BaseURL.Partner.Get}"; }

        public string Create { get => $"{_baseURL}/{BaseURL.Partner.Create}"; }

        public string Update { get => $"{_baseURL}/{BaseURL.Partner.Update}"; }

        public string Delete { get => $"{_baseURL}/{BaseURL.Partner.Delete}"; }
    }

    public class POS365URL_Product
    {
        private readonly string _baseURL;

        public POS365URL_Product(string baseURL) => _baseURL = baseURL;

        public string Get { get => $"{_baseURL}/{BaseURL.Product.Get}"; }

        public string Create { get => $"{_baseURL}/{BaseURL.Product.Create}"; }

        public string Update { get => $"{_baseURL}{BaseURL.Product.Update}"; }

        public string Delete { get => $"{_baseURL}/{BaseURL.Product.Delete}"; }
    }

    public class POS365URL_Account
    {
        private readonly string _baseURL;

        public POS365URL_Account(string baseURL) => _baseURL = baseURL;

        public string Get { get => $"{_baseURL}/{BaseURL.Account.Get}"; }

        public string Create { get => $"{_baseURL}/{BaseURL.Account.Create}"; }

        public string Update { get => $"{_baseURL}{BaseURL.Account.Update}"; }

        public string Delete { get => $"{_baseURL}/{BaseURL.Account.Delete}"; }
    }

    public class POS365URL_Order
    {
        private readonly string _baseURL;

        public POS365URL_Order(string baseURL) => _baseURL = baseURL;

        public string Create { get => $"{_baseURL}/{BaseURL.Order.Create}"; }

        public string Update { get => $"{_baseURL}{BaseURL.Order.Update}"; }
    }
}
