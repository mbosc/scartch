using System.Collections;
using System;
using UnityEngine;

namespace model
{
    public class Timer : MonoBehaviour
    {
		private bool running;
		public bool Running {
			get { return running; }
		}
		private float startTime;
		private float time;
		public float Time {
			get {
				return time;
			}
		}

		public void StartTimer(){
			time = 0;
			startTime = UnityEngine.Time.time;
			running = true;
		}
		void Update(){
			if (running)
				time = UnityEngine.Time.time - startTime;
		}
		public void ResetTimer(){
			running = false;
		}
    }
}