namespace ITKANSys_api.Data.Dtos
{
    public class UserDto
    {
        public bool IsSucceed { get; set; }
        public string Message { get; set; }
        public List<UserInfoDto> Data { get; set; }
    }
}
