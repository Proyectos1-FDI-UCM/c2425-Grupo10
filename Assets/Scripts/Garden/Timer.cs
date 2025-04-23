//---------------------------------------------------------
// Script para gestionar el tiempo en el juego,traduciendo el tiempo real a tiempo de juego.
// Alexia Pérez Santana
// Roots of Life
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using TMPro;
using UnityEngine;
using UnityEngine.UI; // Para mostrar el tiempo en pantalla


/// <summary>
/// Este script gestiona el tiempo del juego, 
/// convirtiendo el tiempo real en tiempo de juego. 
/// Además, muestra un reloj en pantalla para que el jugador 
/// pueda ver el tiempo transcurrido. 
/// Las plantas pueden consultar este tiempo para sus fases de crecimiento.
/// </summary>
public class Timer : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    /// <summary>
    /// Texto para mostrar el tiempo en pantalla.
    /// </summary>
    [SerializeField] private TextMeshProUGUI TimerText;
    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    /// <summary>
    /// Tiempo real transcurrido.
    /// </summary>
    [SerializeField]  private float _realTimeElapsed;

    /// <summary>
    /// Tiempo de juego transcurrido.
    /// </summary>
    [SerializeField] private float _gameTimeElapsed;

    /// <summary>
    /// 1 dia de juego = 6 minutos reales (360 segundos)
    /// </summary>
    private const float _secondsPerGameDay = 24*60*60f;

    /// <summary>
    /// Convertir segundos de la vida real a tiempo de juego
    /// </summary>
    private const float _realSecondtoGameTime = 60f;

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
        _realTimeElapsed = 0f; //Inicializar el tiempo real
        _gameTimeElapsed = 0f; //Inicializar el tiempo de juego
        GameManager.Instance.SetTimer(this);
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        _realTimeElapsed += Time.deltaTime; //Incrementar el tiempo real
        UpdateGameTime(); //Actualizar el tiempo de juego
        DisplayTime(); //Mostrar el tiempo en pantalla
    }
    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    /// <summary>
    /// Devuelve el tiempo de juego transcurrido en minutos.
    /// </summary>
    /// <returns>Tiempo de juego en minutos.</returns>
    public float GetGameTimeInMinutes()
    {
        return (_gameTimeElapsed / 60f); // Convertir a minutos
    }

    public float GetGameTimeInHours()
    {
        return (_gameTimeElapsed / 3600f); // Convertir a minutos
    }

    public float GetGameTimeInDays()
    {
        return (_gameTimeElapsed / 86400f); // Convertir a minutos
    }

    public float GetRealTime()
    {
        return _realTimeElapsed;
    }

    public void SetRealTime(float realTime)
    {
        _realTimeElapsed = realTime;
    }

    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    /// <summary>
    /// Actualiza el tiempo de juego basado en el tiempo real transcurrido.
    /// </summary>
    private void UpdateGameTime()
    {
        _gameTimeElapsed = _realTimeElapsed * _realSecondtoGameTime;
        // Calcular cuántos días de juego han pasado
        int gameDaysPassed = Mathf.FloorToInt(_gameTimeElapsed / _secondsPerGameDay);
        //_gameTimeElapsed = gameDaysPassed * 1440f; // 1 día = 24 horas = 1440 minutos

        //// Reiniciar el contador de tiempo real
        //if (gameDaysPassed > 0)
        //{
        //    _realTimeElapsed -= gameDaysPassed * _secondsPerGameDay;
        //}
    }

    /// <summary>
    /// Muestra el tiempo de juego en el texto de la UI.
    /// </summary>
    private void DisplayTime()
    {
        //int minutes = Mathf.FloorToInt(_gameTimeElapsed % 60f);
       int hours = Mathf.FloorToInt(_gameTimeElapsed / 60f) % 24; // Limitar a 24 horas
        TimerText.text = $"{hours:D2}:00"; // Formato HH:MM
    }

    #endregion

} // class Timer 

