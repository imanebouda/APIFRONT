using System.ComponentModel.DataAnnotations;
/// <summary>
/// Enum de gestion des codes retour des API
/// </summary>
public enum CodeReponse
{
    /// <summary>
    /// API exécutée avec succès
    /// </summary>
    ok = 200,

    /// <summary>
    /// IP non authorisée par les WS Certus
    /// </summary>
    unauthorizedIP = 620,

    /// <summary>
    /// Exécution non authorisée
    /// </summary>
    unauthorized = 401,

    /// <summary>
    /// API a retourné une erreur
    /// </summary>
    error = 500,

    /// <summary>
    /// Paramètres d'appel vide
    /// </summary>
    errorMissingAllParams = 501,

    /// <summary>
    /// Paramètre(s) manquant(s) ou invalide(s)
    /// </summary>
    errorInvalidMissingParams = 502,

    /// <summary>
    /// Format paramètre(s) invalide
    /// </summary>
    errorInvalidParams = 503,
    erreur = 621,
    errorMissingFile = 622
}
public enum Status
{
    [Display(Name = "Nouveaux")]
    perime = 0,

    [Display(Name = "En cours")]
    InProgress = 1,

    [Display(Name = "En cours")]
    InStop = -1,

    [Display(Name = "Terminé")]
    Completed = 2,

    [Display(Name = "Acceptée")]
    Accepter = 3,

    [Display(Name = "Rejetée")]
    Rejeter = 4,

    [Display(Name = "Clôture")]
    Closinge = 5

}
public enum StatusDdp
{
    [Display(Name = "Nouveau")]
    InitialDdp = 0,

    [Display(Name = "Approuvée")]
    ApprouveeDdp = 0,
}
public enum StatusOds
{
    [Display(Name = "Nouveau")]
    InitialODS = 0,

    [Display(Name = "Installation en attente")]
    InitialEnAttente = 1,

    [Display(Name = "Installation en cours")]
    InProgressInstallation = 2,

    [Display(Name = "Installation terminée")]
    CompletedInstallation = 3,

    [Display(Name = "Vérification en cours")]
    InProgressVerification = 4,

    [Display(Name = "Vérification terminée")]
    CompletedVerification = 5,

    [Display(Name = "Installation Rejetée")]
    RejeterInstallation = 6,

    [Display(Name = "Installation Acceptée")]
    AccepterInstallation = 7,

    [Display(Name = "Vérification Rejetée")]
    RejeterVerification = 8,

    [Display(Name = "Vérification Acceptée")]
    AccepterVerification = 9,

    [Display(Name = "Clôture")]
    Closinge = 10
}

