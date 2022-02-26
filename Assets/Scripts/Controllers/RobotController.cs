using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Factory_VR
{
    public class RobotController : MonoBehaviour
    {
       


        #region Global Variables
        public GameObject[] RobotModel = new GameObject[7];
        public GameObject[] Robot = new GameObject[7];
        public float[] Joints = new float[7];
        public float[] JointsFrom = new float[7];
        public float[] JointsTo = new float[7];
        public Material SelectedMaterial;
        public Material NormalMaterial;
        public float jointSpeed = 1f;
        public Vector3 PositionXYZ;
        public ArrayList Points = new ArrayList();

        private bool rotating = false;
        private int RobotJoint = 1;
        #endregion

        #region Functions
        
        void Selection() //Select a Joint and Show in Green
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (RobotJoint < 6)
                    RobotJoint++;
                RobotModel[RobotJoint].GetComponent<MeshRenderer>().material = SelectedMaterial;
                RobotModel[RobotJoint - 1].GetComponent<MeshRenderer>().material = NormalMaterial;
            }

            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (RobotJoint > 1)
                    RobotJoint--;

                RobotModel[RobotJoint].GetComponent<MeshRenderer>().material = SelectedMaterial;
                RobotModel[RobotJoint + 1].GetComponent<MeshRenderer>().material = NormalMaterial;
            }
        }

        void Manipulate() //Manipulate a Selected Joint
        {
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                Robot[RobotJoint].transform.Rotate(Vector3.forward * jointSpeed * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                Robot[RobotJoint].transform.Rotate(-Vector3.forward * jointSpeed * Time.deltaTime);
            }

            PositionXYZ = 100*Robot[0].transform.InverseTransformPoint(Robot[6].transform.position);
            // Debug.Log(PositionXYZ);
            
        }
        #endregion



        void Start()
        {
            RobotModel[1].GetComponent<MeshRenderer>().material = SelectedMaterial;
            for(int i=2;i<=6;i++)
                RobotModel[i].GetComponent<MeshRenderer>().material = NormalMaterial;

            Points.Clear();
        }

        
        void Update()
        {
            Selection();
            Manipulate();



            /* 
            if (Input.GetKeyDown(KeyCode.P))
            {
                for (int i = 0; i < 7; i++)
                {
                    Joints[i] = Robot[i].transform.eulerAngles.z;
                    Debug.Log(Joints[i]);
                }

                Points.AddRange(Joints);

                string result = "List contents: ";
                Debug.Log(Points.Count);
                
                foreach (var item in Points)
                {
                    for (int i = 0; i < 7; i++)
                    {
                        result += item[i].ToString() + ", ";
                    }
                    result += '\n';
                }

               
                Debug.Log(result);
            }


           


            if (Input.GetKeyDown(KeyCode.G) || rotating)
            {                
                JointsFrom = Points[0];
                JointsTo = Points[1];
                rotating = true;
              

                if (rotating)
                {
                    Debug.Log(Points[0][2]);
                    Debug.Log(Points[1][2]);
                    Vector3 from = new Vector3(0,90, Points[0][2]);
                    Vector3 to = new Vector3(0, 90, Points[1][2]);

                    if (Vector3.Distance(Robot[2].transform.eulerAngles, to) > 0.01f)
                    {
                            
                        Robot[2].transform.eulerAngles = Vector3.Lerp(from, to, Time.deltaTime*0.01f);
                    }
                    else
                    {
                        Robot[2].transform.eulerAngles = to;
                        rotating = false;
                    }
                }
                

            }
            */


        }


    }
}

