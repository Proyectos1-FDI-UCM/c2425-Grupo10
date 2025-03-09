//---------------------------------------------------------
// Controlador de la venta de productos
// Responsable: Natalia Nita
// Nombre del juego: Roots of Life
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Gestiona la interfaz de venta, cambiando la descripción
/// según el producto seleccionado.
/// </summary>
public class VentaUI : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector

    [SerializeField] private TextMeshProUGUI textoDescripcionVenta; // Texto encima del botón de vender

    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos Públicos

    /// <summary>
    /// Método que se llama cuando el jugador selecciona un producto.
    /// Cambia la descripción de la venta.
    /// </summary>
    public void SeleccionarProducto(string nombreProducto, int precio)
    {
        textoDescripcionVenta.text = $"Vender {nombreProducto} por {precio} RC";
    }

    public void SeleccionarFresas()
    {
        SeleccionarProducto("Fresas", 40);
    }

    public void SeleccionarZanahorias()
    {
        SeleccionarProducto("Zanahorias", 65);
    }

    #endregion
}
