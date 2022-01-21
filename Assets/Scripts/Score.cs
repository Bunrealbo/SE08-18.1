using System;

public class Score
{

    public string id { get; set; }
    public int score { get; set; }
    public int level { get; set; }

    public Score(string id, int score, int level)
    {
        this.id = id;
        this.score = score;
        this.level = level;
    }

    public override string ToString()
    {
        return String.Format("Score[id={0}, score={1}, level={2}]", this.id, this.score, this.level);
    }
}