using System.Collections;
using System.Collections.Generic;
using TMPro;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEngine.Networking;

public class Character_ToolCommand : ToolCommand
{

    protected string characterTexturePath;
    protected string characterTextDir;
    protected DirectoryInfo characterTextureDirectory;
    protected List<Sprite> character_sprites;
    protected Vector2 sprite_size;

    [SerializeField]
    protected GameObject character_Prefab;
    protected GameObject tmp_character;

    [SerializeField]
    protected TMP_Dropdown sprites_Dropdown;
    protected List<TMP_Dropdown.OptionData> spritesOptions = new List<TMP_Dropdown.OptionData>();
    protected TMP_Dropdown.OptionData spriteOption;

    [SerializeField]
    protected TMP_InputField charcterName_InputField;

    [SerializeField]
    protected Slider characterSize_Slider;

    protected GameObject characterText;
    protected GameObject character_sprite;

    [SerializeField]
    protected ToolOption characterToolOption;


    public override void Start()
    {
        if (characterToolOption == ToolOption.PLAYER_OPTION)
        {
            characterTextDir = "PlayerSprites";
            ToolCommand.onModifyPlayerButtonClick += ModifyMenu;
        }
        else if (characterToolOption == ToolOption.ENEMY_OPTION)
        {
            characterTextDir = "EnemySprites";
            ToolCommand.onModifyEnemyButtonClick += ModifyMenu;
        }

        characterTexturePath = "Assets/Resources/" + characterTextDir;
        if (!Directory.Exists(characterTexturePath))
        {
            Directory.CreateDirectory(characterTexturePath);
        }

        character_sprites = new List<Sprite>();
        sprites_Dropdown.ClearOptions();

        string[] files = Directory.GetFiles(characterTexturePath , "*");
        foreach (string file in files)
        {
            
            if (file.ToLower().EndsWith(".jpg") || file.EndsWith(".jpeg") || file.EndsWith(".png"))
            {
                WWW www = new WWW("file:///" + Path.GetFullPath(file));

                if (www != null)
                {

                    spriteOption = new TMP_Dropdown.OptionData();
                    string[] fullFileName = file.Split('\\');
                    spriteOption.text = fullFileName[fullFileName.Length - 1].Split('.')[0];                    
                    spritesOptions.Add(spriteOption);

                    Texture2D tex = www.texture;
                    Sprite tmp_sprite = Sprite.Create(tex , new Rect(0f , 0f , tex.width , tex.height) , new Vector2(0.5f , 0.5f));
                    character_sprites.Add(tmp_sprite);
                }
            }
        }

        sprites_Dropdown.AddOptions(spritesOptions);

        gameObject.SetActive(false);
    }


    public override void FakeStart()
    {
        base.FakeStart();
        InstantiateCharacter();
        tmp_character.transform.position = new Vector3(0 , 0 , -1);
        FillCharacterField();
        ChangePlayerSize();
        ChangePlayerSprite();
    }

    private void FillCharacterField()
    {
        sprite_size = tmp_character.GetComponentInChildren<SpriteRenderer>().sprite.bounds.size;
        character_sprite = tmp_character.transform.GetChild(0).gameObject;
        characterText = tmp_character.transform.GetChild(1).gameObject;
    }

    private void InstantiateCharacter()
    {
        tmp_character = GameObject.Instantiate(character_Prefab , transform);
        if (tmp_character == null)
        {
            Debug.Log("Null Character, MissingPrefab");
        }
    }
    protected override void ModifyMenu()
    {
        if (!gameObject.activeInHierarchy)
        {
            Debug.Log("ModifyMenu Start");
            gameObject.SetActive(true);
            GameMenager.SetGameIsPaused(true);
            tmp_character = GameMenager.CurrentSelectedCharacter;
            FillCharacterField();
            if (tmp_character.CompareTag("Player"))
            {
                characterToolOption = ToolOption.PLAYER_OPTION;
            }
            else
            {
                characterToolOption = ToolOption.ENEMY_OPTION;
            }
        }
    }

    public void ChangePlayerName()
    {
        tmp_character.GetComponentInChildren<TextMeshPro>().text = charcterName_InputField.text;
    }

    public void ChangePlayerSize()
    {

        character_sprite.transform.localScale = new Vector3(characterSize_Slider.value , characterSize_Slider.value , 1);
        characterText.transform.position = character_sprite.transform.position + new Vector3(0 , 0.5f + (sprite_size.y * 0.5f * character_sprite.transform.localScale.y) , 0);
    }

    public void ChangePlayerSprite()
    {
        Sprite tmp_sprite = character_sprites[sprites_Dropdown.value];
        tmp_character.GetComponentInChildren<SpriteRenderer>().sprite = character_sprites[sprites_Dropdown.value];
        sprite_size = tmp_character.GetComponentInChildren<SpriteRenderer>().sprite.bounds.size;
        characterText.transform.position = character_sprite.transform.position + new Vector3(0 , 0.5f + (sprite_size.y * 0.5f * character_sprite.transform.localScale.y) , 0);
    }

    public override void DeleteCharacter()
    {
        if (tmp_character != null)
        {
            GameObject.Destroy(tmp_character);
        }
        base.DeleteCharacter();
    }

    public override void CloseSubMenu()
    {
        tmp_character.transform.SetParent(map.transform);
        ResizeBoxCollider();
        base.CloseSubMenu();
    }

    void ResizeBoxCollider()
    {
        sprite_size = tmp_character.GetComponentInChildren<SpriteRenderer>().sprite.bounds.size;
        tmp_character.GetComponent<BoxCollider2D>().size = sprite_size * tmp_character.transform.GetChild(0).localScale;
    }
}