using UnityEngine;
using System.Collections.Generic;

public class EnemySystem : MonoBehaviour {
  public GameObject enemyPrefab;
  public GameObject thePlayer;

  public int maxEnemies = 1;
  public float minEnemyDistance = 10f;
  public float maxEnemyDistance = 100f;
  public float minEnemyHeightPosition = 10f;
  public float maxEnemyHeightPosition = 100f;
  private List<GameObject> enemies = new List<GameObject>();
  private static ILogger logger = Debug.unityLogger;
  
  private void OnEnemyDeath(GameObject enemy) {
    enemies.Remove(enemy);
  }

  void Update() {
    if(enemies.Count < maxEnemies && thePlayer) {
      float angle = UnityEngine.Random.Range(-Mathf.PI, Mathf.PI);
      float distance = UnityEngine.Random.Range(minEnemyDistance, maxEnemyDistance);
      float height = UnityEngine.Random.Range(minEnemyHeightPosition, maxEnemyHeightPosition);
      Vector3 newPos = new Vector3(Mathf.Cos(angle) * distance, height,
                                   Mathf.Sin(angle) * distance);
      GameObject obj = Instantiate(enemyPrefab, newPos, Quaternion.identity);
      obj.GetComponentInChildren<EnemyMeshController>().SetOnDeathCallback(() => {
        enemies.Remove(obj);
        Destroy(obj);
      });
      enemies.Add(obj);
    }
  }
}