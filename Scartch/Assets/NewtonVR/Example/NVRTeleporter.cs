using UnityEngine;
using System.Collections;

namespace NewtonVR.Example
{
    [RequireComponent(typeof(NVRHand))]
    [RequireComponent(typeof(Rigidbody))]
    public class NVRTeleporter : MonoBehaviour
    {
        private Color LineColor = new Color(0, 255 / 255.0f, 76 / 255.0f);
        private float LineWidth = 0.2f;

        private LineRenderer Line;

        private NVRHand Hand;

        private NVRPlayer Player;

        private void Awake()
        {
            Line = this.GetComponent<LineRenderer>();
            Hand = this.GetComponent<NVRHand>();

            if (Line == null)
            {
                Line = this.gameObject.AddComponent<LineRenderer>();
            }

            if (Line.sharedMaterial == null)
            {
                Line.material = new Material(Shader.Find("Unlit/Color"));
                Line.material.SetColor("_Color", LineColor);
                NVRHelpers.LineRendererSetColor(Line, LineColor, LineColor);
            }

            Line.useWorldSpace = true;
        }

        private void Start()
        {
            Player = Hand.Player;
        }

        private Vector3 previousHitPosition;
        private bool previousHit;

        private void LateUpdate()
        {
            if ((Hand.Inputs[NVRButtons.Y].IsPressed || Hand.Inputs[NVRButtons.B].IsPressed))
            {
                Line.enabled = true;
                Line.material.SetColor("_Color", LineColor);
                NVRHelpers.LineRendererSetColor(Line, LineColor, LineColor);
                NVRHelpers.LineRendererSetWidth(Line, LineWidth, LineWidth);

                RaycastHit hitInfo;
                bool hit = Physics.Raycast(this.transform.position, this.transform.forward, out hitInfo, 1000);
                Vector3 endPoint;

                if (hit == true && hitInfo.collider.gameObject.CompareTag("floor"))
                {
                    endPoint = hitInfo.point;
                    previousHit = true;
                    previousHitPosition = hitInfo.point;
                }
                else
                {
                    previousHit = false;
                    endPoint = this.transform.position + (this.transform.forward * 1000f);
                }

                Line.SetPositions(new Vector3[] { this.transform.position, endPoint });
            }

            if (previousHit && (Hand.Inputs[NVRButtons.Y].PressUp == true || Hand.Inputs[NVRButtons.B].PressUp == true))
            {
                NVRInteractable LHandInteractable = Player.LeftHand.CurrentlyInteracting;
                NVRInteractable RHandInteractable = Player.RightHand.CurrentlyInteracting;


                Vector3 offset = Player.Head.transform.position - Player.transform.position;
                offset.y = 0;

                Player.transform.position = previousHitPosition - offset;
                if (LHandInteractable != null)
                {
                    LHandInteractable.transform.position = Player.LeftHand.transform.position;
                }
                if (RHandInteractable != null)
                {
                    RHandInteractable.transform.position = Player.RightHand.transform.position;
                }
            }
        }
    }
}