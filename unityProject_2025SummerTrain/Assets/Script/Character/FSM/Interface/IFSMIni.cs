using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFSMIni
{
    public int getID();
    public void setID(int id);
    // 初始化
    public void Init(int ID);
}
