using UnityEngine;

namespace Scripts.Player
{
    public class PlayerSetter : MonoBehaviour
    {
        [SerializeField] private MonoBehaviour[] m_ComponentsToEnable;

        void Start()
        {
            foreach (var component in m_ComponentsToEnable)
            {
                component.enabled = true;
            }
        }
    }
}
