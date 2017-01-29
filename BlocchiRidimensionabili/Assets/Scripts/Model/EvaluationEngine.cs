using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace model
{
    public class EvaluationEngine
    {
        private Queue<Block> fluxes;

        public EvaluationEngine()
        {
            fluxes = new Queue<Block>();
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
            Block prioritaryBlock = fluxes.Dequeue();
            prioritaryBlock.Execute();

            //Get next block
            if (prioritaryBlock.Next != null)
                fluxes.Enqueue(prioritaryBlock.Next);
        }
    }
}

