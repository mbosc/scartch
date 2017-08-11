using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Model;

namespace View
{
    public class TimerWindow : Resources.VRWindow
    {
        private Timer timer;

        protected override void Start()
        {
            base.Start();
            timer = Timer.instance;
            timer.TimeChanged += UpdateMe;
        }

        private void UpdateMe()
        {
            
        }
    }
}
