﻿using UnityEngine;
using System.Collections;

namespace NewtonVR.Example
{
    public class NVRLaserPointer : MonoBehaviour
    {
        private static Color LineColor;
        private Color LineColorBlue = new Color(38 / 255.0f, 75 / 255.0f, 255 / 255.0f);
        private Color LineColorRed = new Color(255 / 255.0f, 24 / 255.0f, 24 / 255.0f);
        private static int state = 0;
        private float LineWidth = 0.2f;

        private LineRenderer Line;

        private NVRHand Hand;

        private void NextState()
        {
            state++;
            if (state > 2) state = 0;
            if (state == 1) LineColor = LineColorBlue;
            if (state == 2) LineColor = LineColorRed;
        }

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

        private void LateUpdate()
        {
            if ((Hand.Inputs[NVRButtons.Y].IsPressed || Hand.Inputs[NVRButtons.B].IsPressed))
                return;
            if (Hand.Inputs[NVRButtons.A].PressDown || Hand.Inputs[NVRButtons.X].PressDown)
                NextState();


            Line.enabled = state > 0;


            if (Line.enabled == true)
            {
                Line.material.SetColor("_Color", LineColor);
                NVRHelpers.LineRendererSetColor(Line, LineColor, LineColor);
                NVRHelpers.LineRendererSetWidth(Line, LineWidth, LineWidth);

                RaycastHit hitInfo;
                bool hit = Physics.Raycast(this.transform.position, this.transform.forward, out hitInfo, 1000);
                Vector3 endPoint;

                if (hit == true)
                {
                    endPoint = hitInfo.point;

                    if (Hand.Inputs[NVRButtons.Trigger].PressDown && hitInfo.collider.gameObject.GetComponent<View.Resources.RayHittable>() != null)
                        if (state == 1)
                            hitInfo.collider.gameObject.GetComponent<View.Resources.RayHittable>().HitByBlueRay();
                        else if (state == 2)
                            hitInfo.collider.gameObject.GetComponent<View.Resources.RayHittable>().HitByRedRay();
                }
                else
                {
                    endPoint = this.transform.position + (this.transform.forward * 1000f);
                }

                Line.SetPositions(new Vector3[] { this.transform.position, endPoint });
            }
        }
    }
}