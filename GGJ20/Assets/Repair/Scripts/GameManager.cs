using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public static GameManager instance;
    public float turnDelay = 0.1f;
    public float levelStartDelay = 2f;
    public bool doingSetup;

    public int playerFoodPoints = 100;
    [HideInInspector] public bool playersTurn = true;

    //Objeto mapa
    private BoardManager boardScript;

    private List<Minion> minions = new List<Minion>();
    private bool enemiesMoving;

    private int level = 1;

    private GameObject restartButton;
    private GameObject levelImage;
    private Text levelText;
    
    

    private void Awake()
    {
        if(GameManager.instance == null)
        {
            GameManager.instance = this;
        }else if(GameManager.instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
        boardScript = GetComponent<BoardManager>();
        
    }

    private void Update()
    {
        if (!playersTurn && !enemiesMoving && !doingSetup)
        {
            StartCoroutine(MoveMinions());
        }
    
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;

    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    //Pasa al siguiente nivel
    private void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        
        InitGame();
        level++;
    }

    //Metodo que inicializa el juego
    private void InitGame()
    {
        doingSetup = true;
        levelImage = GameObject.Find("LevelImage");
        levelText = GameObject.Find("LevelText").GetComponent<Text>();
        levelText.text = "Day " + level;
        levelImage.SetActive(true);
        restartButton = GameObject.Find("Restart");
        restartButton.SetActive(false);
        
        boardScript.SetupScene(level);

        Invoke("HideLevelImage", levelStartDelay);
    }

    //Esconde la imagen del menu
    private void HideLevelImage()
    {
        levelImage.SetActive(false);
        doingSetup = false;
    }

    //Finaliza la partida
    public void GameOver()
    {
        levelText.text = "YOU DIED";
        levelImage.SetActive(true);
        restartButton.SetActive(true);
        enabled = false;
    }

    //Corrutina que mueve a los enemigos
    IEnumerator MoveMinions()
    {
        enemiesMoving = true;

        yield return new WaitForSeconds(turnDelay);
        if(minions.Count == 0)
        {
            yield return new WaitForSeconds(turnDelay); //Si no hay enemigos espera hasta coger la siguiente orden del jugador
        }

        for(int i=0; i<minions.Count; i++)
        {
            //mover minion
            yield return new WaitForSeconds(minions[i].moveTime);
        }
        playersTurn = true;
        enemiesMoving = false;
    }

    //Añade a un enemigo a la lista de enemigos
    public void AddMinionToLIst(Minion enemy)
    {
        minions.Add(enemy);
    }

}
