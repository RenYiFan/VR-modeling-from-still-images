using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;
using UnityEngine.UI;

public class Measurement : MonoBehaviour {

    private VRTK_BasePointerRenderer usingBasicPointer = null;

    private LineRenderer areaLine;
    private int LengthOfLineRenderer = 0;
    private Vector3 position;
    private Vector3[] dis = new Vector3[1000];
    private double[] side = new double[1000];
    public Camera cameraN1;
    private double m = 1;
    public Text distenceN1;
    public Text areaN1;
    public Text volumeText;
    public Text surfaceText;
    private double areaNumber;
    private double distance = 0;
    public Button distenceButton;
    public Button areaButton;
    private bool distenceButtonClick = false;
    private bool areaButtonClick = false;
    private bool accurateButtonClick = false;
    private bool test1 = false;
    private bool test2 = false;
    private bool test3 = false;
    private double Adistance = 0;
    public Button accurateButton;
    public InputField accurateNumberInput;

    // Use this for initialization
    void Start () {


        Button disButton = distenceButton.GetComponent<Button>();
        disButton.onClick.AddListener(distenceButtonClicked);
        Button areaButtonFinish = areaButton.GetComponent<Button>();
        areaButtonFinish.onClick.AddListener(areaButtonClicked);

        this.usingBasicPointer = this.GetComponent<VRTK_BasePointerRenderer>();
        //Transform origin = usingBasicPointer.GetOrigin();

    }
	
	// Update is called once per frame
	void Update () {
        Transform origin = usingBasicPointer.GetOrigin();

        //  Debug.Log(origin.position);
        // Debug.Log(origin.forward);

        Ray pointerRaycast = new Ray(origin.position, origin.forward);
        RaycastHit pointerCollidedWith;
        //GameObject volumesum = pointerCollidedWith.transform.name
        GameObject volumesum = GameObject.Find("model/default");
        GameObject volumesum1 = GameObject.Find("model1/default");
        MeshCalculator vol = volumesum.GetComponent<MeshCalculator>();
        float volume = vol.sum;
        float surface = vol.sum1;
        MeshCalculator vol1 = volumesum1.GetComponent<MeshCalculator>();
        float volume1 = vol1.sum;
        float surface1 = vol1.sum1;
        areaLine = GetComponent<LineRenderer>();
        areaLine.SetColors(Color.yellow, Color.yellow);
        areaLine.SetWidth(0.01f, 0.01f);

        if (Input.GetKeyDown(KeyCode.B))
        {
            test1 = true;
            test2 = false;
            test3 = false;
            distance = 0;
            LengthOfLineRenderer = 0;
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            test2 = true;
            test1 = false;
            test3 = false;
            LengthOfLineRenderer = 0;
            areaNumber = 0;
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            test3 = true;
            test2 = false;
            test1 = false;
            LengthOfLineRenderer = 0;
            areaNumber = 0;
            m = 1;
        }

        //accurate calculate
        if (accurateButtonClick == true||test3==true)
        {
            areaLine.loop = false;
            if (Input.GetMouseButtonDown(0))
            {

                if (Physics.Raycast(pointerRaycast, out pointerCollidedWith))
                {
                    position = pointerCollidedWith.point;
                    LengthOfLineRenderer++;
                    if (LengthOfLineRenderer <= 2)
                    {
                        areaLine.SetVertexCount(LengthOfLineRenderer);
                        areaLine.SetPosition(LengthOfLineRenderer - 1, position);
                        dis[LengthOfLineRenderer - 1] = position;
                        if (LengthOfLineRenderer == 2)
                        {
                            Adistance = Vector3.Distance(dis[0], dis[1]);
                        }
                        else
                            Adistance = 0;
                    }
                }
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                LengthOfLineRenderer = 0;
                Adistance = 0;
            }
            InputField accurate = GameObject.Find("InputField").GetComponent<InputField>();
            string Saccurate = accurate.text.ToString();
            double Daccurate = System.Convert.ToDouble(Saccurate);
            m = Daccurate / Adistance;
            Debug.Log(m);
        }

        //length
        if (distenceButtonClick == true || test1 == true)
        {

            if (Input.GetMouseButtonDown(0))
            {

                if (Physics.Raycast(pointerRaycast, out pointerCollidedWith))
                {
                    position = pointerCollidedWith.point;
                    LengthOfLineRenderer++;
                    //       if (LengthOfLineRenderer <= 6)
                    //      {
                    areaLine.SetVertexCount(LengthOfLineRenderer);
                    areaLine.SetPosition(LengthOfLineRenderer - 1, position);
                    // areaLine.positionCount = dis.Length;
                    dis[LengthOfLineRenderer - 1] = position;
                    // dis[LengthOfLineRenderer - 1] = position;
                    if (LengthOfLineRenderer >= 2)
                        distance = distance + Vector3.Distance(dis[LengthOfLineRenderer - 2], dis[LengthOfLineRenderer - 1]);
                    else
                        distance = 0;
                    //      }
                }
                if (Input.GetKeyDown(KeyCode.R))
                {
                    LengthOfLineRenderer = 0;
                    distance = 0;
                    areaNumber = 0;
                    volume = 0;
                    surface = 0;
                }
                distenceN1.text = "distence:" + distance * m;
            }

        }

        //area measurement
        if (areaButtonClick == true || test2 == true)
        {

            if (Input.GetMouseButtonDown(0))
            {

                if (Physics.Raycast(pointerRaycast, out pointerCollidedWith))
                {
                    position = pointerCollidedWith.point;
                    LengthOfLineRenderer++;
                    if (LengthOfLineRenderer <= 6)
                    {
                        areaLine.SetVertexCount(LengthOfLineRenderer);
                        areaLine.SetPosition(LengthOfLineRenderer - 1, position);

                        dis[LengthOfLineRenderer - 1] = position;
                        if (LengthOfLineRenderer == 3)
                        {
                            areaLine.loop = true;
                            //Debug.Log(dis[0]);
                            // side[LengthOfLineRenderer - 1] = System.Math.Sqrt(System.Math.Pow(dis[LengthOfLineRenderer].x - dis[LengthOfLineRenderer - 1].x, 2)
                            //     + System.Math.Pow(dis[LengthOfLineRenderer].y - dis[LengthOfLineRenderer - 1].y, 2)
                            //     + System.Math.Pow(dis[LengthOfLineRenderer].z - dis[LengthOfLineRenderer - 1].z, 2));
                            side[0] = System.Math.Sqrt(System.Math.Pow(dis[1].x - dis[0].x, 2)
                                 + System.Math.Pow(dis[1].y - dis[0].y, 2)
                                 + System.Math.Pow(dis[1].z - dis[0].z, 2));
                            side[1] = System.Math.Sqrt(System.Math.Pow(dis[2].x - dis[1].x, 2)
                                  + System.Math.Pow(dis[2].y - dis[1].y, 2)
                                    + System.Math.Pow(dis[2].z - dis[1].z, 2));
                            side[2] = System.Math.Sqrt(System.Math.Pow(dis[0].x - dis[2].x, 2)
                                  + System.Math.Pow(dis[0].y - dis[2].y, 2)
                                  + System.Math.Pow(dis[0].z - dis[2].z, 2));

                            if (side[0] + side[1] <= side[2] || side[0] + side[2] <= side[1] || side[1] + side[2] <= side[0])
                            {
                                areaNumber = 0;
                            }
                            else
                            {
                                double p = (side[0] + side[1] + side[2]) / 2;
                                //Debug.Log(p);
                                areaNumber = System.Math.Sqrt(p * (p - side[0]) * (p - side[1]) * (p - side[2]));
                            }
                            //Debug.Log(dis[LengthOfLineRenderer].x);
                            //Debug.Log(side[0]);
                            // Debug.Log(areaNumber);
                        }
                        if (LengthOfLineRenderer == 4)
                        {
                            areaLine.loop = true;
                            side[0] = System.Math.Sqrt(System.Math.Pow(dis[1].x - dis[0].x, 2)
                                 + System.Math.Pow(dis[1].y - dis[0].y, 2)
                                 + System.Math.Pow(dis[1].z - dis[0].z, 2));
                            side[1] = System.Math.Sqrt(System.Math.Pow(dis[2].x - dis[1].x, 2)
                                  + System.Math.Pow(dis[2].y - dis[1].y, 2)
                                    + System.Math.Pow(dis[2].z - dis[1].z, 2));
                            side[2] = System.Math.Sqrt(System.Math.Pow(dis[0].x - dis[2].x, 2)
                                  + System.Math.Pow(dis[0].y - dis[2].y, 2)
                                  + System.Math.Pow(dis[0].z - dis[2].z, 2));
                            side[3] = System.Math.Sqrt(System.Math.Pow(dis[3].x - dis[2].x, 2)
                                + System.Math.Pow(dis[3].y - dis[2].y, 2)
                                + System.Math.Pow(dis[3].z - dis[2].z, 2));
                            side[4] = System.Math.Sqrt(System.Math.Pow(dis[0].x - dis[3].x, 2)
                                + System.Math.Pow(dis[0].y - dis[3].y, 2)
                                + System.Math.Pow(dis[0].z - dis[3].z, 2));
                            double p1 = (side[0] + side[1] + side[2]) / 2;
                            double p2 = (side[2] + side[3] + side[4]) / 2;
                            areaNumber = System.Math.Sqrt(p1 * (p1 - side[0]) * (p1 - side[1]) * (p1 - side[2]))
                                + System.Math.Sqrt(p2 * (p2 - side[2]) * (p2 - side[3]) * (p2 - side[4]));
                        }
                        if (LengthOfLineRenderer == 5)
                        {
                            areaLine.loop = true;
                            side[0] = System.Math.Sqrt(System.Math.Pow(dis[1].x - dis[0].x, 2)
                                 + System.Math.Pow(dis[1].y - dis[0].y, 2)
                                 + System.Math.Pow(dis[1].z - dis[0].z, 2));
                            side[1] = System.Math.Sqrt(System.Math.Pow(dis[2].x - dis[1].x, 2)
                                  + System.Math.Pow(dis[2].y - dis[1].y, 2)
                                    + System.Math.Pow(dis[2].z - dis[1].z, 2));
                            side[2] = System.Math.Sqrt(System.Math.Pow(dis[0].x - dis[2].x, 2)
                                  + System.Math.Pow(dis[0].y - dis[2].y, 2)
                                  + System.Math.Pow(dis[0].z - dis[2].z, 2));
                            side[3] = System.Math.Sqrt(System.Math.Pow(dis[3].x - dis[2].x, 2)
                                + System.Math.Pow(dis[3].y - dis[2].y, 2)
                                + System.Math.Pow(dis[3].z - dis[2].z, 2));
                            side[4] = System.Math.Sqrt(System.Math.Pow(dis[0].x - dis[3].x, 2)
                                + System.Math.Pow(dis[0].y - dis[3].y, 2)
                                + System.Math.Pow(dis[0].z - dis[3].z, 2));
                            side[5] = System.Math.Sqrt(System.Math.Pow(dis[4].x - dis[3].x, 2)
                                + System.Math.Pow(dis[4].y - dis[3].y, 2)
                                + System.Math.Pow(dis[4].z - dis[3].z, 2));
                            side[6] = System.Math.Sqrt(System.Math.Pow(dis[0].x - dis[4].x, 2)
                                + System.Math.Pow(dis[0].y - dis[4].y, 2)
                                + System.Math.Pow(dis[0].z - dis[4].z, 2));

                            double p1 = (side[0] + side[1] + side[2]) / 2;
                            double p2 = (side[2] + side[3] + side[4]) / 2;
                            double p3 = (side[4] + side[5] + side[6]) / 2;
                            areaNumber = System.Math.Sqrt(p1 * (p1 - side[0]) * (p1 - side[1]) * (p1 - side[2]))
                                + System.Math.Sqrt(p2 * (p2 - side[2]) * (p2 - side[3]) * (p2 - side[4]))
                                + System.Math.Sqrt(p3 * (p3 - side[4]) * (p3 - side[5]) * (p3 - side[6]));
                        }

                    }
                }
                if (Input.GetKeyDown(KeyCode.R))
                {
                    LengthOfLineRenderer = 0;
                    areaNumber = 0;
                    volume = 0;
                    surface = 0;
                }

                areaN1.text = "area:" + areaNumber * m;
            }

        }

        //Read volmue
        if (Input.GetMouseButtonDown(0))
        {

            if (Physics.Raycast(pointerRaycast, out pointerCollidedWith))
            {
                if (pointerCollidedWith.transform.tag == "model")
                {
                    volumeText.text = "Volume= " + Mathf.Abs(volume)*m*m*m;
                    surfaceText.text = "Surface area = " + surface*m*m;
                    //Debug.Log("Volume= " + Mathf.Abs(volume));
                    // Debug.Log("Area = " + surface);
                }
                if (pointerCollidedWith.transform.tag == "model1")
                {
                    volumeText.text = "Volume= " + Mathf.Abs(volume1) * m * m * m;
                    surfaceText.text = "Surface area = " + surface1 * m * m;
                    //Debug.Log("Volume= " + Mathf.Abs(volume));
                    // Debug.Log("Area = " + surface);
                }
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                LengthOfLineRenderer = 0;
                areaNumber = 0;
                volume = 0;
                surface = 0;
            }

        }

        // GameObject.Find("model/default").SendMessage("MeshCalculator", "sum");

    }



    void distenceButtonClicked()
    {
        accurateButtonClick = false;
        areaButtonClick = false;
        distenceButtonClick = true;
        LengthOfLineRenderer = 0;
        distance = 0;
        // lineRenderer.enabled = true;
    }
    void areaButtonClicked()
    {
        accurateButtonClick = false;
        areaButtonClick = true;
        distenceButtonClick = false;
        LengthOfLineRenderer = 0;
        areaNumber = 0;
        // Destroy(lineRenderer.gameObject, 1);
    }
    void accurateButtonClicked()
    {
        accurateButtonClick = true;
        areaButtonClick = false;
        distenceButtonClick = false;
        LengthOfLineRenderer = 0;
        m = 1;
        // Destroy(lineRenderer.gameObject, 1);
    }

}
