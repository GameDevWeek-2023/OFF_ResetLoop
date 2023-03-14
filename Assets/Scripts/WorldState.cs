namespace DefaultNamespace
{
    public class WorldState
    {
        private int _time = 0;
        
        public void Tick()
        {
            GameEvents.Instance.OnTimeChanged(_time);
        }
    }
}