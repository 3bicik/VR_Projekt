using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
  public static bool EndGame { get; set; }
  public static int KillCounter { get; set; }

  public static void Reset()
  {
    EndGame = false;
    KillCounter = 0;
  }
}
