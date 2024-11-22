using UnityEngine;
namespace FS_ThirdPerson
{
    public class Slowmotion : MonoBehaviour
    {
        [Range(0,1)]
        public float speed;
        void Update()
        {
            if (Input.GetKeyDown("3"))
                Debug.Break();

            if (Input.GetKeyDown("1"))
                Time.timeScale = 1f;

            if (Input.GetKeyDown("2"))
                Time.timeScale = .1f;
        }
    }
}
