using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ITKANSys_api.Utility.ApiResponse
{
    public class formatObject
    {
        public dynamic ConvertObjectToDynamic(object obj)
        {
            // Convertir l'objet en chaîne JSON
            string jsonString = JsonConvert.SerializeObject(obj);

            // Parser la chaîne JSON en un objet dynamique
            dynamic body = JObject.Parse(jsonString);

            return body;
        }

    }
}
