//---------------------------------------------------------
// Script para manejar la evolución y recolección de la planta.
// Responsable: Natalia Nita, Julia Vera, Iria Docampo y Alexia Pérez Santana
// Nombre del juego: Roots of Life
// Curso 2024-25
//---------------------------------------------------------

using UnityEngine;

/// <summary>
/// Maneja la evolución de la planta.
/// Permite su recolección cuando alcanza su fase máxima.
/// </summary>
public class PlantaEvolucion : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector

    [SerializeField] private Sprite PlantaFase1;
    [SerializeField] private Sprite PlantaFase2;
    [SerializeField] private Sprite PlantaFase3;
    [SerializeField] private Sprite PlantaFase4;

    [SerializeField] private GameObject PrefabDead;    // Prefab planta muerta

    [SerializeField] private GameObject PrefabSuelo; // Prefab efecto tierra mojada
    [SerializeField] private GameObject PrefabAvisoRiego; // Prefab regadera (se activa cuando es necesario regar la planta)
    [SerializeField] private GameObject PrefabWarningDead; // Prefab aviso muerte (se activa cuando faltan 10 segundos para que la planta muera)
    [SerializeField] private GameObject PrefabAvisoRecolecta; // Prefab hoz (se activa cuando es necesario recolectar la planta)
    [SerializeField] private GameObject PrefabMaceta; // Si no funciona, pruebo a eliminar la maceta y reactivarla cuando arrancas la planta
    [SerializeField] private string NombrePlanta = "Cultivo";

    [SerializeField] private int Tipo = 1;

    // Pruebas
    [SerializeField] private int TiempoCrecimiento = 5;
    [SerializeField] private int TiempoRegado = 3;
    [SerializeField] private int TiempoMuerte = 3;


    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados

    private SpriteRenderer _spriteRenderer;
    private GameObject _avisos;
    private GameObject _maceta;
    private CropSpawner _cropSpawner;

    private int EstadoCrecimiento; // = -1 para los cultivos muertos
    private int _guardarEstado;
    private bool EstadoRiego;
    private bool _isDead;
    
    private float TimerCrecimiento;
    private float TimerRiego;
   // private float TimerDead;
    private bool _cosechado = false;

    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        
        //_plantas = transform.parent;
        
        // Se cambia en plantar - para poder implementar un nuevo metodo de inicializacion si es necesario

        //_spriteRenderer.sprite = PlantaFase1;  // Inicia en fase 1
        //EstadoCrecimiento = 0;
        
        EstadoRiego = false;
        _isDead = false;
        TimerCrecimiento = TiempoCrecimiento;

    }

    private void Update()
    {
        // IRIA - Cambios que he hecho en partes ajenas del update: Instaciar el contador de muerte (20s) cuando pasa de la fase 1 y termina el de regado.
        // Cambios que he hecho en métodos ajenos: 1) En Regar(), reseteo el contador de muerte después del de riego.



            if (TimerRiego <= 0f)
            {
                AvisoRiego();
            }

            if (EstadoCrecimiento >= 0) // Si se ha regado y plantado
            {
                if (EstadoRiego && EstadoCrecimiento == 0) TimerCrecimiento -= Time.deltaTime;
                else if (TimerCrecimiento > 0) TimerCrecimiento -= Time.deltaTime;
                // Debug.Log("Timer Crecimiento: " + TimerCrecimiento);

                TimerRiego -= Time.deltaTime;
              //  Debug.Log("Timer Riego: " + TimerRiego);

            }

            if (TimerCrecimiento <= 0f)
            {
                Crecimiento();
            }

            if (TimerRiego <= -2) // 10 seg antes de que muera salta el aviso
            {
                WarningDead();
            //   Debug.Log("WARNING DEATH");
                 //_guardarEstado = EstadoCrecimiento;
            }

            if (TimerRiego <= -TiempoMuerte)
            {
                 EstadoCrecimiento = -1; 
                Death();
               TimerCrecimiento = 3f;
               TimerRiego = 3f;
            Destroy(gameObject);
            }
        
    }

    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos Públicos

    /// <summary>
    /// Se activa cuando la planta inicia desde una semilla, establece el estado de crecimiento y de riego y el tiempo de crecimiento.
    /// </summary>
    public void Planta(GameObject maceta)
    {
        _maceta = maceta;

        _cropSpawner = _maceta.GetComponent<CropSpawner>();

        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.sprite = PlantaFase1;  // Inicia en fase 1

        EstadoCrecimiento = 0;
        EstadoRiego = false;
        TimerCrecimiento = TiempoCrecimiento; // Lleva TiempoCrecimiento creciendo 

    }

    /// <summary>
    /// Cosecharla planta.
    /// </summary>
    public void Cosechar()
    {
        Destroy(_avisos);

        _cropSpawner.Reactivar();
        _cosechado = true;

        Destroy(gameObject); // Elimina la planta del mapa tras recogerla

        int [] i = GameManager.Instance.Inventario();
        i[Tipo]++;
    }

 

    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados

    /// <summary>
    /// Regar la planta
    /// </summary>
    private void Regar()
    {
        Destroy(_avisos); // Se eliminan los avisos de riego
        LevelManager.Instance.Regar(); // Se avisa al level manager para que contabilice el agua de la regadera

        // Se moja la tierra
        GameObject suelo = Instantiate(PrefabSuelo, transform.position, Quaternion.identity); 
        suelo.transform.SetParent(transform);

        // Inicia la cuenta atrás del tiempo de regado
        TimerRiego = TiempoMuerte;
       // TimerDead = TiempoMuerte;
        EstadoRiego = true;

    }

    /// <summary>
    /// Muerte de la planta
    /// </summary>
    private void Death()
    {
        Destroy(_avisos); // Se eliminan los avisos de muerte

        GameObject dead = Instantiate(PrefabDead, transform.position, Quaternion.identity);
        dead.transform.SetParent(transform.parent);

        ChangeDeadSprite Script = dead.GetComponent<ChangeDeadSprite>();
        Script.ChangeSprite(EstadoCrecimiento);

        Debug.Log("Planta muerta en estado: " + EstadoCrecimiento);
        // TimerRiego = TiempoMuerte;
        //AvisoRecolecta();

       // Destroy(gameObject); // DestruyePlanta
    }

    /// <summary>
    /// Cambia el estado de la planta según su crecimiento.
    /// </summary>
    private void Crecimiento()
    {
        if (EstadoCrecimiento == 0)
        {
            _spriteRenderer.sprite = PlantaFase2;
            EstadoCrecimiento = 1;
            TimerCrecimiento = TiempoCrecimiento;
           // _guardarEstado = 1;
        }
        else if (EstadoCrecimiento == 1)
        {
            _spriteRenderer.sprite = PlantaFase3;
            EstadoCrecimiento = 2;
            TimerCrecimiento = TiempoCrecimiento;
            //_guardarEstado = 2;
        }
        else if (EstadoCrecimiento == 2)
        {
            _spriteRenderer.sprite = PlantaFase4;
            EstadoCrecimiento = 3;
            TimerCrecimiento = TiempoCrecimiento;
            AvisoRecolecta();
            //_guardarEstado = 3;
        }
        Debug.Log("ESTADO:" + EstadoCrecimiento);

    }
    // AVISOS ------------------
    /// <summary>
    /// Métodos que instancian los prefabs de avisos para el riego, la recolecta y la muerte.
    /// </summary>
    private void AvisoRiego()
    {
        Destroy(_avisos);

        if (EstadoCrecimiento !=3 && EstadoCrecimiento != -1 && TimerRiego > -TiempoRegado)
        {
            _avisos = Instantiate(PrefabAvisoRiego, transform.position, Quaternion.identity);
            _avisos.transform.SetParent(transform);

            EstadoRiego = false;
        }
    }

    private void AvisoRecolecta()
    {
        if (_avisos != null) 
        { 
            Destroy(_avisos.gameObject); 
        }

        _avisos = Instantiate(PrefabAvisoRecolecta, transform.position, Quaternion.identity);

        _avisos.transform.SetParent(transform);
        EstadoRiego = true;
    }

    private void WarningDead()
    {
        Destroy(_avisos);

        _avisos = Instantiate(PrefabWarningDead, transform.position, Quaternion.identity); // Instanciar prefab

        _avisos.transform.SetParent(transform);

        Debug.Log("Aviso muerte.");
    }

    // AVISOS ------------------

    #endregion

    // ---- EVENTOS ----
    #region Eventos

    /// <summary>
    /// Detecta si el jugador interactúa con la planta.
    /// </summary>
    private void OnCollisionStay2D()
    {
        Debug.Log("Colisión con planta detectada.");

        // Si el jugador tiene la regadera y presiona el botón, riega la planta
        int Regadera = LevelManager.Instance.Regadera();
        if (InputManager.Instance.UsarIsPressed() && LevelManager.Instance.Herramientas() == 2 && Regadera > 0 && !EstadoRiego)
        {
            Regar();
        }

        // Si la planta está lista para cosechar y el jugador tiene guantes (Herramienta 1), la recoge
        if (InputManager.Instance.UsarIsPressed() && LevelManager.Instance.Herramientas() == 3 && EstadoCrecimiento == 3)
        {
            Cosechar();
        }
    }

    #endregion
}
