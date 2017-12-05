using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockMusseumObjs : MonoBehaviour {

    [SerializeField]
    private MusseumObject[] musseumObjects;

    private List<int> unlockObjects;

    private void Start()
    {
        //PlayerPrefs.SetString("UnlockObjects", "");
        EnableObjsMusseum();
    }

    public void UnlockObj (int id)
    {
        if (!unlockObjects.Contains(id))
        {
            for (int i = 0; i < musseumObjects.Length; i++)
            {
                if (musseumObjects[i].GetId() == id)
                {
                    musseumObjects[i].gameObject.SetActive(true);
                    unlockObjects.Add(id);
                    PlayerPrefs.SetString("UnlockObjects", CastIntListToString(unlockObjects, ","));
                }
            }
        }
	}

    public void EnableObjsMusseum()
    {
        unlockObjects = CastStringToIntList(PlayerPrefs.GetString("UnlockObjects"), ',');

        for (int i = 0; i < musseumObjects.Length; i++)
        {
            if (unlockObjects.Contains(musseumObjects[i].GetId()))
            {
                musseumObjects[i].gameObject.SetActive(true);
            }
        }
    }

    private List<int> CastStringToIntList(string stringToCast, char separator)
    {
        if (stringToCast != "")
        {
            string[] strgArray = stringToCast.Split(separator);
            List<int> intList = new List<int>();

            for (int i = 0; i < strgArray.Length; i++)
            {
                intList.Add(int.Parse(strgArray[i]));
            }

            return intList;
        }
        else
        {
            return new List<int>();
        }

    }

    private string CastIntListToString(List<int> intListToCast, string separator)
    {
        int sizeList = intListToCast.Count;
        string[] castString = new string[intListToCast.Count];

        for (int i = 0; i < sizeList; i++)
        {
            castString[i] = "" + intListToCast[i];
        }

        return string.Join(separator, castString);
    }

}
