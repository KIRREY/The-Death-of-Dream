using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

public class ObjectManager : Singleton<ObjectManager>
{
    public Dictionary<string, bool> objectActive = new Dictionary<string, bool>();

    private void OnEnable()
    {
        EventHandler.BeforeSceneChangeEvent += OnBeforeSceneChangeEvent;
        EventHandler.AfterSceneChangeEvent += OnAfterSceneChangeEvent;
    }

    private void OnDisable()
    {
        EventHandler.BeforeSceneChangeEvent -= OnBeforeSceneChangeEvent;
        EventHandler.AfterSceneChangeEvent -= OnAfterSceneChangeEvent;
    }

    private void OnAfterSceneChangeEvent()
    {
        foreach (var _object in Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[])
        {
            if (_object.gameObject.scene != SceneManager.GetActiveScene())
                continue;
            if (objectActive.ContainsKey(_object.name))
            {
                _object.gameObject.SetActive(objectActive[_object.name]);
                Debug.Log(_object.name + objectActive[_object.name]);
            }
        }
    }

    private void OnBeforeSceneChangeEvent()
    {
        if (PlayerManager.Instance.ifChasing)
            EventHandler.CallExitChasingEvent(false);
        foreach (var _object in Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[])
        {
            if (_object.gameObject.scene != SceneManager.GetActiveScene())
                continue;
            if (objectActive.ContainsKey(_object.name))
            {
                objectActive[_object.name] = _object.gameObject.activeInHierarchy;
            }
            else
            {
                objectActive.Add(_object.name, _object.gameObject.activeInHierarchy);
            }
        }
    }

    private void Start()
    {
        objectActive.Clear();
    }
}
