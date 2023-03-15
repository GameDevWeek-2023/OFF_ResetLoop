namespace model
{
    public class Position
    {
        private float _x;
        private float _y;

        public Position(float x, float y)
        {
            this._x = x;
            this._y = y;
        }

        public float X
        {
            get => _x;
            set => _x = value;
        }

        public float Y
        {
            get => _y;
            set => _y = value;
        }
    }
}