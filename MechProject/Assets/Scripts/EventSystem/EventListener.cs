using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventListener : MonoBehaviour {

    [SerializeField]
    ScriptableEvent m_event;

    [SerializeField]
    UnityEvent m_response;

    private void OnEnable()
    {
        m_event.AddListener(this);
    }

    private void OnDisable()
    {
        m_event.RemoveListener(this);
    }

    public void Response()
    {
        m_response.Invoke();
    }
}
