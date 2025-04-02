//---------------------------------------------------------
// Este Script se encarga de inicializar las plantas y modificar los sprites de los cultivos 
// Julia Vera Ruiz
// Roots of Life
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// CropSpriteEditor, inicializa las plantas 
/// Modifica los sprites en función del TimerManager
/// </summary>
public class CropSpriteEditor : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints
    [SerializeField] private Timer gameTimer;
    /// <summary>
    /// Sprites de Estados de las Plantas Vivas
    /// </summary>

    [SerializeField] private Sprite PlantStage1;
    [SerializeField] private Sprite PlantStage2;
    [SerializeField] private Sprite PlantStage3;
    [SerializeField] private Sprite PlantStage4;
    [SerializeField] private Sprite PlantStageWeeds;

    /// <summary>
    /// Sprites de Estados de las Plantas Muertas
    /// </summary>

    [SerializeField] private Sprite PlantDeadStage1;
    [SerializeField] private Sprite PlantDeadStage2;
    [SerializeField] private Sprite PlantDeadStage3;
    [SerializeField] private Sprite PlantDeadStage4;

    [SerializeField] private Sprite WarningWater;
    [SerializeField] private Sprite WarningDeath;
    [SerializeField] private Sprite WarningCollect;

    [SerializeField] private Items item;

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // privados se nombren en formato _camelCase (comienza con _, 
    // primera palabra en minúsculas y el resto con la 
    // primera letra en mayúsculas)
    // Ejemplo: _maxHealthPoints

    /// <summary>
    /// Sprite Renderer de los avisos
    /// </summary>
    /// 
    private SpriteRenderer _warning;

    /// <summary>
    /// Sprites de Estados de las Plantas
    /// </summary>
    /// 
    private SpriteRenderer _spriteRenderer;


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
        GardenData.Active(transform, item); // Inicializa la Planta en GardenManager

        _warning = transform.GetChild(0).transform.GetComponent<SpriteRenderer>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        Warning("Water");
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
    /// Cambia los avisos 
    /// </summary>
    public void Warning(string Type)
    {
        if (Type == "Desactivate")
        {
            _warning.enabled = false;
        }
        if (Type == "Water")
        {
            _warning.enabled = true;
            _warning.sprite = WarningWater;
        }
        if (Type == "Death")
        {
            _warning.enabled = true;
            _warning.sprite = WarningDeath;
        }
        if (Type == "Collect")
        {
            _warning.enabled = true;
            _warning.sprite = WarningCollect;
        }
    }

    /// <summary>
    /// Cambia los sprites 
    /// </summary>
    public void State(string Type)
    {
        if (Type == "State1")
        {
            _spriteRenderer.enabled = true;
            _spriteRenderer.sprite = PlantStage1;
        }
        if (Type == "State2")
        {
            _spriteRenderer.enabled = true;
            _spriteRenderer.sprite = PlantStage2;
        }
        if (Type == "State3")
        {
            _spriteRenderer.enabled = true;
            _spriteRenderer.sprite = PlantStage3;
        }
        if (Type == "State4")
        {
            _spriteRenderer.enabled = true;
            _spriteRenderer.sprite = PlantStage4;
        }
        if (Type == "State-4")
        {
            _spriteRenderer.enabled = true;
            _spriteRenderer.sprite = PlantStageWeeds;
        }
    }

    /// <summary>
    /// Modifica los valores de crecimiento
    /// </summary>
    public void Growing(int state)
    {
        if (state == 1) State("State2");
        else if (state == 2) State("State3");
        else if (state == 3) State("State4");
        //Grown(transform); // Modifica el timer de crecimiento
    }

    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)

    #endregion

} // class CropSpriteEditor 
// namespace
