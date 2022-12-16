namespace CRV.AX.POS365Integration
{
    public class AxConstants
    {
        public const int AX_API_RESULT_SUCCESS_STATUS_GROUP = 200;
        public const int AX_API_RESULT_SUCCESS_STATUS_CODE = 200;
        public const int AX_API_RESULT_EXCEPTION_STATUS_CODE = 999;

        // public const string AX_ROOT_FOLDER = @"D:\AXMiddleware\POS365";             // DeloymentPC
        public const string AX_ROOT_FOLDER = @"\\CRHQMIDAPPS01\AXMiddleware\POS365";   // Local PC
    }

    public static class AxEnum
    {
        public enum AxEnvironments
        {
            DEV,
            TEST,
            STAGE,
            UAT,
            PROD
        }

        public enum AxParameters
        {
            /// <summary>
            /// Environment
            /// </summary>
            ENV
        }

        public static string GetParameterKey(AxParameters param)
        {
            return $"--{param}";
        }

        public enum AxPOS365ExportType
        {
            All,
            Stores,
            Transactions,
            TransactionSales,
            PaymentTrans,
            Customers,
            Tenders,
            Products
        }
    }

    public static class AxPOS365FileName
    {
        public const string AX_STORE = "POS365_Stores";
        public const string AX_CUSTOMER = "POS365_Customers";
        public const string AX_TENDER = "POS365_Tenders";
        public const string AX_PRODUCT = "POS365_Products";
        public const string AX_TRANSACTION = "POS365_Transactions";
        public const string AX_TRANSACTIONSALE = "POS365_TransactionSales";
        public const string AX_PAYMENTTRANS = "POS365_PaymentTrans";
    }   
}
