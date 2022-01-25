using Common.Constants;
using System.IO;

namespace ToDoAppWeb.Auth
{
    public static class AuthInitializer
    {
        public static void Initialize()
        {
            File.WriteAllText(Constants.CurrentUserLogFileName, "{}");
        }
    }
}
