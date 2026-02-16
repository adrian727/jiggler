using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Threading;

public class Insomnia : Form
{
    // --- 1. Import Kernel32 Power API ---
    [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    static extern uint SetThreadExecutionState(uint esFlags);

    // Flags to tell Windows "Don't Sleep"
    private const uint ES_CONTINUOUS = 0x80000000;
    private const uint ES_SYSTEM_REQUIRED = 0x00000001;
    private const uint ES_DISPLAY_REQUIRED = 0x00000002;

    private NotifyIcon trayIcon;
    private ContextMenu trayMenu;

    public Insomnia()
    {
        // Setup System Tray
        trayMenu = new ContextMenu();
        trayMenu.MenuItems.Add("Exit", OnExit);

        trayIcon = new NotifyIcon();
        trayIcon.Text = "Insomnia (Display Locked On)";
        
        // Icon: A Yellow "Sun" / Circle
        trayIcon.Icon = CreateSimpleIcon();
        
        trayIcon.ContextMenu = trayMenu;
        trayIcon.Visible = true;

        // --- THE MAGIC LINE ---
        // This tells Windows: "Keep System Awake + Keep Display On"
        PreventSleep();
    }

    private void PreventSleep()
    {
        // ES_CONTINUOUS: The state remains until we change it or close the app
        // ES_DISPLAY_REQUIRED: Forces the display to stay on
        // ES_SYSTEM_REQUIRED: Forces the CPU to stay on
        SetThreadExecutionState(ES_CONTINUOUS | ES_SYSTEM_REQUIRED | ES_DISPLAY_REQUIRED);
    }

    private Icon CreateSimpleIcon()
    {
        // Draw a Yellow Sun icon
        Bitmap bitmap = new Bitmap(16, 16);
        using (Graphics g = Graphics.FromImage(bitmap))
        {
            g.Clear(Color.Transparent);
            // Draw circle
            g.FillEllipse(Brushes.Gold, 2, 2, 12, 12);
        }
        return Icon.FromHandle(bitmap.GetHicon());
    }

    protected override void OnLoad(EventArgs e)
    {
        Visible = false;       // Hide window
        ShowInTaskbar = false; // Hide from taskbar
        base.OnLoad(e);
    }

    private void OnExit(object sender, EventArgs e)
    {
        // Reset the power state to normal before exiting
        SetThreadExecutionState(ES_CONTINUOUS);
        
        trayIcon.Visible = false;
        Application.Exit();
    }

    [STAThread]
    public static void Main()
    {
        bool createdNew;
        using (Mutex mutex = new Mutex(true, "Global\\MyInsomniaApp", out createdNew))
        {
            if (createdNew) Application.Run(new Insomnia());
        }
    }
}