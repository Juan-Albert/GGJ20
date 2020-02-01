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

    public int playerLife = 6;
    private BoardManager boardScript;

    private List<Minion> minions = new List<Minion>();

    private int level = 1;
    
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

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;

    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    private void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        
        InitGame();
        level++;
    }

    private void InitGame()
    {

        boardScript.SetupScene(level);

    }
    
    public void GameOver()
    {
        enabled = false;
    }

    IEnumerator MoveMinions(MovingObject.MovementDirection movDir)
    {

        yield return new WaitForSeconds(turnDelay);

        for(int i=0; i<minions.Count; i++)
        {
            minions[i].MoveMinion(movDir);
            yield return new WaitForSeconds(minions[i].moveTime);
        }
    }

    public void AddMinionToLIst(Minion enemy)
    {
        minions.Add(enemy);
    }

}
