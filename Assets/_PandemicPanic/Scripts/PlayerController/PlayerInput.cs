using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;


public class PlayerInput : MonoBehaviour
{
    public int m_playerId;

    private APRController m_ragdollController;
    private Player m_playerInputController;
    public Transform m_facingForward;
    public float m_facingDirAngleThreshold;

    private void Start()
    {

        m_ragdollController = GetComponent<APRController>();
        m_playerInputController = ReInput.players.GetPlayer(m_playerId);
    }

    public void ChangeCursorState(bool p_activeState)
    {
        Cursor.visible = !false;

        if (!p_activeState)
        {

            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }

    private void Update()
    {
        GetInput();
    }


    public void GetInput()
    {
        #region Movement Input

        m_ragdollController.Move(new Vector3(m_playerInputController.GetAxis("MoveHorizon"), 0, m_playerInputController.GetAxis("MoveVertical")));

        Vector3 facingDir = new Vector3(m_playerInputController.GetAxis("LookHorizon"),0, m_playerInputController.GetAxis("LookVertical"));

        if (Vector3.Angle(new Vector3(m_facingForward.forward.x, 0, m_facingForward.forward.z),facingDir) > m_facingDirAngleThreshold)
        {
            if (Mathf.Sign(Vector3.Dot(new Vector3(m_facingForward.forward.x, 0, m_facingForward.forward.z).normalized, Quaternion.AngleAxis(90,Vector3.up) * facingDir)) > 0)
            {
                m_ragdollController.TurnCharacter(true,false);
            }
            else
            {
                m_ragdollController.TurnCharacter(false, true);
            }
        }
        #endregion

        #region Jump
        if (m_playerInputController.GetButtonDown("Jump"))
        {
            m_ragdollController.JumpInput(true);
        }
        #endregion

        #region Arm Inputs
        if (m_playerInputController.GetButtonDown("LeftPickup"))
        {
            m_ragdollController.LeftPickupInput(true);
        }
        if (m_playerInputController.GetButtonUp("LeftPickup"))
        {
            m_ragdollController.LeftPickupInput(false);
        }

        if (m_playerInputController.GetButtonDown("RightPickup"))
        {
            m_ragdollController.RightPickupInput(true);
        }
        if (m_playerInputController.GetButtonUp("RightPickup"))
        {
            m_ragdollController.RightPickupInput(false);
        }

        if (m_playerInputController.GetButtonDown("Throw"))
        {
            m_ragdollController.PickupAndThrow(true);
        }

        if (m_playerInputController.GetButtonDown("LeftPunch"))
        {
            m_ragdollController.PunchInput(false, true);
        }
        if (m_playerInputController.GetButtonDown("RightPunch"))
        {
            m_ragdollController.PunchInput(true, false);
        }

        #endregion

        if (m_playerInputController.GetButtonDown("Ragdoll"))
        {
            m_ragdollController.ActivateRagdoll();
        }
    }

}