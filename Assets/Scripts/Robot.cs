using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using System.Globalization;

namespace Factory_VR
{
    [Serializable]
    public class Robot
    {

        public string name;
        public int JointNumber;
        public Quaternion[,] Exit;
        public string path;

        public string dir = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        private string dirProgs = "";
        public GameObject[] RobotModel = new GameObject[7];
        public GameObject[] Robot_Obj = new GameObject[7];
        public Quaternion[] Joints = new Quaternion[7];
        public float[] JointsFrom = new float[7];
        public float[] JointsTo = new float[7];
        public Material SelectedMaterial;
        public Material NormalMaterial;
        public float jointSpeed = 1f;
        public float[] ZeroJoint = new float[7];
        public float[] JointAngle = new float[7];
        public float[] JointsTest = new float[7];
        public float[,] Program = new float[999, 7];
        public int ProgramEmptyLine = 0;
        public float[] QuaternionDistance = new float[7];


        private int RobotJoint = 1;

        public float[] Difference;

        //Classe para construir robôs Industriais 6DoF esféricos
        public Robot(string _name, int _JointNumber)
        {

            name = _name;
            JointNumber = _JointNumber + 1;

            //Cria uma pasta para o Robô
            string diraux = dir + @"\" + name;
            //Debug.Log(diraux);
            // If directory does not exist, create it
            if (!Directory.Exists(diraux))
            {
                Directory.CreateDirectory(diraux);
            }

            dirProgs = diraux + @"\Programs";
            //Debug.Log(dirProgs);
            // If directory does not exist, create it
            if (!Directory.Exists(dirProgs))
            {
                Directory.CreateDirectory(dirProgs);
            }

            FileStream stream = new FileStream(dirProgs + @"\ini.txt", FileMode.OpenOrCreate);
            StreamWriter RobotWriter = new StreamWriter(stream);
            RobotWriter.Write(name);
            RobotWriter.WriteLine();
            RobotWriter.Write("Welcome");
            RobotWriter.Close();
        }

        public void PrintJointTest()
        {
            for (int i = 0; i < JointNumber; i++)
            {
                Debug.Log(Robot_Obj[i].transform.localEulerAngles.z);
            }
        }
        //Executa Inicializações do Robô
        public void RobotInitiate(float[] _ZeroJoint)
        {
            ZeroJoint = _ZeroJoint;
            RobotModel[1].GetComponent<MeshRenderer>().material = SelectedMaterial;
            for (int i = 2; i <= 6; i++)
                RobotModel[i].GetComponent<MeshRenderer>().material = NormalMaterial;

            JointAngle = ZeroJoint;
        }

        //Rotina de Animação de Seleção de Junnta
        public void Selection() //Select a Joint and Show in Green
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

        //Rotina de Manipulçao de Junta Selecionada
        public void Manipulate() //Manipulate a Selected Joint
        {


            if (Input.GetKey(KeyCode.LeftArrow))
            {
                JointAngle[RobotJoint] += jointSpeed * Time.deltaTime;
                Robot_Obj[RobotJoint].transform.Rotate(Vector3.forward * jointSpeed * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                JointAngle[RobotJoint] -= jointSpeed * Time.deltaTime;
                Robot_Obj[RobotJoint].transform.Rotate(-Vector3.forward * jointSpeed * Time.deltaTime);
            }

        }

        //Retorna o valor da posição x,y,z do endEffector
        public Vector3 GetEndEffectorPosition()
        {
            Vector3 PositionXYZ = 100 * Robot_Obj[0].transform.InverseTransformPoint(Robot_Obj[6].transform.position);
            return PositionXYZ;
        }

        //Retorna os Qauternions da Posição de junta atual
        public Quaternion[] GetInstantJointPoint()
        {
            Quaternion[] JointGroup = new Quaternion[7];
            for (int i = 0; i <= JointNumber; i++)
            {
                JointGroup[i] = Robot_Obj[i].transform.localRotation;
            }

            return JointGroup;
        }

        public void CreateProgram(string programName)
        {
            programName = dirProgs + programName;
            using (FileStream fs = File.Create(programName))
            {
                // Add some text to file    
                Byte[] title = new UTF8Encoding(true).GetBytes("ProgramYaskawa");
                fs.Write(title, 0, title.Length);
            }
        }

        //Escreve uma linha com um ponto em Quaternion em um arquivo
        public void WriteProgram(Quaternion[] Point, string programName)
        {
            programName = dirProgs + programName;
            //Debug.Log(programName);          

            try
            {
                string Pos = "";
                for (int i = 0; i <= JointNumber; i++)
                {
                    if (i < JointNumber)
                        Pos = Pos + Point[i].ToString("G") + ";";
                    else
                        Pos = Pos + Point[i].ToString();
                }

                // Create a new file     
                using (StreamWriter fs = File.AppendText(programName))
                {
                    fs.WriteLine(Pos);
                    // Debug.Log("Escrito");
                }

            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.ToString());
            }
        }

        //Apaga o arquiuvo de Prgrama
        public void CleanProgram()
        {
            FileStream fileStream = File.Open(path, FileMode.Open);

            fileStream.SetLength(0);
            fileStream.Close();
        }

        //Escreve a Junta atual no Console
        public string PrintJointPosition()
        {

            string Pos = "";
            string pos2file = "";
            for (int i = 1; i <= JointNumber; i++)
            {
                Pos = Pos + "   J" + (i).ToString() + ": " + JointAngle[i].ToString();
                if (i != JointNumber)
                    pos2file = pos2file + JointAngle[i].ToString() + ";";
                else
                    pos2file = pos2file + JointAngle[i].ToString();             
            }           
            return Pos;
        }

        //Decodifica o Arquivo de Programa
        public Quaternion[,] SplitFileString(string file)
        {
            string path = dirProgs + @"\" + file + @".txt";
            int j;
            string[] JointStrings = new string[1000];
            int i = 1;
            StreamReader reader = new StreamReader(path);

            JointStrings[0] = reader.ReadLine();
            while (JointStrings[i - 1] != null)
            {
                JointStrings[i] = reader.ReadLine();
                i++;
            }
            int progSize = i - 1;

            Quaternion[,] ReadJoint = new Quaternion[progSize, JointNumber+1];

            for (i = 0; i <= progSize - 1; i++)
            {
                string[] sArray = JointStrings[i].Split(';');
                for (j = 0; j <= JointNumber; j++)
                {
                    ReadJoint[i, j] = StringToQuaternion(sArray[j]);
                }
            }
            return ReadJoint;
        }

        //Converte String para Quaternion
        public static Quaternion StringToQuaternion(string sQuaternion)
        {
            // Remove the parentheses
            if (sQuaternion.StartsWith("(") && sQuaternion.EndsWith(")"))
            {
                sQuaternion = sQuaternion.Substring(1, sQuaternion.Length - 2);
            }
            // split the items
            string[] sArray = sQuaternion.Split(',');

            // store as a Vector3
            Quaternion result = new Quaternion(
                float.Parse(sArray[0], CultureInfo.InvariantCulture),
                float.Parse(sArray[1], CultureInfo.InvariantCulture),
                float.Parse(sArray[2], CultureInfo.InvariantCulture),
                float.Parse(sArray[3], CultureInfo.InvariantCulture));
            return result;
        }

        public float[] CalculateQuaternionDistance(Quaternion[,] A, int JointNumber, int index)
        {
            float[] QuaternionDistance = new float[JointNumber + 1];
            float[] SpeedCoefficient = new float[JointNumber + 1];
            float maxValue = 0;

            for (int i = 0; i <= JointNumber; i++)
            {
                QuaternionDistance[i] = Quaternion.Angle(A[index-1,i].normalized, A[index,i].normalized);
                if (QuaternionDistance[i] > maxValue)
                    maxValue = QuaternionDistance[i];
                //Debug.Log("J"+ i + ":"+QuaternionDistance[i]);
            }          

            for (int i = 0; i <= JointNumber; i++)
            {
                SpeedCoefficient[i] = (QuaternionDistance[i] / maxValue);
                //Debug.Log("J"+ i + ":"+SpeedCoefficient[i]);
            }
            return SpeedCoefficient;
        }



        //Move o Robô para a posição desejada no programa
        public int SetRobotToPositionNOW(string file, int line, float speed)
        {            
            
            string path = dirProgs + @"\" + file + @".txt";
            var lineCount = File.ReadAllLines(path).Length;

            if (line < lineCount)
            {
                Quaternion[,] ReadJoint = new Quaternion[lineCount, 7];
                ReadJoint = SplitFileString(file);
                Exit = ReadJoint;
                float[] QuaternionDistance = new float[7];

                for (int i = 0; i <= JointNumber; i++)
                {
                    Robot_Obj[i].transform.localRotation = ReadJoint[line, i];
                }
                return 1;
            }
            else
            {
                return -1;
            }
            
          

        }//SetRobotToPositionNOW()

        public int SetRobotToPosition(string file, int line, float speed)
        {


            int flag = 0;
            string path = dirProgs + @"\" + file + @".txt";
            var lineCount = File.ReadAllLines(path).Length;
            float[] speedVec = new float[JointNumber+1];

            if (line < lineCount)
            {
                Quaternion[,] ReadJoint = new Quaternion[lineCount, JointNumber+1];
                ReadJoint = SplitFileString(file);
                Exit = ReadJoint;
                

                QuaternionDistance = CalculateQuaternionDistance(ReadJoint, 6, line);
                for (int i = 0; i <= JointNumber; i++)
                {
                    speedVec[i] = speed * QuaternionDistance[i];    
                }

                    for (int i = 0; i <= JointNumber; i++)
                {
                    Robot_Obj[i].transform.localRotation = Quaternion.RotateTowards(Robot_Obj[i].transform.localRotation, ReadJoint[line, i],  speedVec[i]);
                    Difference[i] = Quaternion.Angle(Robot_Obj[i].transform.localRotation, ReadJoint[line, i]);

                    if (Math.Abs(Quaternion.Angle(Robot_Obj[i].transform.localRotation, ReadJoint[line, i])) < 0.001)
                    {
                        flag++;
                        Debug.Log(flag);
                    }
                }


                //Debug.Log(flag);
                if (flag == 7)
                    return 1;
                else
                    return 0;
            }
            else
                return -1;
        }//SetRobotToPosition()



        //public void RunProgram(string file, float speed)
        //{
        //    int i = 1;
        //    int step = 0;
        //    while(i<4)
        //    { 
        //        step = SetRobotToPositionNOW(file, i, speed);
        //        Debug.Log(i);
        //        if (step == 1)
        //            i++;
        //    }
        //}
    }//class Robot
}//namespace
