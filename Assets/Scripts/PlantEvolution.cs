//---------------------------------------------------------
// Script para manejar la evolución y recolección de la planta.
// Responsable: Natalia Nita, Julia Vera, Iria Docampo y Alexia Pérez Santana
// Nombre del juego: Roots of Life
// Curso 2024-25
//---------------------------------------------------------

using UnityEngine;

/// <summary>
/// Clase que maneja la evolución de la planta.
/// Permite su recolección cuando alcanza su fase máxima.
/// </summary>
public class PlantEvolution : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector

    //Sprite de las plantas en sus respectivos estados
    [SerializeField] private Sprite PlantStage1;
    [SerializeField] private Sprite PlantStage2;
    [SerializeField] private Sprite PlantStage3;
    [SerializeField] private Sprite PlantStage4;

    // Prefab planta muerta
    [SerializeField] private GameObject PrefabDead;

    // Prefab efecto tierra mojada
    [SerializeField] private GameObject PrefabSoil;

    // Prefab regadera (se activa cuando es necesario regar la planta)
    [SerializeField] private GameObject PrefabWateringWarning;

    // Prefab aviso muerte (se activa cuando faltan 10 segundos para que la planta muera)
    [SerializeField] private GameObject PrefabDeadWarning;

    // Prefab hoz (se activa cuando es necesario recolectar la planta)
    [SerializeField] private GameObject PrefabHarvestWarning;

    // Prefab de la maceta
    [SerializeField] private GameObject PrefabPot; 

    //Nombre por el que se detecta que algo es una planta
    [SerializeField] private string PlantName = "Crop";

    //Tipo de planta
    [SerializeField] private int Type = 1;

    // Pruebas
    [SerializeField] private int GrowthTime = 5; // Tiempo de crecimiento
    [SerializeField] private int WateringTime = 3; // Tiempo de riego
    [SerializeField] private int DeathTime = 3; // Tiempo antes de morir


    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados

    /// <summary>
    /// Componente SpriteRenderer de la planta.
    /// </summary>
    private SpriteRenderer _spriteRenderer;

    /// <summary>
    /// Objeto que contiene los avisos.
    /// </summary>
    private GameObject _warnings;

    /// <summary>
    /// Referencia a la maceta.
    /// </summary>
    private GameObject _pot;

    /// <summary>
    /// Componente CropSpawner asociado a la maceta.
    /// </summary>
    private CropSpawner _cropSpawner;

    /// <summary>
    /// Estado de crecimiento de la planta.
    /// </summary>
    private int _growthState; // = -1 para los cultivos muertos

    /// <summary>
    /// Estado guardado de la planta.
    /// </summary>
    private int _savedState;

    /// <summary>
    /// Indica si la planta ha sido regada.
    /// </summary>
    private bool _isWatered;

    /// <summary>
    /// Indica si la planta está muerta.
    /// </summary>
    private bool _isDead;

    /// <summary>
    /// Temporizador para el crecimiento de la planta.
    /// </summary>
    private float _growthTimer;

    /// <summary>
    /// Temporizador para el riego de la planta.
    /// </summary>
    private float _wateringTimer;

    // private float TimerDead;

    /// <summary>
    /// Indica si la planta ha sido cosechada.
    /// </summary>
    private bool _harvested = false;

    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    /// <summary>
    /// Se llama al comenzar el juego. Inicializa los componentes necesarios.
    /// </summary>
    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();

        //_plantas = transform.parent;

        // Se cambia en plantar - para poder implementar un nuevo metodo de inicializacion si es necesario

        //_spriteRenderer.sprite = PlantPhase1;  // Inicia en fase 1
        //_growthState = 0;

        _isWatered = false;
        _isDead = false;
        _growthTimer = GrowthTime; 
    }

    /// <summary>
    /// Se llama cada frame, si el MonoBehaviour está habilitado.
    /// </summary>
    private void Update()
    {
        // IRIA - Cambios que he hecho en partes ajenas del update: Instaciar el contador de muerte (20s) cuando pasa de la fase 1 y termina el de regado.
        // Cambios que he hecho en métodos ajenos: 1) En Watering(), reseteo el contador de muerte después del de riego.

        // Lógica para manejar el crecimiento y riego de la planta.
        if (_growthState >= 0) // Si se ha regado y plantado
        {
             if (_isWatered && _growthState == 0) _growthTimer -= Time.deltaTime;
                
             else if (_growthTimer > 0) _growthTimer -= Time.deltaTime;
            // Debug.Log("Timer Crecimiento: " + _growthTimer);

            _wateringTimer -= Time.deltaTime;
            // Debug.Log("Timer Riego: " + _wateringTimer);

        }

        if (_wateringTimer <= 0f)
        {
               WateringWarning();
        }
            
        if (_growthTimer <= 0f)
        {
                Growth();
        }

        if (_wateringTimer <= -2) // 10 seg antes de que muera salta el aviso
        {
             WarningDead();
            // Debug.Log("WARNING DEATH");
            //_savedState = _growthState;
        }

        if (_wateringTimer <= -DeathTime)
        {
                 _growthState = -1; 
                 Death();
                 _growthTimer = 3f;
                 _wateringTimer = 3f;
                 Destroy(gameObject);
        }
        
    }

    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos Públicos

    /// <summary>
    /// Se activa cuando la planta inicia desde una semilla, establece el estado de crecimiento y de riego y el tiempo de crecimiento.
    /// </summary>
    public void Plant(GameObject pot)
    {
        _pot = pot;

        _cropSpawner = _pot.GetComponent<CropSpawner>();

        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.sprite = PlantStage1;  // Inicia en fase 1

        _growthState = 0;
        _isWatered = false;
        _growthTimer = GrowthTime; // Lleva _growthTimer creciendo 

    }

    /// <summary>
    /// Cosechar la planta.
    /// </summary>
    public void Harvest()
    {
        Destroy(_warnings);

        _cropSpawner.Reactivar();
        _harvested = true;

        Destroy(gameObject); // Elimina la planta del mapa tras recogerla

        int [] i = GameManager.Instance.Inventario();
        i[Type]++;
    }


    /// <summary>
    /// Regar la planta
    /// </summary>
    public void Watering()
    {
        Destroy(_warnings); // Se eliminan los avisos de riego

        // Se moja la tierra
        GameObject suelo = Instantiate(PrefabSoil, transform.position, Quaternion.identity);
        suelo.transform.SetParent(transform);

        // Inicia la cuenta atrás del tiempo de regado
        _wateringTimer = DeathTime;
        //_timerDead = DeathTime;
        _isWatered = true;

    }

    /// <summary>
    /// Obtiene el temporizador de riego.
    /// </summary>
    public float GetWateringTimer()
    {
        return _wateringTimer;
    }

    /// <summary>
    /// Obtiene el estado de crecimiento de la planta.
    /// </summary>
    public float GetGrowthState()
    {
        return _growthState;
    }
    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados



    /// <summary>
    /// Muerte de la planta
    /// </summary>
    private void Death()
    {
        Destroy(_warnings); // Se eliminan los avisos de muerte

        GameObject dead = Instantiate(PrefabDead, transform.position, Quaternion.identity);
        dead.transform.SetParent(transform.parent);

        ChangeDeadSprite Script = dead.GetComponent<ChangeDeadSprite>();
        Script.ChangeSprite(_growthState);

        Debug.Log("Planta muerta en estado: " + _growthState);
        //_wateringTimer = DeathTime;
        //HarvestWarning();

        // Destroy(gameObject); // DestruyePlanta
    }

    /// <summary>
    /// Cambia el estado de la planta según su crecimiento.
    /// </summary>
    public void Growth()
    {
        if (_growthState == 0)
        {
            _spriteRenderer.sprite = PlantStage2;
            _growthState = 1;
            _growthTimer = GrowthTime;
            //_savedState = 1;
        }

        else if (_growthState == 1)
        {
            _spriteRenderer.sprite = PlantStage3;
            _growthState = 2;
            _growthTimer = GrowthTime;
            //_savedState = 2;
        }

        else if (_growthState == 2)
        {
            _spriteRenderer.sprite = PlantStage4;
            _growthState = 3;
            _growthTimer = GrowthTime;
            HarvestWarning();
            //_savedState = 3;
        }
        Debug.Log("ESTADO:" + _growthState);

    }
    // AVISOS ------------------
    /// <summary>
    /// Métodos que instancian los prefabs de avisos para el riego, la recolecta y la muerte.
    /// </summary>
    private void WateringWarning()
    {
        Destroy(_warnings);

        if (_growthState != 3 && _growthState != -1 && _wateringTimer > -WateringTime)
        {
            _warnings = Instantiate(PrefabWateringWarning, transform.position, Quaternion.identity);
            _warnings.transform.SetParent(transform);

            _isWatered = false;
        }
    }

    /// <summary>
    /// Muestra el aviso de recolección.
    /// </summary>
    private void HarvestWarning()
    {
        if (_warnings != null) 
        { 
            Destroy(_warnings.gameObject); 
        }

        _warnings = Instantiate(PrefabHarvestWarning, transform.position, Quaternion.identity);

        _warnings.transform.SetParent(transform);
        _isWatered = true;
    }

    /// <summary>
    /// Muestra el aviso de muerte.
    /// </summary>
    private void WarningDead()
    {
        Destroy(_warnings);

        _warnings = Instantiate(PrefabDeadWarning, transform.position, Quaternion.identity); // Instanciar prefab

        _warnings.transform.SetParent(transform);

        Debug.Log("Aviso muerte.");
    }

    // AVISOS ------------------

    #endregion

    // ---- EVENTOS ----
    #region Eventos

    #endregion
}
