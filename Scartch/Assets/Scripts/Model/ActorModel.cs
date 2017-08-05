using System;

namespace Model
{
    public class ActorModel
    {
        private string name;

        private ActorModel(string name)
        {
            this.name = name;
        }

        public static ActorModel[] actorModels;

        public static ActorModel GetActorModel(int num)
        {
            if (actorModels == null)
            {
                actorModels = new ActorModel[3];
                actorModels[0] = new ActorModel("Sound 1");
                actorModels[1] = new ActorModel("Sound 2");
                actorModels[2] = new ActorModel("Sound 3");
            }
            if (num > actorModels.Length)
                throw new ArgumentException("invalid sound number");
            else
                return actorModels[num];
        }
    }
}