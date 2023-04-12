using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class ChangeMap : ToolCommand
{
    protected string mapTexturePath;
    protected string mapTextDir = "Maps";
    protected DirectoryInfo mapTextureDirectory;
    protected List<Sprite> map_sprites;

    GameObject currentMap;
    GameObject mapSprite;

    [SerializeField]
    protected Slider mapSize_slider;

    [SerializeField]
    protected TMP_Dropdown map_sprites_dropdown;
    protected List<TMP_Dropdown.OptionData> map_sprites_options = new List<TMP_Dropdown.OptionData>();
    protected TMP_Dropdown.OptionData map_sprite_option;

  
    public override void Start()
    {
        mapTexturePath = "Assets/Resources/" + mapTextDir;
        mapTextureDirectory = new DirectoryInfo(mapTexturePath);
        map_sprites = new List<Sprite>();
        map_sprites_dropdown.ClearOptions();

        if (!Directory.Exists(mapTexturePath))
        {
            Directory.CreateDirectory(mapTexturePath);
        }

        string[] files = Directory.GetFiles(mapTexturePath , "*");
        foreach (string file in files)
        {
            if (file.ToLower().EndsWith(".jpg") || file.ToLower().EndsWith(".jpeg") || file.EndsWith(".png"))
            {
                Debug.Log(Path.GetFullPath(file));
                WWW www = new WWW("file:///" + Path.GetFullPath(file));

                if (www != null)
                {
                    map_sprite_option = new TMP_Dropdown.OptionData();
                    string[] fullFileName = file.Split('\\');
                    map_sprite_option.text = fullFileName[fullFileName.Length - 1].Split('.')[0];
                    map_sprites_options.Add(map_sprite_option);

                    Texture2D tex = www.texture;
                    Sprite tmp_sprite = Sprite.Create(tex , new Rect(0f , 0f , tex.width , tex.height) , new Vector2(0.5f , 0.5f));

                    map_sprites.Add(tmp_sprite);
                }
            }
        }

        map_sprites_dropdown.AddOptions(map_sprites_options);
        currentMap = map;
        gameObject.SetActive(false);
    }

    public void ChangeMapSprite()
    {
        Sprite tmp_sprite = map_sprites[map_sprites_dropdown.value];
        currentMap.GetComponent<SpriteRenderer>().sprite = tmp_sprite;
    }

    public void ChangeMapSize()
    {
        currentMap.transform.localScale = new Vector3(mapSize_slider.value , mapSize_slider.value , 1);
    }
}
