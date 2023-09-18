using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using System.IO;
using Godot;
public static class LocalSaveSystem
{
	private static Type[] types = { typeof(TownData), typeof(CurrentSlot), typeof(Statistics) };
	private static readonly string encryptionCodeWord = "T84783VMMST0R83jfdndA";

	public static void Save<T>(T data, string path)
	{
		if(!types.Contains(typeof(T)))
		{
			throw new ArgumentException("Type not supported");
		}
		try
		{
			Directory.CreateDirectory(Path.GetDirectoryName(path));
			// string dataToStore = JsonUtility.ToJson(data, true);
			//dataToStore = EncryptDecrypt(dataToStore);
			// using (FileStream stream = new FileStream(path, FileMode.Create))
			// {
			// 	using (StreamWriter writer = new StreamWriter(stream))
			// 	{
			// 		writer.Write(dataToStore);
			// 	}
			// }
		}
		catch (Exception e)
		{
			GD.PrintErr("Error occured when trying to save data to file: " + path + "\n" + e);
		}
	}

	public static T Load<T>(string path)
	{
		if (!types.Contains(typeof(T)))
		{
			throw new ArgumentException("Type not supported");
		}
		T loadedData = default(T);
		if (File.Exists(path))
		{
			try
			{
				string dataToLoad = "";
				// using (FileStream stream = new FileStream(path, FileMode.Open))
				// {
				// 	using (StreamReader reader = new StreamReader(stream))
				// 	{
				// 		dataToLoad = reader.ReadToEnd();
				// 	}
				// }
				//dataToLoad = EncryptDecrypt(dataToLoad);
				// loadedData = JsonUtility.FromJson<T>(dataToLoad);
			}
			catch (Exception e)
			{
				GD.Print("Error occured when trying to load data from file: " + path + "\n" + e);
				return default(T);
			}
		}
		return loadedData;
	}

	private static string EncryptDecrypt(string data)
	{
		string modifiedData = "";
		for (int i = 0; i < data.Length; i++)
		{
			modifiedData += (char)(data[i] ^ encryptionCodeWord[i % encryptionCodeWord.Length]);
		}
		return modifiedData;
	}
}
