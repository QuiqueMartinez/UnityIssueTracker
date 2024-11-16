# Unity Issue Tracker

**Unity Issue Tracker** is a Unity package designed to analyze and manage issues directly from the Unity Editor. It provides a streamlined way for developers to track problems, pending tasks, and areas for improvement in their code.

---

## Features

- Automatically scans code for user-defined attributes.
- Generates a list of issues categorized by priority, status, and tags.
- Integrates a custom Editor window to view and manage issues.
- Fully compatible with Unity Package Manager (UPM) for easy integration.

---

## Installation

### Option 1: Add to `manifest.json`
Add the following entry to your project's `Packages/manifest.json` file:

```json
{
    "dependencies": {
        "com.yourname.issue-tracker": "https://github.com/yourname/unity-issue-tracker.git"
    }
}```
### Option 2: Install from a local folder
- Clone the repository or download it as a ZIP file.
- Move the package folder to a location outside your project.
- Add the package as a local dependency in manifest.json:

### Option3: Copy the package files directly in your project
- Copy all the Editor and Runtime directories with their content into any folder under Assets in yout project.

### Assembly
- The issue tracker uses its owm assembly. If using the option 3, this file can be skipped making the functionality accessible to the main assembly. 

###Usage

##0. Create the issues

#### Código en C#:
Issues are attribites associated to classes and methods.
```csharp
        [AutoIssue.Issue(
        Priority = AutoIssue.Priority.High,
        Tag = AutoIssue.Tag.TODO,
        Description = "Tutorial connector service.")]
		```

The available lablels are listed in the table (can be easily edited).

| Priority  |    Tag    |  Status   |
|-----------|-----------|-----------|
| Low       | BUG       | Open      |
| Medium    | FEATURE   | InProgress|
| High      | TODO      | Review    |
|           | HACK      | Closed    |
|           | HARDCODED |           |
|           | MAGIC     |           |
|           | REVAMP    |           |
|           | SMELL     |           |


##1. Open the Editor Window
Go to Window > Issue Tracker in the Unity Editor menu.
The Issue Tracker window will open, displaying the current list of issues.
![Issue tracker window](Documentation~/images/IssueTracker.png)
##2. Scan for Issues
Click the Scan button in the Editor window to analyze your codebase.
The tool will detect all attributes marked with IssueAttribute and populate the list.
Yellow color indicates the agter a compile, the issue list might have changed.

##3. View and Manage Issues
Issues are displayed with details such as priority, status, and description.
Use the filters to refine the displayed list by tags, priority, or status.
