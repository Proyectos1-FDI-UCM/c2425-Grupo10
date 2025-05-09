//---------------------------------------------------------
// Control de la animaci�n de nubes entre escenas
// Responsable: Javi
// Nombre del juego: Roots of Life
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Gestiona la animaci�n de unas nubes que se abren o cierran al cambiar de escena,
/// incluyendo tambi�n un efecto de zoom con Cinemachine o la c�mara principal.
/// Se asegura de no destruirse al cambiar de escena para mantenerse entre transiciones.
/// </summary>
public class Cloud : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)

    /// <summary>
    /// Referencia al transform de la nube izquierdas.
    /// </summary>
    [SerializeField] private Transform LeftCloud;
    [SerializeField] private Transform LeftCloud1;
    [SerializeField] private Transform LeftCloud2;
    [SerializeField] private Transform LeftCloud3;
    [SerializeField] private Transform LeftCloud4;

    /// <summary>
    /// Referencia al transform de la nube derechas.
    /// </summary>
    [SerializeField] private Transform RightCloud;
    [SerializeField] private Transform RightCloud2;
    [SerializeField] private Transform RightCloud1;
    [SerializeField] private Transform RightCloud3;
    [SerializeField] private Transform RightCloud4;

    /// <summary>
    /// Referencia al transform de la nube izquierdas.
    /// </summary>
    [SerializeField] private CinemachineVirtualCamera VirtualCam;

    /// <summary>
    /// Referencia a la c�mara principal para escenas sin Cinemachine.
    /// </summary>
    [SerializeField] private Camera Cam;

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)

    /// <summary>
    /// Distancia que las nubes se mover�n para separarse del centro.
    /// </summary>
    private float SeparationDistance = 1200f;

    /// <summary>
    /// Duraci�n de la animaci�n de las nubes en segundos.
    /// </summary>
    private float CloudDuration = 1f;

    /// <summary>
    /// Duraci�n de la animaci�n del zoom de la c�mara en segundos.
    /// </summary>
    private float ZoomDuration = 1f;

    /// <summary>
    /// Valor del tama�o ortogr�fico cuando la c�mara est� cercana.
    /// </summary>
    private float CloseZoom = 6f;

    /// <summary>
    /// Valor del tama�o ortogr�fico cuando la c�mara est� alejada.
    /// </summary>
    private float FarZoom = 10f;

    /// <summary>
    /// Instancia singleton de la clase Cloud.
    /// </summary>
    private static Cloud _instance;

    /// <summary>
    /// Posiciones iniciales para los transforms de las nubes izquierdas y derechas.
    /// </summary>
    private Vector3 _posIniLeft, _posIniLeft1, _posIniLeft2, _posIniLeft3, _posIniLeft4, _posIniRight, _posIniRight1, _posIniRight2, _posIniRight3, _posIniRight4;

    /// <summary>
    /// Posiciones abiertas (separadas) para los transforms de las nubes derechas e izquierdas.
    /// </summary>
    private Vector3 _posOpenIzq, _posOpenIzq1, _posOpenIzq2, _posOpenIzq3, _posOpenIzq4, _posOpenDer, _posOpenDer1, _posOpenDer2, _posOpenDer3, _posOpenDer4;

    /// <summary>
    /// Tiempo transcurrido durante la animaci�n actual.
    /// </summary>
    private float _time = 0f;

    /// <summary>
    /// Indicador para determinar si las nubes est�n anim�ndose actualmente.
    /// </summary>
    private bool _isAnimating;

    /// <summary>
    /// Indicador para determinar si las nubes se est�n abriendo o cerrando.
    /// </summary>
    private bool _isClearing;

    /// <summary>
    /// Referencia al componente de audio para reproducir sonidos de movimiento de nubes.
    /// </summary>
    private AudioSource _audioSource;

    #endregion

    // ---- M�TODOS DE MONOBEHAVIOUR ----
    #region M�todos de MonoBehaviour

    /// <summary>
    /// Awake se llama antes del Start. Asegura que el objeto no se destruya
    /// entre escenas y mantiene una sola instancia.
    /// </summary>
    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Start inicializa posiciones de las nubes y las prepara para animaci�n.
    /// </summary>
    void Start()
    {
        // Almacena las posiciones iniciales de todas las nubes izquierdas
        _posIniLeft = LeftCloud.position;
        _posIniLeft1 = LeftCloud1.position;
        _posIniLeft2 = LeftCloud2.position;
        _posIniLeft3 = LeftCloud3.position;
        _posIniLeft4 = LeftCloud4.position;

        // Almacena las posiciones iniciales de todas las nubes derechas
        _posIniRight = RightCloud.position;
        _posIniRight1 = RightCloud1.position;
        _posIniRight2 = RightCloud2.position;
        _posIniRight3 = RightCloud3.position;
        _posIniRight4 = RightCloud4.position;

        // Calcula las posiciones abiertas (nubes alejadas) para las nubes izquierdas
        _posOpenIzq = _posIniLeft + Vector3.left * SeparationDistance;
        _posOpenIzq1 = _posIniLeft1 + Vector3.left * SeparationDistance;
        _posOpenIzq2 = _posIniLeft2 + Vector3.left * SeparationDistance;
        _posOpenIzq3 = _posIniLeft3 + Vector3.left * SeparationDistance;
        _posOpenIzq4 = _posIniLeft4 + Vector3.left * SeparationDistance;

        // Calcula las posiciones abiertas (nubes alejadas) para las nubes derechas
        _posOpenDer = _posIniRight + Vector3.right * SeparationDistance;
        _posOpenDer1 = _posIniRight1 + Vector3.right * SeparationDistance;
        _posOpenDer2 = _posIniRight2 + Vector3.right * SeparationDistance; 
        _posOpenDer3 = _posIniRight3 + Vector3.right * SeparationDistance;
        _posOpenDer4 = _posIniRight4 + Vector3.right * SeparationDistance;

        // Obtiene el componente de audio para reproducir sonidos de transici�n
        _audioSource = GetComponent<AudioSource>();

        // Comienza con las nubes ocultas (abiertas)
        HideClouds();
    }

    /// <summary>
    /// Update se encarga de animar las nubes y realizar el zoom durante la transici�n.
    /// </summary>
    void Update()
    {
        // Busca la c�mara virtual si no est� asignada
        if (VirtualCam == null)
            VirtualCam = FindObjectOfType<CinemachineVirtualCamera>();

        if (_isAnimating)
        {
            _time += Time.deltaTime;
            float t = Mathf.Clamp01(_time / CloudDuration);  // Duraci�n de las nubes

            // Animaci�n de las nubes
            if (_isClearing)
            {
                // Mueve las nubes separ�ndolas(abri�ndolas)
                LeftCloud.position = Vector3.Lerp(_posIniLeft, _posOpenIzq, t);
                LeftCloud1.position = Vector3.Lerp(_posIniLeft1, _posOpenIzq1, t);
                LeftCloud2.position = Vector3.Lerp(_posIniLeft2, _posOpenIzq2, t);
                LeftCloud3.position = Vector3.Lerp(_posIniLeft3, _posOpenIzq3, t);
                LeftCloud4.position = Vector3.Lerp(_posIniLeft4, _posOpenIzq4, t);

                RightCloud.position = Vector3.Lerp(_posIniRight, _posOpenDer, t);
                RightCloud1.position = Vector3.Lerp(_posIniRight1, _posOpenDer1, t);
                RightCloud2.position = Vector3.Lerp(_posIniRight2, _posOpenDer2, t);
                RightCloud3.position = Vector3.Lerp(_posIniRight3, _posOpenDer3, t);
                RightCloud4.position = Vector3.Lerp(_posIniRight4, _posOpenDer4, t);

            }
            else
            {
                // Mueve las nubes junt�ndolas (cerr�ndolas)
                LeftCloud.position = Vector3.Lerp(_posOpenIzq, _posIniLeft, t);
                LeftCloud1.position = Vector3.Lerp(_posOpenIzq1, _posIniLeft1, t);
                LeftCloud2.position = Vector3.Lerp(_posOpenIzq2, _posIniLeft2, t);
                LeftCloud3.position = Vector3.Lerp(_posOpenIzq3, _posIniLeft3, t);
                LeftCloud4.position = Vector3.Lerp(_posOpenIzq4, _posIniLeft4, t);

                RightCloud.position = Vector3.Lerp(_posOpenDer, _posIniRight, t);
                RightCloud1.position = Vector3.Lerp(_posOpenDer1, _posIniRight1, t);
                RightCloud2.position = Vector3.Lerp(_posOpenDer2, _posIniRight2, t);
                RightCloud3.position = Vector3.Lerp(_posOpenDer3, _posIniRight3, t);
                RightCloud4.position = Vector3.Lerp(_posOpenDer4, _posIniRight4, t);

            }

            // Animaci�n del zoom de la c�mara
            float cameraT = Mathf.Clamp01(_time / ZoomDuration);  // Duraci�n para el zoom de la c�mara
            if (_isClearing)
            {
                if (SceneManager.GetActiveScene().name == "Escena_Build")
                {
                    VirtualCam.m_Lens.OrthographicSize = Mathf.Lerp(FarZoom, CloseZoom, cameraT);
                }
                else
                {
                    Cam = Camera.main;
                    Cam.orthographicSize = Mathf.Lerp(FarZoom, CloseZoom, cameraT);
                }
            }

            // Si ya ha alcanzado el final de la animaci�n, detener la animaci�n
            if (t >= 1f && cameraT >= 1f)
                _isAnimating = false;
        }
    }

    /// <summary>
    /// M�todo llamado cuando se destruye el componente.
    /// </summary>
    protected void OnDestroy()
    {
        // Limpia la referencia de instancia singleton si esta es la instancia actual
        if (this == _instance)
        {
            // �ramos la instancia de verdad, no un clon.
            _instance = null;
        } // if somos la instancia principal  

    }

    #endregion

    // ---- M�TODOS P�BLICOS ----
    #region M�todos p�blicos

    /// <summary>
    /// Propiedad para acceder a la �nica instancia de la clase.
    /// </summary>
    public static Cloud Instance
    {
        get
        {
            Debug.Assert(_instance != null);
            return _instance;
        }
    }

    /// <summary>
    /// Devuelve verdadero si la instancia singleton existe, falso en caso contrario.
    /// �til durante el cierre para evitar usar un Cloud que podr�a haber sido destruido.
    /// </summary>
    /// <returns>Verdadero si la instancia existe.</returns>
    public static bool HasInstance()
    {
        return _instance != null;
    }

    /// <summary>
    /// Muestra las nubes cerr�ndose. Se usa para ocultar contenido antes del cambio de escena.
    /// </summary>
    public void ShowClouds()
    {
        _time = 0f;
        _isClearing = false;
        _isAnimating = true;
        _audioSource.Play();
    }

    /// <summary>
    /// Oculta las nubes abri�ndolas. Se usa al cargar una escena nueva.
    /// </summary>
    public void HideClouds()
    {
        _time = 0f;
        _isClearing = true;
        _isAnimating = true;
        _audioSource.Play();
    }

    /// <summary>
    /// Configura los par�metros de la c�mara para la escena final con una vista m�s amplia y transici�n m�s lenta.
    /// </summary>
    public void FinalCamera()
    {
        FarZoom = 25;
        CloseZoom = 10;
        ZoomDuration = 10;
    }

    #endregion
} // class Cloud