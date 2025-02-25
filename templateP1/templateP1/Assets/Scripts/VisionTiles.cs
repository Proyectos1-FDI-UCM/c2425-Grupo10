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
    public Visibility visibility;
    public int playersInside = 0;
    public Collider2D playerCollider;
    public bool isTransparent = false;
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

        InvokeRepeating(nameof(CheckPlayerOnTile), 0f, 0.05f); // Solo iniciar si no está en ejecución
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
    private void SetTilemapAlpha(float alpha)
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
            playerCollider = other; // Guardamos el colisionador del jugador

            if (!isTransparent)
            {
                isTransparent = true;
                SetTilemapAlpha(transparentAlpha);
                visibility.visibility(0.5f);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playersInside--;
            if (playersInside <= 0)
            {
                playersInside = 0; // Evita valores negativos

                // 🔹 Verificar si el jugador sigue tocando el Tilemap antes de restaurar opacidad
                if (!IsAnyPartOfPlayerOnTile(other))
                {
                    SetTilemapAlpha(1f);
                    visibility.visibility(1f);
                    isTransparent = false;
                }
            }
        }
    }

    private void CheckPlayerOnTile()
    {
        if (playerCollider == null || playersInside <= 0)
        {
            CancelInvoke(nameof(CheckPlayerOnTile));
            return;
        }

        if (!IsAnyPartOfPlayerOnTile(playerCollider))
        {
            SetTilemapAlpha(1f);
            visibility.visibility(1f);
            isTransparent = false;
            playerCollider = null;
            CancelInvoke(nameof(CheckPlayerOnTile)); // Detener el ciclo
        }
    }

    private bool IsAnyPartOfPlayerOnTile(Collider2D player)
    {
        if (player == null) return false;

        Bounds bounds = player.bounds;
        Vector3[] checkPoints = new Vector3[]
        {
            bounds.center, // Centro
            bounds.min, // Esquina inferior izquierda
            bounds.max, // Esquina superior derecha
            new Vector3(bounds.min.x, bounds.max.y, 0), // Esquina superior izquierda
            new Vector3(bounds.max.x, bounds.min.y, 0)  // Esquina inferior derecha
        };

        foreach (var point in checkPoints)
        {
            Vector3Int cellPosition = tilemap.WorldToCell(point);
            if (tilemap.HasTile(cellPosition))
            {
                return true;
            }
        }
        return false;
    }

    #endregion

} // class VisionTiles 
// namespace
