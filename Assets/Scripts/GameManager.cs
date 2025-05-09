using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class GameManager : MonoBehaviour 
{ 
    static private GameManager instance;
    static public GameManager Instance {  get { return instance; } }
    [SerializeField] private MovementController player;
}