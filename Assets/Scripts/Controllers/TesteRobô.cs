using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Factory_VR;


namespace Factory_VR
{
    public class TesteRobô : MonoBehaviour
    {


        public Robot MH24 = new Robot("MH24", 6);
        public float[] ZeroJoint = { 0,0,0,0,0,0,0};
        // Start is called before the first frame update
        void Start()
        {
            MH24.RobotInitiate(ZeroJoint);
            
            
            
        }

        // Update is called once per frame
        void Update()
        {
            MH24.Selection();
            MH24.Manipulate();


            if (Input.GetKeyDown(KeyCode.K))
            {
                MH24.CreateProgram(@"\P1.txt");
            }

            if (Input.GetKeyDown(KeyCode.P))
            {

                MH24.PrintJointPosition();
                MH24.WriteProgram(MH24.GetInstantJointPoint(), @"\P1.txt");
                //Debug.Log(MH24.GetInstantJointPoint());
                //MH24.StoreJointPoint(MH24.GetInstantJointPoint());

                // MH24.WriteProgram(MH24.GetInstantJointPoint());

            }


            if (Input.GetKeyDown(KeyCode.Q))
            {
                MH24.PrintJointTest();

            }


            




        }
        
        void OnGUI()
        {
            GUIStyle style = new GUIStyle();
            style.fontSize = 24;


            GUI.Label(new Rect(10, 50, 0, 0), MH24.PrintJointPosition(), style);
        }
    }
}

