using ITKANSys_api.Data.Dtos;
using ITKANSys_api.Models;
using ITKANSys_api.Models.Dtos;

namespace ITKANSys_api.Utility.ApiResponse
{
    public class GetDataResult
    {
        public bool IsSucceed { get; set; }
        public string Message { get; set; }
        public ProcessusDto data { get; set; }
        public ProcesDocumentsDto Data { get; set; }
        public ProcedureDto DataProcedure { get; set; }
        public ProcDocumentsDto ProcDocument { get; set; }
        public ResultatsIndicateursDto ResultatsIndicateurs { get; set; }
        public IndicateursDto DataIndicateur { get; set; }
        public PQDto DataPQ { get; set; }
        public MQDto DataMQ { get; set; }    
    }
}