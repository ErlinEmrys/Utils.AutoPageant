using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Erlin.Utils.AutoPageant;

/// <summary>
/// Main program
/// </summary>
static class Program
{
    private const int WM_SETTEXT = 12;
    private const int BN_CLICKED = 245;

    [DllImport("user32.dll", CharSet = CharSet.Unicode)]
    private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, string lParam);

    [DllImport("user32.dll", CharSet = CharSet.Unicode)]
    public static extern IntPtr SendMessage(IntPtr hWnd, int msg, int wParam, IntPtr lParam);

    [DllImport("user32.dll", CharSet = CharSet.Unicode)]
    private static extern IntPtr FindWindowEx(
        IntPtr hwndParent,
        IntPtr hwndChildAfter,
        string lpszClass,
        string? lpszWindow
    );

    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main(string[] args)
    {
        if (args.Length < 3 || !File.Exists(args[0]))
        {
            return;
        }

        Process process = Process.Start(args[0], Environment.ExpandEnvironmentVariables(args[1]));
        while (!process.HasExited && !process.WaitForInputIdle(1))
        {
            Thread.Sleep(100);
        }

        if (!process.HasExited)
        {
            IntPtr txtBox = FindWindowEx(process.MainWindowHandle, IntPtr.Zero, "Edit", null);
            _ = SendMessage(txtBox, WM_SETTEXT, IntPtr.Zero, args[2]);
            IntPtr button = FindWindowEx(process.MainWindowHandle, IntPtr.Zero, "Button", "O&K");
            _ = SendMessage(button, BN_CLICKED, 0, IntPtr.Zero);
        }
    }
}
