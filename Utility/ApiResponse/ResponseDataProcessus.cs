using ITKANSys_api.Data.Services;
using ITKANSys_api.Models;
using Newtonsoft.Json;
using System.Collections;

namespace ITKANSys_api.Utility.ApiResponse
{
    [Serializable]
    public class ResponseDataProcessus
    {
        public bool IsSucceed { get; set; }
        public string Message { get; set; }
        public Processus data { get; set; }
        [JsonProperty("Data")]
        public List<Processus> Data { get; set; }
        public IEnumerable DATA { get; set; }
        public int NbRows { get; set; }
        public int TotalRows { get; set; }





    }
}
