using ITKANSys_api.Models;
using Newtonsoft.Json;
using System.Collections;

namespace ITKANSys_api.Utility.ApiResponse
{
    /// <summary>
    /// Describes the result of Kendo DataSource read operation. 
    /// </summary>
    [Serializable]
    public class DataSourceResult
    {
        /// <summary>
        /// Code réponse (résultat du traitement)
        /// </summary>
        public CodeReponse codeReponse;

        /// <summary>
        /// Message (détail ou description de l'erreur)
        /// </summary>
        public string msg;

        public int TotalRows;


        public string Message;

        public bool IsSucceed;

        public int NbRows;


        /// <summary>
        /// Données retournées (résultat de l'exécution de l'api)
        /// </summary>
        public IEnumerable data { get; set; }

        /// <summary>
        /// Nombre d'enregistrements retournés (en fonction de la pagination)
        /// </summary>


        /// <summary>
        /// Constructeur avec data
        /// </summary>
        /// <param name="Data">Données à ajouter dans la réponse</param>
        public DataSourceResult(IEnumerable Data)
        {
            this.data = data;
        }

        [JsonProperty("resultData")]
        public object Data { get; set; }

        /// <summary>
        /// Constructeur par défaut
        /// </summary>
        public DataSourceResult()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="codeReponse">Code réponse de l'api</param>
        /// <param name="msg">Message retourné par l'api</param>
        public DataSourceResult(CodeReponse codeReponse, string msg)
        {
            this.codeReponse = codeReponse;
            this.msg = msg;
        }
    }
}
