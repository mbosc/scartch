using System.Collections.Generic;
using System;
using UnityEngine;

namespace model
{
    public class Actor
    {

        private Vector3 position, rotation;
        private float scale, volume;
        private string message;
        private HashSet<Hat> hats;
        private HashSet<Block> blocks;
        private HashSet<Variable> variables;
        private Model model;
        private bool isMessageVisible;
        public event Action ActorChanged;

        public Actor(Vector3 position, Vector3 rotation, float scale, float volume, string message, Model model)
        {
            hats = new HashSet<Hat>();
            blocks = new HashSet<Block>();
            variables = new HashSet<Variable>();
            this.position = position;
            this.rotation = rotation;
            this.scale = scale;
            this.volume = volume;
            this.message = message;
            this.model = model;
        }

        public Actor(Model model)
            : this(new Vector3(0, 0, 0), new Vector3(0, 0, 0), 1, 75, "", model)
        {

        }

        public Vector3 Position
        {
            get
            {
                return position;
            }
            set
            {
                // prevent actor from exiting the playground
                if (Mathf.Abs(value.x) - Environment.MaxX > 0)
                    value.x = Environment.MaxX * Mathf.Sign(position.x);
                if(Mathf.Abs(value.z) - Environment.MaxZ > 0)
                    value.z = Environment.MaxZ * Mathf.Sign(position.z);
                if (value.y - Environment.MaxX > 0)
                    value.y = Environment.MaxY;
                if (value.y < 0)
                    value.y = 0;

                position = value;
                if (ActorChanged != null)
                    ActorChanged();
            }
        }

        public Vector3 Rotation
        {
            get
            {
                return rotation;
            }
            set
            {
                rotation = value;
                if (ActorChanged != null)
                    ActorChanged();
            }
        }

        public float Scale
        {
            get
            {
                return scale;
            }
            set
            {

                if (scale < 0)
                    // e poi questo chi lo cattura?
                    throw new ArgumentException("Scala negativa");
                // TODO verifica limiti del sistema
                scale = value;
                if (ActorChanged != null)
                    ActorChanged();
            }
        }

        public float Volume
        {
            get
            {
                return volume;
            }
            set
            {
                if (volume < 0 || volume > 100)
                    throw new ArgumentException("Volume must be in range [0, 100]");
                volume = value;
                if (ActorChanged != null)
                    ActorChanged();
            }
        }

        public string Message
        {
            get
            {
                return message;
            }
            set
            {
                if (message == null)
                    throw new ArgumentException("Message is null");
                message = value;
                if (ActorChanged != null)
                    ActorChanged();
            }
        }

        public void ShowMessage()
        {
            isMessageVisible = true;
            if (ActorChanged != null)
                ActorChanged();
        }

        public void HideMessage()
        {
            isMessageVisible = false;
            if (ActorChanged != null)
                ActorChanged();

        }

        public void AddHat(Hat hat)
        {
            hats.Add(hat);
        }
        public void RemoveHat(Hat hat)
        {
            hats.Remove(hat);
        }
        public void AddBlock(Block block)
        {
            blocks.Add(block);
        }
        public void RemoveBlock(Block block)
        {
            blocks.Remove(block);
        }
        public void AddVariable(Variable variable)
        {
            variables.Add(variable);
        }
        public void RemoveVariable(Variable variable)
        {
            variables.Remove(variable);
        }
        public Model Model
        {
            get
            {
                return model;
            }
            set
            {
                if (value == null)
                    throw new ArgumentException("Cannot assign a null model to an actor");
                model = value;
                if (ActorChanged != null)
                    ActorChanged();
            }
        }
    }
}