using UnityEngine;

[System.Serializable]
public class ImpactEvent : UnityEngine.Events.UnityEvent { };
public class ImpactDetect : MonoBehaviour
{
	public APRController APR_Player;
    public float ImpactForce;
	public float KnockoutForce;


    public bool m_canBeKnockedOut = true;

    public ImpactEvent m_knockedOutEvent, m_impactEvent;
	void OnCollisionEnter(Collision col)
	{

        //Sound on impact
        if (col.relativeVelocity.magnitude > ImpactForce)
        {
            m_impactEvent.Invoke();
        }

        if (!m_canBeKnockedOut) return;
        //Knockout by impact
		if(col.relativeVelocity.magnitude > KnockoutForce)
		{
			APR_Player.ActivateRagdoll();

            m_knockedOutEvent.Invoke();

        }
        

	}
}
