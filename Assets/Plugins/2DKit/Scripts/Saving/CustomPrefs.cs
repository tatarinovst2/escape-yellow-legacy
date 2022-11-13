/*
	PreviewLabs.PlayerPrefs
	April 1, 2014 version

	Public Domain
	
	To the extent possible under law, PreviewLabs has waived all copyright and related or neighboring rights to this document. This work is published from: Belgium.
	
	http://www.previewlabs.com
	
*/

/*
	This is based on PreviewLabs.PlayerPrefs

	Changes:
	Removed encryption
	Removed long-type support
	Added Vector support
	Refactored default values for GetTYPE() methods
	Made savefiles more readable for the user
	Custom filenames
	Multiple instances (For example: one for in-game values, one for settings)
	Added version control

	v1.0 / 1
	Started version control

	v1.0.1 / 2
	Debugged version control
*/

using UnityEngine;
using System;
using System.Collections;
using System.IO;
using System.Text;

namespace Kit.Saving
{
	public class CustomPrefs
	{
		private readonly Hashtable _playerPrefsHashtable = new Hashtable();
		private bool _hashTableChanged = false;
		private string _serializedOutput = "";
		private string _serializedInput = "";
		private const string PARAMETERS_SEPERATOR = "\n";
		private const string KEY_VALUE_SEPERATOR = ":";
		private const string VECTOR_VALUES_SEPERATOR = "|";
		private string[] _seperators = new string[] { PARAMETERS_SEPERATOR, KEY_VALUE_SEPERATOR };
		public string fileName = Application.persistentDataPath + "/DefaultSave.txt";

		public const string VERSION_NAME = "v1.0.1";
		public const int VERSION = 2;

		public CustomPrefs(string pathEnding)
		{
			fileName = Application.persistentDataPath + pathEnding;

			StreamReader fileReader = null;

			if (File.Exists(fileName))
			{
				fileReader = new StreamReader(fileName);
				_serializedInput = fileReader.ReadToEnd();
			}

			if (!string.IsNullOrEmpty(_serializedInput))
			{
				Deserialize();
			}

			if (fileReader != null)
			{
				fileReader.Close();
			}
		}

		public bool HasKey(string key)
		{
			return _playerPrefsHashtable.ContainsKey(key);
		}

		public void SetString(string key, string value)
		{
			if (!_playerPrefsHashtable.ContainsKey(key))
			{
				_playerPrefsHashtable.Add(key, value);
			}
			else
			{
				_playerPrefsHashtable[key] = value;
			}

			_hashTableChanged = true;
		}

		public void SetInt(string key, int value)
		{
			if (!_playerPrefsHashtable.ContainsKey(key))
			{
				_playerPrefsHashtable.Add(key, value);
			}
			else
			{
				_playerPrefsHashtable[key] = value;
			}

			_hashTableChanged = true;
		}

		public void SetFloat(string key, float value)
		{
			if (!_playerPrefsHashtable.ContainsKey(key))
			{
				_playerPrefsHashtable.Add(key, value);
			}
			else
			{
				_playerPrefsHashtable[key] = value;
			}

			_hashTableChanged = true;
		}

		public void SetBool(string key, bool value)
		{
			if (!_playerPrefsHashtable.ContainsKey(key))
			{
				_playerPrefsHashtable.Add(key, value);
			}
			else
			{
				_playerPrefsHashtable[key] = value;
			}

			_hashTableChanged = true;
		}

		public string GetString(string key, string defaultValue = "")
		{
			if (_playerPrefsHashtable.ContainsKey(key))
			{
				return _playerPrefsHashtable[key].ToString();
			}

			return defaultValue;
		}

		public int GetInt(string key, int defaultValue = 0)
		{
			if (_playerPrefsHashtable.ContainsKey(key))
			{
				return (int)_playerPrefsHashtable[key];
			}

			return defaultValue;
		}

		public float GetFloat(string key, float defaultValue = 0.0f)
		{
			if (_playerPrefsHashtable.ContainsKey(key))
			{
				return (float)_playerPrefsHashtable[key];
			}

			return defaultValue;
		}

		public bool GetBool(string key, bool defaultValue = false)
		{
			if (_playerPrefsHashtable.ContainsKey(key))
			{
				if (_playerPrefsHashtable[key].GetType() == typeof(bool))
				{
					return (bool)_playerPrefsHashtable[key];
				}
				else if (_playerPrefsHashtable[key].GetType() == typeof(int))
				{
					return ((int)_playerPrefsHashtable[key] != 0);
				}
			}

			return defaultValue;
		}

		// My code

		public void SetVector2(string key, Vector2 value)
		{
			if (!_playerPrefsHashtable.ContainsKey(key))
			{
				_playerPrefsHashtable.Add(key, value);
			}
			else
			{
				_playerPrefsHashtable[key] = value;
			}

			_hashTableChanged = true;
		}

		public Vector2 GetVector2(string key, Vector2 defaultValue = new Vector2())
		{
			if (_playerPrefsHashtable.ContainsKey(key))
			{
				return (Vector2)_playerPrefsHashtable[key];
			}

			return defaultValue;
		}

		public void SetVector3(string key, Vector3 value)
		{
			if (!_playerPrefsHashtable.ContainsKey(key))
			{
				_playerPrefsHashtable.Add(key, value);
			}
			else
			{
				_playerPrefsHashtable[key] = value;
			}

			_hashTableChanged = true;
		}

		public Vector3 GetVector3(string key, Vector3 defaultValue = new Vector3())
		{
			if (_playerPrefsHashtable.ContainsKey(key))
			{
				return (Vector3)_playerPrefsHashtable[key];
			}

			return defaultValue;
		}

		//

		public void DeleteKey(string key)
		{
			_playerPrefsHashtable.Remove(key);
		}

		public void DeleteAll()
		{
			_playerPrefsHashtable.Clear();
		}

		public void Flush()
		{
			SetString("CustomPrefs Version", VERSION_NAME);
			SetInt("CustomPrefs Version Count", VERSION);

			if (_hashTableChanged)
			{
				Serialize();

				string output = (_serializedOutput);

				string folder = new DirectoryInfo(Path.GetDirectoryName(fileName)).FullName;

				if (!Directory.Exists(folder))
				{
					Directory.CreateDirectory(folder);
				}

				StreamWriter fileWriter = File.CreateText(fileName);

				if (fileWriter == null)
				{
					Debug.LogWarning("PlayerPrefs::Flush() opening file for writing failed: " + fileName);
					return;
				}

				fileWriter.Write(output);

				fileWriter.Close();

				_serializedOutput = "";
			}
		}

		private void Serialize()
		{
			IDictionaryEnumerator myEnumerator = _playerPrefsHashtable.GetEnumerator();
			StringBuilder sb = new StringBuilder();
			bool firstString = true;

			while (myEnumerator.MoveNext())
			{
				if (!firstString)
				{
					sb.Append(PARAMETERS_SEPERATOR);
				}

				sb.Append(EscapeNonSeperators(myEnumerator.Key.ToString(), _seperators));
				sb.Append(" ");
				sb.Append(KEY_VALUE_SEPERATOR);
				sb.Append(" ");
				sb.Append(EscapeNonSeperators(ToString(myEnumerator.Value), _seperators));
				sb.Append(" ");
				sb.Append(KEY_VALUE_SEPERATOR);
				sb.Append(" ");
				sb.Append(GetType(myEnumerator.Value));
				firstString = false;
			}

			_serializedOutput = sb.ToString();

			Type GetType(object value)
            {
				if (value is Vector2)
                {
					return typeof(Vector2);
                }

				if (value is Vector3)
                {
					return typeof(Vector3);
                }

				return value.GetType();
			}

			string ToString(object value)
            {
				if (value is Vector2)
				{
					return ((Vector2)value).x + VECTOR_VALUES_SEPERATOR + ((Vector2)value).y;
 				}

				if (value is Vector3)
				{
					return ((Vector3)value).x + VECTOR_VALUES_SEPERATOR + ((Vector3)value).y + VECTOR_VALUES_SEPERATOR + ((Vector3)value).z;
				}

				return value.ToString();
			}
		}

		private void Deserialize()
		{
			string[] parameters = _serializedInput.Split(new string[] { PARAMETERS_SEPERATOR }, StringSplitOptions.RemoveEmptyEntries);

			foreach (string parameter in parameters)
			{
				string[] parameterContent = parameter.Split(new string[] { " " + KEY_VALUE_SEPERATOR + " " }, StringSplitOptions.None);

				_playerPrefsHashtable.Add(DeEscapeNonSeperators(parameterContent[0], _seperators), GetTypeValue(parameterContent[2], DeEscapeNonSeperators(parameterContent[1], _seperators)));

				if (parameterContent.Length > 3)
				{
					Debug.LogWarning("PlayerPrefs::Deserialize() parameterContent has " + parameterContent.Length + " elements");
				}
			}
		}

		private string EscapeNonSeperators(string inputToEscape, string[] seperators)
		{
			inputToEscape = inputToEscape.Replace("\\", "\\\\");

			for (int i = 0; i < seperators.Length; ++i)
			{
				inputToEscape = inputToEscape.Replace(seperators[i], "\\" + seperators[i]);
			}

			return inputToEscape;
		}

		private string DeEscapeNonSeperators(string inputToDeEscape, string[] seperators)
		{
			for (int i = 0; i < seperators.Length; ++i)
			{
				inputToDeEscape = inputToDeEscape.Replace("\\" + seperators[i], seperators[i]);
			}

			inputToDeEscape = inputToDeEscape.Replace("\\\\", "\\");

			return inputToDeEscape;
		}

		// 

		private object GetTypeValue(string typeName, string value)
		{
			if (typeName == "System.String")
			{
				return value.ToString();
			}

			if (typeName == "System.Int32")
			{
				return Convert.ToInt32(value);
			}

			if (typeName == "System.Boolean")
			{
				return Convert.ToBoolean(value);
			}

			if (typeName == "System.Single") //float
			{
				return Convert.ToSingle(value);
			}

			if (typeName == "System.Int64") //long
			{
				return Convert.ToInt64(value);
			}

			if (typeName == "UnityEngine.Vector2")
            {
				return ConvertToVector2(value);
            }

			if (typeName == "UnityEngine.Vector3")
			{
				return ConvertToVector3(value);
			}

			Debug.LogError("Unsupported type: " + typeName);
			return null;

			Vector2 ConvertToVector2(string value)
            {
				string[] values = value.Split(new string[] { VECTOR_VALUES_SEPERATOR }, StringSplitOptions.None);

				return new Vector2(Convert.ToSingle(values[0]), Convert.ToSingle(values[1]));
            }

			Vector3 ConvertToVector3(string value)
			{
				string[] values = value.Split(new string[] { VECTOR_VALUES_SEPERATOR }, StringSplitOptions.None);

				return new Vector3(Convert.ToSingle(values[0]), Convert.ToSingle(values[1]), Convert.ToSingle(values[2]));
			}
		}
	}
}