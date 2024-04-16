using ITKANSys_api.Models.Entities.Param;

namespace ITKANSys_api.Utility.ApiResponse
{
    public class ResponceData
    {
        public bool IsSucceed { get; set; }
        public string Message { get; set; }
        public Categories data { get; set; }
    }
}
