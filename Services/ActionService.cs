using ITKANSys_api.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ITKANSys_api.Services
{
    public class ActionService : IActionService
    {
        private readonly ApplicationDbContext _context;

        public ActionService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Models.Entities.Action>> GetAllActions()
        {
            return await _context.Actions
                .Include(a => a.user)
                .Include(a => a.typeAction)
                .Include(a => a.statusAction)
                .ToListAsync();
        }

        public async Task<Models.Entities.Action?> GetAction(int actionId)
        {
            return await _context.Actions
                .Include(a => a.user)
                .Include(a => a.typeAction)
                .Include(a => a.statusAction)
                .FirstOrDefaultAsync(a => a.ID == actionId);
        }

        public async Task<Models.Entities.Action> AddAction(Models.Entities.Action action)
        {
            // Vérifie l'existence de l'entité User
            var existingUser = await _context.Users.FindAsync(action.UserId);
            if (existingUser == null)
            {
                throw new ArgumentException("L'utilisateur spécifié n'existe pas dans la base de données.");
            }
            action.user = existingUser;

            // Vérifie l'existence de l'entité TypeAction
            var existingTypeAction = await _context.TypeActions.FindAsync(action.typeActionId);
            if (existingTypeAction == null)
            {
                throw new ArgumentException("Le type d'action spécifié n'existe pas dans la base de données.");
            }
            action.typeAction = existingTypeAction;

            // Vérifie l'existence de l'entité StatusAction
            var existingStatusAction = await _context.StatusActions.FindAsync(action.statusActionId);
            if (existingStatusAction == null)
            {
                throw new ArgumentException("Le statut d'action spécifié n'existe pas dans la base de données.");
            }
            action.statusAction = existingStatusAction;

            // Ajoute l'objet Action au contexte
            _context.Actions.Add(action);
            await _context.SaveChangesAsync();

            return action;
        }

        public async Task<Models.Entities.Action?> UpdateAction(int actionId, Models.Entities.Action request)
        {
            var existingAction = await _context.Actions.FindAsync(actionId);

            if (existingAction == null)
            {
                return null;
            }

            // Mise à jour des propriétés
            existingAction.libelle = request.libelle;
            existingAction.CreationDate = request.CreationDate;
            
            existingAction.description = request.description;

            if (request.UserId != 0) // Vérifie si UserId est fourni et différent de 0
            {
                var existingUser = await _context.Users.FindAsync(request.UserId);
                if (existingUser == null)
                {
                    throw new ArgumentException("L'utilisateur spécifié n'existe pas dans la base de données.");
                }
                existingAction.UserId = request.UserId;
                existingAction.user = existingUser;
            }

            if (request.typeActionId != 0) // Vérifie si typeActionId est fourni et différent de 0
            {
                var existingTypeAction = await _context.TypeActions.FindAsync(request.typeActionId);
                if (existingTypeAction == null)
                {
                    throw new ArgumentException("Le type d'action spécifié n'existe pas dans la base de données.");
                }
                existingAction.typeActionId = request.typeActionId;
                existingAction.typeAction = existingTypeAction;
            }

            if (request.statusActionId != 0) // Vérifie si statusActionId est fourni et différent de 0
            {
                var existingStatusAction = await _context.StatusActions.FindAsync(request.statusActionId);
                if (existingStatusAction == null)
                {
                    throw new ArgumentException("Le statut d'action spécifié n'existe pas dans la base de données.");
                }
                existingAction.statusActionId = request.statusActionId;
                existingAction.statusAction = existingStatusAction;
            }

            await _context.SaveChangesAsync();

            return existingAction;
        }

        public async Task<List<Models.Entities.Action>?> DeleteAction(int actionId)
        {
            var action = await _context.Actions.FindAsync(actionId);

            if (action == null)
            {
                return null;
            }

            _context.Actions.Remove(action);
            await _context.SaveChangesAsync();

            return await _context.Actions.ToListAsync();
        }
    }
}
