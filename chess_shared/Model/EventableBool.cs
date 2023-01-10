using System;

namespace chess_shared.Model
{
    public class EventableBool
    {
        private bool _state;
        public event Action StateChanged;

        public bool State
        {
            get => _state;

            set
            {
                if (_state != value)
                {
                    _state = value;
                    StateChanged?.Invoke();
                }
            }
        }
    }
}
