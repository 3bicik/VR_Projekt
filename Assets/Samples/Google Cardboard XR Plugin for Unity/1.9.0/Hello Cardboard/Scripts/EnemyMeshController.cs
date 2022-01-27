//-----------------------------------------------------------------------
// <copyright file="ObjectController.cs" company="Google LLC">
// Copyright 2020 Google LLC
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>
//-----------------------------------------------------------------------


using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Timers;
using System.Threading.Tasks;

/// <summary>
/// Controls target objects behaviour.
/// </summary>
public class EnemyMeshController : MonoBehaviour
{
  public GameObject killCounter;
  public GameObject thePlayer;
  public Text counterText;
  private static ILogger logger = Debug.unityLogger;

  public float TOTAL_SHOOTING_TIME = 3;
  public float TOTAL_DYING_TIME = 5;
  private float elapsedShootingTime = 0;
  private float elapsedDyingTime = 0;
  private bool isObjectInCamera = false;
  private bool isShot = false;

  private GameObject imageObject = null;
  private Image shootingProgress = null;

  private SkinnedMeshRenderer mySkinnedMeshRenderer;
  private Animator myAnimator;
  public delegate void OnDeath();
  public OnDeath onDeathCallback;

  private float moveSpeed = 1f;

  /// <summary>
  /// Start is called before the first frame update.
  /// </summary>
  public void Start()
  {
    mySkinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
    myAnimator = GetComponentInParent<Animator>();
    thePlayer = GameObject.Find("Player");
    killCounter = GameObject.Find("KillCounter");
    counterText = killCounter.GetComponent<Text>();
    LookAtThePlayer();
  }

  public void Update()
  {

    imageObject = GameObject.FindGameObjectWithTag("ShootingProgress");
    if (imageObject != null)
    {
      shootingProgress = imageObject.GetComponent<Image>();
    }

    if (isObjectInCamera == true && isShot == false)
    {
      shootingProgress.fillAmount = (TOTAL_SHOOTING_TIME - elapsedShootingTime) / TOTAL_SHOOTING_TIME;
      elapsedShootingTime += Time.deltaTime;
    }
    if (elapsedShootingTime >= TOTAL_SHOOTING_TIME)
    {
      isShot = true;
      elapsedShootingTime = 0;
      myAnimator.Play("Die");
    }
    if (isShot == true)
    {
      elapsedDyingTime += Time.deltaTime;
    }
    else
    {
      if (transform.parent.position != thePlayer.transform.position)
      {
        transform.parent.position = Vector3.MoveTowards(transform.parent.position, thePlayer.transform.position, moveSpeed * Time.deltaTime);
      }
      else
      {
        Debug.Log("PLAYER DEAD");
        GameManager.EndGame = true;
      }
    }
    if (elapsedDyingTime >= TOTAL_DYING_TIME)
    {
      isShot = false;
      elapsedDyingTime = 0;
      GameManager.KillCounter++;
      counterText.text = GameManager.KillCounter.ToString();
      myAnimator.Play("IdleBattle");
      onDeathCallback();
    }
  }

  public void OnPointerEnter()
  {
    isObjectInCamera = true;
  }

  public void OnPointerExit()
  {
    isObjectInCamera = false;
  }

  private void LookAtThePlayer()
  {
    transform.parent.LookAt(GameObject.Find("Player").transform);
  }

  public void SetOnDeathCallback(OnDeath callback)
  {
    onDeathCallback = callback;
  }

  public void SetThePlayer(GameObject thePlayer)
  {
    logger.Log("Player set");
    this.thePlayer = thePlayer;
    LookAtThePlayer();
  }
}
