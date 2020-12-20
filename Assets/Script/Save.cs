using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class Save
{
    public static void SaveUser (User user)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/player.carbon";
        FileStream stream = new FileStream(path, FileMode.Create);

        Userdata data = new Userdata(user);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static Userdata LoadUser()
    {
        string path = Application.persistentDataPath + "/player.carbon";
        if(File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            Userdata data = formatter.Deserialize(stream) as Userdata;
            stream.Close();

            return data;

        } else{
            Debug.LogError("Save file not found in "+path);
            return null;
        }
    }
}
