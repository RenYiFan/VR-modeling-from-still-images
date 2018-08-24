using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class distence : MonoBehaviour
{

    private LineRenderer lineRenderer;
    private Vector3 position;
    private Vector3[] dis = new Vector3[1000];
    private double distance = 0;
    private int index = 0;
    private int LengthOfLineRenderer = 0;
    public Camera cameraN1;
    RaycastHit hit = new RaycastHit();
    public Text distenceN1;
    public Text areaN1;
    public Button distenceButton;
    public Button areaButton;
    public Button accurateButton;
    private bool distenceButtonClick=false;
    private bool areaButtonClick = false;
    private bool accurateButtonClick = false;
    private LineRenderer areaLine;
    private double[] side = new double[1000];
    private double areaNumber;
    public InputField accurateNumberInput;
    private double m = 1;
    private double Adistance = 0;


    //Initialization
    void Start()
    {
       
        distenceN1.text = "distence:" + distance;
        areaN1.text = "area:" + areaNumber;
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        //lineRenderer.material = new Material(Shader.Find("Materials/green"));
        lineRenderer.SetColors(Color.yellow, Color.yellow);
        lineRenderer.SetWidth(0.03f, 0.03f);
        //areaLine = gameObject.AddComponent<LineRenderer>();
      //  areaLine.SetColors(Color.yellow, Color.yellow);
       // areaLine.SetWidth(0.01f, 0.01f);

        Button disButton = distenceButton.GetComponent<Button>();
        Button areaButtonFinish = areaButton.GetComponent<Button>();
        Button accurateButtonFinish = accurateButton.GetComponent<Button>();
        disButton.onClick.AddListener(distenceButtonClicked);
        areaButtonFinish.onClick.AddListener(areaButtonClicked);
        accurateButtonFinish.onClick.AddListener(accurateButtonClicked);

    }

    void Update() {
        pointDistence();
        areaMeasure();
        accurateNumber();

    }

    void distenceButtonClicked() {
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

    public void accurateNumber() {
        if (accurateButtonClick == true)
        {
            lineRenderer = GetComponent<LineRenderer>();
            lineRenderer.loop = false;
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    position = hit.point;
                    LengthOfLineRenderer++;
                    if (LengthOfLineRenderer <= 2)
                    {
                        lineRenderer.SetVertexCount(LengthOfLineRenderer);
                        lineRenderer.SetPosition(LengthOfLineRenderer - 1, position);
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
        }
       // Debug.Log(m);
    }
    public void pointDistence()
    {
        if (distenceButtonClick == true)
        {
            lineRenderer = GetComponent<LineRenderer>();
            lineRenderer.loop = false;
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    position = hit.point;
                    LengthOfLineRenderer++;
                    lineRenderer.SetVertexCount(LengthOfLineRenderer);
                    lineRenderer.SetPosition(LengthOfLineRenderer - 1, position);
                    dis[LengthOfLineRenderer - 1] = position;
                    if (LengthOfLineRenderer >= 2)
                        distance = distance + Vector3.Distance(dis[LengthOfLineRenderer - 2], dis[LengthOfLineRenderer - 1]);
                    else
                        distance = 0;
                }
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                LengthOfLineRenderer = 0;
                distance = 0;
            }
            distenceN1.text = "distence:" + distance*m;
        }
    }

    public void areaMeasure() {
        if (areaButtonClick == true)
        {
            lineRenderer = GetComponent<LineRenderer>();
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    position = hit.point;
                    LengthOfLineRenderer++;
                    //areaLine.SetVertexCount(LengthOfLineRenderer);
                    //areaLine.SetPosition(LengthOfLineRenderer - 1, position);


                    // Debug.Log(position);
                    //LengthOfLineRenderer++;
                    if (LengthOfLineRenderer <= 6)
                    {
                        lineRenderer.SetVertexCount(LengthOfLineRenderer);
                        lineRenderer.SetPosition(LengthOfLineRenderer - 1, position);
                        // areaLine.positionCount = dis.Length;
                        dis[LengthOfLineRenderer - 1] = position;

                        // Debug.Log(LengthOfLineRenderer);
                        if (LengthOfLineRenderer == 3)
                        {
                            lineRenderer.loop = true;
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
                            lineRenderer.loop = true;
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
                            lineRenderer.loop = true;
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
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                LengthOfLineRenderer = 0;
                areaNumber = 0;
            }

            areaN1.text = "area:" + areaNumber*m;
        }

    }
}
