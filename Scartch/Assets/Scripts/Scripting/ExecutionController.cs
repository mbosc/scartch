﻿using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace Scripting
{
    public class ExecutionController : MonoBehaviour
    {
        private float delay = 1;

        public float Delay
        {
            get { return delay; }
            set { delay = value; }
        }


        private static ExecutionController instance;

        public static ExecutionController Instance
        {
            get { return instance; }
        }

        private void Start()
        {
            if (instance == null)
                instance = this;
            else
                Destroy(this.gameObject);
        }

        private Queue<Flux> fluxes;

        private ExecutionController()
        {
            fluxes = new Queue<Flux>();
        }

        public void AddFlux(Flux flux)
        {
            fluxes.Enqueue(flux);
        }

        private void ExecuteNext()
        {
            var flux = fluxes.Dequeue();
            Flux.current = flux;
            flux.CurrentBlock.Execute();
            if (flux.CurrentBlock != null)
            {
                fluxes.Enqueue(flux);
            }
            else
                flux.EndExecution();
        }

        public void Execute()
        {
            StartCoroutine(Run());
        }
        public void ExecuteTest()
        {
            StartCoroutine(Run(true));
        }

        public void Stop()
        {
            StopAllCoroutines();
            fluxes = new Queue<Flux>();
        }

        private IEnumerator Run(bool test = false)
        {
            while(fluxes.Count > 0)
            {
                ExecuteNext();
                yield return new WaitForSeconds(Delay);
            }
            Controller.EnvironmentController.Instance.ChangeMode(true);
        }
    }
}
