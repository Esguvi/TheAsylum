using UnityEngine;

namespace SkinlessZombie
{
    public class AnimationController : MonoBehaviour
    {
        public Transform weaponPivot;
        public Transform rightHand;
        public GameObject axePrefab;

        private void Start()
        {
            if (axePrefab != null)
            {
                GameObject axeInstance = Instantiate(axePrefab, rightHand);
                axeInstance.transform.position = weaponPivot.position;
                axeInstance.transform.localScale = weaponPivot.localScale;
                axeInstance.transform.localRotation = weaponPivot.localRotation;
                axeInstance.SetActive(true);
            }
        }
    }
}
