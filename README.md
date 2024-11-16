# Unity Issue Tracker

**Unity Issue Tracker** is a Unity package designed to analyze and manage issues directly from the Unity Editor. It provides a streamlined way for developers to track problems, pending tasks, and areas for improvement in their code. 
The main motivation is that the typical TODO list does not reference the exact class or method in the code. This tool can be useful for code maintenance.

---

## Features

- Automatically scans code for user-defined attributes.
- Generates a list of issues categorized by priority, status, and tags.
- Integrates a custom Editor window to view and manage issues.


---

## Installation

### Option 1: Add to `manifest.json`
Add the following entry to your project's `Packages/manifest.json` file:

```json
{
    "dependencies": {
    ...
    "com.quiquemartinez.unity-issue-tracker": "https://github.com/QuiqueMartinez/UnityIssueTracker.git",
    ...
    }
}
```

### Option 2: Install from a local folder
- Clone the repository or download it as a ZIP file.
- Move the package folder to a location outside your project.
- Add the package as a local dependency in manifest.json:

### Option3: Copy the package files directly into your project
- Copy all the Editor and Runtime directories with their content into any folder under Assets in your project.

### Assembly
- The issue tracker uses its own assembly. If using option 3, this file can be skipped making the functionality accessible to the main assembly. 

### Usage

## 0. Create the issues

Issues are attributes associated with classes and methods.
```csharp
        [AutoIssue.Issue(
        Priority = AutoIssue.Priority.High,
        Tag = AutoIssue.Tag.TODO,
        Description = "Tutorial connector service.")]
	// Your class or method here
```
		```

The available labels are listed in the table (can be easily edited).

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

## 1. Open the Editor Window
Go to Window > Issue Tracker in the Unity Editor menu.
The Issue Tracker window will open, displaying the current list of issues.
![Issue tracker window](Documentation~/images/IssueTracker.png)

## 2. Scan for Issues
Click the Scan button in the Editor window to analyze your codebase.
The tool will detect all attributes marked with IssueAttribute and populate the list.
The yellow indicates that the issue list might have changed after a compilation.

## 3. View and Manage Issues
Issues are displayed with details such as priority, status, and description.
The filters refine the displayed list by tags, priority, or status.

## 4. Requirements
Made for Unity 6 or later
Compatible with Windows, macOS, and Linux

## 5. Limitations
- Only opens the file in VS, can not go to the exact line of code.

## 6. Planned improvements
- Filter by tag, priority, or status.
- Order.
- Group by assembly.	
- Scan only the specified assembly list.

## 7. License and Disclaimer
This package is distributed under the Unlicense, which allows you to freely use, modify, and distribute the code. However, no warranty of any kind is provided, and the author takes no responsibility for any issues, damages, or consequences arising from its use. There is no guarantee of maintenance, updates, or support for this package. Use it at your own risk.
