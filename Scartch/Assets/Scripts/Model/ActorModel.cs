using System;
using System.Linq;

namespace Model
{
    public class ActorModel
    {
        private string name;

        private ActorModel(string name)
        {
            this.name = name;
        }

        private static ActorModel[] actorModels;

        public static ActorModel GetActorModel(int num)
        {
            if (actorModels == null)
            {
                actorModels = new ActorModel[3];
                actorModels[0] = new ActorModel("Model 1");
                actorModels[1] = new ActorModel("Model 2");
                actorModels[2] = new ActorModel("Model 3");
            }
            if (num > actorModels.Length)
                throw new ArgumentException("invalid actormodel number");
            else
                return actorModels[num];
        }
        public static int IndexOfModel(ActorModel actor)
        {
            if (actorModels == null)
            {
                actorModels = new ActorModel[3];
                actorModels[0] = new ActorModel("Model 1");
                actorModels[1] = new ActorModel("Model 2");
                actorModels[2] = new ActorModel("Model 3");
            }
            if (!actorModels.Contains(actor))
                throw new ArgumentException("invalid actormodel");
            else
                return actorModels.ToList().IndexOf(actor);
        }
    }
}