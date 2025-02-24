//---------------------------------------------------------
// Breve descripción del contenido del archivo
// Responsable de la creación de este archivo
// Nombre del juego
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
using UnityEngine.Tilemaps;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class VisionTiles : MonoBehaviour
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
    public Tilemap tilemap;
    public float transparentAlpha = 0.5f;
    private int playersInside = 0;
    private bool isPlayerOverTile = false;
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
        if (tilemap == null)
            tilemap = GetComponent<Tilemap>();
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
    public void SetTilemapAlpha(float alpha)
    {
        if (tilemap != null)
        {
            Color color = tilemap.color;
            color.a = alpha;
            tilemap.color = color;
        }
    }
    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playersInside++;
            isPlayerOverTile = true; // El jugador está sobre los tiles

            // Si es el primer jugador en entrar, hacemos transparente el Tilemap
            if (playersInside == 1)
            {
                SetTilemapAlpha(transparentAlpha);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playersInside--;

            // Verificamos si el jugador sigue en los tiles
            isPlayerOverTile = CheckIfPlayerIsOnTile(other.transform.position);

            if (playersInside <= 0 && !isPlayerOverTile)
            {
                playersInside = 0; // Asegurar que no sea negativo
                SetTilemapAlpha(1f); // Volver a ser opaco solo si el jugador realmente salió de los tiles
            }
        }
    }
    private bool CheckIfPlayerIsOnTile(Vector3 playerPosition)
    {
        Vector3Int cellPosition = tilemap.WorldToCell(playerPosition);
        return tilemap.HasTile(cellPosition);
    }

    #endregion

} // class VisionTiles 
// namespace
