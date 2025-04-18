//---------------------------------------------------------
// Breve descripción del contenido del archivo
// Responsable de la creación de este archivo
// Nombre del juego
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using System;
using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class TutorialManager : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)

    /// <summary>
    /// Ref al GameManager
    /// </summary>
    [SerializeField] private GameManager GameManager;

    ///<summary>
    ///Ref al PlayerMovement
    /// </summary>
    [SerializeField] private PlayerMovement PlayerMovement;

    ///<summary>
    ///Ref al UIManager
    /// </summary>
    [SerializeField] private UIManager UIManager;
    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)

    /// <summary>
    /// Int para saber la fase del tutorial 
    /// </summary>
    private int _tutorialPhase = 0;

    /// <summary>
    /// Dialogo actual del tutorial
    /// </summary>
    private string _actualDialogueText;
    private string _actualDialogueButtonText;

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
        if(GameManager.GetCinematicState() == true)
        {
            UIManager.HideDialogue();
        }
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if (GameManager.GetCinematicState() == false && _tutorialPhase == 0)
        {
            //Activar el tutorial
            _tutorialPhase++;
            FindTutorialPhase();
            UIManager.ShowDialogue(_actualDialogueText, _actualDialogueButtonText);
        }
        
    }
    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    
    /// <summary>
    /// Metodo para saber si el boton ha sido pulsado
    /// </summary>
    /// <param name="buttonText"></param>
    public void OnTutorialButtonPressed(string buttonText)
    {
        Debug.Log("Boton:" + buttonText);
        if (buttonText == "Continuar")
        {
            NextDialogue();
            FindTutorialPhase();
            UIManager.ShowDialogue(_actualDialogueText, _actualDialogueButtonText);
        }
        else if (buttonText == "Probar" || buttonText == "Cerrar")
        {
            UIManager.HideDialogue(); // Asume que tienes un método para ocultarlo
            if (_tutorialPhase == 5)
            {
                UIManager.HideMap();
            }
        }
    }
    ///<summary>
    ///Metodo para obtener la fase del tutorial
    /// </summary>
    public int GetTutorialPhase()
    {
        return _tutorialPhase;
    }

    ///<summary>
    ///Metodo para avanzar en el dialogo
    /// </summary>
    public void NextDialogue()
    {
        _tutorialPhase++;
        FindTutorialPhase();
        UIManager.ShowDialogue(_actualDialogueText, _actualDialogueButtonText);
    }
    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados



    ///<summary>
    ///Metodo para asignar los tetxos dependiendo de la fase del tutorial
    /// </summary>
    private void FindTutorialPhase()
    {
        if (_tutorialPhase == 1)
        {
            _actualDialogueText = "Madame Moo: ¡Muuuy buenas, Connie! Soy Madame Moo, vaca de gafas y sabia consejera. Estoy aquí para enseñarte cómo convertirte en la mejor granjera de RootWood.\r\n¡Sigue mis consejos y estarás un paso más cerca de tu casa soñada y una vida de lujo y fertilizante premium!";
            _actualDialogueButtonText = "Continuar";
        }
        if (_tutorialPhase == 2)
        {
            _actualDialogueText = "Madame Moo: Para usar cosas, como palas o regaderas, solo tienes que pulsar la tecla E.\r\n¡Es como decirle a la vida: “¡Estoy lista para interactuar contigo!”";
            _actualDialogueButtonText = "Continuar";
        }
        if (_tutorialPhase == 3)
        {
            _actualDialogueText = "Madame Moo: ¿Te perdiste? ¿No sabes si estás en tu huerto o en casa de la vecina?\r\nPulsa la tecla M y abre el mapa. ¡Orientarte es clave para no acabar regando el gallinero!";
            _actualDialogueButtonText = "Probar";
        }
        if (_tutorialPhase == 4)
        {
            _actualDialogueText = "Madame Moo: ¡Muuhh! (Alegría), ya casi lo tienes. A lo largo del pueblo hay varias casas a tu disposición, y cada una sirve para algo distinto.\r\nPero de eso ya hablaremos en su momento."; 
            _actualDialogueButtonText = "Continuar";
        }
        if (_tutorialPhase == 5)
        {
            _actualDialogueText = "Madame Moo: También tienes varias herramientas para usar en tu huerto.\r\nUsa el selector de herramientas (1-5) para elegir lo que necesitas.\r\n¡Una vaca no puede arar con la lengua, ni tú cosechar sin azada!";
            _actualDialogueButtonText = "Probar";
        }
        if (_tutorialPhase == 6)
        {
            _actualDialogueText = "Madame Moo: Por último. Pulsa TAB para abrir tu inventario.\r\nAhí podrás ver todo lo que llevas encima: semillas, cultivos, y herramienta y quién sabe, ¡quizás algún queso viejo que olvidaste! (broma).";
            _actualDialogueButtonText = "Probar";
        }
        if (_tutorialPhase == 7)
        {
            _actualDialogueText = "Madame Moo: ¡Uy, Connie! Ese inventario está más vacío que una lechera en sequía...\r\n¡Vamos a llenarlo de cosas buenas antes de que los grillos monten una fiesta ahí dentro!\r\nVe a la tienda y compra unas cuantas semillas.\r\nSin semillas, no hay cosecha… ¡y sin cosecha, no hay cheddar! Y no hablo de queso precisamente.";
            _actualDialogueButtonText = "Continuar";
        }
        if (_tutorialPhase == 8)
        {   
            _actualDialogueText = "Madame Moo: Ve a la tienda y compra unas cuantas semillas.\r\nSin semillas, no hay cosecha… ¡y sin cosecha, no hay cheddar! Y no hablo de queso precisamente.";
            _actualDialogueButtonText = "Cerrar";
        }
        if (_tutorialPhase == 9)
        {
            _actualDialogueText = "Para continuar, necesitas comprar al menos unas poquitas semillas.\r\nNo seas tímida con el monedero, Connie. ¡La inversión inicial es el primer paso hacia tu casa soñada!";
            _actualDialogueButtonText = "Cerrar";
        }
        if (_tutorialPhase == 10)
        {
            _actualDialogueText = "¡De vuelta al huerto!\r\nSelecciona tus semillas desde el inventario, acércate a un surco y plántalas con cariño.\r\n¡No las tires como si fueran cacahuetes! Son el futuro.";
            _actualDialogueButtonText = "Cerrar";
        }
        if (_tutorialPhase == 11)
        {
            _actualDialogueText = "Después de plantar, toca regar.\r\nUsa la regadera para darles un buen bañito. Las plantas no crecen con cariño… ¡crecen con agua!";
            _actualDialogueButtonText = "Continuar";
        }
        if (_tutorialPhase == 12)
        {
            _actualDialogueText = "¿Se acabó el agua? ¡Normal, no eres una fuente ambulante!\r\nAcércate al pozo para rellenar tu regadera.\r\nRecuerda: una planta sedienta es una planta triste… ¡y una planta triste no se vende!";
            _actualDialogueButtonText = "Continuar";
        }
        if (_tutorialPhase == 13)
        {
            _actualDialogueText = "Cuando veas una planta bien crecidita entonces ¡Es hora de cosechar!\r\nSelecciona la HOZ y recógela, querida!";
            _actualDialogueButtonText = "Continuar";
        }
        if (_tutorialPhase == 14)
        {
            _actualDialogueText = "No todas las plantas prosperan, y eso está bien.\r\nSi ves una que no va a dar más de sí… arráncala con la PALA sin miedo.\r\nAsí haces espacio para nuevas oportunidades. ¡Como en la vida!";
            _actualDialogueButtonText = "Cerrar";
        }

    }
    
    
    #endregion

} // class TutorialManager 
// namespace
