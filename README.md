# Native Insomnia (C#)

A lightweight, single-file Windows utility designed to prevent the computer **and the display** from going to sleep.

Unlike traditional "Mouse Jigglers" that simulate input, this tool uses the Windows Kernel API (`SetThreadExecutionState`) to explicitly tell the Operating System (and VDI sessions) that a critical process is running and the display must remain active.

## 🚀 Why this version?

* **VDI Optimized:** Solves the "Black Screen / Monitor Timeout" issue common in VMware Horizon and Citrix where mouse movements are ignored by power policies.
* **No Installation Required:** Uses the .NET Framework compiler (`csc.exe`) already present on every Windows machine (Windows 7/10/11/Server).
* **No Admin Rights Needed:** You can compile and run this in user-space.
* **Zero Dependencies:** No Java, no Python, no external DLLs.
* **Stealthy:** Runs as a background process with a System Tray icon.

## 📋 Features

* **Kernel-Level Keep Awake:** Sets the `ES_DISPLAY_REQUIRED` and `ES_SYSTEM_REQUIRED` flags.
* **System Tray Integration:** Minimizes to tray (Yellow Sun icon).
* **Single Instance:** Prevents multiple copies from running simultaneously.
* **Tiny Footprint:** The resulting `.exe` is usually under 10KB.

## 🛠️ How to Build (On the Target Machine)

You do not need Visual Studio. You only need the source code file.

1. Copy the code block below and save it as a file named `Insomnia.cs`.
2. Open **Command Prompt** (cmd.exe).
3. Navigate to the folder where you saved the file.
4. Run the following command to compile it:

```cmd
C:\Windows\Microsoft.NET\Framework64\<version>\csc.exe /target:winexe /out:Insomnia.exe Insomnia.cs

```

> **Note:** If you are on an older 32-bit system, remove the `64` from `Framework64` in the path.

5. You will now see `Insomnia.exe` in the folder.

## 💻 Usage

1. Double-click `Insomnia.exe`.
2. The app will launch silently. Look for a **Yellow Sun icon** in your System Tray (bottom right, near the clock).
3. **To Quit:** Right-click the icon and select **Exit**. (Normal sleep settings will resume immediately).

## ⚠️ Disclaimer

This tool is intended for educational purposes and personal productivity (e.g., keeping a machine awake during long downloads or presentations). Please ensure you comply with your organization's IT security policies regarding screen locking and software execution.