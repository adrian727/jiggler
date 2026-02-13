using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Threading;

public class MouseJiggler : Form
{
    // P/Invoke - Importing the native Windows function that simulates actual input
    [DllImport("user32.dll")]
    static extern void mouse_event(uint dwFlags, int dx, int dy, uint dwData, int dwExtraInfo);

    // Flag to tell Windows we are moving the mouse
    private const uint MOUSEEVENTF_MOVE = 0x0001;

    private NotifyIcon trayIcon;
    private ContextMenu trayMenu;
    private System.Windows.Forms.Timer moveTimer;

    public MouseJiggler()
    {
        // --- 1. Setup System Tray ---
        trayMenu = new ContextMenu();
        trayMenu.MenuItems.Add("Exit", OnExit);

        trayIcon = new NotifyIcon();
        trayIcon.Text = "Jiggler v2 (Active)";
        trayIcon.Icon = CreateSimpleIcon();
        trayIcon.ContextMenu = trayMenu;
        trayIcon.Visible = true;

        // --- 2. Setup Timer (Runs every 60 seconds) ---
        moveTimer = new System.Windows.Forms.Timer();
        moveTimer.Interval = 60000; // 60 seconds
        moveTimer.Tick += new EventHandler(DoJiggle);
        moveTimer.Start();
    }

    private void DoJiggle(object sender, EventArgs e)
    {
        // 1. Move Mouse Right by 1 pixel (Relative move)
        // This generates a hardware-level input event
        mouse_event(MOUSEEVENTF_MOVE, 1, 0, 0, 0);
        
        // 2. Wait 100ms so the OS definitely sees it
        Thread.Sleep(100);

        // 3. Move Mouse Left by 1 pixel (Back to start)
        mouse_event(MOUSEEVENTF_MOVE, -1, 0, 0, 0);
    }

    private Icon CreateSimpleIcon()
    {
        // Create a Green Square icon this time to indicate v2
        Bitmap bitmap = new Bitmap(16, 16);
        using (Graphics g = Graphics.FromImage(bitmap))
        {
            g.Clear(Color.Transparent);
            g.FillRectangle(Brushes.Green, 2, 2, 12, 12);
        }
        return Icon.FromHandle(bitmap.GetHicon());
    }

    protected override void OnLoad(EventArgs e)
    {
        Visible = false;       // Hide the window
        ShowInTaskbar = false; // Hide from taskbar
        base.OnLoad(e);
    }

    private void OnExit(object sender, EventArgs e)
    {
        trayIcon.Visible = false;
        Application.Exit();
    }

    [STAThread]
    public static void Main()
    {
        // Single Instance Check (Mutex)
        bool createdNew;
        using (Mutex mutex = new Mutex(true, "Global\\MyJigglerAppV2", out createdNew))
        {
            if (createdNew)
            {
                Application.Run(new MouseJiggler());
            }
            // If not createdNew, app closes silently (already running)
        }
    }
}