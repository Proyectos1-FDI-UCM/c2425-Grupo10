//---------------------------------------------------------
// Archivo para Instanciar el Canva en el GameManager
// Javier Librada
// Nombre del juego
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Instancia el Canvas en el GameManager
/// </summary>
public class Canvas : MonoBehaviour
{
    
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
        GameManager.Instance.SetCanvas(this.gameObject);
    }
    #endregion  

} // class Canvas 
// namespace
