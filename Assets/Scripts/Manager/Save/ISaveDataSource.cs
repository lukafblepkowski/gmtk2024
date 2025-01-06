using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISaveDataSource
{
    void LoadData(GameData data);
    void SaveData(ref GameData data); //Reference so it may be modified and then further added upon by other savedatasources
}
