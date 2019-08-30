namespace WebServerWithRestApi.App
{
    public class ApiOperationIdentifier
    {
        public string GetOperation(string apiTitle)
        {
            foreach (var property in typeof(ApiOperation).GetMethods())
            {
                var attributes = (OperationAttribute[])property.GetCustomAttributes (typeof(OperationAttribute), false);
                foreach (var attribute in attributes)
                {

                    if (apiTitle == attribute.Operation)
                        return property.Name;
                }
            }
            return "Operation not found...";
        }
    }
}
