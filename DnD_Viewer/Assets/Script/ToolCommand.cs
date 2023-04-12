using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
//using System.Diagnostics;

public enum ToolOption { PLAYER_OPTION = 0, ENEMY_OPTION = 1, EFFECT_RANGE_OPTION = 2, MAP_OPTION = 3, NONE_OPTION = 4 }

public class ToolCommand : MonoBehaviour
{
    [SerializeField]
    protected GameObject map;

    //protected static Dictionary<ToolOption , GameObject> toolOptionsMenu;

    [SerializeField]
    private GameObject toolsButtons;
    [SerializeField]
    private GameObject playerOptions;
    [SerializeField]
    private GameObject enemyOptions;
    [SerializeField]
    private GameObject mapOptions;

    [SerializeField]
    GameObject modify_button;
    protected bool isEventCalled = false;

    public delegate void OnModifyButtonClick();
    public static event OnModifyButtonClick onModifyPlayerButtonClick;
    public static event OnModifyButtonClick onModifyEnemyButtonClick;

    public virtual void Start()
    {
        if (modify_button != null)
        {
            modify_button.SetActive(false);
        }
        else
        {
            Debug.Log("Missing modifyButton field in" + this.name);
        }

        if (toolsButtons != null)
        {
            toolsButtons.SetActive(false);
        }
        else
        {
            Debug.Log("Missing toolsButton field in" + this.name);
        }
    }

    public virtual void Update()
    {
        if (GameMenager.CurrentSelectedCharacter != null)
        {
            modify_button.SetActive(true);
            modify_button.transform.GetChild(0).gameObject.SetActive(true);
        }
        else
        {
            modify_button.SetActive(false);
        }
    }

    public virtual void FakeStart()
    {
        gameObject.SetActive(true);
        GameMenager.SetGameIsPaused(true);
    }

    public void ShowAndHideToolsButton()
    {
        if (toolsButtons != null)
        {
            if (toolsButtons.activeInHierarchy)
            {
                toolsButtons.SetActive(false);
            }
            else
            {
                toolsButtons.SetActive(true);
            }
        }
    }


    public void CallModifyMenu()
    {
        GameMenager.SetGameIsPaused(true);

        if (GameMenager.CurrentSelectedCharacter.CompareTag("Player"))
        {
            onModifyPlayerButtonClick();

        }
        else
        {
            onModifyEnemyButtonClick();
        }
    }

    public virtual void DeleteCharacter()
    {
        CloseSubMenu();
    }

    protected virtual void ModifyMenu()
    {
        FakeStart();
    }

    public virtual void CloseSubMenu()
    {
        gameObject.SetActive(false);
        GameMenager.SetGameIsPaused(false);
    }
}

