//---------------------------------------------------------
// Gestión del sistema de tutoriales del juego
// Javier Librada, Julia Vera y Alexia Pérez
// Roots of Life
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using System;
using UnityEngine;
using UnityEngine.SceneManagement;
// Añadir aquí el resto de directivas using


/// <summary>
/// TutorialManager se encarga de gestionar el sistema de tutoriales del juego.
/// Controla las fases del tutorial principal y los tutoriales específicos de cada escena,
/// mostrando diálogos guiados, notificaciones y tareas para que el jugador aprenda
/// los controles y mecánicas del juego de forma progresiva.
/// </summary>
public class TutorialManager : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)

    ///<summary>
    ///Ref al PlayerMovement
    /// </summary>
    private PlayerMovement PlayerMovement;

    ///<summary>
    ///Ref al UIManager
    /// </summary>
    private UIManager UIManager;

    ///<summary>
    ///Ref al soundManager
    /// </summary>
    private SoundManager SoundManager;

    ///<summary>
    ///ref al notification manager
    /// </summary>
    private NotificationManager NotificationManager;

    /// <summary>
    /// Controla la fase actual del tutorial principal.
    /// </summary>
    private int _tutorialPhase = 0;

    /// <summary>
    /// Controla la fase del tutorial específico de las escenas de compra y venta.
    /// </summary>
    private int _tutorialPhaseEscenas = 0;

    /// <summary>
    /// Controla la fase del tutorial específico de la escena de mejora.
    /// </summary>
    private int _tutorialPhaseMejora = 0;

    /// <summary>
    /// Indica si ya se ha iniciado el tutorial de mejoras.
    /// </summary>
    private bool tutorialMejora = false;

    /// <summary>
    /// Controla la fase del tutorial específico de la escena del banco.
    /// </summary>
    private int _tutorialPhaseBanco = 0;

    /// <summary>
    /// Indica si ya se ha iniciado el tutorial del banco.
    /// </summary>
    private bool tutorialBanco = false;

    /// <summary>
    /// Indica si hay un tutorial específico de escena en progreso.
    /// </summary>
    private bool _tutorialInProgress = false;

    /// <summary>
    /// Dialogo actual del tutorial
    /// </summary>
    private string _actualDialogueText;

    /// <summary>
    /// Texto del botón del diálogo actual (Continuar, Probar, Cerrar, etc.).
    /// </summary>
    private string _actualDialogueButtonText;

    /// <summary>
    /// Dialogo Actual de las notificaciones
    /// </summary>
    private string _actualNotificationText;

    /// <summary>
    /// Texto de las tareas que aparecen en la notificación actual.
    /// </summary>
    private string _actualNotificationTaskText;

    /// <summary>
    /// Indica si hay una notificación de tutorial activa.
    /// </summary>
    private bool _isNotificationActive = false;

    /// <summary>
    /// Indica si hay un diálogo de tutorial activo.
    /// </summary>
    private bool _isDialogueActive = false;

    /// <summary>
    /// ID de la notificación activada actualmente.
    /// </summary>
    private int _notificationActive = 0;

    /// <summary>
    /// Etiqueta de color para el texto de Madame Moo en los diálogos.
    /// </summary>
    private string MadameMooColor = "<color=white>Madame Moo:</color>";

    /// <summary>
    /// Total de subtareas que debe completar el jugador en la tarea actual.
    /// </summary>
    private int _toTaskCompleted = 0;

    /// <summary>
    /// Número de tareas que ya ha completado el jugador.
    /// </summary>
    private int _taskDone = 0;

    /// <summary>
    /// Indica si el jugador está en el tutorial principal.
    /// </summary>
    private bool _isInMainTutorial = false;

    /// <summary>
    /// Texto que muestra la tecla de interacción según el dispositivo (teclado/controlador).
    /// </summary>
    private string _use;

    /// <summary>
    /// Texto que muestra la tecla del mapa según el dispositivo (teclado/controlador).
    /// </summary>
    private string _map;

    /// <summary>
    /// Texto que muestra la tecla del inventario según el dispositivo (teclado/controlador).
    /// </summary>
    private string _inventory;

    /// <summary>
    /// Texto que muestra las teclas de selección de herramientas según el dispositivo.
    /// </summary>
    private string _toolSelector;

    /// <summary>
    /// Texto que muestra la tecla de selección de semillas según el dispositivo.
    /// </summary>
    private string _seedSelector;

    /// <summary>
    /// Texto que muestra la tecla para salir según el dispositivo (teclado/controlador).
    /// </summary>
    private string _exit;

    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    // Por defecto están los típicos (Update y Start) pero:
    // - Hay que añadir todos los que sean necesarios
    // - Hay que borrar los que no se usen 

    /// <summary>
    /// Se ejecuta al iniciar el script. Obtiene una referencia al NotificationManager.
    /// </summary>
    void Start()
    {
        NotificationManager = GetComponent<NotificationManager>();

        
    }

    /// <summary>
    /// Se ejecuta cada frame. Actualiza las teclas según el dispositivo, controla
    /// el inicio de los tutoriales específicos de cada escena y verifica la
    /// compleción de tareas.
    /// </summary>
    void Update()
    {
        // Actualizar textos de teclas según si se usa mando o teclado
        if (GameManager.Instance.GetControllerUsing())
        {
            _use = "Cuadrado/X";
            _map = "Panel";
            _inventory = "Flecha Arriba";
            _toolSelector = "(R1/L1)";
            _seedSelector = "Triangulo/Y";
            _exit = "Círculo/B";
        }
        else
        {
            _use = "E";
            _map = "M";
            _inventory = "TAB";
            _toolSelector = "(1-5)";
            _seedSelector = "5";
            _exit = "Q";
        }

        // Inicializar tutorial principal cuando termina la cinemática
        if (SceneManager.GetActiveScene().name != "Menu" || SceneManager.GetActiveScene().name != "Menu_Pausa")
        {
            InitializeReferences();
            if (GameManager.Instance.GetCinematicState() == false && _tutorialPhase == 0)
            {
                //Activar el tutorial
                _tutorialPhase++;
                FindTutorialPhase();
                UIManager.ShowDialogue(_actualDialogueText, _actualDialogueButtonText);
                SoundManager.MadameMooSound();
            }
        }

        // Buscar SoundManager si aún no se ha encontrado
        if (SoundManager == null)
        {
            SoundManager = FindObjectOfType<SoundManager>();
        }

        //if (InventoryManager.GetInventory(Items.Letuce) >= 1 && _tutorialPhase == 8) 
        //if (SceneManager.GetActiveScene().name == "Escena_Build" && _tutorialPhase == 9) 
        //{
        //    //NextDialogue();
        //}

        // Iniciar tutoriales específicos según la escena actual
        if (SceneManager.GetActiveScene().name == "Escena_Compra" && _tutorialPhaseEscenas == 0)
        {
            UIManager.HideNotification("NoTutorial");
            _tutorialInProgress = true;
            NextDialogue();
        }
        if (SceneManager.GetActiveScene().name == "Escena_Venta" && _tutorialPhaseEscenas == 3)
        {
            UIManager.HideNotification("NoTutorial");
            _tutorialInProgress = true;
            //_tutorialPhaseEscenas = 3;
            NextDialogue();
        }
        if (SceneManager.GetActiveScene().name == "Escena_Mejora" && _tutorialPhaseMejora == 0 && !tutorialMejora)
        {
            UIManager.HideNotification("NoTutorial");
            _tutorialInProgress = true;
            _tutorialPhaseEscenas = 0;
            _tutorialPhaseBanco = 0;
            tutorialMejora = true;
            _tutorialPhaseMejora = 1;

            NextDialogue();
        }
        if (SceneManager.GetActiveScene().name == "Escena_Banco" && _tutorialPhaseBanco== 0 && !tutorialBanco)
        {
            UIManager.HideNotification("NoTutorial");
            _tutorialInProgress = true;
            _tutorialPhaseEscenas = 0;
            _tutorialPhaseMejora = 0;
            _tutorialPhaseBanco = 1;
            tutorialBanco = true;
            NextDialogue();
        }

        // Verificar si se han completado todas las tareas asignadas
        if (SceneManager.GetActiveScene().name == "Escena_Build" && _toTaskCompleted != 0)
        {
            if (_toTaskCompleted - _taskDone == 0)
            {
                _toTaskCompleted = 0;
                _taskDone = 0;
                Invoke("NextDialogue", 0.6f);
            }
        }

    }
    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos

    /// <summary>
    /// Gestiona las acciones a realizar cuando se pulsa un botón del diálogo del tutorial.
    /// </summary>
    /// <param name="buttonText">Texto del botón pulsado (Continuar, Probar, Cerrar).</param>
    public void OnTutorialButtonPressed(string buttonText)
    {
        Debug.Log("Boton:" + buttonText);
        if (buttonText == "Continuar")
        {

            // Avanzar al siguiente diálogo
            Invoke("NextDialogue", 0.1f);
            if (SceneManager.GetActiveScene().name == "Escena_Build")
            {
                FindTutorialPhase();
            }
            if (SceneManager.GetActiveScene().name == "Escena_Mejora")
            {
                FindTutorialPhaseMejora();
            }
            if (SceneManager.GetActiveScene().name == "Escena_Banco")
            {
                FindTutorialPhaseBanco();
            }
            UIManager.HideDialogueButton();
            UIManager.ShowDialogue(_actualDialogueText, _actualDialogueButtonText);
            SoundManager.MadameMooSound();
        }
        else if (buttonText == "Probar")
        {
            // Mostrar notificación de tarea y ocultar diálogo
            if (UIManager.GetInventoryVisible() == true)
            {
                UIManager.ToggleInventory();
            }

                UIManager.ShowNotification(_actualNotificationText, _actualNotificationTaskText, 2, "Tutorial");
                _isNotificationActive = true;
                _notificationActive = UIManager.GetAvailableNotification();
                UIManager.HideDialogueButton();
                UIManager.HideDialogue();

                if (SceneManager.GetActiveScene().name == "Escena_Build") UIManager.HideMap();
        }
        else if (buttonText == "Cerrar")
        {
            // Cerrar diálogo y notificación
            UIManager.HideDialogueButton();
            UIManager.HideDialogue();
            UIManager.HideNotification("Tutorial");
        }
    }

    /// <summary>
    /// Obtiene la fase actual del tutorial principal.
    /// </summary>
    /// <returns>Número de fase del tutorial principal.</returns>
    public int GetTutorialPhase()
    {
        return _tutorialPhase;
    }

    /// <summary>
    /// Establece la fase del tutorial principal.
    /// Solo se usa al cargar una partida guardada.
    /// </summary>
    /// <param name="phase">Nueva fase del tutorial principal.</param>
    public void SetTutorialPhase(int phase)
    {
        _tutorialPhase = phase;
    }

    /// <summary>
    /// Obtiene la fase actual del tutorial de escenas (compra/venta).
    /// </summary>
    /// <returns>Número de fase del tutorial de escenas.</returns>
    public int GetTutorialPhaseEscena()
    {
        return _tutorialPhaseEscenas;
    }

    /// <summary>
    /// Establece la fase del tutorial de escenas.
    /// Solo se usa al cargar una partida guardada.
    /// </summary>
    /// <param name="phase">Nueva fase del tutorial de escenas.</param>
    public void SetTutorialPhaseEscena(int phase)
    {
        _tutorialPhaseEscenas = phase;
    }

    /// <summary>
    /// Obtiene la fase actual del tutorial de mejoras.
    /// </summary>
    /// <returns>Número de fase del tutorial de mejoras.</returns>
    public int GetTutorialPhaseMejora()
    {
        return _tutorialPhaseMejora;
    }

    /// <summary>
    /// Cambia la fase del tutorial (Banco), solo se usa al cargar el juego
    /// </summary>
    /// <param name="phase"></param>
    public void SetTutorialPhaseBanco(int phase)
    {
        _tutorialPhaseBanco = phase;
    }

    /// <summary>
    /// Establece la fase del tutorial del banco.
    /// Solo se usa al cargar una partida guardada.
    /// </summary>
    /// <param name="phase">Nueva fase del tutorial del banco.</param>
    public int GetTutorialPhaseBanco()
    {
        return _tutorialPhaseBanco;
    }

    /// <summary>
    /// Establece la fase del tutorial del banco.
    /// Solo se usa al cargar una partida guardada.
    /// </summary>
    /// <param name="phase">Nueva fase del tutorial del banco.</param>
    public void SetTutorialPhaseMejora(int phase)
    {
        _tutorialPhaseMejora = phase;
    }

    /// <summary>
    /// Indica si hay un diálogo de tutorial activo actualmente.
    /// </summary>
    /// <returns>True si hay un diálogo activo, false en caso contrario.</returns>
    public bool IsDialogueActive()
    {
        return _isDialogueActive;
    }

    /// <summary>
    /// Reinicia todos los datos del tutorial para una nueva partida.
    /// </summary>
    public void ResetTutorialManager()
    {
        _tutorialPhase = 0;
        _actualDialogueButtonText = "";
        _actualDialogueText = "";
        _actualNotificationTaskText = "";
        _actualNotificationText = "";
        _isDialogueActive = false;
        _isNotificationActive = false;
        _taskDone = 0;
        _tutorialPhaseMejora = 0;
        _tutorialInProgress = false;
        _tutorialPhaseBanco = 0;
        _tutorialPhaseEscenas = 0;
        Debug.Log("ResetTutorial");
    }

    /// <summary>
    /// Obtiene el texto del diálogo actual del tutorial.
    /// </summary>
    /// <returns>Texto del diálogo actual.</returns>
    public string GetDialogueText()
    {
        return _actualDialogueText;
    }

    /// <summary>
    /// Obtiene el texto del botón del diálogo actual del tutorial.
    /// </summary>
    /// <returns>Texto del botón actual (Continuar, Probar, Cerrar).</returns>
    public string GetDialogueButtonText()
    {
        return _actualDialogueButtonText;
    }

    /// <summary>
    /// Incrementa el contador de subtareas completadas.
    /// </summary>
    public void SubTask()
    {
        _taskDone++;
    }

    /// <summary>
    /// Establece el número total de subtareas que deben completarse.
    /// </summary>
    /// <param name="i">Número total de subtareas.</param>
    public void SetTask(int i)
    {
        _toTaskCompleted = i;
    }

    /// <summary>
    /// Avanza a la siguiente fase del tutorial correspondiente
    /// según la escena en la que se encuentre el jugador.
    /// </summary>
    public void NextDialogue()
    {
        if (!_tutorialInProgress && SceneManager.GetActiveScene().name != "Escena_Banco")
        {
            //UIManager.HideMap();
            _tutorialPhase++;
            FindTutorialPhase();
            UIManager.ShowDialogue(_actualDialogueText, _actualDialogueButtonText);
            SoundManager.MadameMooSound();
            UIManager.HideNotification("Tutorial");
            _isNotificationActive = false;
        }
        else if (SceneManager.GetActiveScene().name == "Escena_Compra" || SceneManager.GetActiveScene().name == "Escena_Venta") 
        {
            _tutorialPhaseEscenas++;
            FindTutorialPhase(_tutorialPhaseEscenas);
            UIManager.ShowDialogue(_actualDialogueText, _actualDialogueButtonText);
            SoundManager.MadameMooSound();
            UIManager.HideNotification("Tutorial");
            _isNotificationActive = false;
        }
        else if (SceneManager.GetActiveScene().name == "Escena_Mejora")
        {
            Debug.Log("Funciona Respira");
            _tutorialPhaseMejora++;
            FindTutorialPhaseMejora();
            UIManager.ShowDialogue(_actualDialogueText, _actualDialogueButtonText);
            SoundManager.MadameMooSound();
            UIManager.HideNotification("Tutorial");
            _isNotificationActive = false;

        }
        else if (SceneManager.GetActiveScene().name == "Escena_Banco")
        {
            Debug.Log("Funciona Respira");
            _tutorialPhaseBanco++;
            FindTutorialPhaseBanco();
            UIManager.ShowDialogue(_actualDialogueText, _actualDialogueButtonText);
            SoundManager.MadameMooSound();
            UIManager.HideNotification("Tutorial");
            _isNotificationActive = false;
        }
    }

    /// <summary>
    /// Muestra nuevamente el diálogo actual al pulsar en la notificación.
    /// </summary>
    public void ActualDialogue()
    {
        UIManager.ShowDialogue(_actualDialogueText, _actualDialogueButtonText);
        SoundManager.MadameMooSound();
    }

    /// <summary>
    /// Marca una tarea como completada en la notificación de tutorial.
    /// </summary>
    /// <param name="checkbox">Índice del checkbox a marcar como completado.</param>
    public void CheckBox(int checkbox)
    {
        UIManager.Check(checkbox);
    }

    /// <summary>
    /// Modifica el texto de la notificación de tutorial actual.
    /// </summary>
    /// <param name="text">Nuevo texto principal de la notificación.</param>
    /// <param name="task">Nuevo texto de las tareas de la notificación.</param>
    public void ModifyNotification (string text, string task)
    {
        _actualNotificationText = text;
        _actualNotificationTaskText = task;
    }
    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados



    /// <summary>
    /// Configura los textos de diálogos y notificaciones según la fase actual 
    /// del tutorial principal.
    /// </summary>
    private void FindTutorialPhase()
    {
        if (_tutorialPhase == 1)
        {
            _actualDialogueText = MadameMooColor + " ¡Muuuy buenas, Connie! Soy Madame Moo, vaca de gafas y sabia consejera. Estoy aquí para enseñarte cómo convertirte en la mejor granjera de RootWood.\r\n¡Sigue mis consejos y estarás un paso más cerca de tu casa soñada y una vida de lujo y fertilizante premium!";
            _actualDialogueButtonText = "Continuar";
            _isInMainTutorial = true;
        }
        if (_tutorialPhase == 2)
        {
            _actualDialogueText = MadameMooColor + " Para usar cosas, como palas o regaderas, solo tienes que pulsar la tecla "+  _use +".\r\n¡Es como decirle a la vida: “¡Estoy lista para interactuar contigo!”";
            _actualDialogueButtonText = "Continuar";
        }
        if (_tutorialPhase == 3)
        {
            _actualDialogueText = MadameMooColor + " ¿Te perdiste? ¿No sabes si estás en tu huerto o en casa de la vecina?\r\nPulsa la tecla "+ _map + " y abre el mapa. ¡Orientarte es clave para no acabar regando el gallinero!";
            _actualDialogueButtonText = "Probar";
            _actualNotificationText = "Abre el mapa\nPulsa "+ _map;
            _actualNotificationTaskText = "[ ] Mapa";
        }
        if (_tutorialPhase == 4)
        {
            _actualDialogueText = MadameMooColor + " ¡Muuhh! (Alegría), ya casi lo tienes. A lo largo del pueblo hay varias casas a tu disposición, y cada una sirve para algo distinto.\r\nPero de eso ya hablaremos en su momento."; 
            _actualDialogueButtonText = "Continuar";
        }
        if (_tutorialPhase == 5)
        {
            _actualDialogueText = MadameMooColor + " También tienes varias herramientas para usar en tu huerto.\r\nUsa el selector de herramientas "+ _toolSelector+" para elegir lo que necesitas.\r\n¡Una vaca no puede arar con la lengua, ni tú cosechar sin azada!";
            _actualDialogueButtonText = "Continuar";
        }
        if (_tutorialPhase == 6)
        {
            _actualDialogueText = MadameMooColor + " Para que tu huerto tenga color necesitarás más de un tipo de semilla.\r\nUsa el selector de herramientas "+ _seedSelector+" para cambiar entre las diferentes semillas.";
            _actualDialogueButtonText = "Probar";
            _actualNotificationText = "Selecciona todas \nlas herramientas";
            _actualNotificationTaskText = "[ ] Regadera\r\n[ ] Hoz\r\n[ ] Pala\r\n[ ] Semillas";
            _toTaskCompleted = 4;
        }
        if (_tutorialPhase == 7)
        {
            _actualDialogueText = MadameMooColor + " Por último. Pulsa "+ _inventory +" para abrir tu inventario.\r\nAhí podrás ver todo lo que llevas encima: semillas, cultivos y herramientas y quién sabe, ¡quizás algún queso viejo que olvidaste! (broma).";
            _actualDialogueButtonText = "Probar";
            _actualNotificationText = "Abre el inventario\nPulsa " + _inventory;
            _actualNotificationTaskText = "[ ] Inventario";
        }
        if (_tutorialPhase == 8)
        {
            _actualDialogueText = MadameMooColor + " ¡Uy, Connie! Ese inventario está más vacío que una lechera en sequía...\r\n¡Vamos a llenarlo de cosas buenas antes de que los grillos monten una fiesta ahí dentro!";
            _actualDialogueButtonText = "Continuar";
        }
        if (_tutorialPhase == 9)
        {   
            _actualDialogueText = MadameMooColor + " Ve a la tienda y compra unas cuantas semillas.\r\nSin semillas, no hay cosecha… ¡y sin cosecha, no hay cheddar! Y no hablo de queso precisamente.";
            _actualDialogueButtonText = "Probar";
            _actualNotificationText = "Ve a la casa \nde compra";
            _actualNotificationTaskText = "[ ] Compra una\r\n    semilla de\r\n    lechuga";
        }
        if (_tutorialPhase == 10) // Compra
        {
            _actualDialogueText = MadameMooColor + " Para continuar, necesitas comprar al menos unas poquitas semillas.\r\nNo seas tímida con el monedero, Connie. ¡La inversión inicial es el primer paso hacia tu casa soñada!";
            _actualDialogueButtonText = "Continuar";
        }
        if (_tutorialPhase == 11)
        {
            _actualDialogueText = MadameMooColor + "Más adelante podrás desbloquear nuevas semillas pero de momento...\n pulsa el icono de lechuga para comprar tu primera semilla.";
            _actualDialogueButtonText = "Probar";

            _actualNotificationText = "Mi primera \ncompra";
            _actualNotificationTaskText = "[ ] Pulsa la\r\nlechuga";

        }
        if (_tutorialPhase == 12)
        {
            _actualDialogueText = MadameMooColor + " Con una no será suficiente para cultivar todo un huerto no crees?.\r\nPulsa el botón de más (+), para añadir unas pocas semillas.";
            _actualDialogueButtonText = "Probar";

            _actualNotificationText = "Mi primera \ncompra";
            _actualNotificationTaskText = "[ ] Pulsa el\r\nbotón de más(+)";
        }
        if (_tutorialPhase == 13)
        {
            _actualDialogueText = MadameMooColor + " Con todo listo es hora de pasar por caja.\r\nPulsa el botón de comprar, y despidete de algunas moneditas (Por ahora).\nPara salir de la interfaz pulsa " + _exit;
            _actualDialogueButtonText = "Probar";
            _actualNotificationText = "Mi primera \ncompra";
            _actualNotificationTaskText = "[ ] Pulsa \r\ncomprar";
        }
        if (_tutorialPhase == 14) // Huerto
        {
            _actualDialogueText = MadameMooColor + " ¡De vuelta al huerto!\r\nSelecciona tus semillas desde el inventario, acércate a un surco y plántalas con cariño.\r\n¡No las tires como si fueran cacahuetes! Son el futuro.";
            _actualDialogueButtonText = "Probar";
            _actualNotificationText = "Planta una lechuga";
            _actualNotificationTaskText = "[ ] Planta\r\nuna semilla de\r\nlechuga";
        }
        if (_tutorialPhase == 15)
        {
            _actualDialogueText = MadameMooColor + " Después de plantar, toca regar.\r\nUsa la regadera para darles un buen bañito. Las plantas no crecen con cariño… ¡crecen con agua!";
            _actualDialogueButtonText = "Probar";
            _actualNotificationText = "Riega la lechuga";
            _actualNotificationTaskText = "[ ] Riega\r\nlas semilla de\r\nlechuga";
        }
        if (_tutorialPhase == 16)
        {
            _actualDialogueText = MadameMooColor + " ¿Se acabó el agua? ¡Normal, no eres una fuente ambulante!\r\nAcércate al pozo para rellenar tu regadera.\r\nRecuerda: una planta sedienta es una planta triste… ¡y una planta triste no se vende!";
            _actualDialogueButtonText = "Probar";
            _actualNotificationText = "Rellena tu regadera";
            _actualNotificationTaskText = "[ ] Rellena\r\nla regadera";
        }
        if (_tutorialPhase == 17)
        {
            _actualDialogueText = MadameMooColor + " Cuando veas una planta bien crecidita entonces ¡Es hora de cosechar!\r\nSelecciona la HOZ y recógela, querida!";
            _actualDialogueButtonText = "Continuar";
        }
        if (_tutorialPhase == 18)
        {
            _actualDialogueText = MadameMooColor + " No todas las plantas prosperan, y eso está bien.\r\nSi ves una que no va a dar más de sí… arráncala con la PALA sin miedo.\r\nAsí haces espacio para nuevas oportunidades. ¡Como en la vida!";
            _actualDialogueButtonText = "Continuar";
        }
        if (_tutorialPhase == 19)
        {
            _actualDialogueText = MadameMooColor + " Las malas hierbas aparecen solas y molestan más que un gallo insomne.\n Arráncalas!";
            _actualDialogueButtonText = "Probar";
            _actualNotificationText = "Aprende a \ncuidar tu huerto";
            _actualNotificationTaskText = "[ ] Cosechar\r\n[ ] Muerte\r\n[ ] Mala hierba";
            SetTask(3);
        }

        if (_tutorialPhase == 20) // Venta
        {
            _actualDialogueText = MadameMooColor + " ¡Es hora de hacer negocios!\n Dirígete al puesto de ventas con tus cultivos recién cosechados.\n Nada dice \"éxito\" como vender tu primer nabo, quiero decir… cultivo.";
            _actualDialogueButtonText = "Probar";
            _actualNotificationText = "Vende tu \nprimera cosecha";
            _actualNotificationTaskText = "[ ] Vende\n una lechuga";
        }

        if (_tutorialPhase == 21)
        {
            _actualDialogueText = MadameMooColor + " Pulsa el icono de lechuga para vender tu primer cultivo.";
            _actualDialogueButtonText = "Probar";
        }
        if (_tutorialPhase == 22)
        {
            _actualDialogueText = MadameMooColor + " ¿Tienes más lechugas que vender?.\r\nPulsa el botón de más, para añadir más.";
            _actualDialogueButtonText = "Probar";
        }
        if (_tutorialPhase == 23)
        {
            _actualDialogueText = MadameMooColor + " Con todo listo es hora de ver cuanto nos dan por tu primera cosecha.\r\nPulsa el botón de vender, y verás que sembrar te traerá algo más que plantas.";
            _actualDialogueButtonText = "Probar";
        }

        if (_tutorialPhase == 24)
        {
            _actualDialogueText = MadameMooColor + " Cada moneda que ganes es un paso más cerca de mudarte a tu casa soñada.\n ¡Connie, estás empezando a florecer! Ya estás preparada para visitar todo el pueblo.";
            _actualDialogueButtonText = "Continuar";
        }

        if (_tutorialPhase == 25)
        {
            _actualDialogueText = MadameMooColor + " ¡Ay! Querida, mientras entras en todas las casas ¡vigila tu energía! Arriba a la derecha tienes la barra de energía que disminuye al correr. Para recuperar energia simplemente deberás estar quieta, pero seguro que con la edad que tienes ya lo sabias... ¿Como? ¡Connie yo estoy como nueva! No inventes...";
            _actualDialogueButtonText = "Continuar";
        }
        if (_tutorialPhase == 26)
        {
            _actualDialogueText = MadameMooColor + " ¡Así que eso es todo por ahora...Casi se me olvidaba, para que no te olvides de las cosas como me pasa a mi, consulta la enciclopedia en el menú para resolver tus dudas. Ahora si, ¡Muuucha suerte ahí fuera!";
            _actualDialogueButtonText = "Cerrar";
            _isInMainTutorial = false;
        }

    }

    /// <summary>
    /// Configura los textos de diálogos y notificaciones según la fase actual 
    /// del tutorial de escenas (compra/venta).
    /// </summary>
    /// <param name="i">Fase actual del tutorial de escenas.</param>
    private void FindTutorialPhase(int i)
    {
        if (SceneManager.GetActiveScene().name == "Escena_Compra")
        { 
            if (i == 1)
            {
                _actualDialogueText = MadameMooColor + " ¡Muuuy buenas, Connie! Encantada de encontrarnos de nuevo.\r\nAhora voy a enseñarte a comprar en el mercado, estas un paso más cerca de recoger tu primera cosecha!";
                _actualDialogueButtonText = "Continuar";
            }
            if (i == 2)
            {
                _actualDialogueText = MadameMooColor + " ¡Lo primero es lo primero, Connie! Hay que saludar. \r\nAcércate al mostrador y habla con el Señor Relincho.";
                _actualDialogueButtonText = "Probar";
                _actualNotificationText = "Acercate al \nmostrador";
                _actualNotificationTaskText = "[ ] Pulsa " + _use+" para hablar.";
                _tutorialPhaseEscenas++;
                _tutorialInProgress = false;
            }
        }
        else if (SceneManager.GetActiveScene().name == "Escena_Venta")
        {
            if (i == 4)
            {
                _actualDialogueText = MadameMooColor + " ¡Muuuy buenas, Vaca-Connie! Perdona... era un chiste, ¿Lo entiendes? La vaca soy yo (risa).\r\nBueno, ¡Ahora voy a enseñarte a sacar provecho de tus cultivos en el mercado, vamos a vender tu cosecha!";
                _actualDialogueButtonText = "Continuar";
            }
            if (i == 5)
            {
                _actualDialogueText = MadameMooColor + " ¡Ya lo has aprendido, lo primero es lo primero, Connie! A saludar. \r\nAcercate al mostrador y habla con el joven de Jamoncio.";
                _actualDialogueButtonText = "Probar";
                _actualNotificationText = "Acercate al \nmostrador";
                _actualNotificationTaskText = "[ ] Pulsa " + _use+ " para hablar.";
                _tutorialInProgress = false;
            }
        }
    }

    /// <summary>
    /// Configura los textos de diálogos y notificaciones según la fase actual 
    /// del tutorial del banco.
    /// </summary>
    private void FindTutorialPhaseBanco()
    {
            if (_tutorialPhaseBanco == 2)
            {
                _actualDialogueText = MadameMooColor + " ¡Muuuy buenas, Connie! Encantada de encontrarnos de nuevo.\r\nAhora voy a enseñarte todo lo que puedes hacer con tu dinero!";
                _actualDialogueButtonText = "Continuar";
            }
            if (_tutorialPhaseBanco == 3)
            {
                _actualDialogueText = MadameMooColor + "¡Lo primero es saludar a los gemelos Copi y Pasti, Connie! \r\nAcercate al mostrador y habla con Capi... bueno o con Pasti.";
                _actualDialogueButtonText = "Probar";
                _actualNotificationText = "Acercate al \nmostrador";
                _actualNotificationTaskText = "[ ] Pulsa " + _use+ " para hablar.";
                _tutorialPhaseEscenas++;
            }
        if (_tutorialPhaseBanco == 4)
        {
            _actualDialogueText = MadameMooColor + " ¡Vamos a ingresar dinero! \r\nPero antes hay varias cosas importantes que debes saber. ";
            _actualDialogueButtonText = "Continuar";
            _actualNotificationText = "";
        }
        if (_tutorialPhaseBanco == 5)
        {
            _actualDialogueText = MadameMooColor + " Cuando ingreses dinero, tienes que estar segura de que no lo vas a necesitar \r\nLuego no podrás recuperarlo. \nAdemás nunca podrás ingresar todo tu dinero, siempre debes quedarte al menos con 1000 RootCoins. ";
            _actualDialogueButtonText = "Continuar";
        }
        if (_tutorialPhaseBanco == 6)
        {
            _actualDialogueText = MadameMooColor + " Pulsa el botón ingresar \r\n Después desliza la barra hasta llegar a la cantidad justa que quieras ingresar.";
            _actualDialogueButtonText = "Probar";
            _actualNotificationText = "Ingresa dinero";
            _actualNotificationTaskText = "[ ] Pulsa ingresar. \n[ ] Desliza \n la barra.";
        }
        if (_tutorialPhaseBanco == 7)
        {
            _actualDialogueText = MadameMooColor + " ¡Vamos con la mudanza! Como bien sabes para mudarte a tu deseada casa en la playa necesitas dinero\r\n Con tu dinero del banco, podrás comprar tu nuevo hogar.";
            _actualDialogueButtonText = "Continuar";
        }
        if (_tutorialPhaseBanco == 8)
        {
            _actualDialogueText = MadameMooColor + " Pulsa el botón mudanza \r\n Después selecciona la casa\n Cuando tengas el suficiente dinero, repite estos pasos y podrás mudarte a tu casa soñada.";
            _actualDialogueButtonText = "Probar";
            _actualNotificationText = "Mudanza";
            _actualNotificationTaskText = "[ ] Pulsa mudanza. \n[ ] Pulsa la casa.";
        }
        if (_tutorialPhaseBanco == 9)
        {
            _actualDialogueText = MadameMooColor + " Esto es todo por ahora ¿Crees que volveremos a vernos? Recuerda: no todo es tumbarse al sol… alguien tiene que clavar la sombrilla.. ¡Muuucha suerte ahí fuera!";
            _actualDialogueButtonText = "Cerrar";
            _actualNotificationText = " ";
            _tutorialInProgress = false;
        }


    }

    /// <summary>
    /// Configura los textos de diálogos y notificaciones según la fase actual 
    /// del tutorial de mejoras.
    /// </summary>
    private void FindTutorialPhaseMejora()
    {
        if (_tutorialPhaseMejora == 2)
        {
            _actualDialogueText = MadameMooColor + " ¡Muuuy buenas, Connie! Encantada de encontrarnos de nuevo.\r\nAhora voy a enseñarte a mejorar tus herramientas y tu huerto!";
            _actualDialogueButtonText = "Continuar";
        }
        if (_tutorialPhaseMejora == 3)
        {
            _actualDialogueText = MadameMooColor + "¡Primero Saluda, Connie! \r\nAcércate al mostrador y habla con Lana del Rey, antigua cantante, ahora oveja.\"";
            _actualDialogueButtonText = "Probar";
            _actualNotificationText = "Acercate al \nmostrador";
            _actualNotificationTaskText = "[ ] Pulsa " + _use+ " para hablar.";
        }
        if (_tutorialPhaseMejora == 4)
        {
            _actualDialogueText = MadameMooColor + " ¡Vamos con las ampliaciones! \r\nCon ellas podrás hacer crecer el tamaño de tu huerto y el de tu cosecha! ";
            _actualDialogueButtonText = "Continuar";
        }
        if (_tutorialPhaseMejora == 5)
        {
            _actualDialogueText = MadameMooColor + " Pulsa el botón de Ampliar y después selecciona la maceta.";
            _actualDialogueButtonText = "Probar";
            _actualNotificationText = "Prueba a \ampliar \ntu huerto";
            _actualNotificationTaskText = "[ ] Pulsa Ampliar\n[ ] Pulsa la maceta";
        }
        if (_tutorialPhaseMejora == 6)
        {
            _actualDialogueText = MadameMooColor + " Cuando reunas el suficiente dinero, podrás disfrutar de esta ampliación. \r\n¡Mientras tanto toca trabajar duro Connie!";
            _actualDialogueButtonText = "Continuar";
        }
        if (_tutorialPhaseMejora == 7)
        {
            _actualDialogueText = MadameMooColor + " ¡Vamos con las mejoras! \r\nCon ellas podrás ampliar la capacidad de tu regadera. ¡Al final vas a ser un pozo con patas (risa)!";
            _actualDialogueButtonText = "Continuar";
        }
        if (_tutorialPhaseMejora == 8)
        {
            _actualDialogueText = MadameMooColor + " Pulsa el botón de Mejorar y después selecciona la regadera.";
            _actualDialogueButtonText = "Probar";
            _actualNotificationText = "Prueba a \nmejorar \ntu regadera";
            _actualNotificationTaskText = "[ ] Pulsa Mejorar\n[ ] Pulsa la regadera";
        }
        if (_tutorialPhaseMejora == 9)
        {
            _actualDialogueText = MadameMooColor + " Cuando reunas el suficiente dinero, podrás disfrutar de esta mejora. ¡Mientras tanto toca hacer viajes al pozo!";
            _actualDialogueButtonText = "Continuar";

        }
        if (_tutorialPhaseMejora == 10)
        {
            _actualDialogueText = MadameMooColor + " Esto es todo por ahora ¿Crees que volveremos a vernos? Recuerda: no esperes cosechar aguacates si solo siembras excusas. ¡Muuucha suerte ahí fuera!";
            _actualDialogueButtonText = "Cerrar";
            _tutorialInProgress = false;
        }

    }

    /// <summary>
    /// Inicializa las referencias a otros componentes necesarios para el funcionamiento
    /// del sistema de tutoriales.
    /// </summary>
    private void InitializeReferences()
    {
        PlayerMovement = FindObjectOfType<PlayerMovement>();
        UIManager = FindObjectOfType<UIManager>();
    }
    #endregion

} // class TutorialManager 
// namespace
