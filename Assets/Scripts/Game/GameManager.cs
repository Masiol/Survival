using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ResourcesDataBase))]
[RequireComponent(typeof(GameOptions))]
public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    [HideInInspector] public GameOptions options = null;
    [HideInInspector] public ResourcesDataBase resources = null;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            options = GetComponent<GameOptions>();
            resources = GetComponent<ResourcesDataBase>();
            resources.Init();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        SetCursor(false);
    }

    public void SetCursor(bool _visible)
    {
        Cursor.visible = _visible;
        Cursor.lockState = (_visible) ? CursorLockMode.None : CursorLockMode.Locked;
    }

    public void StopPlayer()
    {

    }
}
