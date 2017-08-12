using System;
using System.Collections;
using System.Collections.Generic;
using Model;
using UnityEngine;

namespace View
{
    public class ActorViewer : Resources.RayHittable
    {
        private Model.Actor actor;
        public event Action Selected, Destroyed;
        private GameObject spawn;
        public GameObject model;
        public GameObject message;
        public UnityEngine.UI.Text messageText;
        public AudioSource audioSource;
        public GameObject highlight;

        public void Init(Model.Actor actor, GameObject spawn)
        {
            this.actor = actor;
            this.spawn = GameObject.Instantiate(spawn);
            this.spawn.transform.SetParent(spawn.transform.parent, false);
            transform.SetParent(this.spawn.transform, false);
            transform.localPosition = transform.localEulerAngles = Vector3.zero;
            transform.localScale = Vector3.one;

            actor.Destroyed += Actor_Destroyed;
            actor.MessageChanged += Actor_MessageChanged;
            actor.Moved += Actor_Moved;
            actor.ModelChanged += Actor_ModelChanged;
            actor.ScaleChanged += Actor_ScaleChanged;
            actor.VolumeChanged += Actor_VolumeChanged;
            actor.SoundPlayed += Actor_SoundPlayed;

            message.SetActive(actor.IsMessageVisible);
            messageText.text = actor.Message;
            if (actor.Volume > 1)
                actor.Volume = 1;
            if (actor.Volume < 0)
                actor.Volume = 0;
            audioSource.volume = actor.Volume;
            if (model != null)
                Destroy(model);
            model = GameObject.Instantiate(ScartchResourceManager.instance.modelPrefabs[ActorModel.IndexOfModel(actor.Model)]);
            model.transform.SetParent(transform, false);
            transform.localPosition = actor.Position;
            transform.localEulerAngles = actor.Rotation;
            model.transform.localScale = Vector3.one * actor.Scale;

            Highlighted = false;
        }

        private void Actor_SoundPlayed(AudioClip obj)
        {
            audioSource.clip = obj;
            audioSource.Play();
        }

        private void OnDestroy()
        {
            actor.Destroyed -= Actor_Destroyed;
            actor.MessageChanged -= Actor_MessageChanged;
            actor.Moved -= Actor_Moved;
            actor.ModelChanged -= Actor_ModelChanged;
            actor.ScaleChanged -= Actor_ScaleChanged;
            actor.VolumeChanged -= Actor_VolumeChanged;
            actor.SoundPlayed -= Actor_SoundPlayed;

        }

        private void Actor_VolumeChanged(float obj)
        {
            if (obj > 1)
                obj = 1;
            if (obj < 0)
                obj = 0;
            audioSource.volume = obj;
        }

        private void Actor_ScaleChanged(float obj)
        {
            transform.localScale = Vector3.one * obj;
            Actor_Moved(actor.Position, actor.Rotation);
        }

        private void Actor_ModelChanged(ActorModel obj)
        {
            if (model != null)
                Destroy(model);
            model = GameObject.Instantiate(ScartchResourceManager.instance.modelPrefabs[ActorModel.IndexOfModel(obj)]);
            model.transform.SetParent(transform, false);
            model.transform.localPosition = Vector3.zero;
            model.transform.localEulerAngles = Vector3.zero;
            model.transform.localScale = Vector3.one;
        }

        private void Actor_Moved(Vector3 arg1, Vector3 arg2)
        {
            spawn.transform.localPosition = arg1 + new Vector3(0,23*actor.Scale,0);
            transform.localEulerAngles = arg2;
        }

        private void Actor_MessageChanged(bool arg1, string arg2)
        {
            message.SetActive(arg1);
            messageText.text = arg2;
        }

        private void Actor_Destroyed()
        {
            Destroy(this.gameObject);
        }

        public override void HitByBlueRay()
        {
            if (Selected != null)
                Selected();
        }

        public override void HitByRedRay()
        {
            if (Destroyed != null)
                Destroyed();
        }

        private bool highLighted;

        public bool Highlighted
        {
            get { return highLighted; }
            set { highLighted = value;
                highlight.SetActive(value);
            }
        }

    }
}
