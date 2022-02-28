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
        private int i = 0;
        public float speed = 2;
        public int flag1 = 0;
        public int step;
        public int j = 0;
        public int test = 0;
        int Run = 0;
        // Start is called before the first frame update
        void Start()
        {
            MH24.RobotInitiate(ZeroJoint);  
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            
            MH24.Selection();
            MH24.Manipulate();
            if (Run == 1)
            {
                if (test != -1 || step != -1)
                {
                    if (step == 1)
                    {
                        j++;
                        step = 0;
                    }

                    if (flag1 == 0)
                    {
                        test = MH24.SetRobotToPositionNOW("P3", j, speed);
                        flag1 = 1;
                    }
                    else
                        step = MH24.SetRobotToPosition("P3", j + 1, speed);
                }
                else
                    Run = 0;
            }
                          


            if (Input.GetKeyDown(KeyCode.K))
            {
                MH24.CreateProgram(@"\P3.txt");
            }

            if (Input.GetKeyDown(KeyCode.P))
            {
                MH24.PrintJointPosition();
                MH24.WriteProgram(MH24.GetInstantJointPoint(), @"\P3.txt");
                //Debug.Log(MH24.GetInstantJointPoint());
                //MH24.StoreJointPoint(MH24.GetInstantJointPoint());
                // MH24.WriteProgram(MH24.GetInstantJointPoint());
            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
                MH24.PrintJointTest();
            }

            if (Input.GetKeyDown(KeyCode.E))
                Run = 1;
            
        }
        
        void OnGUI()
        {
            GUIStyle style = new GUIStyle();
            style.fontSize = 24;


            GUI.Label(new Rect(10, 50, 0, 0), MH24.PrintJointPosition(), style);
        }
    }
}

