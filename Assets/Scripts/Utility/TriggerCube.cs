using System;
using UnityEngine;
using UnityEngine.Events;

namespace Utility
{
    [RequireComponent(typeof(BoxCollider), typeof(Rigidbody))]
    public class TriggerCube : MonoBehaviour
    {
        [Serializable]
        public class TriggerEvent : UnityEvent<Collider> { }

        public TriggerEvent OnEnter;
        public TriggerEvent OnStay;
        public TriggerEvent OnExit;

        // OnTriggerEnter вызывается, когда Collider входит в триггер
        private void OnTriggerEnter(Collider other)
        {
            OnEnter?.Invoke(other);
        }

        // OnTriggerExit вызывается, когда Collider перестает касаться триггера
        private void OnTriggerExit(Collider other)
        {
            OnExit?.Invoke(other);
        }

        // OnTriggerStay вызывается в каждом кадре для всех элементов Collider, которые касаются триггера
        private void OnTriggerStay(Collider other)
        {
            OnStay?.Invoke(other);
        }

        // Метод Awake вызывается во время загрузки экземпляра сценария
        private void Awake()
        {
            GetComponent<Collider>().isTrigger = true;
            GetComponent<Rigidbody>().isKinematic = true;
        }


    }
}
