//---------------------------------------------------------
// Breve descripción del contenido del archivo
// Responsable: Natalia Nita
// Nombre del juego: Roots of Life
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
using TMPro;

/// <summary>
/// Clase encargada de gestionar la selección de productos para vender,
/// actualizar la cantidad de productos y su precio, y mostrar la 
/// descripción correspondiente en la interfaz de usuario.
/// </summary>
public class VenderProducto : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    [SerializeField] private TextMeshProUGUI descripcionTexto; // Texto para mostrar la descripción del producto
    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    private int cantidad = 1; // Cantidad inicial de productos
    private int precioPorProducto = 0; // Precio del producto seleccionado
    private string nombreProducto = ""; // Nombre del producto seleccionado
    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    // Documentar cada método que aparece aquí.

    /// <summary>
    /// Cambia el producto seleccionado a Maíz y actualiza la interfaz.
    /// </summary>
    public void SeleccionarMaiz()
    {
        cantidad = 1; // Resetea la cantidad a 1
        nombreProducto = "Maíz"; // Cambia el nombre del producto
        precioPorProducto = 90; // Cambia el precio del Maíz
        ActualizarDescripcion(); // Actualiza la descripción con el nuevo producto y precio
    }

    /// <summary>
    /// Cambia el producto seleccionado a Fresas y actualiza la interfaz.
    /// </summary>
    public void SeleccionarFresas()
    {
        cantidad = 1; // Resetea la cantidad a 1
        nombreProducto = "Fresas"; // Cambia el nombre del producto
        precioPorProducto = 40; // Cambia el precio de las Fresas
        ActualizarDescripcion(); // Actualiza la descripción con el nuevo producto y precio
    }

    /// <summary>
    /// Cambia el producto seleccionado a Zanahorias y actualiza la interfaz.
    /// </summary>
    public void SeleccionarZanahorias()
    {
        cantidad = 1; // Resetea la cantidad a 1
        nombreProducto = "Zanahorias"; // Cambia el nombre del producto
        precioPorProducto = 65; // Cambia el precio de las Zanahorias
        ActualizarDescripcion(); // Actualiza la descripción con el nuevo producto y precio
    }

    /// <summary>
    /// Cambia el producto seleccionado a Lechuga y actualiza la interfaz.
    /// </summary>
    public void SeleccionarLechuga()
    {
        cantidad = 1; // Resetea la cantidad a 1
        nombreProducto = "Lechuga"; // Cambia el nombre del producto
        precioPorProducto = 20; // Cambia el precio de las Lechugas
        ActualizarDescripcion(); // Actualiza la descripción con el nuevo producto y precio
    }

    /// <summary>
    /// Incrementa la cantidad del producto y actualiza la descripción.
    /// </summary>
    public void AumentarCantidad()
    {
        if (cantidad > 0) // Asegura que la cantidad es mayor que 0
        {
            cantidad++; // Incrementa la cantidad
            ActualizarDescripcion(); // Actualiza la descripción
        }
    }

    /// <summary>
    /// Disminuye la cantidad del producto y actualiza la descripción.
    /// </summary>
    public void DisminuirCantidad()
    {
        if (cantidad > 1)  // Asegura que la cantidad no sea menor que 1
        {
            cantidad--; // Disminuye la cantidad
            ActualizarDescripcion(); // Actualiza la descripción
        }
    }
    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados

    /// <summary>
    /// Actualiza la descripción del producto en la interfaz de usuario,
    /// mostrando la cantidad y el precio total del producto.
    /// </summary>
    private void ActualizarDescripcion()
    {
        if (!string.IsNullOrEmpty(nombreProducto) && precioPorProducto > 0)
        {
            // Muestra la cantidad y el precio total del producto en la interfaz
            descripcionTexto.text = $"{cantidad} {nombreProducto}(s) = {cantidad * precioPorProducto} rootcoins";
        }
    }
    #endregion
} // class VenderProducto
