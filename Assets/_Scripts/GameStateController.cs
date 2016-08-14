using System;
using UnityEngine;
using UnityEngine.SceneManagement;

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

        public static bool InGame { get { return Instance.CurrentState == GameState.InGame; } }

        public static bool NoSimulate {get { return Instance.CurrentState != GameState.InGame; } }

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

        [UnityMessage]
        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                SceneManager.LoadScene(0);
        }
    }
}