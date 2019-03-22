using UnityEngine;
using System.IO;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;

/// <summary>
/// The all in one data saving class.
/// </summary>
/// <typeparam name="T"></typeparam>
public static class Data<T>
{
	/// <summary>
	/// This is assigned to the Out LoadData incase it fails to find the proper file. 
	/// </summary>
	static T blank;

	
	public static void DeleteSave(string FileName)
	{
        Debug.Log(Application.persistentDataPath + "/" + FileName + ".Dat");
        File.Delete(Application.persistentDataPath+"/"+FileName+".Dat");
	}

	/// <summary>
	/// Will create a new file with the name FileName and save the T saveData to it.
	/// </summary>
	/// <param name="FileName"></param>
	/// <param name="SaveData"></param>
	public static void SaveInstant(string FileName, T SaveData)
	{
        Debug.Log(Application.persistentDataPath + "/" + FileName + ".Dat");
        BinaryFormatter binary = new BinaryFormatter();
		//Creates a new file at the correct location using the FileName given.
		FileStream file = File.Create(Application.persistentDataPath + "/"+FileName+".Dat");
		//Saves the given data.
		binary.Serialize(file, SaveData);
		//Closes the folder.
		file.Close();
	}

	/// <summary>
	/// Will create a new file with the name FileName and save the T saveData to it.
	/// </summary>
	/// <param name="FileName"></param>
	/// <param name="SaveData"></param>
	public static IEnumerator SaveDistributed(string FileName, T SaveData)
	{
        Debug.Log(Application.persistentDataPath + "/" + FileName + ".Dat");
        BinaryFormatter binary = new BinaryFormatter();
		yield return null;
		//Creates a new file at the correct location using the FileName given.
		FileStream file;
		yield return null;
		file = File.Create(Application.persistentDataPath+"/"+FileName+".Dat");
		yield return null;
		//Saves the given data.
		binary.Serialize(file, SaveData);
		yield return null;
		//Closes the folder.
		file.Close();
		yield return null;
	}


	//Will find the existing saved data and assign it to the T LoadData given.
	public static T Load(string FileName)
	{
		Debug.Log(Application.persistentDataPath+"/"+FileName+".Dat");
		//If the file exists at the given location..
		if(File.Exists(Application.persistentDataPath+"/"+FileName+".Dat"))
		{
			BinaryFormatter binary = new BinaryFormatter();
			//Opens the file at the location using the FileName given.
			FileStream file = File.Open(Application.persistentDataPath+"/"+FileName+".Dat", FileMode.Open);
			//Assigns the saved data to the new Out T LoadData.
			T dat = (T)binary.Deserialize(file);
			file.Close();
			return dat;
        }
		else
		{
			return blank;
        }
	}

	public static bool CheckForSave(string FileName)
	{
        Debug.Log(Application.persistentDataPath + "/" + FileName + ".Dat");

		//If the file exists at the given location..
		if(File.Exists(Application.persistentDataPath+"/"+FileName+".Dat"))
		{
			return true;
		}
		else
		{
			return false;
		} 
	}
}