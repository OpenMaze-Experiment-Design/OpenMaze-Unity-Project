﻿using System;
using System.Collections;
using data;
using unity.wrapper;
using UnityEngine;

namespace loading
{
    public class LoadingService : ILoadingService
    {
        private readonly Data _data;
        private static ILoadingService _loadingService;
        private readonly ISceneWrapper _sceneWrapper;
        private bool _doneTransitioning;

        // Singleton service
        public static ILoadingService Create()
        {
            return _loadingService ?? (_loadingService = new LoadingService(
                       DataSingleton.GetData(),
                       SceneWrapper.Create()));
        }

        private LoadingService(Data data, ISceneWrapper sceneWrapper)
        {
            _data = data;
            _sceneWrapper = sceneWrapper;
        }

        public bool IsLoading()
        {
            return !_doneTransitioning;
        }

        public void TransitionNextSceneWithDelay(int sceneNumber)
        {
            _sceneWrapper.SwitchScene(BeginLoad(sceneNumber));
        }

        /**
         * This function will try to load a scene
         */
        private IEnumerator BeginLoad(int sceneNumber)
        {
            var entryTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            _doneTransitioning = false;

            if (sceneNumber != Constants.LoadingScreen){
                _sceneWrapper.LoadScene("StopGapLoad");
            } else {
                _sceneWrapper.LoadScene(sceneNumber);
            }

            _sceneWrapper.FreezeTime();
            yield return null;
            
            AsyncOperation scenePromise = null;

            if (sceneNumber != Constants.LoadingScreen){
                scenePromise = _sceneWrapper.LoadAsyncScene(sceneNumber);
                scenePromise.allowSceneActivation = false;
            }
            
            var loadingDelta = DateTimeOffset.Now.ToUnixTimeMilliseconds() - entryTime;

            
            while (loadingDelta < _data.MinLoadMsDelay)
            {
                loadingDelta = DateTimeOffset.Now.ToUnixTimeMilliseconds() - entryTime;

                if (loadingDelta > _data.MinLoadMsDelay - 150 && scenePromise != null)
                {
                    scenePromise.allowSceneActivation = true;
                }
                else
                {
                    yield return null;
                }
            }
            
            loadingDelta = DateTimeOffset.Now.ToUnixTimeMilliseconds() - entryTime;

            if (loadingDelta > _data.MinLoadMsDelay * 1.1)
            {
                if (_data.IgnoreMinLoadMsDelay)
                {
                    Debug.Log("Note, your MinLoadMsDelay has been reached and the flag " +
                              "IgnoreMinLoadMsDelay has been set. " +
                              "If this is unintended, please re-tune the value in your config file.");
                }
                else
                {

                    Debug.LogError("The minimum loading delay has been exceeded. The current delay is " +
                                   $"{_data.MinLoadMsDelay} but the loading took {loadingDelta}. Please re-tune " +
                                       "LoadMinMsDelay in the config file or set the flag IgnoreLoadMinMsDelay to true");
                    #if UNITY_EDITOR
                        UnityEditor.EditorApplication.isPlaying = false;
                    #endif
                }

            }

            _doneTransitioning = true;
            _sceneWrapper.RestartTime();
        }
    }
}