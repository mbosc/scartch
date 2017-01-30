using System.Collections;
using System;
using UnityEngine;

namespace model
{
    public class Environment : MonoBehaviour
    {

        #region Singleton Pattern Code
        private static Environment instance;

        

		void Start(){
			if (instance != null)
				Destroy (this);
			else
				instance = this;
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
                if (playMode && PlayModeStarted != null)
                    PlayModeStarted();
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

		void Update()
		{
			if (Input.GetKeyDown(KeyCode.P))
			{
				PlayMode = !PlayMode;
				Debug.Log("Play mode: " + PlayMode);
			}
		}

		public static Environment Instance {
			get {
				return instance;
			}
		}
    }
}