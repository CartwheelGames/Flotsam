using System.Collections;
using UnityEngine;
namespace Project
{
	public class Flotsam : MonoBehaviour
	{
        private bool isDead;
        private void OnDeath()
        {
            isDead = true;
        }
        private void OnCollisionEnter2D(Collision2D col)
        {
            if (!isDead && col.gameObject.layer == Layer.Hazard.ToIndex())
            {
                OnDeath();
            }
        }
	}
}
