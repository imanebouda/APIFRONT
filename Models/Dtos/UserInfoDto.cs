namespace ITKANSys_api.Data.Dtos
{
    public class UserInfoDto
    {
        public int Id { get; set; }
        public string NomCompletUtilisateur { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string RolesName { get; set; }
        public int IdRole { get; set; }
    }
}
