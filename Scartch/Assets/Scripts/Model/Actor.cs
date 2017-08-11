using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Model
{
    public class Actor
    {
        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }


        private Vector3 position;

        public Vector3 Position
        {
            get { return position; }
            set
            {
                position = value;
                if (Moved != null)
                    Moved(Position, Rotation);
            }
        }

        private Vector3 rotation;

        public Vector3 Rotation
        {
            get { return rotation; }
            set
            {
                rotation = value;
                if (Moved != null)
                    Moved(Position, Rotation);
            }
        }

        private float scale;

        public float Scale
        {
            get { return scale; }
            set
            {
                scale = value;
                if (ScaleChanged != null)
                    ScaleChanged(Scale);
            }
        }

        private float volume;

        public float Volume
        {
            get { return volume; }
            set
            {
                volume = value;
                if (VolumeChanged != null)
                    VolumeChanged(Volume);
            }
        }

        private string message;

        public string Message
        {
            get { return message; }
            set
            {
                message = value;
                if (MessageChanged != null)
                    MessageChanged(IsMessageVisible, Message);
            }
        }

        private bool isMessageVisible;

        public bool IsMessageVisible
        {
            get { return isMessageVisible; }
            set
            {
                isMessageVisible = value;
                if (MessageChanged != null)
                    MessageChanged(IsMessageVisible, Message);
            }
        }

        private ActorModel model;

        public ActorModel Model
        {
            get { return model; }
            set
            {
                model = value;
                if (ModelChanged != null)
                    ModelChanged(Model);
            }
        }

        private List<Variable> localVariables;

        public static int MaxX = 240, MaxY = 180, MaxZ = 240, BorderTolerance = 15;

        public Actor()
        {
            localVariables = new List<Variable>();
        }

        public event System.Action<Vector3, Vector3> Moved;
        public event System.Action<float> ScaleChanged, VolumeChanged;
        public event System.Action<bool, string> MessageChanged;
        public event System.Action<ActorModel> ModelChanged;
        public event System.Action Destroyed;

        public void Destroy()
        {
            if (Destroyed != null)
                Destroyed();
        }

        public void AddVariable(Variable var)
        {
            localVariables.Add(var);
        }

        public void RemoveVariable(Variable var)
        {
            localVariables.Remove(var);
        }
    }
}