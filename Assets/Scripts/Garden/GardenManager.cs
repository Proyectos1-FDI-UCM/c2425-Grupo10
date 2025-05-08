//---------------------------------------------------------
// Maneja los cutlivos y sus procesos
// Alexia Pérez Santana y Julia Vera
// Roots of Life
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// Este script gestiona los cultivos, 
/// Tiene Métodos de Acción (Water, Harvest...) que dependen de herramientas
/// Métodos de avisos (WaterWarning, DeathWarning...) que dependen del Timer
/// </summary>
public class GardenManager : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)

    /// <summary>
    /// Carpeta con todas las posiciones en las que el jugador puede plantar
    /// </summary>
    [SerializeField] private GameObject PlantingSpots;

    /// <summary>
    /// Timer que converte el tiempo de juego en tiempo real
    /// </summary>
    [SerializeField] private Timer gameTimer;

    /// <summary>
    /// Mejora 0 del huerto
    /// </summary>
    [SerializeField] private GameObject GardenLevel0;

    /// <summary>
    /// Mejora 1 del huerto
    /// </summary>
    [SerializeField] private GameObject GardenLevel1;

    /// <summary>
    /// Mejora 2 del huerto
    /// </summary>
    [SerializeField] private GameObject GardenLevel2;

    /// <summary>
    /// Mejora 3 del huerto
    /// </summary>
    [SerializeField] private GameObject GardenLevel3;

    /// <summary>
    /// Mejora 3 del huerto
    /// </summary>
    [SerializeField] private GameObject GardenLevel4;


    ///<summary>
    /// Mejora Actual
    /// </summary>
    [SerializeField] private int UpgradeLevel;

    /// <summary>
    /// Carpeta con todas las posiciones en las que el jugador puede plantar
    /// </summary>
    [SerializeField] private GameObject[] Prefabs = new GameObject[(int)Items.Count/2];
    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)

    //// Tiempo desde el último riego
    //private float _timeSinceLastWatering; 

    Transform[] Plants;

    /// <summary>
    /// Array con los tamaños de huerto
    /// </summary>
    private int[] GardenSize = { 6, 12, 18, 24, 36 };

    /// <summary>
    /// Probabilidad de que aparezca mala hierbas
    /// </summary>
    private int _maxProb = 4;

    /// <summary>
    /// Tutorial Manager
    /// </summary>
    private TutorialManager TutorialManager;

    /// <summary>
    /// Activa una mala hierba para el tutorial
    /// </summary>
    private bool WeedTutorial = false;

    private bool doneWeed = false;
    private bool donePlant = false;
    private bool done = false;

    [SerializeField] Plant[] Garden;

    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    /// <summary>
    /// Start is called on the frame when a script is enabled just before 
    /// any of the Update methods are called the first time.
    /// </summary>
    void Awake() 
    {
        InitializeReferences();
        GameManager.Instance.InitializeGardenManager();
        UpgradeLevel = GameManager.Instance.GetGardenUpgrades();
        SetUpgrade(UpgradeLevel);

        TutorialManager = FindObjectOfType<TutorialManager>();
    }

    private void Start()
    {
        //InitChangeScene();
        Garden = GardenData.GetGarden();
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        Garden = GardenData.GetGarden();
        bool isFastTime = gameTimer.IsFastTimeActive(); // Obtener el estado del tiempo rápido

        for (int i = 0; i < GardenSize[UpgradeLevel]; i++)
        {
            //Debug.Log(GardenData.GetPlant(i).Item);

            Plant Plant = GardenData.GetPlant(i);

            if (GardenData.GetPlant(i).Active)
            {
                Items item = Plant.Item;
                float MaxGrowth = GardenData.GetMaxGrowthTime(item);
                float MaxWater = GardenData.GetMaxWaterTime(item);
                float MaxDeath = GardenData.GetMaxDeathTime(item);
                int State = Plant.State;

                if (State == 0)
                {
                    GardenData.ModifyGrowthTimer(i, gameTimer.GetGameTimeInHours());
                    GardenData.ModifyWaterTimer(i, gameTimer.GetGameTimeInHours());
                    GrowthWarning(GardenData.GetPlant(i), i);

                }

                // Lógica Crecimiento
                if ((gameTimer.GetGameTimeInHours() - Plant.GrowthTimer) >= MaxGrowth && State < 4 && State > 0)
                {
                    GrowthWarning(GardenData.GetPlant(i), i);
                }

                // Lógica Aviso Cosecha y Aviso Riego / Muerte

                // Si el tiempo rápido está activo, ignoramos la lógica de riego/muerte
                if (!isFastTime)
                {
<<<<<<< Updated upstream
<<<<<<< Updated upstream
                    GardenData.ModifyWaterWarning(i, true);
                    WaterWarning(GardenData.GetPlant(i), i);
                    //GardenData.ModifyWaterWarning(i);
                }

                 // Aviso Muerte
                if ((gameTimer.GetGameTimeInHours() - Plant.WaterTimer) >= MaxWater + (MaxDeath / 2) && gameTimer.GetGameTimeInHours() - Plant.WaterTimer < MaxWater + MaxDeath && State > 0 && State < 4 )
                {
                    Debug.Log("Aviso Muerte");
                    GardenData.ModifyDeathWarning(i, true);
                    DeathWarning(GardenData.GetPlant(i), i);
                    
                }
=======
                    // Aviso Riego
                    if ((gameTimer.GetGameTimeInHours() - Plant.WaterTimer) >= MaxWater && (gameTimer.GetGameTimeInHours() - Plant.WaterTimer) < MaxWater + (MaxDeath / 2) && State > 0 && State < 4)
                    {
                        WaterWarning(GardenData.GetPlant(i), i);
                        //GardenData.ModifyWaterWarning(i);
                    }

                    // Aviso Muerte
                    if ((gameTimer.GetGameTimeInHours() - Plant.WaterTimer) >= MaxWater + (MaxDeath / 2) && gameTimer.GetGameTimeInHours() - Plant.WaterTimer < MaxWater + MaxDeath && State > 0 && State < 4)
                    {
                        Debug.Log("Aviso Muerte");
                        DeathWarning(GardenData.GetPlant(i), i);
>>>>>>> Stashed changes

=======
                    // Aviso Riego
                    if ((gameTimer.GetGameTimeInHours() - Plant.WaterTimer) >= MaxWater && (gameTimer.GetGameTimeInHours() - Plant.WaterTimer) < MaxWater + (MaxDeath / 2) && State > 0 && State < 4)
                    {
                        WaterWarning(GardenData.GetPlant(i), i);
                        //GardenData.ModifyWaterWarning(i);
                    }

                    // Aviso Muerte
                    if ((gameTimer.GetGameTimeInHours() - Plant.WaterTimer) >= MaxWater + (MaxDeath / 2) && gameTimer.GetGameTimeInHours() - Plant.WaterTimer < MaxWater + MaxDeath && State > 0 && State < 4)
                    {
                        Debug.Log("Aviso Muerte");
                        DeathWarning(GardenData.GetPlant(i), i);

>>>>>>> Stashed changes
                    }

                    // Muerte
                    if (gameTimer.GetGameTimeInHours() - Plant.WaterTimer >= MaxWater + MaxDeath && State > 0 && State < 4)
                    {
                        Death(Plant, i);
                        Debug.Log("Muerte");
                    }
                }
                else
                {
                    // En modo tiempo rápido, si hay avisos de riego o muerte, los quitamos
                    if (Plant.WaterWarning || Plant.DeathWarning)
                    {
                        GardenData.ModifyWaterWarning(i, false);
                        GardenData.ModifyDeathWarning(i, false);

                        // Y actualizamos el timer de agua para que no necesite riego
                        GardenData.ModifyWaterTimer(i, gameTimer.GetGameTimeInHours());

                        // Actualizar el sprite para quitar los avisos visuales
                        Transform Crop = SearchPlant(Plant);
                        if (Crop != null)
                        {
                            CropSpriteEditor Call = Crop.GetComponent<CropSpriteEditor>();
                            if (Call != null)
                            {
                                Call.Warning("Desactivate");
                            }
                        }
                    }
                }

                //Cosechar
                if (State == 4)
                {
                    HarvestWarning(GardenData.GetPlant(i), i);
                }

            }
        }

        if (GameManager.Instance.GetGardenUpgrades() > UpgradeLevel)
        {
            UpgradeLevel++;
            SetUpgrade(UpgradeLevel);
        }

    }

        

        #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos

    /// <summary>
    /// Riega: Modifica los valores de riego de una planta por su posición
    /// </summary>
    public void Water(Transform transform)
    {
        int i = 0;
        while (i < GardenSize[UpgradeLevel] && GardenData.GetPlant(i).Position != transform.position)
        {
            i++;
        }

        if (GardenData.GetPlant(i).Position == transform.position)
        {
            GardenData.ModifyWaterTimer(i, gameTimer.GetGameTimeInHours());
            GardenData.ModifyWaterWarning(i, false);
            GardenData.ModifyDeathWarning(i, false);

        }
    }

    /// <summary>
    /// Cultiva: Modifica los valores de cultivo de una planta por su posición (es decir la desactiva)
    /// </summary>
    public void Harvest(Transform transform)
    {
        int i = 0;
        while (i <= GardenSize[UpgradeLevel] && (!GardenData.GetPlant(i).Active || GardenData.GetPlant(i).Position != transform.position))
        {
            i++;
        }
        Plant Plant = GardenData.GetPlant(i);
        Debug.Log("Plant position: " + Plant.Position + "Transform Position: " + transform.position + "i: " + i);

        if (Plant.Position == transform.position)
        {
            Debug.Log(Plant.State);
            if (Plant.State == 4)
            {
                if (InventoryManager.BoolModifyInventory(Plant.Item, 1))
                {
                    GardenData.ModifyHarvestWarning(i, false);
                    CropSpriteEditor cropSpriteEditor = transform.GetChild(0).GetComponent<CropSpriteEditor>();

                    int random;
                    if (!WeedTutorial)
                    {
                        random = 0;
                        WeedTutorial = true;
                    }
                    else random = UnityEngine.Random.Range(0, _maxProb);

                    if (random == 0)
                    {
                        GardenData.ModifyState(i, (-6));
                        cropSpriteEditor.Warning("Desactivate");
                        cropSpriteEditor.Growing(-6);

                        if (TutorialManager.GetTutorialPhase() == 19 && !done)
                        {
                            TutorialManager.CheckBox(0);
                            TutorialManager.SubTask();
                            done = true;
                        }
                    }
                    else
                    {
                        GardenData.Deactivate(i);
                        cropSpriteEditor.Destroy();
                    }
                }
            }
        }
        else Debug.Log("Plant.Position != transform");
    }

    /// <summary>
    /// MalasHierbas: Modifica los valores de la semilla (Elimina)
    /// </summary>
    public void Weed(Transform transform)
    {
        int i = 0;
        while (i < GardenSize[UpgradeLevel] && (!GardenData.GetPlant(i).Active || GardenData.GetPlant(i).Position != transform.position))
        {
            i++;
        }
        Plant Plant = GardenData.GetPlant(i);
        Debug.Log("Plant position: " + Plant.Position + "Transform Position: " + transform.position + "i: " + i);

        if (Plant.Position == transform.position)
        {
            if (Plant.State < 0)
            {
                CropSpriteEditor cropSpriteEditor = transform.GetChild(0).GetComponent<CropSpriteEditor>();
                GardenData.Deactivate(i);
                cropSpriteEditor.Destroy();

                if (TutorialManager.GetTutorialPhase() == 19 && Plant.State == -6 && !doneWeed)
                {
                    TutorialManager.CheckBox(2);
                    TutorialManager.SubTask();
                    doneWeed = true;
                }

                else if (TutorialManager.GetTutorialPhase() == 19 && Plant.State > -6 && !donePlant)
                {
                    TutorialManager.CheckBox(1);
                    TutorialManager.SubTask();
                    donePlant = true;
                }
            }
        }
    }

    public void SetUpgrade(int Level)
    {
        if (Level == 0)
        {
            GardenLevel0.SetActive(true);

            Plants = new Transform[PlantingSpots.transform.childCount];
            for (int i = 0; i < GardenSize[UpgradeLevel]; i++)
            {
                Plants[i] = PlantingSpots.transform.GetChild(i).transform;
                Plants[i].gameObject.SetActive(true);
            }

            GardenLevel1.SetActive(false);
            GardenLevel2.SetActive(false);
            GardenLevel3.SetActive(false);
            GardenLevel4.SetActive(false);

        }
        else if (Level == 1)
        {
            GardenLevel1.SetActive(true);

            Plants = new Transform[PlantingSpots.transform.childCount];
            for (int i = 0; i < GardenSize[UpgradeLevel]; i++)
            {
                Plants[i] = PlantingSpots.transform.GetChild(i).transform;
                Plants[i].gameObject.SetActive(true);
            }

            GardenLevel0.SetActive(false);
            GardenLevel2.SetActive(false);
            GardenLevel3.SetActive(false);
            GardenLevel4.SetActive(false);

        }
        else if (Level == 2)
        {
            GardenLevel2.SetActive(true);

            Plants = new Transform[PlantingSpots.transform.childCount];
            for (int i = 0; i < GardenSize[UpgradeLevel]; i++)
            {
                Plants[i] = PlantingSpots.transform.GetChild(i).transform;
                Plants[i].gameObject.SetActive(true);
            }

            GardenLevel0.SetActive(false);
            GardenLevel1.SetActive(false);
            GardenLevel3.SetActive(false);
            GardenLevel4.SetActive(false);


        }
        else if (Level == 3)
        {
            GardenLevel3.SetActive(true);

            Plants = new Transform[PlantingSpots.transform.childCount];
            for (int i = 0; i < GardenSize[UpgradeLevel]; i++)
            {
                Plants[i] = PlantingSpots.transform.GetChild(i).transform;
                Plants[i].gameObject.SetActive(true);
            }

            GardenLevel0.SetActive(false);
            GardenLevel1.SetActive(false);
            GardenLevel2.SetActive(false);
            GardenLevel4.SetActive(false);


        }
        else if (Level == 4)
        {
            GardenLevel4.SetActive(true);

            Plants = new Transform[PlantingSpots.transform.childCount];
            for (int i = 0; i < GardenSize[UpgradeLevel]; i++)
            {
                Plants[i] = PlantingSpots.transform.GetChild(i).transform;
                Plants[i].gameObject.SetActive(true);
            }

            GardenLevel0.SetActive(false);
            GardenLevel1.SetActive(false);
            GardenLevel2.SetActive(false);
            GardenLevel3.SetActive(false);

        }
    }
    public int GetGardenSize()
    {
        return GardenSize[UpgradeLevel];
    }

    /// <summary>
    /// Método que carga los valores de las plantas entre escenas
    /// </summary>
    public void InitChangeScene()
    {

        for (int i = 0; i < GardenSize[UpgradeLevel]; i++)
        {
            Plant plant = GardenData.GetPlant(i);
            if (plant.Active) 
            {
                GameObject Prefab = Prefabs[(int)plant.Item%((int)Items.Count/2)];
                GameObject Crop = Instantiate(Prefab, plant.Position, Quaternion.identity);
                Crop.transform.parent = PlantingSpots.transform.GetChild(plant.Child);

                WaterWarning(plant, i);
                DeathWarning(plant, i);
                //HarvestWarning(plant, i);

                CropSpriteEditor cropSpriteEditor = Crop.GetComponent<CropSpriteEditor>();

                if (cropSpriteEditor != null) 
                { 
                    cropSpriteEditor.Growing(plant.State);
                }
            }
            Debug.Log($"Plant: {i} instanciated in child: {plant.Child}");
        }
        Debug.Log("ChangeScene");
    }
    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados

    /// <summary>
    /// Método para avisar del riego
    /// </summary>
    public void WaterWarning(Plant plant, int ArrayIndex)
    {
        if (plant.WaterWarning && plant.State > 0) // Solo si no está muerto
        {
            Transform Crop = SearchPlant(plant);

            if (Crop != null)
            {
                CropSpriteEditor Call = Crop.GetComponent<CropSpriteEditor>();
                Call.Warning("Water");
                GardenData.ModifyWaterWarning(ArrayIndex, true);
            }
        }
    }

    /// <summary>
    /// Método para avisar del crecimiento
    /// </summary>
    public void GrowthWarning(Plant plant, int ArrayIndex)
    {
        Debug.Log("Plant Growing");
        Transform Crop = SearchPlant(plant);

        GardenData.ModifyState(ArrayIndex, plant.State + 1);
        GardenData.ModifyGrowthTimer(ArrayIndex, gameTimer.GetGameTimeInHours());

        if (Crop != null)
        {
            CropSpriteEditor Call = Crop.GetComponent<CropSpriteEditor>();
            Call.Growing(plant.State+ 1);

        }
    }

    /// <summary>
    /// Método para avisar de la muerte
    /// </summary>
    public void DeathWarning(Plant plant, int ArrayIndex)
    {
        if (plant.DeathWarning)
        {
            Debug.Log("DeathWarning");
            GardenData.ModifyState(ArrayIndex, plant.State);
            Transform Crop = SearchPlant(plant);

            if (Crop != null)
            {
                CropSpriteEditor Call = Crop.GetComponent<CropSpriteEditor>();
                Call.Warning("Death");
                GardenData.ModifyDeathWarning(ArrayIndex, true);
            }
        }
    }

    /// <summary>
    /// Método para que se mueran las plantas
    /// </summary>
    public void Death(Plant plant, int ArrayIndex)
    {
        Debug.Log("Death");
        Transform Crop = SearchPlant(plant);

        if (Crop != null)
        {
            CropSpriteEditor Call = Crop.GetComponent<CropSpriteEditor>();
            Call.Growing(-1 * plant.State);
            Call.Warning("Desactivate");
            GardenData.ModifyState(ArrayIndex, -1 * plant.State);
            GardenData.ModifyDeathWarning(ArrayIndex, false);
            GardenData.ModifyWaterWarning(ArrayIndex, false);
        }
    }

    /// <summary>
    /// Método para avisar de la muerte
    /// </summary>
    public void HarvestWarning(Plant plant, int ArrayIndex)
    {
        Transform Crop = SearchPlant(plant);

        if (Crop != null)
        {
            CropSpriteEditor Call = Crop.GetComponent<CropSpriteEditor>();
            Call.Warning("Harvest");
            GardenData.ModifyHarvestWarning(ArrayIndex, true);

        }
    }

    /// <summary>
    /// Método para buscar la posición de la planta correcta
    /// </summary>
    public Transform SearchPlant(Plant plant)
    {
        Transform transform = null;
        if (PlantingSpots.transform.childCount >= plant.Child)
        {
            if(PlantingSpots.transform.GetChild(plant.Child).transform.childCount > 0) 
                {
                transform = PlantingSpots.transform.GetChild(plant.Child).transform.GetChild(0);
            }
        }
        return transform;

    }

   
    public void UpdateAllPlantsWater()
    {
        Garden = GardenData.GetGarden();

        // Actualizar todas las plantas activas
        for (int i = 0; i < GardenSize[UpgradeLevel]; i++)
        {
            Plant plant = GardenData.GetPlant(i);

            if (plant.Active && plant.State > 0 && plant.State < 4)
            {
                // Actualizar el timer de agua para que no necesite riego
                GardenData.ModifyWaterTimer(i, gameTimer.GetGameTimeInHours());

                // Quitar los avisos si existen
                if (plant.WaterWarning || plant.DeathWarning)
                {
                    GardenData.ModifyWaterWarning(i, false);
                    GardenData.ModifyDeathWarning(i, false);

                    // Actualizar el sprite para quitar los avisos visuales
                    Transform Crop = SearchPlant(plant);
                    if (Crop != null)
                    {
                        CropSpriteEditor Call = Crop.GetComponent<CropSpriteEditor>();
                        Call.Warning("Desactivate");
                    }
                }
            }
        }
    }

    
    public void HandleTimeSpeedChange(bool isFastMode)
    {
        Garden = GardenData.GetGarden();

        for (int i = 0; i < GardenSize[UpgradeLevel]; i++)
        {
            Plant plant = GardenData.GetPlant(i);

            if (plant.Active && plant.State > 0 && plant.State < 4)
            {
                if (isFastMode)
                {
                    // Si activamos el modo rápido, actualizar el timer de agua y quitar avisos
                    GardenData.ModifyWaterTimer(i, gameTimer.GetGameTimeInHours());

                    // Quitar los avisos si existen
                    if (plant.WaterWarning || plant.DeathWarning)
                    {
                        GardenData.ModifyWaterWarning(i, false);
                        GardenData.ModifyDeathWarning(i, false);

                        // Actualizar el sprite para quitar los avisos visuales
                        Transform Crop = SearchPlant(plant);
                        if (Crop != null)
                        {
                            CropSpriteEditor Call = Crop.GetComponent<CropSpriteEditor>();
                            Call.Warning("Desactivate");
                        }
                    }
                }
                // No necesitamos hacer nada especial al volver al modo normal
                // Las plantas seguirán su crecimiento normal basado en los timers actualizados
            }
        }
    }

    // Método para eliminar todos los avisos visuales de plantas
    public void ClearAllWarningSprites()
    {
        Garden = GardenData.GetGarden(); // Asegurarse de tener datos actualizados

        for (int i = 0; i < GardenSize[UpgradeLevel]; i++)
        {
            Plant plant = GardenData.GetPlant(i);

            if (plant.Active) // Procesar TODAS las plantas activas, no solo las que ya tienen avisos
            {
                // Siempre actualizar el timer de agua para prevenir futuros avisos
                GardenData.ModifyWaterTimer(i, gameTimer.GetGameTimeInHours());

                // Desactivar los avisos en los datos, incluso si no tienen avisos actualmente
                if (plant.WaterWarning || plant.DeathWarning)
                {
                    GardenData.ModifyWaterWarning(i, false);
                    GardenData.ModifyDeathWarning(i, false);
                }

                // Desactivar el sprite de aviso en TODOS los casos
                Transform Crop = SearchPlant(plant);
                if (Crop != null)
                {
                    CropSpriteEditor Call = Crop.GetComponent<CropSpriteEditor>();
                    if (Call != null)
                    {
                        Call.Warning("Desactivate");
                    }
                }
            }
        }

        Debug.Log("Todos los avisos visuales de plantas han sido limpiados");
    }

    ///<summary>
    ///Mertodo para inicializar el huerto
    /// </summary>
    private void InitGarden()
    {
        SetUpgrade(GameManager.Instance.GetGardenUpgrades());
    }

    ///<summary>
    ///Metodo para inicializar referencias
    /// </summary>
    private void InitializeReferences()
    {
        if (gameTimer == null)
        {
            gameTimer = FindObjectOfType<Timer>();
        }
    }
    #endregion

} // class TimerManager
