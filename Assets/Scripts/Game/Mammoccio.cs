using UnityEngine;
using System.Collections;

public class Mammoccio : MonoBehaviour 
{
    public int playerNum = 0;

    float AngleSx=0;
    float lastAngleSx= 0;
    public float currentVersSx = 0;

    float AngleDx = 0;
    float lastAngleDx = 0;
    public float currentVersDx = 0;

    public float force = 30f;
    public float applyRightDist = 10f;
	// Use this for initialization
	void Start () {
	
	}

    float lastUpdate = 0;
	void Update () {
        if (lastUpdate == 0 || Time.time - lastUpdate > 0.1f)
        {
            lastUpdate = Time.time;

            this.checkRotation();
            Vector3 dir = Vector3.zero;

            if (this.currentVersSx > 0.2f)
            {
               // dir += (this.transform.forward - this.transform.right) * force;
                this.rigidbody.AddForceAtPosition(this.transform.forward * force, this.transform.forward + this.transform.position - this.transform.right * applyRightDist);
            }

            if (this.currentVersDx > 0.2f)
            {
                //dir += (this.transform.forward + this.transform.right) * force;
                this.rigidbody.AddForceAtPosition(this.transform.forward * force, this.transform.forward + this.transform.position + this.transform.right * applyRightDist);
            }

            //this.rigidbody.AddForce(dir);
            
        }
        
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
    public static float GetAxisValue(int playerNumber, AxesMapping axisName)
    {
        return Input.GetAxis(playerNumber + "_Axis" + (int)axisName);
    }
}

