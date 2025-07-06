using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMIni_1002 : MonoBehaviour, IFSMIni
{
    private FSM_1002 fSM => GetComponent<FSM_1002>();
    private IParameterController parameterController => GetComponent<IParameterController>();
    private int ID;

    private void OnEnable()
    {
        // parameterController.Init(ID);

        // fSM.enabled = true;
    }
    public int getID()
    {
        return ID;
    }

    public void setID(int id)
    {
        ID = id;
    }
    // 初始化
    public void Init(int ID)
    {
        parameterController.Init(ID);

        fSM.enabled = true;
    }
}
