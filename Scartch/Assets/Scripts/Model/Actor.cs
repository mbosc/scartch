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
            set
            {
                name = value;
                if (NameChanged != null)
                    NameChanged(name);
            }
        }

        public event System.Action<string> NameChanged;

        private Vector3 position = Vector3.zero;

        public Vector3 Position
        {
            get { return position; }
            set
            {
                var px = value.x > MaxX ? MaxX : (value.x < -MaxX ? -MaxX : value.x);
                var py = value.y > MaxY ? MaxY : (value.y < 0 ? 0 : value.y);
                var pz = value.z > MaxZ ? MaxZ : (value.z < -MaxZ ? -MaxZ : value.z);
                
                position = new Vector3(px, py, pz);

                if (Moved != null)
                    Moved(Position, Rotation);
            }
        }

        private Vector3 rotation = Vector3.zero;

        public Vector3 Rotation
        {
            get { return rotation; }
            set
            {
                var rx = value.x % 360;
                var ry = value.y % 360;
                var rz = value.z % 360;
                rotation = new Vector3(rx, ry, rz);
                if (Moved != null)
                    Moved(Position, Rotation);
            }
        }

        private float scale = 1;

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

        private float volume = 0.5f;

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

        private string message = "";

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

        private bool isMessageVisible = false;

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

        private ActorModel model = ActorModel.GetActorModel(0);

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

        public List<Variable> localVariables;

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
        public event System.Action<AudioClip> SoundPlayed;

        public void PlaySound(AudioClip clip)
        {
            if (SoundPlayed != null)
                SoundPlayed(clip);
        }

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
            var.Destroy();
            localVariables.Remove(var);
        }

        public void RemoveVariable(int num)
        {
            localVariables.RemoveAt(num);
        }

        public Variable GetVariable(int num)
        {
            return localVariables[num];
        }

        public int GetVariableNumber()
        {
            return localVariables.Count;
        }
    }
}