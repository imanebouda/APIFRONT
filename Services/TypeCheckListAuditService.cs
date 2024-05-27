using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ITKANSys_api.Models.Entities;
using ITKANSys_api.Controllers;
using ITKANSys_api.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ITKANSys_api.Services
{
    public class TypeCheckListAuditService : ITypeCheckListAuditService
    {
        private static List<TypeCheckListAudit> typeCheckListAudits = new List<TypeCheckListAudit>
        {
            /*  new TypeCheckListAudit
              {   id= 1,
                  name = "String",
                  niveau = "String",
                  code = "001247",
                  description = "TypeCheckListAudit 1"
              },
              new TypeCheckListAudit
              {   id= 2,
                  name = "Stringttttt",
                  niveau = "String",
                  code = "003277",
                  description = "TypeCheckListAudit 2"
              }*/
        };
        private ApplicationDbContext _context;
        public TypeCheckListAuditService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<TypeCheckListAudit>> getAllTypeCheckListAudit()
        {
            var TypeCheckListAudits = await _context.TypeCheckList.ToListAsync();
            return TypeCheckListAudits;
        }
        /* public List<TypeCheckListAudit> getAllTypeCheckListAudit()
         {
             return TypeCheckListAudits;
         }*/
        public List<TypeCheckListAudit> addTypeCheckListAudit(TypeCheckListAudit nTypeCheckListAudit)
        {
            typeCheckListAudits.Add(nTypeCheckListAudit);
            return typeCheckListAudits;
        }

        /* public List<TypeCheckListAudit>? deleteTypeCheckListAudit(int CheckListId)
         {
             var TypeCheckListAudit = TypeCheckListAudits.Find(x => x.id == CheckListId);
             if (TypeCheckListAudit is null)
                 return null;

             TypeCheckListAudits.Remove(TypeCheckListAudit);

             return TypeCheckListAudits;
         }*/
        public async Task<List<TypeCheckListAudit>?> deleteTypeCheckListAudit(int id)
        {
            var TypeCheckListAuditAudit = await _context.TypeCheckList.FindAsync(id);
            if (TypeCheckListAuditAudit == null)
                return null;

            _context.TypeCheckList.Remove(TypeCheckListAuditAudit);
            await _context.SaveChangesAsync();

            return await getAllTypeCheckListAudit(); // Retourner la liste mise à jour des SiteAudits
        }

        public async Task<List<TypeCheckListAudit>?> updateTypeCheckListAudit(int id, TypeCheckListAudit request)
        {

            var checkList = await _context.TypeCheckList.FindAsync(id);
            if (checkList == null)
                return null;

            checkList.type = request.type;

            await _context.SaveChangesAsync();

            return await getAllTypeCheckListAudit(); // Retourner la liste mise à jour des SiteAudits
        }


        public async Task<TypeCheckListAudit?> getTypeCheckListAudit(int id)
        {
            var typeCheckList = await _context.TypeCheckList.FindAsync(id);


            return typeCheckList;
        }

    }

}

