namespace CRV.AX.POS365Integration.Common
{
    public static class BaseURL
    {
        public static BaseURL_Store Store { get => new BaseURL_Store(); }
        public static BaseURL_Partner Partner { get => new BaseURL_Partner(); }
        public static BaseURL_Product Product { get => new BaseURL_Product(); }
        public static BaseURL_Account Account { get => new BaseURL_Account(); }
        public static BaseURL_Order Order { get => new BaseURL_Order(); }
    }

    public class BaseURL_Store
    {
        public string Login { get => "api/auth/credentials?format=json"; }
    }

    public class BaseURL_Partner
    {
        public string Get { get => "api/partners?format=json&Type=1&Top={0}"; }

        public string Create { get => "api/partners"; }

        public string Update { get => "api/partners"; }

        public string Delete { get => "api/partners/{0}?format=json"; }
    }

    public class BaseURL_Product
    {
        public string Get { get => "api/products?format=json&Type=1&Top={0}"; }

        public string Create { get => "api/products"; }

        public string Update { get => "api/products"; }

        public string Delete { get => "api/products/{0}?format=json"; }
    }

    public class BaseURL_Account
    {
        public string Get { get => "api/accounts?format=json&Type=1&Top={0}"; }

        public string Create { get => "api/accounts"; }

        public string Update { get => "api/accounts"; }

        public string Delete { get => "api/accounts/{0}?format=json"; }
    }

    public class BaseURL_Order
    {
        public string Create { get => "api/orders"; }

        public string Update { get => "api/orders"; }
    }
}
