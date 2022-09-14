using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest
{
    public string Title { get; set; }

    public QuestStatus Status { get; set; }

    public List<QuestContent> Content { get; set; }
}


public class QuestContent
{
    public QuestStatus Status { get; set; }
    public List<string> Texts { get; set; }
}

public enum QuestStatus
{
    PENDING = 1,
    ACTIVE = 2,
    RESOLVED = 3,
    COMPLETE = 4
}
