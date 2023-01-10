using System;

namespace chess_shared.Model
{
    public class EventableBool<T>
    {
        private T _value;
        public event Action ValueChanged;

        public T Value
        {
            get => _value;

            set
            {
                if (!Equals(value, _value))
                {
                    _value = value;
                    ValueChanged?.Invoke();
                }
            }
        }
    }
}
