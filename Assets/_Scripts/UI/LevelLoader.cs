using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Playables;

public class LevelLoader : MonoBehaviour
{
    public PlayableDirector playableDirector;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && RotatingTriangle.LevelSelected != -1) 
        {
            LoadLevel((int)RotatingTriangle.LevelSelected);
        }
    }

    public void LoadLevel(int sceneIndex)
    {
        playableDirector.Play();

        StartCoroutine(LoadAsynchronously(sceneIndex));
    }

    private IEnumerator LoadAsynchronously(int scenenIndex)
    {
        yield return new WaitForSeconds((float)playableDirector.duration);
        AsyncOperation operation = SceneManager.LoadSceneAsync(scenenIndex);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);

            yield return null;
        }
    }
}
