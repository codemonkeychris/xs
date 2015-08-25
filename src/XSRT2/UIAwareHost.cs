using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace XSRT2
{
    public sealed class UIAwareHost
    {
        string programFileName;
        bool autoCheckUpdates = false;
        bool overwriteIfExists = true;
        Type appType;
        DispatcherTimer fileWatcherUpdateTimer;

        public UIAwareHost(ContentControl displayControl, Type appType, string programFileName)
        {
            displayControl.SizeChanged += DisplayControl_SizeChanged;
            this.programFileName = programFileName;
            this.appType = appType;
            Runtime = new XSRuntime(displayControl);
        }

        public XSRuntime Runtime { get; private set; }
        public bool AutoCheckUpdates
        {
            get { return autoCheckUpdates; }
            set { autoCheckUpdates = value; UpdateAutoTimer(); }
        }
        public bool OverwriteIfExists
        {
            get { return overwriteIfExists; }
            set { overwriteIfExists = value; }
        }
        private void DisplayControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Runtime.SizeChanged(e.NewSize.Width, e.NewSize.Height);
        }
        void UpdateAutoTimer()
        {
            if (fileWatcherUpdateTimer != null)
            {
                fileWatcherUpdateTimer.Stop();
                fileWatcherUpdateTimer = null;
            }

            if (autoCheckUpdates)
            {
                fileWatcherUpdateTimer = new DispatcherTimer();
                fileWatcherUpdateTimer.Interval = TimeSpan.FromMilliseconds(250);
                fileWatcherUpdateTimer.Tick += autoTimer_Tick;
                fileWatcherUpdateTimer.Start();
            }
        }

        void autoTimer_Tick(object sender, object e)
        {
            CheckForUpdates(forceReload: false);
        }
        public async void CheckForUpdates(bool forceReload)
        {
            await CheckFile(forceReload);
            Runtime.RenderIfNeeded();
        }
        async Task<string> InitFile()
        {

            StorageFile file;
            try
            {
                var found = await Windows.Storage.ApplicationData.Current.RoamingFolder.TryGetItemAsync(programFileName);
                if (found == null)
                {
                    var program = await RuntimeHelpers.GetResource(appType, programFileName);
                    file = await Windows.Storage.ApplicationData.Current.RoamingFolder.CreateFileAsync(programFileName, Windows.Storage.CreationCollisionOption.FailIfExists);
                    await FileIO.WriteTextAsync(file, program);
                }
                else
                {
                    file = (StorageFile)found;
                    if (overwriteIfExists)
                    {
                        var program = await RuntimeHelpers.GetResource(appType, programFileName);
                        await FileIO.WriteTextAsync(file, program);
                    }
                }
            }
            catch
            {
                file = await Windows.Storage.ApplicationData.Current.RoamingFolder.CreateFileAsync(programFileName, CreationCollisionOption.OpenIfExists);
            }

            if (overwriteIfExists)
            {
                return await RuntimeHelpers.GetResource(appType, programFileName);
            }
            else
            {
                return await ReadText(file);
            }
        }

        // This is here purely as a debugging aid for hard coding content of the file in cases
        // where there is a problem
        // 
        static async Task<string> ReadText(StorageFile file)
        {
            try
            {
                var str = await FileIO.ReadTextAsync(file);
                return str;
            }
            catch (UnauthorizedAccessException)
            {
                // need wait
                return XSRuntime.ProgramWithMessage("Failed to load file (access denied)");
            }
        }
        async Task<string> CheckFile(bool forceReload)
        {
            var file = await Windows.Storage.ApplicationData.Current.RoamingFolder.CreateFileAsync(programFileName, Windows.Storage.CreationCollisionOption.OpenIfExists);
            var contents = await ReadText(file);
            return await Runtime.SetProgram(contents, forceReload);
        }

        public async void Startup()
        {
            await InitFile();
            await CheckFile(forceReload: false);
            Runtime.RenderIfNeeded();
        }
    }
}
