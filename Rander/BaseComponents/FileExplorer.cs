using System;
using System.Collections.Generic;
using System.Text;
using Ookii.Dialogs.Wpf;

namespace Rander
{
    public class FileExplorer
    {
        public static string OpenFile(string title, string allowedExtensions = "Any File|*.*", string defaultExtension = "*.*")
        {
            VistaOpenFileDialog Dialog = new VistaOpenFileDialog();
            Dialog.Title = title;
            Dialog.RestoreDirectory = true;
            Dialog.Filter = allowedExtensions;
            Dialog.DefaultExt = defaultExtension;
            Dialog.ValidateNames = true;
            Dialog.ShowDialog();
            return Dialog.FileName;
        }

        public static string OpenFolder(string title, bool showNewFolder = false)
        {
            VistaFolderBrowserDialog Dialog = new VistaFolderBrowserDialog();
            Dialog.Description = title;
            Dialog.ShowNewFolderButton = showNewFolder;
            Dialog.ShowDialog();
            return Dialog.SelectedPath;
        }

        public static string SaveFile(string title, string allowedExtensions = "Any File|*.*", string defaultExtension = "*.*")
        {
            VistaSaveFileDialog Dialog = new VistaSaveFileDialog();
            Dialog.Title = title;
            Dialog.RestoreDirectory = true;
            Dialog.Filter = allowedExtensions;
            Dialog.DefaultExt = defaultExtension;
            Dialog.ValidateNames = true;
            Dialog.ShowDialog();
            return Dialog.FileName;
        }
    }
}
