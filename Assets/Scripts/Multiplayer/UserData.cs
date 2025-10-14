using System;

public class UserData
{
    public int ID;
    public string Name;
    public string Email;
    public DateTime UpdatedAt;
    public bool isAdmin;

    public UserData() 
    { 
        UpdatedAt = DateTime.Now;
    }
}
