using ITKANSys_api.Config;
using ITKANSys_api.Interfaces;
using ITKANSys_api.Models.Entities.Param;
using ITKANSys_api.Utility.ApiResponse;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ITKANSys_api.Services.Param
{
    public class CategorieService : ICategorieService
    {
        private IConfiguration configuration;
        private ApplicationDbContext _ctx;

        public CategorieService(ApplicationDbContext context)
        {
            configuration = AppConfig.GetConfig();
            _ctx = context;
        }

        public async Task<DataSourceResult> Search(Object record)
        {
            DataSourceResult dataResult = new DataSourceResult();
            dynamic body = "";

            int take;
            int skip;

            string libelle;

            try
            {
                record = record.ToString().Replace("ValueKind = Object : ", "");

                if (record == null || record.ToString().Length == 0)
                {
                    dataResult.codeReponse = CodeReponse.error;
                    dataResult.msg = configuration.GetSection("MessagesAPI:ParamsEmpty").Value;
                }
                else
                {
                    body = JObject.Parse(record.ToString());

                    libelle = (body.libelle != "" ? (string)body.libelle : null);

                    take = Int32.Parse((String)body.take);
                    skip = Int32.Parse((String)body.skip);

                    using (_ctx)
                    {
                        var categories = await _ctx.Categories
                            .Where(c => c.deleted_at == null
                             && (libelle == null || (libelle != null && c.Libelle.ToLower().Contains(libelle.ToLower()))))
                            .AsNoTracking()
                            .Select(c => new
                            {
                                c.ID,
                                c.Libelle,
                            })
                            .ToListAsync();

                        var resultTake = categories.Skip(skip).Take(take);

                        dataResult.TotalRows = categories.Count();
                        dataResult.NbRows = resultTake.Count();
                        dataResult.codeReponse = CodeReponse.ok;
                        dataResult.data = resultTake;
                        dataResult.msg = "success";
                    }
                }
            }
            catch (Exception ex)
            {
                dataResult.codeReponse = CodeReponse.error;
                dataResult.msg = (ex.InnerException == null) ? ex.Message : ex.InnerException.Message;
            }

            return dataResult;
        }

        public async Task<DataSourceResult> Insert(Object record)
        {
            DataSourceResult dataResult = new DataSourceResult();
            dynamic body;

            try
            {
                record = record.ToString().Replace("ValueKind = Object : ", "");

                if (record == null || record.ToString().Length == 0)
                {
                    dataResult.codeReponse = CodeReponse.errorMissingAllParams;
                    dataResult.msg = configuration.GetSection("MessagesAPI:ParamsEmpty").Value;
                }
                else
                {
                    body = JObject.Parse(record.ToString());

                    if (!IsValidInsert(body))
                    {
                        dataResult.codeReponse = CodeReponse.errorInvalidMissingParams;
                        dataResult.msg = configuration.GetSection("MessagesAPI:ParamsInvalid").Value;
                    }
                    else
                    {
                        var data = new Categories
                        {
                            Libelle = body.libelle,
                        };

                        data.created_at = DateTime.Now;

                        using (_ctx)
                        {
                            await _ctx.Categories.AddAsync(data);
                            await _ctx.SaveChangesAsync();

                            dataResult.codeReponse = CodeReponse.ok;
                            dataResult.msg = "Ajouté";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                dataResult.codeReponse = CodeReponse.error;
                dataResult.msg = (ex.InnerException == null) ? ex.Message : ex.InnerException.Message;
            }

            return dataResult;
        }

        public async Task<DataSourceResult> Update(Object record)
        {
            DataSourceResult dataResult = new DataSourceResult();
            dynamic body;
            int ID = 0;

            try
            {
                record = record.ToString().Replace("ValueKind = Object : ", "");

                if (record == null || record.ToString().Length == 0)
                {
                    dataResult.codeReponse = CodeReponse.errorMissingAllParams;
                    dataResult.msg = configuration.GetSection("MessagesAPI:ParamsEmpty").Value;
                }
                else
                {
                    body = JObject.Parse(record.ToString());

                    if (!IsValidInsert(body))
                    {
                        dataResult.codeReponse = CodeReponse.errorInvalidMissingParams;
                        dataResult.msg = configuration.GetSection("MessagesAPI:ParamsInvalid").Value;
                    }
                    else
                    {
                        int.TryParse((string)body.ID, out ID);

                        if (ID == 0)
                        {
                            dataResult.codeReponse = CodeReponse.error;
                            dataResult.msg = configuration.GetSection("MessagesAPI:ParamsInvalid").Value + " id doit être un entier.";
                        }
                        else
                        {
                            using (_ctx)
                            {
                                Categories categorie = _ctx.Categories.Where(c => c.deleted_at == null && c.ID == ID).FirstOrDefault();

                                if (categorie != null)
                                {
                                    categorie.Libelle = body.Libelle;
                                    categorie.updated_at = DateTime.Now;

                                    await _ctx.SaveChangesAsync();

                                    dataResult.codeReponse = CodeReponse.ok;
                                    dataResult.msg = "Modifié";
                                }
                                else
                                {
                                    dataResult.codeReponse = CodeReponse.error;
                                    dataResult.msg = configuration.GetSection("MessagesAPI:UnknownRecord").Value;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                dataResult.codeReponse = CodeReponse.error;
                dataResult.msg = (ex.InnerException == null) ? ex.Message : ex.InnerException.Message;
            }

            return dataResult;
        }

        public async Task<DataSourceResult> Delete(Object record)
        {
            dynamic body;
            DataSourceResult dataResult = new DataSourceResult();
            int id;

            try
            {
                record = record.ToString().Replace("ValueKind = Object : ", "");

                if (record == null || record.ToString().Length == 0)
                {
                    dataResult.codeReponse = CodeReponse.errorMissingAllParams;
                    dataResult.msg = configuration.GetSection("MessagesAPI:ParamsEmpty").Value;
                }
                else
                {
                    body = JObject.Parse(record.ToString());

                    if (body.id == null)
                    {
                        dataResult.codeReponse = CodeReponse.errorInvalidMissingParams;
                        dataResult.msg = configuration.GetSection("MessagesAPI:ParamsInvalid").Value;
                    }
                    else
                    {
                        int.TryParse((string)body.id, out id);

                        if (id == 0)
                        {
                            dataResult.codeReponse = CodeReponse.error;
                            dataResult.msg = configuration.GetSection("MessagesAPI:ParamsInvalid").Value + " id doit être un entier.";
                        }
                        else
                        {
                            using (_ctx)
                            {
                                Categories categorie = _ctx.Categories
                                    .Where(c => c.deleted_at == null && c.ID == id)
                                    .FirstOrDefault();

                                if (categorie != null)
                                {
                                    categorie.deleted_at = DateTime.Now;
                                    await _ctx.SaveChangesAsync();

                                    dataResult.codeReponse = CodeReponse.ok;
                                    dataResult.msg = "Supprimé";
                                }
                                else
                                {
                                    dataResult.codeReponse = CodeReponse.error;
                                    dataResult.msg = configuration.GetSection("MessagesAPI:UnknownRecord").Value;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                dataResult.codeReponse = CodeReponse.error;
                dataResult.msg = (ex.InnerException == null) ? ex.Message : ex.InnerException.Message;
            }

            return dataResult;
        }

        private bool IsValidInsert(dynamic body)
        {
            bool result = true;

            if (body.Libelle == null)
            {
                result = false;
            }

            return result;
        }
    }
}
