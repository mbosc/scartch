using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace model
{
    public class EvaluationEngine
    {
        private Queue<Block> fluxes;
        private Dictionary<string, List<Hat>> subscribers;

        public EvaluationEngine()
        {
            fluxes = new Queue<Block>();
            subscribers = new Dictionary<string, List<Hat>>();
        }

        public void AddSubscriber(string message, Hat hat)
        {
            if (!subscribers.ContainsKey(message))
                subscribers.Add(message, new List<Hat>());

            subscribers[message].Add(hat);
        }

        public void RemoveSubscriber(string message, Hat hat)
        {
            try
            {
                subscribers[message].Remove(hat);
            }
            catch { }
        }

        public void EvaluateMessage(string message)
        {
            List<Hat> outl = new List<Hat>();
            subscribers.TryGetValue(message, out outl);
            outl.ForEach(s => s.Execute());
        }

        public void AddFlux(Block block)
        {
            fluxes.Enqueue(block);
        }


        //Questa funzione viene chiamata dal sistema a divisione di tempo.
        public void ExecuteNext()
        {
            if (fluxes.Count == 0)
                // no blocks to execute
                return;

            //Execute current block
            Block activeBlock = fluxes.Dequeue();
            activeBlock = activeBlock.ExecuteAndGetNext();

            //Get next block
            if (activeBlock != null)
                fluxes.Enqueue(activeBlock);
        }
    }
}

