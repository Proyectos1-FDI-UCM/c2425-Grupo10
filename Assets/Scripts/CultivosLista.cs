//---------------------------------------------------------
// Archivo que gestiona la lista de cultivos en el juego.
// Responsable: Alexia Pérez Santana
// Juego: Roots of Life
// Proyecto 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
using System.Collections.Generic; // Necesario para listas

/// <summary>
/// Clase que representa la lista de cultivos disponibles en el juego.
/// Se encarga de almacenar los diferentes tipos de cultivos y su información,
/// como el nombre, el sprite y la cantidad disponible.
/// </summary>
[System.Serializable]
public class CultivosLista : MonoBehaviour
{
    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados

    /// <summary>
    /// Lista de cultivos disponibles en el juego.
    /// </summary>
    [SerializeField] private List<Cultivo> _cultivosDisponibles;

    /// <summary>
    /// Clase interna que representa un cultivo individual.
    /// Contiene el nombre del cultivo, su sprite y la cantidad en el inventario.
    /// </summary>
    [System.Serializable]
    private class Cultivo
    {
        [SerializeField] private string _nombre;    // Nombre del cultivo
        [SerializeField] private Sprite _sprite;   // Imagen del cultivo
        [SerializeField] private int _cantidad;    // Cantidad recolectada

        /// <summary>
        /// Constructor de un cultivo.
        /// </summary>
        /// <param name="nombre">Nombre del cultivo.</param>
        /// <param name="sprite">Imagen representativa del cultivo.</param>
        public Cultivo(string nombre, Sprite sprite)
        {
            _nombre = nombre;
            _sprite = sprite;
            _cantidad = 1;  // Al recolectar un cultivo, comienza con 1 unidad
        }

        /// <summary>
        /// Incrementa la cantidad de este cultivo en el inventario.
        /// </summary>
        public void IncrementarCantidad()
        {
            _cantidad++;
        }

        /// <summary>
        /// Reduce la cantidad del cultivo en el inventario.
        /// </summary>
        /// <param name="cantidad">Cantidad a reducir.</param>
        /// <returns>True si se pudo reducir, False si no hay suficientes unidades.</returns>
        public bool ReducirCantidad(int cantidad)
        {
            if (_cantidad >= cantidad)
            {
                _cantidad -= cantidad;
                return true; // Venta o uso exitoso
            }
            return false; // No hay suficientes cultivos
        }

        /// <summary>
        /// Obtiene el nombre del cultivo.
        /// </summary>
        public string GetNombre()
        {
            return _nombre;
        }

        /// <summary>
        /// Obtiene la cantidad disponible del cultivo.
        /// </summary>
        public int GetCantidad()
        {
            return _cantidad;
        }

        /// <summary>
        /// Obtiene el sprite del cultivo.
        /// </summary>
        public Sprite GetSprite()
        {
            return _sprite;
        }
    }

    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    /// <summary>
    /// Start se ejecuta antes de la primera actualización de fotogramas.
    /// Inicializa la lista de cultivos con los cultivos predefinidos.
    /// </summary>
    private void Start()
    {
        InicializarCultivos();
    }

    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos

    /// <summary>
    /// Devuelve una copia de la lista de cultivos disponibles.
    /// No se devuelve la lista original para evitar modificaciones externas.
    /// </summary>
    public List<(string, int, Sprite)> GetCultivosDisponibles()
    {
        List<(string, int, Sprite)> copiaLista = new List<(string, int, Sprite)>();
        foreach (Cultivo cultivo in _cultivosDisponibles)
        {
            copiaLista.Add((cultivo.GetNombre(), cultivo.GetCantidad(), cultivo.GetSprite()));
        }
        return copiaLista;
    }

    /// <summary>
    /// Busca un cultivo en la lista de cultivos disponibles.
    /// </summary>
    /// <param name="nombreCultivo">Nombre del cultivo a buscar.</param>
    /// <returns>Objeto Cultivo si existe, null si no se encuentra.</returns>
    private Cultivo BuscarCultivo(string nombreCultivo)
    {
        foreach (Cultivo cultivo in _cultivosDisponibles)
        {
            if (cultivo.GetNombre() == nombreCultivo)
            {
                return cultivo;
            }
        }
        return null;
    }

    /// <summary>
    /// Añade un cultivo al inventario. Si ya existe, aumenta su cantidad.
    /// </summary>
    /// <param name="nombreCultivo">Nombre del cultivo a añadir.</param>
    public void AgregarCultivo(string nombreCultivo)
    {
        Cultivo cultivo = BuscarCultivo(nombreCultivo);
        if (cultivo != null)
        {
            cultivo.IncrementarCantidad();
        }
    }

    /// <summary>
    /// Reduce la cantidad de un cultivo en el inventario.
    /// </summary>
    /// <param name="nombreCultivo">Nombre del cultivo.</param>
    /// <param name="cantidad">Cantidad a reducir.</param>
    /// <returns>True si se pudo vender, False si no hay suficientes unidades.</returns>
    public bool VenderCultivo(string nombreCultivo, int cantidad)
    {
        Cultivo cultivo = BuscarCultivo(nombreCultivo);
        if (cultivo != null)
        {
            return cultivo.ReducirCantidad(cantidad);
        }
        return false;
    }

    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados

    /// <summary>
    /// Inicializa la lista de cultivos con los cultivos predeterminados del juego.
    /// </summary>
    private void InicializarCultivos()
    {
        _cultivosDisponibles = new List<Cultivo>
        {
            new Cultivo("Zanahoria", null), // Se pueden asignar sprites desde el Inspector
            new Cultivo("Maíz", null),
            new Cultivo("Fresa", null),
            new Cultivo("Lechuga", null)
        };
    }

    #endregion   
} // class CultivosLista
