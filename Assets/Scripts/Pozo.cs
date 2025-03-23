//---------------------------------------------------------
// Maneja la funcionalidad del pozo en el juego.
// Responsable de la creación de este archivo
// Roots of Life
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Clase que representa un pozo, permitiendo la recolección de agua
/// y la interacción con la regadera.
/// </summary>
public class WateringHole : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    /// <summary>
    /// Máxima capacidad de agua que puede contener la regadera.
    /// </summary>
    [SerializeField] int MaxWaterCapacity;

    /// <summary>
    /// Máxima capacidad de agua que puede contener la regadera.
    /// </summary>
    [SerializeField] GameObject IrrigationPrefab;

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    /// <summary>
    /// Referencia al aviso de riego que se mostrará en pantalla.
    /// </summary>
    private GameObject _warning;

    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    // Por defecto están los típicos (Update y Start) pero:
    // - Hay que añadir todos los que sean necesarios
    // - Hay que borrar los que no se usen 

    /// <summary>
    /// Se llama al comenzar el juego. Inicializa el pozo.
    /// </summary>
    void Start()
    {
     //   MaxWaterCapacity = LevelManager.Instance.GetMaxWater();
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
    /// <summary>
    /// Método para destruir el aviso de riego.
    /// </summary>
    public void DestroyWarning()
    {
        Debug.Log("Destroying warning");
        Destroy(_warning);
    }

    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Método comentado que puede ser implementado más adelante.
    /// <summary>
    /// Lógica para llenar la regadera
    /// </summary>

    // private void OnCollisionStay2D()
    // {

    /// Añadir animación llenar regadera
    // if ((InputManager.Instance.UseWasPressedThisFrame() || InputManager.Instance.UseIsPressed()) && LevelManager.Instance.Tools() == 2 && LevelManager.Instance.WateringCan() < MaxWaterCapacity) 
    //  {

    // LevelManager.Instance.FillWateringCan(MaxWaterCapacity); /// Llama al método que controla la cantidad de agua de la regadera

    /// Animación Provisional
    //  Vector3 position = new Vector3 (4.23f, 5.82f, 0f);
    //  _warning = Instantiate(IrrigationPrefab, position, Quaternion.identity);

    //   Invoke("DestroyWarning", 1f);

    //   }

    // }

    #endregion

} // class WateringHole 
// namespace
