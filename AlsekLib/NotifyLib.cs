using AlsekLibShared;
using static CitizenFX.Core.Native.API;

namespace AlsekLib
{
    /// <summary>
    /// Notifications class to easily show notifications using custom made templates,
    /// or completely custom style if none of the templates are fitting for the current task.
    /// </summary>
    public static class NotifyLib
    {
        /// <summary>
        /// Show a custom notification above the mini map.
        /// </summary>
        /// <param name="message">Message to display.</param>
        /// <param name="blink">Should the notification blink 3 times?</param>
        /// <param name="saveToBrief">Should the notification be logged to the brief (PAUSE menu > INFO > Notifications)?</param>
        public static void Custom(string message, bool blink = true, bool saveToBrief = true)
        {
            SetNotificationTextEntry("CELL_EMAIL_BCON"); // 10x ~a~
            foreach (string s in CitizenFX.Core.UI.Screen.StringToArray(message))
            {
                AddTextComponentSubstringPlayerName(s);
            }
            DrawNotification(blink, saveToBrief);
        }

        /// <summary>
        /// Show a notification with "Alert: " prefixed to the message.
        /// </summary>
        /// <param name="message">The message to be displayed on the notification.</param>
        /// <param name="blink">Should the notification blink 3 times?</param>
        /// <param name="saveToBrief">Should the notification be logged to the brief (PAUSE menu > INFO > Notifications)?</param>
        public static void Alert(string message, bool blink = true, bool saveToBrief = true)
        {
            Custom("~y~~h~Alert~h~~s~: " + message, blink, saveToBrief);
        }
        
        /// <summary>
        /// Show a notification with "Error: " prefixed to the message.
        /// </summary>
        /// <param name="message">The message to be displayed on the notification.</param>
        /// <param name="blink">Should the notification blink 3 times?</param>
        /// <param name="saveToBrief">Should the notification be logged to the brief (PAUSE menu > INFO > Notifications)?</param>
        public static void Error(string message, bool blink = true, bool saveToBrief = true)
        {
            Custom("~r~~h~Error~h~~s~: " + message, blink, saveToBrief);
            AlsekLibShared.DebugLog.Log(message, false, false, DebugLog.LogLevel.error);
        }

        /// <summary>
        /// Show a notification with "Info: " prefixed to the message.
        /// </summary>
        /// <param name="message">The message to be displayed on the notification.</param>
        /// <param name="blink">Should the notification blink 3 times?</param>
        /// <param name="saveToBrief">Should the notification be logged to the brief (PAUSE menu > INFO > Notifications)?</param>
        public static void Info(string message, bool blink = true, bool saveToBrief = true)
        {
            Custom("~b~~h~Info~h~~s~: " + message, blink, saveToBrief);
        }

        /// <summary>
        /// Show a notification with "Success: " prefixed to the message.
        /// </summary>
        /// <param name="message">The message to be displayed on the notification.</param>
        /// <param name="blink">Should the notification blink 3 times?</param>
        /// <param name="saveToBrief">Should the notification be logged to the brief (PAUSE menu > INFO > Notifications)?</param>
        public static void Success(string message, bool blink = true, bool saveToBrief = true)
        {
            Custom("~g~~h~Success~h~~s~: " + message, blink, saveToBrief);
        }

        /// <summary>
        /// Shows a custom notification with an image attached.
        /// </summary>
        /// <param name="textureDict"></param>
        /// <param name="textureName"></param>
        /// <param name="message"></param>
        /// <param name="title"></param>
        /// <param name="subtitle"></param>
        /// <param name="safeToBrief"></param>
        public static void CustomImage(string textureDict, string textureName, string message, string title, string subtitle, bool saveToBrief, int iconType = 0)
        {
            SetNotificationTextEntry("CELL_EMAIL_BCON"); // 10x ~a~
            foreach (string s in CitizenFX.Core.UI.Screen.StringToArray(message))
            {
                AddTextComponentSubstringPlayerName(s);
            }
            SetNotificationMessage(textureName, textureDict, false, iconType, title, subtitle);
            DrawNotification(false, saveToBrief);
        }
    }
}