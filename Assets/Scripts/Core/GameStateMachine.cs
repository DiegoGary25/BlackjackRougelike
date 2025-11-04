using System;

namespace HouseTakes21.Core
{
    /// <summary>
    /// Represents the high-level game state machine for the run.
    /// </summary>
    public sealed class GameStateMachine
    {
        /// <summary>
        /// Enumerates the possible global states.
        /// </summary>
        public enum GameState
        {
            Boot,
            TableStart,
            HandStart,
            PlayerTurn,
            DealerTurn,
            Resolve,
            TableEnd,
            Intermission,
            Death,
        }

        private GameState currentState;

        /// <summary>
        /// Occurs when the state changes.
        /// </summary>
        public event Action<GameState>? OnStateChanged;

        /// <summary>
        /// Gets the current state.
        /// </summary>
        public GameState CurrentState => currentState;

        /// <summary>
        /// Sets the state to the specified value, invoking the change event.
        /// </summary>
        /// <param name="state">The new state.</param>
        public void SetState(GameState state)
        {
            if (currentState == state)
            {
                return;
            }

            currentState = state;
            OnStateChanged?.Invoke(currentState);
        }
    }
}
