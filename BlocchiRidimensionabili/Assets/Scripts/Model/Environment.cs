using System.Collections.Generic;
using System;
using UnityEngine;
using System.Collections;

namespace model
{
    public class Environment : MonoBehaviour
    {

        #region Singleton Pattern Code
        private static Environment instance;



        void Start() {
            if (instance != null)
                Destroy(this);
            else
                instance = this;
            PlayModeStarted += () => StartCoroutine(playRoutine(ExecutionSpeed));
        }
        #endregion

        public static int MaxX = 240, MaxZ = 240, MaxY = 180;
        public static int BorderTolerance = 15;

        private bool playMode = false;
        public bool PlayMode
        {
            get { return playMode; }
            set {
                if (value)
                    evaluationEngine = new EvaluationEngine();
                playMode = value;
                Debug.Log("Play mode: " + PlayMode);
                if (playMode && PlayModeStarted != null)
                    PlayModeStarted();
            }
        }

        public List<Actor> actors;

        public void Wait(float time)
        {
            StartCoroutine(waitRoutine(time));
        }

        private IEnumerator waitRoutine(float time)
        {
            yield return new WaitForSeconds(time);
        }

        public List<InteractionItem> controllers;

        public event Action PlayModeStarted;
        private EvaluationEngine evaluationEngine;
        public EvaluationEngine EvaluationEngine
        {
            get
            {
                return evaluationEngine;
            }
        }

        public float ExecutionSpeed;
        void Update()
        {
            //TODO Debug
            if (Input.GetKeyDown(KeyCode.P))
            {
                Environment.Instance.PlayMode = !Environment.Instance.PlayMode;
            }



        }

        private IEnumerator playRoutine(float executionSpeed)
        {
            while (playMode)
            {
                EvaluationEngine.ExecuteNext();
                yield return new WaitForSeconds(executionSpeed);
            }
        }

        public static Environment Instance {
			get {
				return instance;
			}
		}
    }
}