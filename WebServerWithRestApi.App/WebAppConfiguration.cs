using System.Collections.Generic;

namespace WebServerWithRestApi.App
{
    public class WebAppConfiguration
    {
        public Dictionary<string, string> WebApps { get; private set; } = new Dictionary<string, string>()
        {
            {"todo", @"C:\Users\rparkar\Desktop\Assignments\WebServerWithRestApi.App\WebApps\ToDo" }
        };
    }
}
