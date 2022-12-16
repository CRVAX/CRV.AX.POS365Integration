using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CRV.AX.POS365Integration.Common
{
    public static class AxFolder
    {
        public static async Task<string> GetCurrentDirectoryAsync() => await Task.Run(() => { return Directory.GetCurrentDirectory(); });

        public static async Task<string> CreateFolderifNotExistedAsync(string path)
        {
            return await Task.Run(() =>
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                return path;
            });
        }

        public static async Task<string> GetAxDirectoryAsync(string parentPath, string subFolderName)
        {
            string cur = string.IsNullOrEmpty(parentPath) ? await GetCurrentDirectoryAsync() : parentPath;
            return await CreateFolderifNotExistedAsync($@"{cur}\{subFolderName}");
        } 

        public static async Task<string> GetProcessedDirectoryAsync(string parentPath = null) => await GetAxDirectoryAsync(parentPath, "Processed");

        public static async Task<string> GetSucceededDirectoryAsync(string parentPath = null) => await GetAxDirectoryAsync(parentPath, "Succeeded");

        public static string GetAxFolder() => $@"{AxConstants.AX_ROOT_FOLDER}\{AxInit.AxEnvironment}";

        public static List<string> GetFiles(string path, AxEnum.AxPOS365ExportType exportType, string storeNumber)
        {
            string[] files = Directory.GetFiles(path);
            List<string> lstFiles = new List<string>();
            lstFiles.AddRange(files);
            lstFiles.RemoveAll(f => (new FileInfo(f).Name.StartsWith(".")));

            Dictionary<int, string> dicFileName = new Dictionary<int, string>
            {
                { (int)AxEnum.AxPOS365ExportType.Stores, $"{AxPOS365FileName.AX_STORE}" },
                { (int)AxEnum.AxPOS365ExportType.Customers, $"{AxPOS365FileName.AX_CUSTOMER}_{storeNumber}" },
                { (int)AxEnum.AxPOS365ExportType.Tenders, $"{AxPOS365FileName.AX_TENDER}_{storeNumber}" },
                { (int)AxEnum.AxPOS365ExportType.Products, $"{AxPOS365FileName.AX_PRODUCT}_{ storeNumber}" },
                { (int)AxEnum.AxPOS365ExportType.Transactions, $"{AxPOS365FileName.AX_TRANSACTION}_{ storeNumber}" },
                { (int)AxEnum.AxPOS365ExportType.TransactionSales, $"{AxPOS365FileName.AX_TRANSACTIONSALE}_{ storeNumber}" },
                { (int)AxEnum.AxPOS365ExportType.PaymentTrans, $"{AxPOS365FileName.AX_PAYMENTTRANS}_{ storeNumber}" }
            };

            lstFiles = lstFiles.Where(x => new FileInfo(x).Name.StartsWith(dicFileName[(int)exportType])).ToList();
            return lstFiles;
        }
    }
}
