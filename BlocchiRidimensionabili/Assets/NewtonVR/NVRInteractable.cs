using UnityEngine;
using System.Collections;
using System.Linq;

namespace NewtonVR
{
    public abstract class NVRInteractable : MonoBehaviour
    {
        public Rigidbody Rigidbody;

        public bool CanAttach = true;
        public event System.Action Selected, Unselected, StaySelected;
        public bool DisableKinematicOnAttach = true;
        public bool EnableKinematicOnDetach = false;
        public float DropDistance = 1;

        public bool EnableGravityOnDetach = true;

        public NVRHand attachedHand = null;

        protected Collider[] Colliders;
        protected Vector3 ClosestHeldPoint;

        public NVRHand AttachedHand
        {
            get
            {
                return attachedHand;
            }
            set
            {
                var oldvalue = attachedHand;
                attachedHand = value;
                if (value == null)
                {
                    if (Unselected != null)
                        Unselected();
                }
                else if (oldvalue != value)
                {
                    if (Selected != null)
                        Selected();
                }
            }
        }

        public virtual bool IsAttached
        {
            get
            {
                return AttachedHand != null;
            }
        }

        protected virtual void Awake()
        {   
            if (Rigidbody == null)
                Rigidbody = this.GetComponent<Rigidbody>();

            if (Rigidbody == null)
            {
                Debug.LogError("There is no rigidbody attached to this interactable.");
            }

        }

        protected virtual void Start()
        {
            UpdateColliders();
        }

        public virtual void ResetInteractable()
        {
            Awake();
            Start();
            AttachedHand = null;
        }

        public virtual void UpdateColliders()
        {
            var AllColliders = this.GetComponentsInChildren<Collider>();
            var Collist = new System.Collections.Generic.List<Collider>();
            var n = name;
            AllColliders.ToList().ForEach(s => {
                if (s.gameObject.layer == this.gameObject.layer)
                    Collist.Add(s);
                    });
            Colliders = Collist.ToArray();
            NVRInteractables.Register(this, Colliders);
        }

        protected virtual bool CheckForDrop()
        {
            float shortestDistance = float.MaxValue;

            for (int index = 0; index < Colliders.Length; index++)
            {
                //todo: this does not do what I think it does.
                Vector3 closest = Colliders[index].ClosestPointOnBounds(AttachedHand.transform.position);
                float distance = Vector3.Distance(AttachedHand.transform.position, closest);

                if (distance < shortestDistance)
                {
                    shortestDistance = distance;
                    ClosestHeldPoint = closest;
                }
            }

            if (DropDistance != -1 && AttachedHand.CurrentInteractionStyle != InterationStyle.ByScript && shortestDistance > DropDistance)
            {
                DroppedBecauseOfDistance();
                return true;
            }

            return false;
        }

        //Remove items that go too high or too low.
        protected virtual void Update()
        {
            if (IsAttached)
                if (StaySelected != null)
                    StaySelected();
            if (this.transform.position.y > 10000 || this.transform.position.y < -10000)
            {
                if (AttachedHand != null)
                    AttachedHand.EndInteraction(this);

                Destroy(this.gameObject);
            }
        }

        public virtual void BeginInteraction(NVRHand hand)
        {
            AttachedHand = hand;

            if (DisableKinematicOnAttach == true)
            {
                Rigidbody.isKinematic = false;
            }
        }

        public virtual void InteractingUpdate(NVRHand hand)
        {
            if (hand.UseButtonUp == true)
            {
                UseButtonUp();
            }

            if (hand.UseButtonDown == true)
            {
                UseButtonDown();
            }
        }

        public virtual void HoveringUpdate(NVRHand hand, float forTime)
        {

        }

        public void ForceDetach()
        {
            if (AttachedHand != null)
                AttachedHand.EndInteraction(this);

            if (AttachedHand != null)
                EndInteraction();
        }

        public virtual void EndInteraction()
        {
            AttachedHand = null;
            ClosestHeldPoint = Vector3.zero;

            if (EnableKinematicOnDetach == true)
            {
                Rigidbody.isKinematic = true;
            }

            if (EnableGravityOnDetach == true)
            {
                Rigidbody.useGravity = true;
            }
        }

        protected virtual void DroppedBecauseOfDistance()
        {
            AttachedHand.EndInteraction(this);
        }

        public virtual void UseButtonUp()
        {

        }

        public virtual void UseButtonDown()
        {

        }


        public virtual void AddExternalVelocity(Vector3 velocity)
        {
            Rigidbody.AddForce(velocity, ForceMode.VelocityChange);
        }

        public virtual void AddExternalAngularVelocity(Vector3 angularVelocity)
        {
            Rigidbody.AddTorque(angularVelocity, ForceMode.VelocityChange);
        }

        protected virtual void OnDestroy()
        {
            ForceDetach();
            NVRInteractables.Deregister(this);
        }
    }
}
