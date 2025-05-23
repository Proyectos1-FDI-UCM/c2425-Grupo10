//---------------------------------------------------------
// Este script funciona como el manager de la herramienta de regadera, donde se permite al jugador regar y rellenar la regadera.
// Javier Librada Jerez
// Roots of Life
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using TMPro;
using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class WateringCanManager : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints

    ///<summary> 
    /// referencia al animator del player
    /// </summary>
    [SerializeField] private Animator PlayerAnimator;

    ///<summary>
    /// Cantidad Actual de Agua en la regadera
    /// </summary>
    [SerializeField] private int _waterAmount = 6;

    ///<summary>
    ///Cantidad maxima de Agua en la regadera
    /// </summary>
    [SerializeField] private int _maxWaterAmount;

    ///<summary> 
    ///Referencia al SelectorManager
    /// </summary>
    [SerializeField] private SelectorManager SelectorManager;



    ///<summary> 
    ///Referencia al SelectorManager
    /// </summary>
    [SerializeField] private PlayerMovement PlayerMovement;

    ///<summary> 
    /// Referencia al InventoryUI
    /// </summary>
    [SerializeField] private UIManager UIManager; 

    ///<summary>
    ///PrefabAvisoRiego
    /// </summary>
    [SerializeField] private GameObject PrefabWatering;


    ///<summary>
    ///Posición exacta del pozo
    /// </summary>
    [SerializeField] private Vector2 WellPosition;

    /// <summary>
    /// Carpeta con todas las posiciones en las que el jugador puede plantar
    /// </summary>
    [SerializeField] private GameObject PlantingSpots;

    /// <summary>
    /// Distancia mínima a la que debe estar el jugador del lugar disponible para plantar
    /// </summary>
    [SerializeField] private float InitialMinDistance;

    /// <summary>
    /// GardenManager, para llamar al método Watering
    /// </summary>
    [SerializeField] private GardenManager GardenManager;

    ///<summary>
    ///Ref al sound manager
    /// </summary>
    [SerializeField] private SoundManager SoundManager;

    ///<summary>
    ///ref al notification maanager
    /// </summary>
    [SerializeField] private NotificationManager NotificationManager;

    ///<summary>
    ///Audio de regar
    /// </summary>
    [SerializeField] private AudioClip WateringAudio;

    ///<summary>
    ///Audio de rellenar
    /// </summary>
    [SerializeField] private AudioClip FillingAudio;

    ///<summary>
    ///Audio source
    /// </summary>
    [SerializeField] private AudioSource WateringCanAudio;

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // privados se nombren en formato _camelCase (comienza con _, 
    // primera palabra en minúsculas y el resto con la 
    // primera letra en mayúsculas)
    // Ejemplo: _maxHealthPoints

    ///<summary>
    ///Referencia al CropSpriteEditor
    /// </summary>
    private CropSpriteEditor cropSpriteEditor;


    ///<summary>
    ///GameObject donde instanciar el Prefab de aviso
    /// </summary>
    private GameObject _warning;



    ///<summary>
    ///Booleano para permitir recargar
    /// </summary>
    private bool _isInWellArea = false;

    ///<summary>
    ///Booleano para permitir regar
    /// </summary>
    private bool _isInCropArea = false;

    ///<summary>
    ///Transform del crop con el que estas colisionando
    /// </summary>
    private Transform _cropTransform;


    /// <summary>
    /// Array con el transform de todas los lugares disponibles para plantar
    /// </summary>
    private Transform[] Pots;

    ///<summary>
    ///Notificacion activada
    /// </summary>
    private bool _notificationActive = false;

    /// <summary>
    /// Tutorial Manager
    /// </summary>
    private TutorialManager _tutorialManager;

    ///<summary>
    ///booleanos para activar/desactivar el riego/rellenado
    /// </summary>
    private bool _canWater = true;

    private string _use;
    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    // Por defecto están los típicos (Update y Start) pero:
    // - Hay que añadir todos los que sean necesarios
    // - Hay que borrar los que no se usen 

    /// <summary>
    /// Metodo awake para inicializar las referencias
    /// </summary>
    private void Awake()
    {
        InitializeReferences();
        GameManager.Instance.InitializeWateringCanManager();
        WateringCanAudio = GetComponent<AudioSource>();
    }
    /// <summary>
    /// Start is called on the frame when a script is enabled just before 
    /// any of the Update methods are called the first time.
    /// </summary>
    void Start()
    {
        GetUpgradeWateringCan();

        UIManager.UpdateWaterBar(_waterAmount, _maxWaterAmount);

        _waterAmount = GameManager.Instance.LastWaterAmount();

        Pots = new Transform[GardenManager.GetGardenSize()]; // Inicia el tamaño del array al tamaño del total de hijos de la carpeta PlantingSpots
        for (int i = 0; i < GardenManager.GetGardenSize(); i++)
        {
            Pots[i] = PlantingSpots.transform.GetChild(i).transform; // Establece en el array todos los transforms de los lugares para plantar (dentro de la carpeta PlantingSpots)
        }

        _tutorialManager = FindObjectOfType<TutorialManager>();

}

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if (GameManager.Instance.GetControllerUsing())
        {
            _use = "Cuadrado";
        }
        else
        {
            _use = "E";
        }
            GetUpgradeWateringCan();

        UIManager.UpdateWaterBar(_waterAmount, _maxWaterAmount);

        _waterAmount = GameManager.Instance.LastWaterAmount();

        if (InputManager.Instance.UseWateringCanWasPressedThisFrame() && !_isInWellArea && _canWater)
        {
            if(_tutorialManager.GetTutorialPhase() >= 15)
            {
                Watering();
                _canWater = false;
                if (_tutorialManager.GetTutorialPhase() == 15)
                {
                    _tutorialManager.CheckBox(0);
                    _tutorialManager.Invoke("NextDialogue", 0.6f);
                }
            }
            else
            {
                UIManager.ShowNotification("Avanza en el tutorial\npara usar", "NoCounter", 4, "Tool");
                Invoke("NoWarning", 1f);
            }
        }

        if (_isInWellArea && _waterAmount < _maxWaterAmount)
        {
            Debug.Log("Rellenar");
            UIManager.ShowNotification("Presiona "+ _use + "\npara rellenar", "NoCounter", 1, "NoTutorial");
            _notificationActive = true;
            // 
            if (InputManager.Instance.UseWateringCanWasPressedThisFrame())
            {

                FillWateringCan(_maxWaterAmount);
                _canWater = false;
                if (_tutorialManager.GetTutorialPhase() == 16)
                {
                    _tutorialManager.CheckBox(0);
                    _tutorialManager.Invoke("NextDialogue", 0.6f);
                }
            }

        }

        else if (!_isInWellArea || _waterAmount == _maxWaterAmount)
        {
            UIManager.HideNotification("NoTutorial");
            if (_notificationActive)
            {
                SoundManager.NextButtonSound();
                _notificationActive = false;
            }
            

        }
        if (_isInCropArea && _waterAmount > 0)
        {
            if (GardenData.GetPlant(_cropTransform).WaterTimer == 4)
            {
                UIManager.ShowNotification("Presiona "+ _use + "\npara regar", "NoCounter", 1, "NoTutorial");
            }

        }
        else if (!_isInCropArea || _waterAmount == 0)
        {


        }
    }
    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    // Documentar cada método que aparece aquí con ///<summary>
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)
    // Ejemplo: GetPlayerController

    /// <summary>
    /// Metodo para obtener el agua que hay actualmente en la regadera
    /// </summary>
    /// <returns></returns>
    public int GetAmountWateringCan()
    {

        return _waterAmount;

    }

    /// <summary>
    /// Metodo para obtener la cantidad maxima que puede tener la regadera
    /// </summary>
    /// <returns></returns>
    public int GetMaxWaterAmount()
    {

        return _maxWaterAmount;

    }

    /// <summary>
    /// Metodo para llenar la regadera al colisionar con el pozo
    /// </summary>
    /// <param name="i"></param>
    public void FillWateringCan(int i)
    {
        PlayerAnimator.SetBool("Filling", true);
        WateringCanAudio.clip = FillingAudio;
        WateringCanAudio.pitch = 3;
        WateringCanAudio.Play();
        Debug.Log("Recargando");

        PlayerMovement.DisablePlayerMovement();

        _waterAmount = i;

        InstanceWarning();

        UIManager.UpdateWaterBar(_waterAmount, _maxWaterAmount);

        GameManager.Instance.UpdateWaterAmount();

        Invoke("NotFilling", 1f);

    }

    /// <summary>
    /// Metodo para regar, resta 1 a la cantidad actual de agua y actualiza la barra
    /// </summary>
    public void Watering()
    {
        Debug.Log("Watering");

        if (_waterAmount > 0)
        {

            PlayerAnimator.SetBool("Watering", true);
            WateringCanAudio.clip = WateringAudio;
            WateringCanAudio.pitch = 2;
            WateringCanAudio.Play();
            Invoke("NotWatering", 1f);


            PlayerMovement.DisablePlayerMovement();

            _waterAmount -= 1; ;

            UIManager.UpdateWaterBar(_waterAmount, _maxWaterAmount);

            GameManager.Instance.UpdateWaterAmount();

            Invoke("WaterPlant", 1f); // Se realizan los cambios de riego en la planta al terminar la animación

        }

    }

    /// <summary>
    /// Método que realiza los cambios del riego
    /// (Procesa los cambios en la planta) Al terminar la animación
    /// </summary>
    private void WaterPlant()
    {
        Debug.Log("WateringCan");
        Transform Pot = FindNearestPot(transform, Pots);

        Debug.Log("FindNearestPot: " + Pot);

        if (Pot != null)
        {
            Transform Plant = Pot.transform.GetChild(0);

            if (Plant != null)
            {
                CropSpriteEditor cropSpriteEditor = Plant.GetComponent<CropSpriteEditor>();
                cropSpriteEditor.Warning("Desactivate");
                GardenManager.Water(Pot.transform);
            }
        }
    }

    /// <summary>
    /// Método para modificar el máximo agua de la regadera despues de las mejoras
    /// <summary>
    public void UpgradeWateringCan(int upgrade)
    {

        if (upgrade == 0)
        {

            _maxWaterAmount = 6;

        }

        else if (upgrade == 1)
        {

            _maxWaterAmount = 9;

        }

        else if (upgrade == 2)
        {

            _maxWaterAmount = 15;

        }

        else if (upgrade == 3)
        {

            _maxWaterAmount = 20;

        }

    }

    /// <summary>
    /// Metodo para destruir el aviso de recarga en el pozo
    /// </summary>
    public void DestroyAvisos()
    {

        Debug.Log("Invoke");

        Destroy(_warning);

    }
     
    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)

    /// <summary>
    /// Metodo para obtener la mejora actual de regadera desde el GameManager
    /// </summary>
    private void GetUpgradeWateringCan()
    {

        if ((GameManager.Instance.GetWateringCanUpgrades() == 0)) UpgradeWateringCan(0);
        else if ((GameManager.Instance.GetWateringCanUpgrades() == 1)) UpgradeWateringCan(1);
        else if ((GameManager.Instance.GetWateringCanUpgrades() == 2)) UpgradeWateringCan(2);
        else if ((GameManager.Instance.GetWateringCanUpgrades() == 3)) UpgradeWateringCan(3);


    }

    /// <summary>
    /// Metodo para desactivar la animacion de regar
    /// </summary>
    private void NotWatering()
    {

        PlayerAnimator.SetBool("Watering", false);
        _canWater = true;
        this.gameObject.SetActive(true);

        PlayerMovement.EnablePlayerMovement();

    }

    ///<summary>
    ///metodo para desactivar la recarga
    /// </summary>
    private void NotFilling()
    {
        PlayerAnimator.SetBool("Filling", false);
        _canWater = true;
        Destroy(_warning);

        PlayerMovement.EnablePlayerMovement();

    }
    ///<summary>
    ///metodo para ocultar la notificacion
    /// </summary>
    private void NoWarning()
    {
        UIManager.HideNotification("Tool");
    }

    /// <summary>
    /// metodo para detectar colision con pozo/cultivo
    /// </summary>
    /// <param name="collision"></param>
    void OnTriggerStay2D(Collider2D collision)
    {

        if (collision.CompareTag("Crop"))
        {
            _isInCropArea = true;
            cropSpriteEditor = collision.GetComponent<CropSpriteEditor>();
            _cropTransform = collision.gameObject.transform;
        }

        if (collision.CompareTag("Pozo"))
        {

            _isInWellArea = true;

        }

    }
    /// <summary>
    /// metodo para detectar cuando deja de colisionar con pozo/cultivo.
    /// </summary>
    /// <param name="collision"></param>
     void OnTriggerExit2D(Collider2D collision)
    {

        if (collision.CompareTag("Pozo"))
        {

            _isInWellArea = false;
            if(_notificationActive)
            {
                SoundManager.NextButtonSound();
                _notificationActive = false;
            }
            
        }

        if (collision.CompareTag("Crop"))
        {
            _isInCropArea = false;
            cropSpriteEditor = null;
        }

    }
    
    /// <summary>
    /// Metodo para Instanciar el aviso de que se esta recargando en el pozo.
    /// </summary>
    private void InstanceWarning()
    {

        _warning = Instantiate(PrefabWatering, WellPosition, Quaternion.identity);

    }

    /// <summary>
    /// Este método se llama cuando el jugador tiene seleccionada la regadera y pulsa E
    /// Busca cual es la planta para regar más cercano al jugador
    /// </summary>
    private Transform FindNearestPot(Transform Player, Transform[] Pots)
    {
        Debug.Log("FindNearestPot");

        Transform NearestPot = null;

        foreach (Transform pot in Pots)
        {
            float MinDistance = InitialMinDistance;
            if (pot.childCount != 0) // Comprueba que hay planta en esta posición
            {
                float SqrDistance = (Player.position - pot.position).sqrMagnitude;
                if (SqrDistance < MinDistance)
                {
                    MinDistance = SqrDistance;
                    NearestPot = pot;
                }
            }
        }

        return NearestPot;
    }

    ///<summary>
    ///Metodo para inicializar las referencias
    /// </summary>
    private void InitializeReferences()
    {
        SoundManager = FindObjectOfType<SoundManager>();
        NotificationManager = FindObjectOfType<NotificationManager>();
    }
    #endregion

} // class WateringCanManager 
// namespace
