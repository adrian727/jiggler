# Native Mouse Jiggler (C#)

A lightweight, single-file Windows utility designed to prevent the computer from going to sleep or locking the screen.

It runs silently in the system tray and simulates a microscopic mouse movement every 60 seconds if no activity is detected.

## 🚀 Why this version?

* **No Installation Required:** Uses the .NET Framework compiler (`csc.exe`) already present on every Windows machine (Windows 7/10/11).
* **No Admin Rights Needed:** You can compile and run this in user-space.
* **Zero Dependencies:** No Java (`JAVA_HOME`), no Python, no external DLLs.
* **Stealthy:** Runs as a background process with a System Tray icon.

## 📋 Features

* **Smart Detection:** Only moves the mouse if you haven't moved it yourself in the last 60 seconds.
* **System Tray Integration:** Minimizes to tray (Red Dot icon).
* **Single Instance:** Prevents multiple copies from running simultaneously.
* **Tiny Footprint:** The resulting `.exe` is usually under 10KB.

## 🛠️ How to Build (On the Windows Machine)

You do not need Visual Studio. You only need the source code file.

1. Copy the code below and save it as a file named `Jiggler.cs`.
2. Open **Command Prompt** (cmd.exe).
3. Navigate to the folder where you saved the file.
4. Run the following command to compile it:

```cmd
C:\Windows\Microsoft.NET\Framework64\v4.0.30319\csc.exe /target:winexe /out:Jiggler.exe Jiggler.cs

```

> **Note:** If you are on a 32-bit system (rare nowadays), remove the `64` from `Framework64`.

5. You will now see `Jiggler.exe` in the folder.

## 💻 Usage

1. Double-click `Jiggler.exe`.
2. The app will launch silently. Look for a **Red Dot icon** in your System Tray (bottom right, near the clock).
3. **To Quit:** Right-click the Red Dot icon and select **Exit**.

## ⚠️ Disclaimer

This tool is intended for educational purposes and personal productivity (e.g., keeping a machine awake during long downloads or presentations). Please ensure you comply with your organization's IT security policies regarding screen locking and software execution.