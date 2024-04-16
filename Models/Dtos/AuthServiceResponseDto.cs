namespace ITKANSys_api.Core.Dtos
{
    public class AuthServiceResponseDto
    {

        public bool IsSucceed { get; set; }
        public string Message { get; set; }
        public Dictionary<string, string> Data { get; set; }

    }
}
