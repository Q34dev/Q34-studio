using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cutscene : MonoBehaviour
{
    private SceneLoader sceneLoader;

    private SpriteRenderer sRenderer;

    [SerializeField] private Sprite[] images;

    private int index;

    void Start()
    {
        sceneLoader = GetComponent<SceneLoader>();

        sRenderer = GetComponent<SpriteRenderer>();

        index = 0;
    }

    public void NextImage()
    {
        if (index < images.Length - 1)
        {
            sRenderer.sprite = images[index + 1];

            index++;
        }
        else
        {
            EndCutscene();
        }
    }

    public void EndCutscene()
    {
        sceneLoader.LoadNextScene();
    }
}
