using System;
using System.Collections;
using UnityEngine;

namespace Assets._Scripts
{
    [UnityComponent]
    public class Delay : MonoBehaviour
    {
        public static Delay Instance { get; private set; }
        
        [UnityMessage]
        public void Awake()
        {
            Instance = this;
        }

        public static Coroutine Of(float time, Action action)
        {
            return Instance.InternalOf(time, action);
        }

        public static Coroutine Frame(Action action)
        {
            return Instance.InternalFrame(action);
        }

        public static Coroutine TemporarilySetBool(Action<bool> setter, float time, bool temporarilySetTo = true)
        {
            return Instance.InternalTemporarilySetBool(setter, time, temporarilySetTo);
        }

        private Coroutine InternalOf(float time, Action action)
        {
            return StartCoroutine(OfDelay(time, action));
        }

        private Coroutine InternalFrame(Action action)
        {
            return StartCoroutine(DelayFrame(action));
        }

        private Coroutine InternalTemporarilySetBool(Action<bool> setter, float time, bool temporarilySetTo)
        {
            return StartCoroutine(Temporarily(() => setter(temporarilySetTo), () => setter(!temporarilySetTo), time));
        }

        private IEnumerator Temporarily(Action before, Action after, float delay)
        {
            before();
            yield return new WaitForSeconds(delay);
            after();
        }

        private IEnumerator OfDelay(float time, Action action)
        {
            yield return new WaitForSeconds(time);
            action();
        }

        private IEnumerator DelayFrame(Action action)
        {
            yield return null;
            action();
        }
    }
}