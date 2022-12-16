using System;
using System.IO;
using System.Threading.Tasks;

namespace CRV.AX.POS365Integration.Common
{
    public static class TextFileHelper
    {
        public static async Task<(bool, string, string[])> ReadFileAsync(string filePath)
        {
            return await Task.Run(() =>
            {
                try
                {
                    string[] result = File.ReadAllLines(filePath);
                    return (true, "Read file successfully.", result);
                }
                catch (FileNotFoundException fnfEx)
                {
                    string fnfExMsg = $"TextFileHelper.ReadFileAsync(filePath: {filePath})\nFileNotFoundException: {fnfEx.InnerException?.Message ?? fnfEx.Message}";
                    Console.WriteLine(fnfExMsg);
                    return (false, fnfExMsg, null);
                }
                catch (Exception ex)
                {
                    string exMsg = $"TextFileHelper.ReadFileAsync(filePath: {filePath})\nException: {ex.InnerException?.Message ?? ex.Message}";
                    Console.WriteLine(exMsg);
                    return (false, exMsg, null);
                }
            });
        }

        public static async Task<(bool, string)> WriteFileAsync(string filePath, string[] textContent)
        {
            return await Task.Run(() =>
            {
                try
                {
                    File.AppendAllLines(filePath, textContent);
                    return (true, "Write to file successfully.");
                }
                catch (UnauthorizedAccessException uaEx)
                {
                    string uaExMsg = $"TextFileHelper.WriteFileAsync(filePath: {filePath})\nUnauthorizedAccessException: {uaEx.InnerException?.Message ?? uaEx.Message}";
                    Console.WriteLine(uaExMsg);
                    return (false, uaExMsg);
                }
                catch (Exception ex)
                {
                    string exMsg = $"TextFileHelper.WriteFileAsync(filePath: {filePath})\nException: {ex.InnerException?.Message ?? ex.Message}";
                    Console.WriteLine(exMsg);
                    return (false, exMsg);
                }
            });
        }

        public static async Task<(bool, string)> RenameFileAsync(string oldFilePath, string newFilePath)
        {
            return await Task.Run(() =>
            {
                try
                {
                    File.Move(oldFilePath, newFilePath);
                    return (true, "Reanme successfully.");
                }
                catch (FileNotFoundException fnfEx)
                {
                    string fnfExMsg = $"TextFileHelper.ReadFileAsync(filePath: {oldFilePath}, newFilePath: {newFilePath})\nFileNotFoundException: {fnfEx.InnerException?.Message ?? fnfEx.Message}";
                    Console.WriteLine(fnfExMsg);
                    return (false, fnfExMsg);
                }
                catch (Exception ex)
                {
                    string exMsg = $"TextFileHelper.RenameFile(oldFilePath: {oldFilePath}, newFilePath: {newFilePath})\nException: {ex.InnerException?.Message ?? ex.Message}";
                    Console.WriteLine(exMsg);
                    return (false, exMsg);
                }
            });
        }

        public static async Task<(bool, string)> AxFastRenameAsync(string filePath)
        {
            string newFileName = $"{new FileInfo(filePath).Name}";
            string path = Directory.GetParent(filePath).FullName;
            string processedPath = await AxFolder.GetProcessedDirectoryAsync(path);
            processedPath = await AxFolder.CreateFolderifNotExistedAsync($"{processedPath}\\{DateTime.Now.ToString("yyyyMMdd")}");
            string newFilePath = $@"{processedPath}\{newFileName}";

            return await RenameFileAsync(filePath, newFilePath);
        }

        public static string Right(this string str, int len) => string.IsNullOrEmpty(str) ? string.Empty : (str.Length <= len ? str : str.Substring(str.Length - len));

        public static string Left(this string str, int len) => string.IsNullOrEmpty(str) ? string.Empty : (str.Length <= len ? str : str.Substring(0, len - 1));
    }
}
