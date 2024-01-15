using CsvHelper;

/// <summary>
/// Classe représentant les prescriptions.
/// </summary>
public class Prescriptions
{
    /// <summary>
    /// Liste des prescriptions.
    /// </summary>
    public List<string> Prescription { get; set; } = new List<string>();

    /// <summary>
    /// Méthode pour obtenir une prescription aléatoire.
    /// </summary>
    /// <returns>Un tableau de chaînes de caractères représentant les mots de la prescription.</returns>
    public string[] GetPrescription()
    {
        // Création d'un objet Random pour générer des nombres aléatoires.
        Random random = new Random();

        // Sélection d'un index aléatoire dans la liste des prescriptions.
        int index = random.Next(Prescription.Count);

        // Renvoie la prescription à l'index sélectionné, divisée en mots.
        return Prescription[index].Split(' ');
    }
}