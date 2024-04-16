using ITKANSys_api.Config;
using ITKANSys_api.Utility.ApiResponse;
using System.Security.Claims;
using System;

namespace ITKANSys_api.Utility.Other
{
        /// <summary>
        /// Classe regroupant les méthodes de communication
        /// </summary>
        public class CustomHelper
        {
            //Gestionnaire de paramètres
            private IConfiguration configuration;

            //Contexte de la base de données
            private ApplicationDbContext _ctx;

            /// <summary>
            /// Constructeur par défaut
            /// </summary>
            public CustomHelper(ApplicationDbContext context)
            {
                configuration = AppConfig.GetConfig();
                _ctx = context;
            }

            /// <summary>
            /// Méthode pour récupérer l'id de l'utilisateur à partir du token
            /// </summary>
            /// <param name="claims"></param>
            /// <returns></returns>
            public static int get_id_user_FromClaims(IEnumerable<Claim> claims)
            {
                string id_user = null;
                Claim claimUser = claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Actor, StringComparison.InvariantCultureIgnoreCase));

                if (claimUser != null)
                {
                    id_user = claimUser.Value;
                    return int.Parse((string)id_user);
                }
                else
                {
                    return 0;
                }
            }

            /// <summary>
            /// Méthode de vérification de l'api key & url referer
            /// </summary>
            /// <param name="request">Requête HTTP</param>
            /// <returns></returns>
            public DataSourceResult CheckHeaderRequest(HttpRequest request)
            {
                //Déclaration des variables
                DataSourceResult dataResult = new DataSourceResult();
                Microsoft.Extensions.Primitives.StringValues traceValue;
                String separator = ";";
                String[] urlsList = null;
                string urls = "";

                //On récupère la clé de l'api
                request.Headers.TryGetValue(configuration.GetSection("Auth:KeyName").Value.ToString(), out traceValue);

                //On vérifie l'existence de la clé api
                if (traceValue.Count > 0)
                {
                    if (traceValue[0].ToString() != configuration.GetSection("Auth:KeyValue").Value)
                    {
                        //On définit le retour avec le détail de l'erreur
                        dataResult.codeReponse = CodeReponse.unauthorized;
                        dataResult.msg = configuration.GetSection("MessagesAPI:APIKeyIncorrect").Value;
                    }
                    else
                    {
                        //On récupère les urls autorisées
                        urls = configuration.GetSection("Auth:AuthorizedUrls").Value;
                        urlsList = urls.Split(separator);

                        //On vérifie l'url referer
                        //if (!urlsList.Contains(request.GetTypedHeaders().Referer.OriginalString))
                        //{
                        //    //On définit le retour avec le détail de l'erreur
                        //    dataResult.codeReponse = CodeReponse.unauthorized;
                        //    dataResult.msg = configuration.GetSection("MessagesAPI:UrlUnauthorized").Value;
                        //}
                    }
                }
                //Si aucune clé, on retourne un message d'erreur
                else
                {
                    //On définit le retour avec le détail de l'erreur
                    dataResult.codeReponse = CodeReponse.unauthorized;
                    dataResult.msg = configuration.GetSection("MessagesAPI:APIKeyEmpty").Value;
                }

                return dataResult;
            }
        }
}
