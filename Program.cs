using System.Diagnostics;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using System.CommandLine;
using System.Text;

/// <summary>
/// Classe principale du programme.
/// </summary>
public class Program
{
    /// <summary>
    /// Options pour le sérialiseur JSON.
    /// </summary>
    public static JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions
    {
        WriteIndented = true,
        Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
    };

    /// <summary>
    /// Nom du fichier contenant les données de base des médicaments.
    /// </summary>
    const string CIS_bdpm = "CIS_bdpm";

    /// <summary>
    /// Nom du fichier contenant les données de composition des médicaments.
    /// </summary>
    const string CIS_COMPO_bdpm = "CIS_COMPO_bdpm";

    /// <summary>
    /// Nom du fichier contenant les données de présentation des médicaments.
    /// </summary>
    const string CIS_CIP_bdpm = "CIS_CIP_bdpm";

    /// <summary>
    /// Nom du fichier contenant les données des groupes génériques.
    /// </summary>
    const string CIS_GENER_bdpm = "CIS_GENER_bdpm";

    /// <summary>
    /// Nom du fichier contenant les données des avis SMR de la HAS.
    /// </summary>
    const string CIS_HAS_SMR_bdpm = "CIS_HAS_SMR_bdpm";

    /// <summary>
    /// Nom du fichier contenant les données des avis ASMR de la HAS.
    /// </summary>
    const string CIS_HAS_ASMR_bdpm = "CIS_HAS_ASMR_bdpm";

    /// <summary>
    /// Nom du fichier contenant les informations importantes.
    /// </summary>
    const string CIS_InfoImportantes = "CIS_InfoImportantes";

    /// <summary>
    /// Nom du fichier contenant les conditions de prescription et de délivrance.
    /// </summary>
    const string CIS_CPD_bdpm = "CIS_CPD_bdpm";

    /// <summary>
    /// Nom du fichier contenant les liens vers les avis de la Commission de la transparence.
    /// </summary>
    const string HAS_LiensPageCT_bdpm = "HAS_LiensPageCT_bdpm";

    /// <summary>
    /// Nom du fichier contenant les données de disposition spéciale.
    /// </summary>
    const string CIS_CIP_Dispo_Spec = "CIS_CIP_Dispo_Spec";

    /// <summary>
    /// Liste pour stocker les données des médicaments.
    /// </summary>
    static List<Medication> medications = new List<Medication>();

    /// <summary>
    /// Liste pour stocker les données de composition des médicaments.
    /// </summary>
    static List<MedicationComposition> medicationCompositions = new List<MedicationComposition>();

    /// <summary>
    /// Liste pour stocker les données de présentation des médicaments.
    /// </summary>
    static List<MedicationPresentation> medicationPresentations = new List<MedicationPresentation>();

    /// <summary>
    /// Liste pour stocker les données des groupes génériques.
    /// </summary>
    static List<GenericGroup> genericGroups = new List<GenericGroup>();

    /// <summary>
    /// Liste pour stocker les données des avis SMR de la HAS.
    /// </summary>
    static List<HasSmrOpinion> hasSmrOpinions = new List<HasSmrOpinion>();

    /// <summary>
    /// Liste pour stocker les données des avis ASMR de la HAS.
    /// </summary>
    static List<HasAsmrOpinion> hasAsmrOpinions = new List<HasAsmrOpinion>();

    /// <summary>
    /// Liste pour stocker les informations importantes.
    /// </summary>
    static List<ImportantInformation> importantInformations = new List<ImportantInformation>();

    /// <summary>
    /// Liste pour stocker les conditions de prescription et de délivrance.
    /// </summary>
    static List<PrescriptionDispensingConditions> prescriptionDispensingConditions = new List<PrescriptionDispensingConditions>();

    /// <summary>
    /// Liste pour stocker les liens vers les avis de la Commission de la transparence.
    /// </summary>
    static List<TransparencyCommissionOpinionLinks> transparencyCommissionOpinionLinks = new List<TransparencyCommissionOpinionLinks>();

    /// <summary>
    /// Point d'entrée principal de l'application.
    /// </summary>
    /// <param name="args">Arguments de la ligne de commande.</param>
    /// <returns>Code de sortie de l'application.</returns>
    public static async Task<int> Main(string[] args)
    {
        // Initialisation du chronomètre pour mesurer le temps d'exécution.
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        // Options de la ligne de commande.
        var modelOption = new Option<string>(
            new string[] { "--model", "-m" },
            description: "Modèle à utiliser : ai ou dataset ou database"
        );

        var datasetOption = new Option<string[]>(
            new string[] { "--dataset", "-d" },
            description: "Jeu de données à utiliser : CIS_bdpm, CIS_COMPO_bdpm, CIS_CIP_bdpm, CIS_GENER_bdpm, CIS_HAS_SMR_bdpm, CIS_HAS_ASMR_bdpm, CIS_InfoImportantes, CIS_CPD_bdpm, HAS_LiensPageCT_bdpm, all"
        );

        var mergeOption = new Option<bool>(
            new string[] { "--merge" },
            description: "Fusionner les jeux de données"
        );

        var outputDirOption = new Option<DirectoryInfo>(
            new string[] { "--outputDir", "-o" },
            description: "Répertoire de sortie"
        );

        var outputUrlOption = new Option<Uri>(
            new string[] { "--outputUrl", "-u" },
            description: "URL de sortie"
        );

        var sentenceCountOption = new Option<int>(
            new string[] { "--sentenceCount", "-s" },
            description: "Nombre de phrases à générer"
        );
        var inputOption = new Option<FileInfo>(
            new string[] { "--input", "-i" },
            description: "Fichier d'entrée"
        );

        // Commande principale.
        var rootCommand = new RootCommand("MedicAppToolsDatasets")
        {
            modelOption,
            datasetOption,
            mergeOption,
            outputDirOption,
            outputUrlOption,
            sentenceCountOption,
            inputOption
        };

        // Définition du gestionnaire de la commande.
        rootCommand.SetHandler(async (model, datasetInput, merge, outputDir, outputUrl, sentenceCount, inputDir) =>
        {
            try
            {
                string[] dataset = new string[] {
                        CIS_bdpm,
                        CIS_COMPO_bdpm,
                        CIS_CIP_bdpm,
                        CIS_GENER_bdpm,
                        CIS_HAS_SMR_bdpm,
                        CIS_HAS_ASMR_bdpm,
                        CIS_InfoImportantes,
                        CIS_CPD_bdpm,
                        HAS_LiensPageCT_bdpm,
                    };

                // Validation des arguments de la ligne de commande.
                if (model == "ai")
                {
                    if (datasetInput == null)
                    {
                        throw new Exception("Dataset is required for model dataset");
                    }
                    else
                    {
                        if (!datasetInput.Contains("all"))
                        {
                            dataset = dataset.Where(d => datasetInput.Contains(d)).ToArray();

                            if (dataset.Length == 0)
                            {
                                throw new Exception("Dataset is required for model dataset");
                            }
                        }
                    }
                    if (sentenceCount == 0)
                    {
                        throw new Exception("Sentence count is required for model ai");
                    }
                    if (inputDir == null)
                    {
                        throw new Exception("Input file is required for model ai");
                    }
                    else if (!inputDir.Exists)
                    {
                        throw new Exception("Input file does not exist");
                    }
                    else if (File.ReadAllLines(inputDir.FullName).Length < 1)
                    {
                        throw new Exception("Input file contains empty lines");
                    }
                    if (outputDir == null && outputUrl == null)
                    {
                        throw new Exception("Output directory or output URL is required for model ai");
                    }
                }
                else if (model == "dataset")
                {
                    if (datasetInput == null)
                    {
                        throw new Exception("Dataset is required for model dataset");
                    }
                    else
                    {
                        if (!datasetInput.Contains("all"))
                        {
                            dataset = dataset.Where(d => datasetInput.Contains(d)).ToArray();

                            if (dataset.Length == 0)
                            {
                                throw new Exception("Dataset is required for model dataset");
                            }
                        }
                    }

                    if (outputDir == null && outputUrl == null)
                    {
                        throw new Exception("Output directory or output URL is required for model dataset");
                    }
                }
                else if (model == "database")
                {
                    if (datasetInput == null)
                    {
                        throw new Exception("Dataset is required for model database");
                    }
                    else
                    {
                        if (!datasetInput.Contains("all"))
                        {
                            dataset = dataset.Where(d => datasetInput.Contains(d)).ToArray();

                            if (dataset.Length == 0)
                            {
                                throw new Exception("Dataset is required for model database");
                            }
                        }
                    }

                    if (outputDir == null)
                    {
                        throw new Exception("Output directory is required for model database");
                    }
                }
                else
                {
                    throw new Exception("Unknown model");
                }

                // Téléchargement et conversion des jeux de données.
                await DownloadAndConvert(dataset.ToList());

                // Traitement en fonction du modèle et des options spécifiées.
                if (model == "ai")
                {
                    Merge();
                    List<SentenceData> sentences = MakeIAModel(sentenceCount, inputDir);

                    if (outputDir != null)
                    {
                        await SaveAsJsonAsync(sentences, outputDir, new FileInfo("sentences"));
                    }
                    else
                    {
                        await UploadJsonAsync(sentences, outputUrl);
                    }
                }
                else if (model == "dataset")
                {
                    if (merge)
                    {
                        Merge();
                        if (outputDir != null)
                        {
                            await SaveAsJsonAsync(medications, outputDir, new FileInfo("merged_" + CIS_bdpm));
                        }
                        else
                        {
                            await UploadJsonAsync(medications, outputUrl);
                        }
                    }
                    else
                    {
                        if (outputDir != null)
                        {
                            await SaveDatasets(dataset.ToList(), outputDir);
                        }
                        else
                        {
                            await UploadDatasets(dataset.ToList(), outputUrl);
                        }
                    }
                }
                else if (model == "database")
                {
                    if (outputDir != null)
                    {
                        var file = new FileInfo(Path.Join(outputDir.FullName, "data.db"));
                        if (file.Exists)
                        {
                            file.Delete();
                        }
                        using (var context = new MedicationContext(outputDir.FullName))
                        {
                            context.Database.EnsureDeleted();
                            context.Database.EnsureCreated();

                            context.Medications.AddRange(medications);

                            context.SaveChanges();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            // Arrêt du chronomètre et affichage du temps d'exécution.
            stopwatch.Stop();
            Console.WriteLine($"Temps d'exécution: {stopwatch.ElapsedMilliseconds} ms");
        }, modelOption, datasetOption, mergeOption, outputDirOption, outputUrlOption, sentenceCountOption, inputOption);

        // Invocation de la commande avec les arguments de la ligne de commande.
        return await rootCommand.InvokeAsync(args);
    }

    /// <summary>
    /// Télécharge et convertit les jeux de données spécifiés.
    /// </summary>
    /// <param name="datasets">Liste des jeux de données à télécharger et convertir.</param>
    public static async Task DownloadAndConvert(List<string> datasets)
    {
        // Initialisation des tâches pour chaque type de données.
        List<Task> tasks = new List<Task>();
        Task<List<Medication>> medicationsTask = Task.FromResult(new List<Medication>());
        Task<List<MedicationComposition>> medicationCompositionsTask = Task.FromResult(new List<MedicationComposition>());
        Task<List<MedicationPresentation>> medicationPresentationsTask = Task.FromResult(new List<MedicationPresentation>());
        Task<List<GenericGroup>> genericGroupsTask = Task.FromResult(new List<GenericGroup>());
        Task<List<HasSmrOpinion>> hasSmrOpinionsTask = Task.FromResult(new List<HasSmrOpinion>());
        Task<List<HasAsmrOpinion>> hasAsmrOpinionsTask = Task.FromResult(new List<HasAsmrOpinion>());
        Task<List<ImportantInformation>> importantInformationsTask = Task.FromResult(new List<ImportantInformation>());
        Task<List<PrescriptionDispensingConditions>> prescriptionDispensingConditionsTask = Task.FromResult(new List<PrescriptionDispensingConditions>());
        Task<List<TransparencyCommissionOpinionLinks>> transparencyCommissionOpinionLinksTask = Task.FromResult(new List<TransparencyCommissionOpinionLinks>());

        // Pour chaque jeu de données, on lance une tâche de téléchargement et de conversion.
        foreach (var dataset in datasets)
        {
            switch (dataset)
            {
                case CIS_bdpm:
                    medicationsTask = DownloadAndConvertToCsvRecordsAsync<Medication>(CIS_bdpm);
                    tasks.Add(medicationsTask);
                    break;
                case CIS_COMPO_bdpm:
                    medicationCompositionsTask = DownloadAndConvertToCsvRecordsAsync<MedicationComposition>(CIS_COMPO_bdpm);
                    tasks.Add(medicationCompositionsTask);
                    break;
                case CIS_CIP_bdpm:
                    medicationPresentationsTask = DownloadAndConvertToCsvRecordsAsync<MedicationPresentation>(CIS_CIP_bdpm);
                    tasks.Add(medicationPresentationsTask);
                    break;
                case CIS_GENER_bdpm:
                    genericGroupsTask = DownloadAndConvertToCsvRecordsAsync<GenericGroup>(CIS_GENER_bdpm);
                    tasks.Add(genericGroupsTask);
                    break;
                case CIS_HAS_SMR_bdpm:
                    hasSmrOpinionsTask = DownloadAndConvertToCsvRecordsAsync<HasSmrOpinion>(CIS_HAS_SMR_bdpm);
                    tasks.Add(hasSmrOpinionsTask);
                    break;
                case CIS_HAS_ASMR_bdpm:
                    hasAsmrOpinionsTask = DownloadAndConvertToCsvRecordsAsync<HasAsmrOpinion>(CIS_HAS_ASMR_bdpm);
                    tasks.Add(hasAsmrOpinionsTask);
                    break;
                case CIS_InfoImportantes:
                    importantInformationsTask = DownloadAndConvertToCsvRecordsAsync<ImportantInformation>(CIS_InfoImportantes);
                    tasks.Add(importantInformationsTask);
                    break;
                case CIS_CPD_bdpm:
                    prescriptionDispensingConditionsTask = DownloadAndConvertToCsvRecordsAsync<PrescriptionDispensingConditions>(CIS_CPD_bdpm);
                    tasks.Add(prescriptionDispensingConditionsTask);
                    break;
                case HAS_LiensPageCT_bdpm:
                    transparencyCommissionOpinionLinksTask = DownloadAndConvertToCsvRecordsAsync<TransparencyCommissionOpinionLinks>(HAS_LiensPageCT_bdpm);
                    tasks.Add(transparencyCommissionOpinionLinksTask);
                    break;
                default:
                    throw new Exception($"Jeu de données inconnu: {dataset}");
            }
        }

        // On attend que toutes les tâches soient terminées.
        await Task.WhenAll(tasks);

        // On récupère les résultats des tâches.
        medications = medicationsTask.Result;
        medicationCompositions = medicationCompositionsTask.Result;
        medicationPresentations = medicationPresentationsTask.Result;
        genericGroups = genericGroupsTask.Result;
        hasSmrOpinions = hasSmrOpinionsTask.Result;
        hasAsmrOpinions = hasAsmrOpinionsTask.Result;
        importantInformations = importantInformationsTask.Result;
        prescriptionDispensingConditions = prescriptionDispensingConditionsTask.Result;
        transparencyCommissionOpinionLinks = transparencyCommissionOpinionLinksTask.Result;
    }

    /// <summary>
    /// Enregistre les jeux de données spécifiés dans le répertoire de sortie.
    /// </summary>
    /// <param name="datasets">Liste des jeux de données à enregistrer.</param>
    /// <param name="output">Répertoire de sortie.</param>
    public static async Task SaveDatasets(List<string> datasets, DirectoryInfo output)
    {
        // Initialisation de la liste des tâches.
        List<Task> tasks = new List<Task>();

        // Pour chaque jeu de données, on lance une tâche d'enregistrement.
        foreach (var dataset in datasets)
        {
            switch (dataset)
            {
                case CIS_bdpm:
                    tasks.Add(SaveAsJsonAsync(medications, output, new FileInfo(CIS_bdpm)));
                    break;
                case CIS_COMPO_bdpm:
                    tasks.Add(SaveAsJsonAsync(medicationCompositions, output, new FileInfo(CIS_COMPO_bdpm)));
                    break;
                case CIS_CIP_bdpm:
                    tasks.Add(SaveAsJsonAsync(medicationPresentations, output, new FileInfo(CIS_CIP_bdpm)));
                    break;
                case CIS_GENER_bdpm:
                    tasks.Add(SaveAsJsonAsync(genericGroups, output, new FileInfo(CIS_GENER_bdpm)));
                    break;
                case CIS_HAS_SMR_bdpm:
                    tasks.Add(SaveAsJsonAsync(hasSmrOpinions, output, new FileInfo(CIS_HAS_SMR_bdpm)));
                    break;
                case CIS_HAS_ASMR_bdpm:
                    tasks.Add(SaveAsJsonAsync(hasAsmrOpinions, output, new FileInfo(CIS_HAS_ASMR_bdpm)));
                    break;
                case CIS_InfoImportantes:
                    tasks.Add(SaveAsJsonAsync(importantInformations, output, new FileInfo(CIS_InfoImportantes)));
                    break;
                case CIS_CPD_bdpm:
                    tasks.Add(SaveAsJsonAsync(prescriptionDispensingConditions, output, new FileInfo(CIS_CPD_bdpm)));
                    break;
                case HAS_LiensPageCT_bdpm:
                    tasks.Add(SaveAsJsonAsync(transparencyCommissionOpinionLinks, output, new FileInfo(HAS_LiensPageCT_bdpm)));
                    break;
                default:
                    throw new Exception($"Jeu de données inconnu: {dataset}");
            }
        }

        // On attend que toutes les tâches soient terminées.
        await Task.WhenAll(tasks);
    }

    /// <summary>
    /// Télécharge les jeux de données spécifiés à l'URI de sortie.
    /// </summary>
    /// <param name="datasets">Liste des jeux de données à télécharger.</param>
    /// <param name="output">URI de sortie.</param>
    public static async Task UploadDatasets(List<string> datasets, Uri output)
    {
        // Initialisation de la liste des tâches.
        List<Task> tasks = new List<Task>();

        // Pour chaque jeu de données, on lance une tâche de téléchargement.
        foreach (var dataset in datasets)
        {
            switch (dataset)
            {
                case CIS_bdpm:
                    tasks.Add(UploadJsonAsync(medications, output));
                    break;
                case CIS_COMPO_bdpm:
                    tasks.Add(UploadJsonAsync(medicationCompositions, output));
                    break;
                case CIS_CIP_bdpm:
                    tasks.Add(UploadJsonAsync(medicationPresentations, output));
                    break;
                case CIS_GENER_bdpm:
                    tasks.Add(UploadJsonAsync(genericGroups, output));
                    break;
                case CIS_HAS_SMR_bdpm:
                    tasks.Add(UploadJsonAsync(hasSmrOpinions, output));
                    break;
                case CIS_HAS_ASMR_bdpm:
                    tasks.Add(UploadJsonAsync(hasAsmrOpinions, output));
                    break;
                case CIS_InfoImportantes:
                    tasks.Add(UploadJsonAsync(importantInformations, output));
                    break;
                case CIS_CPD_bdpm:
                    tasks.Add(UploadJsonAsync(prescriptionDispensingConditions, output));
                    break;
                case HAS_LiensPageCT_bdpm:
                    tasks.Add(UploadJsonAsync(transparencyCommissionOpinionLinks, output));
                    break;
                default:
                    throw new Exception($"Jeu de données inconnu: {dataset}");
            }
        }

        // On attend que toutes les tâches soient terminées.
        await Task.WhenAll(tasks);
    }

    /// <summary>
    /// Fusionne les différentes listes de données en une seule liste de médicaments.
    /// </summary>
    public static void Merge()
    {
        // Fusion des avis SMR de la HAS avec les liens vers les avis de la Commission de la transparence.
        hasSmrOpinions
            .GroupJoin(
                transparencyCommissionOpinionLinks,
                hasSmrOpinion => hasSmrOpinion.HasDossierCode,
                transparencyCommissionOpinionLink => transparencyCommissionOpinionLink.HasDossierCode,
                (hasAsmrOpinion, transparencyCommissionOpinionLink) =>
                {
                    hasAsmrOpinion.TransparencyCommissionOpinionLinks = transparencyCommissionOpinionLink.ToList();
                    return hasAsmrOpinion;
                }
            )
            .ToList();

        // Fusion des avis ASMR de la HAS avec les liens vers les avis de la Commission de la transparence.
        hasAsmrOpinions
            .GroupJoin(
                transparencyCommissionOpinionLinks,
                hasAsmrOpinion => hasAsmrOpinion.HasDossierCode,
                transparencyCommissionOpinionLink => transparencyCommissionOpinionLink.HasDossierCode,
                (hasAsmrOpinion, transparencyCommissionOpinionLink) =>
                {
                    hasAsmrOpinion.TransparencyCommissionOpinionLinks = transparencyCommissionOpinionLink.ToList();
                    return hasAsmrOpinion;
                }
            )
            .ToList();

        // Fusion des différentes listes de données en une seule liste de médicaments.
        medications
            .GroupJoin(
                medicationCompositions,
                medication => medication.CISCode,
                medicationComposition => medicationComposition.CISCode,
                (medication, medicationComposition) =>
                {
                    medication.MedicationCompositions = medicationComposition.ToList();
                    return medication;
                }
            )
            .GroupJoin(
                medicationPresentations,
                medication => medication.CISCode,
                medicationPresentation => medicationPresentation.CISCode,
                (medication, medicationPresentation) =>
                {
                    medication.MedicationPresentations = medicationPresentation.ToList();
                    return medication;
                }
            )
            .GroupJoin(
                genericGroups,
                medication => medication.CISCode,
                genericGroup => genericGroup.CISCode,
                (medication, genericGroup) =>
                {
                    medication.GenericGroups = genericGroup.ToList();
                    return medication;
                }
            )
            .GroupJoin(
                hasSmrOpinions,
                medication => medication.CISCode,
                hasSmrOpinion => hasSmrOpinion.CISCode,
                (medication, hasSmrOpinion) =>
                {
                    medication.HasSmrOpinions = hasSmrOpinion.ToList();
                    return medication;
                }
            )
            .GroupJoin(
                hasAsmrOpinions,
                medication => medication.CISCode,
                hasAsmrOpinion => hasAsmrOpinion.CISCode,
                (medication, hasAsmrOpinion) =>
                {
                    medication.HasAsmrOpinions = hasAsmrOpinion.ToList();
                    return medication;
                }
            )
            .GroupJoin(
                importantInformations,
                medication => medication.CISCode,
                importantInformation => importantInformation.CISCode,
                (medication, importantInformation) =>
                {
                    medication.ImportantInformations = importantInformation.ToList();
                    return medication;
                }
            )
            .GroupJoin(
                prescriptionDispensingConditions,
                medication => medication.CISCode,
                prescriptionDispensingCondition => prescriptionDispensingCondition.CISCode,
                (medication, prescriptionDispensingCondition) =>
                {
                    medication.PrescriptionDispensingConditions = prescriptionDispensingCondition.ToList();
                    return medication;
                }
            )
            .ToList();
    }

    /// <summary>
    /// Télécharge de manière asynchrone le fichier spécifié à partir de l'API de base de données publique des médicaments.
    /// </summary>
    /// <param name="fileName">Le nom du fichier à télécharger.</param>
    /// <returns>Retourne le contenu du fichier sous forme de chaîne de caractères.</returns>
    public static async Task<string> DownloadAsync(string fileName)
    {
        // Création d'un nouveau client HTTP.
        using (HttpClient client = new HttpClient())
        {
            // Construction de l'URI de base de l'API.
            Uri baseApiUri = new Uri("https://base-donnees-publique.medicaments.gouv.fr/telechargement.php?fichier=");
            var fileUriBuilder = new UriBuilder(baseApiUri);
            // Ajout du nom du fichier à l'URI.
            fileUriBuilder.Query += fileName + ".txt";
            Uri fileUri = fileUriBuilder.Uri;

            // Envoi d'une requête GET à l'URI du fichier.
            HttpResponseMessage response = await client.GetAsync(fileUri);
            if (response.IsSuccessStatusCode)
            {
                // Si la réponse est un succès, on lit le contenu de la réponse.
                if (response.Content.Headers.ContentType != null)
                {
                    // On définit l'encodage du contenu sur "latin1".
                    response.Content.Headers.ContentType.CharSet = "latin1";
                }
                string result = await response.Content.ReadAsStringAsync();
                return result;
            }
            else
            {
                // Si la réponse est un échec, on lance une exception avec le code d'état de la réponse.
                throw new Exception($"Error: {response.StatusCode}");
            }
        }
    }

    /// <summary>
    /// Convertit les données CSV en une liste d'enregistrements de type T.
    /// </summary>
    /// <param name="csvData">Les données CSV sous forme de chaîne de caractères.</param>
    /// <returns>Retourne une liste d'enregistrements de type T.</returns>
    public static List<T> ConvertToCsvRecords<T>(string csvData)
    {
        // Utilisation d'un lecteur de texte pour lire les données CSV.
        using (TextReader reader = new StringReader(csvData))
        {
            // Configuration du lecteur CSV.
            var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = "\t", // Définition du délimiteur sur la tabulation.
                HasHeaderRecord = false, // Indication qu'il n'y a pas d'en-tête.
                TrimOptions = TrimOptions.Trim, // Indication que les valeurs doivent être nettoyées.
                IgnoreBlankLines = true, // Indication que les lignes vides doivent être ignorées.
                BadDataFound = null, // Indication que les données incorrectes doivent être ignorées.
            };

            // Utilisation d'un lecteur CSV pour lire les données CSV.
            using (var csv = new CsvReader(reader, csvConfig))
            {
                // Conversion des données CSV en une liste d'enregistrements de type T.
                var records = csv.GetRecords<T>().ToList();
                return records;
            }
        }
    }

    /// <summary>
    /// Télécharge de manière asynchrone le fichier spécifié et le convertit en une liste d'enregistrements CSV de type T.
    /// </summary>
    /// <param name="fileName">Le nom du fichier à télécharger.</param>
    /// <returns>Retourne une liste d'enregistrements CSV de type T.</returns>
    public static async Task<List<T>> DownloadAndConvertToCsvRecordsAsync<T>(string fileName)
    {
        // Téléchargement des données CSV.
        string csvData = await DownloadAsync(fileName);
        // Conversion des données CSV en une liste d'enregistrements de type T.
        List<T> records = ConvertToCsvRecords<T>(csvData);
        return records;
    }

    /// <summary>
    /// Enregistre une liste d'enregistrements de type T en format JSON dans un fichier spécifié.
    /// </summary>
    /// <param name="records">La liste d'enregistrements à enregistrer.</param>
    /// <param name="output">Le répertoire de sortie où le fichier sera enregistré.</param>
    /// <param name="fileName">Le nom du fichier de sortie.</param>
    public static async Task SaveAsJsonAsync<T>(List<T> records, DirectoryInfo output, FileInfo fileName)
    {
        // Si le répertoire de sortie n'existe pas, on le crée.
        if (!output.Exists)
        {
            output.Create();
        }
        // Si le répertoire de sortie existe et contient déjà un fichier avec le même nom, on le supprime.
        else if (output.Exists && output.GetFiles().Any(f => f.Name == fileName.Name + ".json"))
        {
            output.GetFiles().Where(f => f.Name == fileName.Name + ".json").ToList().ForEach(f => f.Delete());
        }

        // Sérialisation des enregistrements en format JSON.
        string json = JsonSerializer.Serialize(
            records,
            jsonSerializerOptions
            );

        // Écriture du JSON dans le fichier de sortie.
        await File.WriteAllTextAsync(output.FullName + "/" + fileName.Name + ".json", json);
    }

    /// <summary>
    /// Télécharge de manière asynchrone une liste d'enregistrements de type T au format JSON vers une URI spécifiée.
    /// </summary>
    /// <param name="records">La liste d'enregistrements à télécharger.</param>
    /// <param name="output">L'URI de sortie où les données seront téléchargées.</param>
    public static async Task UploadJsonAsync<T>(List<T> records, Uri output)
    {
        // Sérialisation des enregistrements en format JSON.
        string json = JsonSerializer.Serialize(
            new RequestJson<List<T>> { Data = records },
            jsonSerializerOptions
        );

        // Création d'un contenu de type StringContent avec le JSON, l'encodage UTF8 et le type de média "application/json".
        StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

        // Utilisation d'un client HTTP pour envoyer les données.
        using (HttpClient client = new HttpClient())
        {
            // Envoi d'une requête POST à l'URI de sortie avec le contenu.
            HttpResponseMessage response = await client.PostAsync(output, content);
            // Si la réponse n'est pas un succès, on lance une exception avec le code d'état de la réponse.
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Error: {response.StatusCode}");
            }
        }
    }

    /// <summary>
    /// Crée un modèle d'IA en générant un certain nombre de phrases, en les tokenisant et en les retournant sous forme de liste de données de phrases.
    /// </summary>
    /// <param name="sentenceCount">Le nombre de phrases à générer.</param>
    /// <param name="input">Le fichier d'entrée contenant les prescriptions.</param>
    public static List<SentenceData> MakeIAModel(int sentenceCount, FileInfo input)
    {
        Prescriptions prescriptions = new Prescriptions();

        prescriptions.Prescription = File.ReadAllLines(input.FullName).ToList();

        // Création d'une liste pour stocker les données des phrases.
        List<SentenceData> sentences = new List<SentenceData>();

        // Génération du nombre spécifié de phrases.
        for (int i = 0; i < sentenceCount; i++)
        {
            // Obtention d'une prescription aléatoire.
            string[] prescription = prescriptions.GetPrescription();

            // Tokenisation de la prescription.
            List<string> prescriptionToken = tokenize(prescription, Labels.O, Labels.O);

            // Tant que la prescription contient le modèle de médicament,
            // on le remplace par un nom de médicament aléatoire.
            while (prescription.Contains(PatternPrescriptions.Drug))
            {
                // Création d'un objet Random pour générer un index aléatoire.
                Random random = new Random();
                // Génération d'un index aléatoire.
                int index = random.Next(medications.Count);
                // Obtention du médicament à l'index aléatoire.
                Medication medication = medications[index];
                // Division du nom du médicament en plusieurs mots.
                string[] medicationName = medication.Name.Split(' ');

                // Obtention de la position du modèle de médicament dans la prescription.
                int medicationPosition = Array.IndexOf(prescription, PatternPrescriptions.Drug);

                // Division de la prescription en deux parties : avant et après le modèle de médicament.
                var tmp_left_prescription = prescription.Take(medicationPosition).ToList();
                var tmp_right_prescription = prescription.Skip(medicationPosition + 1).ToList();

                // Remplacement du modèle de médicament par le nom du médicament dans la prescription.
                prescription = tmp_left_prescription.Concat(medicationName).Concat(tmp_right_prescription).ToArray();

                // Division des tokens de la prescription en deux parties : avant et après le modèle de médicament.
                var tmp_left_prescriptionToken = prescriptionToken.Take(medicationPosition).ToList();
                var tmp_right_prescriptionToken = prescriptionToken.Skip(medicationPosition + 1).ToList();

                // Remplacement du modèle de médicament par les tokens du nom du médicament dans les tokens de la prescription.
                prescriptionToken = tmp_left_prescriptionToken.Concat(tokenize(medicationName, Labels.B_Drug, Labels.I_Drug)).Concat(tmp_right_prescriptionToken).ToList();
            }

            // Tant que la prescription contient le modèle de quantité de médicament,
            // on le remplace par une quantité aléatoire.
            while (prescription.Contains(PatternPrescriptions.DrugQuantity))
            {
                // Création d'un objet Random pour générer une quantité aléatoire.
                Random random = new Random();
                // Génération d'une quantité aléatoire entre 1 et 10.
                int quantity = random.Next(1, 10);
                // Conversion de la quantité en tableau de chaînes de caractères.
                string[] medicationQuantity = new string[] { quantity.ToString() };

                // Obtention de la position du modèle de quantité de médicament dans la prescription.
                int quantityPosition = Array.IndexOf(prescription, PatternPrescriptions.DrugQuantity);

                // Division de la prescription en deux parties : avant et après le modèle de quantité de médicament.
                var tmp_left_prescription = prescription.Take(quantityPosition).ToList();
                var tmp_right_prescription = prescription.Skip(quantityPosition + 1).ToList();

                // Remplacement du modèle de quantité de médicament par la quantité dans la prescription.
                prescription = tmp_left_prescription.Concat(medicationQuantity).Concat(tmp_right_prescription).ToArray();

                // Division des tokens de la prescription en deux parties : avant et après le modèle de quantité de médicament.
                var tmp_left_prescriptionToken = prescriptionToken.Take(quantityPosition).ToList();
                var tmp_right_prescriptionToken = prescriptionToken.Skip(quantityPosition + 1).ToList();

                // Remplacement du modèle de quantité de médicament par les tokens de la quantité dans les tokens de la prescription.
                prescriptionToken = tmp_left_prescriptionToken.Concat(tokenize(medicationQuantity, Labels.B_DrugQuantity, Labels.I_DrugQuantity)).Concat(tmp_right_prescriptionToken).ToList();
            }

            // Tant que la prescription contient le modèle de fréquence de médicament,
            // on le remplace par une fréquence aléatoire.
            while (prescription.Contains(PatternPrescriptions.DrugFrequency))
            {
                // Création d'un objet Random pour générer un index aléatoire.
                Random random = new Random();
                // Génération d'un index aléatoire.
                int index = random.Next(PatternPrescriptionsData.Frequency.Count);
                // Obtention de la fréquence à l'index aléatoire.
                string[] medicationFrequency = PatternPrescriptionsData.Frequency[index].Split(' ');

                // Obtention de la position du modèle de fréquence de médicament dans la prescription.
                int frequencyPosition = Array.IndexOf(prescription, PatternPrescriptions.DrugFrequency);

                // Division de la prescription en deux parties : avant et après le modèle de fréquence de médicament.
                var tmp_left_prescription = prescription.Take(frequencyPosition).ToList();
                var tmp_right_prescription = prescription.Skip(frequencyPosition + 1).ToList();

                // Remplacement du modèle de fréquence de médicament par la fréquence dans la prescription.
                prescription = tmp_left_prescription.Concat(medicationFrequency).Concat(tmp_right_prescription).ToArray();

                // Division des tokens de la prescription en deux parties : avant et après le modèle de fréquence de médicament.
                var tmp_left_prescriptionToken = prescriptionToken.Take(frequencyPosition).ToList();
                var tmp_right_prescriptionToken = prescriptionToken.Skip(frequencyPosition + 1).ToList();

                // Remplacement du modèle de fréquence de médicament par les tokens de la fréquence dans les tokens de la prescription.
                prescriptionToken = tmp_left_prescriptionToken.Concat(tokenize(medicationFrequency, Labels.B_DrugFrequency, Labels.I_DrugFrequency)).Concat(tmp_right_prescriptionToken).ToList();
            }

            // Tant que la prescription contient le modèle de durée de médicament,
            // on le remplace par une durée aléatoire.
            while (prescription.Contains(PatternPrescriptions.DrugDuration))
            {
                // Création d'un objet Random pour générer un index et un nombre aléatoires.
                Random random = new Random();
                // Génération d'un index aléatoire.
                int index = random.Next(PatternPrescriptionsData.Duration.Count);
                // Génération d'un nombre aléatoire entre 1 et 10.
                int number = random.Next(1, 10);

                // Création de la durée du médicament en combinant le nombre et la durée à l'index aléatoire.
                string[] medicationDuration = $"{number} {PatternPrescriptionsData.Duration[index]}".Split(' ');

                // Obtention de la position du modèle de durée de médicament dans la prescription.
                int durationPosition = Array.IndexOf(prescription, PatternPrescriptions.DrugDuration);

                // Division de la prescription en deux parties : avant et après le modèle de durée de médicament.
                var tmp_left_prescription = prescription.Take(durationPosition).ToList();
                var tmp_right_prescription = prescription.Skip(durationPosition + 1).ToList();

                // Remplacement du modèle de durée de médicament par la durée dans la prescription.
                prescription = tmp_left_prescription.Concat(medicationDuration).Concat(tmp_right_prescription).ToArray();

                // Division des tokens de la prescription en deux parties : avant et après le modèle de durée de médicament.
                var tmp_left_prescriptionToken = prescriptionToken.Take(durationPosition).ToList();
                var tmp_right_prescriptionToken = prescriptionToken.Skip(durationPosition + 1).ToList();

                // Remplacement du modèle de durée de médicament par les tokens de la durée dans les tokens de la prescription.
                prescriptionToken = tmp_left_prescriptionToken.Concat(tokenize(medicationDuration, Labels.B_DrugDuration, Labels.I_DrugDuration)).Concat(tmp_right_prescriptionToken).ToList();
            }

            // Tant que la prescription contient le modèle de forme de médicament,
            // on le remplace par une forme aléatoire.
            while (prescription.Contains(PatternPrescriptions.DrugForm))
            {
                // Création d'un objet Random pour générer un index aléatoire.
                Random random = new Random();
                // Génération d'un index aléatoire.
                int index = random.Next(PatternPrescriptionsData.Form.Count);
                // Obtention de la forme à l'index aléatoire.
                string[] medicationForm = PatternPrescriptionsData.Form[index].Split(' ');

                // Obtention de la position du modèle de forme de médicament dans la prescription.
                int formPosition = Array.IndexOf(prescription, PatternPrescriptions.DrugForm);

                // Division de la prescription en deux parties : avant et après le modèle de forme de médicament.
                var tmp_left_prescription = prescription.Take(formPosition).ToList();
                var tmp_right_prescription = prescription.Skip(formPosition + 1).ToList();

                // Remplacement du modèle de forme de médicament par la forme dans la prescription.
                prescription = tmp_left_prescription.Concat(medicationForm).Concat(tmp_right_prescription).ToArray();

                // Division des tokens de la prescription en deux parties : avant et après le modèle de forme de médicament.
                var tmp_left_prescriptionToken = prescriptionToken.Take(formPosition).ToList();
                var tmp_right_prescriptionToken = prescriptionToken.Skip(formPosition + 1).ToList();

                // Remplacement du modèle de forme de médicament par les tokens de la forme dans les tokens de la prescription.
                prescriptionToken = tmp_left_prescriptionToken.Concat(tokenize(medicationForm, Labels.B_DrugForm, Labels.I_DrugForm)).Concat(tmp_right_prescriptionToken).ToList();
            }

            // Création d'une nouvelle instance de SentenceData avec la prescription et ses tokens.
            SentenceData sentence = new SentenceData()
            {
                Sentence = string.Join(" ", prescription),
                Labels = prescriptionToken
            };

            // Ajout de la sentence à la liste.
            sentences.Add(sentence);
        }

        // Retourne la liste des sentences.
        return sentences;
    }

    /// <summary>
    /// Tokenize une phrase en utilisant les labels spécifiés pour le premier mot et les mots intérieurs.
    /// </summary>
    /// <param name="sentence">La phrase à tokenizer.</param>
    /// <param name="begin">Le label à utiliser pour le premier mot.</param>
    /// <param name="inside">Le label à utiliser pour les mots intérieurs.</param>
    /// <returns>Retourne une liste de tokens.</returns>
    public static List<string> tokenize(string[] sentence, string begin, string inside)
    {
        // Création d'une nouvelle liste pour stocker les tokens.
        List<string> tokens = new List<string>();
        // Récupération des mots de la phrase.
        string[] words = sentence;
        // Parcours de chaque mot de la phrase.
        for (int i = 0; i < words.Length; i++)
        {
            // Si c'est le premier mot, on ajoute le label 'begin' à la liste des tokens.
            if (i == 0)
            {
                tokens.Add(begin);
            }
            // Sinon, on ajoute le label 'inside' à la liste des tokens.
            else
            {
                tokens.Add(inside);
            }
        }
        // Retourne la liste des tokens.
        return tokens;
    }
}