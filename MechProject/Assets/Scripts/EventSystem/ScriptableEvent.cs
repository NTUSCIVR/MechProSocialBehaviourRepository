using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Event")]
public class ScriptableEvent : ScriptableObject {
    List<EventListener> m_eventListeners = new List<EventListener>();

    public void Raise()
    {
        for (int i = m_eventListeners.Count - 1; i >= 0; --i)
        {
            m_eventListeners[i].Response();
        }
    }

    public void AddListener(EventListener _eventListener)
    {
        if (m_eventListeners.Contains(_eventListener))
            return;

        m_eventListeners.Add(_eventListener);

    }

    public void RemoveListener(EventListener _eventListener)
    {
        if (!m_eventListeners.Contains(_eventListener))
            return;

        m_eventListeners.Remove(_eventListener);
    }
}
