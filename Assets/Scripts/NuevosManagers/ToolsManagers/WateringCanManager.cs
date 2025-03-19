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
    /// Cantidad Actual de Agua en la regadera
    /// </summary>
    [SerializeField] int WaterAmount = 6;

    ///<summary>
    ///CAntidad maxima de Agua en la regadera
    /// </summary>
    [SerializeField] int MaxWaterAmount;

    ///<summary>
    ///Referencia al collider del jugador 
    /// </summary>
    //[SerializeField] private Collider2D PlayerCollider;

    ///<summary>
    ///Referencia al collider del pozo 
    /// </summary>
    //[SerializeField] private Collider2D WellCollider;

    ///<summary>
    ///Referencia al collider de la planta 
    /// </summary>
   //[SerializeField] private Collider2D PlantCollider;

    ///<summary> 
    ///Referencia al GameManager
    /// </summary>
    [SerializeField] GameManager GameManager;

    ///<summary> 
    ///Referencia al SelectorManager
    /// </summary>
    [SerializeField] PlayerMovement PlayerMovement;
    ///<summary> 
    /// referencia al animator del player
    /// </summary>
    [SerializeField] private Animator PlayerAnimator;

    ///<summary> 
    ///Referencia al SelectorManager
    /// </summary>
    [SerializeField] SelectorManager SelectorManager;

    ///<summary>
    ///PrefabAvisoRiego
    /// </summary>
    [SerializeField] GameObject PrefabWatering;

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
    ///GameObject donde instanciar el Prefab de aviso
    /// </summary>
    private GameObject _warning;
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
        GetUpgradeWateringCan();
        SelectorManager.UpdateWaterBar(WaterAmount, MaxWaterAmount);
        WaterAmount = GameManager.Instance.LastWaterAmount();
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        GetUpgradeWateringCan();
        SelectorManager.UpdateWaterBar(WaterAmount, MaxWaterAmount);
        WaterAmount = GameManager.Instance.LastWaterAmount();

        if(InputManager.Instance.UseWateringCanWasPressedThisFrame())
        {
            Watering();
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
        return WaterAmount;
    }

    /// <summary>
    /// Metodo para obtener la cantidad maxima que puede tener la regadera
    /// </summary>
    /// <returns></returns>
    public int GetMaxWaterAmount()
    {
        return MaxWaterAmount;
    }

    /// <summary>
    /// Metodo para llenar la regadera al colisionar con el pozo
    /// </summary>
    /// <param name="i"></param>
    public void FillWateringCan(int i)
    {
        Debug.Log("Recargando");
        PlayerMovement.enablemovement = false;
        WaterAmount = i;
        SelectorManager.UpdateWaterBar(WaterAmount, MaxWaterAmount);
        GameManager.Instance.UpdateWaterAmount();
        PlayerMovement.enablemovement = true;

    }
    /// <summary>
    /// Metodo para regar, resta 1 a la cantidad actual de agua y actualiza la barra
    /// </summary>
    public void Watering()
    {
        if (WaterAmount > 0)
        {
            PlayerAnimator.SetBool("Watering", true);
            Invoke("NotWatering", 1f);

            PlayerMovement.enablemovement = false;
            WaterAmount -= 1; ;
            SelectorManager.UpdateWaterBar(WaterAmount, MaxWaterAmount);
            GameManager.Instance.UpdateWaterAmount();

            this.gameObject.SetActive(false);
        }

    }
    /// <summary>
    /// Método para modificar el máximo agua de la regadera despues de las mejoras
    /// <summary>
    public void UpgradeWateringCan(int upgrade)
    {
        if (upgrade == 0)
        {
            MaxWaterAmount = 6;
        }
        else if (upgrade == 1)
        {
            MaxWaterAmount = 9;
        }
        else if (upgrade == 2)
        {
            MaxWaterAmount = 15;
        }
        else if (upgrade == 3)
        {
            MaxWaterAmount = 20;
        }

    }

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
        if ((GameManager.GetMejorasRegadera() == 0))
        {
            UpgradeWateringCan(0);
        }
        else if ((GameManager.GetMejorasRegadera() == 1))
        {
            UpgradeWateringCan(1);
        }
        else if ((GameManager.GetMejorasRegadera() == 2))
        {
            UpgradeWateringCan(2);
        }
        else if ((GameManager.GetMejorasRegadera() == 3))
        {
            UpgradeWateringCan(3);
        }
    }

    private void NotWatering()
    {
        PlayerAnimator.SetBool("Watering", false);
        this.gameObject.SetActive(true);
        PlayerMovement.enablemovement = true;

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Pozo") && InputManager.Instance.UseWateringCanWasPressedThisFrame()) // Asegúrate de que el pozo tenga el tag "Pozo"
        {
            Debug.Log("colision con pozo");
            FillWateringCan(MaxWaterAmount);
        }
    }
    #endregion

} // class WateringCanManager 
// namespace
