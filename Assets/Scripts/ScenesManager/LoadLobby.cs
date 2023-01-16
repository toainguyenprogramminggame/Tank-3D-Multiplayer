using System.Collections;
using UnityEngine;
using Tank3DMultiplayer.SceneManage.Bootstrap;

namespace Tank3DMultiplayer.SceneManage
{
    public class LoadLobby : MonoBehaviour
    {
        void Start()
        {
            StartCoroutine(WaitToLoadLobby());
        }

        IEnumerator WaitToLoadLobby()
        {
            yield return new WaitUntil(() => LoadSceneManager.Instance != null && LoadDataFromServer.Instance != null
                                        && LoadDataFromServer.Instance.IsCompleteLoaded);

            // Load the menu
            LoadSceneManager.Instance.LoadScene(SceneName.Lobby, false);
        }
    }
}

