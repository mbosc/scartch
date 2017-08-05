using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Model
{
    public class Timer : MonoBehaviour
    {
        private Timer()
        {
            time = 0;
        }

        public static Timer instance;

        public static Timer Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Timer();
                }
                return instance;
            }
        }

        private float time;

        public float Time
        {
            get
            {
                return time;
            }
            set
            {
                time = value;
                if (TimeChanged != null)
                    TimeChanged();
            }
        }

        public bool Running { get; private set; }

        public event System.Action Started, Stopped, TimeChanged;

        public void DoStart()
        {
            if (!Running)
            {
                Reset();
                Running = true;
                if (Started != null)
                    Started();
            }
        }

        public void Reset()
        {
            Time = 0;
        }

        public void Stop()
        {
            if (Running)
            {
                Running = false;
                if (Stopped != null)
                    Stopped();
            }
        }

        public void Update()
        {
            if (Running)
            {
                time += UnityEngine.Time.deltaTime;
            }
        }
    }
}