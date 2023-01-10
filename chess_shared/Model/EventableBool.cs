using System;

namespace chess_shared.Model
{
    public class EventableBool<T>
    {
        private T _value;
        public event Action StateChanged;

        public T Value
        {
            get => _value;

            set
            {
                if (!Equals(value, _value))
                {
                    _value = value;
                    StateChanged?.Invoke();
                }
            }
        }
    }
}
