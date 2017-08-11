using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace View
{
    public class ChooseModelWindow : Resources.VRWindow
    {
        public event System.Action<Model.ActorModel> ModelChosen;
        public List<Resources.VRButton> buttons;

        protected override void Awake()
        {
            base.Awake();
            buttons.ForEach(x => x.Pressed += ButtonPressed);
        }

        private void ButtonPressed(object sender, System.EventArgs e)
        {
            if (ModelChosen != null)
                ModelChosen(Model.ActorModel.GetActorModel(buttons.IndexOf(sender as Resources.VRButton)));
            Close();
        }
    }
}
