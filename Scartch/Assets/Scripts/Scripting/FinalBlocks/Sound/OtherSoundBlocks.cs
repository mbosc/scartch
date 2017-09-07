using System;
using System.Collections;
using System.Collections.Generic;
using Model;
using UnityEngine;
using View;
using System.Linq;

namespace Scripting
{
    public class PlaySoundBlock : Block
    {
        public PlaySoundBlock(Actor owner, List<Option> optionList, List<ReferenceSlotViewer> referenceSlotViewers, BlockViewer viewer, bool sample) : base(owner, optionList, ScriptingType.sound, referenceSlotViewers, viewer, sample)
        {
        }

        public static string description = "Play {sound 1|sound 2|sound 3}";

        public override string Description
        {
            get
            {
                return description;
            }
        }

        public override void Execute()
        {
            var sound = Sound.GetSound(GetOption(0).Value);
            owner.PlaySound(sound);
            Flux.current.CurrentBlock = Next;
        }
    }

    public class StopAllSoundsBlock : Block
    {
        public StopAllSoundsBlock(Actor owner, List<Option> optionList, List<ReferenceSlotViewer> referenceSlotViewers, BlockViewer viewer, bool sample) : base(owner, optionList, ScriptingType.sound, referenceSlotViewers, viewer, sample)
        {
        }

        public static string description = "Stop all sounds";

        public override string Description
        {
            get
            {
                return description;
            }
        }

        public override void Execute()
        {
            UnityEngine.GameObject.FindObjectsOfType<AudioSource>().ToList().ForEach(x => x.Stop());
            Flux.current.CurrentBlock = Next;
        }
    }

    public class VolumeReference : ExpressionReference
    {

        public static string description = "Volume";

        public VolumeReference(Actor owner, List<Option> optionList, List<ReferenceSlotViewer> referenceSlotViewers, ReferenceViewer viewer, bool sample) : base(owner, optionList, ScriptingType.sound, referenceSlotViewers, viewer, RefType.numberType, sample)
        {
        }

        public override string Description
        {
            get
            {
                return description;
            }
        }

        public override string Evaluate()
        {
            float val = owner.Volume;
            return val.ToStdStr();
        }
    }
}
