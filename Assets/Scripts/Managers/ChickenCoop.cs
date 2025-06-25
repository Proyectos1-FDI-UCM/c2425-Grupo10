//---------------------------------------------------------
// Script responsable del gallinero
// Iria Docampo
// Roots of Life
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Se elige aleatoriamente un momento en el que se pone el huevo. Cuando llega ese momento, si aún hay capacidad se lleva a cabo la puesta. El jugador puede recogerlos de 1 en 1 (una notificación avisará de cuántos hay) 
/// </summary>
public class ChickenCoop : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints


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
    /// Referencia al Timer
    /// </summary>
    private Timer _timer;

    /// <summary>
    /// Referencia al UIManager
    /// </summary>
    private UIManager _uIManager;

    /// <summary>
    /// Contador de huevos puestos
    /// </summary>
    private int _eggs;

    /// <summary>
    /// Máximo de huevos que puede poner una gallina por el rango de tiempo elegido
    /// </summary>
    private int _maxEggs = 1;

    /// <summary>
    /// Cada cuantos dias se ponen huevos
    /// </summary>
    private int _days = 1;

    /// <summary>
    /// Numero de gallonas
    /// </summary>
    private int _chickens = 3;

    private bool coll = false;

    /// <summary>
    /// Referencia al tiempo en el juego
    /// </summary>
    private float _currentTime;

    /// <summary>
    /// Array que contiene los momentos del día en el que se pone cada huevo
    /// </summary>
    private int [] _layingTimes;

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
        InitializeReferences();
        _eggs = 0;
        _layingTimes = new int [_maxEggs * _chickens];
        SetNewLayingTimes();
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        _currentTime =  _timer.GetGameTimeInMinutes(); // En qué momento del día nos encontramos
      //  int currTime = Mathf.FloorToInt(_currentTime) % (24 * _days); // Cada cuántas horas se pone 1 huevo según _days
      int currTime = Mathf.FloorToInt(_currentTime) % (48 * _days); // Cada cuántas horas se pone 1 huevo según _days

        for (int i = 0; i < _layingTimes.Length; i++)
        {
            if (currTime == _layingTimes[i] && _eggs < _maxEggs)
            {
                _eggs++;
                SetNewLayingTime(i);
            }
        }

        if (coll)
        {
            Debug.Log("HUEVOS" + _eggs);
            if (_eggs > 0)
            {
                _uIManager.ShowNotification($"hay {_eggs} huevos \n pulsa E para\n recoger", "NoCounter", 1, "NoTutorial");


                if (InputManager.Instance.UsarWasPressedThisFrame())
                {
                    _eggs--;
                    _uIManager.HideNotification("NoTutorial");
                    _uIManager.ShowNotification($"hay {_eggs} huevos \n pulsa E para\n recoger", "NoCounter", 1, "NoTutorial");

                    AddEggsInventory(1);
                    // LLamar sonido
                }
            }
            else if (_eggs == 0)
            {
                _uIManager.ShowNotification("No hay huevos...", "NoCounter", 1, "NoTutorial");
            }
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

    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)
    private void InitializeReferences()
    {
        _uIManager = FindObjectOfType<UIManager>();
       // _soundManager = FindObjectOfType<SoundManager>();
        _timer = FindObjectOfType<Timer>();
    }

    /// <summary>
    /// Método para reestablecer nuevos tiempos de puesta para todas las gallinas
    /// </summary>
    private void SetNewLayingTimes()
    {
        for (int i = 0; i < _layingTimes.Length; i++)
        {
            SetNewLayingTime(i);
        }
    }

    /// <summary>
    /// Método para reestablecer un nuevo tiempo de puesta para una gallina en concreto
    /// </summary>
    private void SetNewLayingTime(int i)
    {
      _layingTimes[i] = UnityEngine.Random.Range(0, (24 * _days));
    }


    /// <summary>
    /// Añade 1 huevo al inventario
    /// </summary>
    private void AddEggsInventory(int quantity)
    {
        InventoryManager.BoolModifyInventory(Items.Egg, quantity);
        _uIManager.ActualizeInventory();
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        coll = true;
        
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        coll = false;
        _uIManager.HideNotification("NoTutorial");
    }
    #endregion   

} // class ChickenCoop 
// namespace
