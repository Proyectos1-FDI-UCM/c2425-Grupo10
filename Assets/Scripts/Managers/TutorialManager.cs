//---------------------------------------------------------
// Breve descripción del contenido del archivo
// Responsable de la creación de este archivo
// Nombre del juego
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using System;
using UnityEngine;
using UnityEngine.SceneManagement;
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

    ///<summary>
    ///Ref al soundManager
    /// </summary>
    [SerializeField] private SoundManager SoundManager;
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

    /// <summary>
    /// Booeano para saber si la notificacion esta activa
    /// </summary>
    private bool _isNotificationActive = false;

    /// <summary>
    /// Booeano para saber si el tutorial esta activo
    /// </summary>
    private bool _isDialogueActive = false;

    ///<summary>
    ///Notificacion activada
    /// </summary>
    private int _notificationActive = 0;

    ///<summary>
    ///Texto de madame moo con sus colores
    /// </summary>
    private string MadameMooColor = "<color=white>Madame Moo:</color>";

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
            //UIManager.HideDialogue();
        }
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if (SceneManager.GetActiveScene().name != "Menu")
        {
            InitializeReferences();
            if (GameManager.GetCinematicState() == false && _tutorialPhase == 0)
            {
                //Activar el tutorial
                _tutorialPhase++;
                FindTutorialPhase();
                UIManager.ShowDialogue(_actualDialogueText, _actualDialogueButtonText);
                SoundManager.MadameMooSound();
            }
        }
        if (SoundManager == null)
        {
            SoundManager = FindObjectOfType<SoundManager>();
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
            UIManager.HideDialogueButton();
            UIManager.ShowDialogue(_actualDialogueText, _actualDialogueButtonText);
        }
        else if (buttonText == "Probar" || buttonText == "Cerrar")
        {
            UIManager.HideDialogue(); // Asume que tienes un método para ocultarlo
            if (_tutorialPhase == 5)
            {
                UIManager.HideMap();
                UIManager.ShowNotification("Selecciona todas \nlas herramientas", "[ ] Regadera\r\n[ ] Hoz\r\n[ ] Pala\r\n[ ] Semillas", 2, "Tutorial");
                _isNotificationActive = true;
                _notificationActive = UIManager.GetAvailableNotification();
            }
            if (_tutorialPhase == 6)
            {
                UIManager.ShowNotification("Abre el inventario\nPulsa TAB", "[ ] Inventario", 2, "Tutorial");
                _isNotificationActive = true;
                _notificationActive = UIManager.GetAvailableNotification();

            }
            if (_tutorialPhase == 3)
            {
                UIManager.ShowNotification("Abre el mapa\nPulsa M", "[ ] Mapa", 2, "Tutorial");
                _isNotificationActive = true;
                _notificationActive = UIManager.GetAvailableNotification();

            }
            if (_tutorialPhase == 8)
            {
                if(UIManager.GetInventoryVisible() == true)
                {
                    UIManager.ToggleInventory();
                }
                
                UIManager.ShowNotification("Ve a la casa \nde compra", "[ ] Compra una\r\n    semilla de\r\n    lechuga", 2, "Tutorial");
                _isNotificationActive = true;
                _notificationActive = UIManager.GetAvailableNotification();
            }
            if (_tutorialPhase == 15)
            {
                UIManager.ShowNotification("Aprende a \ncuidar tu huerto", "[ ] Regadera\r\n[ ] Cosechar\r\n[ ] Muerte\r\n[ ] Mala hierba", 2, "Tutorial");
                _isNotificationActive = true;
                _notificationActive = UIManager.GetAvailableNotification();
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
        SoundManager.MadameMooSound();
        if (_tutorialPhase == 6 ||_tutorialPhase == 8 || _tutorialPhase == 5 || _tutorialPhase == 3)
        {
            UIManager.HideNotification("Tutorial");
            SoundManager.NextButtonSound();
            _isNotificationActive = false;
        }
    }

    ///<summary>
    ///Metodo para mostrar el dialogo actual(pulsando notificicacion)
    /// </summary>
    public void ActualDialogue()
    {
        UIManager.ShowDialogue(_actualDialogueText, _actualDialogueButtonText);
        SoundManager.MadameMooSound();
    }

    ///<summary>
    ///Metodo para marcar la tarea como hecha
    /// </summary>
    public void CheckBox(int checkbox)
    {
        UIManager.Check(checkbox);
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
            _actualDialogueText = MadameMooColor + " ¡Muuuy buenas, Connie! Soy Madame Moo, vaca de gafas y sabia consejera. Estoy aquí para enseñarte cómo convertirte en la mejor granjera de RootWood.\r\n¡Sigue mis consejos y estarás un paso más cerca de tu casa soñada y una vida de lujo y fertilizante premium!";
            _actualDialogueButtonText = "Continuar";
        }
        if (_tutorialPhase == 2)
        {
            _actualDialogueText = MadameMooColor + " Para usar cosas, como palas o regaderas, solo tienes que pulsar la tecla E.\r\n¡Es como decirle a la vida: “¡Estoy lista para interactuar contigo!”";
            _actualDialogueButtonText = "Continuar";
        }
        if (_tutorialPhase == 3)
        {
            _actualDialogueText = MadameMooColor + " ¿Te perdiste? ¿No sabes si estás en tu huerto o en casa de la vecina?\r\nPulsa la tecla M y abre el mapa. ¡Orientarte es clave para no acabar regando el gallinero!";
            _actualDialogueButtonText = "Probar";
        }
        if (_tutorialPhase == 4)
        {
            _actualDialogueText = MadameMooColor + " ¡Muuhh! (Alegría), ya casi lo tienes. A lo largo del pueblo hay varias casas a tu disposición, y cada una sirve para algo distinto.\r\nPero de eso ya hablaremos en su momento."; 
            _actualDialogueButtonText = "Continuar";
        }
        if (_tutorialPhase == 5)
        {
            _actualDialogueText = MadameMooColor + " También tienes varias herramientas para usar en tu huerto.\r\nUsa el selector de herramientas (1-5) para elegir lo que necesitas.\r\n¡Una vaca no puede arar con la lengua, ni tú cosechar sin azada!";
            _actualDialogueButtonText = "Probar";
        }
        if (_tutorialPhase == 6)
        {
            _actualDialogueText = MadameMooColor + " Por último. Pulsa TAB para abrir tu inventario.\r\nAhí podrás ver todo lo que llevas encima: semillas, cultivos, y herramienta y quién sabe, ¡quizás algún queso viejo que olvidaste! (broma).";
            _actualDialogueButtonText = "Probar";
        }
        if (_tutorialPhase == 7)
        {
            _actualDialogueText = MadameMooColor + " ¡Uy, Connie! Ese inventario está más vacío que una lechera en sequía...\r\n¡Vamos a llenarlo de cosas buenas antes de que los grillos monten una fiesta ahí dentro!";
            _actualDialogueButtonText = "Continuar";
        }
        if (_tutorialPhase == 8)
        {   
            _actualDialogueText = MadameMooColor + " Ve a la tienda y compra unas cuantas semillas.\r\nSin semillas, no hay cosecha… ¡y sin cosecha, no hay cheddar! Y no hablo de queso precisamente.";
            _actualDialogueButtonText = "Cerrar";
        }
        if (_tutorialPhase == 9)
        {
            _actualDialogueText = MadameMooColor + " Para continuar, necesitas comprar al menos unas poquitas semillas.\r\nNo seas tímida con el monedero, Connie. ¡La inversión inicial es el primer paso hacia tu casa soñada!";
            _actualDialogueButtonText = "Cerrar";
        }
        if (_tutorialPhase == 10)
        {
            _actualDialogueText = MadameMooColor + " ¡De vuelta al huerto!\r\nSelecciona tus semillas desde el inventario, acércate a un surco y plántalas con cariño.\r\n¡No las tires como si fueran cacahuetes! Son el futuro.";
            _actualDialogueButtonText = "Cerrar";
        }
        if (_tutorialPhase == 11)
        {
            _actualDialogueText = MadameMooColor + " Después de plantar, toca regar.\r\nUsa la regadera para darles un buen bañito. Las plantas no crecen con cariño… ¡crecen con agua!";
            _actualDialogueButtonText = "Continuar";
        }
        if (_tutorialPhase == 12)
        {
            _actualDialogueText = MadameMooColor + " ¿Se acabó el agua? ¡Normal, no eres una fuente ambulante!\r\nAcércate al pozo para rellenar tu regadera.\r\nRecuerda: una planta sedienta es una planta triste… ¡y una planta triste no se vende!";
            _actualDialogueButtonText = "Continuar";
        }
        if (_tutorialPhase == 13)
        {
            _actualDialogueText = MadameMooColor + " Cuando veas una planta bien crecidita entonces ¡Es hora de cosechar!\r\nSelecciona la HOZ y recógela, querida!";
            _actualDialogueButtonText = "Continuar";
        }
        if (_tutorialPhase == 14)
        {
            _actualDialogueText = MadameMooColor + " No todas las plantas prosperan, y eso está bien.\r\nSi ves una que no va a dar más de sí… arráncala con la PALA sin miedo.\r\nAsí haces espacio para nuevas oportunidades. ¡Como en la vida!";
            _actualDialogueButtonText = "Continuar";
        }
        if (_tutorialPhase == 15)
        {
            _actualDialogueText = MadameMooColor + " Las malas hierbas aparecen solas y molestan más que un gallo insomne.\n Arráncalas!";
            _actualDialogueButtonText = "Cerrar";
        }

        if (_tutorialPhase == 16)
        {
            _actualDialogueText = MadameMooColor + " ¡Es hora de hacer negocios!\n Dirígete al puesto de ventas con tus cultivos recién cosechados.\n Nada dice \"éxito\" como vender tu primer nabo, quiero decir… cultivo";
            _actualDialogueButtonText = "Cerrar";
        }

        if (_tutorialPhase == 17)
        {
            _actualDialogueText = MadameMooColor + " Cada moneda que ganes es un paso más cerca de mudarte a tu casa soñada.\n ¡Connie, estás empezando a florecer! Volveré cuando me necesites, mientras tanto asegúrate de visitar todo el pueblo.";
            _actualDialogueButtonText = "Continuar";
        }

        if (_tutorialPhase == 18)
        {
            _actualDialogueText = MadameMooColor + " ¡Así que eso es todo por ahora, constelación de estiércol! Recuerda: la vida es como un cultivo... si no la riegas, se te va al pasto. ¡Muuucha suerte ahí fuera!";
            _actualDialogueButtonText = "Cerrar";
        }



    }

    ///<summary>
    ///Metodo para asignar las referencias
    /// </summary>
    private void InitializeReferences()
    {
        PlayerMovement = FindObjectOfType<PlayerMovement>();
        UIManager = FindObjectOfType<UIManager>();
    }
    #endregion

} // class TutorialManager 
// namespace
