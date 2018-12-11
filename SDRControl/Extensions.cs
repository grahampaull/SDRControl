using Newtonsoft.Json;

namespace SDRControl
{
    public static class Extensions
    {
        //Source from https://stackoverflow.com/a/51428508/2409660
        public static bool TryParseJson<T>(this string @this, out T result)
        {
            if(string.IsNullOrWhiteSpace(@this))
            {
                 result = default(T);
                 return false;
            }   

            bool success = true;
            var settings = new JsonSerializerSettings
            {
                Error = (sender, args) => { success = false; args.ErrorContext.Handled = true; },
                MissingMemberHandling = MissingMemberHandling.Error
            };
            result = JsonConvert.DeserializeObject<T>(@this, settings);
            return success;
        }
    }
}