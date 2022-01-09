using System;
using System.Collections.Generic;

public class Score
{

    public string Id { get; set; }
    public int Scorex { get; set; }
    public int Level { get; set; }

    public Score(string id, int score, int level)
    {
        this.Id = id;
        this.Scorex = score;
        this.Level = level;
    }

    public override string ToString()
    {
        return String.Format("Score[id={0}, score={1}, level={2}]", this.Id, this.Scorex, this.Level);
    }
}
