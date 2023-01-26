
using System;
using System.Collections;
using Tank3DMultiplayer.Support;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Tank3DMultiplayer.SceneManage
{
    public class LoadSceneManager : SingletonPersistent<LoadSceneManager>
    {
        public SceneName SceneActive => m_sceneActive;
        private SceneName m_sceneActive;


        // After running the menu scene, which initiates this manager, we subscribe to these events
        // due to the fact that when a network session ends it cannot longer listen to them.
        public void Init()
        {
            //NetworkManager.Singleton.SceneManager.OnLoadComplete -= OnLoadComplete;
            //NetworkManager.Singleton.SceneManager.OnLoadComplete += OnLoadComplete;
        }


        public void LoadScene(SceneName sceneToLoad, bool isNetworkSessionActive = false)
        {
            StartCoroutine(Loading(sceneToLoad, isNetworkSessionActive));
        }

        // Coroutine for the loading effect. It use an alpha in out effect
        private IEnumerator Loading(SceneName sceneToLoad, bool isNetworkSessionActive)
        {
            if (isNetworkSessionActive)
            {
                if (NetworkManager.Singleton.IsServer)
                    LoadSceneNetwork(sceneToLoad);
            }
            else
            {
                LoadSceneLocal(sceneToLoad);
            }

            // Because the scenes are not heavy we can just wait a second and continue with the fade.
            // In case the scene is heavy instead we should use additive loading to wait for the
            // scene to load before we continue
            yield return new WaitForSeconds(1f);
        }


        // Load the scene using the regular SceneManager, use this if there's no active network session
        private void LoadSceneLocal(SceneName sceneToLoad)
        {

            SceneManager.LoadScene(sceneToLoad.ToString());
            //switch (sceneToLoad)
            //{
            //    case SceneName.Menu:
            //        if (AudioManager.Instance != null)
            //            AudioManager.Instance.PlayMusic(AudioManager.MusicName.intro);
            //        break;
            //}
        }

        // Load the scene using the SceneManager from NetworkManager. Use this when there is an active
        // network session
        private void LoadSceneNetwork(SceneName sceneToLoad)
        {
            NetworkManager.Singleton.SceneManager.LoadScene(sceneToLoad.ToString(), LoadSceneMode.Single);
        }

        //// This callback function gets triggered when a scene is finished loading
        //// Here we set up what to do for each scene, like changing the music
        //private void OnLoadComplete(ulong clientId, string sceneName, LoadSceneMode loadSceneMode)
        //{
        //    if (!NetworkManager.Singleton.IsServer)
        //        return;

        //    Enum.TryParse(sceneName, out m_sceneActive);

        //    if (m_sceneActive == SceneName.Room)
        //    {
        //        //if (!RoomManager.Instance.CanClientConnect(clientId))
        //        //    return;
        //    }

        //    // What to initially do on every scene when it finishes loading
        //    switch (m_sceneActive)
        //    {
        //        // When a client/host connects tell the manager
        //        case SceneName.Room:
        //            RoomManager.Instance.OnJoinedRoom(clientId);
        //            break;

        //        case SceneName.CharacterSelection:
        //            // Do Something
        //            break;

        //        case SceneName.GamePlay:
        //            GamePlayManager.Instance.InitGameplayScene(clientId);
        //            break;

        //        default:
        //            break;

        //    }
        //}
    }
}

