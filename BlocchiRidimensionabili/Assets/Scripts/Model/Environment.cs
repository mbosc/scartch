using System.Collections.Generic;
using System;
using UnityEngine;
using System.Collections;
using System.Linq;

namespace model
{
    public class Environment : MonoBehaviour
    {

        #region Singleton Pattern Code
        private static Environment instance;

		private model.Timer timer;
		public Timer Timer { get {return timer;}}

        void Start() {
            if (instance != null)
                Destroy(this);
            else
                instance = this;
            PlayModeStarted += () => StartCoroutine(playRoutine(StepDeltaTime));
            variables = new List<Variable>();
			actors = new List<Actor> ();
			controllers = new List<InteractionItem> ();
			timer = GetComponent<Timer> ();
		}
        #endregion

        public static int MaxX = 240, MaxZ = 240, MaxY = 180;
        public static int BorderTolerance = 15;

        private bool playMode = false;
        public bool PlayMode
        {
            get { return playMode; }
            set {
				if (value) {
					evaluationEngine = new EvaluationEngine ();
					timer.StartTimer ();
				} else {
					timer.StopTimer ();
				}
                playMode = value;
                Debug.Log("Play mode: " + PlayMode);
                if (playMode && PlayModeStarted != null)
                    PlayModeStarted();
            }
        }

        // TODO non in diagramma
        private IList<Variable> variables;
        public List<Variable> Variables
        {
            get
            {
                return variables.ToList();
            }
        }
        public void AddVariable(Variable var)
        {
            variables.Add(var);
        }
        public void RemoveVariable(Variable var)
        {
            variables.Remove(var);
        }
        
        private IList<Actor> actors;
		public List<Actor> Actors {
			get {
				return actors.ToList(); 
			}
		}
		public void AddActor(Actor act){
			actors.Add (act);
		}
		public void RemoveActor(Actor act){
			actors.Remove (act);
		}


        public void Wait(float secs)
        {
            StartCoroutine(waitRoutine(secs));
        }

        private IEnumerator waitRoutine(float time)
        {
            yield return new WaitForSeconds(time);
        }

        private IList<InteractionItem> controllers;
		public List<InteractionItem> Controllers {
			get {
				return controllers.ToList ();
			}
		}
        public event Action PlayModeStarted;
        private EvaluationEngine evaluationEngine;
        public EvaluationEngine EvaluationEngine
        {
            get
            {
                return evaluationEngine;
            }
        }

		// TODO mettere anche l'accessor qui (non lo faccio subito per facilitare il debug in editor
		public float StepDeltaTime;
        public NewtonVR.NVRHand manosx;

        void Update()
        {
            //TODO Debug
            if (Input.GetKeyDown(KeyCode.P))
            {
                Environment.Instance.PlayMode = !Environment.Instance.PlayMode;
            }
            if (manosx.Inputs[NewtonVR.NVRButtons.Y].PressDown)
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