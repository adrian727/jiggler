using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Threading;

public class MouseJiggler : Form
{
    private NotifyIcon trayIcon;
    private ContextMenu trayMenu;
    private System.Windows.Forms.Timer moveTimer;
    private Point lastPosition;
    private DateTime lastMoveTime;

    // Import Windows API to move mouse physically
    [DllImport("user32.dll")]
    static extern bool SetCursorPos(int X, int Y);

    [DllImport("user32.dll")]
    static extern bool GetCursorPos(out Point lpPoint);

    // Single Instance Mutex
    private static Mutex mutex = null;

    public MouseJiggler()
    {
        // 1. Setup System Tray
        trayMenu = new ContextMenu();
        trayMenu.MenuItems.Add("Exit", OnExit);

        trayIcon = new NotifyIcon();
        trayIcon.Text = "Mouse Jiggler";

        // Create a simple icon on the fly (Red Circle) so you don't need an external .ico file
        trayIcon.Icon = CreateSimpleIcon();

        trayIcon.ContextMenu = trayMenu;
        trayIcon.Visible = true;

        // 2. Setup Timer (Checks every 30 seconds)
        moveTimer = new System.Windows.Forms.Timer();
        moveTimer.Interval = 30000; // 30 seconds
        moveTimer.Tick += new EventHandler(CheckActivity);
        moveTimer.Start();

        // Initialize tracking
        GetCursorPos(out lastPosition);
        lastMoveTime = DateTime.Now;
    }

    private void CheckActivity(object sender, EventArgs e)
    {
        Point currentPos;
        GetCursorPos(out currentPos);

        // If mouse has moved since last check
        if (currentPos != lastPosition)
        {
            lastPosition = currentPos;
            lastMoveTime = DateTime.Now;
        }
        else
        {
            // No movement detected
            // Jiggle the mouse: Move 1 pixel right, then back
            SetCursorPos(currentPos.X + 1, currentPos.Y);
            System.Threading.Thread.Sleep(50);
            SetCursorPos(currentPos.X, currentPos.Y);

            // Log acts as "keep alive"
            lastMoveTime = DateTime.Now;
        }
    }

    private Icon CreateSimpleIcon()
    {
        // Draw a 16x16 red circle to use as an icon
        Bitmap bitmap = new Bitmap(16, 16);
        using (Graphics g = Graphics.FromImage(bitmap))
        {
            g.Clear(Color.Transparent);
            g.FillEllipse(Brushes.Red, 1, 1, 14, 14);
        }
        return Icon.FromHandle(bitmap.GetHicon());
    }

    protected override void OnLoad(EventArgs e)
    {
        Visible = false; // Hide the form window
        ShowInTaskbar = false; // Remove from taskbar
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
        const string appName = "com.example.MouseJiggler";
        bool createdNew;

        mutex = new Mutex(true, appName, out createdNew);

        if (!createdNew)
        {
            // App is already running
            return;
        }

        Application.Run(new MouseJiggler());
    }
}