using System.Collections;
using UnityEngine;

public class HandController : MonoBehaviour
{
    public APRController APR_Player;
    public bool hasJoint;
    private bool hasWaitedAfterThrow = true;

    private bool m_pickupInputDown;
    private int m_playerId;
    private Pickupable m_pickup;
    public void AssignPlayerID(int p_playerID)
    {
        m_playerId = p_playerID;
    }
    public void ChangeHandInput(bool p_inputDown)
    {
        m_pickupInputDown = p_inputDown;


        if (hasJoint && !p_inputDown)
        {
            this.gameObject.GetComponent<FixedJoint>().breakForce = 0;
            hasJoint = false;
            hasWaitedAfterThrow = false;
        }

        if (hasJoint && this.gameObject.GetComponent<FixedJoint>() == null)
        {
            hasJoint = false;
            hasWaitedAfterThrow = false;
        }

        if (!p_inputDown)
        {
            if (m_pickup != null)
            {
                m_pickup.DropObject();
                m_pickup = null;
            }
        }
    }



    //Grab on collision
    void OnCollisionEnter(Collision col)
    {
        if (APR_Player.useControls && hasWaitedAfterThrow)
        {

            if (col.gameObject.tag == "Object" && !hasJoint)
            {
                m_pickup = col.gameObject.GetComponent<Pickupable>();
                if (m_pickup != null)
                {
                    if (m_pickup.IsHeld())
                    {
                        return;
                    }
                    m_pickup.Pickup(m_playerId);
                }


                if (m_pickupInputDown && !hasJoint)
                {
                    hasJoint = true;
                    hasWaitedAfterThrow = false;
                    this.gameObject.AddComponent<FixedJoint>();
                    this.gameObject.GetComponent<FixedJoint>().breakForce = 100000;
                    this.gameObject.GetComponent<FixedJoint>().connectedBody = col.gameObject.GetComponent<Rigidbody>();

                }
            }

            else if (col.gameObject.tag == "Player" && col.gameObject.layer != LayerMask.NameToLayer(APR_Player.thisPlayerLayer) && !hasJoint)
            {
                if (m_pickupInputDown && !hasJoint)
                {
                    hasJoint = true;
                    hasWaitedAfterThrow = false;
                    this.gameObject.AddComponent<FixedJoint>();
                    this.gameObject.GetComponent<FixedJoint>().breakForce = 100000;
                    this.gameObject.GetComponent<FixedJoint>().connectedBody = col.gameObject.GetComponent<Rigidbody>();
                }
            }

        }
    }

    void OnJointBreak()
    {
        StartCoroutine(DelayCoroutine());
        IEnumerator DelayCoroutine()
        {
            yield return new WaitForSeconds(1f);
            hasWaitedAfterThrow = true;
        }
    }
}
