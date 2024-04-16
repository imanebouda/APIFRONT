using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ITKANSys_api.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string NomCompletUtilisateur { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string? Token { get; set; }      // Renommez la clé étrangère en CreatedById
        public DateTime created_at { get; set; }
        public DateTime?  updated_at { get; set; }
        public DateTime? deleted_at { get; set; }

    

        // Supprimez la propriété [ForeignKey("IdRole")]

        // Ajoutez la clé étrangère vers le modèle Role ici
           public int IdRole { get; set; }
           [ForeignKey("IdRole")]
           public Role UserRole { get; set; }
    }
}
