//---------------------------------------------------------
// Maneja la aplicación de abono en los cultivos
// Alexia Pérez Santana
// Roots Of Life
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// FertilizerManager maneja la aplicación de abono a las plantas
/// </summary>
public class FertilizerManager : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    /// <summary>
    /// Transform del jugador para detectar interacciones
    /// </summary>
    [SerializeField] private Transform PlayerTransform;

    /// <summary>
    /// Referencia al GardenManager
    /// </summary>
    [SerializeField] private GardenManager GardenManager;

    /// <summary>
    /// Distancia máxima para aplicar abono
    /// </summary>
    [SerializeField] private float MaxFertilizerDistance = 2f;

    /// <summary>
    /// Multiplicador de velocidad de crecimiento del abono
    /// </summary>
    [SerializeField] private float FertilizerGrowthMultiplier = 0.5f; // Las plantas crecen 50% más rápido

    /// <summary>
    /// Referencia al InputManager
    /// </summary>
    [SerializeField] private InputManager InputManager;

    /// <summary>
    /// AudioSource para el sonido de aplicar abono
    /// </summary>
    [SerializeField] private AudioClip FertilizerAudio;

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    //// <summary>
    /// Referencia al gameTimer
    /// </summary>
    private Timer _gameTimer;

    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    // Por defecto están los típicos (Update y Start) pero:
    // - Hay que añadir todos los que sean necesarios
    // - Hay que borrar los que no se usen 

    /// <summary>
    /// Start is called on the frame when a script is enabled just before 
    /// any of the Update methods are called the first time.
    /// </summary>
    void Start()
    {
        _gameTimer = FindObjectOfType<Timer>();
        if (InputManager == null)
            InputManager = InputManager.Instance;

        // NUEVO: Asignar PlayerTransform automáticamente si no está asignado
        if (PlayerTransform == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                PlayerTransform = player.transform;
                Debug.Log("PlayerTransform asignado automáticamente");
            }
            else
            {
                Debug.LogError("No se encontró el jugador. Asegúrate de que el jugador tenga el tag 'Player'");
            }
        }

        // NUEVO: Asignar GardenManager automáticamente si no está asignado
        if (GardenManager == null)
        {
            GardenManager = FindObjectOfType<GardenManager>();
            if (GardenManager != null)
            {
                Debug.Log("GardenManager asignado automáticamente");
            }
            else
            {
                Debug.LogError("No se encontró GardenManager en la escena");
            }
        }
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        //// Detectar input para aplicar abono
        //if (InputManager.FertilizerWasPressedThisFrame())
        //{
        //    TryApplyFertilizer();
        //}
    }
    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    /// <summary>
    /// Intenta aplicar abono a la planta más cercana
    /// </summary>
    public void TryApplyFertilizer()
    {
        // Verificar que el jugador tenga abono
        if (InventoryManager.GetInventoryItem(Items.Fertilizer) <= 0)
        {
            Debug.Log("No tienes abono en el inventario.");
            return;
        }

        // Buscar la planta más cercana
        Transform nearestPlant = FindNearestFertilizablePlant();

        if (nearestPlant != null)
        {
            ApplyFertilizerToPlant(nearestPlant);
        }
        else
        {
            Debug.Log("No hay plantas válidas cerca para aplicar abono.");
        }
    }

    /// <summary>
    /// Aplica abono a una planta específica
    /// </summary>
    public void ApplyFertilizerToPlant(Transform plantTransform)
    {
        // Buscar el índice de la planta en GardenData
        int plantIndex = FindPlantIndex(plantTransform.position);

        if (plantIndex >= 0)
        {
            Plant plant = GardenData.GetPlant(plantIndex);

            // Verificar que la planta sea válida para abono
            if (plant.Active && plant.State > 0 && plant.State < 4 && !plant.HasFertilizer)
            {
                // Aplicar abono
                GardenData.ModifyFertilizer(plantIndex, true, _gameTimer.GetGameTimeInHours(), FertilizerGrowthMultiplier);

                // Cambiar el prefab del suelo a uno con abono
                if (GardenManager != null)
                {
                    GardenManager.ChangeSoilPrefab(plantIndex, true);
                }

                // Consumir abono del inventario
                InventoryManager.ModifyInventorySubstract(Items.Fertilizer, 1);

                // Efectos visuales y sonoros
                PlayFertilizerEffects(plantTransform);

                Debug.Log($"Abono aplicado a la planta en posición {plantTransform.position}");
            }
            else if (plant.HasFertilizer)
            {
                Debug.Log("Esta planta ya tiene abono aplicado.");
            }
            else
            {
                Debug.Log("No se puede aplicar abono a esta planta.");
            }
        }
    }

    /// <summary>
    /// Verifica si una planta puede recibir abono
    /// </summary>
    public bool CanReceiveFertilizer(Transform plantTransform)
    {
        int plantIndex = FindPlantIndex(plantTransform.position);

        if (plantIndex >= 0)
        {
            Plant plant = GardenData.GetPlant(plantIndex);
            return plant.Active && plant.State > 0 && plant.State < 4 && !plant.HasFertilizer;
        }

        return false;
    }

    public void SetFertilizerSelected(bool selected)
    {
        // Por ahora, no hace nada
        // Más adelante puedes añadir lógica específica aquí
    }
    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    /// <summary>
    /// Encuentra la planta más cercana que puede recibir abono
    /// </summary>
    private Transform FindNearestFertilizablePlant()
    {
        Transform nearestPlant = null;
        float nearestDistance = MaxFertilizerDistance;

        // Buscar en todas las plantas activas
        int gardenSize = GardenManager.GetGardenSize();

        for (int i = 0; i < gardenSize; i++)
        {
            Plant plant = GardenData.GetPlant(i);

            if (plant.Active && plant.State > 0 && plant.State < 4 && !plant.HasFertilizer)
            {
                float distance = Vector3.Distance(PlayerTransform.position, plant.Position);

                if (distance < nearestDistance)
                {
                    // Verificar que existe el GameObject de la planta
                    Transform plantTransform = GardenManager.SearchPlant(plant);
                    if (plantTransform != null)
                    {
                        nearestDistance = distance;
                        nearestPlant = plantTransform;
                    }
                }
            }
        }

        return nearestPlant;
    }

    /// <summary>
    /// Encuentra el índice de una planta por su posición
    /// </summary>
    private int FindPlantIndex(Vector3 position)
    {
        int gardenSize = GardenManager.GetGardenSize();

        for (int i = 0; i < gardenSize; i++)
        {
            Plant plant = GardenData.GetPlant(i);

            if (plant.Active && plant.Position == position)
            {
                return i;
            }
        }

        return -1;
    }

    /// <summary>
    /// Reproduce efectos visuales y sonoros al aplicar abono
    /// </summary>
    private void PlayFertilizerEffects(Transform plantTransform)
    {
        // Reproducir sonido usando AudioSource.PlayClipAtPoint (VERSIÓN SIMPLE)
        if (FertilizerAudio != null)
        {
            AudioSource.PlayClipAtPoint(FertilizerAudio, plantTransform.position, 0.7f);
        }

        // Aquí podrías añadir efectos de partículas, animaciones, etc.
        // Por ejemplo:
        // GameObject particles = Instantiate(fertilizerParticlesPrefab, plantTransform.position, Quaternion.identity);

        //Cambiar el color del suelo para indicar que tiene abono
        CropSpriteEditor cropEditor = plantTransform.GetChild(0).GetComponent<CropSpriteEditor>();
        if (cropEditor != null)
        {
            cropEditor.ShowFertilizerEffect();
        }
    }

    #endregion

} // class FertilizerManager 
// namespace
