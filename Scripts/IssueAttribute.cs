using UnityEditor;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace AutoIssue
{
    public enum Priority
    {
        Low,
        Medium,
        High
    }

    public enum Tag
    {
        BUG,
        FEATURE,
        TODO,
        HACK,
        HARDCODED,
        MAGIC,
        REVAMP,
        SMELL,
        
    }

    public enum Status
    {
        Open,
        InProgress,
        Review,
        Closed
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public sealed class IssueAttribute : Attribute
    {
        public Priority Priority { get; set; } = Priority.Medium;
        public Tag Tag { get; set; } = Tag.TODO;
        public Status Status { get; set; } = Status.Open;
        public string Description { get; set; } = "No description provided";

        public IssueAttribute() { }

        public IssueAttribute(string description)
        {
            Description = description;
        }
    }
}
