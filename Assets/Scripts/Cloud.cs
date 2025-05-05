//---------------------------------------------------------
// Control de la animación de nubes entre escenas
// Responsable: Javi
// Nombre del juego: Roots of Life
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Gestiona la animación de unas nubes que se abren o cierran al cambiar de escena,
/// incluyendo también un efecto de zoom con Cinemachine o la cámara principal.
/// Se asegura de no destruirse al cambiar de escena para mantenerse entre transiciones.
/// </summary>
public class Cloud : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)

    [SerializeField] private Transform NubeIzquierda;
    [SerializeField] private Transform NubeIzquierda1;
    [SerializeField] private Transform NubeIzquierda2;
    [SerializeField] private Transform NubeIzquierda3;
    [SerializeField] private Transform NubeIzquierda4;

    [SerializeField] private Transform NubeDerecha;
    [SerializeField] private Transform NubeDerecha2;
    [SerializeField] private Transform NubeDerecha1;
    [SerializeField] private Transform NubeDerecha3;
    [SerializeField] private Transform NubeDerecha4;

    [SerializeField] private float DistanciaSeparacion = 10f;
    [SerializeField] private float Duracion = 2f;
    [SerializeField] private float ZoomCerca = 6f;
    [SerializeField] private float ZoomLejos = 10f;

    [SerializeField] private CinemachineVirtualCamera VirtualCamera;
    [SerializeField] private Camera Cam;

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)

    private static Cloud _instance;

    private Vector3 _posIniIzq, _posIniIzq1, _posIniIzq2, _posIniIzq3, _posIniIzq4, _posIniDer, _posIniDer1, _posIniDer2, _posIniDer3, _posIniDer4;
    private Vector3 _posOpenIzq, _posOpenIzq1, _posOpenIzq2, _posOpenIzq3, _posOpenIzq4, _posOpenDer, _posOpenDer1, _posOpenDer2, _posOpenDer3, _posOpenDer4;

    private float _tiempo = 0f;
    private bool _animando;
    private bool _aclarando;
    private AudioSource _audioSource;

    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

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
    /// Start inicializa posiciones de las nubes y las prepara para animación.
    /// </summary>
    void Start()
    {
        _posIniIzq = NubeIzquierda.position;
        _posIniIzq1 = NubeIzquierda1.position;
        _posIniIzq2 = NubeIzquierda2.position;
        _posIniIzq3 = NubeIzquierda3.position;
        _posIniIzq4 = NubeIzquierda4.position;


        _posIniDer = NubeDerecha.position;
        _posIniDer1 = NubeDerecha1.position;
        _posIniDer2 = NubeDerecha2.position;
        _posIniDer3 = NubeDerecha3.position;
        _posIniDer4 = NubeDerecha4.position;


        _posOpenIzq = _posIniIzq + Vector3.left * DistanciaSeparacion;
        _posOpenIzq1 = _posIniIzq1 + Vector3.left * DistanciaSeparacion;
        _posOpenIzq2 = _posIniIzq2 + Vector3.left * DistanciaSeparacion;
        _posOpenIzq3 = _posIniIzq3 + Vector3.left * DistanciaSeparacion;
        _posOpenIzq4 = _posIniIzq4 + Vector3.left * DistanciaSeparacion;

        _posOpenDer = _posIniDer + Vector3.right * DistanciaSeparacion;
        _posOpenDer1 = _posIniDer1 + Vector3.right * DistanciaSeparacion;
        _posOpenDer2 = _posIniDer2 + Vector3.right * DistanciaSeparacion; 
        _posOpenDer3 = _posIniDer3 + Vector3.right * DistanciaSeparacion;
        _posOpenDer4 = _posIniDer4 + Vector3.right * DistanciaSeparacion;

        _audioSource = GetComponent<AudioSource>();
        HideClouds();
    }

    /// <summary>
    /// Update se encarga de animar las nubes y realizar el zoom durante la transición.
    /// </summary>
    void Update()
    {
        if (VirtualCamera == null)
            VirtualCamera = FindObjectOfType<CinemachineVirtualCamera>();

        if (_animando)
        {
            _tiempo += Time.deltaTime;
            float t = Mathf.Clamp01(_tiempo / Duracion);

            if (_aclarando)
            {
                NubeIzquierda.position = Vector3.Lerp(_posIniIzq, _posOpenIzq, t);
                NubeIzquierda1.position = Vector3.Lerp(_posIniIzq1, _posOpenIzq1, t);
                NubeIzquierda2.position = Vector3.Lerp(_posIniIzq2, _posOpenIzq2, t);
                NubeIzquierda3.position = Vector3.Lerp(_posIniIzq3, _posOpenIzq3, t);
                NubeIzquierda4.position = Vector3.Lerp(_posIniIzq4, _posOpenIzq4, t);

                NubeDerecha.position = Vector3.Lerp(_posIniDer, _posOpenDer, t);
                NubeDerecha1.position = Vector3.Lerp(_posIniDer1, _posOpenDer1, t);
                NubeDerecha2.position = Vector3.Lerp(_posIniDer2, _posOpenDer2, t);
                NubeDerecha3.position = Vector3.Lerp(_posIniDer3, _posOpenDer3, t);
                NubeDerecha4.position = Vector3.Lerp(_posIniDer4, _posOpenDer4, t);


                if (SceneManager.GetActiveScene().name == "Escena_Build")
                {

                    VirtualCamera.m_Lens.OrthographicSize = Mathf.Lerp(ZoomLejos, ZoomCerca, t);

                }
                else
                {
                    Cam = Camera.main;
                    Cam.orthographicSize = Mathf.Lerp(ZoomLejos, ZoomCerca, t);
                }
            }
            else
            {
                NubeIzquierda.position = Vector3.Lerp(_posOpenIzq, _posIniIzq, t);
                NubeIzquierda1.position = Vector3.Lerp(_posOpenIzq1, _posIniIzq1, t);
                NubeIzquierda2.position = Vector3.Lerp(_posOpenIzq2, _posIniIzq2, t);
                NubeIzquierda3.position = Vector3.Lerp(_posOpenIzq3, _posIniIzq3, t);
                NubeIzquierda4.position = Vector3.Lerp(_posOpenIzq4, _posIniIzq4, t);

                NubeDerecha.position = Vector3.Lerp(_posOpenDer, _posIniDer, t);
                NubeDerecha1.position = Vector3.Lerp(_posOpenDer1, _posIniDer1, t);
                NubeDerecha2.position = Vector3.Lerp(_posOpenDer2, _posIniDer2, t);
                NubeDerecha3.position = Vector3.Lerp(_posOpenDer3, _posIniDer3, t);
                NubeDerecha4.position = Vector3.Lerp(_posOpenDer4, _posIniDer4, t);

            }

            if (t >= 1f)
                _animando = false;
        }
    }

    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos

    /// <summary>
    /// Muestra las nubes cerrándose. Se usa para ocultar contenido antes del cambio de escena.
    /// </summary>
    public void ShowClouds()
    {
        _tiempo = 0f;
        _aclarando = false;
        _animando = true;
        _audioSource.Play();
    }

    /// <summary>
    /// Oculta las nubes abriéndolas. Se usa al cargar una escena nueva.
    /// </summary>
    public void HideClouds()
    {
        _tiempo = 0f;
        _aclarando = true;
        _animando = true;
        _audioSource.Play();
    }

    #endregion
} // class Cloud