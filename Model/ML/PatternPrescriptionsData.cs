/// <summary>
/// Classe représentant les données des prescriptions.
/// </summary>
class PatternPrescriptionsData
{
    /// <summary>
    /// Liste des fréquences de prescription.
    /// </summary>
    public static List<string> Frequency = new List<string>() {
        PatternPrescriptionsFrequency.Daily,
        PatternPrescriptionsFrequency.Morning,
        PatternPrescriptionsFrequency.Noon,
        PatternPrescriptionsFrequency.Evening,
        PatternPrescriptionsFrequency.OnceADay,
        PatternPrescriptionsFrequency.TwiceADay,
        PatternPrescriptionsFrequency.ThreeTimesADay,
        PatternPrescriptionsFrequency.EveryFourHours,
        PatternPrescriptionsFrequency.EverySixHours,
        PatternPrescriptionsFrequency.EveryFourToSixHours,
        PatternPrescriptionsFrequency.EveryTwoDays,
        PatternPrescriptionsFrequency.At6PM,
        PatternPrescriptionsFrequency.At8PM,
    };

    /// <summary>
    /// Liste des durées de prescription.
    /// </summary>
    public static List<string> Duration = new List<string>() {
        PatternPrescriptionsDuration.Day,
        PatternPrescriptionsDuration.Week,
        PatternPrescriptionsDuration.Month,
        PatternPrescriptionsDuration.Year,
    };

    /// <summary>
    /// Liste des formes de prescription.
    /// </summary>
    public static List<string> Form = new List<string>() {
        PatternPrescriptionsForm.Tablet,
        PatternPrescriptionsForm.Capsule,
        PatternPrescriptionsForm.Suppository,
        PatternPrescriptionsForm.Ovule,
        PatternPrescriptionsForm.Powder,
        PatternPrescriptionsForm.Solution,
        PatternPrescriptionsForm.Suspension,
        PatternPrescriptionsForm.Syrup,
        PatternPrescriptionsForm.Cream,
        PatternPrescriptionsForm.Ointment,
        PatternPrescriptionsForm.Gel,
        PatternPrescriptionsForm.Spray,
        PatternPrescriptionsForm.Drops,
        PatternPrescriptionsForm.TabletShort,
        PatternPrescriptionsForm.CapsuleShort,
        PatternPrescriptionsForm.SuppositoryShort,
    };
}