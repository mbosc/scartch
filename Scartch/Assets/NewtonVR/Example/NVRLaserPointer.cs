using UnityEngine;
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

        public UnityEngine.UI.Image alertImage;
        public UnityEngine.UI.Text alertText;

        public void Alert(string text)
        {
            Debug.Log("Sending alert to " + this.gameObject.name + ": " + text);
            alertText.text = "<color=red>Alert</color>\n" + text;
            StartCoroutine(ShowAlert());
        }

        private IEnumerator ShowAlert()
        {
            alertText.enabled = true;
            alertImage.enabled = true;
            yield return new WaitForSeconds(5);
            alertText.enabled = false;
            alertImage.enabled = false;
        }

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

            alertImage.enabled = false;
            alertText.enabled = false;
        }

        private void LateUpdate()
        {
            if ((Hand.Inputs[NVRButtons.Y].IsPressed || Hand.Inputs[NVRButtons.B].IsPressed))
                return;
            if (Controller.EnvironmentController.Instance.InPlayMode)
            {
                if (state != 1)
                {
                    state = 1;
                    LineColor = LineColorBlue;
                }
                var viewer = GetComponent<View.InputViewer>();
                if (Hand.Inputs[NVRButtons.A].PressDown || Hand.Inputs[NVRButtons.X].PressDown)
                {
                    viewer.PressKey(0);
                }
            }
            else
            {
                if (Hand.Inputs[NVRButtons.A].PressDown || Hand.Inputs[NVRButtons.X].PressDown)
                    NextState();
            }

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

                    if (Hand.Inputs[NVRButtons.Trigger].PressDown)
                    {
                        //Debug.Log(this.gameObject.name + " pointed at " + hitInfo.collider.gameObject.name + " (state " + state + ", RayHittable " + (hitInfo.collider.gameObject.GetComponentInParent<View.Resources.RayHittable>() != null ? "yes" : "no") + ")");
                        if (hitInfo.collider.gameObject.GetComponentInParent<View.Resources.RayHittable>() != null)
                            if (state == 1)
                            {
                                hitInfo.collider.gameObject.GetComponentInParent<View.Resources.RayHittable>().HitByBlueRay();
                                ScartchResourceManager.instance.lastRayCaster = this;
                            }
                            else if (state == 2)
                            {
                                hitInfo.collider.gameObject.GetComponentInParent<View.Resources.RayHittable>().HitByRedRay();
                                ScartchResourceManager.instance.lastRayCaster = this;
                            }
                    }
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