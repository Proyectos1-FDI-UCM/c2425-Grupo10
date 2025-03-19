//---------------------------------------------------------
// Breve descripción del contenido del archivo
// Responsable de la creación de este archivo
// Nombre del juego
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class ChangeDeadSprite : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints


    // Sprites plantas muertas
    [SerializeField] private Sprite DeadCrop2;
    [SerializeField] private Sprite DeadCrop3;
    [SerializeField] private Sprite DeadCrop4;

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // privados se nombren en formato _camelCase (comienza con _, 
    // primera palabra en minúsculas y el resto con la 
    // primera letra en mayúsculas)
    // Ejemplo: _maxHealthPoints
    private SpriteRenderer _spriteRenderer;
  //  private GameObject gameObject;
    private int EstadoCrecimiento;
    private GameObject _maceta;
    private CropSpawner _cropSpawner;
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
          _maceta = gameObject.transform.parent.gameObject;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _cropSpawner = _maceta.GetComponent<CropSpawner>();
        if (_cropSpawner == null) Debug.Log("NO HAY CROPSPAWNER");
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        
    }
    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    // Documentar cada método que aparece aquí con ///<summary>
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)
    // Ejemplo: GetPlayerController

    public void ChangeSprite(int E)
    {
        switch (E)
        {
            case 1:
                _spriteRenderer.sprite = DeadCrop2;
                Debug.Log("Sprite cambiado.");
                break;
            case 2:
                _spriteRenderer.sprite = DeadCrop3;
                Debug.Log("Sprite cambiado.");
                break;
            case 3:
                _spriteRenderer.sprite = DeadCrop4;
                Debug.Log("Sprite cambiado.");
                break;
        }
    }
    public void TearDeadCrop()
    {
        _cropSpawner.Reactivar();
        //if (_cropSpawner != null) Debug.Log("!!!!!"); 
        Destroy(gameObject); // Elimina la planta muerta del mapa tras recogerla
    }

    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)

    /// <summary>
    /// Afrrancar planta muerta. Igual que Cosechar() pero sin guardar en el inventario.
    /// </summary>

    #endregion
    // ---- EVENTOS ----
    #region Eventos

    /// <summary>
    /// Detecta si el jugador interactúa con la planta.
    /// </summary>
    private void OnCollisionStay2D()
    {
        //Si la planta está muerta y el jugador presiona el botón teniendo la hoz (Herramienta 3), la arranca.

        if (InputManager.Instance.UsarIsPressed() && LevelManager.Instance.Herramientas() == 3)
        {
            TearDeadCrop();
        }
    }

    #endregion

} // class ChangeDeadSprite 
// namespace
