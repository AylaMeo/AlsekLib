using CitizenFX.Core;

namespace AlsekLibServer
{
    public class CommonFunctionsLib
    {
        public static string GetPlayerLicense(Player p)
        {
            //string steamid = GetPlayerIdentifier(p.Handle, 0);
            if (string.IsNullOrWhiteSpace(p.Identifiers["license"]))
            {
                AlsekLibShared.DebugLog.Log($"Oh no {p.Name}, Doesn't have a identifier!?",false, false, AlsekLibShared.DebugLog.LogLevel.error);
                return null;
            }
            else
            {
                string licenseid = p.Identifiers["license"];
                return licenseid;
            }
        }
    }
}