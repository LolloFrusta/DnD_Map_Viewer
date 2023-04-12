using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UIElements;

public class CameraControl : MonoBehaviour
{
    private Camera game_camera;
    private float scroll_speed = 5;
    RaycastHit2D hit;

    // Start is called before the first frame update
    void Start()
    {
        game_camera = GetComponent<Camera>();
    }


    // Update is called once per frame
    void Update()
    {
        if (!GameMenager.GetGameIsPaused())
        {
            if (game_camera.orthographic)
            {
                game_camera.orthographicSize -= Input.GetAxis("Mouse ScrollWheel") * scroll_speed;
                game_camera.orthographicSize = Mathf.Clamp(game_camera.orthographicSize , 2f , 30f);
            }
            else
            {
                game_camera.fieldOfView -= Input.GetAxis("Mouse ScrollWheel") * scroll_speed;
                game_camera.fieldOfView = Mathf.Clamp(game_camera.fieldOfView , 8f , 90);
            }
            if (Input.GetMouseButtonDown((int)MouseButton.RightMouse))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                hit = Physics2D.Raycast(ray.origin , ray.direction , 100f);
                if (hit.collider.CompareTag("Map"))
                {
                    GameMenager.CurrentSelectedCharacter = null;
                }
                else
                {
                    GameMenager.CurrentSelectedCharacter = hit.transform.gameObject;
                }

            }
        }
    }
}
