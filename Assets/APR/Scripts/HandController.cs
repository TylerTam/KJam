using System.Collections;
using UnityEngine;

public class HandController : MonoBehaviour
{
    public APRController APR_Player;
    public bool hasJoint;
    private bool hasWaitedAfterThrow = true;

    bool m_pickupInputDown;


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
    }



    //Grab on collision
    void OnCollisionEnter(Collision col)
    {
        if (APR_Player.useControls && hasWaitedAfterThrow)
        {

            if (col.gameObject.tag == "Object" && !hasJoint)
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
