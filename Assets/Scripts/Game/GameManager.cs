using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ColorFill.Events;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ColorFill
{
    [ExecuteAlways]
    public class GameManager : MonoBehaviour
    {
        public EmptyEventChannel OnGameLoaded;
        public IntEventChannel OnLevelLoaded;
        public EmptyEventChannel OnLevelCompleted;
        public EmptyEventChannel OnMapInitializationFinished;

        public List<Map> MapList;

        public float LevelLoadDelay = 2.0f;
        public float LevelRestartDelay = 1.0f;
        
        private Map _currentMap;
        private int _currentMapIndex;
        
        void Start()
        {
            if (Application.isPlaying)
            {
                _currentMapIndex = 0;
                
                OnGameLoaded?.Raise();
                
                OnLevelLoaded.Raise(PlayerStats.Level);
                
                StartCoroutine(WaitForAllMapToStartInitialize());
            }
        }

        void StartGame()
        {
            _currentMap = MapList[_currentMapIndex];
            
            MapList.ForEach(map => { if (map != _currentMap) map.gameObject.SetActive(false); });

            _currentMap.StartMap();
        }

        public void RestartGame()
        {
            StartCoroutine(ReloadScene());
        }

        public void StartNewMap()
        {
            if (MapList.TrueForAll(m => m.IsCompleted))
            {
                OnLevelCompleted.Raise();
                PlayerStats.Level++;
                StartCoroutine(LoadNextScene());
            }
            else
                StartCoroutine(GoToNextMap());
        }

        IEnumerator WaitForAllMapToStartInitialize()
        {
            yield return new WaitUntil(() => MapList.TrueForAll((map) => map.StartInitialization));
            // For the starting lag when you play from the editor
#if UNITY_EDITOR
            yield return new WaitForSeconds(0.5f);
#endif

            OnMapInitializationFinished.Raise();
            StartGame();
        }

        IEnumerator ReloadScene()
        {
            yield return new WaitForSeconds(LevelRestartDelay);
            Scene currentScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(currentScene.name);
        }

        IEnumerator LoadNextScene()
        {
            yield return new WaitForSeconds(LevelLoadDelay);
            
            string newSceneName = $"Level{PlayerStats.Level}";
            SceneManager.LoadScene(newSceneName);
        }

        IEnumerator GoToNextMap()
        {
            yield return new WaitForSeconds(1.0f);
            
            _currentMapIndex++;
            for (int i = 0; i < MapList.Count; i++)
            {
                if (i == _currentMapIndex)
                    _currentMap = MapList[i];
                else
                    StartCoroutine(DisableMapAfterSeconds(MapList[i], 0.5f));
            }

            _currentMap.gameObject.SetActive(true);
            _currentMap.StartMap();
            _currentMap.SetActiveMap(true);
        }

        IEnumerator DisableMapAfterSeconds(Map m, float seconds)
        {
            yield return new WaitForSeconds(seconds);
            m.gameObject.SetActive(false);
        }
    } 
}
