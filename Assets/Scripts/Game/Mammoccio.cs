using UnityEngine;
using System;
using System.Collections;

public class Mammoccio : MonoBehaviour 
{
    public int playerNum = 0;

    float AngleSx=0;
    float lastAngleSx= 0;
    float currentVersSx = 0;
    float AngleDx = 0;
    float lastAngleDx = 0;
    float currentVersDx = 0;

    public float force = 30f;
    public float applyRightDist = 10f;

    public Transform bardasciaTarget;
    public float MaxBardasciaRotation = 20;
    public float BardasciaRotationPerSecond = 50f;
    public Transform scudoTarget;

    float lastUpdate = 0;

    Vector3 startBardasciaDir;
    void Start()
    {
        startBardasciaDir = bardasciaTarget.forward;
    }

	void Update () {
        if (lastUpdate == 0 || Time.time - lastUpdate > 0.1f)
        {
            lastUpdate = Time.time;

            this.checkRotation();
            Vector3 dir = Vector3.zero;

            if (this.currentVersSx > 0.2f)
            {
                this.rigidbody.AddForceAtPosition(this.transform.forward * force, this.transform.forward + this.transform.position - this.transform.right * applyRightDist);
            }

            if (this.currentVersDx > 0.2f)
            {
                this.rigidbody.AddForceAtPosition(this.transform.forward * force, this.transform.forward + this.transform.position + this.transform.right * applyRightDist);
            }
        }
        this.CheckBardasciaRotation();
        this.CheckScudoPosition();

	}
    private void checkRotation()
    {
        float x = GetAxisValue(this.playerNum, AxesMapping.LEFT_X_AXIS);
        float y = -GetAxisValue(this.playerNum, AxesMapping.LEFT_Y_AXIS);
        this.AngleSx = Mathf.Atan2(1, 0) - Mathf.Atan2(y, x);
        if (this.AngleSx < 0) this.AngleSx += 2 * Mathf.PI;
        if (this.AngleSx != 0)
            this.currentVersSx = this.AngleSx - this.lastAngleSx;

        this.lastAngleSx = this.AngleSx;

        float dxx = GetAxisValue(this.playerNum, AxesMapping.RIGHT_X_AXIS);
        float dxy = -GetAxisValue(this.playerNum, AxesMapping.RIGHT_Y_AXIS);
        this.AngleDx = Mathf.Atan2(1, 0) - Mathf.Atan2(dxy, dxx);
        if (this.AngleDx < 0) this.AngleDx += 2 * Mathf.PI;
        if (this.AngleDx != 0)
            this.currentVersDx = this.AngleDx - this.lastAngleDx;

        this.lastAngleDx = this.AngleDx;
    }
    public void CheckScudoPosition()
    {
        float y = 0;
        float x = 0;
        if (GetButton(this.playerNum, ButtonMapping.BUTTON_A))
            y += 1;
        if (GetButton(this.playerNum, ButtonMapping.BUTTON_B))
            x += 1;
        if (GetButton(this.playerNum, ButtonMapping.BUTTON_X))
            x -= 1;
        if (GetButton(this.playerNum, ButtonMapping.BUTTON_Y))
            y -= 1;


    }
    float lastDpadInput = 0;
    Vector3 targetBardasciaRotation=Vector3.zero;
    public void CheckBardasciaRotation()
    {
        float y = 0;
        float x = 0;

#if UNITY_STANDALONE_WIN
        //Su windows è necessario un controllo più raffinato sull'input
        if (GetAxisValue(this.playerNum, AxesMapping.DPAD_Y) > 0.1f)
        {
            if (lastDpadInput == 0 || (Time.time - lastDpadInput) > 0.2f)
            {
                lastDpadInput = Time.time;
                y += 1;
            }
        }
        if (GetAxisValue(this.playerNum, AxesMapping.DPAD_Y) < -0.1f)
        {
            if (lastDpadInput == 0 || (Time.time - lastDpadInput) > 0.2f)
            {
                lastDpadInput = Time.time;
                y -= 1;
            }
        }
        if (GetAxisValue(this.playerNum, AxesMapping.DPAD_X) > 0.1f)
        {
            if (lastDpadInput == 0 || (Time.time - lastDpadInput) > 0.2f)
            {
                lastDpadInput = Time.time;
                x += 1;
            }
        }
        if (GetAxisValue(this.playerNum, AxesMapping.DPAD_X) < -0.1f)
        {
            if (lastDpadInput == 0 || (Time.time - lastDpadInput) > 0.2f)
            {
                lastDpadInput = Time.time;
                x -= 1;
            }
        }
#elif UNITY_STANDALONE_OSX
			//Moving the highlight
			if(InputManager.GetButtonDown(this.playerNum, ButtonMapping.DPAD_UP))
			{
				y += 1;
			}
			if(InputManager.GetButtonDown(this.playerNum, ButtonMapping.DPAD_DOWN))
			{
				y -= 1;
			}
			if(InputManager.GetButtonDown(this.playerNum, ButtonMapping.DPAD_LEFT))
			{
				 x += 1;
			}
			if(InputManager.GetButtonDown(this.playerNum, ButtonMapping.DPAD_RIGHT))
			{
				x -= 1;
			}
#endif

        targetBardasciaRotation = new Vector3(Mathf.Clamp(targetBardasciaRotation.x + y * BardasciaRotationPerSecond * Time.deltaTime, -MaxBardasciaRotation, MaxBardasciaRotation), Mathf.Clamp(targetBardasciaRotation.y + x * BardasciaRotationPerSecond * Time.deltaTime, -MaxBardasciaRotation, MaxBardasciaRotation), 0);

        bardasciaTarget.transform.localRotation = Quaternion.Lerp(bardasciaTarget.transform.localRotation, Quaternion.Euler(targetBardasciaRotation), 0.2f);

    }

    public static float GetAxisValue(int playerNumber, AxesMapping axisName)
    {
        return Input.GetAxis(playerNumber + "_Axis" + (int)axisName);
    }
    protected bool GetButton(int playerNumber, ButtonMapping buttonName)
    {
        return Input.GetKey(BToCode(playerNumber, buttonName));
    }
    public static bool GetButtonDown(int playerNumber, ButtonMapping buttonName)
	{
		return Input.GetKeyDown(BToCode(playerNumber, buttonName));
	}
    protected static KeyCode BToCode(int playerNumber, ButtonMapping buttonName)
    {
        playerNumber++; //1-based
        return (KeyCode)Enum.Parse(typeof(KeyCode), "Joystick" + playerNumber + "Button" + (int)buttonName);
    }
}

