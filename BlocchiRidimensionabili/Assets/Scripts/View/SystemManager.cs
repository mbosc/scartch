using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using model;

namespace view
{
    public class SystemManager : MonoBehaviour
    {

        private Environment env;

        // Use this for initialization
        void Start()
        {
            env = Environment.Instance;

        }

        
    }
}