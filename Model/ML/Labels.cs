/// <summary>
/// Classe représentant les étiquettes utilisées pour l'annotation des entités dans le texte.
/// </summary>
public class Labels
{
    /// <summary>
    /// Étiquette pour les mots qui ne correspondent à aucune entité.
    /// </summary>
    public const string O = "O";

    /// <summary>
    /// Étiquette pour le premier mot d'une entité de type "Doctor".
    /// </summary>
    public const string B_Doctor = "B-Doctor";

    /// <summary>
    /// Étiquette pour les mots suivants dans une entité de type "Doctor".
    /// </summary>
    public const string I_Doctor = "I-Doctor";

    /// <summary>
    /// Étiquette pour le premier mot d'une entité de type "Patient".
    /// </summary>
    public const string B_Patient = "B-Patient";

    /// <summary>
    /// Étiquette pour les mots suivants dans une entité de type "Patient".
    /// </summary>
    public const string I_Patient = "I-Patient";

    /// <summary>
    /// Étiquette pour le premier mot d'une entité de type "Prescription".
    /// </summary>
    public const string B_Prescription = "B-Prescription";

    /// <summary>
    /// Étiquette pour les mots suivants dans une entité de type "Prescription".
    /// </summary>
    public const string I_Prescription = "I-Prescription";

    /// <summary>
    /// Étiquette pour le premier mot d'une entité de type "Drug".
    /// </summary>
    public const string B_Drug = "B-Drug";

    /// <summary>
    /// Étiquette pour les mots suivants dans une entité de type "Drug".
    /// </summary>
    public const string I_Drug = "I-Drug";

    /// <summary>
    /// Étiquette pour le premier mot d'une entité de type "DrugForm".
    /// </summary>
    public const string B_DrugForm = "B-DrugForm";

    /// <summary>
    /// Étiquette pour les mots suivants dans une entité de type "DrugForm".
    /// </summary>
    public const string I_DrugForm = "I-DrugForm";

    /// <summary>
    /// Étiquette pour le premier mot d'une entité de type "DrugFrequency".
    /// </summary>
    public const string B_DrugFrequency = "B-DrugFrequency";

    /// <summary>
    /// Étiquette pour les mots suivants dans une entité de type "DrugFrequency".
    /// </summary>
    public const string I_DrugFrequency = "I-DrugFrequency";

    /// <summary>
    /// Étiquette pour le premier mot d'une entité de type "DrugDuration".
    /// </summary>
    public const string B_DrugDuration = "B-DrugDuration";

    /// <summary>
    /// Étiquette pour les mots suivants dans une entité de type "DrugDuration".
    /// </summary>
    public const string I_DrugDuration = "I-DrugDuration";

    /// <summary>
    /// Étiquette pour le premier mot d'une entité de type "DrugDosage".
    /// </summary>
    public const string B_DrugDosage = "B-DrugDosage";

    /// <summary>
    /// Étiquette pour les mots suivants dans une entité de type "DrugDosage".
    /// </summary>
    public const string I_DrugDosage = "I-DrugDosage";

    /// <summary>
    /// Étiquette pour le premier mot d'une entité de type "DrugRoute".
    /// </summary>
    public const string B_DrugRoute = "B-DrugRoute";

    /// <summary>
    /// Étiquette pour les mots suivants dans une entité de type "DrugRoute".
    /// </summary>
    public const string I_DrugRoute = "I-DrugRoute";

    /// <summary>
    /// Étiquette pour le premier mot d'une entité de type "DrugQuantity".
    /// </summary>
    public const string B_DrugQuantity = "B-DrugQuantity";

    /// <summary>
    /// Étiquette pour les mots suivants dans une entité de type "DrugQuantity".
    /// </summary>
    public const string I_DrugQuantity = "I-DrugQuantity";
}