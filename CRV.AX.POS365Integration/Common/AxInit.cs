using System.Collections.Generic;
using AXLogExtension.Common;

namespace CRV.AX.POS365Integration.Common
{
    public static class AxInit
    {
        public static string AxEnvironment { get; set; }

        public static string FunctionName { get; set; }

        public static bool IsSingleRunFuntion { get; set ; }

        public static void Init(string[] args)
        {

            // Set default value
            AxEnvironment = nameof(AxEnum.AxEnvironments.UAT);
            AxWriteLineAndLog.IsWriteToConsole = true;

            #region Environment
            List<string> acceptedEnvironments = new List<string>(new string[]
            {
                AxEnum.AxEnvironments.DEV.ToString(),
                AxEnum.AxEnvironments.TEST.ToString(),
                AxEnum.AxEnvironments.UAT.ToString(),
                AxEnum.AxEnvironments.STAGE.ToString(),
                AxEnum.AxEnvironments.PROD.ToString()
            });

            foreach (string s in args)
            {
                string[] parts = s.Split('=');
                if (parts.Length == 2 && parts[0].ToUpper() == AxEnum.GetParameterKey(AxEnum.AxParameters.ENV))
                {
                    if (acceptedEnvironments.Contains(parts[1].Trim().ToUpper()))
                    {
                        AxEnvironment = parts[1].ToUpper();
                    }
                }
            }
            #endregion Environment

            #region Auto Mapper configuration

            #endregion Auto Mapper configuration
        }
    }
}
