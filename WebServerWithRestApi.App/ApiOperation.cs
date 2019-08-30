using Newtonsoft.Json.Linq;

namespace WebServerWithRestApi.App
{
    public class ApiOperation
    {
        [Operation("year")]
        public JObject CheckIfLeapYear(JObject data)
        {
            string result = "";
            int year = 0;
            try
            {
                year = (int)data.SelectToken("year");
            }
            catch
            {
                return new JObject(new JProperty("Error", "Invalid property name"));
            }
            if (year % 400 != 0)
            {
                if (year % 4 == 0 && year % 100 != 0)
                {
                    result += $"{year} is a leap year.";
                }
                else
                {
                    result += $"{year} is not a leap year.";
                }
            }
            else
            {
                result += $"{year} is a leap year.";
            }
            return new JObject(new JProperty("result", result));
        }
    }
}
