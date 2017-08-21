using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Model;

namespace View
{
    public class TimerWindow : Resources.VRWindow
    {
        private Timer timer;
        public UnityEngine.UI.Text value;

        public void Init()
        {
            timer = Timer.instance;
            timer.TimeChanged += UpdateMe;
        }

        private void UpdateMe()
        {
            value.text = timer.Time.ToString("N2");
        }
    }
}
