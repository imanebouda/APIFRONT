using ITKANSys_api.Data.OtherObjects;
using Microsoft.VisualBasic.FileIO;

namespace ITKANSys_api.Interfaces
{
    public class FileUploadModel
    {
        public IFormFile ProcesDocuments { get; set; }
        public int  ProcessusID {get ; set ;}
        public string Libelle { get; set; }
    }
}
