using AutoMapper;
using ITKANSys_api.Models;
using ITKANSys_api.Utility.ApiResponse;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System;

namespace ITKANSys_api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DropdownController : ControllerBase
    {
        private readonly List<string> Options = new List<string> { "Rq", "NC", "NCC" };

        private readonly ApplicationDbContext _context;
        public DropdownController(ApplicationDbContext context)
         {
            _context = context;
        }

        [HttpGet("options")]
        public ActionResult<IEnumerable<string>> GetOptions()
         {
            return Ok(Options);
        }





        [HttpPost("selected")]
        public IActionResult SaveSelectedChoice(int checklistId, UserChoice userChoice)
        {
            // Assurez-vous que l'ID de la liste de contrôle est valide
            if (checklistId <= 0)
            {
                return BadRequest("ID de la liste de contrôle non valide.");
            }

            // Vérifie si le modèle est valide
            if (ModelState.IsValid)
            {
                try
                {
                    // Associez l'ID de la liste de contrôle au choix de l'utilisateur
                    userChoice.CheckListAuditId = checklistId;

                    // Ajoute le choix de l'utilisateur à la base de données
                    _context.UserChoices.Add(userChoice);
                    _context.SaveChanges();

                    return Ok("Le choix a été enregistré avec succès pour la liste de contrôle spécifiée.");
                }
                catch (Exception ex)
                {
                    // Gère les erreurs d'ajout dans la base de données
                    return StatusCode(500, "Une erreur est survenue lors de l'enregistrement du choix.");
                }
            }
            else
            {
                // Si le modèle n'est pas valide, renvoie un message d'erreur
                return BadRequest("Les données du choix de l'utilisateur ne sont pas valides.");
            }
        }

    }



}


