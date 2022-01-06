using System;
using System.Collections.Generic;

public class FacebookPlayer
{
    public string Id { get; set; }
    public string Name { get; set; }
    public Dictionary<string, string> FriendsDict { get; set; }
        
    public FacebookPlayer(string id, string name, Dictionary<string, string> friendsDict)
    {
        this.Id = id;
        this.Name = name;
        this.FriendsDict = friendsDict;
    }

    public override string ToString()
    {
        string FriendIdListString = "";
        foreach (var friend in this.FriendsDict)
        {
            FriendIdListString += friend.Key + ", ";
        }
        int length = FriendIdListString.Length - 2;
        FriendIdListString = FriendIdListString.Substring(0, length);
        string FbPlayer = String.Format("FacebookPlayer[{0}, {1}]\nFriendIdList[{2}]", this.Id, this.Name, FriendIdListString);
        return FbPlayer;
    }
}