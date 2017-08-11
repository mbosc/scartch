﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Model;

namespace View
{
    public class TimerWindow : Resources.VRWindow
    {
        private Timer timer;
        public UnityEngine.UI.Text value;

        protected override void Start()
        {
            base.Start();
            timer = Timer.instance;
            timer.TimeChanged += UpdateMe;
        }

        private void UpdateMe()
        {
            value.text = timer.Time.ToString("N2");
        }

        protected override void Update()
        {
            base.Update();
            if (Input.GetKey(KeyCode.S))
                timer.DoStart();
        }
    }
}
