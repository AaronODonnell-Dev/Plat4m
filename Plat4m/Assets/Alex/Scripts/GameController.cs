using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
public class GameController : MonoBehaviour
{
	public static GameController controller;
	public int pipHealth;
	public float squeakHealth;
	public int minorCollectables;
	public int majorCollectabels;
	Transform pPositon;
	void Awake()
	{
		pPositon = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
	}

	public void Save()
	{

		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Open(Application.persistentDataPath + "/PlayerSave.dat", FileMode.Create);

		// Data Container
		PlayerData data = new PlayerData();

		//-----------------------------------------
		//All player Data

		data.pipHealth = pipHealth;
		data.squeakHealth = squeakHealth;
		data.minorCollectables = minorCollectables;
		data.majorCollectabels = majorCollectabels;
		data.pPosX = pPositon.position.x;
		data.pPosY = pPositon.position.y;
		data.pPosZ = pPositon.position.z;
		data.pRotX = pPositon.rotation.x;
		data.pRotY = pPositon.rotation.y;
		data.pRotZ = pPositon.rotation.z;
		
		//------------------------------------------

		bf.Serialize(file, data);
		file.Close();

	}
	public void Load()
	{
		if (File.Exists(Application.persistentDataPath + "/PlayerSave.dat"))
		{
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath + "/PlayerSave.dat", FileMode.Open);
			PlayerData data = (PlayerData)bf.Deserialize(file);

			pipHealth = data.pipHealth;
			squeakHealth = data.squeakHealth;
			minorCollectables = data.minorCollectables;
			majorCollectabels = data.majorCollectabels;
			pPositon.position = new Vector3(data.pPosX, data.pPosY, data.pPosZ);
			pPositon.rotation = new Quaternion(data.pRotX, data.pRotY, data.pRotZ, 1);
		}

	}
}
[Serializable]
class PlayerData
{
	public int pipHealth;
	public float squeakHealth;
	public int minorCollectables;
	public int majorCollectabels;
	public float pPosX;
	public float pPosY;
	public float pPosZ;
	public float pRotX;
	public float pRotY;
	public float pRotZ;

}


