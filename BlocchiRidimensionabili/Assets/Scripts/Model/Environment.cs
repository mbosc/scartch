using System.Collections;
using System;
namespace model
{
    public class Environment
    {

        #region Singleton Pattern Code
        private static Environment instance;

        private Environment() { }

        public static Environment Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Environment();
                }
                return instance;
            }
        }
        #endregion

        public static int MaxX = 240, MaxZ = 240, MaxY = 180;

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
    }
}