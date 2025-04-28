//---------------------------------------------------------
// Breve descripción del contenido del archivo
// Responsable de la creación de este archivo
// Nombre del juego
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Clase encargada de gestionar la visualización del mapa del juego y la posición del jugador sobre el mismo.
/// </summary>
public class MapManager : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)

    [SerializeField] private Transform playerMarker; // El marcador que representa al jugador en el mapa

    [SerializeField] private Transform player; // El transform del jugador (para obtener la posición)

    [SerializeField] private Transform Text;


    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Aquí puedes agregar cualquier atributo privado si lo necesitas, en este caso no se necesitan más atributos.
    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    /// <summary>
    /// Start is called before the first frame update.
    /// Este método se ejecuta una vez al inicio.
    /// </summary>
    void Start()
    {
        // Aquí podrías realizar inicializaciones si fuera necesario.
    }

    /// <summary>
    /// Update is called once per frame.
    /// Este método se ejecuta cada fotograma, y es donde actualizamos la posición del marcador.
    /// </summary>
    void Update()
    {
        UpdatePlayerMarkerPosition();
    }

    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    // Si necesitas que este método sea accesible desde otras clases, puedes hacerla pública.
    // En este caso, todos los métodos que necesitamos son privados.
    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados

    /// <summary>
    /// Actualiza la posición del marcador del jugador en el mapa según su posición en el mundo.
    /// </summary>
    private void UpdatePlayerMarkerPosition()
    {
        playerMarker.position = player.position;
    }

    #endregion
}
