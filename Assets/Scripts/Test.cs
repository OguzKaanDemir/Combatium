using UnityEngine;
using Sirenix.OdinInspector;

namespace Scripts
{
    public class Test : MonoBehaviour
    {
        public int input;

        public int min = -1200;
        public int max = 1200;

        [Button("New Input")]
        public void NewInput()
        {
            for (int i = 0; i < input; i++)
            {
                print(Calculate(i));
            }
        }

        public int Calculate(int number)
        {
            int val = number;

            while (true)
            {
                if (val > max)
                {
                    val = max + (max - val);
                }
                else if (val < min)
                {
                    val = min - (max + val);
                }
                else
                {
                    break;
                }
            }

            return val;
        }
    }
}
