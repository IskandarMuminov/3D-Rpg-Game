using UnityEngine;
using RPG.Attributes;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make new Weapon", order = 0)]
    public class WeaponConfig : ScriptableObject 
    {
        [SerializeField] AnimatorOverrideController animatorOverride;
        [SerializeField] Weapon equippedPrefab = null;
        [SerializeField] float weaponDamage = 10f;
        [SerializeField] float percentageBonus = 0f ;
        [SerializeField] float weaponRange = 2f;
        [SerializeField] bool isRightHanded = true;
        [SerializeField] Projectiles projectiles = null;
        
        const string weaponName = "Weapon";

        public Weapon Spawn(Transform rightHand, Transform leftHand, Animator animator)
        {
            DestroyOldWeapon(rightHand,leftHand);

            Weapon weapon = null;

            if(equippedPrefab != null)
            {
                Transform handTransform = GetTransform(rightHand, leftHand);
                weapon = Instantiate(equippedPrefab, handTransform);
                weapon.gameObject.name = weaponName;
            }
            var overrideController = animator.runtimeAnimatorController as AnimatorOverrideController;

            if (animatorOverride != null)
            {
                animator.runtimeAnimatorController = animatorOverride;
            }
            else if(overrideController != null)
            {
                animator.runtimeAnimatorController = overrideController.runtimeAnimatorController;
            }

            return weapon;
        }

        public void DestroyOldWeapon(Transform rightHand, Transform lefHand)
        {
            Transform prevWeapon = rightHand.Find(weaponName);
            if(prevWeapon == null)
            {
                prevWeapon = lefHand.Find(weaponName);

            }
            if(prevWeapon == null) return;
            prevWeapon.name = "DESTROYING";
            
            Destroy(prevWeapon.gameObject);
        }

        private Transform GetTransform(Transform rightHand, Transform leftHand)
        {
            Transform handTransform;
            if (isRightHanded) handTransform = rightHand;
            else handTransform = leftHand;
            return handTransform;
        }

        public bool HasProjectile()
        {
            return projectiles !=null;
        }

        public void LaunchProjectile(Transform rightHand, Transform leftHand, Health target, GameObject instigator, float calculatedDamage)
        {
            Projectiles projectileInstance = Instantiate(projectiles, GetTransform(rightHand, leftHand).position, Quaternion.identity);
            projectileInstance.SetTarget(target,instigator, calculatedDamage);
        }

        public float GetWeaponRange()
        {
            return weaponRange;
        }

        public float GetPercentageBonus()
        {
            return percentageBonus;
        }

        public float GetWeaponDamage()
        {
            return weaponDamage;
        }
    }
}
