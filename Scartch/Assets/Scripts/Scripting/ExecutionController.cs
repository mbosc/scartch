using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace Scripting
{
    public class ExecutionController : MonoBehaviour
    {
        public float delay = 0.1f;

        public float Delay
        {
            get { return delay; }
            set { delay = value; }
        }
        public AuxiliaryTimer auxTimer;

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
            Debug.Log("   Added flux initiated by " + flux.initiator + " starting with " + flux.CurrentBlock);
            fluxes.Enqueue(flux);
        }

        private void ExecuteNext()
        {
            var flux = fluxes.Dequeue();
            Flux.current = flux;
            Debug.Log("Executing flux initiated by " + flux.initiator + "; Block " + flux.CurrentBlock);
            if (flux.CurrentBlock != null)
            {
                flux.CurrentBlock.Execute();
                fluxes.Enqueue(flux);
            }
            else
            {
                Debug.Log("   Flux initiated by " + flux.initiator + " terminates");
                flux.EndExecution();
            }
        }

        public void Execute()
        {
            Model.Timer.instance.DoStart();
            StartCoroutine(Run());
        }

        public void PauseFor(float time)
        {
            StopAllCoroutines();
            StartCoroutine(WaitAndReprise(time));
        }

        IEnumerator WaitAndReprise(float time)
        {
            yield return new WaitForSeconds(time);
            Execute();
        }

        public void Stop()
        {
            StopAllCoroutines();
            auxTimer.StopAllCoroutines();
            Model.Timer.instance.Stop();
            fluxes = new Queue<Flux>();
        }

        private IEnumerator Run()
        {
            while(fluxes.Count > 0)
            {
                ExecuteNext();
                yield return new WaitForSeconds(Delay);
            }
            Controller.EnvironmentController.Instance.ChangeMode();
        }
    }
}
