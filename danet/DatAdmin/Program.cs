using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Reflection;
using System.Threading;

namespace DatAdmin
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

#if (DEBUG)
            RunMain();
#else
            try
            {
                RunMain();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
#endif

        }

        static void RunMain()
        {
            Core.IsGUIDatAdmin = true;
            Async.MainThread = Thread.CurrentThread;
            Async.WaitDialog = new WaitDialog();
            NodeFactory.RegisterRootCreator(RootTreeNode.CreateRoot);
            FileTextProvider.LoadStdTexts();

            DatAdmin.Plugins.AddAssembly(Assembly.GetAssembly(typeof(Program)));

            PluginTools.LoadPlugins();
            Application.Run(new MainForm());
        }
    }
}