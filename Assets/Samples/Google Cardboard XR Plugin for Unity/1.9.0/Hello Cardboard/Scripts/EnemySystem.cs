using UnityEngine;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Timers;
using System.Threading.Tasks;


public class EnemySystem : MonoBehaviour
{
  public GameObject enemyPrefab;
  public GameObject thePlayer;
  public GameObject killCounter;
  public Text counterText;

  public int maxEnemies = 1;
  public float minEnemyDistance = 10f;
  public float maxEnemyDistance = 100f;
  public float minEnemyHeightPosition = 10f;
  public float maxEnemyHeightPosition = 100f;
  private List<GameObject> enemies = new List<GameObject>();
  private static ILogger logger = Debug.unityLogger;

  private GameObject gameManager;

  private GameObject gameOverComponent;
  private Text gameOverText;

  private void OnEnemyDeath(GameObject enemy)
  {
    enemies.Remove(enemy);
    GameManager.EndGame = false;
  }

  void Start()
  {
    gameManager = GameObject.Find("GameManager");
    gameOverComponent = GameObject.Find("GameOver");
    logger.Log(gameOverComponent);
    gameOverText = gameOverComponent.GetComponent<Text>();
    logger.Log(gameOverText);
    killCounter = GameObject.Find("KillCounter");
    counterText = killCounter.GetComponent<Text>();
  }

  void Update()
  {
    if (GameManager.EndGame)
    {
      enemies.ForEach((obj) =>
      {
        enemies.Remove(obj);
        Destroy(obj);
      });
      gameOverText.enabled = true;
      Invoke("Restart", 5);
    }
    else
    {
      gameOverText.enabled = false;
      if (enemies.Count < maxEnemies && thePlayer)
      {
        float angle = UnityEngine.Random.Range(-Mathf.PI, Mathf.PI);
        float distance = UnityEngine.Random.Range(minEnemyDistance, maxEnemyDistance);
        float height = UnityEngine.Random.Range(minEnemyHeightPosition, maxEnemyHeightPosition);
        Vector3 newPos = new Vector3(Mathf.Cos(angle) * distance, height,
                                    Mathf.Sin(angle) * distance);
        GameObject obj = Instantiate(enemyPrefab, newPos, Quaternion.identity);
        obj.GetComponentInChildren<EnemyMeshController>().SetOnDeathCallback(() =>
        {
          enemies.Remove(obj);
          Destroy(obj);
        });
        enemies.Add(obj);
      }
    }
  }

  void Restart()
  {
    counterText.text = "0";
    GameManager.Reset();
  }
}