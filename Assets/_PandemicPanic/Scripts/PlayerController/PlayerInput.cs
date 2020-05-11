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

    private bool m_canControl = true;

    public ImpactDetect m_knockOutDetect;

    public float m_knockOutTime, m_knockOutSafeTime;
    private float m_knockOutTimer;

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
        if (m_canControl) {
            GetInput();


            if (!m_knockOutDetect.m_canBeKnockedOut)
            {
                if(m_knockOutTimer > m_knockOutSafeTime)
                {
                    m_knockOutDetect.m_canBeKnockedOut = true;
                }
                else
                {
                    m_knockOutTimer += Time.deltaTime;
                }
            }
        }
        else
        {
            if(m_knockOutTimer > m_knockOutTime)
            {
                m_canControl = true;
                m_knockOutTimer = 0;

            }
            else
            {
                m_knockOutTimer += Time.deltaTime;
            }
        }
    }

    public void KnockedOut()
    {
        m_knockOutTimer = 0;
        m_canControl = false;
        m_knockOutDetect.m_canBeKnockedOut = false;
    }

    public Vector3 m_aim;
    public void GetInput()
    {
        #region Movement Input

        m_ragdollController.Move(new Vector3(m_playerInputController.GetAxis("MoveHorizon"), 0, m_playerInputController.GetAxis("MoveVertical")));

        Vector3 facingDir = new Vector3(m_playerInputController.GetAxis("LookHorizon"),0, m_playerInputController.GetAxis("LookVertical"));
        m_aim = facingDir;


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
            print("LArm");
            m_ragdollController.LeftPickupInput(true);
        }
        if (m_playerInputController.GetButtonUp("LeftPickup"))
        {
            print("RArm");
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


    private void OnDisable()
    {
        if (m_ragdollController == null) return;
        m_ragdollController.TurnCharacter(false, false);
        m_ragdollController.RightPickupInput(false);
        m_ragdollController.LeftPickupInput(false);
    }
}