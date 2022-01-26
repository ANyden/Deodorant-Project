using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using NoteKey;

[ExecuteAlways]
public class NotesCollection : MonoBehaviour
{
    public List<noteKey> keysMasterList;
    public List<noteKey> keysAlpha;
    public List<noteKey> keysNumeric;


    public enum gameMode { alpha, numeric };
    public gameMode gameModeSetTo;


    void masterListInUse()
    {
        switch (gameModeSetTo)
        {
            case gameMode.alpha:
                keysMasterList = keysAlpha;
                break;
            case gameMode.numeric:
                keysMasterList = keysNumeric;
                break;
        }
    }
    void Start()
    {
        masterListInUse();
    }

    
}
