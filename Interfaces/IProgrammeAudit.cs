namespace ITKANSys_api.Interfaces
{
    public interface IProgrammeAudit
    {


        Task<List<ProgrammeAudit>> GetProgrammeAuditsForAudit(int auditId);

        Task<List<ProgrammeAudit>> GetProgrammeAudits();
    }
}
