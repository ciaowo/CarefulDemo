using CommonLib;
using CommonLib.Logs;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using WPFCommonLib.Views.MessageBoxControl;

namespace DxnWPFCommonLib.Services
{
    public class DialogService : IDialogService
    {
        public DialogService(ILog log)
        {
            Log = log;
        }

        public ILog Log { get; private set; }

        public void Alert(string message)
        {
            MessageWindow.ShowMessageBoxWindow(message, "消息提示", WPFCommonLib.Views.MessageBoxControl.MessageBoxButton.Ok);
            //MessageBox.Show(message, Localization.Resources.MessageDialog_Title_Alert, System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning, MessageBoxResult.OK);
        }

        public bool Confirm(string message, string title, ConfirmType type = ConfirmType.YesNo)
        {
            var buttons = WPFCommonLib.Views.MessageBoxControl.MessageBoxButton.YesNo;
            if (type == ConfirmType.OKCancel)
            {
                buttons = WPFCommonLib.Views.MessageBoxControl.MessageBoxButton.OkCancel;
            }
            var result = MessageWindow.ShowMessageBoxWindow(message, title, buttons, MessageBoxImageType.Question);
            //var result = MessageBox.Show(message, title, buttons, System.Windows.MessageBoxImage.Question);

            if (type == ConfirmType.OKCancel)
            {
                return result == MessageBoxResult.OK ? true : false;
            }
            else
            {
                return result == MessageBoxResult.Yes ? true : false;
            }
        }

        private void SetErrorInfo(StringBuilder errorMessage, Exception ex, int level)
        {
            errorMessage.AppendLine("Error Type:" + ex.GetType().ToString());
            errorMessage.AppendLine("Error Message[" + level + "]:" + ex.Message);
            errorMessage.AppendLine("Error Stack:" + ex.StackTrace);
        }

        public void ShowException(Exception ex)
        {
            // 记录日志
            StringBuilder errorMessage = new StringBuilder();
            SetErrorInfo(errorMessage, ex, 1);
            if (ex.InnerException != null)
            {
                SetErrorInfo(errorMessage, ex.InnerException, 2);
                if (ex.InnerException.InnerException != null)
                    SetErrorInfo(errorMessage, ex.InnerException.InnerException, 3);
            }

            //Log.LogError(errorMessage.ToString());

            // 界面弹出显示消息
            var message = new StringBuilder();
            message.AppendLine("异常提示");
            message.AppendLine();
            if (ex is System.Reflection.TargetInvocationException && ex.InnerException != null)
            {
                // 当异常为调用目标发生异常时，直接显示其 InnerException
                message.AppendLine(ex.InnerException.Message);
            }
            else
            {
                message.AppendLine(ex.Message);
            }

#if DEBUG
            message.AppendLine($"Source: {ex.Source}");
            message.AppendLine(ex.ToString());
            message.AppendLine(ex.InnerException?.Message);
#endif

            MessageWindow.ShowMessageBoxWindow(message.ToString(), "异常提示", WPFCommonLib.Views.MessageBoxControl.MessageBoxButton.Ok);
        }

        public void ShowExceptionLog(string title, string error)
        {
            // 记录日志
            //Log.LogError(title + error);


        }
        public void ShowMessage(string message, string title)
        {
            MessageWindow.ShowMessageBoxWindow(message, title, WPFCommonLib.Views.MessageBoxControl.MessageBoxButton.Ok);
            //MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);
        }

        public bool ShowSaveFileDialog(string filter, out string filePath, string defalutFileName = "")
        {
            filePath = string.Empty;
            try
            {
                SaveFileDialog dialog = new SaveFileDialog();
                dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                dialog.Filter = filter;
                dialog.FileName = defalutFileName;
                var result = dialog.ShowDialog();
                if (result.HasValue && result.Value)
                {
                    filePath = dialog.FileName;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        public bool ShowSelectFileDialog(string filter, out string filePath)
        {
            filePath = string.Empty;

            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = filter;
            dialog.CheckFileExists = true;
            var result = dialog.ShowDialog();
            if (result.HasValue && result.Value)
            {
                filePath = dialog.FileName;
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool ShowSelectFileDialog(string filter, out string filePath, string defalutFileName)
        {
            filePath = string.Empty;

            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = filter;
            dialog.CheckFileExists = true;
            dialog.InitialDirectory = defalutFileName;
            
            var result = dialog.ShowDialog();
            if (result.HasValue && result.Value)
            {
                filePath = dialog.FileName;
                return true;
            }
            else
            {
                return false;
            }
        }
        public List<string> ShowSelectMultiFileDialog(out string filePath, string filter = "")
        {
            filePath = string.Empty;
            List<string> fileNames = new List<string>();
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Multiselect = true;
            dialog.CheckFileExists = true;
            if (!string.IsNullOrWhiteSpace(filter))
            {
                dialog.Filter = filter;
            }
            var result = dialog.ShowDialog();
            if (result.HasValue && result.Value)
            {
                foreach (var item in dialog.FileNames)
                {
                    fileNames.Add(item);
                }
                filePath = dialog.FileName;
                return fileNames;
            }
            else
            {
                return null;
            }
        }
        public bool ShowSelectFolderDialog(out string folderPath, string defaultPath)
        {
            System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();
            dialog.SelectedPath = defaultPath;
            var result = dialog.ShowDialog();
            folderPath = dialog.SelectedPath;
            return result == System.Windows.Forms.DialogResult.OK ? true : false;
        }

    }
}