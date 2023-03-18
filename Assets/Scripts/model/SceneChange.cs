namespace model
{
    public class SceneChange
    {
        private WorldState.Scene oldScene;
        private WorldState.Scene newScene;

        public SceneChange(WorldState.Scene oldScene, WorldState.Scene newScene)
        {
            this.oldScene = oldScene;
            this.newScene = newScene;
        }

        public WorldState.Scene OldScene => oldScene;

        public WorldState.Scene NewScene => newScene;
    }
}