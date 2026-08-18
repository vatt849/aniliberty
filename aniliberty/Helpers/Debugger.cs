using aniliberty.Common;
using System.Diagnostics;

namespace aniliberty.Helpers
{
    class Debugger
    {
        public static void WriteLine(string message, DebuggerCategory category = DebuggerCategory.Info) => Debug.WriteLine(message, category.GetStringValue());
    }

    public enum DebuggerCategory
    {
        [StringValue("info")]
        Info,
        [StringValue("navigation")]
        Navigation,
        [StringValue("api")]
        API,
        [StringValue("settings")]
        Settings,
        [StringValue("app")]
        App,
        [StringValue("player")]
        Player,
    }
}
