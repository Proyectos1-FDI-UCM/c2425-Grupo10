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

    ///<summary>
    ///ref al notification manager
    /// </summary>
    [SerializeField] private NotificationManager NotificationManager;
    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)

    /// <summary>
    /// Int para saber la fase del tutorial 
    /// </summary>
    [SerializeField] private int _tutorialPhase = 0;

    /// <summary>
    /// Int para saber la fase del tutorial de compra 
    /// </summary>
    [SerializeField]private int _tutorialPhaseEscenas = 0;

    [SerializeField] private int _tutorialPhaseMejora = 0;
    private bool tutorialMejora = false;
    [SerializeField] private int _tutorialPhaseBanco = 0;
    private bool tutorialBanco = false;

    private bool _tutorialInProgress = false;

    /// <summary>
    /// Dialogo actual del tutorial
    /// </summary>
    [SerializeField]private string _actualDialogueText;
    private string _actualDialogueButtonText;

    /// <summary>
    /// Dialogo Actual de las notificaciones
    /// </summary>
    private string _actualNotificationText;

    /// <summary>
    /// Dialogo Actual de las tareas de notificaciones
    /// </summary>
    private string _actualNotificationTaskText;

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

    /// <summary>
    /// Subtareas totales
    /// </summary>
    [SerializeField]private int _toTaskCompleted = 0;
    
    /// <summary>
    /// Tareas hechas
    /// </summary>
    [SerializeField]private int _taskDone = 0;

    private bool _isInMainTutorial = false;
    private string _use;
    private string _map;
    private string _inventory;
    private string _toolSelector;
    private string _seedSelector;
    private string _exit;

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
        NotificationManager = GetComponent<NotificationManager>();
        if(GameManager.Instance.GetCinematicState() == true)
        {
            //UIManager.HideDialogue();
        }
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
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
        if (SoundManager == null)
        {
            SoundManager = FindObjectOfType<SoundManager>();
        }
        //if (InventoryManager.GetInventory(Items.Letuce) >= 1 && _tutorialPhase == 8) 
        //if (SceneManager.GetActiveScene().name == "Escena_Build" && _tutorialPhase == 9) 
        //{
        //    //NextDialogue();
        //}
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
    /// Metodo para saber si el boton ha sido pulsado
    /// </summary>
    /// <param name="buttonText"></param>
    public void OnTutorialButtonPressed(string buttonText)
    {
        Debug.Log("Boton:" + buttonText);
        if (buttonText == "Continuar")
        {
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
            UIManager.HideDialogueButton();
            UIManager.HideDialogue();
            UIManager.HideNotification("Tutorial");
        }
    }
    ///<summary>
    ///Metodo para obtener la fase del tutorial
    /// </summary>
    public int GetTutorialPhase()
    {
        return _tutorialPhase;
    }

    /// <summary>
    /// Cambia la fase del tutorial, solo se usa al cargar el juego
    /// </summary>
    /// <param name="phase"></param>
    public void SetTutorialPhase(int phase)
    {
        _tutorialPhase = phase;
    }
    public bool IsDialogueActive()
    {
        return _isDialogueActive;
    }

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

    ///<summary>
    ///Metodo para obtener la fase del tutorial mejora
    /// </summary>
    public int GetTutorialPhaseMejora()
    {
        return _tutorialPhaseMejora;
    }

    ///<summary>
    ///Metodo para obtener la fase del tutorial banco
    /// </summary>
    public int GetTutorialPhaseBanco()
    {
        return _tutorialPhaseBanco;
    }
    public void SubTask()
    {
        _taskDone++;
    }
    public void SetTask(int i)
    {
        _toTaskCompleted = i;
    }

    ///<summary>
    ///Metodo para avanzar en el dialogo
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

    public void ModifyNotification (string text, string task)
    {
        _actualNotificationText = text;
        _actualNotificationTaskText = task;
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
            _actualDialogueText = MadameMooColor + " Por último. Pulsa "+ _inventory +" para abrir tu inventario.\r\nAhí podrás ver todo lo que llevas encima: semillas, cultivos, y herramienta y quién sabe, ¡quizás algún queso viejo que olvidaste! (broma).";
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
            _actualDialogueText = MadameMooColor + "Mas adelantes podras desbloquear nuevas semillas pero de momento...\n pulsa el icono de lechuga para comprar tu primera semilla";
            _actualDialogueButtonText = "Probar";

            _actualNotificationText = "Mi primera \ncompra";
            _actualNotificationTaskText = "[ ] Pulsa la\r\nlechuga";

        }
        if (_tutorialPhase == 12)
        {
            _actualDialogueText = MadameMooColor + " Con una no será suficiente para cultivar todo un huerto no crees?.\r\nPulsa el botón de más(+), para añadir unas pocas semillas";
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
            _actualDialogueText = MadameMooColor + " ¡Es hora de hacer negocios!\n Dirígete al puesto de ventas con tus cultivos recién cosechados.\n Nada dice \"éxito\" como vender tu primer nabo, quiero decir… cultivo";
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
            _actualDialogueText = MadameMooColor + " ¿Tienes más lechugas que vender?.\r\nPulsa el botón de más, para añadir más";
            _actualDialogueButtonText = "Probar";
        }
        if (_tutorialPhase == 23)
        {
            _actualDialogueText = MadameMooColor + " Con todo listo es hora de ver cuanto nos dan por tu primera cosecha.\r\nPulsa el botón de vender, y verás que sembrar te traerá algo más que plantas";
            _actualDialogueButtonText = "Probar";
        }

        if (_tutorialPhase == 24)
        {
            _actualDialogueText = MadameMooColor + " Cada moneda que ganes es un paso más cerca de mudarte a tu casa soñada.\n ¡Connie, estás empezando a florecer! Ya estas preparada para visitar todo el pueblo.";
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
                _actualDialogueText = MadameMooColor + " ¡Lo primero es lo primero, Connie! Hay que saludar. \r\nAcercate al mostrador y habla con el Señor Relincho";
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
                _actualDialogueText = MadameMooColor + " ¡Ya lo has aprendido, lo primero es lo primero, Connie! A saludar. \r\nAcercate al mostrador y habla con el joven de Jamoncio";
                _actualDialogueButtonText = "Probar";
                _actualNotificationText = "Acercate al \nmostrador";
                _actualNotificationTaskText = "[ ] Pulsa " + _use+ " para hablar.";
                _tutorialInProgress = false;
            }
        }
    }
    private void FindTutorialPhaseBanco()
    {
            if (_tutorialPhaseBanco == 2)
            {
                _actualDialogueText = MadameMooColor + " ¡Muuuy buenas, Connie! Encantada de encontrarnos de nuevo.\r\nAhora voy a enseñarte todo lo que puedes hacer con tu dinero!";
                _actualDialogueButtonText = "Continuar";
            }
            if (_tutorialPhaseBanco == 3)
            {
                _actualDialogueText = MadameMooColor + "¡Lo primero es saludar a los gemelos Copi y Pasti, Connie! \r\nAcercate al mostrador y habla con Capi... bueno o con Pasti";
                _actualDialogueButtonText = "Probar";
                _actualNotificationText = "Acercate al \nmostrador";
                _actualNotificationTaskText = "[ ] Pulsa " + _use+ " para hablar.";
                _tutorialPhaseEscenas++;
            }
        if (_tutorialPhaseBanco == 4)
        {
            _actualDialogueText = MadameMooColor + " ¡Vamos a ingresar dinero! \r\nPero antes hay varias cosas importantes que debes saber ";
            _actualDialogueButtonText = "Continuar";
            _actualNotificationText = "";
        }
        if (_tutorialPhaseBanco == 5)
        {
            _actualDialogueText = MadameMooColor + " Cuando ingreses dinero, tienes que estar segura de que no lo vas a necesitar \r\nLuego no podrás recuperarlo. \nAdemás nunca podrás ingresar todo tu dinero, siempre debes quedarte al menos con 1000 RootCoins ";
            _actualDialogueButtonText = "Continuar";
        }
        if (_tutorialPhaseBanco == 6)
        {
            _actualDialogueText = MadameMooColor + " Pulsa el botón ingresar \r\n Después desliza la barra hasta llegar a la cantidad justa que quieras ingresar";
            _actualDialogueButtonText = "Probar";
            _actualNotificationText = "Ingresa dinero";
            _actualNotificationTaskText = "[ ] Pulsa ingresar. \n[ ] Desliza \n la barra.";
        }
        if (_tutorialPhaseBanco == 7)
        {
            _actualDialogueText = MadameMooColor + " ¡Vamos con la mudanza! Como bien sabes para mudarte a tu deseada casa en la playa necesitas dinero\r\n Con tu dinero del banco, podrás comprar tu nuevo hogar";
            _actualDialogueButtonText = "Continuar";
        }
        if (_tutorialPhaseBanco == 8)
        {
            _actualDialogueText = MadameMooColor + " Pulsa el botón mudanza \r\n Después selecciona la casa\n Cuando tengas el suficiente dinero, repite estos pasos y podrás mudarte a tu casa soñada";
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
    private void FindTutorialPhaseMejora()
    {
        if (_tutorialPhaseMejora == 2)
        {
            _actualDialogueText = MadameMooColor + " ¡Muuuy buenas, Connie! Encantada de encontrarnos de nuevo.\r\nAhora voy a enseñarte a mejorar tus herramientas y tu huerto!";
            _actualDialogueButtonText = "Continuar";
        }
        if (_tutorialPhaseMejora == 3)
        {
            _actualDialogueText = MadameMooColor + "¡Primero Saluda, Connie! \r\nAcercate al mostrador y habla con Lana del Rey, antigua cantante, ahora oveja.\"";
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
            _actualDialogueText = MadameMooColor + " Pulsa el botón de Ampliar y después selecciona la maceta";
            _actualDialogueButtonText = "Probar";
            _actualNotificationText = "Prueba a \ampliar \ntu huerto";
            _actualNotificationTaskText = "[ ] Pulsa Ampliar\n[ ] Pulsa la maceta";
        }
        if (_tutorialPhaseMejora == 6)
        {
            _actualDialogueText = MadameMooColor + " Cuando reunas el suficiente dinero, podrás disfrutar de esta ampliación \r\n¡Mientras tanto toca trabajar duro Connie!";
            _actualDialogueButtonText = "Continuar";
        }
        if (_tutorialPhaseMejora == 7)
        {
            _actualDialogueText = MadameMooColor + " ¡Vamos con las mejoras! \r\nCon ellas podrás ampliar la capacidad de tu regadera. ¡Al final vas a ser un pozo con patas (risa)!";
            _actualDialogueButtonText = "Continuar";
        }
        if (_tutorialPhaseMejora == 8)
        {
            _actualDialogueText = MadameMooColor + " Pulsa el botón de Mejorar y después selecciona la regadera";
            _actualDialogueButtonText = "Probar";
            _actualNotificationText = "Prueba a \nmejorar \ntu regadera";
            _actualNotificationTaskText = "[ ] Pulsa Mejorar\n[ ] Pulsa la regadera";
        }
        if (_tutorialPhaseMejora == 9)
        {
            _actualDialogueText = MadameMooColor + " Cuando reunas el suficiente dinero, podrás disfrutar de esta mejora ¡Mientras tanto toca hacer viajes al pozo!";
            _actualDialogueButtonText = "Continuar";

        }
        if (_tutorialPhaseMejora == 10)
        {
            _actualDialogueText = MadameMooColor + " Esto es todo por ahora ¿Crees que volveremos a vernos? Recuerda: no esperes cosechar aguacates si solo siembras excusas. ¡Muuucha suerte ahí fuera!";
            _actualDialogueButtonText = "Cerrar";
            _tutorialInProgress = false;
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
