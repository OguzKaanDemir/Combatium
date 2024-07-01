using TMPro;
using Fusion;
using UnityEngine;
namespace Scripts.Managers
{
    [RequireComponent(typeof(NetworkRunner))]
    public class NetworkManager : MonoBehaviour
    {
        private static NetworkRunner m_Runner;
        public static NetworkRunner Runner
        {
            get
            {
                return m_Runner;
            }
        }
    }
}
