using CitizenFX.Core;
using static CitizenFX.Core.Native.API;

namespace AlsekLibShared
{
    public static class DebugLog
    {
        public static bool DebugMode = false;
        
        public enum LogLevel
        {
            none = 0,
            info = 1,
            success = 2,
            warning = 3,
            error = 4
        }

        /// <summary>
        /// Debug logging, only if debugmode is enabled OR it's a warning/error
        /// </summary>
        /// <param name="data"></param>
        public static void Log(dynamic data, bool newLine, bool overRideDebugMode, LogLevel level = LogLevel.none)
        {
            if (DebugMode || level == LogLevel.error || level == LogLevel.warning || overRideDebugMode)
            {
                string prefix = $"[{GetCurrentResourceName()}] ";
                if (level == LogLevel.info)
                {
                    prefix = $"[{GetCurrentResourceName()}] [INFO] ";
                }
                else if (level == LogLevel.success)
                {
                    prefix = $"[{GetCurrentResourceName()}] [SUCCESS] ";
                }
                else if (level == LogLevel.warning)
                {
                    prefix = $"[{GetCurrentResourceName()}] [WARNING] ";
                }
                else if (level == LogLevel.error)
                {
                    prefix = $"[{GetCurrentResourceName()}] [ERROR] ";
                }

                if (newLine)
                {
                    Debug.WriteLine($"\n{prefix}[DEBUG LOG] [{data.ToString()}]\n");
                }
                else
                {
                    Debug.WriteLine($"{prefix}[DEBUG LOG] [{data.ToString()}]");
                }
                //Debug.WriteLine($"{GetCurrentResourceName()}:{msg}");
            }
        }
    }
}