using System;
using UnityEngine;

namespace Assets._Scripts
{
    public enum GameState
    {
        InGame,
        InInteraction,
        Paused
    };

    [UnityComponent]
    public class GameStateController : MonoBehaviour
    {
        public event Action<GameState> GameStateChanged;

        public static GameStateController Instance { get; private set; }

        public GameState CurrentState { get; private set; }

        [UnityMessage]
        public void Awake()
        {
            Instance = this;
            CurrentState = GameState.Paused;
        }

        [UnityMessage]
        public void Start()
        {
            SetState(GameState.InGame);
        }

        public void SetState(GameState state)
        {
            CurrentState = state;

            if (GameStateChanged != null)
                GameStateChanged(CurrentState);
        }
    }
}